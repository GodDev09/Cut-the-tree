using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventDispatcher;
public class Grass : MonoBehaviour
{
    public Rigidbody2D rigidbody;
    CircleCollider2D box;
    float maxSize = 3;
    bool isActive = false;
    void OnSpawned()
    {
        this.RegisterListener(EventID.START_GAME, (param) => ON_START_GAME());
        this.RegisterListener(EventID.GAME_OVER, (param) => ON_OVER());
        isActive = true;
        box = GetComponent<CircleCollider2D>();
        transform.localScale = new Vector3(maxSize / 20, maxSize / 20, 1);
        Invoke("LocLock", 2f);
        //this.GetComponent<SpriteRenderer>().sortingLayerName = "Grass";
    }

    void OnDespawned()
    {

    }

    void Update()
    {
        //if (GameManager.Instance.stateGame == StateGame.PLAYING)
        //{
        //    if (transform.position.x > 2.7f || transform.position.x < -2.7f || transform.position.y > 4.7f || transform.position.y < -4.7f)
        //    {
        //        rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
        //    }
        //}
    }

    void ON_START_GAME()
    {
        box.isTrigger = false;
        //GameManager.Instance.lstGrass.Add(this);
        
        Invoke("LocLock", 2f);
    }

    void ON_OVER()
    {
        DeActive();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Cutter")
        {
            if (isActive)
            {
                isActive = false;
                GameManager.Instance.numGrass--;
                DeActive();
            }
        }
    }

    public void DeActive()
    {
        EZ_Pooling.EZ_PoolManager.Despawn(this.transform);
    }

    void LocLock()
    {
        //rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
        box.isTrigger = true;
        this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, Random.Range(0f, 1f));
    }

    //    public void Refresh()
    //    {
    //        box.isTrigger = false;
    //        transform.localScale = new Vector3(maxSize / 20, maxSize / 20, 1);
    //    }
}
