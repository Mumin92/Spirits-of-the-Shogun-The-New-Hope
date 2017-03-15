using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseGame : MonoBehaviour {

    public Transform Canvas;

	void Update () {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Pause();
        }
	}

    public void Pause()
    {
            if (Canvas.gameObject.activeInHierarchy == false)
            {
                Canvas.gameObject.SetActive(true);
                Time.timeScale = 0;
            }
            else
            {
                Canvas.gameObject.SetActive(false);
                Time.timeScale = 1;
            }
    }
}
