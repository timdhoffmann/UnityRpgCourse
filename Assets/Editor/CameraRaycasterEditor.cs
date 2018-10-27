using UnityEditor;

// TODO consider changing to a property drawer
[CustomEditor(typeof(CameraRaycaster))]
public class CameraRaycasterEditor : Editor
{
    // Stores the UI state.
    private bool isLayerPrioritiesUnfolded = true;

    /// <summary>
    /// Modifies and draws the properties.
    /// Gets called at every OnGUI in Unity's execution order.
    /// </summary>
    public override void OnInspectorGUI()
    {
        // 1.
        // Needed at the beginning of each custom editor script's OnInspectorGUI().
        // Reads and serializes cameraRaycaster instance.
        serializedObject.Update();

        // 2.
        // Custom modifications to properties.

        // Draws a custom foldout and stores its state.
        isLayerPrioritiesUnfolded = EditorGUILayout.Foldout(isLayerPrioritiesUnfolded, "Layer Priorities");

        if (isLayerPrioritiesUnfolded)
        {
            // Indents contained lines.
            EditorGUI.indentLevel++;
            {
                // Lines to be drawn indented.

                BindArraySize();
                BindArrayElements();
            }
            // Un-indents contained lines.
            EditorGUI.indentLevel--;
        }

        // 3.
        // Needed at the end of each custom editor script's OnInspectorGUI().
        // Applies modified properties back (de-serializes back to
        // cameraRaycaster (and creates undo point).
        serializedObject.ApplyModifiedProperties();
    }

    #region Helper Methods
    /// <summary>
    /// Modifies the size of the array.
    /// </summary>
    private void BindArraySize()
    {
        // Gets the desired array's size from the serialized object.
        int currentArraySize = serializedObject.FindProperty("layerPriorities.Array.size").intValue;

        // Draws an int field with the current array size.
        int requiredArraySize = EditorGUILayout.IntField("Size", currentArraySize);

        // Detects if the user changes the required array size.
        if (requiredArraySize != currentArraySize)
        {
            // Sets the array size to the required size.
            serializedObject.FindProperty("layerPriorities.Array.size").intValue = requiredArraySize;
        }
    }

    /// <summary>
    /// Modifies the content of the array (here, the layers represented as int).
    /// </summary>
    private void BindArrayElements()
    {
        // Gets the desired array's size from the serialized object.
        int currentArraySize = serializedObject.FindProperty("layerPriorities.Array.size").intValue;

        for (int i = 0; i < currentArraySize; i++)
        {
            // Finds the current array element's property (layer stored as int).
            var prop = serializedObject.FindProperty(string.Format("layerPriorities.Array.data[{0}]", i)); // for i = 0, "layerPriorities.Array.data[0]"

            // Gets the respective layer, represented by the current int, from the layer settings.
            prop.intValue = EditorGUILayout.LayerField(string.Format("Layer {0}:", i), prop.intValue);
        }
    }
    #endregion
}