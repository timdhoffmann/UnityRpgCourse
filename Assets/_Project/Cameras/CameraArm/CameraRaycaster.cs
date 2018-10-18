using UnityEngine;

#region Helpers Classes
/// <summary>
/// Provides arguments to be sent with the LayerChanged event.
/// </summary>
public class LayerChangedEventArgs : System.EventArgs
{
    public LayerChangedEventArgs(Layer currentLayer)
    {
        CurrentLayer = currentLayer;
    }

    public Layer CurrentLayer { get; }
}
#endregion

/// <summary>
/// Responsible for raycasting from
/// the main camera to the mouse position.
/// </summary>
public class CameraRaycaster : MonoBehaviour
{
    #region EVENTS
    public event System.EventHandler<LayerChangedEventArgs> LayerChanged;

    /// <summary>
    /// Method used to raise the event when the layer changed.
    /// </summary>
    protected virtual void OnLayerChanged(Layer currentLayer)
    {
        // Raises event, if subscribers are present.
        LayerChanged?.Invoke(this, new LayerChangedEventArgs(currentLayer));
    }
    #endregion

    #region FIELDS
    private Layer[] _layerPriorities =
    {
        // The order matters! More specific goes last.
        // TODO: Find better solution to avoid double management of layers.
        Layer.Default,
        Layer.Enemy,
        Layer.Walkable,
    };

    [SerializeField] private float _distanceToBackground = 100f;
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