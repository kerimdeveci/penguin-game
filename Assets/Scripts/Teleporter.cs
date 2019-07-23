using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Teleporter : MonoBehaviour
{
    public string destination;
    public Material material1;
    public Material material2;
    Renderer rend;
    UI ui;
    float timeStartFade = 0;
    float fadeTime;

    private void Start()
    {
        rend = GetComponent<Renderer>();
        ui = GameObject.FindGameObjectWithTag("Canvas").GetComponent<UI>();
        fadeTime = ui.GetFadeTime();
    }

    void Update()
    {
        
        float lerp = Mathf.PingPong(Time.time, 1) / 1;
        rend.material.Lerp(material1, material2, lerp);
        if (!Mathf.Approximately(timeStartFade, 0) && Time.time - timeStartFade > fadeTime)
        {
            SceneManager.LoadScene(destination);
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.name == "Player")
        {
            ui.DoFade();
            timeStartFade = Time.time;
        }
    }
}
