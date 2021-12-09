using System;
using System.Collections.Generic;
using UnityEngine;

public class EntryPoint : MonoBehaviour, IWaiter
{
    List<IPipeListener> core = new List<IPipeListener>();

    public string assetBundle1 = "http://myassets.com/bundle1";
    public string assetBundle2 = "http://myassets.com/bundle2";
    public string assetBundle3 = "http://myassets.com/bundle3";
    
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        
        InitCore();
        LoadGame();
    }

    private void InitCore()
    {
        core.Add(new GameLoader());
        core.Add(new GameSceneLoader());
    }

    private void LoadGame()
    {
        Debug.Log("Loading game..");
        
        GameLoadAsstsAction.Process(assetBundle1).Wait(this);
        GameLoadAsstsAction.Process(assetBundle2).Wait(this);
        GameLoadAsstsAction.Process(assetBundle3).Wait(this);
    }

    public void Update()
    {
        //SmartPipe2.Update();
    }
    

    private void StartGame()
    {
        Debug.Log("Starting Game");
        GameSceneLoadAction.Process("MatchGamesExample");
    }

    public void OnAllActionsCompleted()
    {
        StartGame();
    }
}
