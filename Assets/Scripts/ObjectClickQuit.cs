using UnityEngine;
using System.Collections;

public class ObjectClickQuit : MonoBehaviour
{
    void OnMouseDown()
    {
        Application.Quit();
    }
}
