using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIWhenTimerZero : MonoBehaviour
{
	private float _timeAtLastFrame;
	private ParticleSystem _particleSystem;
	private float _deltaTime;

	public void Awake()
	{
		_timeAtLastFrame = Time.realtimeSinceStartup;
		_particleSystem = gameObject.GetComponent<ParticleSystem>();
	}

	public void Update()
	{
		_deltaTime = Time.realtimeSinceStartup - _timeAtLastFrame;
		_timeAtLastFrame = Time.realtimeSinceStartup;
		if (Time.timeScale == 0)
		{
			_particleSystem.Simulate(_deltaTime, false, false);
			_particleSystem.Play();
		}
	}
}
