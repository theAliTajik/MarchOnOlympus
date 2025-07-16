using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game;
using DG.Tweening;

public class MechanicsDisplay : MonoBehaviour
{
    [SerializeField] private Vector3 m_offset;
    [SerializeField] private Transform m_parent;
    [SerializeField] private MechanicAddList m_mechanicAddList;


    [SerializeField]
    private Dictionary<MechanicType, MechanicWidget> m_widgets = new Dictionary<MechanicType, MechanicWidget>();

    private MechanicsList m_mechanicsList;

    private class DisplayOperation
    {
        public MechanicType mechanicType;
        public bool Operation; // true = update, false = Remove

        public DisplayOperation(MechanicType type, bool operation)
        {
            this.mechanicType = type;
            this.Operation = operation;
        }
    }

    private Queue<DisplayOperation> m_DisplayQueue = new Queue<DisplayOperation>();
    private bool isProcessing = false;


    public void Configure(IHaveMechanics owner, MechanicsList mechanicsList)
    {
        m_mechanicAddList.Config(owner);

        m_mechanicsList = mechanicsList;
        m_mechanicsList.OnMechanicUpdated += OnMechanicsUpdated;
        m_mechanicsList.OnMechanicRemoved += OnMechanicsEnd;


        List<MechanicType> CurrentMechanics = m_mechanicsList.GetAllMechanics();
        if (CurrentMechanics.Count > 0)
        {
            foreach (MechanicType mechanicType in CurrentMechanics)
            {
                DisplayMechanic(mechanicType);
            }
        }
    }

    private void OnMechanicsUpdated(MechanicType mechanicType)
    {
        m_DisplayQueue.Enqueue(new DisplayOperation(mechanicType, true));
        StartCoroutine(ProcessNextOperation());
    }

    private void OnMechanicsEnd(MechanicType mechanicType)
    {
        m_DisplayQueue.Enqueue(new DisplayOperation(mechanicType, false));
        StartCoroutine(ProcessNextOperation());
    }

    private IEnumerator ProcessNextOperation()
    {
        if (m_DisplayQueue.Count == 0) yield break;

        isProcessing = true;
        DisplayOperation operation = m_DisplayQueue.Dequeue();
        bool updateMechanic = operation.Operation;

        bool animationDone = false;
        if (updateMechanic)
        {
            DisplayMechanic(operation.mechanicType);
            animationDone = true;
        }
        else
        {
            HideMechanic(operation.mechanicType, () => animationDone = true);
        }

        yield return new WaitUntil(() => animationDone);

        if (m_DisplayQueue.Count > 0)
        {
            StartCoroutine(ProcessNextOperation());
        }

        isProcessing = false;
    }

    private void DisplayMechanic(MechanicType mechanicType)
    {
        BaseMechanic mechanic = m_mechanicsList.GetMechanic(mechanicType);
        if (mechanic == null)
        {
            Debug.Log($"WARNING: Mechanic of type {mechanicType} could not be displayed. was not found in mechanic list");
            return;
        }
        
        int StackAmount = mechanic.Stack;

        bool hasWidget = m_widgets.TryGetValue(mechanicType, out MechanicWidget widget);
        if (hasWidget)
        {
            widget.transform.DOScale(Vector3.one, 0.25f);
            widget.SetCount(StackAmount);
        }
        else
        { 
            Sprite icon = MechanicsDb.Instance.FindByType(mechanicType).icon;
            MechanicWidget mechanicWidget = PoolMechanicWidget.Instance.GetItem();
            mechanicWidget.Config(icon, StackAmount);
#if UNITY_EDITOR
            mechanicWidget.name = "MechanicWidget " + mechanicType.ToString();
#endif

            Transform t = mechanicWidget.transform;
            t.SetParent(m_parent);
            t.localScale = Vector3.one;
            m_widgets.Add(mechanicType, mechanicWidget);
        }
    }

    private void HideMechanic(MechanicType mechanicType, Action finishCallback)
    {
        if (m_widgets.TryGetValue(mechanicType, out MechanicWidget widget))
        {
            widget.transform.DOScale(Vector3.zero, 0.25f).OnComplete(() =>
            {
                PoolMechanicWidget.Instance.ReturnToPool(widget);
                m_widgets.Remove(mechanicType);
                finishCallback?.Invoke();
            });
        }
    }

    public Vector3 GetOffset()
    {
        return m_offset;
    }
}