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
    public GameObject boxCenter;
    public GameObject cutter;
    public Transform preBoom;
    public Transform[] lstPosSpawn;
    public List<Boom> lstBoom = new List<Boom>();
    public List<Grass> lstGrass = new List<Grass>();
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
        
    }

    // Update is called once per frame
    void Update()
    {
        if (stateGame == StateGame.PLAYING)
        {
            if (numGrass <= 0)
                Win();
        }
    }

    void ON_START_GAME()
    {
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
        boxCenter.SetActive(false);
        cutter.SetActive(true);
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
