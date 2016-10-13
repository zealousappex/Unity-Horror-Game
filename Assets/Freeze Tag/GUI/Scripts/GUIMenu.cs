using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GUIMenu : MonoBehaviour {

    GameObject PauseMenu;
    GameObject InfoUI;

    // Use this for initialization
    void Start () {

        PauseMenu = GameObject.Find("PauseMenu");
        InfoUI = GameObject.Find("InfoUI");

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

    //Resume button from menu
    public void Resume()
    {
        MenuOff();
    }

    //Resume button from menu
    public void Ready()
    {
        ShowMouse(false);
        InfoUI.SetActive(false);
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

    //Show Menu
    private void MenuOn()
    {
        PauseMenu.SetActive(true);
        Time.timeScale = 0;

       ShowMouse(true);

    }

    //Hide Menu
    private void MenuOff()
    {
        PauseMenu.SetActive(false);
        Time.timeScale = 1;

        if  (InfoUI.activeSelf == false)
        {
            ShowMouse(false);
        }
      
    }

    //Show Menu
    private void ShowMouse(bool ShowMouseBool)
    {
        if (ShowMouseBool)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        else {

            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

        }

    }

}

