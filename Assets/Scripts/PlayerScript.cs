using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class PlayerScript : MonoBehaviour
{
    public GameObject m_weapon;
    public GameObject m_melee;  
    public Transform m_weaponSpawn;
    public GameObject TripleShotDisplay;
    public Slider InvincibilityTimer;
    public Slider MultiDimensionalTimer;
    public float m_speed;
    public float m_focusDuration;
    public float m_focusSlowdown;
    public float m_switchCooldownDuration;
    public AudioClip hurt1;
    public AudioClip hurt2;
    public AudioClip hurt3;
    public AudioSource sfxKunaiThrow;

    Animator playerAnimator;

    [HideInInspector]
    public PlayerHealth playerHealth;
    [HideInInspector]
    public bool m_travelling = false;
    [HideInInspector]
    public bool m_proceeding = false;

    private Rigidbody2D m_rigidBody;
    private SpriteRenderer m_spriteRenderer;

    private AudioClip[] takeDamage;

    private float m_currentSpeed;

    private float m_minX = -8.5f; //-8.5f
    [HideInInspector]
    public float m_maxX = 2.0f;
    private float m_minY = -4.5f;
    private float m_maxY = 0;

    private float m_focusDurationCount = 0.0f;
    private float m_switchCooldownCount = 0.0f;
    private float m_weaponCooldownCount = 0.0f;
    private float m_tripleShotGap = 7.5f;

    private bool m_invincible = false;
    [HideInInspector]
    public bool m_multidimensional = false;
    private bool m_tripleShot = false;
    private int m_tripleShotAmmunition = 0;
    private float m_tripleShotDuration = 3.0f;
    private float m_invincibilityDuration = 3.0f;
    private float m_multidimensionalDuration = 3.0f;


    void Start()
    {
        //m_spriteRenderer = GetComponent<SpriteRenderer>();
        m_melee = transform.FindChild("MeleeObject").gameObject;
        m_melee.SetActive(false);
        playerHealth = GetComponent<PlayerHealth>();
        playerAnimator = GetComponent<Animator>();
        takeDamage = new AudioClip[3];
        takeDamage[0] = hurt1;
        takeDamage[1] = hurt2;
        takeDamage[2] = hurt3;
        sfxKunaiThrow.volume = PlayerPrefs.GetFloat("SFX Volume");
    }

    void Update()
    {
        //Debug.Log(m_currentSpeed, gameObject);
        //Debug.Log(transform.FindChild("WeaponSpawn").transform.localPosition);
        m_proceeding = false;
        if (LevelData.m_fighting)
            m_maxX = 8.5f;
        else
            m_maxX = 2.0f;

        if (Time.time > m_focusDurationCount)
            m_currentSpeed = m_speed;
        else
            m_currentSpeed = m_speed * m_focusSlowdown;
        if(GetComponent<PlayerHealth>().isDead || GameObject.Find("GameData").GetComponent<WinEvent>().m_won)
            return;

        playerAnimator.SetBool("Walk", false);
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A))
            playerAnimator.SetBool("Walk", true);

        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(Vector3.up * m_currentSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(Vector3.down * m_currentSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D))
        {
            if (transform.position.x <= m_maxX)
            {
                transform.Translate(Vector3.right * m_currentSpeed * Time.deltaTime);
            }
            m_proceeding = true;
            //if (m_spriteRenderer.flipX && Time.time > m_focusDurationCount)
            //{
            //    m_spriteRenderer.flipX = false;
            //    m_weapon.GetComponent<BulletObjectBehaviour>().FlipObject(false);
            //    FlipChildPrefabs();
            //}
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(Vector3.left * m_currentSpeed * Time.deltaTime);
            //if (!m_spriteRenderer.flipX && Time.time > m_focusDurationCount)
            //{
            //    m_spriteRenderer.flipX = true;
            //    m_weapon.GetComponent<BulletObjectBehaviour>().FlipObject(true);
            //    m_weapon.GetComponent<SpriteRenderer>().flipX = true;
            //    FlipChildPrefabs();
            //}
        }

        if (transform.position.y < m_minY) { transform.position = new Vector3(transform.position.x, m_minY, 0); }
        if (transform.position.y > m_maxY) { transform.position = new Vector3(transform.position.x, m_maxY, 0); }
        if (transform.position.x < m_minX) { transform.position = new Vector3(m_minX, transform.position.y, 0); }
        //if(!LevelData.m_fighting)
        //if (transform.position.x > m_maxX) { transform.position = new Vector3(m_maxX, transform.position.y, 0); }
        if (transform.position.x > m_maxX)
        {
            if (!LevelData.m_fighting)
                m_travelling = true;
            transform.Translate(Vector3.left * (m_currentSpeed*1.5f) * Time.deltaTime);
            if(transform.position.x < m_maxX)
                transform.position = new Vector3(m_maxX, transform.position.y, 0);
            //transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        }
        else
        {
            m_travelling = false;
        }
        

        Vector3 mousePositions = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
        if (mousePositions.x > transform.position.x)
        {
            if (transform.localScale.x < 0)
            {
                transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y);
                //FlipChildPrefabs();
            }
        }
        else
        {
            if (transform.localScale.x > 0)
            {
                transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y);
                //FlipChildPrefabs();
            }
        }

        if (Input.GetKey(KeyCode.Z) || Input.GetKey(KeyCode.Mouse0))
        {
            if (Time.time > m_weaponCooldownCount && !m_melee.GetComponent<MeleeBehaviourScript>().m_meleeing)
            {
                m_weaponCooldownCount = Time.time + m_weapon.GetComponent<BulletObjectBehaviour>().m_fireRate;   //Increment nextFire time with the current system time + fireRate
                m_focusDurationCount = Time.time + m_focusDuration;

                Vector2 target = Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
                Vector2 myPos = new Vector2(transform.position.x, transform.position.y);
                Vector2 direction = target - myPos;
                direction.Normalize();
                Quaternion rotation = Quaternion.Euler(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);
                m_weaponSpawn.transform.rotation = rotation;
                Instantiate(m_weapon, m_weaponSpawn.position, m_weaponSpawn.rotation);
                playerAnimator.SetTrigger("RangedAttack");
                sfxKunaiThrow.Play();
                if (m_tripleShotAmmunition > 0)
                {
                    rotation = Quaternion.Euler(0, 0, Mathf.Atan2(direction.y, direction.x) * (Mathf.Rad2Deg) + 10.0f);
                    m_weaponSpawn.transform.rotation = rotation;
                    Instantiate(m_weapon, m_weaponSpawn.position, m_weaponSpawn.rotation);
                    rotation = Quaternion.Euler(0, 0, Mathf.Atan2(direction.y, direction.x) * (Mathf.Rad2Deg) - 10.0f);
                    m_weaponSpawn.transform.rotation = rotation;
                    Instantiate(m_weapon, m_weaponSpawn.position, m_weaponSpawn.rotation);

                    m_tripleShotAmmunition--;

                    /*m_weaponSpawn.Rotate(0.0f, 0.0f, m_tripleShotGap);
                    Instantiate(m_weapon, m_weaponSpawn.position, m_weaponSpawn.rotation);
                    m_weaponSpawn.Rotate(0.0f, 0.0f, -m_tripleShotGap);
                    Instantiate(m_weapon, m_weaponSpawn.position, m_weaponSpawn.rotation);
                    m_weaponSpawn.Rotate(0.0f, 0.0f, -m_tripleShotGap);
                    Instantiate(m_weapon, m_weaponSpawn.position, m_weaponSpawn.rotation);
                    m_weaponSpawn.Rotate(0.0f, 0.0f, +m_tripleShotGap);*/
                }
                //else
                    //Instantiate(m_weapon, m_weaponSpawn.position, m_weaponSpawn.rotation);  //Instantiate fire shot
                //  GetComponent<AudioSource>().Play();                                                     //Play Fire sound
            }
        }
        if (Input.GetKey(KeyCode.X) || Input.GetKey(KeyCode.Mouse1))
        {
            m_melee.GetComponent<MeleeBehaviourScript>().Attack();
            playerAnimator.SetTrigger("Attack");
        }

        if (Time.time > m_switchCooldownCount && !gameObject.GetComponent<PlayerHealth>().isDead)
        {
            if (Input.GetKey(KeyCode.R) || Input.GetKey(KeyCode.Space))
            {
                if (LevelData.m_spirit)
                    LevelData.m_spirit = false;
                else
                    LevelData.m_spirit = true;
                m_switchCooldownCount = Time.time + m_switchCooldownDuration;
            }
        }
        if (m_tripleShotAmmunition > 0)
        {
            if (!TripleShotDisplay.activeSelf)
                TripleShotDisplay.SetActive(true);
            TripleShotDisplay.GetComponentInChildren<Text>().text = "x " + m_tripleShotAmmunition.ToString();
           /* TripleShotTimer.gameObject.SetActive(true);
            TripleShotTimer.value = m_tripleShotDuration;
            m_tripleShotDuration -= Time.deltaTime;
            if (m_tripleShotDuration <= 0)
            {
                m_tripleShot = false;
                TripleShotTimer.gameObject.SetActive(false);
                m_tripleShotDuration = 3.0f;
            }*/
        }
        else
        {
            if (TripleShotDisplay.activeSelf)
                TripleShotDisplay.SetActive(false);
        }
        if (m_invincible)
        {
            InvincibilityTimer.gameObject.SetActive(true);
            InvincibilityTimer.value = m_invincibilityDuration;
            m_invincibilityDuration -= Time.deltaTime;
            if (m_invincibilityDuration <= 0)
            {
                m_invincible = false;
                InvincibilityTimer.gameObject.SetActive(false);
                m_invincibilityDuration = 3.0f;
            }

        }
        if (m_multidimensional)
        {
            MultiDimensionalTimer.gameObject.SetActive(true);
            MultiDimensionalTimer.value = m_multidimensionalDuration;
            m_multidimensionalDuration -= Time.deltaTime;
            if (m_multidimensionalDuration <= 0)
            {
                m_multidimensional = false;
                MultiDimensionalTimer.gameObject.SetActive(false);
                m_multidimensionalDuration = 3.0f;
            }

        }
        /*if (m_invincible)
        {
            Canvas.gameObject.SetActive(true);
            powerupTimer.value = m_tripleShotDuration;
            m_tripleShotDuration -= Time.deltaTime;
            if (m_tripleShotDuration <= 0)
            {
                m_tripleShot = false;
                Canvas.gameObject.SetActive(false);
                m_tripleShotDuration = 3.0f;
            }

        }*/
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "EnemyBullet" && !m_invincible) //|| other.gameObject.tag )
        {
            
            //Instantiate(some particles, transform.position, transform.rotation);                 //Instantiate Particles
            //SharedValues_Script.gameover = true;
            playerHealth.TakeDamage(20);
            SoundManager.instance.RandomizeSfx(takeDamage);
            //Destroy(gameObject);
        }
        else if (other.gameObject.tag == "TripleShot")
        {
            //Instantiate(some particles, transform.position, transform.rotation);                 //Instantiate Particles
            //SharedValues_Script.gameover = true;
            Destroy(other.gameObject);
            //if(!m_tripleShot)
            //    m_tripleShot = true;
            m_tripleShotAmmunition += 10;
            playerHealth.PowerupEffect();
        }
        else if (other.gameObject.tag == "Invincibility")
        {
            //Instantiate(some particles, transform.position, transform.rotation);                 //Instantiate Particles
            //SharedValues_Script.gameover = true;
            Destroy(other.gameObject);
            if (m_invincible)
                m_invincibilityDuration = 3.0f;
            m_invincible = true;
            playerHealth.PowerupEffect();
        }
        else if (other.gameObject.tag == "Multidimensional")
        {
            //Instantiate(some particles, transform.position, transform.rotation);                 //Instantiate Particles
            //SharedValues_Script.gameover = true;
            Destroy(other.gameObject);
            if (m_multidimensional)
                m_multidimensionalDuration = 3.0f;
            m_multidimensional = true;
            playerHealth.PowerupEffect();
        }
        else if (other.gameObject.tag == "Health")
        {
            //Instantiate(some particles, transform.position, transform.rotation);                 //Instantiate Particles
            //SharedValues_Script.gameover = true;
            playerHealth.RestoreDamage(20);
            Destroy(other.gameObject);
        }
    }

    void FlipChildPrefabs()
    {
        m_melee.transform.localPosition = new Vector3(m_melee.transform.localPosition.x * -1.0f, m_melee.transform.localPosition.y);
        m_weaponSpawn.transform.localPosition = new Vector3(m_weaponSpawn.transform.localPosition.x * -1.0f, m_weaponSpawn.transform.localPosition.y);
    }
    void OnApplicationQuit()
    {
        m_weapon.GetComponent<SpriteRenderer>().flipX = false;
    }

    /*void playerTakeDamage(float damage)
    {
        playerHealth -= damage;
        playerHealthBar.value -= damage;
    }*/
}