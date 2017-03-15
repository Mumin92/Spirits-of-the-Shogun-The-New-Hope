using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeBehaviourScript : MonoBehaviour
{
    public float m_maxDuration;
    public float m_cooldown;
    public bool m_threat;

    public bool m_meleeing = false;
    private float m_durationCount = 0.0f;
    private float m_cooldownCount = 0.0f;

    void Start()
    {
        if (m_threat)
            gameObject.tag = "EnemyBullet";
        /* if (GetComponent<PlayAudioEffect>() != null)
             GetComponent<PlayAudioEffect>().playAudio();*/
    }
    void Update ()
    {
        if (m_meleeing)
        {
            if (Time.time >= m_durationCount)
            {
                gameObject.SetActive(false);
                m_meleeing = false;
                m_cooldownCount = Time.time + m_cooldown;
            }
        }
    }

    public void Attack()
    {
        if (!m_meleeing)
        {
            if (Time.time >= m_cooldownCount)
            {
                gameObject.SetActive(true);
                gameObject.GetComponent<SpiritProperty>().m_spirit = LevelData.m_spirit;
                m_durationCount = Time.time + m_maxDuration;
                m_meleeing = true;
            }
        }
    }
}