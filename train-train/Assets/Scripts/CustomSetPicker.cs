using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CustomSetPicker : MonoBehaviour {

    public static string PreviousScene = "";
    public static SymbolMappings pickable = Data.Profile.customMappings;
    public GameObject customTemplate;
    public TextMeshProUGUI selectedCounterTextBox;
    public TMP_InputField input;
    private bool isInDeleteMode = false;


    public static void Modify( SymbolMappings pickable ) {
        CustomSetPicker.pickable = pickable;
        CustomSetPicker.PreviousScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene("CustomSetPick");
    }

    // Use this for initialization
    private void Start() {
        DrawCustoms(pickable.all());
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

    public bool isSelected(SymbolMapping mapping) {
        return pickable.isSelected(mapping);
    }

    public void HandleSelectRequest(SymbolMapping mapping, GameObject gameObject) {
        if (isSelected(mapping)) 
            pickable.deselect(mapping);
        else 
            pickable.select(mapping);

        if (isInDeleteMode) {
            pickable.remove(mapping);
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
        List<Texture2D> textures = new List<Texture2D>();
        for (int i = 0; i < paths.Length; i++ ){
            Texture2D texture = NativeGallery.LoadImageAtPath(paths[i], 700, false);
            if( texture != null)
                textures.Add(texture);
        }
        HandlePictureAddition(textures);
    }

    private void HandlePictureAddition(List<Texture2D> textures) {
        List<SymbolMapping> mapping = new List<SymbolMapping>();
        textures.ForEach( t => mapping.Add( new SymbolMapping(t) ) );
        HandleSymbolMappingAddition(mapping);
    }

    public void HandleTextSymbolAddition() {
        var mapping = new List<SymbolMapping>();
        mapping.Add(new SymbolMapping( input.text ) );
        HandleSymbolMappingAddition(mapping);
    }

    private void HandleSymbolMappingAddition( List<SymbolMapping> mapping ) {
        mapping.ForEach( t => pickable.add( t ) );
        DrawCustoms( mapping );
    }

    private void DrawCustoms(List<SymbolMapping> mappings) {
        selectedCounterTextBox.text = pickable.NumberOfSelected().ToString();
        for (int i = 0; i < mappings.Count; i++) {
            Instantiate( customTemplate, transform).GetComponent<CustomSetMapping>().setMapping(mappings[i]);
        }
    }
}
