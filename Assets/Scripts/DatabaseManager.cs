using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;

public class DatabaseManager : MonoBehaviour
{
    private string baseUrl = "https://maheshmpa2.azurewebsites.net"; // Replace with your Azure app service URL

    public IEnumerator RegisterUser(string username, string password)
    {
        string registerUrl = baseUrl + "/register.php";
        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        formData.Add(new MultipartFormDataSection("username", username));
        formData.Add(new MultipartFormDataSection("password", password));

        using (UnityWebRequest www = UnityWebRequest.Post(registerUrl, formData))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("User registered successfully!");
            }
        }
    }

    public IEnumerator LoginUser(string username, string password)
    {
        string loginUrl = baseUrl + "/login.php";
        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        formData.Add(new MultipartFormDataSection("username", username));
        formData.Add(new MultipartFormDataSection("password", password));

        using (UnityWebRequest www = UnityWebRequest.Post(loginUrl, formData))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("Login successful!");
            }
        }
    }
}
