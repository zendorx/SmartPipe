using System;
using UnityEngine;

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
            SmartPipe.Update();
        }

        public static bool HasInstance()
        {
            return instance != null;
        }
    }
