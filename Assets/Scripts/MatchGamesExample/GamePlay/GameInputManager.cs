using System;
using Unity.Mathematics;
using UnityEngine;

namespace MatchGamesExample.GamePlay
{
    public class GameInputManager : MonoBehaviour, IPipeListener
    {
        private bool isEnabled = false;
        
        public void Awake()
        {
            SmartPipe.RegisterListener<GameStartedAction>(this, OnGameStarted);    
        }

        private void OnGameStarted(GameStartedAction obj)
        {
            isEnabled = true;
        }

        private static int2 ToInt(Vector3 v)
        {
            return new int2((int) v.x, (int) v.y);
        }
        public void Update()
        {
            if (!isEnabled)
                return;

            //if (Input.GetMouseButton(0))
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 mousePos = Input.mousePosition;
                mousePos.z = Camera.main.nearClipPlane;
                var wp = Camera.main.ScreenToWorldPoint(mousePos);

                var pos = Utils.PositionToIndex(wp);
                
                Debug.Log("Clicked: " + pos);
                GameClickAction.Emmit(pos);
            }
        }
    }
}