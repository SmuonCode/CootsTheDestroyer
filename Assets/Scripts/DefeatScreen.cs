using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class DefeatScreen : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(DropText());
    }

    [SerializeField] Transform textOrigin;
    [SerializeField] float dropDuration;
    [SerializeField] AnimationCurve textDropCurve;
    [SerializeField] Vector2 startPoint;
    [SerializeField] Vector2 endPoint;
    [SerializeField] float waitDuration;

    IEnumerator DropText()
    {
        yield return new WaitForSeconds(waitDuration);

        for (float i = 0; i < 1; i += Time.deltaTime / dropDuration)
        {
            textOrigin.localPosition = Vector2.Lerp(startPoint, endPoint, textDropCurve.Evaluate(i));
            yield return null;
        }

    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
