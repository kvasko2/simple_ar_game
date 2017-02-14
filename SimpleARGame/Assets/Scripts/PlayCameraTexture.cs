using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayCameraTexture : MonoBehaviour {

	public float camWidth, camHeight;
	private WebCamTexture wbt;
	public float scaleMultiplier = 1f;

	// Use this for initialization
	void Start () {
		Renderer renderer = gameObject.GetComponent<Renderer> ();
		wbt = new WebCamTexture ();

		WebCamDevice[] devices = WebCamTexture.devices;
		Debug.Log ("Possible cameras:");
		for (int i = 0; i < devices.Length; i++) {
			Debug.Log ("---" + devices[i].name);
		}
		
		renderer.material.mainTexture = wbt;
		wbt.Play();
	}
	
	// Update is called once per frame
	void Update () {
		camWidth = wbt.width * scaleMultiplier;
		camHeight = wbt.height * scaleMultiplier;

		transform.localScale = new Vector3(camWidth/100f, camHeight/100f, 1f);
	}
}
