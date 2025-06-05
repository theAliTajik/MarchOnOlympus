using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeTransition : BaseTransition
{
    [SerializeField] private Image m_image;
    [SerializeField] private float m_time;

    private bool m_isIn;
    private bool m_isInit;
    private Color m_transparent;


    public override void ComeIn(Action callback)
    {
        m_isIn = true;

        if (!m_isInit)
        {
            m_transparent = m_image.color;
            m_transparent.a = 0;
            m_isInit = true;
        }

        gameObject.SetActive(true);
        m_image.DOFade(1, m_time).OnComplete(() => callback?.Invoke());
    }

    public override void GoOut(Action callback)
    {
        m_isIn = false;
        m_image.DOFade(0, m_time).OnComplete(() =>
        {
            gameObject.SetActive(false);
            callback?.Invoke();
        });
    }

    public override bool IsIn()
    {
        return m_isIn;
    }
}