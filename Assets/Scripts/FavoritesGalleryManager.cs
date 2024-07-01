using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class FavoriteGalleryManager : MonoBehaviour
{
    public GameObject imagePrefab; // Prefab with an Image component
    public Transform contentPanel;

    private string favoritesPath;

    void Start()
    {
        favoritesPath = Path.Combine(Application.persistentDataPath, "Favorites");
        LoadFavorites();
    }

    private void LoadFavorites()
    {
        if (Directory.Exists(favoritesPath))
        {
            string[] files = Directory.GetFiles(favoritesPath, "*.png");
            foreach (string file in files)
            {
                AddImageToGallery(file);
            }
        }
    }

    public void AddImageToGallery(string filePath)
    {
        GameObject newImage = Instantiate(imagePrefab, contentPanel);
        Texture2D texture = LoadTexture(filePath);
        newImage.GetComponent<Image>().sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
    }

    public void RemoveImageFromGallery(string filePath)
    {
        foreach (Transform child in contentPanel)
        {
            Image image = child.GetComponent<Image>();
            if (image != null && image.sprite.texture.name == Path.GetFileNameWithoutExtension(filePath))
            {
                Destroy(child.gameObject);
                break;
            }
        }
    }

    private Texture2D LoadTexture(string filePath)
    {
        byte[] fileData = File.ReadAllBytes(filePath);
        Texture2D texture = new Texture2D(2, 2);
        texture.LoadImage(fileData);
        return texture;
    }
}
