using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameLoader : IPipeListener
{
    public GameLoader()
    {
        SmartPipe2.RegisterListener_AsProccessor<GameLoadAsstsAction>(this, Load);
    }

    private void Load(GameLoadAsstsAction action)
    {
        Task.Delay(Random.Range(1000, 5000)).ContinueWith(t=> Loaded(action));
    }


    private void Loaded(GameLoadAsstsAction action)
    {
        Debug.Log("Loaded: " + action.url);
        action.SetCompleted();

    }
}
