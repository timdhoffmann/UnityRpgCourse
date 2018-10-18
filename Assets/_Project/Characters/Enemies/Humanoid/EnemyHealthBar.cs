using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    private RawImage healthBarRawImage = null;
    private Enemy enemy = null;

    // Use this for initialization
    private void Start()
    {
        enemy = GetComponentInParent<Enemy>(); // Different to way player's health bar finds player
        healthBarRawImage = GetComponent<RawImage>();
    }

    // Update is called once per frame
    private void Update()
    {
        float xValue = -(enemy.CurrentHealthAsPercentage / 2f) - 0.5f;
        healthBarRawImage.uvRect = new Rect(xValue, 0f, 0.5f, 1f);
    }
}