using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour {

      //Players Health
    public int CurrentHealth;                                   // The current health the player has.
    public Slider HealthSlider;                                 // Reference to the UI's health bar.

    // The audio clips hurt and death.
    AudioSource PlayerAudio;                                    // Reference to the AudioSource component.
    public AudioClip ScreamClip;
    public AudioClip DeathClip;

    //HeartBeat Audio
    public GameObject HeartBeat;
    private AudioSource HeartBeatAudio;

    // The Image and flash of palyer getting hurt
    public Image DamageImage;                                   // Reference to an image to flash on the screen on being hurt.
    public Image BloodSpatter;                                   // Reference to an image to blood on the screen on being hurt.
    public float FlashSpeed = 1f;                               // The speed the DamageImage will fade at.
    public Color FlashColour = new Color(1f, 0f, 0f, 0.1f);     // The colour the DamageImage is set to, to flash.
    public Color BloodColor = new Color(255f, 255f, 255f, 0.1f);

    //Game Over Manager
    public GameObject HudCanvas;
    Animator GameOverAnimator;

    //Player Animatior
    Animator PlayerAnimator;                                    // Reference to the Animator component.
    

    bool isDead;                                                // Whether the player is dead.
    bool damaged;                                               // True when the player gets damaged.

    void Awake()
    {
        // Setting up the references.
        PlayerAnimator = GetComponent<Animator>();
        PlayerAudio = GetComponent<AudioSource>();

        //Get Heartbeat audio refrence
        HeartBeatAudio = HeartBeat.GetComponent<AudioSource>();

        // Set the initial health of the player.
        CurrentHealth = 100;

        //Game OVer Canvas animation
        GameOverAnimator = HudCanvas.GetComponent<Animator>();
      
    }


    void Update()
    {
        // If the player has just been damaged...
        if (damaged)
        {
            DamageImage.color = FlashColour;
            BloodSpatter.color = BloodColor;
        }
        // Otherwise...
        else
        {
            // Blood spatter and red flash <<<This needs to be taken out but CrossFadeAlpha doesnt work in Unity <<<
            if (DamageImage.color != Color.clear)
            {
                DamageImage.color = Color.Lerp(DamageImage.color, Color.clear, FlashSpeed * Time.deltaTime);
                BloodSpatter.color = Color.Lerp(BloodSpatter.color, Color.clear, FlashSpeed * Time.deltaTime);
              
            }
          
        }

        // Reset the damaged flag.
        damaged = false;
    }


    public void TakeDamage(int amount)
    {
      
        
        // isDead
        if (isDead == false)  // Is Player dead
        {



            // Set the damaged flag so the screen will flash.
            damaged = true;

        // Reduce the current health by the damage amount.
        CurrentHealth -= amount;

        // Set the health bar's value to the current health.
        HealthSlider.value = CurrentHealth;

        // Play the hurt sound effect.
        PlayerAudio.clip = ScreamClip;
        PlayerAudio.Play();

        //Heartbeat Audio
        if (CurrentHealth == 75)
        {
            HeartBeatAudio.pitch = 1.3f;

        } else if (CurrentHealth == 50)
        {
            HeartBeatAudio.pitch = 1.7f;
        }
        else if (CurrentHealth == 25)
        {
            HeartBeatAudio.pitch = 2f;
        }


            HeartBeatAudio.Play();


        // If the player has lost all it's health and the death flag hasn't been set yet...
        if (CurrentHealth <= 0 && !isDead)
        {
            // ... it should die.
            HeartBeatAudio.pitch = 1f;
            Death();
        }

        }


    }


    void Death()
    {
        // Set the death flag so this function won't be called again.
        isDead = true;

        // Tell the animator that the player is dead.
        PlayerAnimator.SetTrigger("Die");
        GameOverAnimator.SetTrigger("GameOver");

        // Set the audiosource to play the death clip and play it (this will stop the hurt sound from playing).
        PlayerAudio.clip = DeathClip;
        PlayerAudio.Play();

    }


    public void RestartLevel()
    {
        // Reload the level that is currently loaded.
        SceneManager.LoadScene(0);
    }

}
