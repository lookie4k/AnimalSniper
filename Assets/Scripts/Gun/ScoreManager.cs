using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour {

    public Text scoreText;

    private static int score;
    private static int bestScore;

	void Start () {
        DontDestroyOnLoad(gameObject);

        SocketManager.socket.On("score", UpdateScore);
	}
	

    public void AddScore(int amount)
    {
        JSONObject score = new JSONObject();
        score.AddField("score", amount);
        SocketManager.socket.Emit("score", score);
    }

    private void UpdateScore(SocketIOEvent e)
    {
        int serverScore = (int) e.data.GetField("score").f;
        score = serverScore;
        if (score > PlayerPrefs.GetInt("BestScore", 0))
            PlayerPrefs.SetInt("BestScore", score);
        SetDisplayScore(score);
    }

    private void SetDisplayScore(int score)
    {
        scoreText.text = "Score: " + score;
    }

    public static void DisplayScore()
    {
        Text bestScore = GameObject.Find("BestScore").GetComponent<Text>();
        Text yourScore = GameObject.Find("YourScore").GetComponent<Text>();

        bestScore.text = "Best Score: " + PlayerPrefs.GetInt("BestScore");
        yourScore.text = "Your Score: " + score;
    }

    public int GetScore()
    {
        return score;
    }
}
