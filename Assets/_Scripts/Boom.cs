using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventDispatcher;

public class Boom : MonoBehaviour
{
    public float speed;
    bool isCanMove = true;
    Vector3 target = Vector3.zero;
    float maxX = 2.7f;
    float maxY = 4.7f;
    // Use this for initialization
    void OnSpawned()
    {
        GameManager.Instance.lstBoom.Add(this);
        speed = Random.Range(1.5f, GameConfig.Instance.MaxSpeedBoom);
        this.RegisterListener(EventID.GAME_OVER, (param) => ON_OVER());
        NextPoint();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.stateGame == StateGame.PLAYING)
        {
            if (target != Vector3.zero)
            {
                Move();
            }
            else
            {
                NextPoint();
            }
        }
    }

    void ON_OVER()
    {
        EZ_Pooling.EZ_PoolManager.Despawn(this.transform);
    }

    void Move()
    {
        if (Vector3.Distance(transform.position, target) >= 0.5f)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
        }
        else
        {
            NextPoint();
        }
    }

    void NextPoint()
    {
        target = new Vector3(Random.Range(-maxX, maxX), Random.Range(-maxY, maxY), 0);
    }

    public void Refresh()
    {
        EZ_Pooling.EZ_PoolManager.Despawn(this.transform);
    }
}
