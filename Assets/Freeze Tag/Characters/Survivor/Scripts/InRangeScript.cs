using UnityEngine;
using System.Collections;

public class InRangeScript : MonoBehaviour {

    AudioSource audioToPlay;
    public bool isInTrigger;

    void Start()
    {
        audioToPlay = GetComponent<AudioSource>();
    }

    void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Killer"))

        {
            isInTrigger = true;
            audioToPlay.volume = 1;
            audioToPlay.Play();
        }
    }


    void OnTriggerExit(Collider other)

    {
        if (other.CompareTag("Killer"))

        {

            isInTrigger = false;
            StartCoroutine(fadeOut());

        }
    }

    //Fade music
    IEnumerator fadeOut()
    {
        float t = 1;
        while (t > 0.0f && !isInTrigger)
        {
            t -= 0.02f;
            audioToPlay.volume = t;
            yield return new WaitForSeconds(0.1f);
        }
        if (!isInTrigger)
        {
            audioToPlay.volume = 0.0f;
            audioToPlay.Stop();
        }

    }
}
