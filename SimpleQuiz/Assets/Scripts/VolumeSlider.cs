using UnityEngine;

public class VolumeSlider : MonoBehaviour
{

    private AudioSource AudioSrc;

    private float AudioVolume = 1f;

    void Start()
    {
        AudioSrc = SoundManager.sm.audioController;
    }

    void Update()
    {
        AudioSrc.volume = AudioVolume;
    }

    public void SetVolume(float vol)
    {
        AudioVolume = vol;
    }
}
