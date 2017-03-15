using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlPictureBehaviour : MonoBehaviour
{
    private bool m_disappearing = false;

    private void Start()
    {
        GetComponent<SpriteRenderer>().color = Color.white;
    }

	void Update ()
    {
        if (GameObject.Find("Ikari").GetComponent<PlayerScript>().m_travelling)
            m_disappearing = true;

        if(m_disappearing)
            GetComponent<SpriteRenderer>().color = Color.Lerp(GetComponent<SpriteRenderer>().color, Color.clear, 4.0f * Time.deltaTime);

        if (GetComponent<SpriteRenderer>().color.a == 0)
            DestroyObject(gameObject);
    }
}
