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
    public float fireDelay;
    private bool isFire;

    public Transform fireTransform;
    public GunFireEffect[] fireEffects;

    public Magazine magazine;
    public PlayerMove playerMove;
    public ScoreManager scoreManager;

    public void FireSetUp(Transform fireLocation)
    {
        fireTransform = fireLocation.transform;
        magazine = fireTransform.GetComponentInParent<Magazine>();
        playerMove = fireTransform.GetComponentInParent<PlayerMove>();

        SocketManager.socket.On("fire", FirePlayer);
    }

    public void Fire()
    {
        if (isFire)
            return;
        if (!magazine.IsRemaining())
        {
            GameObject.Find(SocketManager.id).transform.GetChild(2).GetComponent<PlayerSound>().PlaySound(3, 5f, 5f);
            return;
        }
        isFire = !isFire;

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
        string id = e.data.GetField("id").str;
        JSONObject data = e.data.GetField("data");

        JSONObject posJson = data.GetField("pos");
        JSONObject angleJson = data.GetField("angle");

        float posX = posJson.GetField("x").f;
        float posY = posJson.GetField("y").f;
        float angleX = angleJson.GetField("x").f;
        float angleY = angleJson.GetField("y").f;

        Ray2D ray = new Ray2D(new Vector2(posX, posY), new Vector2(angleX, angleY));
        RaycastHit2D rayHit = Physics2D.Raycast(ray.origin, ray.direction, 1000f);
        SoundManager.GetInstance().PlaySound(6, 1f, 2f, new Vector3(posX, posY, 0));

        if (SocketManager.id.Equals(id))
        {
            scoreManager.AddScore(5);
            StartCoroutine(PlayReloadSound());
        }

        for (int i = 0; i < fireEffects.Length; i++)
            fireEffects[i].OnEffect(ray, rayHit);

        if (rayHit.collider == null || rayHit.collider.tag == "Obstacle" || rayHit.collider.tag != "Player")
            return;

        PlayerHealth enemy = rayHit.collider.GetComponent<PlayerHealth>();
        enemy.Damage();

        if (SocketManager.id.Equals(id))
            scoreManager.AddScore(5);
    }

    private IEnumerator PlayReloadSound()
    {
        yield return new WaitForSeconds(1.5f);
        GameObject.Find(SocketManager.id).transform.GetChild(2).GetComponent<PlayerSound>().PlaySound(0, 0.5f, 0.5f);

        JSONObject json = new JSONObject();
        json.AddField("id", SocketManager.id);
        json.AddField("index", 0);
        json.AddField("min", 0.1f);
        json.AddField("max", 0.1f);

        SocketManager.socket.Emit("player_sound", json);
        yield return new WaitForSeconds(fireDelay);
        isFire = !isFire;
    }
}
