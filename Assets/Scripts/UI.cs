using System;
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
    GameObject pauseScreen;
    float fadeTime = 1f;
    float fadeAlphaLimit = 1f;
    bool isGameOver = false;
    List<Tuple<string, string>> optionsPause;
    int optionsPauseIndex;
    GameObject optionsPauseCursor;

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
        pauseScreen = transform.Find("PauseScreen").gameObject;
        pauseScreen.SetActive(false);

        optionsPause = new List<Tuple<string, string>>();
        optionsPause.Add(Tuple.Create("Resume", "ResumeGame"));
        optionsPause.Add(Tuple.Create("Main Menu", "GoMenu"));
        optionsPauseIndex = 0;
        optionsPauseCursor = transform.Find("PauseScreen/Options/Cursor").gameObject;

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

        if (Input.GetButtonDown("Cancel"))
        {
            if(pauseScreen.activeInHierarchy)
            {
                Time.timeScale = 1;
                pauseScreen.SetActive(false);
            }
            else
            {
                Time.timeScale = 0;
                pauseScreen.SetActive(true);
            }
        }

        if (optionsPauseCursor.activeInHierarchy && Input.GetButtonDown("Submit"))
        {
            Debug.Log(optionsPause[optionsPauseIndex]);
        }

        if (pauseScreen.activeInHierarchy && !Mathf.Approximately(Input.GetAxisRaw("Vertical"), 0))
        {
            float vertical = Input.GetAxisRaw("Vertical");
            if (vertical > 0 && optionsPauseIndex > 0)
            {
                optionsPauseIndex--;
                optionsPauseCursor.transform.localPosition = new Vector3(optionsPauseCursor.transform.localPosition.x, optionsPauseCursor.transform.localPosition.y + 60);
            }

            if (vertical < 0 && optionsPauseIndex < optionsPause.Count - 1)
            {
                optionsPauseIndex++;
                optionsPauseCursor.transform.localPosition = new Vector3(optionsPauseCursor.transform.localPosition.x, optionsPauseCursor.transform.localPosition.y - 60);
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

    public void ResumeGame()
    {
        Time.timeScale = 1;
        pauseScreen.SetActive(false);
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Home");
    }

    public void GoMenu()
    {
        ResumeGame();
        SceneManager.LoadScene("Menu");
    }

    public bool IsPaused()
    {
        return pauseScreen.activeInHierarchy;
    }
}
