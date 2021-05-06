using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PicturePicker : MonoBehaviour {

    public static string PreviousScene = "";
    public static Pickable pickable = Data.Profile.drivers;
    public GameObject imageTemplate;
    public TextMeshProUGUI selectedCounterTextBox;
    private bool isInDeleteMode = false;


    public static void Modify(Pickable pickable) {
        PicturePicker.pickable = pickable;
        PicturePicker.PreviousScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene("PicturePicker");
    }

    // Use this for initialization
    private void Start() {
        DrawPictures(pickable.AllTextures());
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

    public bool isSelected(Texture2D texture) {
        return pickable.IsSelected(texture);
    }

    public void HandleSelectRequest(Texture2D texture, GameObject gameObject) {
        if (isSelected(texture)) 
            pickable.Deselect(texture);
        else 
            pickable.Select(texture);

        if (isInDeleteMode) {
            pickable.Remove(texture);
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
        DrawPictures(textures);
        textures.ForEach(t => pickable.Add(t));
    }

    private void DrawPictures(List<Texture2D> textures) {
        selectedCounterTextBox.text = pickable.NumberOfSelected().ToString();
        for (int i = 0; i < textures.Count; i++) {
            Rect rect = new Rect(0, 0, textures[i].width, textures[i].height);
            Sprite sprite = Sprite.Create(textures[i], rect, new Vector2(0.5f, 0.5f));
            Image image = ((GameObject)Instantiate(imageTemplate, transform)).GetComponent<Image>();
            image.preserveAspect = true;
            image.sprite = sprite;
        }
    }
}
