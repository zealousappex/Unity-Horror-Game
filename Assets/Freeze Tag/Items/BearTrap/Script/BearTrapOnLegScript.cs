using UnityEngine;
using System.Collections;

public class BearTrapOnLegScript : MonoBehaviour
{

    // Bear trap on leg
    public GameObject BearTrapOnLeg;        // Bear trap on leg
    Animator BearTrapOnLegAnimator;        // Bear trap on leg Animation
    public AudioClip BearTrapShutting;      // Bear trap shutting audio
    public AudioClip BearTrapOpening;       // Bear trap Opening audio
    private AudioSource BearTrapOpeningAudio;       // Bear trap audio source


    // Use this for initialization
    void Start()
    {

        // Bear trap on leg
        BearTrapOnLegAnimator = BearTrapOnLeg.GetComponent<Animator>();
        BearTrapOpeningAudio = BearTrapOnLeg.GetComponent<AudioSource>();
        BearTrapOnLeg.SetActive(false);
    }
    // Bear trap on leg animation
    public void BearTrapOnLegAnimation(bool Closed)
    {


        if (Closed)
        {

            BearTrapOnLegAnimator.SetBool("Closed", true);
            BearTrapOpeningAudio.PlayOneShot(BearTrapShutting);
        }
        else {

            BearTrapOnLegAnimator.SetBool("Closed", false);
            BearTrapOpeningAudio.PlayOneShot(BearTrapOpening);
        }


    }

    public void BearTrapOnLegEnable(bool Enable)
    {

        BearTrapOnLeg.SetActive(Enable);
    }
}
