using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Audio;
//using UnityStandardAssets.ImageEffects;


public class Settings : MonoBehaviour
{
    public Slider trainSpeedSlider;
    public Slider lightColorSlider;
    public Slider soundsSlider;
    public Slider musicSlider;
    public Toggle doesGameEndToogle;
    public Toggle limitPassengers;
    public Toggle allowScore;
    public Toggle allowLabels;
    public Toggle calmbackground;
    public Toggle leftHand;
    public TMP_Dropdown symbolType;
    public TMP_Dropdown colorScheme;
    public TMP_InputField inputFieldFromDigits, inputFieldToDigits;
    public TMP_InputField inputFieldFromLetters, inputFieldToLetters;
    public GameObject SelectStationSymbolPopUp, DigitsRangeSelection, LettersRangeSelection,
                      ChangeSelectedPicturesButton, ChangeSelectedCustomButton;

    public float GammaCorrection;
    public AudioMixer audioMixer;
    public void SetLevel(float value)
    {
        Data.Profile.contrast = lightColorSlider.value;
        //directionalLight.color = Color.red;
        //RenderSettings.ambientLight = Color.red;
        //RenderSettings.ambientLight = new Color(0.5f, 0.5f, 0.5f, 1);
        //RenderSettings.ambientLight = new Color(contrastSlider.value / 255f, contrastSlider.value / 255f, contrastSlider.value / 255f);
    }

    public void SetColorScheme(int value)
    {
        Data.Profile.colorScheme = colorScheme.value;
        //directionalLight.color = Color.red;
        //RenderSettings.ambientLight = Color.red;
        //RenderSettings.ambientLight = new Color(0.5f, 0.5f, 0.5f, 1);
        //RenderSettings.ambientLight = new Color(contrastSlider.value / 255f, contrastSlider.value / 255f, contrastSlider.value / 255f);
    }

    public void onBackClick()
    {
        Data.save();
        SceneManager.LoadScene("Menu");
    }

    public void onCredentialsClick()
    {
        SceneManager.LoadScene("Credentials");
    }

    public void onSymbolPicturePickClick()
    {
        ShowSymbolEditFields();
        SelectStationSymbolPopUp.SetActive(true);
    }

    public void onSymbolPicturePickExitClick()
    {
        SaveInputFields();
        SelectStationSymbolPopUp.SetActive(false);
    }

    public void onChangeSymbolPictureClick()
    {
        PicturePicker.Modify(Data.Profile.textureSymbols);
    }

    public void onChangeSymbolCustomClick()
    {
        CustomSetPicker.Modify(Data.Profile.customMappings);
    }

    public void onResetProfileClick()
    {
        Data.reset();
        Start();
    }

    public void Slider_Changed(float newValue)
    {
        Data.Profile.trainSpeed = newValue;
    }

    public void Music_Changed(float newValue)
    {
        audioMixer.SetFloat("musicVolume", newValue);
        Data.Profile.music = newValue;
    }
    public void Sounds_changed(float newValue)
    {
        audioMixer.SetFloat("soundsVolume", newValue);
        Data.Profile.sounds = newValue;
    }
    private void ShowSymbolEditFields()
    {
        ChangeSelectedPicturesButton.SetActive(false);
        DigitsRangeSelection.SetActive(false);
        LettersRangeSelection.SetActive(false);
        ChangeSelectedCustomButton.SetActive(false);

        switch (Data.Profile.symbolType)
        {
            case SymbolType.SimpleTextures:
                ChangeSelectedPicturesButton.SetActive(true);
                break;
            case SymbolType.NumberRange:
                inputFieldFromDigits.text = Data.Profile.numberRange.begin.ToString();
                inputFieldToDigits.text = Data.Profile.numberRange.end.ToString();
                DigitsRangeSelection.SetActive(true);
                break;
            case SymbolType.Letters:
                inputFieldFromLetters.text = Data.Profile.letters.list[0].ToString();
                inputFieldToLetters.text = Data.Profile.letters.list[Data.Profile.letters.list.Count - 1].ToString();
                LettersRangeSelection.SetActive(true);
                break;
            case SymbolType.CustomMapping:
                ChangeSelectedCustomButton.SetActive(true);
                break;
        }
    }

    public void Symbol_type_selected(int newValue)
    {
        if (Data.Profile.symbolType != (SymbolType)newValue) Data.Profile.ResetScore();
        if ((SymbolType)newValue <= SymbolType.Class5Math && (SymbolType)newValue > SymbolType.Class3Math)
        {
            this.setPassengersProfileValue(false, true);
        }
        else if((SymbolType)newValue <= SymbolType.Class3Math && (SymbolType)newValue >= SymbolType.Class1Math)
        {
            this.setPassengersProfileValue(true, false);
        }
        else
        {
            this.setPassengersProfileValue(true, true);
        }
        Data.Profile.symbolType = (SymbolType)newValue;
        ShowSymbolEditFields();
        SaveInputFields();
    }

    public void ColorSchemeSelected(int newValue)
    {
        Data.Profile.colorScheme = newValue;
        //ShowSymbolEditFields();
        // SaveInputFields();
    }

    public void SaveInputFields()
    {
        switch (Data.Profile.symbolType)
        {
            case SymbolType.NumberRange:
                Data.Profile.numberRange.begin = int.Parse(inputFieldFromDigits.text);
                Data.Profile.numberRange.end = int.Parse(inputFieldToDigits.text);
                break;

            case SymbolType.Letters:
                Data.Profile.letters.list.Clear();
                for (char c = inputFieldFromLetters.text[0]; c <= inputFieldToLetters.text[0]; c++)
                    Data.Profile.letters.list.Add(c);
                break;
        }
    }

    public void OnToggleDoesGameEndClick(bool newValue)
    {
        Data.Profile.doesEnd = newValue;
    }

    public void OnToggleAllowScoreClick(bool newValue)
    {
        Data.Profile.allowScore = newValue;
    }

    public void OnToggleAllowLabelsClick(bool newValue)
    {
        Data.Profile.allowLabels = newValue;
    }

    public void OnToggleSetCalmBackgroundClick(bool newValue)
    {
        Data.Profile.calmBackground = newValue;
    }

    public void OnToggleSetLeftHandClick(bool newValue)
    {
        Data.Profile.leftHand = newValue;
    }
    public void setPassengersProfileValue(bool animals, bool humans)
    {
        Profile.PassengersAnimals = animals;
        Data.Profile.isAnimalsOn = animals;

        Profile.PassengersHumans = humans;
        Data.Profile.isHumansOn = humans;

        Data.Profile.passengers = Profile.defaultPassengers();
    }

    
    public void OnToggleLimitPassengersClick(bool newValue)
    {
        Data.Profile.limitPassengers = newValue;
    }

    private void Start()
    {
        Input.multiTouchEnabled = false;
        allowScore.isOn = Data.Profile.allowScore;
        allowLabels.isOn = Data.Profile.allowLabels;
        calmbackground.isOn = Data.Profile.calmBackground;
        leftHand.isOn = Data.Profile.leftHand;
        doesGameEndToogle.isOn = Data.Profile.doesEnd;
        limitPassengers.isOn = Data.Profile.limitPassengers;
        symbolType.value = (int)Data.Profile.symbolType;
        trainSpeedSlider.value = Data.Profile.trainSpeed;
        musicSlider.value = Data.Profile.music;
        soundsSlider.value = Data.Profile.sounds;
        colorScheme.value = Data.Profile.colorScheme;
    }
}
