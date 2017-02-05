using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using SocketIO;

public class SceneUI : MonoBehaviour {

    private Type type;
    private InputField inputField;

    void Start()
    {
        if (SceneManager.GetActiveScene().name.Equals("Start"))
        {
            type = Type.NULL;
            inputField = transform.GetChild(2).GetComponent<InputField>();
        }


        if (SceneManager.GetActiveScene().name.Equals("Gameover"))
            ScoreManager.DisplayScore();
    }

    public void RePlay()
    {
        Destroy(GameObject.Find("SocketManager"));
        Destroy(GameObject.Find("SoundManager"));
        Destroy(GameObject.Find("ScoreManager"));
        SceneManager.LoadScene("Start");
    }

    public void SelectDog()
    {
        type = Type.DOG;
    }

    public void SelectCat()
    {
        type = Type.CAT;
    }

    public void SelectBear()
    {
        type = Type.BEAR;
    }

    public void GameStart()
    {
        if (!ExistName() || !IsSelect())
            return;
        JSONObject json = new JSONObject();
        json.AddField("name", inputField.text);
        json.AddField("type", type.ToString());

        SocketManager.socket.On("get_id", GetId);
        SocketManager.characterInfo = json;
        SocketManager.socket.Emit("get_id");

        SceneManager.LoadScene("Game");

        SoundManager.GetInstance().PlayBackgroundSound();
    }

    private void GetId(SocketIOEvent e)
    {
        SocketManager.id = e.data.GetField("id").str;
    }

    private bool ExistName()
    {
        return inputField.text != "";
    }

    private bool IsSelect()
    {
        return type != Type.NULL;
    }

    private enum Type
    {
        DOG, CAT, BEAR, NULL
    }
}
