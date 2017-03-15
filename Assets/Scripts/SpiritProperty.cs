using UnityEngine;
using System.Collections;

public class SpiritProperty : MonoBehaviour
{
    public bool m_IsEnemy;
    public bool m_spirit;

    void Start()
    {
        if(!m_IsEnemy)
            m_spirit = LevelData.m_spirit;
    }

    public void setMode(bool p_val)
    {
        m_spirit = p_val;
    }
    public bool getMode()
    {
        return m_spirit;
    }
}