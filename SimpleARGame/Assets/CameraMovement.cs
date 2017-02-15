using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {

	public Camera cam;
	public Quaternion rotation;
	Gyroscope gyro;
	bool hasGyro = false;

	// Use this for initialization
	void Start () {
		if (cam == null) {
			Debug.LogError ("Camera has not been assigned.");
		}
		if (SystemInfo.supportsGyroscope) {
			gyro = Input.gyro;
			gyro.enabled = true;
			Debug.Log ("Supports gyro!!");
			hasGyro = true;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (hasGyro){
			rotation = gyro.attitude;
			cam.transform.rotation = rotation;
			cam.transform.Rotate (0f, 0f, 180f, Space.Self); // Swap handedness of quaternion to gyroscope.
			cam.transform.Rotate(90f, 180f, 0f, Space.World); // Rotate to accommodate for camera being on the back of the device (phone).
		}
	}
}
