using UnityEngine;
using System.Collections;

public class BearTrapsScript : MonoBehaviour {

    private void Start()
    {
        gameObject.name = gameObject.GetInstanceID().ToString();
    }

}
