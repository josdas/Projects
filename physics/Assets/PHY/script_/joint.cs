using UnityEngine;
using System.Collections;

public class joint : MonoBehaviour {
	public GameObject target;
	private Vector3 direction;
	// Use this for initialization
	void Start () {
		direction = gameObject.transform.position - target.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		gameObject.transform.position = direction + target.transform.position;
	}
}
