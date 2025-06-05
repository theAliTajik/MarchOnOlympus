using System;
using UnityEngine;
using UnityEngine.UI;

public class IntentionWidget : MonoBehaviour
{
    [SerializeField] private Image m_image;
    [SerializeField] private OnEnterDetector m_onEnterImageDetector;
    [SerializeField] private EnemyIntentionTooltip m_tooltip;
    [SerializeField] private Vector3 m_offset;
    
    [SerializeField] private Sprite m_attackSprite;
    [SerializeField] private Sprite m_blockSprite;
    [SerializeField] private Sprite m_tauntSprite;

    public void Config(BaseEnemy enemy)
    {
        enemy.OnIntentionDetermined += ActivateIntention;
    }

    public void Start()
    {
        m_onEnterImageDetector.OnEnter += ShowTooltip;
        m_onEnterImageDetector.OnExit += HideTooltip;
        HideTooltip();
    }

    public void ActivateIntention(Intention intention, string description)
    {
        m_image.gameObject.SetActive(true);
        m_tooltip.SetDescription(description);

        IntentionsDb.IntentionInfo info = IntentionsDb.Instance.FindByType(intention);
        m_image.sprite = info.icon;
    }

    public void HideIntention()
    {
        m_image.gameObject.SetActive(false);
    }

    public void ShowTooltip()
    {
        m_tooltip.gameObject.SetActive(true);
    }

    public void HideTooltip()
    {
        m_tooltip.gameObject.SetActive(false);
    }

    public Vector3 GetOffset()
    {
        return m_offset;
    }
}
