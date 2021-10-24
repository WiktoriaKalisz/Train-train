using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Credentials : MonoBehaviour {

    public void onBackCredentialsClick()
    {
        Debug.Log("weszlo");
        SceneManager.LoadScene("Settings");
    }

}
