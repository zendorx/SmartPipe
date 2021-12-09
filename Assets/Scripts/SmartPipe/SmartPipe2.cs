﻿using System;
using System.Collections.Generic;
using UnityEngine;

public class SmartPipe2
{
    public delegate void OnAction<in T>(T obj);

    struct InfoActionData
    {
        public IInfoAction action;
        public IPipeListener listener;
        public OnAction<object> callback;
    }

    struct ProccessActionData
    {
        public IProcessAction action;
        public IPipeListener listener;
        public OnAction<object> callback;
    }

    struct FactoryActionData
    {
        public IFactoryAction action;
        public IPipeListener listener;
        public OnAction<object> callback;
    }

    struct QueueData
    {
        public IFactoryAction factory;
        public IInfoAction info;
        public IProcessAction process;
    }
    
    private static Dictionary<Type, List<InfoActionData> > _listenersInfo =new Dictionary<Type, List<InfoActionData>>();
    private static Dictionary<Type, ProccessActionData > _listenersProcessor =new Dictionary<Type, ProccessActionData>();
    private static Dictionary<Type, FactoryActionData > _listenersFactory =new Dictionary<Type, FactoryActionData>();
    
    private static Queue<QueueData> _actionQueue = new Queue<QueueData>();

    public static int actionsPerFrame = 1;
    
    public static void Update()
    {
        for (int i = 0; i < actionsPerFrame; i++)
        {
            if (_actionQueue.Count == 0)
                break;
            
            var data = _actionQueue.Dequeue();

            if (data.info != null)
            {
                EmmitInfoNow(data.info);
            }
            else if (data.process != null)
            {
                EmmitProcessNow(data.process);
            }
            else if (data.factory != null)
            {
                EmmitFactoryNow(data.factory);
            }
            else
            {
                Debug.LogError("Queue Data does not contain data");
            }
        }
    }
    
    private static void EmmitFactoryNow(IFactoryAction factory)
    {
        var type = factory.GetType();
        
        if (_listenersProcessor.ContainsKey(type))
        {
            var listener = _listenersProcessor[type];
            listener.callback(factory);
        }
        else
        {
            Debug.LogError("Pipe does not contains factory listener for action:" + type);
        }
    }
    
    private static void EmmitProcessNow(IProcessAction process)
    {
        var type = process.GetType();
        
        if (_listenersProcessor.ContainsKey(type))
        {
            var listener = _listenersProcessor[type];
            listener.callback(process);
        }
        else
        {
            Debug.LogError("Pipe does not contains process listener for action:" + type);
        }
    }

    private static void EmmitInfoNow(IInfoAction info)
    {
        var type = info.GetType();
        
        if (_listenersInfo.ContainsKey(type))
        {
            var list = _listenersInfo[type];

            foreach (var l in list)
            {
                l.callback(info);
            }
        }
    }

    public static void Emmit(IProcessAction action)
    {
        var queueData = new QueueData
        {
            process = action
        };

        _actionQueue.Enqueue(queueData);
    }
    
    public static void Emmit(IFactoryAction action)
    {
        var queueData = new QueueData
        {
            factory = action
        };

        _actionQueue.Enqueue(queueData);
    }
    
    public static void Emmit(IInfoAction action)
    {
        var queueData = new QueueData
        {
            info = action
        };

        var process = action as IProcessAction;
        if (process != null)
        {
            if (!process.IsCompleted)
            {
                Debug.LogError("Trying to emmit not completed process action as info: " + action);
                return;
            }
        }
        
        var factory = action as IFactoryAction;
        if (factory != null)
        {
            if (!factory.isResolved)
            {
                Debug.LogError("Trying to emmit not completed factory action as info: " + action);
                return;
            }
        }
        
        _actionQueue.Enqueue(queueData);
    }
    
    //emmit event for all
    public static void RegisterListener<T>(IPipeListener listener, OnAction<T> callback) where T : class
    {
        var type = typeof(T);
        
        if (!_listenersInfo.ContainsKey(type))
        {
            _listenersInfo[type] = new List<InfoActionData>();
        }
        
        var list = _listenersInfo[type];

        InfoActionData data = new InfoActionData
        {
            listener = listener,
        };

        data.callback = delegate(object o) { callback(o as T); };
        
        list.Add(data);
    }
    
    //result command and emmit info
    public static void RegisterListener_AsProccessor<T>(IPipeListener listener, OnAction<T> callback) where T : class
    {
        var type = typeof(T);
        
        if (_listenersProcessor.ContainsKey(type))
        {
            Debug.LogError($"Process listener ({listener})for action already registered: " + type);
            return;
        }
        
        ProccessActionData data = new ProccessActionData
        {
            listener = listener,
        };

        data.callback = delegate(object o) { callback(o as T); };
        _listenersProcessor[type] = data;
    }
    
    
    //Resolve command and return result
    public static void RegisterListener_AsFactory<T>(IPipeListener listener, OnAction<T> callback) where T : class
    {
        var type = typeof(T);
        
        if (_listenersFactory.ContainsKey(type))
        {
            Debug.LogError($"Factory listener ({listener})for action already registered: " + type);
            return;
        }
        
        FactoryActionData data = new FactoryActionData
        {
            listener = listener,
        };

        data.callback = delegate(object o) { callback(o as T); };
        _listenersFactory[type] = data;
    }

    public static void Unregister(IPipeListener listener)
    {
        foreach (var types in _listenersInfo)
        {
            types.Value.RemoveAll(x => x.listener == listener);
        }

        var keysProcessor = _listenersProcessor.Keys;
        foreach (var key in keysProcessor)
        {
            if (_listenersProcessor[key].listener == listener)
            {
                _listenersProcessor.Remove(key);
            }
        } 
        
        var keysFactory = _listenersFactory.Keys;
        foreach (var key in keysFactory)
        {
            if (_listenersProcessor[key].listener == listener)
            {
                _listenersProcessor.Remove(key);
            }
        } 
    }
    
    
}