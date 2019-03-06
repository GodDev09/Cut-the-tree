using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_ADS
using UnityEngine.Advertisements; // only compile Ads code on supported platforms
#endif

public class LoadDataJson : MonoBehaviour
{
    public static LoadDataJson Instance;
    public GameObject gameManager;
    public bool isReset;
    void Awake()
    {
        if (Instance != null)
        {
            return;
        }
        Instance = this;
        if (isReset)
        {
            PlayerPrefs.DeleteAll();
        }
    }
    private string gameConfig = "GameConfig";

    void Start()
    {
        LoadGameConfig();
        //Ads.Instance.RequestAd();
        //Ads.Instance.RequestBanner();
        //if (PlayerPrefs.GetInt(KeyPrefs.IS_CONTINUE) == 1)
        //{
        //    Ads.Instance.ShowBanner();
        //}
#if UNITY_ADS
        Advertisement.Initialize(GameConfig.Instance.ID_UnityAds_ios, true);
#endif
        //Purchaser.Instance.Init();
    }
    public void LoadGameConfig()
    {
        var objJson = SimpleJSON_DatDz.JSON.Parse(loadJson(gameConfig));
        //Debug.Log(objJson);
        Debug.Log("<color=yellow>Done: </color>LoadGameConfig !");
        if (objJson != null)
        {
            GameConfig.Instance.MinGrass = objJson["MinGrass"].AsInt;
            GameConfig.Instance.MaxGrass = objJson["MaxGrass"].AsInt;
            GameConfig.Instance.MaxSpeedCutter = objJson["MaxSpeedCutter"].AsFloat;
            GameConfig.Instance.MaxSpeedBoom = objJson["MaxSpeedBoom"].AsFloat;
            //GameConfig.Instance.GoldStart = objJson["GoldStart"].AsLong;
            //GameConfig.Instance.CoinStart = objJson["CoinStart"].AsLong;
            //GameConfig.Instance.ID_UnityAds_ios = objJson["ID_UnityAds_ios"];
            //GameConfig.Instance.ID_Inter_android = objJson["ID_Inter_android"];
            //GameConfig.Instance.ID_Inter_ios = objJson["ID_Inter_ios"];
            //GameConfig.Instance.ID_Banner_ios = objJson["ID_Banner_ios"];
            //GameConfig.Instance.kProductID50 = objJson["kProductID50"];
            //GameConfig.Instance.kProductID300 = objJson["kProductID300"];
            //GameConfig.Instance.kProductID5000 = objJson["kProductID5000"];
            //GameConfig.Instance.link_ios = objJson["link_ios"];
            //GameConfig.Instance.link_android = objJson["link_android"];
            //GameConfig.Instance.string_Share = objJson["string_Share"];
            //for (int i = 0; i < objJson["introduction"].Count; i++)
            //{
            //    GameConfig.Instance.lstIntroduction.Add(objJson["introduction"][i]);
            //}
            GameManager.Instance.minGrass = GameConfig.Instance.MinGrass;
            GameManager.Instance.maxGrass = GameConfig.Instance.MaxGrass;
            GameManager.Instance.cutter.GetComponent<MoveController>().maxSpeed = GameConfig.Instance.MaxSpeedCutter;
        }
        gameManager.SetActive(true);
    }

    string loadJson(string _nameJson)
    {
        TextAsset _text = Resources.Load(_nameJson) as TextAsset;
        return _text.text;
    }
}
