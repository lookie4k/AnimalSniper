using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

    private static SoundManager instance;

    //public AudioSource source;
    public AudioClip[] soundClip;

    public List<AudioSource> gcSource;
    public AudioSource bgm;
    public AudioSource startBgm;


    private bool playOboe = false;

    void Awake()
    {
        instance = this;
        DontDestroyOnLoad(transform.gameObject);
        startBgm = PlayBgm();
        gcSource = new List<AudioSource>();
    }

    void Update()
    {
        for (int i = 0; i < gcSource.ToArray().Length; i++)
        {
            AudioSource source = gcSource[i];
            if (source.isPlaying)
                continue;
            gcSource.Remove(source);
            Destroy(source);
        }
    }


    public void PlaySound(int index, float min, float max, Vector3 pos)
    {
        AudioSource tempSource = gameObject.AddComponent<AudioSource>();
        tempSource.clip = soundClip[index];
        tempSource.spatialBlend = 1f;
        tempSource.spread = 1f;
        tempSource.maxDistance = max;
        tempSource.minDistance = min;
        tempSource.transform.position = pos;
        tempSource.Play();
        gcSource.Add(tempSource);
    }

    private AudioSource PlayBgm()
    {
        AudioSource tempSource = gameObject.AddComponent<AudioSource>();
        tempSource.clip = soundClip[5];
        tempSource.loop = true;
        tempSource.Play();
        bgm = tempSource;
        StartCoroutine(SoundUp(tempSource));

        return tempSource;
    }

    public void PlayBackgroundSound()
    {
        AudioSource tempSource = gameObject.AddComponent<AudioSource>();
        tempSource.clip = soundClip[3];
        tempSource.loop = true;
        tempSource.Play();
        bgm = tempSource;
        StartCoroutine(SoundUp(tempSource));
        gcSource.Add(tempSource);
    }

    private IEnumerator SoundUp(AudioSource tempSource)
    {
        for (float i = 0f; i <= 0.1f; i += 0.01f)
        {
            tempSource.volume = i;
            yield return new WaitForSeconds(0.03f);
        }
        yield return null;
    }

    public static SoundManager GetInstance()
    {
        return instance;
    }
}
