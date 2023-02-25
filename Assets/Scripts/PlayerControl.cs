using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerControl : MonoBehaviour
{
    void Start()
    {
        health = 10;
        healthBarRect.sizeDelta = new Vector2(health * 10 * healthMultiplier, 20);
        healthBarRect.anchoredPosition = new Vector2(25f + (health * 5 * healthMultiplier), -25);
        NextDialouge();
    }

    [SerializeField] Rigidbody2D rigBod;
    [SerializeField] float jumpForce;
    int jumps;
    [SerializeField] float runSpeed;

    public int health;
    [SerializeField] RectTransform healthBarRect;

    [SerializeField] GameObject playerProjectileObj;
    [SerializeField] float shootDelay;
    float shootTime;
    [SerializeField] float projectileSpeed;

    [SerializeField] AttackManager attackManager;
    [SerializeField] Animator animator;

    [SerializeField] Transform background;
    [SerializeField] float backgroundSpeed;
    [SerializeField] Transform ground;
    [SerializeField] float groundSpeed;

    bool gameStarted;

    [SerializeField] AudioSource playerShootSound;
    [SerializeField] Vector2 shootPitchRange;

    void Update()
    {
        animator.SetFloat("Speed", Mathf.Abs(rigBod.velocity.x));
        animator.SetFloat("YSpeed", Mathf.Abs(rigBod.velocity.y));
        animator.SetBool("CanPlay", canPlay);
        animator.SetInteger("Stage", attackManager.stage);

        if (attackManager.stage == 1 && canPlay)
        {
            GroundMovement();
        } else if (canPlay)
        {
            AirMovement();
        }

        if (Input.GetMouseButton(0) && shootTime <= 0 && canPlay)
        {
            Vector3 projectileSpawnOffset = new Vector3(0.7f * transform.localScale.x, 0.3f, 0);
            GameObject projectile = Instantiate(playerProjectileObj, transform.position + projectileSpawnOffset/* new Vector2(transform.position.x + (0.5f * transform.localScale.x), transform.position.y) */, Quaternion.identity);
            projectile.GetComponent<Rigidbody2D>().velocity = new Vector2(projectileSpeed * transform.localScale.x, 0);
            Destroy(projectile, 10);

            playerShootSound.pitch = Random.Range(shootPitchRange.x, shootPitchRange.y);
            playerShootSound.Play();

            shootTime = shootDelay;
        }
        shootTime -= Time.deltaTime;
        iFrames -= Time.deltaTime;

        if (background.position.x < -9.4f)
        {
            background.position = new Vector2(29f, background.position.y);
        }
        if (ground.position.x < -9.1f)
        {
            ground.position = new Vector2(42f, ground.position.y);
        }
        if (gameStarted)
        {
            background.position = new Vector2(background.position.x - (Time.deltaTime * backgroundSpeed), background.position.y);
            ground.position = new Vector2(ground.position.x - (Time.deltaTime * groundSpeed), ground.position.y);
        }
        
    }

    bool grounded;
    float iFrames;
    [SerializeField] float healthMultiplier;

    [SerializeField] AudioSource playerHurtSound;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Ground" || collision.gameObject.name == "TopBound")
        {
            grounded = true;
            jumps = 3;
        }

        if (collision.gameObject.CompareTag("EnemyProjectile") && canPlay)
        {
            if (iFrames <= 0)
            {
                health -= collision.gameObject.GetComponent<GeneralProjectile>().damage;
                healthBarRect.sizeDelta = new Vector2(health * 10 * healthMultiplier, 20);
                healthBarRect.anchoredPosition = new Vector2(25f + (health * 5 * healthMultiplier), -25);
                iFrames = 1;

                StartCoroutine(CameraShake(collision.gameObject.GetComponent<GeneralProjectile>().damage / 15f));
            }

            if (collision.gameObject.GetComponent<GeneralProjectile>().projectileType != "Harpoon" && collision.gameObject.GetComponent<GeneralProjectile>().projectileType != "Boomerang" && collision.gameObject.GetComponent<GeneralProjectile>().projectileType != "Laser" && collision.gameObject.GetComponent<GeneralProjectile>().projectileType != "Missile" && collision.gameObject.GetComponent<GeneralProjectile>().projectileType != "Explosion" && collision.gameObject.GetComponent<GeneralProjectile>().projectileType != "FireBreath")
            {
                Destroy(collision.gameObject);
            }
            
        }
        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("EnemyProjectile") && canPlay)
        {
            if (iFrames <= 0 && collision.gameObject.GetComponent<GeneralProjectile>().damage != 0f)
            {
                health -= collision.gameObject.GetComponent<GeneralProjectile>().damage;
                healthBarRect.sizeDelta = new Vector2(health * 10 * healthMultiplier, 20);
                healthBarRect.anchoredPosition = new Vector2(25f + (health * 5 * healthMultiplier), -25);
                iFrames = 1;

                StartCoroutine(CameraShake(collision.gameObject.GetComponent<GeneralProjectile>().damage / 15f));

                if (health <= 0)
                {
                    StartCoroutine(StartDeathScreen());
                }
            }

            if (collision.gameObject.GetComponent<GeneralProjectile>().projectileType != "Harpoon" && collision.gameObject.GetComponent<GeneralProjectile>().projectileType != "Boomerang" && collision.gameObject.GetComponent<GeneralProjectile>().projectileType != "Laser" && collision.gameObject.GetComponent<GeneralProjectile>().projectileType != "Missile" && collision.gameObject.GetComponent<GeneralProjectile>().projectileType != "Explosion" && collision.gameObject.GetComponent<GeneralProjectile>().projectileType != "FireBreath")
            {
                Destroy(collision.gameObject);
            }
            
        }
        
    }
    
   /*  void OnExitBoxcollider2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Ground")
        {
            grounded = false;
        }
    } */

    void GroundMovement()
    {
        if (Input.GetKeyDown("space") || Input.GetKeyDown("w"))
        {
            if (grounded || jumps > 0)
            {
                jumps --;
                grounded = false;

                rigBod.velocity = new Vector2(rigBod.velocity.x, jumpForce);
            }
        }

        float xDelta = Input.GetAxisRaw("Horizontal"); 
        rigBod.velocity = new Vector2(runSpeed * xDelta, rigBod.velocity.y);

        
        if (xDelta != 0f)
        {
            transform.localScale = new Vector2(xDelta, transform.localScale.y);                
        }
    }

    [SerializeField] float airSpeed;

    void AirMovement()
    {
        float xDelta = Input.GetAxisRaw("Horizontal");
        float yDelta = Input.GetAxisRaw("Vertical");

        rigBod.velocity = new Vector2(airSpeed * xDelta, airSpeed * yDelta);

        if (xDelta != 0f)
        {
            transform.localScale = new Vector2(xDelta, transform.localScale.y);                
        }
    }

    [SerializeField] TMP_Text speakerBox;
    [SerializeField] TMP_Text dialougeBox;
    [SerializeField] RectTransform dialougeBoxRect;
    [SerializeField] GameObject ludwig;
    int dialougeNumber;
    public bool canPlay;

    [SerializeField] AudioSource musicSource;

    public void NextDialouge()
    {
        dialougeNumber ++;

        if (dialougeNumber == 1)
        {
            speakerBox.text = "You:";
            dialougeBox.text = "OMG! Ludwig! I love your videos!";
        }
        if (dialougeNumber == 2)
        {
            dialougeBox.text = "Can I get a photo?!?!";
        }
        if (dialougeNumber == 3)
        {
            speakerBox.text = "Ludwig:";
            dialougeBox.text = "Ew. A fan. I hate those.";
        }
        if (dialougeNumber == 4)
        {
            dialougeBox.text = "Coots, get him!";
        }
        if (dialougeNumber == 5)
        {
            dialougeBoxRect.position = new Vector2(0, -1000);
            /* ludwig.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 16);
            Destroy(ludwig, 5f); */
            musicSource.Play();
            StartCoroutine(LudwigLeaves());
        }
    }

    [SerializeField] float flightPeriod;
    [SerializeField] Vector2 ludwigStart;
    [SerializeField] Vector2 ludwigEnd;
    [SerializeField] AnimationCurve ludwigFlightCurve;
    [SerializeField] Transform cootsTrans;
    [SerializeField] Vector2 cootsStart;
    [SerializeField] Vector2 cootsEnd;
    [SerializeField] AnimationCurve cootsMoveCurve;

    [SerializeField] ParticleSystem ludwigJetpack;

    IEnumerator LudwigLeaves()
    {
        ludwigJetpack.Play();

        for (float i = 0; i < 1; i+= Time.deltaTime / flightPeriod)
        {
            ludwig.transform.position = Vector2.Lerp(ludwigStart, ludwigEnd, ludwigFlightCurve.Evaluate(i));
            cootsTrans.transform.position = Vector2.Lerp(cootsStart, cootsEnd, cootsMoveCurve.Evaluate(i));

            yield return null;
        }

        Destroy(ludwig);

        canPlay = true;
        gameStarted = true;
        attackManager.ChooseAttack();
    }

    [SerializeField] float shakeDuration;
    [SerializeField] Transform cameraTrans;

    IEnumerator CameraShake(float shakeMagnitude)
    {
        float t = 0;
        Vector3 originalPos = cameraTrans.position;

        playerHurtSound.Play();

        while (t < shakeDuration)
        {
            cameraTrans.position = cameraTrans.position + new Vector3(Random.Range(-1.00f, 1.00f) * shakeMagnitude, Random.Range(-1.00f, 1.00f) * shakeMagnitude, 0f);
            
            t += Time.deltaTime;
            yield return null;
        }

        cameraTrans.position = originalPos;
    }

    [SerializeField] float fadeDuration;
    [SerializeField] SpriteRenderer fadeRend;
    [SerializeField] Color32 fadeStartColor;
    [SerializeField] Color32 fadeEndColor;

    IEnumerator StartDeathScreen()
    {
        for (float i = 0; i < 1; i += Time.deltaTime / fadeDuration)
        {
            fadeRend.color = Color32.Lerp(fadeStartColor, fadeEndColor, i);
            yield return null;
        }

        SceneManager.LoadScene("DeathScreen");
    }
}
