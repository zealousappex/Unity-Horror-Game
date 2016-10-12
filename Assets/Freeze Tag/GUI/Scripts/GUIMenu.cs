using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GUIMenu : MonoBehaviour {

    GameObject PauseMenu;

	// Use this for initialization
	void Start () {

        PauseMenu = GameObject.Find("PauseMenu");
 
        // Turn Off Menu
        MenuOff();
    }
	
	// Update is called once per frame
	void Update () {

        //Show Menu if Escape Key Is pressed
        if (Input.GetKeyDown(KeyCode.Escape))

        {
            MenuOn();
        }
	
	}

    //Show Menu
    private void MenuOn()
    {
        PauseMenu.SetActive(true);
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    //Hide Menu
    private void MenuOff()
    {
        PauseMenu.SetActive(false);
        Time.timeScale = 1;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    //Resume button from menu
    public void Resume()
    {
        MenuOff();
    }

    //Load Main Menu button from menu
    public void MainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }

    //Load Level button from menu
    public void Load()
    {
        SceneManager.LoadScene(PlayerPrefs.GetInt("currentscenesave"));
    }

    //Quit button from menu
    public void Quit()
    {
        Application.Quit();
    }

}

