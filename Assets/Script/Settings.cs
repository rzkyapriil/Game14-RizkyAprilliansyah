using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class Settings : MonoBehaviour
{
    public Toggle MuteToggle;
    public bool isFullscreen;
    // Start is called before the first frame update
    void Start()
    {
        Screen.SetResolution(720, 576, isFullscreen);
        AudioListener.volume = 1;
    }

    // Update is called once per frame
    void Update()
    {
        isFullscreen = false;
    }

    public void SetResolution(int value)
    {
        if(value == 0)
        {
            Debug.Log("720 x 576");
            Screen.SetResolution(720, 576, isFullscreen);
        }
        if(value == 1)
        {
            Debug.Log("1366 x 768");
            Screen.SetResolution(1366, 768, isFullscreen);
        }
        if(value == 2)
        {
            Debug.Log("1920 x 1080");
            Screen.SetResolution(1920, 1080, isFullscreen);
        }
    }

    public void IsFullscreen(bool value)
    {
        if(value){
            Debug.Log("FULLSCREEN ON");
            Screen.fullScreen = value;
            isFullscreen = value;
        }else{
            Debug.Log("FULLSCREEN OFF");
            Screen.fullScreen = value;
            isFullscreen = value;
        }
    }

    public void IsMute(bool value)
    {
        if(value){
            Debug.Log("MUTE ON");
            AudioListener.pause = value;
        }else{
            Debug.Log("MUTE OFF");
            AudioListener.pause = value;
        }
    }

    public void SetVolume(float value)
    {
        Debug.Log("Volume: " + (value*100));
        AudioListener.volume = value;   
    }


    public void ExitGame()
    {
        Debug.Log("Exit");
        Application.Quit();
    }
}
