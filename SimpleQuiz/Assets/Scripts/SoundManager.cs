using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager sm;

    [Header("Background Music")]
    public AudioClip BackgroundMusic;

    [Header("Sound Effects")]
    public AudioClip[] CorrectAnswers;
    public AudioClip[] WrongAnswers;

    [HideInInspector]
    public AudioSource audioController;

    // Start is called before the first frame update
    void Awake()
    {
        if (sm == null)
        {
            sm = GetComponent<SoundManager>();
        }

        audioController = gameObject.AddComponent<AudioSource>();
        DontDestroyOnLoad(this);
    }

    void Start()
    {
        audioController.clip = BackgroundMusic;
        audioController.Play();
        audioController.loop = true;
    }
    
}
