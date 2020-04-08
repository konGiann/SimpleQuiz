using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeControl : MonoBehaviour
{
    AudioSource audioSource;

    float volume = 1f;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = SoundManager.sm.audioController;    
    }

    // Update is called once per frame
    void Update()
    {
        audioSource.volume = volume;
    }

    public void SetVolume(float vol)
    {
        volume = vol;
    }
}
