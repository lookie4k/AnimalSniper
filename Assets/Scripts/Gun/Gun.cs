using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour {

    public float fireDelay;
    private Magazine magazine;

	// Use this for initialization
	void Start () {
        magazine = GetComponent<Magazine>();
        StartCoroutine(FireDelay());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private IEnumerator FireDelay()
    {
        //WaitForSeconds delay = new WaitForSeconds(fireDelay);
        while (true)
        {
            if (Input.GetButton("Fire1") && magazine.IsRemaining())
            {
                Fire();
                yield return new WaitForSeconds(fireDelay);
            }
            yield return null;
        }
    }

    private void Fire()
    {
        magazine.Decrease();
        Debug.Log("Fire");
    }
}
