using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SmartPipe
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
    private static Dictionary<Type, List<ProccessActionData> > _listenersProcessor =new Dictionary<Type, List<ProccessActionData> >();
    private static Dictionary<Type, FactoryActionData > _listenersFactory =new Dictionary<Type, FactoryActionData>();
    
    private static Dictionary<IWaiter, List<IInfoAction> > _waiters = new Dictionary<IWaiter, List<IInfoAction>>();
    
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
        
        if (_listenersFactory.ContainsKey(type))
        {
            var listener = _listenersFactory[type];
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
            var list = _listenersProcessor[type];

            if (list.Count == 0)
            {
                Debug.LogError("Pipe does not contain process listener for action:" + type);
            }

            var mList = list.ToList();
            
            foreach (var l in mList)
            {
                l.callback(process);    
            }
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
        
        CheckWaiters(info);
    }

    public static void Emmit(IProcessAction action)
    {
        CheckInstance();
        
        var queueData = new QueueData
        {
            process = action
        };

        _actionQueue.Enqueue(queueData);
    }
    
    public static void Emmit(IFactoryAction action)
    {
        CheckInstance();

        
        var queueData = new QueueData
        {
            factory = action
        };

        _actionQueue.Enqueue(queueData);
    }
    
    public static void Emmit(IInfoAction action)
    {
        CheckInstance();

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
    public static void RegisterListener<T>(IPipeListener listener, OnAction<T> callback) where T : IInfoAction
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
    public static void RegisterListener_AsProccessor<T>(IPipeListener listener, OnAction<T> callback) where T : IProcessAction
    {
        var type = typeof(T);
        
        if (!_listenersProcessor.ContainsKey(type))
        {
            _listenersProcessor[type] = new List<ProccessActionData>();
        }

        var list = _listenersProcessor[type];
        
        ProccessActionData data = new ProccessActionData
        {
            listener = listener,
        };

        data.callback = delegate(object o) { callback(o as T); };
        list.Add(data);
    }
    
    
    //Resolve command and return result
    public static void RegisterListener_AsFactory<T>(IPipeListener listener, OnAction<T> callback) where T : IFactoryAction
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

        foreach (var types in _listenersProcessor)
        {
            types.Value.RemoveAll(x => x.listener == listener);
        }
        
        var keysFactory = _listenersFactory.Keys.ToList();
        foreach (var key in keysFactory)
        {
            if (_listenersFactory[key].listener == listener)
            {
                _listenersProcessor.Remove(key);
            }
        } 
    }

    private static void CheckWaiters(IInfoAction action)
    {
        var keys = _waiters.Keys.ToList();
        
        foreach (var key in keys)
        {
            var value = _waiters[key];
            
            if (value.Contains(action))
            {
                value.Remove(action);

                if (value.Count == 0)
                {
                    key.OnAllActionsCompleted();
                    _waiters.Remove(key);
                }
            }
        }
    }

    public static void AddWaiter(IWaiter waiter, IInfoAction action)
    {
        if (!_waiters.ContainsKey(waiter))
        {
            _waiters[waiter] = new List<IInfoAction>();
        }
        
        _waiters[waiter].Add(action);
    }

    private static void CheckInstance()
    {
        if (!SmartPipeMono.HasInstance())
        {
            Debug.LogError("Smart pipe has no MONO instance. Cannot update");
        }
    }
}
