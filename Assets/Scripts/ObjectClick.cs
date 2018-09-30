using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ObjectClick : MonoBehaviour
{
    void OnMouseDown()
    {
        SceneManager.LoadScene("game");
    }
}

