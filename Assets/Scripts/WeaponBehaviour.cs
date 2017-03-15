using UnityEngine;
using System.Collections;

public class WeaponBehaviour : MonoBehaviour
{
    public float m_speed;
    public float m_fireRate = 0.5f;
    public GameObject m_ignoreCollisionWith;

    void Start()
    {
        //GetComponent<Rigidbody2D>().velocity = transform.right * m_speed; //Give Velocity to the Player ship shot

        /* if (GetComponent<PlayAudioEffect>() != null)
             GetComponent<PlayAudioEffect>().playAudio();*/
    }
    void Update()
    {
        transform.Translate(Vector3.right * m_speed * Time.deltaTime);
       // Physics2D.IgnoreCollision(m_ignoreCollisionWith.GetComponent<Collider2D>(), GetComponent<Collider2D>());
    }
    //void OnCollisionEnter2D(Collision2D other)
    //{
    //    if (other.gameObject.tag == "Weapon")
    //    {
    //        Physics2D.IgnoreCollision(other.gameObject.GetComponent<Collider2D>(), GetComponent<Collider2D>());
    //    }
    //}
}