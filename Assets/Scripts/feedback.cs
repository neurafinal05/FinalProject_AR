using System.Collections;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class feedback : MonoBehaviour
{
    [SerializeField] InputField feedback1; // This will be the email field
    [SerializeField] InputField feedback2;
    [SerializeField] Text successMessage;
    [SerializeField] Text errorMessage; // Add this for displaying error messages
    [SerializeField] GameObject blurPanel; // Add this for the Blur Panel

    string URL = "https://docs.google.com/forms/u/0/d/e/1FAIpQLSeoZfAh1RLyklN0HUERd-FrwrxkB8Qn3eVfJcqJDZS1A6Izrg/formResponse";

    public void Send()
    {
        if (string.IsNullOrEmpty(feedback1.text) || string.IsNullOrEmpty(feedback2.text))
        {
            ShowErrorMessage("Please fill in both fields.");
        }
        else if (!IsValidEmail(feedback1.text))
        {
            ShowErrorMessage("Please enter a valid email address.");
        }
        else
        {
            StartCoroutine(Post(feedback1.text, feedback2.text));
            ClearInputFields();
        }
    }

    IEnumerator Post(string s1, string s2)
    {
        WWWForm form = new WWWForm();
        form.AddField("entry.561395451", s1);
        form.AddField("entry.88811824", s2); // Replace "entry.123456789" with the actual entry ID for the second input field

        UnityWebRequest www = UnityWebRequest.Post(URL, form);

        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(www.error);
        }
        else
        {
            Debug.Log("Form submitted successfully!");
            ShowSuccessMessage();
        }
    }

    void ClearInputFields()
    {
        feedback1.text = "";
        feedback2.text = "";
    }

    void ShowSuccessMessage()
    {
        successMessage.text = "Form submitted successfully!";
        successMessage.gameObject.SetActive(true);
        blurPanel.SetActive(true); // Enable the Blur Panel
        Invoke("HideSuccessMessage", 3f); // Hide message after 2 seconds
    }

    void HideSuccessMessage()
    {
        successMessage.gameObject.SetActive(false);
        blurPanel.SetActive(false); // Disable the Blur Panel
    }

    void ShowErrorMessage(string message)
    {
        errorMessage.text = message;
        errorMessage.gameObject.SetActive(true);
        Invoke("HideErrorMessage", 3f); // Hide message after 2 seconds
    }

    void HideErrorMessage()
    {
        errorMessage.gameObject.SetActive(false);
    }

    bool IsValidEmail(string email)
    {
        string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        return Regex.IsMatch(email, emailPattern);
    }
}
