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

    private void OnEnable()
    {
        // Variable initializations.
        _cameraRaycaster = GetComponent<CameraRaycaster>();
        Assert.IsNotNull(_cameraRaycaster);

        // Event subscribing.
        _cameraRaycaster.LayerChanged += OnLayerChanged;
    }

    private void OnDisable()
    {
        // Event un-subscribing.
        _cameraRaycaster.LayerChanged -= OnLayerChanged;
    }

    // Use this for initialization
    private void Start()
    {
        Assert.IsNotNull(_standardCursor);
        Cursor.SetCursor(_standardCursor, _standardCursorTopLeftOffset, CursorMode.Auto);
    }

    #region EVENT SUBSCRIBER METHODS
    /// <summary>
    /// Called when raycasting detects a change in the layer hit.
    /// </summary>
    /// <param name="sender">The object that broadcasted the event.</param>
    /// <param name="e">Additional information about the event.</param>
    private void OnLayerChanged(object sender, LayerChangedEventArgs e)
    {
        switch (e.CurrentLayer)
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
    #endregion
}