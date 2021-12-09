
using System;
using UnityEngine;



public abstract class IInfoAction
{
    public void Wait(IWaiter waiter)
    {
        SmartPipe2.AddWaiter(waiter, this);
    }
}

public interface IWaiter
{
    void OnAllActionsCompleted();
}

public abstract class IProcessAction : IInfoAction
{
    private bool _isCompleted = false;

    public bool IsCompleted
    {
        get => _isCompleted;
    }

    public void SetCompleted()
    {
        if (_isCompleted)
        {
            Debug.LogError("Process Action already completed");
            return;
        }
        
        _isCompleted = true;
        SmartPipe2.Emmit(this as IInfoAction);
    }
    
}

public abstract class IFactoryAction : IInfoAction
{
    private bool _isResolved = false;
    
    public bool isResolved
    {
        get => _isResolved;
    }

    public void Resolve()
    {
        if (_isResolved)
        {
            Debug.LogError("Factory Action already completed");
            return;
        }
        
        SmartPipe2.Emmit(this as IInfoAction);
    }


}
