using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingHandler : MonoBehaviour
{
    public static LoadingHandler Instance => instance;
    private static LoadingHandler instance;

    public Action OnSceneLoadCompleted;

    [SerializeField] private Image fadeImg;

    [SerializeField] private float fadeDuration;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        fadeImg.color = Color.black;

        DontDestroyOnLoad(gameObject);
    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;

        //DoFadeAnim(0);
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;

        instance = null;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode loadMode)
    {
        DoFadeAnim(0);
    }

    public void LoadScene(string sceneName)
    {
        DoFadeAnim(1, () => SceneManager.LoadScene(sceneName));
    }
    public void LoadScene(int sceneId)
    {
        DoFadeAnim(1, () => SceneManager.LoadScene(sceneId));
    }

    private void DoFadeAnim(float targetAlpha, Action OnAnimCompleted = null)
    {
        Debug.Log("Cross Fade");
        StartCoroutine(delay());
        IEnumerator delay()
        {
            if (targetAlpha == 1)
                yield return new WaitForSeconds(0.2f);

            fadeImg.CrossFadeColor(new Color(0, 0, 0, targetAlpha), fadeDuration, true, true);

            yield return new WaitForSeconds(fadeDuration);
            OnAnimCompleted?.Invoke();
            OnAnimCompleted = null;

            OnSceneLoadCompleted?.Invoke();
        }
    }
}
