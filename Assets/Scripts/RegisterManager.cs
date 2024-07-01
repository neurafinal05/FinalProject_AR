using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class RegisterManager : MonoBehaviour
{
    public InputField usernameField;
    public InputField passwordField;
    public Text messageText;

    // Minimum password length
    private int minPasswordLength = 6;

    public void Register()
    {
        string username = usernameField.text;
        string password = passwordField.text;

        // Check password length
        if (password.Length < minPasswordLength)
        {
            messageText.text = $"Password must be at least {minPasswordLength} characters long.";
            return;
        }

        StartCoroutine(RegisterRequest(username, password));
    }

    IEnumerator RegisterRequest(string username, string password)
    {
        string registerUrl = "https://maheshmpa2.azurewebsites.net/register.php";
        WWWForm form = new WWWForm();
        form.AddField("username", username);
        form.AddField("password", password);

        using (UnityWebRequest www = UnityWebRequest.Post(registerUrl, form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(www.error);
                messageText.text = "Network Error";
            }
            else
            {
                string responseText = www.downloadHandler.text;
                Debug.Log(responseText);

                // Process response
                var response = JsonUtility.FromJson<Response>(responseText);
                messageText.text = response.message;
            }
        }
    }

    [System.Serializable]
    public class Response
    {
        public string message;
    }
}
