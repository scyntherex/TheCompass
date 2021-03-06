﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ResolutionsDropdownScript : MonoBehaviour
{

    public Dropdown resolutionDropdown;

    public Toggle fullscreenToggle;

    public Text countdownText;

    public GameObject confirmChangesCanvas;

    private int resolutionWidthCurrent; //saved screen width

    private int resolutionHeightCurrent; //saved screen height
    
    public float revertTimer; //Number countdown will start from until reverting back to old resolution  

    //Deactivates confirm canvas.
    void Start()
    {
        confirmChangesCanvas.SetActive(false);
    }
    
    //First, "saves" current width and height and of game screen. Second, sets resolution to whichever user chooses.
    //Next, activates the confirm canvas. Last, starts countdown coroutine.
    public void resolutionDropdownValueChangedHandler(Dropdown target)
    {
        resolutionWidthCurrent = Screen.width;
        resolutionHeightCurrent = Screen.height;
        switch (target.value)
        {
            case 0:
                Screen.SetResolution(1920, 1080, fullscreenToggle.isOn);
                break;
            case 1:
                Screen.SetResolution(1680, 1050, fullscreenToggle.isOn);
                break;
            case 2:
                Screen.SetResolution(1600, 900, fullscreenToggle.isOn);
                break;
            case 3:
                Screen.SetResolution(1440, 900, fullscreenToggle.isOn);
                break;
            case 4:
                Screen.SetResolution(1400, 1050, fullscreenToggle.isOn);
                break;
            case 5:
                Screen.SetResolution(1366, 768, fullscreenToggle.isOn);
                break;
            case 6:
                Screen.SetResolution(1360, 768, fullscreenToggle.isOn);
                break;
            case 7:
                Screen.SetResolution(1280, 1024, fullscreenToggle.isOn);
                break;
            case 8:
                Screen.SetResolution(1280, 960, fullscreenToggle.isOn);
                break;
            case 9:
                Screen.SetResolution(1280, 800, fullscreenToggle.isOn);
                break;
            case 10:
                Screen.SetResolution(1280, 768, fullscreenToggle.isOn);
                break;
            case 11:
                Screen.SetResolution(1280, 720, fullscreenToggle.isOn);
                break;
            case 12:
                Screen.SetResolution(1280, 600, fullscreenToggle.isOn);
                break;
            case 13:
                Screen.SetResolution(1152, 864, fullscreenToggle.isOn);
                break;
            case 14:
                Screen.SetResolution(1024, 768, fullscreenToggle.isOn);
                break;
            case 15:
                Screen.SetResolution(800, 600, fullscreenToggle.isOn);
                break;
            case 16:
                Screen.SetResolution(640, 480, fullscreenToggle.isOn);
                break;
            case 17:
                Screen.SetResolution(640, 400, fullscreenToggle.isOn);
                break;
            case 18:
                Screen.SetResolution(512, 384, fullscreenToggle.isOn);
                break;
            case 19:
                Screen.SetResolution(400, 300, fullscreenToggle.isOn);
                break;
            case 20:
                Screen.SetResolution(320, 240, fullscreenToggle.isOn);
                break;
            case 21:
                Screen.SetResolution(320, 200, fullscreenToggle.isOn);
                break;
        }
        confirmChangesCanvas.SetActive(true);
        StartCoroutine("Countdown");
        
    }

    //First, stops the countdown. Next, saves current screen resolutions. Finally, deactivates confirm canvas.
    public void YesButton()
    {
        StopCoroutine("Countdown");
        resolutionWidthCurrent = Screen.width;
        resolutionHeightCurrent = Screen.height;
        confirmChangesCanvas.SetActive(false);
    }


    //First stops the countdown. Next sets screen to old resolution. Finally, deactivates confirm canvas.
    public void NoButton()
    {
        StopCoroutine("Countdown");
        Screen.SetResolution(resolutionWidthCurrent, resolutionHeightCurrent, fullscreenToggle.isOn);
        confirmChangesCanvas.SetActive(false);
    }

    //Coroutine that starts counting down from revertTimer to 0 and displays countdown. Once done, activates the no button.
    IEnumerator Countdown()
    {
        for(float i = revertTimer; i >= 0; i--)
        {
            countdownText.text = "Keep changes? Reverting back in " + i + " seconds.";
			yield return new WaitForSecondsRealtime (1);
        }        
        NoButton();
    }
}