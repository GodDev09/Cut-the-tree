using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveController : MonoBehaviour
{
    private Vector3 mousePosition;
    private Rigidbody2D rb;
    private Vector2 direction;
    private float moveSpeed = 20f;
    float speed;
    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    Vector3 startPosition;
    Vector3 lastPosition;
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (startPosition == Vector3.zero)
            {
                startPosition = mousePosition;
            }
            direction = (mousePosition - transform.position).normalized;
            float rot_z = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);
            lastPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            speed += Time.deltaTime * moveSpeed;
            if (speed >= moveSpeed)
                speed = moveSpeed;
            if (Vector3.Distance(startPosition, lastPosition) >= 1.5f)//(lastPosition - startPosition).normalized != Vector3.zero)
            {
                rb.velocity = new Vector2(direction.x * speed, direction.y * speed);
            }
        }
        else
        {
            startPosition = lastPosition = Vector3.zero;
            rb.velocity = Vector2.zero;
        }
    }
}
