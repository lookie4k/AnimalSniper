using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;

public class MultiManager : MonoBehaviour {

    public static GameObject player;
    public GameObject[] characters;
    public Gun gunManager;
    public Zoom zoomManager;

    private bool aleadyPlayer;

    private Dictionary<string, GameObject> dictionary = new Dictionary<string, GameObject>();

    void Awake()
    {
        AddTable();
        //GameObject gameObj = dictionary[SocketManager.characterInfo.GetField("type").str];

        //player = Instantiate(gameObj);
        //player.name = "Character";

        SocketManager.socket.On("connect_user", ConnectPlayer);
        SocketManager.socket.On("disconnect", DisconnectPlayer);
        SocketManager.socket.On("id", SetPlayers);
    }

    private void AddTable()
    {
        dictionary.Add("BEAR", characters[0]);
        dictionary.Add("CAT", characters[1]);
        dictionary.Add("DOG", characters[2]);
    }

    private void ConnectPlayer(SocketIOEvent e)
    {
        if (e.data.GetField("id").str.Equals(SocketManager.id))//자신일때
        {
            JSONObject player = e.data.GetField("player");
            string type = player.GetField("type").str;

            MultiManager.player = Instantiate(characters[StringToType(type)]);
            MultiManager.player.name = e.data.GetField("id").str;
            PlayerMove playerMove = MultiManager.player.GetComponent<PlayerMove>();
            Transform fireLocation = MultiManager.player.transform.GetChild(0);

            gunManager.FireSetUp(fireLocation);
            zoomManager.ZoomSetUp(playerMove);
        }
        else  //다른유저일때
        {
            CreatePlayer(e.data);
        }
    }

    private void DisconnectPlayer(SocketIOEvent e)
    {
        Destroy(GameObject.Find(e.data.GetField("id").str));
    }

    private GameObject CreatePlayer(JSONObject data)
    {
        JSONObject player = data.GetField("player");
        JSONObject location = data.GetField("loc");
        JSONObject angle = data.GetField("angle");
        JSONObject animation = data.GetField("ani");

        string type = player.GetField("type").str;
        Vector3 position = new Vector3(location.GetField("x").f, location.GetField("y").f);

        GameObject gameObj = Instantiate(characters[StringToType(type)]);
        gameObj.name = data.GetField("id").str;
        PlayerMove playerMove = gameObj.GetComponent<PlayerMove>();
        playerMove.isMultiPlayer = true;
        playerMove.SetPosition(position);
        playerMove.SetRotation(angle.GetField("x").f, angle.GetField("y").f);
        playerMove.SetAnimation(animation.GetField("run"));

        return gameObj;
    }

    private void SetPlayers(SocketIOEvent e)
    {
        List<JSONObject> players = e.data.GetField("players").list;
        foreach (JSONObject player in players)
        {
            if (player.GetField("id").str.Equals(SocketManager.id))
                return;
            CreatePlayer(player);
        }
    }

    private int StringToType(string type)
    {
        switch (type)
        {
            case "BEAR":
                return 0;
            case "CAT":
                return 1;
            case "DOG":
                return 2;
        }
        return -1;
    }
}
