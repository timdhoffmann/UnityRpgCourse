using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class CustomCursor : MonoBehaviour 
{
    [SerializeField] private Texture2D _standardCursor;
    [SerializeField] private Vector2 _standardCursorTopLeftOffset;
    private CameraRaycaster _cameraRaycaster;

	// Use this for initialization
	void Start () 
	{
        _cameraRaycaster = GetComponent<CameraRaycaster>();
        Assert.IsNotNull(_cameraRaycaster);

        Cursor.SetCursor(_standardCursor, _standardCursorTopLeftOffset, CursorMode.Auto);
	}
	
	// Update is called once per frame
	void Update () 
	{

	}
}