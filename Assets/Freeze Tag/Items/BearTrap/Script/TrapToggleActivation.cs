using UnityEngine;
using System.Collections;

public class TrapToggleActivation : MonoBehaviour {

    private Animator animator;
    bool InTrigger = false;

    void Awake()
    {
        animator = transform.parent.GetComponent<Animator>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")

        {
            InTrigger = true;

        }

        Debug.Log("Enter " + name);
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")

        {
            InTrigger = false;
        }

        Debug.Log("Exit " + name);
    }

    // Update is called once per frame
    void Update () {

        if (InTrigger && Input.GetKeyDown(KeyCode.E))

        {
            if (animator.GetBool("Open") == true)
            {
                animator.SetBool("Open", false);
            }
            else
            {
                animator.SetBool("Open", true);
                Debug.Log("KeyPressed E " + name);
            }
              
        }
    }
}
