using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudAndStarManager : MonoBehaviour
{
    [SerializeField] GameObject starObj;
    [SerializeField] Transform starsOrigin;
    [SerializeField] Vector2 starAmountRange;
    [SerializeField] Vector2 starSizeRange;

    void Start()
    {
        StartCoroutine(SpawnClouds());

        int starAmount = (int)Random.Range(starAmountRange.x, starAmountRange.y);
        for (int i = 0; i < starAmount; i++)
        {
            GameObject star = Instantiate(starObj, starsOrigin.position, Quaternion.identity);
            star.transform.parent = starsOrigin;
            star.transform.localPosition = new Vector2(Random.Range(-8.00f, 8.00f), Random.Range(-4.75f, 4.75f));
            star.transform.localScale *= Random.Range(starSizeRange.x, starSizeRange.y);
        }
    }


    [SerializeField] GameObject cloudObj;
    [SerializeField] float spawnRate;
    [SerializeField] float maxHeight;
    [SerializeField] float rightMax;

    IEnumerator SpawnClouds()
    {
        //Wait
        float spawnRateOffset = Random.Range(0.60f, 1.00f);

        for (float i = 0; i < 1; i += Time.deltaTime * spawnRate * spawnRateOffset)
        {
            yield return null;
        }

        GameObject cloud = Instantiate(cloudObj, new Vector2(rightMax, Random.Range(-maxHeight * 100f, maxHeight * 100f) / 100f + transform.position.y), Quaternion.identity);
        cloud.transform.parent = transform;
        
        StartCoroutine(SpawnClouds());
    }

}
