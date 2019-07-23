using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI : MonoBehaviour
{
    bool startFadeOut;
    bool startFadeIn;
    float fadeScreenAlpha;
    bool fadeOutInProgress;
    bool fadeInInProgress;
    GameObject fadeScreen;
    float fadeTime = 1f;
    float fadeAlphaLimit = 1f;
    bool isGameOver = false;

    // Start is called before the first frame update
    void Start()
    {
        startFadeIn = true;
        startFadeOut = false;
        fadeInInProgress = true;
        fadeOutInProgress = false;
        fadeScreenAlpha = 1;
        fadeScreen = transform.Find("FadeScreen").gameObject;
        fadeScreen.GetComponent<CanvasGroup>().alpha = fadeScreenAlpha;
    }

    // Update is called once per frame
    void Update()
    {
        if (startFadeIn)
        {
            if (!fadeInInProgress)
            {
            }
            else
            {
                fadeScreenAlpha -= 0.02f;
                fadeScreen.GetComponent<CanvasGroup>().alpha = fadeScreenAlpha;
                if (fadeScreenAlpha <= 0)
                {
                    fadeInInProgress = false;
                }
            }
        }

        if (startFadeOut)
        {
            if (!fadeOutInProgress)
            {
            }
            else
            {
                fadeScreenAlpha += 0.02f;
                fadeScreen.GetComponent<CanvasGroup>().alpha = fadeScreenAlpha;
                if (fadeScreenAlpha >= fadeAlphaLimit)
                {
                    fadeOutInProgress = false;
                    if (isGameOver)
                    {
                        Debug.Log("U R DEAD");
                        ReloadScene();
                    }
                }
            }
        }
    }

    public void DoFade()
    {
        startFadeOut = true;
        fadeOutInProgress = true;
    }

    public float GetFadeTime()
    {
        return fadeTime;
    }

    public void DoGameOver()
    {
        DoFade();
        isGameOver = true;
    }

    void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
