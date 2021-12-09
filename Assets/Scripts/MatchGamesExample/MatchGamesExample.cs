using System;
using System.Collections.Generic;
using UnityEngine;

public class MatchGamesExample : MonoBehaviour, IWaiter
{
    List<IPipeListener> core = new List<IPipeListener>();

    public string assetBundle1 = "http://myassets.com/bundle1";
    public string assetBundle2 = "http://myassets.com/bundle2";
    public string assetBundle3 = "http://myassets.com/bundle3";
    
    private void Start()
    {
        InitCore();
        LoadGame();
    }

    private void InitCore()
    {
        core.Add(new GameLoader());
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
        SmartPipe2.Update();
    }
    

    private void StartGame()
    {
        Debug.Log("Starting Game");
    }

    public void OnAllActionsCompleted()
    {
        StartGame();
    }
}
