using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTrailEffect : GunFireEffect
{
    public int maxCount;
    public BulletTrail prefab;

    private List<BulletTrail> storage;

    void Start()
    {
        SetBulletTrails();
    }

    private void SetBulletTrails()
    {
        Transform storageObject = new GameObject("BulletTrails").transform;

        storage = new List<BulletTrail>();
        for (int i = 0; i < maxCount; i++)
        {
            GameObject gameObj = Instantiate(prefab.gameObject);
            gameObj.transform.SetParent(storageObject);
            gameObj.SetActive(false);

            storage.Add(gameObj.GetComponent<BulletTrail>());
        }
    }

    public override void OnEffect(Ray2D ray, RaycastHit2D rayHit)
    {
        BulletTrail bulletTrail = storage.Find(obj => !obj.gameObject.activeInHierarchy);

        if (bulletTrail == null)
            return;

        GameObject gameObj = bulletTrail.gameObject;

        float angle = Mathf.Atan2(ray.direction.y, ray.direction.x) * Mathf.Rad2Deg;
        gameObj.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        gameObj.transform.position = ray.origin;

        gameObj.SetActive(true);

        StartCoroutine(DisableObject(gameObj, 3f));
    }

    private IEnumerator DisableObject(GameObject gameObj, float delay)
    {
        yield return new WaitForSeconds(delay);
        gameObj.SetActive(false);
    }
}
