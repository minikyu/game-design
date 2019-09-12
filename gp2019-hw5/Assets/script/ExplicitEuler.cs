using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplicitEuler : MonoBehaviour
{
	// Start is called before the first frame update
	public float len = 5.0f;
	private float delatime;
	private float cur_r;
	private float next_r;
	private float gravity = 9.81f;
	private float w;
	private Rigidbody rb;

	void Start()
	{
		rb = GetComponent<Rigidbody>();
		cur_r = 15.0f / 180.0f * Mathf.PI;
		w = 0.0f;
		rb.transform.position = new Vector3(-len * Mathf.Sin(cur_r), -len * Mathf.Cos(cur_r), 0);

	}

	// Update is called once per frame
	void FixedUpdate()
	{
		delatime = Time.deltaTime;

		next_r = cur_r + w * delatime;
		w = w - gravity / len * Mathf.Sin(cur_r) * delatime;

		cur_r = next_r;
		rb.transform.position = new Vector3(-len * Mathf.Sin(cur_r), -len * Mathf.Cos(cur_r), 0);

	}
}
