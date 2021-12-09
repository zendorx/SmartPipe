using System;
using UnityEngine;

namespace SmartPipe
{
    public class SmartPipeMono : MonoBehaviour
    {
        private static SmartPipeMono instance;
        
        private void Awake()
        {
            if (instance != null)
            {
                Destroy(gameObject);
                return;
            }

            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void Update()
        {
            SmartPipe2.Update();
        }

        public static bool HasInstance()
        {
            return instance != null;
        }
    }
}