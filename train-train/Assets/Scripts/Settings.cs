using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class Settings : MonoBehaviour {
    public Slider trainSpeedSlider;
    public Toggle doesGameEndToogle;
    public Toggle limitPassengers;
    public Toggle allowScore;
    public TMP_Dropdown symbolType;
    public TMP_InputField inputFieldFromDigits, inputFieldToDigits;
    public TMP_InputField inputFieldFromLetters, inputFieldToLetters;
    public GameObject SelectStationSymbolPopUp, DigitsRangeSelection, LettersRangeSelection,
                      ChangeSelectedPicturesButton, ChangeSelectedCustomButton;

    public void onBackClick() {
        Data.save();
        SceneManager.LoadScene("Menu");
    }

    public void onSymbolPicturePickClick() {
        ShowSymbolEditFields();
        SelectStationSymbolPopUp.SetActive(true);
    }

    public void onSymbolPicturePickExitClick() {
        SaveInputFields();
        SelectStationSymbolPopUp.SetActive(false);
    }

    public void onChangeSymbolPictureClick() {
        PicturePicker.Modify(Data.Profile.textureSymbols);        
    }

    public void onChangeSymbolCustomClick()
    {
        CustomSetPicker.Modify( Data.Profile.customMappings );
    }

    public void onResetProfileClick() {
        Data.reset();
        Start();
    }

    public void Slider_Changed(float newValue) {
        Data.Profile.trainSpeed = newValue;
    }

    private void ShowSymbolEditFields() {
        ChangeSelectedPicturesButton.SetActive(false);
        DigitsRangeSelection.SetActive(false);
        LettersRangeSelection.SetActive(false);
        ChangeSelectedCustomButton.SetActive(false);

        switch (Data.Profile.symbolType) {
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

    public void Symbol_type_selected(int newValue) {
        Data.Profile.symbolType = (SymbolType)newValue;
        ShowSymbolEditFields();
        SaveInputFields();
    }

    public void SaveInputFields() {
         switch (Data.Profile.symbolType) {
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

    public void OnToggleDoesGameEndClick(bool newValue) {
        Data.Profile.doesEnd = newValue;
    }

    public void OnToggleAllowScoreClick(bool newValue) {
        Data.Profile.allowScore = newValue;
    }

    public void OnToggleLimitPassengersClick(bool newValue) {
        Data.Profile.limitPassengers = newValue;
    }

    private void Start() {
        allowScore.isOn = Data.Profile.allowScore;
        doesGameEndToogle.isOn = Data.Profile.doesEnd;
        limitPassengers.isOn = Data.Profile.limitPassengers;
        symbolType.value = (int)Data.Profile.symbolType;
        trainSpeedSlider.value = Data.Profile.trainSpeed;
    }
}
