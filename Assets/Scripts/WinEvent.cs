using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinEvent : MonoBehaviour
{
    public Image m_winImage;
    public Canvas m_winCanvas;

    public Text m_finalScoreDisplay;
    public Text m_highScoreDisplay;

    [HideInInspector]
    public bool m_won = false;

    private Color m_whiteColor = Color.clear;

    private void Start()
    {
        //m_winImage.color
    }

	void Update ()
    {
		if(m_won)
        {
            LevelData.m_spirit = false;
            GameObject.Find("Ikari").transform.Translate(Vector3.right * 3f * Time.deltaTime);
            GameObject.Find("Ikari").GetComponent<Animator>().SetBool("Walk", true);
            if (GameObject.Find("Ikari").transform.localScale.x < 0)
                GameObject.Find("Ikari").transform.localScale = new Vector3(GameObject.Find("Ikari").transform.localScale.x * -1, GameObject.Find("Ikari").transform.localScale.y);
            m_winImage.color = Color.Lerp(m_winImage.GetComponent<Image>().color, Color.white, 2.5f * Time.deltaTime);
        }

        if(m_winImage.color == Color.white)
        {
            m_winCanvas.gameObject.SetActive(true);
            m_finalScoreDisplay.text = GameObject.Find("GameData").GetComponent<LevelData>().getFinalScore().ToString();
            if (GameObject.Find("GameData").GetComponent<LevelData>().getFinalScore() > GameObject.Find("GameData").GetComponent<LevelData>().getHighScore())
                GameObject.Find("GameData").GetComponent<LevelData>().setHighScore(GameObject.Find("GameData").GetComponent<LevelData>().getFinalScore());
            m_highScoreDisplay.text = GameObject.Find("GameData").GetComponent<LevelData>().getFinalScore().ToString();
        }
	}
}
