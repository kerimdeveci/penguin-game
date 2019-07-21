using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageNumber : MonoBehaviour
{
    public GameObject Target { get; set; }
    public int Amount { get; set; }
    float timeSpawned;

    // Start is called before the first frame update
    void Start()
    {
        if (Target != null)
        {
            transform.position = Target.transform.position;
            timeSpawned = Time.time;
            iTween.Init(this.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Target != null && Time.time - timeSpawned > 0.2f)
        {
            iTween.FadeTo(gameObject, iTween.Hash("alpha", 0, "time", 0.3f, "oncomplete", "CallbackDestroy", "oncompletetarget", this.gameObject));
        }
    }

    void CallbackDestroy()
    {
        Destroy(gameObject);
    }
}
