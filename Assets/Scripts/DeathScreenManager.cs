using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathScreenManager : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(VictoryAnimation());
    }

    void Update()
    {
        
    }

    [SerializeField] SpriteRenderer flashRend;
    [SerializeField] float flashDuration;
    [SerializeField] AnimationCurve flashCurve;
    [SerializeField] Color32 startColor;
    [SerializeField] Color32 endColor;
    [SerializeField] float waitDuration;

    [SerializeField] RectTransform cardRect;
    [SerializeField] float cardThrowDuration;
    [SerializeField] AnimationCurve cardCurve;
    [SerializeField] Vector2 cardStart;
    [SerializeField] Vector2 cardEnd;
    [SerializeField] float cardRotationStart;
    [SerializeField] float cardRotationEnd;

    [SerializeField] float menuWaitTime;
    [SerializeField] AudioSource flashSound;

    IEnumerator VictoryAnimation()
    {
        flashSound.Play();
        for (float i = 0; i < 1; i += Time.deltaTime / flashDuration)
        {
            flashRend.color = Color32.Lerp(startColor, endColor, flashCurve.Evaluate(i));
            
            yield return null;
        }

        yield return new WaitForSeconds(waitDuration);

        for (float i = 0; i < 1; i += Time.deltaTime / cardThrowDuration)
        {
            cardRect.localPosition = Vector2.Lerp(cardStart, cardEnd, cardCurve.Evaluate(i));
            cardRect.rotation = Quaternion.Lerp(Quaternion.Euler(0, 0, cardRotationStart), Quaternion.Euler(0, 0, cardRotationEnd), cardCurve.Evaluate(i));
            
            yield return null;
        }

        yield return new WaitForSeconds(menuWaitTime);

        SceneManager.LoadScene("MainMenu");
    }
}
