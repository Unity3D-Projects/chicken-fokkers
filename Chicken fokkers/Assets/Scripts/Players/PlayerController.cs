﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	public bool alive = true;
	public int score = 0;
	public float health = 100;
	[HideInInspector] public GameObject DeathExplosion;
	[HideInInspector] public GameController GameController;
	[HideInInspector] public DamageController DamageController;
	[HideInInspector] public PlayerMovement PlayerMovement;
	[HideInInspector] public ShootController ShootController;
	[HideInInspector] public DetachableWheelScript DetachableWheelScript;
	[HideInInspector] public PlayerAbility PlayerAbility;
	[HideInInspector] public GameObject Vfx;
	[HideInInspector] public GameObject CrashingPlayer;
	[HideInInspector] public Rigidbody2D rb;
	[HideInInspector] public float initialHealth;

	// Use this for initialization
	void Start () {
		score = 0;
		initialHealth = health;
	}

	public void Die(){
		Debug.Log(gameObject.name +" is dead");

		alive = false;

		Vector2 vel = rb.velocity;

		ShootController.CancelShooting();

		GameObject crashingPlayer = Instantiate(CrashingPlayer, transform.position, Quaternion.Euler(0, 0, gameObject.transform.eulerAngles.z));
		Instantiate(DeathExplosion, transform.position, Quaternion.Euler(0, 0, gameObject.transform.eulerAngles.z));

		crashingPlayer.GetComponent<Rigidbody2D>().velocity = vel;

		//--check if the player had a wheel showing - if not, hide on crashingPlayer too
		if(!DetachableWheelScript.hasWheel){
			crashingPlayer.GetComponent<CrashingScript>().RemoveWheel();
		}

		gameObject.SetActive(false);


		//--show the scoreboard - or start another round
		GameController.EndRoundCountdown();
	}

	public void ResetPlayer(){

		//--called when the round begins again - also on the 1st round
		
		alive = true;
		gameObject.SetActive(true);
		DamageController.ResetDamage();
		health = initialHealth;
		PlayerMovement.MoveToStartPos();
		ShootController.ResetFireRate();
		PlayerMovement.autoPilot = true;
		Invoke("CancelAutopilot", 2);
		DetachableWheelScript.Reset();
		PlayerAbility.ResetAbility();

		
	}

	public void StartAutopilot(){
		Debug.Log("start autopilot");
		PlayerMovement.autoPilot = true;
	}

	void CancelAutopilot(){
		PlayerMovement.autoPilot = false;
	}

	void FixedUpdate () {
		if(Input.GetKey("s")){
			PlayerMovement.MoveToStartPos();
		}

	}
}
