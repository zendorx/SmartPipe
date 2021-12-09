using System;
using System.Collections;
using System.Collections.Generic;
using MatchGamesExample.GamePlay;
using MatchGamesExample.GamePlay.View;
using UnityEngine;

public class GameSceneContext : MonoBehaviour
{
    List<IPipeListener> _listeners = new List<IPipeListener>();
    
    void Start()
    {
        Debug.Log("Init game scene");
        
        Init();
        
        FieldCreateAction.Process(6, 6, 4, 100500);
    }

    private void Init()
    {
        _listeners.Add(new FieldController());
        _listeners.Add(new Match3Controller());
    }

    private void OnDestroy()
    {
        foreach (var l in _listeners)
        {
            SmartPipe2.Unregister(l);
        }
    }
}
