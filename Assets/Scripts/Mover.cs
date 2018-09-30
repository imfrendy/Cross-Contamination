using UnityEngine;
using System.Collections;

public class Mover : MonoBehaviour
{
    void AsideJump()
    {
        float move=(-Screen.width / 33);
        transform.position += new Vector3(move, 0, 0);
    }
}
