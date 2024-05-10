using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingSceneController : MonoBehaviour
{
    private static string _nextScene = "Main";

    [SerializeField] private Image progressBar;

    public static void LoadScene(string sceneName)
    {
        _nextScene = sceneName;
        SceneManager.LoadScene("LoadingScene");
    }

    void Start()
    {
        StartCoroutine(LoadSceneProcess());
    }

    IEnumerator LoadSceneProcess()
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(_nextScene);
        asyncOperation.allowSceneActivation = false;

        float timer = 0.0f;
        while (!asyncOperation.isDone)
        {
            yield return null;

            if (asyncOperation.progress < 0.8f)
            {
                progressBar.fillAmount = asyncOperation.progress;
            }
            else
            {
                timer += Time.unscaledDeltaTime;
                progressBar.fillAmount = Mathf.Lerp(0.8f, 1.0f, timer);
                if (progressBar.fillAmount >= 1.0f)
                {
                    asyncOperation.allowSceneActivation = true;
                    yield break;
                }
            }
        }
    }
}
