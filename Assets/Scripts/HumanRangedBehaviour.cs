using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanRangedBehaviour : MonoBehaviour
{
    public GameObject m_weapon;
    public Transform m_weaponSpawn;
    public float m_attackWindup;
    public float m_actionDelay;
    public float m_health;
    public float m_speed;
    public AudioSource sfxRifleShot;
    public AudioSource sfxDeathScream;

    private GameObject m_target;

    private float m_horizontalDestination;
    private float m_verticalDestination;

    private float m_horizontalChange;
    private float m_verticalChange;

    public float deathDelay;
    private float m_minimumRange;
    private float m_attackWindupCount = 0.0f;
    private float m_actionDelayCount = 0.0f;
    private float m_weaponCooldownCount = 0.0f;
    private const float m_offset = 1.5f;
    private const float m_destinationRange = 0.5f;
    private const float m_destinationSpot = 8.0f;

    private bool m_dying = false;
    private bool m_windingUp = false;
    private string m_currentState;
    private const string ATTACKSTATE = "Attack";
    private const string MOVESTATE = "Move";

    Animator animator;

    /*
     * void update() {
     *      räknar upp en counter och när den är förbi gränsen så switchar fienden state
     * }
     * void moveToDestination {
     *      rör spelaren till destinationen och när den är framme så byter den state
     * }
     * void fireAtPlace {
     *      skjuter tills den bestämmer sig för att skjuta från en ny destination.
     * }
     */

    void Start()
    {
        //Random.InitState(Random.seed);
        //m_horizontalDestination = 5.0f + m_offset;
        m_target = GameObject.Find("Ikari");
        m_currentState = MOVESTATE;
        animator = GetComponent<Animator>();
        m_verticalDestination = transform.position.y;
        if (transform.position.x >= 0.0f)
            m_horizontalDestination = m_destinationSpot;
        else if (transform.position.x <= 0.0f)
            m_horizontalDestination = -m_destinationSpot;
        sfxDeathScream.volume = PlayerPrefs.GetFloat("SFX Volume");
        sfxRifleShot.volume = PlayerPrefs.GetFloat("SFX Volume");
    }

    void Update()
    {
        if (m_dying)
            return;
        //Debug.Log(m_horizontalDestination, gameObject);
        //Debug.Log("Horizontal Change: "+m_horizontalChange+" Vertical Change: "+m_verticalChange, gameObject);
        if (m_currentState == MOVESTATE)
            Move();
        else if (m_currentState == ATTACKSTATE)
            Attack();

        /*if (m_target.transform.position.x > transform.position.x - 10.0f && m_target.transform.position.x < transform.position.x + 10.0f)
        //if(transform.position.x < m_target.transform.position.x - 2.25f && transform.position.x > m_target.transform.position.x + 2.25f)
        {
            m_currentState = ATTACKSTATE;
        }
        else
            m_currentState = MOVESTATE;*/

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
        if (transform.position.y > 0f)
            transform.position = new Vector3(transform.position.x, 0);
    }
    void Move()
    {

        if (transform.position.x > m_horizontalDestination + m_destinationRange)
            m_horizontalChange = -1.0f;
        else if (transform.position.x < m_horizontalDestination - m_destinationRange)
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
        if (m_verticalChange == 0.0f && m_horizontalChange == 0.0f)
        {
            m_currentState = ATTACKSTATE;
            animator.SetBool("Walk", false);
        }
    }

    void Attack()
    {
        if (m_windingUp)
        {
            if (Time.time > m_attackWindupCount)
            {
                //m_weaponCooldownCount = Time.time + m_weapon.GetComponent<BulletObjectBehaviour>().m_fireRate;
                animator.SetTrigger("Shoot");
                Instantiate(m_weapon, m_weaponSpawn.position, m_weaponSpawn.rotation);
                sfxRifleShot.Play();
                m_windingUp = false;
                m_actionDelayCount = Time.time + m_actionDelay;
            }
        }
        else
        {
            if (Time.time > m_actionDelayCount)
            {
                m_windingUp = true;
                m_attackWindupCount = Time.time + m_attackWindup;
            }
        }
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
                    if (m_dying)
                        return;
                    StartCoroutine(Death());
                    m_dying = true;
                    /*animator.SetBool("Death", true);
                    yield return new WaitForSeconds(2);
                    Destroy(gameObject);
                    LevelData.m_killCount++;
                    deathDelay -= Time.deltaTime;
                    if (deathDelay <= 0)
                    {
                        Destroy(gameObject);
                        LevelData.m_killCount++;
                    }*/
                }
            }
            else
                Physics2D.IgnoreCollision(other, GetComponent<Collider2D>());
        }
    }

    void FlipChildPrefabs()
    {
        m_weaponSpawn.transform.localPosition = new Vector3(m_weaponSpawn.transform.localPosition.x * -1.0f, m_weaponSpawn.transform.localPosition.y);
    }

    IEnumerator Death()
    {
        animator.SetBool("Death", true);
        sfxDeathScream.Play();
        yield return new WaitForSecondsRealtime(1);
        Destroy(gameObject);
        GameObject.Find("GameData").GetComponent<LevelData>().onKill(gameObject.GetComponent<ScoreProperty>().m_scoreValue, transform.position.x, transform.position.y);
    }
}
