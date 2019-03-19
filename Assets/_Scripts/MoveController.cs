using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventDispatcher;

public class MoveController : MonoBehaviour
{
    private Vector3 mousePosition;
    private Rigidbody2D rb;
    private Vector2 direction;
    public float maxSpeed;
    float speed = 0;
    public GameObject dirObj;
    public bool isActive = false;
    Vector2[] arrDir;
    int delay, j;
    public int ID;
    //public GameObject fire;
    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        this.RegisterListener(EventID.START_GAME, (param) => ON_START_GAME());
        GameManager.Instance.lstCutter.Add(this);
    }

    void ON_START_GAME()
    {
        this.maxSpeed = GameConfig.Instance.MaxSpeedCutter;
        this.transform.position = Vector3.zero;
        delay = GameConfig.Instance.Delay;
        j = delay - 1;
        arrDir = new Vector2[delay];
        ID = Random.Range(0, delay);
        if (GameManager.Instance.lstCutter.IndexOf(this) == 0)
        {
            ID = 0;
        }
    }

    Vector3 startPosition;
    Vector3 lastPosition;
    void FixedUpdate()
    {
        if (GameManager.Instance.stateGame == StateGame.PLAYING)
        {
            if (Input.GetMouseButton(0))
            {
                //mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousePosition = Camera.main.ScreenToViewportPoint(Input.mousePosition);
                if (startPosition == Vector3.zero)
                {
                    startPosition = mousePosition;
                }
                float rot_z = Mathf.Atan2(arrDir[ID].y, arrDir[ID].x) * Mathf.Rad2Deg;
                if (this.name != "Parent")
                {
                    transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);
                }
                lastPosition = Camera.main.ScreenToViewportPoint(Input.mousePosition);
                direction = (lastPosition - startPosition).normalized;
                arrDir[0] = direction;
                speed += Time.deltaTime * maxSpeed / 2;
                if (speed >= maxSpeed)
                    speed = maxSpeed;
                if ((lastPosition - startPosition).normalized != Vector3.zero)//Vector3.Distance(startPosition, lastPosition) >= 1.5f)
                {
                    if (dirObj != null && dirObj.activeSelf)
                    {
                        dirObj.transform.position = new Vector3(dirObj.transform.position.x, dirObj.transform.position.y, 0);
                        dirObj.transform.rotation = Quaternion.Euler(0f, 0f, rot_z);
                    }
                    if (this.name != "Parent")
                    {
                        Move();
                    }
                    else
                    {
                        rb.velocity = new Vector2(arrDir[0].x * speed, arrDir[0].y * speed);
                    }
                }
            }
            else
            {
                speed = 0;
                startPosition = lastPosition = Vector3.zero;
                rb.velocity = Vector2.zero;
                if (dirObj != null && dirObj.activeSelf)
                {
                    dirObj.transform.position = new Vector3(dirObj.transform.position.x, dirObj.transform.position.y, -100);
                }
                //fire.SetActive(false);
            }

            while (j > 0)
            {
                arrDir[j] = arrDir[j - 1];
                j--;
            }
            j = delay - 1;
        }
    }

    void Move()
    {
        rb.velocity = new Vector2(arrDir[ID].x * speed, arrDir[ID].y * speed);
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
        else if (other.tag == "Boom" || other.tag == "Map")
        {
            //GameManager.Instance.lstCutter.Remove(this);
            if (isActive)
            {
                DeActive();
                other.gameObject.GetComponent<Boom>().Refresh();
            }
            //GameManager.Instance.GameOver();
            //UIManager.Instance.ShowPanelEndGame("Game Over");
        }
    }

    public void Active()
    {
        if (this.name != "Parent")
        {
            isActive = true;
            GameManager.Instance.health++;
            this.GetComponent<SpriteRenderer>().enabled = true;
            this.GetComponent<CircleCollider2D>().enabled = true;
            this.transform.position = new Vector3(this.transform.position.x + 0.01f, this.transform.position.y, this.transform.position.z);
            dirObj.SetActive(true);
        }
    }

    public void DeActive()
    {
        if (this.name != "Parent")
        {
            isActive = false;
            this.PostEvent(EventID.CUTTER_DEACTIVE);
            this.transform.position = Vector3.zero;
            this.transform.localPosition = Vector3.zero;
            this.GetComponent<SpriteRenderer>().enabled = false;
            this.GetComponent<CircleCollider2D>().enabled = false;
            dirObj.SetActive(false);
            GameManager.Instance.GameOver();
        }
    }
}
