using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class TouchReact : MonoBehaviour {

	bool isTouching, isLaunching;

	Image myImage;
	public GameObject projectile;
	public Camera mainCam;
	public float waitTime = 2.5f;
	public float projectileZPosFromCam = 2.0f;

	public bool dragPaused;
	public float dragTime, allowedPause;
	public Vector3 startPoint, latestPoint;

	// Use this for initialization
	void Start () {
		isTouching = false;
		isLaunching = false;
		dragPaused = true;
		dragTime = 0f;
		allowedPause = 0.5f;
		startPoint = Vector3.zero;
		latestPoint = Vector3.zero;

		myImage = gameObject.GetComponent<Image>();

		if (projectile == null) {
			Debug.LogError ("Projectile needs to be assigned.");
		} else {
			projectile.SetActive (false);
		}

		if (mainCam == null) {
			Debug.LogError ("Main Cam needs to be assigned.");
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (isTouching) {
			if (SystemInfo.deviceType == DeviceType.Handheld) {
				if (Input.touchCount > 0) {
					//if (Input.GetTouch (0).phase == TouchPhase.Moved) {
						Vector3 touchPos = new Vector3 (Input.GetTouch(0).position.x, Input.GetTouch(0).position.y, projectileZPosFromCam);
						UpdateProjectilePosition (touchPos);
					//}
				}
			} else if (SystemInfo.deviceType == DeviceType.Desktop) {
				if (Input.GetMouseButton (0)) {
					//Debug.Log ("mouse " + Input.mousePosition);
					Vector3 mousePos = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, projectileZPosFromCam);
					//Debug.Log("world pos of mouse: " + mainCam.ScreenToWorldPoint(mousePos).ToString());
					UpdateProjectilePosition(mousePos);
				}
			}
		}
	}

	void UpdateProjectilePosition(Vector3 inputPos){
		if (startPoint == Vector3.zero) {
			startPoint = inputPos;
			dragTime = 0f;
		} else if (inputPos.y < startPoint.y) {
			startPoint = inputPos;
			dragTime = 0.0f;
		} /*else if (inputPos.x != startPoint.x) {
			startPoint.x = inputPos.x;
			dragTime = 0.0f;
		}*/ else if (dragTime > allowedPause) {
			dragTime = 0.0f;
			startPoint = inputPos;
		} else {
			dragTime += Time.deltaTime;
		}

		latestPoint = inputPos;

		// Update the projectile's position on screen.
		projectile.transform.position = mainCam.ScreenToWorldPoint (inputPos);
	}

	void LaunchProjectile(){
		dragTime += Time.deltaTime;
		isLaunching = true;
		Vector3 diff = mainCam.ScreenToWorldPoint(latestPoint) - mainCam.ScreenToWorldPoint(startPoint); // Latest should never have a Y position lower than start.
		Vector3 velocity = diff/dragTime;
		Debug.Log ("velocity??? " + velocity);
		projectile.GetComponent<Rigidbody> ().velocity = new Vector3(velocity.x, velocity.y, velocity.y);
		// Reset
		dragTime = 0f;
		startPoint = Vector3.zero;
		latestPoint = Vector3.zero;
	}

	public void TouchDown(){
		Debug.Log ("touch started");
		isTouching = true;
		myImage.enabled = false;

		projectile.SetActive (true);
	}

	public void TouchUp(){
		Debug.Log ("touch ended");
		if (!isLaunching) {
			isTouching = false;
			LaunchProjectile ();
			StartCoroutine (WaitToReset ());
		}
	}

	void ResetProjectile(){
		myImage.enabled = true;
		projectile.SetActive (false);
		isLaunching = false;
		projectile.GetComponent<Rigidbody> ().velocity = Vector3.zero;
	}

	IEnumerator WaitToReset(){
		yield return new WaitForSeconds (waitTime);
		ResetProjectile ();
	}
}
