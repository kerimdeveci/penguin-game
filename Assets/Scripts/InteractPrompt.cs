using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractPrompt : MonoBehaviour
{
    Player player;
    public float fadeDuration = 1f;
    CanvasGroup canvasGroup;
    bool fadeIn = false;
    bool fadeOut = false;

    // Start is called before the first frame update
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (fadeIn && canvasGroup.alpha < 1)
        {
            canvasGroup.alpha += 0.2f;
        }
        if (fadeOut && canvasGroup.alpha > 0)
        {
            canvasGroup.alpha -= 0.2f;
        }
        if (canvasGroup.alpha > 0)
        {
            if (player.IsListening())
            {
                fadeIn = false;
                fadeOut = true;
            }
        }
    }


    public bool IsFadingIn()
    {
        return fadeIn;
    }

    public bool IsFadingOut()
    {
        return fadeOut;
    }

    public void FadeIn()
    {
        fadeIn = true;
        fadeOut = false;
    }

    public void FadeOut()
    {
        fadeOut = true;
        fadeIn = false;
    }

    //public void OnEnable()
    //{
    //    canvasGroup.alpha = 0;
    //    fadeIn = true;
    //    Debug.Log("OnEnable");
    //}

    //public void OnDisable()
    //{
    //    if (canvasGroup.alpha == 1)
    //    {
    //        fadeOut = true;
    //        StartCoroutine("Fade");
    //    }
    //    Debug.Log("OnDisable");
    //}

    IEnumerator Fade()
    {
        for (float f = 1f; f >= 0; f -= 0.1f)
        {
            yield return new WaitForSeconds(2f);
        }
    }
}
