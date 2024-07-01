using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class send : MonoBehaviour
{
    [SerializeField] InputField feedback1;
    [SerializeField] InputField feedback2;
    [SerializeField] Text successMessage;

    string URL = "https://docs.google.com/forms/u/0/d/e/1FAIpQLSeoZfAh1RLyklN0HUERd-FrwrxkB8Qn3eVfJcqJDZS1A6Izrg/formResponse";

    public void Send()
    {
        StartCoroutine(Post(feedback1.text, feedback2.text));
        ClearInputFields();
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
        Invoke("HideSuccessMessage", 2f); // Hide message after 2 seconds
    }

    void HideSuccessMessage()
    {
        successMessage.gameObject.SetActive(false);
    }
}
