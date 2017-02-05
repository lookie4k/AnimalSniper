using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zoom : MonoBehaviour
{
    public float fov;

    public float zoomInSpeed;
    public float zoomInLight;
    public float zoomInAngle;

    public PlayerMove player;
    private Light playerLight;

    private float defaultSpeed;
    private float defaultLight;
    private float defaultAngle;

    private bool zoomStatus;
    private float defaultSize;

    public void ZoomSetUp(PlayerMove playerMove)
    {
        player = playerMove;
        playerLight = player.transform.GetComponentInChildren<Light>();
        defaultSpeed = player.speed;
        defaultLight = playerLight.range;
        defaultAngle = playerLight.spotAngle;

        defaultSize = Camera.main.orthographicSize;
        zoomStatus = false;
    }

    public void OnZoom()
    {
        if (zoomStatus)
            ZoomOut();
        else
            ZoomIn();

        player.transform.GetComponentInChildren<PlayerSound>().PlaySound(2, 5f, 5f);
    }

    private void ZoomIn()
    {
        player.speed = zoomInSpeed;
        zoomStatus = !zoomStatus;

        StopAllCoroutines();
        StartCoroutine(StartZoom());
    }

    private IEnumerator StartZoom()
    {
        StartCoroutine(LightOn());
        StartCoroutine(AngleOn());

        for (float i = Camera.main.orthographicSize; i <= fov; i += 0.1f)
        {
            Camera.main.orthographicSize = i;
            Camera.main.GetComponent<CameraMove>().InitLimitPosition();
            yield return new WaitForSeconds(0.001f);
        }
        yield return null;
    }

    private void ZoomOut()
    {
        player.speed = defaultSpeed;
        zoomStatus = !zoomStatus;

        StopAllCoroutines();
        StartCoroutine(StopZoom());
    }

    private IEnumerator StopZoom()
    {
        StartCoroutine(LightOff());
        StartCoroutine(AngleOff());

        for (float i = Camera.main.orthographicSize; i >= defaultSize; i -= 0.1f)
        {
            Camera.main.orthographicSize = i;
            Camera.main.GetComponent<CameraMove>().InitLimitPosition();
            yield return new WaitForSeconds(0.001f);
        }
        yield return null;
    }

    private IEnumerator LightOn()
    {
        for (float i = playerLight.range; i <= zoomInLight; i++)
        {
            playerLight.range = i;
            yield return new WaitForSeconds(0.05f);
        }
        yield return null;
    }

    private IEnumerator LightOff()
    {
        for (float i = playerLight.range; i >= defaultLight; i--)
        {
            playerLight.range = i;
            yield return new WaitForSeconds(0.05f);
        }
        yield return null;
    }

    private IEnumerator AngleOn()
    {
        for (float i = playerLight.spotAngle; i <= zoomInAngle; i++)
        {
            playerLight.spotAngle = i;
            yield return new WaitForSeconds(0.1f);
        }
        yield return null;
    }

    private IEnumerator AngleOff()
    {
        for (float i = playerLight.spotAngle; i >= defaultAngle; i--)
        {
            playerLight.spotAngle = i;
            yield return new WaitForSeconds(0.1f);
        }
        yield return null;
    }
}
