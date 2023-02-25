using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackManager : MonoBehaviour
{
    void Start()
    {
        //ChooseAttack();
        stage = 1;
    }

    public int stage;
    //Tank Burst Shot - - -
    //Tank Large Shot - - -
    //Hairball - - -
    //Mouse Trap - - -

    //Eye Laser - - -
    //Drone Spear Throw - - -
    //Drone Propeller Boomerang - - -
    //Rain Cloud - -
    //Drone Machine Gun - - -

    //Meteors - -
    //Split Eye Laser - - -
    //Fire Breath - - -
    //Flaming Hairball Burst Shot - - -
    //Missile - - -

    public void ChooseAttack()
    {
        int attackNum = Random.Range(1, 6);
        //int attackNum = 5;

        if (stage == 1 && playerControl.canPlay)
        {
            if (attackNum == 1){StartCoroutine(TankBurstShot());}
            if (attackNum == 2){StartCoroutine(TankLargeShot());}
            if (attackNum == 3){StartCoroutine(MouseTrap());}
            if (attackNum == 4){StartCoroutine(HairBall());}
            if (attackNum == 5){ChooseAttack();}
        }
        if (stage == 2 && playerControl.canPlay)
        {
            if (attackNum == 1){StartCoroutine(DroneRapidFire());}
            if (attackNum == 2){StartCoroutine(DroneHarpoon());}
            if (attackNum == 3){StartCoroutine(RainCloud());}
            if (attackNum == 4){StartCoroutine(DroneBoomerang());}
            if (attackNum == 5){StartCoroutine(DroneLaser());}
        }
        if (stage == 3 && playerControl.canPlay)
        {
            if (attackNum == 1){StartCoroutine(SpawnMeteors());}
            if (attackNum == 2){StartCoroutine(SpawnMissile());}
            if (attackNum == 3){StartCoroutine(FireBreath());}
            if (attackNum == 4){StartCoroutine(DemonHairballs());}
            if (attackNum == 5){StartCoroutine(EyeLasers());}
        }
    }

    [SerializeField] GameObject player;
    [SerializeField] CootsBehavior cootsBehavior;
    [SerializeField] PlayerControl playerControl;

    [SerializeField] GameObject tankBurst_ShotObj;
    [SerializeField] int tankBurst_ShotSpeed;
    [SerializeField] float tankBurst_DelayTime;

    [SerializeField] Vector3 tankBurstOffset;
    [SerializeField] AudioSource tankShotSound;

    IEnumerator TankBurstShot()
    {
        //Rectract tank barrel
        for (float i = 0; i < 1; i+= Time.deltaTime)
        {
            yield return null;
        }

        Vector2 vector = (player.transform.position - transform.position + tankBurstOffset).normalized;

        for (int i = 0; i < 3; i++)
        {
            GameObject shot = Instantiate(tankBurst_ShotObj, transform.position + tankBurstOffset, Quaternion.identity);
            shot.GetComponent<Rigidbody2D>().velocity = vector * tankBurst_ShotSpeed;
            shot.GetComponent<GeneralProjectile>().cootsBehavior = cootsBehavior;
            Destroy(shot, 10);

            tankShotSound.Play();

            yield return new WaitForSeconds(tankBurst_DelayTime);
        }

        ChooseAttack();
    }

    [SerializeField] Vector2 largeShotVector;
    [SerializeField] Vector3 tankLargeShotOffset;
    IEnumerator TankLargeShot()
    {
        //Rectract tank barrel
        for (float i = 0; i < 1.5f; i+= Time.deltaTime)
        {
            yield return null;
        }

        //Vector2 vector = (player.transform.position - transform.position).normalized;

        GameObject shot = Instantiate(tankBurst_ShotObj, transform.position + tankLargeShotOffset, Quaternion.identity);
        shot.GetComponent<Rigidbody2D>().velocity =  largeShotVector.normalized * tankBurst_ShotSpeed;
        shot.GetComponent<Rigidbody2D>().gravityScale = 0.4f;
        shot.transform.localScale *= 2;
        shot.GetComponent<GeneralProjectile>().damage = 2;
        shot.GetComponent<GeneralProjectile>().cootsBehavior = cootsBehavior;
        Destroy(shot, 10);

        tankShotSound.Play();

        ChooseAttack();
    }

    [SerializeField] GameObject mouseTrapObj;
    [SerializeField] AudioSource mouseTrapThrowSound;
    IEnumerator MouseTrap()
    {
        //Startup animation
        for (float i = 0; i < 1.5f; i+= Time.deltaTime)
        {
            yield return null;
        }

        GameObject trap = Instantiate(mouseTrapObj, transform.position, Quaternion.identity);
        trap.GetComponent<Rigidbody2D>().velocity = new Vector2(-10, 10);
        trap.GetComponent<GeneralProjectile>().player = player;
        trap.GetComponent<GeneralProjectile>().cootsBehavior = cootsBehavior;
        Destroy(trap, 10);

        mouseTrapThrowSound.Play();

        ChooseAttack();
    }

    [SerializeField] float hairBallSpeed;
    [SerializeField] GameObject hairBallObj;
    [SerializeField] Vector3 hairBallOffset;

    [SerializeField] AudioSource hairBallLaunchSound;
    IEnumerator HairBall()
    {
        //Startup animation
        for (float i = 0; i < 1.5f; i+= Time.deltaTime)
        {
            yield return null;
        }

        GameObject ball = Instantiate(hairBallObj, transform.position + hairBallOffset, Quaternion.identity);
        Vector2 vector = (player.transform.position - transform.position).normalized;
        ball.GetComponent<Rigidbody2D>().velocity = vector * hairBallSpeed;
        ball.GetComponent<GeneralProjectile>().player = player;
        ball.GetComponent<GeneralProjectile>().speed = hairBallSpeed;
        ball.GetComponent<GeneralProjectile>().cootsBehavior = cootsBehavior;

        hairBallLaunchSound.Play();

        ChooseAttack();
    }

    //PHASE TWO
    
    [SerializeField] GameObject droneBurstShotObj;
    [SerializeField] float droneBurstShotSpeed;
    [SerializeField] float droneBurstShotDelay;

    [SerializeField] Vector3 droneRapidFireOffset;

    [SerializeField] AudioSource droneRapidShotSound;

    IEnumerator DroneRapidFire()
    {
        //Startup animation
        for (float i = 0; i < 1.5f; i+= Time.deltaTime)
        {
            yield return null;
        }

        cootsBehavior.pauseMovement = true;

        Vector2 vector = (player.transform.position - transform.position + droneRapidFireOffset).normalized;

        for (int i = 0; i < 20; i++)
        {
            if (!cootsBehavior.destroyAllProjectiles)
            {
                GameObject shot = Instantiate(droneBurstShotObj, transform.position + droneRapidFireOffset, Quaternion.identity);
                shot.GetComponent<Rigidbody2D>().velocity = vector * droneBurstShotSpeed;
                shot.GetComponent<GeneralProjectile>().cootsBehavior = cootsBehavior;
                shot.GetComponent<GeneralProjectile>().shouldRotateWithVelocity = true;
                Destroy(shot, 10);

                Vector2 newVector = (player.transform.position - transform.position).normalized;
                vector = (vector * 0.9f) + (newVector * 0.1f);

                droneRapidShotSound.Play();

                yield return new WaitForSeconds(droneBurstShotDelay);
            }
            
        }

        cootsBehavior.pauseMovement = false;
        ChooseAttack();
    }

    [SerializeField] GameObject droneHarpoonObj;
    [SerializeField] float droneHarpoonPeriod;
    [SerializeField] float harpoonDistanceMultiplier;
    [SerializeField] float harpoonOffset;

    IEnumerator DroneHarpoon()
    {
        //Startup animation
        for (float i = 0; i < 1.5f; i+= Time.deltaTime)
        {
            yield return null;
        }

        cootsBehavior.pauseMovement = true;

        GameObject harpoon = Instantiate(droneHarpoonObj, transform.position, Quaternion.identity);
        harpoon.GetComponent<GeneralProjectile>().cootsBehavior = cootsBehavior;
        harpoon.GetComponent<GeneralProjectile>().projectileType = "Harpoon";

        boomerangThrowSound.Play();
        //harpoon.GetComponent<Rigidbody2D>.velocity = new Vector2(-droneHarpoonSpeed, 0);

        for (float i = -1; i < 1; i += Time.deltaTime / droneHarpoonPeriod)
        {
            if (!cootsBehavior.destroyAllProjectiles)
            {
                harpoon.transform.position = new Vector2((Mathf.Pow(i, 2) * harpoonDistanceMultiplier) + harpoonOffset, transform.position.y);

                yield return null;
            }
        }

        if (!cootsBehavior.destroyAllProjectiles)
        {
            Destroy(harpoon);
        }
        

        cootsBehavior.pauseMovement = false;
        ChooseAttack();
    }

    /* [SerializeField] GameObject laser;
    [SerializeField] float maxRotaion;
    [SerializeField] float minRotaion;
    [SerializeField] float laserPeriod;

    IEnumerator EyeLaser()
    {
        //Startup animation
        for (float i = 0; i < 1; i+= Time.deltaTime)
        {
            yield return null;
        }

        cootsBehavior.pauseMovement = true;

        //laser.transform.eulerAngles.y = minRotaion;
        for (float i = 0; i < 1; i+= Time.deltaTime)
        {
            yield return null;
        }
    } */

    [SerializeField] GameObject rainCloud;
    [SerializeField] Vector2 trueStart;
    [SerializeField] Vector2 cloudStart;
    [SerializeField] Vector2 cloudEnd;
    [SerializeField] float cloudMoveSpeed;
    [SerializeField] float cloudAttackSpeed;
    bool shouldRain;

    [SerializeField] AudioSource rainSound;

    IEnumerator RainCloud()
    {
        //Startup animation
        for (float i = 0; i < 1.5f; i+= Time.deltaTime)
        {
            yield return null;
        }

        for (float i = 0; i < 1; i += Time.deltaTime * cloudMoveSpeed)
        {
            rainCloud.transform.position = Vector2.Lerp(trueStart, cloudStart, i);
            yield return null;
        }

        rainSound.Play();
        StartCoroutine(SpawnRain());
        for (float i = 0; i < 1; i += Time.deltaTime * cloudAttackSpeed)
        {
            rainCloud.transform.position = Vector2.Lerp(cloudStart, cloudEnd, i);
            yield return null;
        }
        for (float i = 0; i < 1; i += Time.deltaTime * cloudAttackSpeed)
        {
            rainCloud.transform.position = Vector2.Lerp(cloudEnd, cloudStart, i);
            yield return null;
        }
        shouldRain = false;

        for (float i = 0; i < 1; i += Time.deltaTime * cloudMoveSpeed)
        {
            rainCloud.transform.position = Vector2.Lerp(cloudStart, trueStart, i);
            yield return null;
        }

        ChooseAttack();
    }

    [SerializeField] GameObject rainObj;
    [SerializeField] float spawnDelay;
    [SerializeField] float rainXRange;
    [SerializeField] float rainYOffset;
    float spawnTime;

    IEnumerator SpawnRain()
    {
        shouldRain = true;

        while (shouldRain && !cootsBehavior.destroyAllProjectiles)
        {
            if (spawnTime <= 0f)
            {
                GameObject droplet = Instantiate(rainObj, new Vector2(rainCloud.transform.position.x + Random.Range(-rainXRange, rainXRange), rainCloud.transform.position.y - rainYOffset * Random.Range(0.80f, 1.20f)), Quaternion.identity);
                droplet.GetComponent<GeneralProjectile>().cootsBehavior = cootsBehavior;
                droplet.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 1);
                Destroy(droplet, 3);
                
                spawnTime = spawnDelay;
            }
            
            spawnTime -= Time.deltaTime;
            yield return null;
        }
    }

    [SerializeField] GameObject boomerangObject;
    [SerializeField] AnimationCurve boomerangYCurve;
    [SerializeField] float curveMultiplier;
    [SerializeField] float XMultiplier;
    [SerializeField] float boomerangPeriod;
    [SerializeField] float boomerangOffset;

    [SerializeField] AudioSource boomerangThrowSound;

    IEnumerator DroneBoomerang()
    {
        //Startup animation
        for (float i = 0; i < 1.5f; i+= Time.deltaTime)
        {
            yield return null;
        }

        cootsBehavior.pauseMovement = true;
        Vector2 startVector = transform.position;

        for (float i = 0; i < 1; i += Time.deltaTime)
        {
            transform.position = Vector2.Lerp(startVector, new Vector2(7, 0), i);
            yield return null;
        }

        GameObject boomerang = Instantiate(boomerangObject, transform.position, Quaternion.identity);
        boomerang.GetComponent<GeneralProjectile>().cootsBehavior = cootsBehavior;
        boomerang.GetComponent<GeneralProjectile>().projectileType = "Boomerang";
        boomerangThrowSound.Play();

        
        for (float i = -1f; i < 1f; i += Time.deltaTime / boomerangPeriod)
        {
            if (!cootsBehavior.destroyAllProjectiles)
            {
                boomerang.transform.position = new Vector2((Mathf.Pow(i, 2f) * XMultiplier) + boomerangOffset, boomerangYCurve.Evaluate((i + 1f) / 2f) * curveMultiplier - 3f);
                transform.position = new Vector2(7, 0);

                yield return null;
            }
        }
        
        if (!cootsBehavior.destroyAllProjectiles)
        {
            Destroy(boomerang);
        }

        for (float i = 0; i < 1; i += Time.deltaTime)
        {
            transform.position = Vector2.Lerp(new Vector2(7, 0), startVector, i);
            yield return null;
        }

        cootsBehavior.pauseMovement = false;
        ChooseAttack();
    }

    [SerializeField] float droneLaserStart;
    [SerializeField] float droneLaserEnd;
    [SerializeField] GameObject droneLaserObj;
    [SerializeField] AnimationCurve droneLaserSpeedCurve;
    [SerializeField] float droneLaserPeriod;

    [SerializeField] Vector2 droneLaserOffset;

    [SerializeField] AudioSource laserSound;

    IEnumerator DroneLaser()
    {
        //Startup animation
        for (float i = 0; i < 1.5f; i+= Time.deltaTime)
        {
            yield return null;
        }

        cootsBehavior.pauseMovement = true;
        Vector2 startVector = transform.position;

        for (float i = 0; i < 1; i += Time.deltaTime)
        {
            transform.position = Vector2.Lerp(startVector, new Vector2(7, 0), i);
            yield return null;
        }

        GameObject laser = Instantiate(droneLaserObj, new Vector2(7, 0) + droneLaserOffset, Quaternion.identity);
        laser.GetComponent<GeneralProjectile>().cootsBehavior = cootsBehavior;
        laser.GetComponent<GeneralProjectile>().projectileType = "Laser";

        laserSound.Play();
        
        if (!cootsBehavior.destroyAllProjectiles)
        {
            for (float i = 0; i < 1; i += Time.deltaTime / droneLaserPeriod)
            {
                laser.transform.rotation = Quaternion.Lerp(Quaternion.Euler(0, 0, droneLaserStart), Quaternion.Euler(0, 0, droneLaserEnd), droneLaserSpeedCurve.Evaluate(i));
                transform.position = new Vector2(7, 0);

                yield return null;
            }

            Destroy(laser);
        }

        for (float i = 0; i < 1; i += Time.deltaTime)
        {
            transform.position = Vector2.Lerp(new Vector2(7, 0), startVector, i);
            yield return null;
        }

        cootsBehavior.pauseMovement = false;

        ChooseAttack();

    }

    //PHASE THREE

    [SerializeField] GameObject meteorObj;
    [SerializeField] float baseMeteorSpeed;
    IEnumerator SpawnMeteors()
    {
        //Startup animation
        for (float i = 0; i < 1.5f; i+= Time.deltaTime)
        {
            yield return null;
        }

        int numToSpawn = Random.Range(1, 4);

        for (int i = 0; i < numToSpawn; i++)
        {
            GameObject meteor = Instantiate(meteorObj, new Vector2(Random.Range(-8.00f, 5.25f), Random.Range(6.00f, 7.50f)), Quaternion.identity);
            meteor.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, -Random.Range(baseMeteorSpeed - 0.5f, baseMeteorSpeed + 0.5f));
            meteor.GetComponent<GeneralProjectile>().cootsBehavior = cootsBehavior;

            Destroy(meteor, 25f);

        }

        ChooseAttack();
    }

    [SerializeField] Vector2[] possibleSpawnLocations;
    [SerializeField] GameObject missileObject;

    IEnumerator SpawnMissile()
    {
        //Startup animation
        for (float i = 0; i < 1.5f; i+= Time.deltaTime)
        {
            yield return null;
        }

        GameObject missile = Instantiate(missileObject, possibleSpawnLocations[Random.Range(0, possibleSpawnLocations.Length)], Quaternion.identity);
        missile.GetComponent<GeneralProjectile>().cootsBehavior = cootsBehavior;
        missile.GetComponent<GeneralProjectile>().projectileType = "Missile";
        missile.GetComponent<GeneralProjectile>().shouldRotateWithVelocity = true;
        missile.GetComponent<GeneralProjectile>().player = player;

        ChooseAttack();
    }

    [SerializeField] GameObject fireObj;
    [SerializeField] float fireDuration;

    [SerializeField] Vector2 fireBreathOffset;

    [SerializeField] AudioSource fireBreathSound;

    IEnumerator FireBreath()
    {
        //Startup animation
        for (float i = 0; i < 1.5f; i+= Time.deltaTime)
        {
            yield return null;
        }

        fireObj.transform.localPosition = fireBreathOffset;
        fireBreathSound.Play();

        for (float i = 0; i < fireDuration; i+= Time.deltaTime)
        {
            yield return null;
        }

        fireObj.transform.localPosition = new Vector2(10, 0);

        ChooseAttack();
    }

    [SerializeField] GameObject demonHairballObj;
    [SerializeField] float demonHairballsSpeed;
    [SerializeField] float demonHairBallDelay;

    [SerializeField] Vector3 demonHairBallOffset;

    IEnumerator DemonHairballs()
    {
        //Startup animation - Reuse fire breath animation
        for (float i = 0; i < 1.5f; i+= Time.deltaTime)
        {
            yield return null;
        }

        for (int i = 0; i < 7; i++)
        {
            GameObject hairball = Instantiate(demonHairballObj, transform.position + demonHairBallOffset, Quaternion.identity);
            hairball.GetComponent<Rigidbody2D>().velocity = new Vector2(-demonHairballsSpeed, Random.Range(-2.00f, 2.00f));
            hairball.GetComponent<GeneralProjectile>().cootsBehavior = cootsBehavior;
            hairball.GetComponent<GeneralProjectile>().shouldRotateWithVelocity = true;
            Destroy(hairball, 10);

            hairBallLaunchSound.Play();

            yield return new WaitForSeconds(demonHairBallDelay);
        }

        ChooseAttack();
    }

    [SerializeField] float eyeLaserPeriod;
    [SerializeField] AnimationCurve eyeLaserCurve;

    [SerializeField] Vector2 eyeLaserOffset;

    IEnumerator EyeLasers()
    {
        //Startup animation
        for (float i = 0; i < 1.5f; i+= Time.deltaTime)
        {
            yield return null;
        }

        cootsBehavior.pauseMovement = true;
        Vector2 startVector = transform.position;

        for (float i = 0; i < 1; i += Time.deltaTime)
        {
            transform.position = Vector2.Lerp(startVector, new Vector2(7, 0), i);
            yield return null;
        }

        GameObject laserTop = Instantiate(droneLaserObj, new Vector2(7, 0) + eyeLaserOffset, Quaternion.identity);
        GameObject laserBottom = Instantiate(droneLaserObj, new Vector2(7, 0) + eyeLaserOffset, Quaternion.identity);
        laserTop.GetComponent<GeneralProjectile>().cootsBehavior = cootsBehavior;
        laserTop.GetComponent<GeneralProjectile>().projectileType = "Laser";
        laserBottom.GetComponent<GeneralProjectile>().cootsBehavior = cootsBehavior;
        laserBottom.GetComponent<GeneralProjectile>().projectileType = "Laser";

        laserSound.Play();
        
        /* if (!cootsBehavior.destroyAllProjectiles)
        { */
            for (float i = 0; i < 1; i += Time.deltaTime / eyeLaserPeriod)
            {
                laserTop.transform.rotation = Quaternion.Lerp(Quaternion.Euler(0, 0, 90), Quaternion.Euler(0, 0, 1), eyeLaserCurve.Evaluate(i));
                laserBottom.transform.rotation = Quaternion.Lerp(Quaternion.Euler(0, 0, 90), Quaternion.Euler(0, 0, 179), eyeLaserCurve.Evaluate(i));
                transform.position = new Vector2(7, 0);

                yield return null;
            }

            Destroy(laserTop);
            Destroy(laserBottom);
        //}

        for (float i = 0; i < 1; i += Time.deltaTime)
        {
            transform.position = Vector2.Lerp(new Vector2(7, 0), startVector, i);
            yield return null;
        }

        cootsBehavior.pauseMovement = false;

        ChooseAttack();
    }
}
