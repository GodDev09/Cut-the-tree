using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager Instance = new GameManager();
    public StateGame stateGame = StateGame.NONE;
    void Awake()
    {
        if (Instance != null)
        {
            return;
        }
        Instance = this;
    }
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

[System.Serializable]
public enum StateGame
{
    NONE,
    PLAYING
}
