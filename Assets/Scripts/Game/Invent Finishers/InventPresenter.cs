using System;
using System.Collections.Generic;
using UnityEngine;

public class InventPresenter : Singleton<InventPresenter>
{
    [SerializeField] private InventModel m_model;
    [SerializeField] private InventView m_view;

    private string m_clickedOnFinisher;

    private bool m_waitingForPlayerSelection = false;

    protected override void Init()
    {
    }

    private void Awake()
    {
        m_view.DisableAllButtons();
        m_view.OnClick += OnButtonClicked;
    }

    private void Start()
    {
        m_view.Configure(maxInventLevel:3, inventToLevelConversionRate:20);
        m_model.LoadFinishers();
        m_model.OnInventLevelChanged += OnInventLevelChange;
    }

    private void OnInventLevelChange(int inventLevel)
    {
        CustomDebug.Log("Invent level changed detected by presetner", Categories.Combat.Invent.Root);
        m_view.OnInventChanged(inventLevel);
        UpdateViewButtonsData();
    }

    private void UpdateViewButtonsData()
    {
        List<InventFinisher> finishers = m_model.GetAllLoadedFinishers();
        List<InventActionViewData> viewData = new();
        foreach (var finisher in finishers)
        {
            var Data = new InventActionViewData(finisher);
            viewData.Add(Data);
        }
        m_view.UpdateButtons(viewData);
    }

    private void OnButtonClicked(string id)
    {
        // CustomDebug.LogWarning($"finishers does not contain id of button: {id}", Categories.Combat.Invent.Root);
        if (m_waitingForPlayerSelection)
        {
            var finisher = m_model.GetFinisher(id);
            GameplayEvents.SendOnInventFinisherSelected(finisher);
            m_waitingForPlayerSelection = false;
            return;
        }
        
        TargetType targetType = m_model.GetFinisherTargetType(id);
        IDamageable target;
        if (targetType is not TargetType.ENEMY or TargetType.PLAYER_ENEMY)
        {
            m_model.ExecuteFinisher(id, null);
            return;
        }

        m_view.DisableAllButtons();
        CustomDebug.Log("Started selection of target", Categories.Combat.Invent.Root);
        m_clickedOnFinisher = id;
        EnemySelector.Instance.StartSelection();
        EnemySelector.Instance.OnTargetSelected += OnTargetSelected;
        EnemySelector.Instance.OnNoTargetSelected += OnNoTargetSelected;
    }

    private void OnDestroy()
    {
        if (EnemySelector.Instance)
        {
            EnemySelector.Instance.OnTargetSelected -= OnTargetSelected;
            EnemySelector.Instance.OnNoTargetSelected -= OnNoTargetSelected;
        }
    }

    private void OnNoTargetSelected()
    {
        //TODO: cansel button push
        m_view.ReEnableAllButtons();
    }

    private void OnTargetSelected(IDamageable target)
    {
        m_model.ExecuteFinisher(m_clickedOnFinisher, target);
        m_view.ReEnableAllButtons();
    }


    public List<InventFinisher> GetAllFinishers()
    {
        return m_model.GetAllLoadedFinishers();
    }

    public void GetPlayerSelection()
    {
        m_waitingForPlayerSelection = true;
        //TODO: Change buttons colors to indicate waiting for selection
    }
}