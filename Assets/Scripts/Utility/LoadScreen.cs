using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts.Utility
{
    public class LoadScreen : MonoBehaviour
    {
        [SerializeField] GameObject loadingScreen;
        [SerializeField] Slider slider;

        public bool isTransitionScene;

        private void Awake()
        {
            if(isTransitionScene)
            {
                LoadScene(6);
            }
        }
        public void LoadScene(int sceneId)
        {
            loadingScreen.SetActive(true);
            StartCoroutine(LoadSceneAsync(sceneId));
        }
        IEnumerator LoadSceneAsync(int sceneId)
        {
            AsyncOperation operation = SceneManager.LoadSceneAsync(sceneId);
            while(!operation.isDone)
            {
                slider.value = operation.progress;
                yield return null;
            }
        }
    }
}