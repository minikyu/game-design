using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    
    public KeyCode keyRight;
    public KeyCode keyLeft;
	public AngleController angleController;

    //// current velocity
    private float horizontalVel = 0.0f;
    private float forwardVel = 3.0f;


    void Start()
    {
		angleController = this.transform.GetComponent<AngleController>();
    }

    void Update()
    {
        //// control with keyboard
        if (Input.GetKey(keyLeft))
            horizontalVel = -3.0f;
        else if (Input.GetKey(keyRight))
            horizontalVel = +3.0f;
        else
            horizontalVel = 0.0f;
		//// TODO: Your Implementation:
		//// - Update the horizontal velocity with angleController
		forwardVel += 0.001f;
		horizontalVel = angleController.movingSpeed;

        //// When not dead, update velocity
        if (!GameManager.Instance.IsDead()) {
            this.transform.GetComponent<Rigidbody>().velocity = new Vector3(horizontalVel, 0.0f, forwardVel);
        }
    }
    
    void OnTriggerEnter(Collider other) {
		//// TODO: Your Implementation:
		//// - When collide with obj with tag 'CollisionWall' or 'FallWall', trigger OnDeath() in GameManager
		if (other.gameObject.CompareTag("CollisionWall") || other.gameObject.CompareTag("FallWall"))
		{
			GameObject.Find("GameManager").GetComponent<GameManager>().OnDeath(true);
		}
	}
}
