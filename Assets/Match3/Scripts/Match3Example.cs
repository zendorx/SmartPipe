using System;
using System.Collections;
using System.Collections.Generic;
using Match3.Scripts;
using UnityEngine;

public class Match3Example : MonoBehaviour, IPipeListener
{
    [SerializeField] private Transform root;
    [SerializeField] private Jewel prefab;
    
    private Match3Field field;
    
    
    void Start()
    {
        OLD_SmartPipe.RegisterListener<Match3_CreateJewel>(this, OnNeedJewel);
        
        field = new Match3Field();
        
        Match3_InitField.Emmit(6, 6);
    }

    public void OnNeedJewel(Match3_CreateJewel jewel)
    {
        var obj = Instantiate(prefab, Vector3.zero, Quaternion.identity, root);
        jewel.jewel = obj;
    }

    public void Update()
    {
        OLD_SmartPipe.Update();
    }
}
