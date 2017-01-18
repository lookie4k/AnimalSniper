using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour {

    private Rigidbody2D playerRigidbody;

    public float speed;

    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
    }

    void Update () {
        Move();
        Rotate();
    }

    private void Move()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        Vector3 vector = new Vector3(x, y);

        vector.Normalize();

        playerRigidbody.velocity = vector * speed;
    }

    private void Rotate()
    {
        // TODO   after joystick complete
    }
}
