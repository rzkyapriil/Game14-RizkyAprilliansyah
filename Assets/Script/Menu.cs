using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public GameObject SettingsMenu;
    public GameObject Menus;
    // Start is called before the first frame update
    void Start()
    {
        SettingsMenu.SetActive(false);
    }

    public void LoadSettings()
    {
        Menus.SetActive(false);
        SettingsMenu.SetActive(true);
    }

    public void Close()
    {
        SettingsMenu.SetActive(false);
        Menus.SetActive(true);
    }

    public void LoadScene(string screenIndex)
    {
        SceneManager.LoadScene(screenIndex);
    }
}
