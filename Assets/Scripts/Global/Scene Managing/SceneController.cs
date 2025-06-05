using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : Singleton<SceneController>
{
    private const float INITIAL_WAIT = 0.1f;
    private const float CHECK_DELAY = 0.1f;

    [SerializeField] private BaseTransition m_transition;

    private List<BaseSceneLoaderPart> m_loaders = new List<BaseSceneLoaderPart>();

    protected override void Init()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }


    public void LoadScene(Scenes scene)
    {
        m_transition.ComeIn(() =>
        {
            string sceneName = GetSceneName(scene);
            SceneManager.LoadScene(sceneName);
        });
    }

    private string GetSceneName(Scenes scene)
    {
        return m_sceneNames[(int)scene];
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode loadMode)
    {
        if (scene.buildIndex == 0)
        {
            return;
        }

        StartCoroutine(FinishTransition());
    }

    private IEnumerator FinishTransition()
    {
        yield return new WaitForSeconds(INITIAL_WAIT);

        while (m_loaders.Count > 0)
        {
            yield return new WaitForSeconds(CHECK_DELAY);
        }

        m_transition.GoOut(null);
    }

    public void Register(BaseSceneLoaderPart baseSceneLoaderPart)
    {
        m_loaders.Add(baseSceneLoaderPart);
    }

    public void Unregister(BaseSceneLoaderPart baseSceneLoaderPart)
    {
        m_loaders.Remove(baseSceneLoaderPart);
    }

    private string[] m_sceneNames = new string[] {
        "Menu",
        "Map",
        "Game",
        "Event",
        "Shop",
        "AdvancedShop",
        "Store"
    };
    
    
}


public enum Scenes
{
    Menu,
    Map,
    Game,
    Event,
    Shop,
    AdvancedShop,
    Store,
}
