using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource sfxAudioSource;
    public AudioSource musicAudioSource;
    public static SoundManager instance;
    public float lowPitchRange = .95f;
    public float highPitchRange = 1.05f;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        Food.SetGameOver += StopMusic;
    }

    private void OnDisable()
    {
        Food.SetGameOver -= StopMusic;
    }

    public void PlaySingle(AudioClip clip)
    {
        sfxAudioSource.clip = clip;
        sfxAudioSource.Play();
    }

    public void RandomizeSfx(params AudioClip[] clips)
    {
        var randomIndex = Random.Range(0, clips.Length);

        var randomPitch = Random.Range(lowPitchRange, highPitchRange);

        sfxAudioSource.pitch = randomPitch;
        sfxAudioSource.clip = clips[randomIndex];
        sfxAudioSource.Play();
    }

    private void StopMusic()
    {
        musicAudioSource.Stop();
    }
}
