using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour {

    public int health;

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
    }
}
