using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemyBehaviour : MonoBehaviour
{
    public float m_health;
    public float m_speed;
    // Use this for initialization
    void Start()
    {
        //GetComponent<Rigidbody2D>().velocity = -1 * transform.right * m_speed; //Give Velocity to the Player ship shot
    }

    // Update is called once per frame
    void Update()
    {
       //transform.Translate(Vector3.left * m_speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Weapon")
        {
            if (GetComponent<SpiritProperty>().m_spirit == other.gameObject.GetComponent<SpiritProperty>().m_spirit)
            {
                if (m_health > 0)
                    m_health--;
                if (m_health <= 0)
                {
                    Destroy(gameObject);
                    //LevelData.m_killCount++;
                }
            }
            else
                Physics2D.IgnoreCollision(other, GetComponent<Collider2D>());
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        //if (other.gameObject.tag == "Enemy")
        //{
        //    if (GetComponent<SpiritProperty>().m_spirit != other.gameObject.GetComponent<SpiritProperty>().m_spirit)
        //    {
        //        Physics2D.IgnoreCollision(other, GetComponent<Collider2D>());
        //    }
        //}
    }
}