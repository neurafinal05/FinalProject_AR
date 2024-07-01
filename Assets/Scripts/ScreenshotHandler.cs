using UnityEngine;
using System.Collections;
using System.IO;

public class ScreenshotHandler : MonoBehaviour
{
    private bool isProcessing = false;
    private string galleryPath;

    void Start()
    {
        galleryPath = Path.Combine(Application.persistentDataPath, "Gallery");
        if (!Directory.Exists(galleryPath))
        {
            Directory.CreateDirectory(galleryPath);
        }
    }

    public void TakeScreenshot()
    {
        if (!isProcessing)
            StartCoroutine(CaptureScreenshot());
    }

    private IEnumerator CaptureScreenshot()
    {
        isProcessing = true;
        yield return new WaitForEndOfFrame();

        Texture2D screenImage = new Texture2D(Screen.width, Screen.height);
        screenImage.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        screenImage.Apply();

        byte[] imageBytes = screenImage.EncodeToPNG();
        string fileName = Path.Combine(galleryPath, "Screenshot_" + System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".png");
        File.WriteAllBytes(fileName, imageBytes);

        isProcessing = false;
        Destroy(screenImage);
    }
}
