using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerMove : MonoBehaviour {

    private Rigidbody2D playerRigidbody;
    private Vector3 defaultLoc;

    public float speed;
    public RectTransform aimJoystick;

    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
        defaultLoc = (aimJoystick.position - new Vector3(0, 0, 0));
    }

    void Update () {
        Move();
        Rotate();
    }

    private void Move()
    {
        float x = CrossPlatformInputManager.GetAxisRaw("MoveHorizontal");
        float y = CrossPlatformInputManager.GetAxisRaw("MoveVertical");

        Vector3 vector = new Vector3(x, y);

        playerRigidbody.velocity = vector * speed;
    }

    private void Rotate()
    {
        Vector3 distance = aimJoystick.position - defaultLoc;
        float angle = Mathf.Atan2(distance.y, distance.x) * Mathf.Rad2Deg;
        if (angle == 0)
            return;
        transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
    }
}
