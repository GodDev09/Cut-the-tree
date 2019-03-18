using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventDispatcher;
using System;
using UnityEngine.UI;
public class UIManager : MonoBehaviour
{
    public static UIManager Instance = new UIManager();
    public Text txtNumberGrass;
    public GameObject panelEndGame;
    public Text txtWinOver;
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
    }

    // Update is called once per frame
    void Update()
    {
        txtNumberGrass.text = GameManager.Instance.numGrass.ToString();
    }

    #region === SUPPORT ===
    public void SetActivePanel(GameObject _g)
    {
        if (_g == null || _g.activeSelf)
            return;

        _g.SetActive(true);
        //if (_g.name == "InWall")
        //    _g.GetComponent<Animator>().Play("ActivePanel");
    }

    public void SetDeActivePanel(GameObject _g)
    {
        if (_g == null || !_g.activeSelf)
            return;

        _g.SetActive(false);
        //_g.GetComponent<Animator>().Play("DeActivePanel");
    }

    private string[] cashFormat = new string[]
	{
		"K",
		"M",
		"B",
		"T"
	};
    public string ConvertCash(double cash)
    {
        if (cash < 1000.0)
        {
            return Math.Round(cash).ToString();
        }
        int num = 0;
        double num2 = 0.0;
        for (int i = 0; i < cashFormat.Length; i++)
        {
            num2 = cash / Math.Pow(1000.0, (double)(i + 1));
            if (num2 < 1000.0)
            {
                num2 = Math.Round(num2, (num2 >= 100.0) ? 0 : 1);
                num = i;
                break;
            }
        }
        return num2.ToString() + cashFormat[num];
    }
    #endregion

    #region === UI HOME ===
    public void Btn_Play()
    {
        ScenesManager.Instance.GoToScene(ScenesManager.TypeScene.Main, () =>
            {
                this.PostEvent(EventID.START_GAME);
                StartCoroutine(GameManager.Instance.SpawnCutter());
                AudioManager.Instance.Play("GamePlay", true);
            });
    }

    public void Btn_RePlay()
    {
        AudioManager.Instance.Play("Click");
        Time.timeScale = 1;
        panelEndGame.transform.localScale = new Vector3(0, 0, 0);
        ScenesManager.Instance.GoToScene(ScenesManager.TypeScene.Main, () =>
        {
            //for (int i = 0; i < GameManager.Instance.lstBoom.Count; i++)
            //{
            //    GameManager.Instance.lstBoom[i].Refresh();
            //}         
            GameManager.Instance.maxGrass = GameConfig.Instance.MaxGrass;
            GameManager.Instance.minGrass = GameConfig.Instance.MinGrass;
            GameManager.Instance.numGrass = 0;
            GameManager.Instance.parent.transform.position = Vector3.zero;
            GameManager.Instance.lstCutter[0].transform.localPosition = Vector3.zero;
            for (int i = 1; i < GameManager.Instance.lstCutter.Count; i++)
            {
                GameManager.Instance.lstCutter[i].DeActive();
            }
            GameManager.Instance.lstCutter[0].Active();
            GameManager.Instance.health = 0;
            GameManager.Instance.spawnCutter = 0;
            GameManager.Instance.timeSpawnBoom = 0;
            StartCoroutine(TEST());
            //StartCoroutine(GameManager.Instance.DeSpawnGrass());
            //StartCoroutine(GameManager.Instance.Generate());
            //StartCoroutine(GameManager.Instance.SpawnCutter());
            this.PostEvent(EventID.START_GAME);

        });
    }

    IEnumerator TEST()
    {
        //StartCoroutine(GameManager.Instance.DeSpawnGrass());
        yield return new WaitForEndOfFrame();
        StartCoroutine(GameManager.Instance.Generate());
        //Debug.Log("12");
        yield return new WaitForEndOfFrame();
        StartCoroutine(GameManager.Instance.SpawnCutter());
        //Debug.Log("13");
    }

    public void ShowPanelEndGame(string _state)
    {
        panelEndGame.transform.localScale = new Vector3(1, 1, 1);
        txtWinOver.text = _state;
        Time.timeScale = 0;
    }

    public void ABC()
    {
        ShowPanelEndGame("Win");
    }
    #endregion
}