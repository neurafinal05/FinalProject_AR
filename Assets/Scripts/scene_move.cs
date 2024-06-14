using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class scene_move : MonoBehaviour
{
    public void ButtonMoveScence(string level)
    {
        SceneManager.LoadScene(level);
    }
}