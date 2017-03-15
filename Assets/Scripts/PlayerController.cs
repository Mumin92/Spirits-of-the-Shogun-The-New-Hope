using UnityEngine;

[System.Serializable]
public class PlayerController : MonoBehaviour
{
    public float m_speed;
    public GameObject m_weapon;
    public Transform m_weaponSpawn;
    public float maxY;

    private Rigidbody2D m_rigidBody;
    private SpriteRenderer m_spriteRenderer;

    private float minX = -8.5f;
    private float maxX = 8.5f;
    private float minY = -4.5f;

    private float m_weaponCooldown = 0.0f;
    // Use this for initialization
    void Start()
    {
        m_rigidBody = GetComponent<Rigidbody2D>();
        m_spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.UpArrow))
            transform.Translate(Vector3.up * m_speed * Time.deltaTime);
        else if (Input.GetKey(KeyCode.DownArrow))
            transform.Translate(Vector3.down * m_speed * Time.deltaTime);
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Translate(Vector3.right * m_speed * Time.deltaTime);
            m_spriteRenderer.flipX = false;
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Translate(Vector3.left * m_speed * Time.deltaTime);
            m_spriteRenderer.flipX = true;
        }
        if (transform.position.x < minX) { transform.position = new Vector3(minX, transform.position.y, 0); }
        if (transform.position.x > maxX) { transform.position = new Vector3(maxX, transform.position.y, 0); }

        if (transform.position.y < minY) { transform.position = new Vector3(transform.position.x, minY, 0); }
        if (transform.position.y > maxY) { transform.position = new Vector3(transform.position.x, maxY, 0); }

        if (Input.GetKey(KeyCode.Z) || Input.GetKeyDown(KeyCode.Z))
        {
            if (Time.time > m_weaponCooldown)
            {
               m_weaponCooldown = Time.time + m_weapon.GetComponent<WeaponBehaviour>().m_fireRate;                                //Increment nextFire time with the current system time + fireRate
                Instantiate(m_weapon, m_weaponSpawn.position, m_weaponSpawn.rotation);  //Instantiate fire shot 
                //  GetComponent<AudioSource>().Play();                                                     //Play Fire sound
            }
        }
    }
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            //Instantiate(some particles, transform.position, transform.rotation);                 //Instantiate Particles
            //SharedValues_Script.gameover = true;
            Destroy(gameObject);
        }
    }
}