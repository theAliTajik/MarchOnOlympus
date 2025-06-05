using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EventController : Singleton<EventController>
{
    private void Start()
    {
        MapNodeSaveManager.MarkNodeCompleted();
    }

    public void OnBackToMapClicked()
    {
        FinishEvent();
    }

    public void FinishEvent()
    {
        SceneController.Instance.LoadScene(Scenes.Map);
    }

    protected override void Init()
    {
        
    }
}
