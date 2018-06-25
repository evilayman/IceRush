using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    void Start()
    {
        GameObject GM = new GameObject();
        //Play("Theme", Instantiate(GM, transform));
        Play("Wind", Instantiate(GM, transform));
    }

    public void Play(string name, GameObject sourceObj)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);

        if (s == null)
            return;

        if (!sourceObj.GetComponent<AudioSource>())
        {
            s.source = sourceObj.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.spatialBlend = s.spatialBlend;
            s.source.loop = s.loop;
            s.source.rolloffMode = s.rollOff;
            s.source.maxDistance = s.maxDistance;
        }

        if (!s.source.isPlaying)
            s.source.Play();
    }

    public void PlayForce(string name, GameObject sourceObj)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);

        if (s == null)
            return;

        if (!sourceObj.GetComponent<AudioSource>())
        {
            s.source = sourceObj.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.spatialBlend = s.spatialBlend;
            s.source.loop = s.loop;
            s.source.rolloffMode = s.rollOff;
            s.source.maxDistance = s.maxDistance;
        }

        s.source.Play();
    }

    public void Stop(GameObject sourceObj)
    {
        if (sourceObj.GetComponent<AudioSource>())
            sourceObj.GetComponent<AudioSource>().Stop();
    }
}
