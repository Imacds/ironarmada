using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
	Controls the materials on the individual layers of the parallax backgorund

	Author : Hannah Crawford
*/

public class ParallaxBackground : MonoBehaviour {
	public float perspective = 1f;
	public float scale = 10f;
	public float depthScale = 0f;

	public List<Transform> layers;

	void LateUpdate(){
		for(int i = 0; i < layers.Count; i++){
			Transform l = layers[i];
			float d  = l.transform.position.z - transform.position.z + 1;

			float z = (Camera.main.orthographicSize * 2 + 1) * ((float)Screen.width / (float)Screen.height);
			l.localScale = new Vector3(z,z,z);

			float s = scale * Mathf.Clamp((depthScale * d), 1, Mathf.Infinity);

			l.GetComponent<Renderer>().material.SetTextureScale("_MainTex", new Vector2(s, s));

			float dx = Camera.main.transform.position.x;
			float dy = Camera.main.transform.position.y;
			l.GetComponent<Renderer>().material.SetTextureOffset("_MainTex", new Vector2(dx, dy));
		}
	}
}
