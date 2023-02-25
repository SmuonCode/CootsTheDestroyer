using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundCloudBehavior : MonoBehaviour
{
    [SerializeField] SpriteRenderer rend;
    [SerializeField] Rigidbody2D rigBod;
    [SerializeField] float speed;
    float disMultiplier;

    [SerializeField] Sprite[] spirtes;

    void Start()
    {
        disMultiplier = Random.Range(0.30f, 1.00f);
        transform.localScale *= disMultiplier;
        rend.color = new Color32(255, 255, 255, (byte)(255f * disMultiplier));
        rend.sprite = spirtes[Random.Range(0, 4)];

        rigBod.velocity = new Vector2(disMultiplier * speed, 0);
    }

    void Update()
    {
        //transform.position = new Vector2(transform.position.x * disMultiplier * speed * Time.deltaTime, transform.position.y);

        if (transform.position.x < -14f)
        {
            Destroy(this.gameObject);
        }
    }
}
