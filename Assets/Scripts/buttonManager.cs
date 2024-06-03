using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class buttonManager : MonoBehaviour
{
   public void ButtonMoveScence(string level)
    {
        SceneManager.LoadScene(level);
    }
}
