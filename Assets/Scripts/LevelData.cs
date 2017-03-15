using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[System.Serializable]
public class Wave
{
    public string Name;
    public SubWave[] Subwaves;
    [HideInInspector]
    public bool m_current = false;
    [HideInInspector]
    public bool m_cleared = false;
}

[System.Serializable]
public class SubWave
{
    public string Name;
    public WaveObject[] Enemies;
    [HideInInspector]
    public bool m_cleared = false;
}

[System.Serializable]
public class WaveObject
{
    //public string Type;
    public GameObject Object;
    public Vector2 Location;
}

public class LevelData : MonoBehaviour
{
    public Wave[] Waves;
    public float m_SecondsBetweenSubwaves;
    public Text m_scoreDisplay;
    public Text m_finalScoreDisplay;
    public Text m_highScoreDisplay;
    public Slider m_multiplicationTimer;
    [HideInInspector]
    public Wave currentWave;
    private List<GameObject> m_activeCollectibles = new List<GameObject>();
    private int currentWaveIndex = 0;
    private int currentSubWaveIndex = 0;

    private int m_gameScore = 0;
    private int m_displayedScore = 0;
    private int m_scoreMultiplier = 1;
    private int m_killStreak = 0;
    private static int m_highScore = 0;
    private float m_scoreMultiplierDuration;
    private const int SCOREROLLSPEED = 10;
    private const float MULTIPLICATIONDURATION = 5f;

    [HideInInspector]
    public int m_killCount = 0;
    public static bool m_spirit = false;
    public static bool m_fighting = false;

    private const int m_distance = 100;
    private int m_distanceLeft;

    public GameObject[] m_collectibles;

    private void Start()
    {
        AssignDistance();
        ResetValues();
        m_scoreMultiplierDuration = MULTIPLICATIONDURATION;
    }

    void Update()
    {
        if (!m_fighting)
            Travel();
        else
            Fight();
        //DebugLog();
        if (m_displayedScore < m_gameScore)
            m_displayedScore += SCOREROLLSPEED;
        if (m_displayedScore > m_gameScore)
            m_displayedScore = m_gameScore;
        m_scoreDisplay.text = m_displayedScore.ToString();

        if (m_killStreak > 1)
            m_scoreMultiplier = m_killStreak;
        if (m_multiplicationTimer.gameObject.activeSelf)
        {
            m_multiplicationTimer.GetComponentInChildren<Text>().text = "x" + m_scoreMultiplier;
            m_multiplicationTimer.value = m_scoreMultiplierDuration;
            m_scoreMultiplierDuration -= Time.deltaTime;
            if (m_scoreMultiplierDuration <= 0)
                BreakStreak();
        }
    }

    void Travel()
    {
        if (GameObject.Find("Ikari").GetComponent<PlayerScript>().m_travelling && GameObject.Find("Ikari").GetComponent<PlayerScript>().m_proceeding)
            m_distanceLeft--;
        if (m_distanceLeft < 0)
            m_distanceLeft = 0;
        if (m_distanceLeft == 0)
        {
            m_fighting = true;
            PrepareFight();
        }
        //Debug.Log(m_distanceLeft);
    }

    void Fight()
    {
        //Debug.Log("current: " + currentWave.m_current, gameObject);
        if (!currentWave.m_current)
            StartCoroutine(startWave(currentWave));

        //Debug.Log("Fighting: " + m_fighting, gameObject);
        if (!currentWave.m_current)
            return;
        if (m_killCount >= currentWave.Subwaves[currentSubWaveIndex].Enemies.Length)
        {
            currentWave.Subwaves[currentSubWaveIndex].m_cleared = true;
            m_killCount = 0;
        }
    }

    public void CollectibleSpawn(float p_x, float p_y)
    {
        if (Random.Range(0, 2) == 1)
        {
            Quaternion spawnRotation = Quaternion.identity;
            int m_randomNumber = Random.Range(0, 4);
            //Debug.Log(m_randomNumber);
            m_activeCollectibles.Add(Instantiate(m_collectibles[m_randomNumber], new Vector3(p_x, p_y), spawnRotation));
        }

    }

    IEnumerator startWave(Wave p_wave)
    {
        p_wave.m_current = true;
        for (int i = 0; i < p_wave.Subwaves.Length; i++)
        {
            currentSubWaveIndex = i;
            for (int j = 0; j < p_wave.Subwaves[i].Enemies.Length; j++)
            {
                Quaternion spawnRotation = Quaternion.identity;
                Instantiate(p_wave.Subwaves[i].Enemies[j].Object, p_wave.Subwaves[i].Enemies[j].Location, spawnRotation);
            }
            yield return new WaitUntil(new System.Func<bool>(() => p_wave.Subwaves[i].m_cleared));
            yield return new WaitForSeconds(m_SecondsBetweenSubwaves);
        }
        currentWave.m_current = false;
        m_fighting = false;
        for (int i = 0; i < p_wave.Subwaves.Length; i++)
        {
            p_wave.Subwaves[i].m_cleared = false;
        }
        for (int i = 0; i < m_activeCollectibles.Count; i++)
        {
            Destroy(m_activeCollectibles[i]);
        }
        AssignDistance();
        StopCoroutine("startWave");
    }

    void PrepareFight()
    {
        currentWave = Waves[currentWaveIndex];
        //Debug.Log(currentSubWaveIndex);
        currentWaveIndex++;
        if (currentWaveIndex > (Waves.Length - 1))
            currentWaveIndex = 1;
    }

    void AssignDistance()
    {
        m_distanceLeft = m_distance;
    }

    public void onKill(int p_score, float p_x, float p_y)
    {
        m_killCount++;
        if(m_killStreak < 9)
            m_killStreak++;
        if (m_killStreak > 1)
            m_multiplicationTimer.gameObject.SetActive(true);
        m_scoreMultiplierDuration = MULTIPLICATIONDURATION;
        AddScore(p_score);
        CollectibleSpawn(p_x,p_y);
    }

    public void BreakStreak()
    {
        m_killStreak = 0;
        m_scoreMultiplier = 1;
        m_multiplicationTimer.gameObject.SetActive(false);
    }

    public void AddScore(int p_value)
    {
        m_gameScore += (p_value * m_scoreMultiplier);
        m_scoreDisplay.text = m_displayedScore.ToString();
    }

    public void startLevel(int nr)
    {
        Application.LoadLevel(nr);       //Load Level 0 (same Level) to make a restart
    }

    public void restartGame()
    {
        Application.LoadLevel(Application.loadedLevel);       //Load Level 0 (same Level) to make a restart
    }

    public void GameOver()
    {
        m_finalScoreDisplay.text = m_gameScore.ToString();
        if(m_gameScore > m_highScore)
        {
            m_highScore = m_gameScore;
            m_highScoreDisplay.text = m_gameScore.ToString();
        }
        ResetValues();
    }

    public void ResetValues()
    {
        //currentWave = null;
        m_fighting = false;
        m_spirit = false;
        m_killCount = 0;
        for (int i = 0; i < Waves.Length; i++)
        {
            for (int j = 0; j < Waves[i].Subwaves.Length; j++)
            {
                Waves[i].Subwaves[j].m_cleared = false;
            }
        }
        for (int i = 0; i < m_activeCollectibles.Count; i++)
        {
            Destroy(m_activeCollectibles[i]);
        }
        StopAllCoroutines();
    }
    public void DebugLog()
    {
        Debug.Log("Fighting: " + m_fighting);
        Debug.Log("In Spirit Realm: " + m_spirit);
        Debug.Log("Killcount: " + m_killCount);
    }
}
