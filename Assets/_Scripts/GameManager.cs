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
    public List<Grass> lstGrass = new List<Grass>();
    public List<MoveController> lstCutter = new List<MoveController>();
    public int a = 1;
    public int spawnCutter;
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

            if (lstCutter.Count <= 0)
                GameOver();

            SpawnCutter_EatGrass();
        }
    }

    void ON_START_GAME()
    {
        stateGame = StateGame.PLAYING;
        Generate();
    }

    public void Generate()
    {
        lstBoom.Clear();
        maxGrass = Random.Range(minGrass, maxGrass);

        for (int i = 0; i < maxGrass / lstPosSpawn.Length; i++)
        {
            if (numGrass < maxGrass)
            {
                for (int j = 0; j < lstPosSpawn.Length; j++)
                {
                    EZ_Pooling.EZ_PoolManager.Spawn(preGrass, lstPosSpawn[j].position + new Vector3(Random.Range(-1, 1f), Random.Range(-1, 1f), 0), Quaternion.identity);
                    numGrass++;
                }
            }
            else
                break;
        }

        //for (int i = 0; i < maxGrass / 4; i++)
        //{
        //    if (numGrass < maxGrass)
        //    {
        //        float a = Random.Range(-1, 1f);
        //        float b = Random.Range(-1, 1f);
        //        EZ_Pooling.EZ_PoolManager.Spawn(preGrass, lstPosSpawn[0].position + new Vector3(a, b, 0), Quaternion.identity);
        //        numGrass++;
        //    }
        //    else
        //        break;
        //}
        //for (int i = 0; i < maxGrass / 4; i++)
        //{
        //    if (numGrass < maxGrass)
        //    {
        //        float a = Random.Range(-1, 1f);
        //        float b = Random.Range(-1, 1f);
        //        EZ_Pooling.EZ_PoolManager.Spawn(preGrass, lstPosSpawn[1].position + new Vector3(a, b, 0), Quaternion.identity);
        //        numGrass++;
        //    }
        //    else
        //        break;
        //}
        //for (int i = 0; i < maxGrass / 4; i++)
        //{
        //    if (numGrass < maxGrass)
        //    {
        //        float a = Random.Range(-1, 1f);
        //        float b = Random.Range(-1, 1f);
        //        EZ_Pooling.EZ_PoolManager.Spawn(preGrass, lstPosSpawn[2].position + new Vector3(a, b, 0), Quaternion.identity);
        //        numGrass++;
        //    }
        //    else
        //        break;
        //}
        //for (int i = 0; i < maxGrass / 4; i++)
        //{
        //    if (numGrass < maxGrass)
        //    {
        //        float a = Random.Range(-1, 1f);
        //        float b = Random.Range(-1, 1f);
        //        EZ_Pooling.EZ_PoolManager.Spawn(preGrass, lstPosSpawn[3].position + new Vector3(a, b, 0), Quaternion.identity);
        //        numGrass++;
        //    }
        //    else
        //        break;
        //}
        //StartCoroutine(SpawnCutter());
    }

    public IEnumerator SpawnCutter()
    {
        yield return new WaitForSeconds(3f);
        parent.SetActive(true);

        //StartCoroutine(SpawnBoom());
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
        if (spawnCutter >= 20)
        {
            spawnCutter = 0;
            lstCutter[a].gameObject.SetActive(true);
            a++;
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
        //Time.timeScale = 0;
        stateGame = StateGame.NONE;
        UIManager.Instance.ShowPanelEndGame("Game Over");
    }
}

[System.Serializable]
public enum StateGame
{
    NONE,
    PLAYING
}
