using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class FollowCamera : MonoBehaviour 
{
    private GameObject _player;

	// Use this for initialization
	void Start () 
	{
        _player = GameObject.FindGameObjectWithTag("Player");
        Assert.IsNotNull(_player);
	}

    private void LateUpdate ()
    {
        transform.position = _player.transform.position;
    }
}
