using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenu : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public void StartGame()
    {
        SceneManager.LoadScene("Scene Test 1");
    }
    public void Quit()
    {
        Application.Quit();
    }
}
