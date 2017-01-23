using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerMove : MonoBehaviour {

    private Rigidbody2D playerRigidbody;
    private Animation playerAnimation;

    public float speed;

    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
        playerAnimation = GetComponent<Animation>();
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
        vector.Normalize();

        playerRigidbody.velocity = vector * speed;
    }

    private void Rotate()
    {
        float x = CrossPlatformInputManager.GetAxis("AimHorizontal") * -1;
        float y = CrossPlatformInputManager.GetAxis("AimVertical");

        float angle = Mathf.Atan2(x, y) * Mathf.Rad2Deg;
        if (angle == 180 && x == 0 && y == 0)
            return;

        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
}
