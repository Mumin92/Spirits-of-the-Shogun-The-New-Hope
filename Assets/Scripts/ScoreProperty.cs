using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreProperty : MonoBehaviour
{
    public int m_scoreValue;

    public void setScoreValue(int p_val)
    {
        m_scoreValue = p_val;
    }
    public int getScoreValue()
    {
        return m_scoreValue;
    }
}
