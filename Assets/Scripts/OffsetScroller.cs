using UnityEngine;
using System.Collections;

public class OffsetScroller : MonoBehaviour
{
    public GameObject player;

    public float scrollSpeed;
    private Vector2 savedOffset;

    private float relativeTime; //At least better to use than Time.time

    void Start()
    {
        savedOffset = GetComponent<Renderer>().sharedMaterial.GetTextureOffset("_MainTex");
        relativeTime = Time.time;
        MoveTexture();
    }

    void Update()
    {
        if (player.transform.position.x >= player.GetComponent<PlayerScript>().m_maxX && !LevelData.m_fighting)
        {
            if (player.GetComponent<PlayerScript>().m_travelling)
            {
                MoveTexture();
                relativeTime += .025f; //Adjustable and important for scroll speed
            }
            if (player.transform.position.x > player.GetComponent<PlayerScript>().m_maxX)
                relativeTime += .05f;
        }
    }

    void OnDisable()
    {
        GetComponent<Renderer>().sharedMaterial.SetTextureOffset("_MainTex", savedOffset);
    }
    void MoveTexture()
    {
        float x = Mathf.Repeat(relativeTime * scrollSpeed, 1);
        Vector2 offset = new Vector2(x, savedOffset.y);
        GetComponent<Renderer>().sharedMaterial.SetTextureOffset("_MainTex", offset);
    }
}