using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour {

    public int health;
    public string ani_name;

    void Update()
    {
        if (GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName(ani_name))
            Destroy(gameObject);
    }

    public void SetHealth(int health)
    {
        this.health = health;
    }

    public void Damage()
    {
        health--;
        if (health <= 0)
            Die();

    }

    private void Die()
    {
        // TODO
        Debug.Log("Die");
        if (gameObject.name.Equals(SocketManager.id))
            SocketManager.socket.Emit("die");
    }
}
