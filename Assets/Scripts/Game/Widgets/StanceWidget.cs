using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Game;
using DG.Tweening;

public class StanceWidget : MonoBehaviour
{
    [SerializeField] private Image[] m_selections;
    [SerializeField] private TMP_Text m_countText;
    [SerializeField] private float m_animationTime = 0.25f;
    
    private Stance m_currentstance = Stance.NONE;


    private int m_selectedIndex = -1;

    private void Awake()
    {
        RefsHolder.Register(this);
        GameplayEvents.OnStanceCdChanged += SetCount;
    }

    private void OnDestroy()
    {
        RefsHolder.SetNull(this);
        GameplayEvents.OnStanceCdChanged -= SetCount;
        GameplayEvents.StanceChanged -= ChangeStance;
    }

    private void Start()
    {
        Color c = Color.white;
        c.a = 0;
        for (int i = 0; i < m_selections.Length; i++)
        {
            m_selections[i].color = c;
        }

        GameplayEvents.StanceChanged += ChangeStance;
    }


    public void ChangeStance(Stance stance)
    {
        for (int i = 0; i < m_selections.Length; i++)
        {
            DOTween.Kill(m_selections[i]);
            m_selections[i].DOFade(0, m_animationTime);
        }
        
        m_selectedIndex = (int)stance -1;
        

        if (m_selectedIndex != -1)
        {
            DOTween.Kill(m_selections[m_selectedIndex]);
            m_selections[m_selectedIndex].DOFade(1, m_animationTime);
        }
        m_currentstance = stance;
    }

    public void SetCount(int count)
    {
        if (count >= 0)
        {
            m_countText.text = count.ToString();
        }
        else
        {
            m_countText.text = "\u221e";
        }
    }


    public void ButtonClicked(int index)
    {
        if ((Stance)index + 1 == m_currentstance)
        {
            index = -1;
        }
        CombatManager.Instance.OnStanceSelected((Stance)index + 1);
    }
}