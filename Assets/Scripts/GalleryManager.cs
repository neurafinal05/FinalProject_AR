using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class GalleryManager : MonoBehaviour
{
    public GameObject imagePrefab; // Prefab with an Image component
    public Transform contentPanel;
    public GameObject favoriteGalleryManager; // Reference to the FavoriteGalleryManager GameObject

    private string galleryPath;
    private string favoritesPath;

    void Start()
    {
        galleryPath = Path.Combine(Application.persistentDataPath, "Gallery");
        favoritesPath = Path.Combine(Application.persistentDataPath, "Favorites");
        LoadGallery();
    }

    private void LoadGallery()
    {
        if (Directory.Exists(galleryPath))
        {
            string[] files = Directory.GetFiles(galleryPath, "*.png");
            foreach (string file in files)
            {
                AddImageToGallery(file);
            }
        }
    }

    private void AddImageToGallery(string filePath)
    {
        GameObject newImage = Instantiate(imagePrefab, contentPanel);
        Texture2D texture = LoadTexture(filePath);
        newImage.GetComponent<Image>().sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

        // Add a button to mark as favorite
        Button favoriteButton = newImage.transform.Find("FavoriteButton").GetComponent<Button>(); // Assuming the button is named "FavoriteButton"
        if (favoriteButton != null)
        {
            favoriteButton.onClick.AddListener(() => AddToFavorites(filePath));
        }
        else
        {
            Debug.LogError("Favorite button not found on image prefab.");
        }

        // Add a delete button
        Button deleteButton = newImage.transform.Find("DeleteButton").GetComponent<Button>(); // Assuming the delete button is named "DeleteButton"
        if (deleteButton != null)
        {
            deleteButton.onClick.AddListener(() => DeleteImage(filePath, newImage));
        }
        else
        {
            Debug.LogError("Delete button not found on image prefab.");
        }
    }

    private void AddToFavorites(string filePath)
    {
        string fileName = Path.GetFileName(filePath);
        string destinationPath = Path.Combine(favoritesPath, fileName);

        // Ensure the favorites directory exists
        if (!Directory.Exists(favoritesPath))
        {
            Directory.CreateDirectory(favoritesPath);
        }

        // Copy the image file to the favorites directory
        File.Copy(filePath, destinationPath, true);

        // Notify the FavoriteGalleryManager to update its display
        favoriteGalleryManager.GetComponent<FavoriteGalleryManager>().AddImageToGallery(destinationPath);
    }

    private void DeleteImage(string filePath, GameObject imageObject)
    {
        // Delete the image file from the gallery directory
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }

        // Remove the image GameObject from the UI
        Destroy(imageObject);

        // Check if the image exists in the favorites directory and delete it
        string fileName = Path.GetFileName(filePath);
        string favoriteFilePath = Path.Combine(favoritesPath, fileName);
        if (File.Exists(favoriteFilePath))
        {
            File.Delete(favoriteFilePath);
            // Optional: Notify the FavoriteGalleryManager to update its display
            favoriteGalleryManager.GetComponent<FavoriteGalleryManager>().RemoveImageFromGallery(favoriteFilePath);
        }

        // Optional: Update the gallery display if needed
        // ReloadGallery(); // You may implement a method to reload the gallery
    }

    private Texture2D LoadTexture(string filePath)
    {
        byte[] fileData = File.ReadAllBytes(filePath);
        Texture2D texture = new Texture2D(2, 2);
        texture.LoadImage(fileData);
        return texture;
    }
}
