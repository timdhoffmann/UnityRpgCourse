using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

#region Helper Classes
/// <summary>
/// Provides arguments to be sent with the LayerChanged event.
/// </summary>
public class LayerChangedEventArgs : System.EventArgs
{
    public int NewLayer { get; }

    public LayerChangedEventArgs(int newLayer)
    {
        NewLayer = newLayer;
    }
}

/// <summary>
/// Provides arguments to be sent with the PriorityLayerClicked event.
/// </summary>
public class PriorityLayerClickedEventArgs : System.EventArgs
{
    public RaycastHit RaycastHit { get; }
    public int LayerHit { get; }

    public PriorityLayerClickedEventArgs(RaycastHit raycastHit, int layerHit)
    {
        RaycastHit = raycastHit;
        LayerHit = layerHit;
    }
}
#endregion

/// <inheritdoc />
/// <summary>
/// Responsible for raycasting from
/// the main camera to the mouse position.
/// </summary>
public class CameraRaycaster : MonoBehaviour
{
    #region FIELDS
    // TODO: Need to match name for editor script (remove "_")?
    [SerializeField] private int[] _layerPriorities = null;
    [SerializeField] private float _distanceToBackground = 100f;

    private readonly float _maxRaycastDepth = 100.0f;
    // So get ? from start with Default layer terrain
    private int _topPriorityLayerLastFrame = -1;
    private Camera _camera;
    #endregion

    #region PROPERTIES
    public RaycastHit Hit { get; private set; }
    #endregion

    private void Start()
    {
        _camera = Camera.main;
    }

    #region EVENTS
    // TODO: Check, which parameter-style is preferred (explicit or using an EventArgs class). The latter (see OnPriorityLayerClicked) was auto-implemented by ReSharper and seems to introduce less coupling.

    public event System.EventHandler<LayerChangedEventArgs> LayerChanged;
    /// <summary>
    /// Method used to raise the event when the layer changed.
    /// </summary>
    protected virtual void OnLayerChanged(int newLayer)
    {
        // Raises event, if subscribers are present.
        LayerChanged?.Invoke(this, new LayerChangedEventArgs(newLayer));
    }

    public event System.EventHandler<PriorityLayerClickedEventArgs> PriorityLayerClicked;
    /// <summary>
    /// Method used to raise the event when the layer changed.
    /// </summary>
    /// <param name="e">EventArgs to be sent with the event (auto-implemented by ReSharper).</param>
    protected virtual void OnPriorityLayerClicked(PriorityLayerClickedEventArgs e)
    {
        // Raises event, if subscribers are present.
        PriorityLayerClicked?.Invoke(this, e);
    }
    #endregion

    private void Update()
    {
        // Check if cursor is over an interactable UI element
        if (EventSystem.current.IsPointerOverGameObject())
        {
            NotifyObserversIfLayerChanged(5);
            return; // Stop looking for other objects
        }

        // Raycast to max depth, every frame as things can move under mouse
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] raycastHits = Physics.RaycastAll(ray, _maxRaycastDepth);

        RaycastHit? priorityHit = FindTopPriorityHit(raycastHits);
        if (!priorityHit.HasValue) // if hit no priority object
        {
            NotifyObserversIfLayerChanged(0); // broadcast default layer
            return;
        }

        // Notify delegates of layer change
        var layerHit = priorityHit.Value.collider.gameObject.layer;
        NotifyObserversIfLayerChanged(layerHit);

        // Notify delegates of highest priority game object under mouse when clicked
        if (Input.GetMouseButton(0))
        {
            OnPriorityLayerClicked(new PriorityLayerClickedEventArgs(priorityHit.Value, layerHit));
        }
    }

    private void NotifyObserversIfLayerChanged(int newLayer)
    {
        if (newLayer != _topPriorityLayerLastFrame)
        {
            _topPriorityLayerLastFrame = newLayer;
            OnLayerChanged(newLayer);
        }
    }

    /// <summary>
    /// Finds the top priority layer hit.
    /// </summary>
    /// <param name="raycastHits"></param>
    /// <returns>The hit with the top priority layer (or null!).</returns>
    private RaycastHit? FindTopPriorityHit(RaycastHit[] raycastHits) // Nullable return value type.
    {
        // Form list of layer numbers hit
        var layersOfHitColliders = new List<int>();
        foreach (var hit in raycastHits)
        {
            layersOfHitColliders.Add(hit.collider.gameObject.layer);
        }

        // Step through layers in order of priority looking for a game object with that layer
        foreach (var layer in _layerPriorities)
        {
            foreach (var hit in raycastHits)
            {
                if (hit.collider.gameObject.layer == layer)
                {
                    return hit; // stop looking
                }
            }
        }
        return null; // because cannot use GameObject? nullable
    }
}