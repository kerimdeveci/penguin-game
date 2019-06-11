using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractPrompt : MonoBehaviour
{
    public float fadeDuration = 1f;
    float timer;
    CanvasGroup canvasGroup;
    bool fadeIn = false;
    bool fadeOut = false;

    // Start is called before the first frame update
    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    // Update is called once per frame
    void Update()
    {
        if (fadeIn)
        {
            timer += Time.deltaTime;
            canvasGroup.alpha = timer / fadeDuration;
        }
        if (fadeOut)
        {
            timer += Time.deltaTime;
            canvasGroup.alpha = timer * fadeDuration;
        }
    }

    public void OnEnable()
    {
        canvasGroup.alpha = 0;
        fadeIn = true;
        Debug.Log("OnEnable");
    }

    public void OnDisable()
    {
        if (canvasGroup.alpha == 1)
        {
            fadeOut = true;
            StartCoroutine("Fade");
        }
        Debug.Log("OnDisable");
    }

    IEnumerator Fade()
    {
        for (float f = 1f; f >= 0; f -= 0.1f)
        {
            yield return new WaitForSeconds(2f);
        }
    }
}
