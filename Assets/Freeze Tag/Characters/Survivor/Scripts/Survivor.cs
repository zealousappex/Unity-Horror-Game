using UnityEngine;
using System.Collections;

public class Survivor : MonoBehaviour {

    //Player
    Animator PlayerAnimation;             // Player Animation
    PlayerHealth playerHealth;              // Reference to the player's health.
    public int TrapDamage = 25;           // The amount of health taken away per attack.                                    
    public bool InTrapTriggerArea = false;                 //Player hit a trigger
    public bool IsTrapped = false;                 //Player Is Traped

    //Store object triggered
    private GameObject CollidedObject;      // CollidedObject
    Animator CollidedObjectAnimation;       // CollidedObject Animation

    // Bear trap on leg
    public GameObject BearTrapOnLeg;
    BearTrapOnLegScript BearTrapOnLegScript;              // Reference to the BearTrapOnLegScript

    // Bear trap Carring
    public GameObject BearTrapCarring;      // Bear trap Carring oject refrence 
    public AudioClip BearTrapCarringOpening;       // Bear trap Opening audio
    private AudioSource BearTrapCarringAudio;       // Bear trap audio source

    //Items Found
    public GameObject GasCanIcon;      // CollidedObject
    public bool GasCanFound = false;

    void Start()
    {
        //Player
        playerHealth = GetComponent<PlayerHealth>();    // Reference to the player's health
        PlayerAnimation = GetComponent<Animator>();    // Player Animation

        // Bear Carring
        BearTrapCarring.SetActive(false);
        BearTrapCarringAudio = GetComponent<AudioSource>();
        
        // Bear trap on leg
        BearTrapOnLegScript = BearTrapOnLeg.GetComponent<BearTrapOnLegScript>();    // Reference to the Bear Trap On Leg

        //Gascan                                                
        GasCanIcon.SetActive(false);  // Hide Gas Can in UI since they havent found it yet

    }

    //Weapon damage
    void OnCollisionEnter(Collision collision)
    {
        ContactPoint OtherContactPoint = collision.contacts[0];
        if (OtherContactPoint.otherCollider.tag == "Weapon")
        {
            playerHealth.TakeDamage(TrapDamage);  // ... damage the player.
            Debug.Log("Player Hit by " + OtherContactPoint.otherCollider);
        }
    }



    // which object Collided 
    void OnTriggerEnter(Collider other){
   
        if (!IsTrapped && other.CompareTag("BearTrap"))
        {

            //Store object triggered
            CollidedObject = other.transform.parent.gameObject;    //Get refrence to object triggered
            CollidedObjectAnimation = CollidedObject.GetComponent<Animator>();     //Get refrence to object animation

           //Object trigger name
        switch (other.name){

            //If they are pressing E key they can disable the trap cheking InTrapTriggerArea = true
            case "ToggleActivate":

                //Player hit a trigger
                InTrapTriggerArea = true;

               // Debug.Log(CollidedObject.name + " Safe Area To Disable Trap"); //Toggle trap area to Dis/Enable Trap without Damage
                break;

            case "Pressure_Plate":

                //Is Trap disabled
                if (CollidedObjectAnimation.GetBool("Closed") == false)   //Player Is Traped stop player and damage them
                {

                    //Flag Trapped
                    IsTrapped = true;   // Flag they are trapped

                    //Player BearTrap Animation
                    BearTrapOnLegScript.BearTrapOnLegEnable(true);  //Player BearTrap on leg show
                    BearTrapOnLegScript.BearTrapOnLegAnimation(true);    // Player BearTrap on leg animation

                    // Collided Object Animate
                    CollidedObjectAnimation.SetBool("Closed", true);   // Collided Object Animate
                    CollidedObject.SetActive(false);  // Collided Object Hide

                    //Player Animation
                    PlayerAnimation.SetInteger("CurrentAction", 1);     // Player Animation falling to ground
                    PlayerAnimation.SetBool("Injured", true);   //Player Injured animation for walking/Running <<<<Issue with Mask of player so no animation setup yet<<<<
                        
                    // Heath Bar
                    playerHealth.TakeDamage(TrapDamage);  // ... damage the player.
                    //Debug.Log(CollidedObject.name + " trap was triggered!");
                }


                break;
            }

        }

       //Hit Item type
        else if (!IsTrapped && other.CompareTag("Item"))  //Hit Item type
        {
            switch (other.name)
            {
                case "GasCan":  //  Hit Gas Can

                    GasCanFound = true;
                    Destroy(other.gameObject);  //Remove collider object
                    GasCanIcon.SetActive(true);  // Show Gas Can in UI

                    //Debug.Log(other.name + " Gascan");

                    break;

                case "KeyGate": //  Hit Key


                    break;
            }

          
        }

       // Debug.Log(other.name + " Entered Trigger");

    }


