using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance = null;

    public AudioSource m_humanTheme;
    public AudioSource m_spiritTheme;
    private bool m_currentRealm = false;
    private bool m_changing = false;


    public AudioSource efxSource;

    public float lowPitchRange = .95f;
    public float highPitchRange = 1.05f;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        if (AudioListener.volume == 0.0f)
            AudioListener.volume = 1.0f;
        else
            AudioListener.volume = PlayerPrefs.GetFloat("Master Volume");
        if (m_humanTheme.volume == 0.0f)
            m_humanTheme.volume = 1.0f;
        else
            m_humanTheme.volume = PlayerPrefs.GetFloat("Music Volume");
        if (m_spiritTheme.volume == 0.0f)
            m_spiritTheme.volume = 1.0f;
        else
            m_spiritTheme.volume = PlayerPrefs.GetFloat("Music Volume");

        //DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        if (m_currentRealm != LevelData.m_spirit && !m_changing)
        {
            m_changing = true;
            if (LevelData.m_spirit)
                StartCoroutine(ChangeMusic(m_humanTheme, m_spiritTheme));
            else
                StartCoroutine(ChangeMusic(m_spiritTheme, m_humanTheme));
        }
    }

    private IEnumerator ChangeMusic(AudioSource p_audio1, AudioSource p_audio2)
    {
        float timeCounter = 0f;

        while (!(Mathf.Approximately(timeCounter, 1f)))
        {
            timeCounter = Mathf.Clamp01(timeCounter + 0.2f);
            if(p_audio1 != null)
                p_audio1.volume = 1f - timeCounter;
            if(p_audio2 != null)
                p_audio2.volume = timeCounter;
            yield return new WaitForSeconds(0.02f);
        }
        m_currentRealm = LevelData.m_spirit;
        m_changing = false;
        StopCoroutine("ChangeMusic");
    }

    public void PlayMusic()
    {
        if (m_humanTheme != null)
            m_humanTheme.Play();
        if (m_spiritTheme != null)
            m_spiritTheme.Play();
    }
    public void StopMusic()
    {
        if (m_humanTheme != null)
            m_humanTheme.Stop();
        if (m_spiritTheme != null)
            m_spiritTheme.Stop();
    }
    public void ChangeMusicVol(float p_value)
    {
        if (m_humanTheme != null)
            m_humanTheme.volume = p_value;
        if (m_spiritTheme != null)
            m_spiritTheme.volume = p_value;
    }
    public bool MusicPlays()
    {
        bool m_musicPlaying = false;
        if (m_humanTheme != null && m_humanTheme.isPlaying)
            m_musicPlaying = true;
        if (m_spiritTheme != null && m_spiritTheme.isPlaying)
            m_musicPlaying = true;
        return m_musicPlaying;
    }

    public void PlaySingle(AudioClip clip)
    {
        if (efxSource != null)
        {
            efxSource.clip = clip;
            efxSource.Play();
        }
    }

    public void RandomizeSfx(params AudioClip[] clips)
    {
        int randomIndex = Random.Range(0, clips.Length);
        float randomPitch = Random.Range(lowPitchRange, highPitchRange);

        efxSource.pitch = randomPitch;
        efxSource.clip = clips[randomIndex];
        efxSource.Play();
    }

    public void PlayAttachedClip()
    {
        efxSource.Play();
    }
}
