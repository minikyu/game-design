using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SphereController : MonoBehaviour
{
	private Rigidbody rb;
	public float speed;
	public float count;
	public float time;

	public Text countText;
	public Text winText;
	public Text timeText;

	public Button restart;
	public Button quit;


	// Start is called before the first frame update
	void Start()
	{
		rb = GetComponent<Rigidbody>();
		count = 0;
		time = 60;
		winText.text = "";
		countText.text = "Coins collected: " + count;
		timeText.text = "Left time: " + time;
		InvokeRepeating("Time_count", 2.0f, 1.0F);
		restart.gameObject.SetActive(false);
		quit.gameObject.SetActive(false);
		Time.timeScale = 1;
	}

	// Update is called once per frame
	void FixedUpdate()
	{
		float horiInput = Input.GetAxis("Horizontal");
		float vertiInput = Input.GetAxis("Vertical");

		Vector3 movement = new Vector3(horiInput, 0, vertiInput);

		rb.AddForce(movement * speed);
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Coins"))
		{
			float x = Random.Range(-9, 9);
			float z = Random.Range(-9, 9);

			other.gameObject.transform.position=new Vector3(x,0.5f,z);
			++count;
			countText.text = "Coins collected: " + count;
		}
	}

	void Time_count()

	{
		if (time > 0)
		{
			time--;
			timeText.text = "Left time: " + time;
		}
		else
		{
			winText.text = "Your final score: " + count;
			CancelInvoke();
			restart.gameObject.SetActive(true);
			quit.gameObject.SetActive(true);
			Time.timeScale = 0;
		}
	}
}
