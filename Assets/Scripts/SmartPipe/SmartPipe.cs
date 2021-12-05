using System;
using System.Collections.Generic;
using UnityEngine;
using Object = System.Object;

public class SmartPipe
{
    public delegate void OnAction<in T>(T obj);

    struct ActionListenerData
    {
        public IPipeListener _Listener;
        public OnAction<object> _OnAction;
        public Type _type;
    }
    
    private static Dictionary<Type, List<ActionListenerData> > _listeners =new Dictionary<Type, List<ActionListenerData>>();

    private static SmartPipe _instance = null;
    
    public static void RegisterListener<T>(IPipeListener listener, OnAction<T> callback) where T : class
    {
        var type = typeof(T);
        
        if (!_listeners.ContainsKey(type))
        {
            _listeners[type] = new List<ActionListenerData>();
        }
        
        var list = _listeners[type];

        ActionListenerData data = new ActionListenerData
        {
            _Listener = listener,
            _type = type
        };

        data._OnAction = delegate(object o) { callback(o as T); };
        
        list.Add(data);
    }

    public static T EmmitActionWithResult<T>(object obj) where T : class
    {
        EmmitAction(obj, true);

        return obj as T;
    }
    
    public static void EmmitAction(object obj, bool check = false)
    {
        var type = obj.GetType();
        bool recieved = false;
        if (_listeners.ContainsKey(type))
        {
            var list = _listeners[type];

            foreach (var l in list)
            {
                l._OnAction(obj);
                recieved = true;
            }
        }

        if (check && !recieved)
        {
            Debug.LogError("[Smart Pipe] action is not recieved: " + obj);
        }
    }
    
}
