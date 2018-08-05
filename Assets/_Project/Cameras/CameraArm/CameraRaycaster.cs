using UnityEngine;

public class CameraRaycaster : MonoBehaviour
{
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
    public Layer LayerHit { get; private set; }
    #endregion

    private void Start () // TODO Awake?
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
                LayerHit = layer;
                return;
            }
        }

        // Otherwise return background hit
        var backgroundHit = new RaycastHit
        {
            distance = _distanceToBackground
        };
        Hit = backgroundHit;

        LayerHit = Layer.RaycastEndStop;
    }

    // Nullable return value type.
    private RaycastHit? RaycastForLayer(Layer layer)
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
