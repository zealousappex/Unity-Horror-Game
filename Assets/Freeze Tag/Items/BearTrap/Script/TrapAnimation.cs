using UnityEngine;
using System.Collections;

public class TrapAnimation : MonoBehaviour {

    private Animator animator;


    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            animator.SetBool("Open", true);

        }

        Debug.Log("Enter " + name);
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            animator.SetBool("Open", false);
        }

        Debug.Log("Exit " + name);
    }

}