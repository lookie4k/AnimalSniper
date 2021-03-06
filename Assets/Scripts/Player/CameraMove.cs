﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour {

    public SpriteRenderer background;

    private Vector3 limitPosition;

    // Use this for initialization
    void Start()
    {
        InitLimitPosition();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void LateUpdate()
    {
        Move();
    }

    public void InitLimitPosition()
    {
        Vector3 mapSize = background.bounds.extents;

        Camera mainCamera = Camera.main;
        float cameraHeight = mainCamera.orthographicSize;
        float cameraWidth = mainCamera.aspect * cameraHeight;

        limitPosition = new Vector3(mapSize.x - cameraWidth, mapSize.y - cameraHeight, transform.position.z);
    }

    private void Move()
    {
        if (MultiManager.player == null)
            return;
        Vector3 playerPosition = MultiManager.player.transform.position;

        playerPosition.z = transform.position.z;

        transform.position = ClampVector(playerPosition, -1 * limitPosition, limitPosition);
    }

    private Vector3 ClampVector(Vector3 value, Vector3 min, Vector3 max)
    {
        return Vector3.Min(Vector3.Max(value, min), max);
    }
}
