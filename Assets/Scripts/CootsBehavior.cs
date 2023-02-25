using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//using UnityEngine.UI;
using TMPro;

public class CootsBehavior : MonoBehaviour
{
    public int health;
    [SerializeField] RectTransform healthBarRect;
    [SerializeField] int playerProjectileDamage;

    void Start()
    {
        health = 200;
        //health = 20;
        cootsHealthMultiplier = firstHealthMultiplier;
        healthBarRect.sizeDelta = new Vector2(health * cootsHealthMultiplier, 20);
        healthBarRect.anchoredPosition = new Vector2(-25f - ((health / 2) * cootsHealthMultiplier), -25);
    }

    [SerializeField] AttackManager attackManager;
    [SerializeField] PlayerControl playerControl;

    [SerializeField] Rigidbody2D playerRigBod;

    float cootsHealthMultiplier;
    [SerializeField] float firstHealthMultiplier;
    [SerializeField] float secondHealthMultiplier;
    [SerializeField] float thirdHealthMultiplier;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerProjectile"))
        {
            health -= playerProjectileDamage;
            healthBarRect.sizeDelta = new Vector2(health * cootsHealthMultiplier, 20);
            healthBarRect.anchoredPosition = new Vector2(-25f - ((health / 2) * cootsHealthMultiplier), -25);
            Destroy(collision.gameObject);

            if (health <= 0 && attackManager.stage == 1)
            {
                attackManager.stage ++;
                health = 175;
                //health = 20;
                playerControl.health = 10;
                cootsHealthMultiplier = secondHealthMultiplier;
                healthBarRect.sizeDelta = new Vector2(health * cootsHealthMultiplier, 20);
                healthBarRect.anchoredPosition = new Vector2(-25f - ((health / 2) * cootsHealthMultiplier), -25);

                playerRigBod.gravityScale = 0;
                StartCoroutine(StartSecondStage());
            }
            if (health <= 0 && attackManager.stage == 2)
            {
                attackManager.stage ++;
                health = 150;
                playerControl.health = 10;
                cootsHealthMultiplier = thirdHealthMultiplier;
                healthBarRect.sizeDelta = new Vector2(health * cootsHealthMultiplier, 20);
                healthBarRect.anchoredPosition = new Vector2(-25f - ((health / 2) * cootsHealthMultiplier), -25);
                StartCoroutine(StartThirdStage());
            }
            if (health <= 0 && attackManager.stage == 3)
            {
                SceneManager.LoadScene("EndScreen");
            }

        }

    }

    [SerializeField] GameObject groundObj;
    [SerializeField] GameObject cellingObj;
    [SerializeField] float groundStartY;
    [SerializeField] float  groundEndY;
    [SerializeField] float cellingOffset;
    [SerializeField] float groundMovePeriod;
    public bool destroyAllProjectiles;
    [SerializeField] SpriteRenderer trueBackgroundRend;
    [SerializeField] SpriteRenderer groundRend;

    [SerializeField] Vector2 cootsStart;
    [SerializeField] Vector2 cootsEnd;
    [SerializeField] float moveDuration;
    [SerializeField] AnimationCurve cootsMoveCurve;
    [SerializeField] Animator animator;

    [SerializeField] ParticleSystem playerJetpack;
    [SerializeField] TMP_Text cootsTitle;

    IEnumerator StartSecondStage()
    {
        playerControl.canPlay = false;
        playerRigBod.constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;

        playerJetpack.Play();

        destroyAllProjectiles = true;

        for (float i = 0; i < 1; i+= Time.deltaTime / moveDuration)
        {
            transform.position = Vector2.Lerp(cootsStart, cootsEnd, cootsMoveCurve.Evaluate(i));
            yield return null;
        }

        animator.SetBool("ShouldBeDrone", true);

        for (float i = 0; i < 1; i+= Time.deltaTime / moveDuration)
        {
            transform.position = Vector2.Lerp(cootsEnd, cootsStart, cootsMoveCurve.Evaluate(i));
            yield return null;
        }

        cootsTitle.text = "Air Coots";



        StartCoroutine(DropBackground());
        for (float i = 0; i <= 1; i += Time.deltaTime / groundMovePeriod)
        {
            groundObj.transform.position = Vector2.Lerp(new Vector2(groundObj.transform.position.x, groundStartY), new Vector2(groundObj.transform.position.x, groundEndY), i);
            cellingObj.transform.position = Vector2.Lerp(new Vector2(0, groundStartY + cellingOffset), new Vector2(0, groundEndY + cellingOffset), i);

            yield return null;
        }

        while (!backgroundDropped)
        {
            yield return null;
        }
        groundObj.transform.position = new Vector2(groundObj.transform.position.x, groundEndY);
        cellingObj.transform.position = new Vector2(0, groundEndY + cellingOffset);

        playerRigBod.constraints = RigidbodyConstraints2D.FreezeRotation;
        playerControl.canPlay = true;
        destroyAllProjectiles = false;
        trueBackgroundRend.enabled = false;
        groundRend.enabled = false;

        shouldMove = true;
        StartCoroutine(SecondStageMovement());
        attackManager.ChooseAttack();
    }

    [SerializeField] float backgroundMovePeriod;
    [SerializeField] AnimationCurve backgroundCurve;
    bool backgroundDropped;
    [SerializeField] GameObject backgroundObj;
    [SerializeField] float backgroundStartY;
    [SerializeField] float backgroundEndY;
    [SerializeField] GameObject cloudsOrigin;
    [SerializeField] Vector2 cloudOriginStart;
    
    IEnumerator DropBackground()
    {
        for (float i = 0; i <= 1; i += Time.deltaTime / backgroundMovePeriod)
        {
            backgroundObj.transform.position = new Vector2(backgroundObj.transform.position.x, Mathf.Lerp(backgroundStartY, backgroundEndY, backgroundCurve.Evaluate(i)));
            cloudsOrigin.transform.position = Vector2.Lerp(cloudOriginStart, Vector2.zero, backgroundCurve.Evaluate(i));

            yield return null;
        }

        backgroundDropped = true;
    }

    [SerializeField] GameObject starsOrigin;
    [SerializeField] SpriteRenderer backgroundRend;
    [SerializeField] Color32 startColor;
    [SerializeField] Color32 endColor;

    [SerializeField] SpriteRenderer cootsRend;
    [SerializeField] Color32 cootsColorStart;
    [SerializeField] Color32 cootsColorEnd;

    IEnumerator StartThirdStage()
    {
        playerControl.canPlay = false;
        pauseMovement = true;
        playerRigBod.constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;
        destroyAllProjectiles = true;

        //Simulate Animation Time
        for (float i = 0; i < 2; i+= Time.deltaTime)
        {
            yield return null;
            pauseMovement = true;
        }

        cootsTitle.text = "Demon Coots";

        for (float i = 0; i <= 1; i += Time.deltaTime / backgroundMovePeriod)
        {
            cloudsOrigin.transform.position = Vector2.Lerp(Vector2.zero, -cloudOriginStart, backgroundCurve.Evaluate(i));
            starsOrigin.transform.position = Vector2.Lerp(cloudOriginStart, Vector2.zero, backgroundCurve.Evaluate(i));
            backgroundRend.color = Color32.Lerp(startColor, endColor, i);
            cootsRend.color = Color32.Lerp(cootsColorStart, cootsColorEnd, i);

            pauseMovement = true;

            yield return null;
        }

        playerRigBod.constraints = RigidbodyConstraints2D.FreezeRotation;
        playerControl.canPlay = true;
        pauseMovement = false;
        destroyAllProjectiles = false;

        attackManager.ChooseAttack();

    }

    bool shouldMove;
    public bool pauseMovement;
    [SerializeField] float airMoveSpeed;
    IEnumerator SecondStageMovement()
    {
        shouldMove = true;
        float t = 0;

        while (shouldMove)
        {
            
            
            if (!pauseMovement)
            {
                transform.position = new Vector2(7, Mathf.PingPong(t, 6f) - 3f);
                t += Time.deltaTime * airMoveSpeed;    
            }
            yield return null;
        }
        
    }
}
