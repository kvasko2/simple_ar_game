﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReactToHit : MonoBehaviour {

	public Animator animator;
	public float painThresholdMagnitude;
	public float health;
	public bool isAlive;
	public bool canBeDamaged = true;

	// Use this for initialization
	void Start () {
		if (health == 0f) {
			health = 10f;
		}
		isAlive = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (isAlive) {
			if (health < 0f) {
				animator.SetTrigger ("Faint");
				Debug.Log ("Painful hit!");
				isAlive = false;
			}
		}
	}

	void OnCollisionEnter(Collision collision){
		if (isAlive) {
			if (canBeDamaged) {
				canBeDamaged = false;
				float impactForce = collision.relativeVelocity.magnitude;
				if (impactForce > painThresholdMagnitude) {
					animator.SetTrigger ("Wave");
					Debug.Log ("Painful hit!");
				} else {
					Debug.Log ("Regular Hit");
					animator.SetTrigger ("Pickup");
				}

				health -= impactForce;

				StartCoroutine (ResetDamagable ());
			}
		}
	}

	IEnumerator ResetDamagable(){
		yield return new WaitForSeconds(2f);
		if (isAlive) {
			canBeDamaged = true;
		}
	}
}
