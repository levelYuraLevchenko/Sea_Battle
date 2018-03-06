using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public void Camera()
    {
        Camera cam = GetComponent<Camera>();
        cam.orthographicSize = 4;
    }
}
