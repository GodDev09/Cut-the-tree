﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothFollow2DCamera : MonoBehaviour
{
    [Header("Camera settings")]
    [Tooltip("Reference to the target GameObject")]
    public Transform target;
    [Tooltip("Current relative offset to the target")]
    public Vector3 offset;
    [Tooltip("Multiplier for the movement speed")]
    [Range(0f, 5f)]
    public float smoothSpeed = 0.125f;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void LateUpdate()
    {
        //if (transform.position.x < 3.3f && transform.position.x > -3.3f && transform.position.y < 4 && transform.position.y > -7.5f)
        //{
            Vector3 position = target.position + offset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, position, smoothSpeed);
            transform.position = new Vector3(Mathf.Clamp(smoothedPosition.x, -3.3f, 3.3f), Mathf.Clamp(smoothedPosition.y, -7.5f, 4f), smoothedPosition.z);
            //transform.LookAt(target);
        //}
    }
}