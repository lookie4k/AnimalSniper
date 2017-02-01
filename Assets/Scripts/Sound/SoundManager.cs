using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

    interface SoundListener
    {
        void OnSound(int index);
    }

    private static SoundManager instance;

    public AudioSource source;
    public AudioClip[] soundClip;


    private bool playOboe = false;

    private int oboeIndex;
    private int timpaniIndex;
    private int violinIndex;

    void Awake()
    {
        instance = this;
        DontDestroyOnLoad(transform.gameObject);
        SocketEvent();
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (playOboe)
        {
            PlaySound(soundClip, oboeIndex);
            playOboe = !playOboe;
        }
    }

    private void PlaySound(AudioClip[] clips, int index)
    {
        AudioSource tempSource = gameObject.AddComponent<AudioSource>();
        tempSource.clip = clips[index];
        tempSource.Play();
    }

    private void SocketEvent()
    {
    }

    public void OnSound(int index)
    {
        throw new NotImplementedException();
    }

    public static SoundManager GetInstance()
    {
        return instance;
    }
}
