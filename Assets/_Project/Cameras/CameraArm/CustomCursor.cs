using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent(typeof(CameraRaycaster))]
public class CustomCursor : MonoBehaviour 
{
    [SerializeField] private Texture2D _standardCursor = null;
    [SerializeField] private Vector2 _standardCursorTopLeftOffset = new Vector2(0f, 0f);
    [SerializeField] private Texture2D _attackCursor = null;
    [SerializeField] private Vector2 _attackCursorTopLeftOffset = new Vector2(0f, 0f);
    [SerializeField] private Texture2D _unresolvedCursor = null;
    
    private CameraRaycaster _cameraRaycaster;

    private void OnEnable ()
    {
        // Variable initializations.
        _cameraRaycaster = GetComponent<CameraRaycaster>();
        Assert.IsNotNull(_cameraRaycaster);

        // Event subscribing.
        _cameraRaycaster.LayerChanged += OnLayerChanged;
    }


    private void OnDisable ()
    {
        // Event un-subscribing.
        _cameraRaycaster.LayerChanged -= OnLayerChanged;
    }

    #region EVENT HANDLING
    private void OnLayerChanged (object sender, LayerChangedEventArgs e)
    {
        // TODO: Implement functionality.
        print(this + " received event from " + sender);
    } 
    #endregion

    // Use this for initialization
    private void Start () 
	{
        Assert.IsNotNull(_standardCursor);
        Cursor.SetCursor(_standardCursor, _standardCursorTopLeftOffset, CursorMode.Auto);
	}
	
	// Update is called once per frame
	private void Update () 
	{
        switch (_cameraRaycaster.CurrentLayerHit)
        {
            case Layer.Walkable:
                Cursor.SetCursor(_standardCursor, _standardCursorTopLeftOffset, CursorMode.Auto);
                break;

            case Layer.Enemy:
                Assert.IsNotNull(_attackCursor);
                Cursor.SetCursor(_attackCursor, _attackCursorTopLeftOffset, CursorMode.Auto);
                break;

            default:
                Assert.IsNotNull(_unresolvedCursor);
                Cursor.SetCursor(_unresolvedCursor, _standardCursorTopLeftOffset, CursorMode.Auto);
                break;
        }
	}
}