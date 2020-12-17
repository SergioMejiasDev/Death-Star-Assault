﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script that manages the rotation of the camera in the main menu.
/// </summary>
public class CameraRotation : MonoBehaviour
{
    void Update()
    {
        transform.Rotate(Vector3.up*-2.0f*Time.deltaTime);
    }
}
