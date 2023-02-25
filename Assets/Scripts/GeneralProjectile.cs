using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralProjectile : MonoBehaviour
{
    public int damage;
    public string projectileType;
    public GameObject player;
    [SerializeField] Rigidbody2D rigBod;
    public float speed;

    void Start()
    {
        if (projectileType == "Missile")
        {
            missilePercent = 1f;
        }

        if (projectileType == "Explosion")
        {
            explosionSound.Play();
        }
    }

    [SerializeField] GameObject mouseTrapExplosionObj;
    public CootsBehavior cootsBehavior;
    public bool shouldRotateWithVelocity;

    [SerializeField] AudioSource explosionSound;

    void Update()
    {
        if (projectileType == "MouseTrap" && transform.position.x <= player.transform.position.x)
        {
            GameObject explosion = Instantiate(mouseTrapExplosionObj, transform.position, Quaternion.identity);
            explosion.GetComponent<GeneralProjectile>().cootsBehavior = cootsBehavior;
            explosion.GetComponent<GeneralProjectile>().projectileType = "Explosion";
            explosion.GetComponent<Rigidbody2D>().velocity = new Vector2(-10, 0);
            Destroy(explosion, 0.5f);
            Destroy(this.gameObject);
        }

        if (projectileType != "FireBreath" && cootsBehavior.destroyAllProjectiles)
        {
            if (particleObj != null)
            {
                ReleaseParticles();
            }
            Destroy(this.gameObject);
        }

        if (shouldRotateWithVelocity && rigBod.velocity.x != 0f)
        {
            //Quaternion lookTo = Quaternion.LookRotation(Vector3.forward, rigBod.velocity);
            //transform.rotation = Quaternion.RotateTowards(transform.rotation, lookTo, 1f);
            //I figured out how to do this all by myself lol
            float angle = Mathf.Atan2(rigBod.velocity.y, rigBod.velocity.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }

        if (transform.position.x >= 10 || transform.position.x <= -10 || transform.position.y >= 6 || transform.position.y <= -6)
        {
            if (projectileType == "HairBall")
            {
                Destroy(this.gameObject);
            }
            
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Ground" && projectileType != "MouseTrap" && projectileType != "Explosion" && projectileType != "Laser" && projectileType != "Missile" && projectileType != "FireBreath")
        {
            Destroy(this.gameObject);
        }
        
        if (projectileType == "Missile" || projectileType == "MouseTrap")
        {
            if (collision.gameObject.CompareTag("PlayerProjectile") || collision.gameObject.name == "Player")
            {
                GameObject explosion = Instantiate(mouseTrapExplosionObj, transform.position, Quaternion.identity);
                explosion.GetComponent<GeneralProjectile>().cootsBehavior = cootsBehavior;
                explosion.GetComponent<GeneralProjectile>().projectileType = "Explosion";
                Destroy(explosion, 0.5f);
                Destroy(this.gameObject);
            }
        }
    }

    [SerializeField] float actualVectorWeight;
    [SerializeField] float missileTime;
    [SerializeField] float missilePercent;
    [SerializeField] Color32 missileExplodeColor;
    [SerializeField] SpriteRenderer rend;
    [SerializeField] float missileSpeed;

    [SerializeField] float boomerangRotationSpeed;

    void FixedUpdate()
    {
        if (projectileType == "HairBall")
        {
            rigBod.velocity = ((rigBod.velocity + ((Vector2)(player.transform.position - transform.position)) / 15f) / 2f).normalized * speed;
        }

        if (projectileType == "Missile")
        {
            Vector2 actualVector = player.transform.position - transform.position;
            rigBod.velocity = ((rigBod.velocity * (1f - actualVectorWeight)) + (actualVector * actualVectorWeight)).normalized * missileSpeed;

            missilePercent -= missileTime;
            rend.color = Color32.Lerp(missileExplodeColor, Color.white, missilePercent);

        }

        if (projectileType == "Missile" && missilePercent <= 0f)
        {
            GameObject explosion = Instantiate(mouseTrapExplosionObj, transform.position, Quaternion.identity);
            explosion.GetComponent<GeneralProjectile>().cootsBehavior = cootsBehavior;
            explosion.GetComponent<GeneralProjectile>().projectileType = "Explosion";
            Destroy(explosion, 0.5f);
            Destroy(this.gameObject);
        }

        if (projectileType == "Boomerang")
        {
            transform.Rotate(0, 0, boomerangRotationSpeed);
        }
    }
    
    [SerializeField] GameObject particleObj;
    void ReleaseParticles()
    {
        GameObject obj = Instantiate(particleObj, transform.position, Quaternion.identity);
        obj.GetComponent<ParticleSystem>().Play();
        Destroy(obj, 5);
    }
}
