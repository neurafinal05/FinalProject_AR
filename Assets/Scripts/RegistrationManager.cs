using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.SceneManagement;

public class RegistrationManager : MonoBehaviour
{
    public InputField usernameInput;
    public InputField passwordInput;
    public Text messageText;

    private string registerURL = "https://maheshmpa2.azurewebsites.net/register.php"; // Replace with your actual PHP file URL

    public void RegisterUser()
    {
        string username = usernameInput.text;
        string password = passwordInput.text;

        if (IsPasswordValid(password))
        {
            StartCoroutine(SendRegistrationRequest(username, password));
        }
        else
        {
            messageText.text = "Password must be at least 6 characters long.";
        }
    }

    private bool IsPasswordValid(string password)
    {
        return password.Length >= 6;
    }

    IEnumerator SendRegistrationRequest(string username, string password)
    {
        // Create the registration data
        RegistrationData data = new RegistrationData();
        data.username = username;
        data.password = password;

        // Convert the registration data to JSON
        string jsonData = JsonUtility.ToJson(data);

        // Create a UnityWebRequest to send JSON data
        using (UnityWebRequest www = new UnityWebRequest(registerURL, "POST"))
        {
            byte[] bodyRaw = new System.Text.UTF8Encoding().GetBytes(jsonData);
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
                messageText.text = "Error connecting to server.";
            }
            else
            {
                string responseText = www.downloadHandler.text;
                Debug.Log(responseText);

                // Parse the JSON response
                RegistrationResponse response = JsonUtility.FromJson<RegistrationResponse>(responseText);

                // Display message based on server response
                if (response.message == "User registered")
                {
                    messageText.text = "Registration successful!";
                    // Load the next scene after successful registration
                    SceneManager.LoadScene("login"); // Replace "MainScene" with the name of your target scene
                }
                else if (response.message == "Username already exists")
                {
                    messageText.text = "Username already exists. Please choose another.";
                }
                else
                {
                    messageText.text = "Error: " + response.message;
                }
            }
        }
    }
}

[System.Serializable]
public class RegistrationData
{
    public string username;
    public string password;
}

[System.Serializable]
public class RegistrationResponse
{
    public string message;
}
