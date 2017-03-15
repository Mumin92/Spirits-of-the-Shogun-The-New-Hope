using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneOnClick : MonoBehaviour {

    public void loadByIndex(int sceneIndex)
    {
        if (SoundManager.instance.MusicPlays())
        {
            SoundManager.instance.StopMusic();
        }
        else
        {
            SoundManager.instance.PlayMusic();
        }
        if (Time.timeScale != 1)
        {
            Time.timeScale = 1;
        }
        SceneManager.LoadScene(sceneIndex);
    }

}
