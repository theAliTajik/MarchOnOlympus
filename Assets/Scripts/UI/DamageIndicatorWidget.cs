using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageIndicatorWidget : MonoBehaviour
{
    private const string ANIM_DROP = "Drop";
    private const string ANIM_IDLE = "Idle";

    [SerializeField] private AnimatorHelper m_animator;
    [SerializeField] private TMP_Text m_text;


    public void Play(int damage, Action finishCallback)
    {
        m_text.text = damage.ToString();
        m_animator.Play(ANIM_DROP, finishCallback);
    }
    
}
