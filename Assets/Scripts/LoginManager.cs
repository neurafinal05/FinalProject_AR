using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.SceneManagement;

public class LoginManager : MonoBehaviour
{
    public InputField usernameInput;
    public InputField passwordInput;
    public Text messageText;

    private string loginURL = "https://maheshmpa2.azurewebsites.net/login.php"; // Replace with your actual PHP file URL

    public void LoginUser()
    {
        string username = usernameInput.text;
        string password = passwordInput.text;

        StartCoroutine(SendLoginRequest(username, password));
    }

    IEnumerator SendLoginRequest(string username, string password)
    {
        // Create the login data
        LoginData data = new LoginData();
        data.username = username;
        data.password = password;

        // Convert the login data to JSON
        string jsonData = JsonUtility.ToJson(data);

        // Create a UnityWebRequest to send JSON data
        using (UnityWebRequest www = new UnityWebRequest(loginURL, "POST"))
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
                LoginResponse response = JsonUtility.FromJson<LoginResponse>(responseText);

                // Display message based on server response
                if (response.message == "Login successful")
                {
                    messageText.text = "Login successful!";
                    // Load the next scene
                    SceneManager.LoadScene("instruct"); // Replace "MainScene" with the name of your target scene
                }
                else if (response.message == "Invalid password")
                {
                    messageText.text = "Invalid password. Please try again.";
                }
                else if (response.message == "User not found")
                {
                    messageText.text = "User not found. Please register.";
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
public class LoginData
{
    public string username;
    public string password;
}

[System.Serializable]
public class LoginResponse
{
    public string message;
}
