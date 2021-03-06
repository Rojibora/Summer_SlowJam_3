﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using TinderBox;
public  class QTEScript : MonoBehaviour 
{
	private bool isActive;
	private float _progress;
	public float progress
	{ 
		get
		{
			return _progress;
		}
		set
		{
			AddProgress(value);
		} 
	}

	private Controls _buttonQTE;
	public Controls buttonQTE
	{
		get
		{
			return _buttonQTE;
		}
		set
		{
			_buttonQTE = value;
		}
	}

	private SpriteRenderer buttonRenderer;

	private Action onComplete;

	public enum QTE
	{
		test, mash, alternate
	}

	public QTE QTEType;
	private List<int> playerList;


	void Start()
	{
		playerList = new List<int>();
		buttonRenderer = GetComponentInChildren<SpriteRenderer> ();
		if (buttonRenderer == null) 
		{
			Debug.LogError ("QTE needs a sprite object as a child");
			Destroy (gameObject);
		} 
		else 
		{
			buttonRenderer.gameObject.SetActive(false);
		}

		_progress = 0.0f;
	}

	void Update()
	{
		if (!isActive)
			return;

		foreach (int ID in playerList) 
		{
			bool input = TinderBoxAPI.ControlDown(ID, buttonQTE);
			if(input)
			{
				Debug.Log("HIT IT!");
				progress = 0.05f;
			}
		}

		if (progress >= 1.0f)
		{
			Debug.Log("Progress Complete!");
			isActive = false;
			Destroy(transform.parent.gameObject);
		}
	}

	public void PlayerEnter (int playerID) 
	{
		if (playerList.Count > 0) {
			foreach (int ID in playerList) {
				if (ID.CompareTo (playerID) != 0)
					continue;
				return;
			}
		}
		playerList.Add (playerID);
		isActive = true;
		buttonRenderer.gameObject.SetActive(true);
	}

	public void PlayerExit (int playerID)
	{
		playerList.Remove(playerID);
		if (playerList.Count == 0)
		{
			isActive = false;
			buttonRenderer.gameObject.SetActive(false);
		}
	}

	private void AddProgress(float amt)
	{
		Mathf.Clamp (amt, 0, 1);

		_progress += amt;
		Mathf.Clamp (_progress, 0, 1);
	}
}