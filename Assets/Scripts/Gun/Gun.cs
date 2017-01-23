using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public long fireDelay;

    private Magazine magazine;
    private long lastFireTime;

    private const long TO_SECOND = 10000000;

    void Start()
    {
        magazine = GetComponent<Magazine>();
        lastFireTime = 0;
    }

    void Update()
    {
    }

    public void Fire()
    {
        if (System.DateTime.Now.Ticks - lastFireTime < fireDelay * TO_SECOND)
            return;
        if (!magazine.IsRemaining())
            return;
        lastFireTime = System.DateTime.Now.Ticks;
        magazine.Decrease();
        Debug.Log("Fire");
    }
}
