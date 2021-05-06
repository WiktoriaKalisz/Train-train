using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CustomPicker : MonoBehaviour {

    public static string PreviousScene = "";
    public static SymbolMapping pickable;
    public GameObject customTemplate;
    public TextMeshProUGUI selectedCounterTextBox;
    public TMP_InputField input;
    private bool isInDeleteMode = false;

    public static void Modify(SymbolMapping pickable ) {
        CustomPicker.pickable = pickable;
        CustomPicker.PreviousScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene("CustomPicker");
    }

    // Use this for initialization
    private void Start() {
        DrawCustoms(Data.Profile.customMappings.allMatchees());
    }

    public void DeletePictureOnClick(Image image) {
        isInDeleteMode = !isInDeleteMode;

        Color color = image.color;
        color.a = isInDeleteMode ? 0.4f : 0.0f;
        image.color = color;
    }

    public void BackOnClick() {
        if (!pickable.IsSelectedEnough()) { return; }
        Data.save();
        SceneManager.LoadScene(PreviousScene);
    }

    public bool isSelected(Symbol symbol) {
        return pickable.isSelected(symbol);
    }

    public void HandleSelectRequest(Symbol symbol, GameObject gameObject) {
        if (isSelected(symbol)) 
            pickable.deselect(symbol);
        else 
            pickable.select(symbol);

        if (isInDeleteMode) {
            Data.Profile.customMappings.removeMatchee(symbol);
            Destroy(gameObject);
        }

        selectedCounterTextBox.text = pickable.NumberOfSelected().ToString();
    }

    public void TakePictureOnClick(int maxSize = -1) {
        NativeCamera.TakePicture((path) => {
            if (path == null)
                return;

            Texture2D texture = NativeCamera.LoadImageAtPath(path, 700, false);
            if(texture != null)
                HandlePictureAddition(new List<Texture2D> { texture });
        }, maxSize);
    }

    public void AddPicturesFromGalleryOnClick() {
        if( NativeGallery.IsMediaPickerBusy() )
            return;

        if (NativeGallery.CanSelectMultipleFilesFromGallery())
            NativeGallery.GetImagesFromGallery((paths) => HandlePictureAddition(paths), "Select pictures", "image/*");
        else
            NativeGallery.GetImageFromGallery((path) => HandlePictureAddition(new[] { path }), "Select picture", "image/*");
    }

    private void HandlePictureAddition(string[] paths) {
        var textures = paths.ToList().Select(path => NativeGallery.LoadImageAtPath(path, 700, false)).Where(t => t != null).ToList();
        HandlePictureAddition(textures);
    }

    private void HandlePictureAddition(List<Texture2D> textures) {
        var mapping = textures.Select(t => new Symbol(t)).ToList();
        HandleSymbolAddition(mapping);
    }

    public void HandleTextSymbolAddition() {
        HandleSymbolAddition(new List<Symbol> { new Symbol(input.text) });
    }

    private void HandleSymbolAddition( List<Symbol> mapping ) {
        mapping.ForEach( t => Data.Profile.customMappings.addMatchee( t ) );
        DrawCustoms( mapping );
    }

    private void DrawCustoms(List<Symbol> symbols) {
        selectedCounterTextBox.text = pickable.NumberOfSelected().ToString();
        symbols.ForEach(s => Instantiate(customTemplate, transform).GetComponent<CustomMapping>().setSymbol(s));
    }
}
