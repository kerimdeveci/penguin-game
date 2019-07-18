using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Teleporter : MonoBehaviour
{
    bool startFadeOut;
    float fadeScreenAlpha;
    bool fadeOutInProgress;
    GameObject canvas;
    GameObject fadeScreen;
    public string destination;
    public Material material1;
    public Material material2;
    Renderer rend;

    private void Start()
    {
        startFadeOut = false;
        fadeOutInProgress = false;
        fadeScreenAlpha = 0;
        canvas = GameObject.FindGameObjectWithTag("Canvas");
        fadeScreen = canvas.transform.Find("FadeScreen").gameObject;
        rend = GetComponent<Renderer>();
    }

    void Update()
    {
        
        float lerp = Mathf.PingPong(Time.time, 1) / 1;
        rend.material.Lerp(material1, material2, lerp);
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

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.name == "Player")
        {
            startFadeOut = true;
            fadeOutInProgress = true;
        }
    }
}
