using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI : MonoBehaviour
{
    bool startFadeOut;
    float fadeScreenAlpha;
    bool fadeOutInProgress;
    GameObject fadeScreen;

    // Start is called before the first frame update
    void Start()
    {
        startFadeOut = false;
        fadeOutInProgress = false;
        fadeScreenAlpha = 0;
        fadeScreen = transform.Find("FadeScreen").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (startFadeOut)
        {
            if (!fadeOutInProgress)
            {
                SceneManager.LoadScene(destination);
            }
            else
            {
                fadeScreenAlpha += 0.02f;
                fadeScreen.GetComponent<CanvasGroup>().alpha = fadeScreenAlpha;
                if (fadeScreenAlpha >= 1)
                {
                    fadeOutInProgress = false;
                }
            }
        }
    }

    public void DoFade()
    {
        startFadeOut = true;
        fadeOutInProgress = true;
    }
}
