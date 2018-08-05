using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class CustomCursor : MonoBehaviour 
{
    [SerializeField] private Texture2D _standardCursor = null;
    [SerializeField] private Vector2 _standardCursorTopLeftOffset;
    [SerializeField] private Texture2D _attackCursor = null;
    [SerializeField] private Vector2 _attackCursorTopLeftOffset;
    [SerializeField] private Texture2D _unresolvedCursor = null;
    private CameraRaycaster _cameraRaycaster;

	// Use this for initialization
	void Start () 
	{
        _cameraRaycaster = GetComponent<CameraRaycaster>();
        Assert.IsNotNull(_cameraRaycaster);

        Assert.IsNotNull(_standardCursor);
        Cursor.SetCursor(_standardCursor, _standardCursorTopLeftOffset, CursorMode.Auto);
	}
	
	// Update is called once per frame
	void Update () 
	{
        if (_cameraRaycaster.LayerHit == Layer.Walkable)
        {
            Cursor.SetCursor(_standardCursor, _standardCursorTopLeftOffset, CursorMode.Auto);
        } 
        else if (_cameraRaycaster.LayerHit == Layer.Enemy)
        {
            Assert.IsNotNull(_attackCursor);
            Cursor.SetCursor(_attackCursor, _attackCursorTopLeftOffset, CursorMode.Auto);
        }
        else
        {
            Assert.IsNotNull(_unresolvedCursor);
            Cursor.SetCursor(_unresolvedCursor, _standardCursorTopLeftOffset, CursorMode.Auto);
        }
	}
}