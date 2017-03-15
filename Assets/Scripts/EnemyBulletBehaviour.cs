using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletBehaviour : MonoBehaviour
{
    public float m_speed;
    public float m_fireRate;
    private GameObject m_target;

    private float m_direction = 1.0f;
    private SpriteRenderer m_spriteRenderer;

    private Vector2 direction;
    private float m_xLimit = .9f;
    private float m_yLimit = .5f;

    void Start()
    {
        /* if (GetComponent<PlayAudioEffect>() != null)
             GetComponent<PlayAudioEffect>().playAudio();*/
        m_spriteRenderer = GetComponent<SpriteRenderer>();
        m_target = GameObject.Find("Ikari");
        if (m_spriteRenderer.flipX)
            m_direction = -1.0f;
        else
            m_direction = 1.0f;

        Vector2 target = new Vector2(m_target.transform.position.x, m_target.transform.position.y);
        Vector2 myPos = new Vector2(transform.position.x, transform.position.y);
        direction = target - myPos;
        direction.Normalize();
        //if (direction.x > m_xLimit)
        //    direction.x = m_xLimit;
        //else if (direction.x < -m_xLimit)
        //    direction.x = -m_xLimit;
        //if (direction.y > m_yLimit)
        //    direction.y = m_yLimit;
        //else if (direction.y < -m_yLimit)
        //    direction.y = -m_yLimit;
        //lägg dessa variabler i en ny vector2 och använd den.
        Quaternion rotation = Quaternion.Euler(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);
        transform.rotation = rotation;
    }

    void Update()
    {
        //(GameObject.Find("Ikari").transform.position - transform.position).normalized;
        //transform.Translate(Vector3.right * (m_speed * m_direction) * Time.deltaTime);
        GetComponent<Rigidbody2D>().velocity = direction * m_speed;
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            m_spriteRenderer.flipX = false;
            Destroy(gameObject);
        }
    }

    public void FlipObject(bool p_toLeft)
    {
        //Debug.Log(m_direction, gameObject);
        GetComponent<SpriteRenderer>().flipX = p_toLeft;
    }
}
