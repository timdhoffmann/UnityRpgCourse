using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RawImage))]
public class PlayerHealthBar : MonoBehaviour
{
    #region Fields
    private RawImage healthBarRawImage;
    private Player player;
    #endregion

    #region Methods
    // Use this for initialization
    private void Start()
    {
        player = FindObjectOfType<Player>();
        healthBarRawImage = GetComponent<RawImage>();
    }

    // Update is called once per frame
    private void Update()
    {
        float xValue = -(player.CurrentHealthAsPercentage / 2f) - 0.5f;
        healthBarRawImage.uvRect = new Rect(xValue, 0f, 0.5f, 1f);
    }
    #endregion
}