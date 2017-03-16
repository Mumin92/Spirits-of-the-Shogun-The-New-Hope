using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    private float m_shakeDuration;
    private float m_shakeIntensity = 0.025f;

	void Update () {
        if (m_shakeDuration > 0)
        {
            m_shakeDuration -= Time.deltaTime;
            if (transform.position.x > -m_shakeIntensity)
                transform.position = new Vector2( -m_shakeIntensity, 0);
            else if(transform.position.x < m_shakeIntensity)
                transform.position = new Vector2(m_shakeIntensity, 0);
        }
        else
            transform.position = Vector2.zero;
	}

    public void ShakeScreen()
    {
        m_shakeDuration = .25f;
    }
}
