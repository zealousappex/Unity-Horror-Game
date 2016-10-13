using UnityEngine;
using System.Collections;

public class Killer : MonoBehaviour {

    /// <summary>
    /// //Killer
    /// </summary>
    //Killer Control
    private Transform KillerTransform;
    private Transform KillerEyesTransform;
    public GameObject KillerEyes;
    public float KillerVisonRange = 100.0f;
    public Vector3 KillerLocationPoint;

    //Killer Navigation
    private NavMeshAgent NavigationMesh;
    public Vector3 KillerDestinationPoint;
    public float RemainingDistance = 0.0f;
    public bool Patroling = false;
    public bool InKillerTrigger;
    public bool CanSeePlayer = false;
    private float PatrolRange = 10.0f;
    private float DestinationPointOffset = 0.5f;
   
    //Killer Attack
    Animator AttackAnimation;       //Animation Attack
   
    private float AttackTime;  //Number of attacks per second
    private float AttackRepeatTime = 8; // Number of attacks per second
                                        /// <summary> End

    /// <summary>
    /// //Player
    /// </summary>
    //Player Location
    private GameObject Player;
    private GameObject PlayerEyes;
    private Transform PlayerTransform;
    private Transform PlayerEyesTransform;
    public Vector3 PlayersCurrentPoint;
    
    /// <summary> End

    /// <summary>
    /// //Audio
    /// </summary>
    //Audio Chase
    public GameObject ChaseAudioObject;
    private AudioSource AudioChase;
    /// <summary> End
    /// 



    void Awake() //Awake
    {
        //Patrol  Area
        NavigationMesh = GetComponent<NavMeshAgent>();  //Get Navigation area
        AttackAnimation = GetComponent<Animator>();     //Get refrence to object animation
        AudioChase = ChaseAudioObject.GetComponent<AudioSource>(); //Audio For Chase

        Player = GameObject.Find("Survivor");
        PlayerEyes = GameObject.Find("Survivor/Lindsey/mixamorig:Hips/PlayerEyes");

        //Positons of Killer and Player
        KillerTransform = transform; //cache transform data for easy access/preformance
        KillerEyesTransform = KillerEyes.transform; //cache transform data for easy access/preformance
        PlayerEyesTransform = PlayerEyes.transform; //target the player
       
        PlayerTransform = Player.transform; //target the player
        KillerDestinationPoint = KillerTransform.position;   //Default search location

    }
    void Start()  //Start
    {
        SearchRandomPoint();  //Start Patrol
    }
    /// <summary> End

    /// <summary>
    /// //Search for player
    /// </summary>
    void FixedUpdate()   //Fixed Update
    {

        if (InKillerTrigger == true)  //Player in Killer trigger area
        {

            Vector3 KillerLOS = KillerEyesTransform.position; //Killer Eye location
            Vector3 PlayerRay = PlayerEyesTransform.position - KillerLOS; //player location
            RaycastHit hit;

            if (Physics.Raycast(KillerLOS, PlayerRay, out hit)) //Check if we can see player
            {

                if (hit.collider.gameObject == Player)   //Can we see the player
                {
                    Debug.DrawRay(KillerLOS, PlayerRay, Color.red);   //Debug LOS

                    if (CanSeePlayer == false)
                    {
                        Patroling = false; //Patroling falg
                        CanSeePlayer = true;  //Flag Found
                        NavigationMesh.speed = 5;//Killer Run
                        AudioChase.volume = 1; //Chase volume

                        if (!AudioChase.isPlaying)
                        {
                            AudioChase.Play();  //Play chase audio
                        }

                        Debug.Log("Found Player");

                    }

                    if ((PlayerRay.magnitude > 2)) //How close are we, stop before we run into player
                    {
                        NavigationMesh.destination = PlayerTransform.position; //Send Killer to Player current location or last spoted
                    }
                    else
                    {
                        if (Time.time >= AttackTime) //Only attack once per AttackTime seconds

                        {

                            AttackTime = Time.time + AttackRepeatTime;
                            NavigationMesh.destination = transform.position; //Set current location location
                            AttackAnimation.SetTrigger("Attack");    //AttackAnimation animation
                            Debug.Log("Attack Player");

                        }
                       
                    }


                }
                else //Lost sight of player
                {

                    Debug.DrawRay(KillerLOS, PlayerRay, Color.blue);   //Debug LOS
                    CanSeePlayer = false;// Go to last destination point
                    Debug.Log("Lost sight of Player");

                }

            }


        }

        if (NavigationMesh.remainingDistance < DestinationPointOffset & CanSeePlayer == false) //Choose the next destination point when the agent gets close to the current one
        {
                // Debug.Log("Patrol");
                SearchRandomPoint();//Patrol    
        }

        KillerDestinationPoint = NavigationMesh.destination;
        KillerLocationPoint = KillerTransform.position;
        PlayersCurrentPoint = PlayerTransform.position;
        RemainingDistance = NavigationMesh.remainingDistance;
   
    }
    /// <summary> End



    /// <summary>
    /// //Killer Trigger
    /// </summary>
    void OnTriggerEnter(Collider other)  //Killer Trigger Area Chase
    {

        if (other.gameObject == Player)  //Player in Trigger area
        {
            InKillerTrigger = true; //Flag Player in trigger to activate raycast to detect if we can see them
             Debug.Log("Player in Trigger area");
        }

    }
    void OnTriggerExit(Collider other)//Killer trigger area Player outside of
    {

        if (other.gameObject == Player)  //Player left trigger area
        {
            CanSeePlayer = false;    //Go back to Patrolling
            InKillerTrigger = false;  //Stop raycast search

            StartCoroutine(fadeOut());  //Fade Chase Music

            Debug.Log("Exit Killer Trigger area");
        }

    }
    /// <summary> End




    /// <summary>
    /// //Random Patrol
    /// </summary>
    void SearchRandomPoint() //Patrol random area
    {
        Patroling = true; //Patroling falg
        NavigationMesh.speed = 3; //Return to Patrol speed to normal if he was in a chase before
        AttackAnimation.SetBool("Walking",true);   //Walk Animation

        for (int i = 0; i < 30; i++)  //Find random point on mesh
        {

            Vector3 randomPoint = KillerTransform.position + Random.insideUnitSphere * PatrolRange;
            NavMeshHit hit;


            if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
            {
                NavigationMesh.destination = hit.position;

                return;
            }


        }

        NavigationMesh.destination = Vector3.zero; //no valid position found try again

    }
    /// <summary> End




    /// <summary>
    /// //Wait time between attacks
    /// </summary>
    IEnumerator Wait()  //Wait between Attack times
    {
        yield return new WaitForSeconds(3f);

        yield break;
    }
    /// <summary> End



    /// <summary>
    /// //Music fadeout
    /// </summary>
    IEnumerator fadeOut() //Fade music
    {
       // Debug.Log("Fading Chase Audio");
        float t = 1;
        while (t > 0.0f & InKillerTrigger == false)
        {
            t -= 0.02f;
            AudioChase.volume = t;
            yield return new WaitForSeconds(0.1f);
        }

        if (InKillerTrigger == false)
        {
            AudioChase.volume = 0.0f;
            AudioChase.Stop();
        }

        yield break;
    }
    /// <summary> End




}
