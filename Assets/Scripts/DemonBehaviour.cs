using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonBehaviour : MonoBehaviour
{
    private GameObject m_target;
    private GameObject m_melee;
    private SpriteRenderer m_spriteRenderer;
    private bool m_dying = false;

    private float m_horizontalDestination;
    private float m_verticalDestination;

    private float m_horizontalChange;
    private float m_verticalChange;

    public AudioSource sfxDeathScream;
    public float m_health;
    public float m_speed;
    private const float m_offset = 1.5f;
    private const float m_destinationRange = .75f;

    Animator animator;

    void Start()
    {
        m_target = GameObject.Find("Ikari");
        m_melee = transform.FindChild("MeleeObject").gameObject;
        //m_spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        sfxDeathScream.volume = PlayerPrefs.GetFloat("SFX Volume");
    }

    void Update()
    {
        if (m_dying)
            return;
        //Debug.Log(m_horizontalDestination, gameObject);
        //Debug.Log("Horizontal Change: "+m_horizontalChange+" Vertical Change: "+m_verticalChange, gameObject);
        if (transform.FindChild("MeleeObject").GetComponent<MeleeBehaviourScript>().m_meleeing)
            return;
        m_horizontalDestination = m_target.transform.position.x;
        m_verticalDestination = m_target.transform.position.y;

        if (transform.position.x > m_horizontalDestination + (m_destinationRange + m_offset))
            m_horizontalChange = -1.0f;
        else if (transform.position.x < m_horizontalDestination - (m_destinationRange + m_offset))
            m_horizontalChange = 1.0f;
        else
            m_horizontalChange = 0.0f;

        if (transform.position.y > m_verticalDestination + m_destinationRange)
            m_verticalChange = -1.0f;
        else if (transform.position.y < m_verticalDestination - m_destinationRange)
            m_verticalChange = 1.0f;
        else
            m_verticalChange = 0.0f;
        //if(transform.position.y - m_verticalDestination <= 1)

        transform.Translate(new Vector3(m_horizontalChange, m_verticalChange) * m_speed * Time.deltaTime);

        if (m_target.transform.position.x < transform.position.x)
        {
            if (transform.localScale.x < 0)
            {
                transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y);
                FlipChildPrefabs();
            }
        }
        else
        {
            if (transform.localScale.x > 0)
            {
                transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y);
                FlipChildPrefabs();
            }

        }

        if (m_target.transform.position.x > transform.position.x - (m_destinationRange + m_offset) && m_target.transform.position.x < transform.position.x + (m_destinationRange + m_offset))
        {
            transform.FindChild("MeleeObject").GetComponent<MeleeBehaviourScript>().Attack();
            if (animator != null)
                animator.SetBool("Attack", true);
        }
        if (transform.position.y > 0f)
            transform.position = new Vector3(transform.position.x, 0);
        //animator.SetBool("Attack", false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (m_dying)
            return;
        if (other.gameObject.tag == "Weapon")
        {
            if (GetComponent<SpiritProperty>().m_spirit == other.gameObject.GetComponent<SpiritProperty>().m_spirit || other.gameObject.GetComponent<BulletObjectBehaviour>().m_multidimensional)
            {
                if (m_health > 0)
                    m_health--;
                if (m_health <= 0)
                {

                    StartCoroutine(Death());
                    m_dying = true;
                }
            }
            else
                Physics2D.IgnoreCollision(other, GetComponent<Collider2D>());
        }
    }
    void FlipChildPrefabs()
    {
        //m_melee.transform.localPosition = new Vector3(m_melee.transform.localPosition.x * -1.0f, m_melee.transform.localPosition.y);
    }

    IEnumerator Death()
    {
        if (animator != null)
            animator.SetBool("Walk", false);
        if (animator != null)
            animator.SetBool("Death", true);
        sfxDeathScream.Play();
        yield return new WaitForSecondsRealtime(2.4f);
        Destroy(gameObject);
        GameObject.Find("GameData").GetComponent<LevelData>().onKill(gameObject.GetComponent<ScoreProperty>().m_scoreValue, transform.position.x, transform.position.y);
    }
}
