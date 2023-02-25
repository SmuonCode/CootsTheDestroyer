using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    void Start()
    {
        
    }

    [SerializeField] Transform background;
    [SerializeField] float backgroundSpeed;
    void Update()
    {
        background.position = new Vector2(background.position.x - (Time.deltaTime * backgroundSpeed), background.position.y);
        if (background.position.x < -9.4f)
        {
            background.position = new Vector2(29f, background.position.y);
        }
    }

    public void StartGame()
    {
        StartCoroutine(StartGameCoro());
    }

    [SerializeField] float fadeDuration;
    [SerializeField] SpriteRenderer fadeRend;
    [SerializeField] Color32 startColor;
    [SerializeField] Color32 endColor;

    [SerializeField] Transform canvasOrigin;
    [SerializeField] float originMoveDuration;
    [SerializeField] AnimationCurve originMoveCurve;
    [SerializeField] Vector2 originStart;
    [SerializeField] Vector2 originEnd;

    [SerializeField] float waitDuration;

    IEnumerator StartGameCoro()
    {
        for (float i = 0; i < 1; i += Time.deltaTime / originMoveDuration)
        {
            canvasOrigin.localPosition = Vector2.Lerp(originStart, originEnd, originMoveCurve.Evaluate(i));
            yield return null;
        }

        for (float i = 0; i < 1; i += Time.deltaTime / fadeDuration)
        {
            fadeRend.color = Color32.Lerp(startColor, endColor, i);
            yield return null;
        }

        yield return new WaitForSeconds(waitDuration);

        SceneManager.LoadScene("Game");
    }

    bool optionsVisible;
    [SerializeField] Transform optionsOrigin;
    public void ToggleOptions()
    {
        optionsVisible = !optionsVisible;

        if (optionsVisible)
        {
            optionsOrigin.localPosition = new Vector2(-10, 20);
        } else
        {
            optionsOrigin.localPosition = new Vector2(-10, 1000);
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
