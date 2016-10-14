using UnityEngine;
using System.Collections;

public class StartGame : MonoBehaviour {

    private NavMeshAgent NavigationMesh;
    private float MaxHeight = 2f;
    private float MaxWidth = 90f;
    private float MaxLength = 90f;
    private float MinWidth = 25f;
    private float MinLength = 25f;

    private float BearTrapsmaxSpawn = 300; //Bear Traps amounts
    private float KillersmaxSpawn = 5; //Killers amounts

    // Use this for initialization
    void Start () {

        //Area to spawn
        MaxHeight = 1f;
        MaxWidth = 90f;
        MaxLength = 90f;
        MinWidth = 5f;
        MinLength = 5f;

        for (int i = 1; i < BearTrapsmaxSpawn; i++)
        {
            Vector3 randomPoint = SearchRandomPoint();
            if    (randomPoint == Vector3.zero)
            { 
            }
            else
            {
                Instantiate(Resources.Load("BearTrap"), randomPoint, Quaternion.identity); // Create Bear trap
            }
        }


        //Area to spawn
        MaxHeight = 6.1f;
        MaxWidth = 90f;
        MaxLength = 90f;
        MinWidth = 25f;
        MinLength = 25f;

        //Killers
        for (int i = 1; i < KillersmaxSpawn; i++)
        {
            Vector3 randomPoint = SearchRandomPoint();
            if (randomPoint == Vector3.zero)
            {

            }
            else
            {
                Instantiate(Resources.Load("Killer"), randomPoint, Quaternion.identity); // Create Bear trap
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    /// <summary>
    /// //Random Patrol
    /// </summary>
    public Vector3 SearchRandomPoint() //Patrol random area
    {
        for (int i = 0; i < 30; i++)  //Find random point on mesh
        {

            Vector3 randomPoint = new Vector3(Random.Range(MinWidth, MaxWidth), Random.Range(0, MaxHeight), Random.Range(MinLength, MaxLength));
            NavMeshHit hit;

            if (NavMesh.SamplePosition(randomPoint, out hit, 2.0f, NavMesh.AllAreas))
            {
                return hit.position;
            }
        }
        return Vector3.zero; //no valid position found try again
    }
    /// <summary> End
    /// 
}
