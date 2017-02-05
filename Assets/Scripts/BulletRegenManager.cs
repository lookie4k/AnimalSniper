using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;

public class BulletRegenManager : MonoBehaviour {

    public GameObject bullet;

	void Start () {
        SocketManager.socket.On("bullet_regen", BulletRegen);
	}

    private void BulletRegen(SocketIOEvent e) {
        int index = (int) e.data.GetField("index").f;
        JSONObject loc = e.data.GetField("pos");
        float x = loc.GetField("x").f;
        float y = loc.GetField("y").f;

        Vector3 position = new Vector3(x, y, 8.5f);

        GameObject bullet = Instantiate(this.bullet);
        bullet.transform.position = position;
        bullet.transform.SetParent(transform);
        bullet.name = "Bullet" + index;
    }
}
