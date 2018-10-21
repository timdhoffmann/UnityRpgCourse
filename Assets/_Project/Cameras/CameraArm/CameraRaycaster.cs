using UnityEngine;

#region Helpers Classes
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
    #region EVENTS
    // TODO: Check, which parameter-style is preferred (explicit or using an EventArgs class). The latter (see OnPriorityLayerClicked) was auto-implemented by ReSharper.
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

    #region FIELDS
    // TODO: Need to match name for editor script (remove _)?
    [SerializeField] private int[] _layerPriorities = null;
    [SerializeField] private float _distanceToBackground = 100f;

    private float _maxCameraDepth = 100.0f;
    // So get ? from start with Default layer terrain
    private int topPriorityLayerLastFrame = -1;
    private Camera _viewCamera;
    #endregion

    #region PROPERTIES
    public RaycastHit Hit { get; private set; }
    public Layer CurrentLayerHit { get; private set; }
    #endregion

    private void Start()
    {
        _viewCamera = Camera.main;
    }

    private void Update()
    {
        // Look for and return priority layer hit
        foreach (var layer in _layerPriorities)
        {
            var hit = RaycastForLayer(layer);
            if (hit.HasValue)
            {
                Hit = hit.Value;

                // Checks if the layer has changed.
                if (CurrentLayerHit != layer)
                {
                    CurrentLayerHit = layer;
                    Debug.Log("CurrentLayerHit changed.");

                    // Calls method to rise event.
                    OnLayerChanged(layer);
                }
                return;
            }
        }

        // Otherwise return background hit
        var backgroundHit = new RaycastHit
        {
            distance = _distanceToBackground
        };
        Hit = backgroundHit;

        CurrentLayerHit = Layer.RaycastEndStop;
    }

    /// <summary>
    /// Raycasts for a specified layer.
    /// </summary>
    /// <param name="layer">The layer to raycast.</param>
    /// <returns>The layer hit or null.</returns>
    private RaycastHit? RaycastForLayer(Layer layer) // Nullable return value type.
    {
        int layerMask = 1 << (int)layer; // See Unity docs for mask formation
        Ray ray = _viewCamera.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit; // used as an out parameter
        bool hasHit = Physics.Raycast(ray, out hit, _distanceToBackground, layerMask);
        if (hasHit)
        {
            return hit;
        }
        return null;
    }
}