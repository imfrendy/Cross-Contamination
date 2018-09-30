using UnityEngine;
using System.Collections;

public class GlobalShader : MonoBehaviour {
    public Shader toonShader;
    public Camera mainCam;
    public Camera viewmodelCam;
    // Use this for initialization
    void Start () {
        mainCam.SetReplacementShader(toonShader, "");
        viewmodelCam.SetReplacementShader(toonShader, "");
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
