using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;

public class SocketManager : MonoBehaviour {

    public static string id;
    public static SocketIOComponent socket;
    public static JSONObject characterInfo;

    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
        GameObject go = GameObject.Find("SocketIO");
        DontDestroyOnLoad(go);
        socket = go.GetComponent<SocketIOComponent>();

        StartCoroutine(CheckConnection());
    }

    void OnDisable()
    {
        socket.Close();
        Debug.Log("Socket Died");
    }

    private IEnumerator CheckConnection()
    {
        while (true)
        {
            if (!socket.IsConnected) {
                /*if (EditorUtility.DisplayDialog("Place Selection On Surface?", "Are you sure you want to place on the surface?", "Place", "Do Not Place")) {
                    
                } else
                {

                }*/
            }
            yield return null;
        }
    }
}
