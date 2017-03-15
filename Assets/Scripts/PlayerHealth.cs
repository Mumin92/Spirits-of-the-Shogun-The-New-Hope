using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    public int startingHealth = 100;
    [HideInInspector]
    public int currentHealth;                                   
    public Slider healthSlider;                                 
    public Image damageImage;
    public Image healImage;
    public Image powerupImage;
    //public AudioClip deathClip;                     
    public float flashSpeed = 5f;                               // The speed the damageImage will fade at.
    public Color damageFlashColour;
    public Color healFlashColour;
    public Color powerupFlashColour;
    public Transform Canvas;

    
    Animator playerAnimator;                                              // Reference to the Animator component.
    //AudioSource playerAudio;                                    // Reference to the AudioSource component.
    //PlayerMovement playerMovement;                              // Reference to the player's movement.
    //PlayerShooting playerShooting;                              // Reference to the PlayerShooting script.
    [HideInInspector]
    public bool isDead;

    private bool damaged;
    private bool healed;
    private bool powered;

    public void Awake()
    {
        // Setting up the references.
        playerAnimator = GetComponent<Animator>();
        /*playerAudio = GetComponent<AudioSource>();
        playerMovement = GetComponent<PlayerMovement>();
        playerShooting = GetComponentInChildren<PlayerShooting>();*/
        isDead = false;
        currentHealth = startingHealth;
    }


    void Update()
    {
        if (damaged)
            damageImage.color = damageFlashColour;
        else
            damageImage.color = Color.Lerp(damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
        if (healed)
            healImage.color = healFlashColour;
        else
            healImage.color = Color.Lerp(healImage.color, Color.clear, flashSpeed * Time.deltaTime);
        if (powered)
            powerupImage.color = powerupFlashColour;
        else
            powerupImage.color = Color.Lerp(powerupImage.color, Color.clear, flashSpeed * Time.deltaTime);

        damaged = false;
        healed = false;
        powered = false;
    }


    public void TakeDamage(int amount)
    {
        damaged = true;
        currentHealth -= amount;
        healthSlider.value = currentHealth;
        GameObject.Find("GameData").GetComponent<LevelData>().BreakStreak();

        // Play the hurt sound effect.
        //playerAudio.Play();

        if (currentHealth <= 0 && !isDead)
        {
            StartCoroutine(Death());
        }
    }
    public void RestoreDamage(int amount)
    {
        healed = true;
        currentHealth += amount;
        healthSlider.value = currentHealth;

        // Play a heal sound effect?
        //playerAudio.Play();

        if (currentHealth > 100)
            currentHealth = 100;
    }

    public void PowerupEffect()
    {
        //Debug.Log("itta pupu");
        powered = true;
    }

    IEnumerator Death()
    {
        isDead = true;
        playerAnimator.SetBool("Death", true);
        yield return new WaitForSecondsRealtime(1.5f);
        Canvas.gameObject.SetActive(true);
        SoundManager.instance.StopMusic();
        GameObject.Find("GameData").GetComponent<LevelData>().GameOver();
        Time.timeScale = 0;

        // Turn off any remaining shooting effects.
        //playerShooting.DisableEffects();

        // Tell the animator that the player is dead.
        //anim.SetTrigger("Die");

        // Set the audiosource to play the death clip and play it (this will stop the hurt sound from playing).
        //playerAudio.clip = deathClip;
        //playerAudio.Play();

        // Turn off the movement and shooting scripts.
        //playerMovement.enabled = false;
        //playerShooting.enabled = false;
    }
}
