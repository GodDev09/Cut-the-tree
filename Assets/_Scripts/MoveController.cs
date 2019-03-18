using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveController : MonoBehaviour
{
    private Vector3 mousePosition;
    private Rigidbody2D rb;
    private Vector2 direction;
    public float maxSpeed;
    float speed = 0;
    public GameObject dirObj;
    public bool isActive = false;
    float delay;
    float timeDelay;
    //public GameObject fire;
    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        this.maxSpeed = Random.Range(1.65f,1.85f);
        this.transform.position = Vector3.zero;
        delay = Random.Range(0.5f, 1f);
        //GameManager.Instance.lstCutter.Add(this);
    }

    Vector3 startPosition;
    Vector3 lastPosition;
    void FixedUpdate()
    {
        if (Input.GetMouseButton(0))
        {
            //mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition = Camera.main.ScreenToViewportPoint(Input.mousePosition);
            if (startPosition == Vector3.zero)
            {
                startPosition = mousePosition;
            }
            float rot_z = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            if (this.name != "Parent")
            {
                transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);
            }           
            lastPosition = Camera.main.ScreenToViewportPoint(Input.mousePosition);
            direction = (lastPosition - startPosition).normalized;
            speed += Time.deltaTime * maxSpeed / 2;
            if (speed >= maxSpeed)
                speed = maxSpeed;
            if ((lastPosition - startPosition).normalized != Vector3.zero)//Vector3.Distance(startPosition, lastPosition) >= 1.5f)
            {
                if (timeDelay >= delay)
                {
                    if (dirObj != null && dirObj.activeSelf)
                    {
                        dirObj.transform.position = new Vector3(dirObj.transform.position.x, dirObj.transform.position.y, 0);
                        dirObj.transform.rotation = Quaternion.Euler(0f, 0f, rot_z);
                    }
                    rb.velocity = new Vector2(direction.x * speed, direction.y * speed);
                    //timeDelay = 0;
                }
                else
                {
                    timeDelay += Time.deltaTime;
                }
                
                //transform.Translate(direction.x * speed * Time.deltaTime, direction.y * speed * Time.deltaTime, transform.position.z * Time.deltaTime);
                //fire.SetActive(true);
            }
        }
        else
        {
            timeDelay = 0;
            speed = 0;
            startPosition = lastPosition = Vector3.zero;
            rb.velocity = Vector2.zero;
            if (dirObj != null && dirObj.activeSelf)
            {
                dirObj.transform.position = new Vector3(dirObj.transform.position.x, dirObj.transform.position.y, -100);
            }
            //fire.SetActive(false);
        }

        //if (this.name != "Parent")
        //{
        //    this.transform.position = Vector3.zero;
        //}
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Grass")
        {
            //float a = (((1f / 3f - 1f / 20f) * (GameManager.Instance.maxGrass - GameManager.Instance.numGrass) / GameManager.Instance.maxGrass) + 1f / 20f) * Screen.width * 0.00001f;
            //transform.localScale += new Vector3(a, a, 0);
            GameManager.Instance.spawnCutter++;
            AudioManager.Instance.PlaySaw();
        }
        else if (other.tag == "Boom")
        {
            //GameManager.Instance.lstCutter.Remove(this);
            if (isActive)
            {
                DeActive();
            }               
            //GameManager.Instance.GameOver();
            //UIManager.Instance.ShowPanelEndGame("Game Over");
        }
    }

    public void Active()
    {
        isActive = true;
        GameManager.Instance.health++;
        this.GetComponent<SpriteRenderer>().enabled = true;
        this.GetComponent<CircleCollider2D>().enabled = true;
        this.transform.position = new Vector3(this.transform.position.x + 0.01f, this.transform.position.y, this.transform.position.z);
        dirObj.SetActive(true);
    }

    public void DeActive()
    {
        isActive = false;
        this.transform.position = Vector3.zero;
        this.transform.localPosition = Vector3.zero;
        this.GetComponent<SpriteRenderer>().enabled = false;
        this.GetComponent<CircleCollider2D>().enabled = false;
        dirObj.SetActive(false);
        GameManager.Instance.GameOver();
    }
}
