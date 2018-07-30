using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class CustomCursor : MonoBehaviour 
{
    private CameraRaycaster _cameraRaycaster;

	// Use this for initialization
	void Start () 
	{
        _cameraRaycaster = GetComponent<CameraRaycaster>();
        Assert.IsNotNull(_cameraRaycaster);
	}
	
	// Update is called once per frame
	void Update () 
	{

	}
}