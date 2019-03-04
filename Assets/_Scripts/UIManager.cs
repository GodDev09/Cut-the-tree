using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventDispatcher;
using System;
public class UIManager : MonoBehaviour
{
    public static UIManager Instance = new UIManager();
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

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
                GameManager.Instance.stateGame = StateGame.PLAYING;
            });
    }
    #endregion
}