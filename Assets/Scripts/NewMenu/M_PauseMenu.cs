using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class M_PauseMenu : MonoBehaviour
{
    //Variables
    [SerializeField] GameObject pauseMenu;
    [SerializeField] M_MainMenu mainMenu;
    //Functions

    private void Start() 
    {
        SetActive(false);

    }

    public bool GetActive() => pauseMenu.activeSelf;

    public void SetActive(bool active)
    {
        Cursor.visible = active;

        pauseMenu.SetActive(active);
    }

    public M_MainMenu GetMainMenuObject => mainMenu;
}
