using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventDispatcher;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance = new GameManager();
    public StateGame stateGame = StateGame.NONE;
    public int maxGrass;
    public int minGrass;
    public int numGrass;
    public Transform preGrass;
    public GameObject parent;
    public Transform preBoom;
    public GameObject preCutter;
    public Transform[] lstPosSpawn;
    public List<Boom> lstBoom = new List<Boom>();
    //public List<Grass> lstGrass = new List<Grass>();
    public List<MoveController> lstCutter = new List<MoveController>();
    public int spawnCutter;
    public int health = 0;
    void Awake()
    {
        if (Instance != null)
        {
            return;
        }
        Instance = this;
    }
    // Use this for initialization
    void Start()
    {
        this.RegisterListener(EventID.START_GAME, (param) => ON_START_GAME());
        //Instantiate(preCutter, parent.transform.position, Quaternion.identity, parent.transform);
    }

    // Update is called once per frame
    void Update()
    {
        if (stateGame == StateGame.PLAYING)
        {
            if (numGrass <= 0)
                Win();

            //if (health <= 0)
            //    GameOver();

            SpawnCutter_EatGrass();
        }
    }

    void ON_START_GAME()
    {
        stateGame = StateGame.PLAYING;
        StartCoroutine(Generate());
    }

    public IEnumerator Generate()
    {
        lstBoom.Clear();
        maxGrass = Random.Range(minGrass, maxGrass);

        yield return new WaitForEndOfFrame();
        for (int i = 0; i < maxGrass / lstPosSpawn.Length; i++)
        {

            for (int j = 0; j < lstPosSpawn.Length; j++)
            {
                if (numGrass < maxGrass)
                {
                    EZ_Pooling.EZ_PoolManager.Spawn(preGrass, lstPosSpawn[j].position + new Vector3(Random.Range(-1, 1f), Random.Range(-1, 1f), 0), Quaternion.identity);
                    numGrass++;
                }
                else
                    break;
            }

        }
    }

    //public IEnumerator DeSpawnGrass()
    //{
    //    for (int i = 0; i < lstGrass.Count; i++)
    //    {
    //        lstGrass[i].DeActive();
    //    }
    //    yield return new WaitForEndOfFrame();
    //}

    public IEnumerator SpawnCutter()
    {
        yield return new WaitForSeconds(3f);
        parent.SetActive(true);

        StartCoroutine(SpawnBoom());
    }

    IEnumerator SpawnBoom()
    {
        for (int i = 0; i < Random.Range(2, 5); i++)
        {
            Vector3 pos;
            if (i >= lstPosSpawn.Length)
            {
                pos = lstPosSpawn[lstPosSpawn.Length].position;
            }
            else
            {
                pos = lstPosSpawn[i].position;
            }
            yield return new WaitForSeconds(1f);
            EZ_Pooling.EZ_PoolManager.Spawn(preBoom, pos, Quaternion.identity);
        }
    }

    void SpawnCutter_EatGrass()
    {
        if (spawnCutter >= 30)
        {
            spawnCutter = 0;
            for (int i = 0; i < lstCutter.Count; i++)
            {
                if (!lstCutter[i].isActive)
                {
                    lstCutter[i].Active();
                    break;
                }
            }
            //Instantiate(preCutter, parent.transform.position, Quaternion.identity,parent.transform);
            //EZ_Pooling.EZ_PoolManager.Spawn(preCutter, parent.transform.position, Quaternion.identity);
        }
    }

    public void Win()
    {
        //Time.timeScale = 0;
        stateGame = StateGame.NONE;
        UIManager.Instance.ShowPanelEndGame("Win");
    }

    public void GameOver()
    {
        health--;
        if (health <= 0 && stateGame == StateGame.PLAYING)
        {
            health = 0;
            stateGame = StateGame.NONE;
            this.PostEvent(EventID.GAME_OVER);
            UIManager.Instance.ShowPanelEndGame("Game Over");
        }
    }
}

[System.Serializable]
public enum StateGame
{
    NONE,
    PLAYING
}
