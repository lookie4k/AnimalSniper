using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Bullet : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag != "Player")
            return;
        Magazine magazine = col.gameObject.GetComponent<Magazine>();
        if (!magazine.Increase())
            return;

        int index = Int32.Parse(gameObject.name.Replace("Bullet", ""));

        Destroy(gameObject);

        JSONObject json = new JSONObject();
        json.AddField("index", index);

        SocketManager.socket.Emit("pick_bullet", json);
    }
}
