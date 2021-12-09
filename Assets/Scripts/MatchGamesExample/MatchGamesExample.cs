using System;
using System.Collections.Generic;
using UnityEngine;

public class MatchGamesExample : MonoBehaviour
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
        GameLoadAsstsAction.Process(assetBundle1);
        GameLoadAsstsAction.Process(assetBundle2);
        GameLoadAsstsAction.Process(assetBundle3);
    }

    public void Update()
    {
        SmartPipe2.Update();
    }

    private void StartGame()
    {
        
    }
}
