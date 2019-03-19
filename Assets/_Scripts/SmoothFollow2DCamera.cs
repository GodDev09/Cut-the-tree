using System.Collections;
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
    public float maxX;
    public float minX;
    public float maxY;
    public float minY;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //if (transform.position.x < 3.3f && transform.position.x > -3.3f && transform.position.y < 4 && transform.position.y > -7.5f)
        //{
            Vector3 position = target.position + offset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, position, smoothSpeed);
            transform.position = new Vector3(Mathf.Clamp(smoothedPosition.x, minX, maxX), Mathf.Clamp(smoothedPosition.y, minY, maxY), smoothedPosition.z);
            //transform.LookAt(target);
        //}
    }
}
