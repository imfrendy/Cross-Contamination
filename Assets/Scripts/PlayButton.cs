using UnityEngine;
using System.Collections;

public class PlayButton : MonoBehaviour {

	public void ChangeScene(int scene)
    {
        Application.LoadLevel(scene);
    }
}
