using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSound : MonoBehaviour {

    public AudioClip[] soundClip;

    private List<AudioSource> gcSource;

    // Use this for initialization
    void Start () {
        gcSource = new List<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () {
        for (int i = 0; i < gcSource.ToArray().Length; i++)
        {
            AudioSource source = gcSource[i];
            if (source.isPlaying)
                continue;
            gcSource.Remove(source);
            Destroy(source);
        }
    }

    public void PlaySound(int index, float min, float max)
    {
        AudioSource tempSource = gameObject.AddComponent<AudioSource>();
        tempSource.clip = soundClip[index];
        tempSource.spatialBlend = 1f;
        tempSource.spread = 1f;
        tempSource.maxDistance = max;
        tempSource.minDistance = min;
        tempSource.Play();
        gcSource.Add(tempSource);
    }
}
