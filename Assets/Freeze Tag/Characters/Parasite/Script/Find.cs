using UnityEngine;
using System.Collections;

public class Find : MonoBehaviour {

    public Transform visionPoint;
    public GameObject PlayerController;

    public float visionAngle = 30f;
    public float visionDistance = 10f;
    public float moveSpeed = 2f;
    public float chaseDistance = 3f;

    private Vector3? lastKnownPlayerPosition;

    // Update is called once per frame
    void Update()
    {

        Vector3 lookAt = PlayerController.transform.position;
        lookAt.y = transform.position.y;
        transform.LookAt(lookAt);
    }

    void FixedUpdate()
    {
        Look();
    }

    void Look()
    {
        Vector3 deltaToPlayer = PlayerController.transform.position;
        Vector3 directionToPlayer = deltaToPlayer.normalized;

        float dot = Vector3.Dot(transform.forward, directionToPlayer);

        if (dot < 0)
        {
            return;
        }

        float distanceToPlayer = directionToPlayer.magnitude;

        if (distanceToPlayer > visionDistance)
        {
            return;
        }

        float angle = Vector3.Angle(transform.forward, directionToPlayer);

        if (angle > visionAngle)
        {
            return;
        }

        RaycastHit hit;
        if (Physics.Raycast(transform.position, directionToPlayer, out hit, visionDistance))
        {
            if (hit.collider.gameObject == PlayerController.gameObject)
            {
                
                lastKnownPlayerPosition = PlayerController.transform.position;
            }
        }
        Debug.DrawRay(transform.position, directionToPlayer, Color.green);
    }
}