    // which object Exited Collider
    void OnTriggerExit(Collider other){

        InTrapTriggerArea = false;

        switch (other.name){

            case "ToggleActivate":

                break;

            case "Pressure_Plate":

                break;

        }

       // Debug.Log(other.name + " Exit Trigger");

    }

    //Update is called once per frame
    void Update()

    {

         //Check Flag Trapped
        if (IsTrapped && Input.GetKeyDown(KeyCode.E))    //Player is in trap
        {

            //Disable Trap
            PlayerAnimation.SetInteger("CurrentAction", 0);   //Player Is getting out of trap

            //Player Bear Trap Leg animation script
            BearTrapOnLegScript.BearTrapOnLegAnimation(false); // Player BearTrap on leg animation getting out of trap

            //Wait for aniamtion then show Collided object
            StartCoroutine(WaitOpenTrap(3.0F)); //Wait then call function
            //Debug.Log(CollidedObject.name + " Trap Disabled");

        }

        //Player is Dis/Enabling Trap
        else if (InTrapTriggerArea && Input.GetKeyDown(KeyCode.E))  
        {

            //Collided toggle Dis/Enable with E Key 
            if (CollidedObjectAnimation.GetBool("Closed") == true)   //Check current state of object animation
            {

                //Enable object
                PlayerAnimation.SetTrigger("Drop");       //Player Enable Pickup animation(which is used for Dis/Enable Object)
                CollidedObjectAnimation.SetBool("Closed", false);    //Collided object Open animation
                BearTrapCarringAudio.PlayOneShot(BearTrapCarringOpening);  // Player Audio on Bear Trap
                //Debug.Log(CollidedObject.name + " Trap Enable");        

            }
            else
            {
                //Disable object
                PlayerAnimation.SetTrigger("Drop");  //Player Disable animation(which is used for Dis/Enable Object)
                CollidedObjectAnimation.SetBool("Closed", true);   //Collided object Close animation
                BearTrapCarringAudio.PlayOneShot(BearTrapCarringOpening);  // Player Audio on Bear Trap
                //Debug.Log(CollidedObject.name + " Trap Disable");
            }
         
        }


        //Player is Droping/Picking Up object
        else if (!IsTrapped && Input.GetKeyDown(KeyCode.R))
        {

            //Player Picking up or putting down object
            if (BearTrapCarring.activeSelf == true)  //Is the BearTrap in hand Active Put Down
            {

                // Drop Object Animation and Instantiate object
                PlayerAnimation.SetTrigger("Drop");   // Player Drop animation
                BearTrapCarringAudio.PlayOneShot(BearTrapCarringOpening);  // Player Audio on Bear Trap
                StartCoroutine(WaitPutDown(3F));      // Wait for animation
                //Debug.Log("Put Down Trap");

            }


            //Player is Picking Up Object  and Destroy Object
            else if (InTrapTriggerArea)   //Is the BearTrap in hand NOT Active then PickUp
            {
                InTrapTriggerArea = false;    // Object Picked Up No logger in trigger flag
                PlayerAnimation.SetTrigger("PickUp");   // Player PickUp Animation
                BearTrapCarringAudio.PlayOneShot(BearTrapCarringOpening);  // Player Audio on Bear Trap
                StartCoroutine(WaitPickUp(3F));    // Wait then hide Collided Object
                //Debug.Log(CollidedObject.name + " PickUp Trap");

            }
          
        }

  
    }

    //Player is Trapped and opening trap, Wait for Open Trap Animation Then show Trap closed
    IEnumerator WaitOpenTrap(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        BearTrapOnLegScript.BearTrapOnLegEnable(false);  //Player Bear Trap Leg Active to False to hide Object
        CollidedObject.gameObject.SetActive(true);  //Set trap Active to True to show Object
        CollidedObjectAnimation.SetBool("Closed", true);   // Collided Object Animate
        //Player Is out of trap now
        IsTrapped = false;   //After animation flag out of trap
       // Debug.Log(CollidedObject.name + " Player got out of trap");

    }

    //Player is picking up object, Wait PickUp Object then show Object in Hand and remove collider object
    IEnumerator WaitPickUp(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        BearTrapCarring.SetActive(true);      //Show Bear trap in hand object
        Destroy(CollidedObject.gameObject);  //Remove collider object
        //Debug.Log(CollidedObject.name + " Player PickedUp Trap");
    }

    //Put Down Object Then creat object in front of player
    IEnumerator WaitPutDown(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        Instantiate(Resources.Load("BearTrap"), transform.position + (transform.forward * 0.5f), transform.rotation); // Create Bear trap object in front of user
        BearTrapCarring.SetActive(false); //Hide Beartrap in hand
        //Debug.Log("Player Put Down Trap");

    }

}

