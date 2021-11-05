using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {


    public void Start() {
        Input.multiTouchEnabled = false;
        Debug.Log(Application.persistentDataPath);
        var load = Data.Profile;
        
    }

    public void onPlayClick()
    {
        SceneManager.LoadScene("Game");
    }

    public void onDriverClick() {
        PicturePicker.Modify(Data.Profile.drivers);
    }

    public void onPassengerClick() {
        PicturePicker.Modify(Data.Profile.passengers);
        //SceneManager.LoadScene("PicturePicker");
    }

    public void onSettingsClick()
    {
        SceneManager.LoadScene("Settings");
    }

    public void onQuitClick()
    {
        Data.save();
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
