using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeManager : MonoBehaviour {

    public Slider master;
    public Slider music;
    public Slider sfx;

    private void Awake()
    {
        if (master.value == 0.0f)
            master.value = 1.0f;
        else
            master.value = PlayerPrefs.GetFloat("Master Volume");

        if (music.value == 0.0f)
            music.value = 1.0f;
        else
            music.value = PlayerPrefs.GetFloat("Music Volume");

        if (sfx.value == 0.0f)
            sfx.value = 1.0f;
        else
            sfx.value = PlayerPrefs.GetFloat("SFX Volume");
    }

    public void masterVol()
    {
        AudioListener.volume = master.value;
        PlayerPrefs.SetFloat("Master Volume", master.value);
    }

    public void musicVol()
    {
        SoundManager.instance.ChangeMusicVol(music.value);
        PlayerPrefs.SetFloat("Music Volume", music.value);
    }

    public void sfxVol()
    {
        SoundManager.instance.efxSource.volume = sfx.value;
        PlayerPrefs.SetFloat("SFX Volume", sfx.value);
    }
}
