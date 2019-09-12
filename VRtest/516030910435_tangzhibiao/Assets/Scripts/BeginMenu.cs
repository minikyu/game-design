using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BeginMenu : MonoBehaviour
{

	void Start()
	{

	}
	void Update()
	{
		if (Input.touchCount > 0 || Input.GetKeyDown(KeyCode.Mouse0))
		{
			SceneManager.LoadScene("Game");
		}
	}

}
