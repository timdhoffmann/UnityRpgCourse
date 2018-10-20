using UnityEngine;
using UnityEngine.Assertions;

public class Enemy : MonoBehaviour
{
    #region Fields
    [SerializeField]
    private float _maxHealth = 100f;
    [SerializeField]
    [Range(0f, 100f)]
    private float _currentHealth = 100f;
    #endregion

    public float CurrentHealthAsPercentage => _currentHealth / _maxHealth;

    // Use this for initialization
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
    }
}