using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveController : MonoBehaviour
{
    private Vector3 mousePosition;
    private Rigidbody2D rb;
    private Vector2 direction;
    private float moveSpeed = 15f;
    float speed;
    public GameObject dirObj;
    public GameObject fire;
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
            if ((lastPosition - startPosition).normalized != Vector3.zero)//Vector3.Distance(startPosition, lastPosition) >= 1.5f)
            {
                dirObj.transform.position = new Vector3(dirObj.transform.position.x, dirObj.transform.position.y, 0);
                rb.velocity = new Vector2(direction.x * speed, direction.y * speed);
                //dirObj.transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);
                //fire.SetActive(true);
            }

        }
        else
        {
            speed = 0;
            startPosition = lastPosition = Vector3.zero;
            rb.velocity = Vector2.zero;
            dirObj.transform.position = new Vector3(dirObj.transform.position.x, dirObj.transform.position.y, -100);
            //fire.SetActive(false);
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Grass")
        {
            float a = (((1f / 3f - 1f / 20f) * (GameManager.Instance.maxGrass - GameManager.Instance.numGrass) / GameManager.Instance.maxGrass) + 1f / 20f) * Screen.width * 0.00001f;
            transform.localScale += new Vector3(a, a, 0);
        }
        else if (other.tag == "Boom")
        {
            Debug.Log("Over");
            //GameManager.Instance.GameOver();
            UIManager.Instance.ShowPanelEndGame("Game Over");
        }
    }
}
