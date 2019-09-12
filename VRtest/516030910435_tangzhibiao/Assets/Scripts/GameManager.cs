using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : UnitySingleton<GameManager>
{
    public GameObject player;
	public DeathMenu deathMenu;
	public TileManager tileManager;

    private bool isDead;
	private int score;
	public Text scoretext;
    void Start()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        isDead = false;
		score = 0;
	}

    void Update()
    {
		if (!isDead) {
			//// TODO: Your Implementation:
			//// 1. update score (Hint: you can use running time as the score)
			score += 1;
			//// 2. show score (Hint: show in Canvas/CurrentScore/Text)
			scoretext.text = "Current Score: " + score;
        }
        else if (Input.touchCount > 0 || Input.GetKeyDown(KeyCode.Mouse0)) {
            Restart();
        }
    }

    public bool IsDead() {
        return isDead;
    }
	
    public void OnDeath(bool collision){
        isDead = true;
        print("GameOver");

		//// TODO: Your Implementation:

		//// 2. stop player
		isDead = true;
		
		
		//// 3. hide all tiles (Hint: call function in TileManager.cs)
		tileManager.hideAll();
		//// 1. show DeathMenu (Hint: you can use Show() in DeathMenu.cs)
		//// 4. record high score (Hint: use PlayerPrefs)
		if (PlayerPrefs.GetInt("highest") < score)
		{
			PlayerPrefs.SetInt("highest", score);
		}
		deathMenu.Show(score,PlayerPrefs.GetInt("highest"));
		
    }

    public void Restart(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
