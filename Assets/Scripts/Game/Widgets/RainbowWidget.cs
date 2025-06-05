using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainbowWidget : MonoBehaviour
{
    [SerializeField] private Animator[] m_animators;


    private void Awake()
    {
        RefsHolder.Register(this);
    }

    private void OnDestroy()
    {
        RefsHolder.SetNull(this);
    }

    public void Show(TargetType target, int state)
    {
        for (int i = 0; i < m_animators.Length; ++i)
        {
            if (i == (int)target)
            {
                m_animators[i].SetInteger("State", state);
            }
            else
            {
                m_animators[i].SetInteger("State", 0);
            }
        }
    }

    public void HideAll()
    {
        for (int i = 0; i < m_animators.Length; ++i)
        {
            m_animators[i].SetInteger("State", 0);
        }
    }
}
