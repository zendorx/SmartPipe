using System;
using System.Collections;
using System.Collections.Generic;
using MatchGamesExample.GamePlay;
using MatchGamesExample.GamePlay.View;
using UnityEngine;

public class GameEntryPoint : MonoBehaviour, IPipeListener
{
    List<IPipeListener> _listeners = new List<IPipeListener>();
    
    void Start()
    {
        Debug.Log("Init game scene");
        
        Init();
        
        FieldCreateAction.Process(1, 6, 4, 100500);
    }

    private void Init()
    {
        _listeners.Add(new FieldController());
        _listeners.Add(new Match3Controller());
        
        
        SmartPipe2.RegisterListener<FieldCreateAction>(this, OnFieldCreated);
    }

    private void OnFieldCreated(FieldCreateAction obj)
    {
        Debug.Log("Field created");
        GameStartedAction.Emmit();
    }

    private void OnDestroy()
    {
        foreach (var l in _listeners)
        {
            SmartPipe2.Unregister(l);
        }
    }
    
    
}
