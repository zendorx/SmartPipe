using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneLoader : IPipeListener
{
    public GameSceneLoader()
    {
        SmartPipe.RegisterListener_AsProccessor<GameSceneLoadAction>(this, LoadScene);
    }

    private void LoadScene(GameSceneLoadAction obj)
    {
        Debug.Log("Loading Scene: " + obj.sceneName);
        SceneManager.LoadScene(obj.sceneName);
        obj.SetCompleted();
    }
}
