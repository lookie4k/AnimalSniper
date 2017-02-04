using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using SocketIO;

public class PlayerMove : MonoBehaviour
{

    private Rigidbody2D playerRigidbody;
    private Animator playerAnimator;

    public float speed;
    public float angle;

    public bool isMultiPlayer;

    public Vector2 vector;

    private bool toggle;
    private bool toggle_run;

    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();

        vector = Vector2.up;

        SocketManager.socket.On("multi", MultiMove);
    }

    void Update()
    {
        if (isMultiPlayer)
            return;

        Move();
        Rotate();
    }

    private void Move()
    {
        float x = CrossPlatformInputManager.GetAxisRaw("MoveHorizontal");
        float y = CrossPlatformInputManager.GetAxisRaw("MoveVertical");

        Vector3 vector = new Vector3(x, y);
        vector.Normalize();

        Vector3 pos = transform.position;
        playerRigidbody.velocity = vector * speed;

        JSONObject json = new JSONObject();

        if (vector.sqrMagnitude > 0)
        {
            if (!toggle)
            {
                JSONObject aniJson = new JSONObject();
                aniJson.AddField("run", true);

                SocketManager.socket.Emit("ani_run", aniJson);

                toggle = !toggle;
            }
            json.AddField("x", playerRigidbody.transform.position.x);
            json.AddField("y", playerRigidbody.transform.position.y);

            SocketManager.socket.Emit("move", json);
        }
        else
        {
            if (toggle)
            {
                JSONObject aniJson = new JSONObject();
                aniJson.AddField("run", false);
                SocketManager.socket.Emit("ani_run", aniJson);

                json.AddField("x", playerRigidbody.transform.position.x);
                json.AddField("y", playerRigidbody.transform.position.y);
                SocketManager.socket.Emit("move", json);

                toggle = !toggle;
            }
        }
        playerRigidbody.position = pos;
    }

    private void Rotate()
    {
        float x = CrossPlatformInputManager.GetAxis("AimHorizontal");
        float y = CrossPlatformInputManager.GetAxis("AimVertical");

        if (x == 0 && y == 0)//180
            return;

        JSONObject json = new JSONObject();
        json.AddField("x", x);
        json.AddField("y", y);

        SocketManager.socket.Emit("rotate", json);
    }

    private void MultiMove(SocketIOEvent e)
    {
        if (e.data.GetField("id").str.Equals(gameObject.name))
            Move(e);
    }

    private void Move(SocketIOEvent e)
    {
        JSONObject player = e.data.GetField("player");
        JSONObject location = e.data.GetField("loc");
        JSONObject angle = e.data.GetField("angle");
        JSONObject animation = e.data.GetField("ani");

        float locX = location.GetField("x").f;
        float locY = location.GetField("y").f;
        float angleX = angle.GetField("x").f;
        float angleY = angle.GetField("y").f;
        bool ani_run = animation.GetField("run").b;

        SetPosition(new Vector3(locX, locY));
        SetRotation(angleX, angleY);
        SetAnimation(ani_run);
    }

    public void SetPosition(Vector3 vector)
    {
        if (isMultiPlayer)
        {
            playerRigidbody.position = new Vector3(vector.x, vector.y);
            transform.position = new Vector3(transform.position.x, transform.position.y, 8.5f);
            return;
        }
        playerRigidbody.position = vector;
    }

    public void SetRotation(float angleX, float angleY)
    {
        float rotation = Mathf.Atan2(-angleX, angleY) * Mathf.Rad2Deg;
        angle = Mathf.Atan2(angleX, angleY) * Mathf.Rad2Deg;
        vector = new Vector2(angleX, angleY);
        transform.rotation = Quaternion.AngleAxis(rotation, Vector3.forward);
    }

    public void SetAnimation(bool ani_run)
    {
        if (!toggle_run)
        {
            toggle_run = ani_run;
            playerAnimator.SetBool("run", toggle_run);
        }
        else
        {
            toggle_run = ani_run;
            playerAnimator.SetBool("run", toggle_run);
        }
    }
}
