using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;

public abstract class GunFireEffect : MonoBehaviour
{
    public abstract void OnEffect(Ray2D ray, RaycastHit2D rayHit);
}

public class Gun : MonoBehaviour
{
    public long fireDelay;

    public Transform fireTransform;
    public GunFireEffect[] fireEffects;

    public Magazine magazine;
    public PlayerMove playerMove;
    public long lastFireTime;

    private const long TO_SECOND = 10000000;

    /*void Start()
    {
        fireTransform = MultiManager.fireLocation.transform;
        magazine = fireTransform.GetComponentInParent<Magazine>();
        playerMove = fireTransform.GetComponentInParent<PlayerMove>();
        lastFireTime = 0;

        SocketManager.socket.On("fire", FirePlayer);
    }*/

    public void FireSetUp(Transform fireLocation)
    {
        fireTransform = fireLocation.transform;
        magazine = fireTransform.GetComponentInParent<Magazine>();
        playerMove = fireTransform.GetComponentInParent<PlayerMove>();
        lastFireTime = 0;

        SocketManager.socket.On("fire", FirePlayer);
    }

    public void Fire()
    {
        long currentTime = System.DateTime.Now.Ticks;
        if (currentTime - lastFireTime < fireDelay * TO_SECOND)
            return;
        if (!magazine.IsRemaining())
            return;
        lastFireTime = currentTime;
        magazine.Decrease();

        JSONObject json = new JSONObject();
        JSONObject posJson = new JSONObject();
        JSONObject angleJson = new JSONObject();

        posJson.AddField("x", fireTransform.position.x);
        posJson.AddField("y", fireTransform.position.y);
        angleJson.AddField("x", playerMove.vector.x);
        angleJson.AddField("y", playerMove.vector.y);
        json.AddField("pos", posJson);
        json.AddField("angle", angleJson);

        SocketManager.socket.Emit("fire", json);
    }

    private void FirePlayer(SocketIOEvent e)
    {
        JSONObject posJson = e.data.GetField("pos");
        JSONObject angleJson = e.data.GetField("angle");

        float posX = posJson.GetField("x").f;
        float posY = posJson.GetField("y").f;
        float angleX = angleJson.GetField("x").f;
        float angleY = angleJson.GetField("y").f;

        Ray2D ray = new Ray2D(new Vector2(posX, posY), new Vector2(angleX, angleY));
        RaycastHit2D rayHit = Physics2D.Raycast(ray.origin, ray.direction, 20f);

        for (int i = 0; i < fireEffects.Length; i++)
            fireEffects[i].OnEffect(ray, rayHit);

        if (rayHit.collider == null || rayHit.collider.tag != "Player")
            return;

        PlayerHealth enemy = rayHit.collider.GetComponent<PlayerHealth>();
        enemy.Damage();
    }
}
