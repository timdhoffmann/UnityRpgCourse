using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Player : MonoBehaviour 
{
    [SerializeField] private float _maxHealth = 100f;
    [SerializeField] [Range(0f, 100f)] private float _currentHealth = 100f;

    public float CurrentHealthAsPercentage => _currentHealth / _maxHealth;
    
    // Use this for initialization
    void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}
}