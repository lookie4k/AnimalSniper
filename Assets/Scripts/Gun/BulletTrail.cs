using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTrail : MonoBehaviour
{

    public float speed;
    private Rigidbody2D bulletRigidbody;

    void Start()
    {
        bulletRigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        bulletRigidbody.position += (Vector2) transform.right * speed * Time.deltaTime;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Obstacle" || col.gameObject.tag == "Player")
            gameObject.SetActive(false);
    }
}
