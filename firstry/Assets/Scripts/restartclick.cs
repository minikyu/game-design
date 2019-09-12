using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class restartclick : MonoBehaviour
{
    public void onClick()
	{
		SceneManager.LoadScene(0);
	}
}
