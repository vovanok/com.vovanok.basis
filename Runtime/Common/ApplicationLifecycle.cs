using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace UnityBasis.Common
{
    public class ApplicationLifecycle : MonoBehaviour
    {
        public bool IsGamePaused { get; set; }
        public UnityEvent<float> OnUpdate { get; } = new();
        public UnityEvent<float> OnFixedUpdate { get; } = new();
        public UnityEvent OnBeforeChangeScene { get; } = new();

        public void GoToScene(string sceneName)
        {
            OnBeforeChangeScene.Invoke();
            SceneManager.LoadScene(sceneName);
        }

        public void RunAfterDelay(TimeSpan delay, Action action)
        {
            StartCoroutine(ExecuteAfterDelay((float) delay.TotalSeconds, action));
        }

        public void Exit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        private void Update()
        {
            OnUpdate.Invoke(Time.deltaTime);
        }
        
        private void FixedUpdate()
        {
            OnFixedUpdate.Invoke(Time.deltaTime);
        }

        private IEnumerator ExecuteAfterDelay(float delaySec, Action action)
        {
            yield return new WaitForSeconds(delaySec);
            action();
        }
    }
}
