using UnityEngine;

public class Follow : MonoBehaviour {

    private Transform target;
    public float moveSpeed = 3.0f;
    public float rotationSpeed = 3.0f;

    private Transform myTransform;

    void Awake()
    {
        myTransform = transform; //cache transform data for easy access/preformance
    }

    // Use this for initialization
    void Start () {
        target = GameObject.FindWithTag("Player").transform; //target the player
    }
	
	// Update is called once per frame
	void Update () {

        myTransform.rotation = Quaternion.Slerp(myTransform.rotation,
        Quaternion.LookRotation(target.position - myTransform.position), rotationSpeed * Time.deltaTime);

        //move towards the player
        myTransform.position += myTransform.forward * moveSpeed * Time.deltaTime;
    }
}
