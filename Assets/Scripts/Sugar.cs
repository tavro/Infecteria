using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sugar : MonoBehaviour
{
    private SpriteRenderer sr;

    public AudioSource audioSrc;
    public AudioClip soundClip;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        audioSrc = GameObject.Find("Sound Effect Player").GetComponent<AudioSource>();
    }

    void Update()
    {
        if(sr.color.a < 1.0f)
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, sr.color.a + Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D col) {
        if(col.gameObject.name == "Player") {
            GameObject.Find("GameManager").GetComponent<GameManager>().IncreaseSugar(40);
            
            audioSrc.clip = soundClip;
            audioSrc.Play();
            Destroy(gameObject);
        }
    }
}
