using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using SocketIO;

public class StartScene : MonoBehaviour {

    private Type type;
    private InputField inputField;

    void Start()
    {
        type = Type.NULL;
        inputField = transform.GetChild(2).GetComponent<InputField>();
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

        SocketManager.socket.On("id", GetId);
        SocketManager.socket.Emit("connect", json);
        SocketManager.characterInfo = json;

        SceneManager.LoadScene("Game");
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
