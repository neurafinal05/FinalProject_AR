using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class AuthManager : MonoBehaviour
{
    public InputField usernameField;
    public InputField passwordField;
    public Text messageText;

    private string registerUrl = "https://maheshmpa2.azurewebsites.net/register.php";
    private string loginUrl = "https://maheshmpa2.azurewebsites.net/login.php";

    public void Register()
    {
        StartCoroutine(RegisterUser());
    }

    public void Login()
    {
        StartCoroutine(LoginUser());
    }

    IEnumerator RegisterUser()
    {
        WWWForm form = new WWWForm();
        form.AddField("username", usernameField.text);
        form.AddField("password", passwordField.text);

        using (UnityWebRequest www = UnityWebRequest.Post(registerUrl, form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                messageText.text = "Error: " + www.error;
            }
            else
            {
                messageText.text = www.downloadHandler.text;
            }
        }
    }

    IEnumerator LoginUser()
    {
        WWWForm form = new WWWForm();
        form.AddField("username", usernameField.text);
        form.AddField("password", passwordField.text);

        using (UnityWebRequest www = UnityWebRequest.Post(loginUrl, form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                messageText.text = "Error: " + www.error;
            }
            else
            {
                messageText.text = www.downloadHandler.text;
            }
        }
    }
}
