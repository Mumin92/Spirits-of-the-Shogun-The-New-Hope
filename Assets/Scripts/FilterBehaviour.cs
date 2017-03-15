using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FilterBehaviour : MonoBehaviour
{
    public Transform Canvas;
	
	void Update ()
    {
        if (LevelData.m_spirit)
        {
            Canvas.gameObject.SetActive(true);
        }
        else
        {
            Canvas.gameObject.SetActive(false);
        }
    }
}
