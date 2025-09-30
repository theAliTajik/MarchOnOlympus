using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class HUD : Singleton<HUD>
{
    [SerializeField] private HealthBar m_healthBarPrefab;
    [SerializeField] private HealthBar m_enemyHealthBarPrefab;
    [SerializeField] private MechanicsDisplay m_mechanicsDisplayPrefab;   
    [SerializeField] private DamageIndicator m_damageIndicatorPrefab;
    [SerializeField] private IntentionWidget m_intentionWidgetPrefab;
    [SerializeField] private EnemyIntentionTooltip m_intentionTooltipPrefab;

    [SerializeField] private GameOverPanel m_gameOverPanel;
    [SerializeField] private GameObject m_devHud;
    [SerializeField] private CardDisplayList m_cardDisplayList;
    
    [SerializeField] private Deck m_Deck;

    [SerializeField] private EnergyWidget m_energyWidget;
    [SerializeField] private EndTurnWidget m_endTurnWidget;

    [SerializeField] private TMP_Text m_drawPileCardAmount;
    [SerializeField] private TMP_Text m_discardPileCardAmount;
    

    private RectTransform m_rectTransform;
    private Camera m_mainCamera;
    
    public enum HudObjectType
    {
        HEALTHBAR,
        MECHANICS_DISPLAY,
        INTENTION_WIDGET,
        DAMAGE_INDICATOR
    }
    private Dictionary<IHaveHUD, Dictionary<HudObjectType, GameObject>> m_hudObjects = new Dictionary<IHaveHUD, Dictionary<HudObjectType, GameObject>>();


    public void Start()
    {
        // turn off all panels and popups
        m_gameOverPanel.gameObject.SetActive(false);
        m_devHud.gameObject.SetActive(false);
        m_cardDisplayList.gameObject.SetActive(false);
        
        
        
        UpdateDrawPileCardAmount(m_Deck.CardPiles[CardStorage.DRAW_PILE].Cards.Count);
        UpdateDiscardPileCardAmount(m_Deck.CardPiles[CardStorage.DISCARD_PILE].Cards.Count);
        SpawnHPBar(CombatManager.Instance.Player);
        SpawnDamageIndicator(CombatManager.Instance.Player);
    }

    public void GameOver(bool victory)
    {
        m_devHud.gameObject.SetActive(false);
        m_endTurnWidget.SetState(false);
        m_energyWidget.SetAnimation(false);
        m_gameOverPanel.gameObject.SetActive(true);
        m_gameOverPanel.DisplayEndGame(victory);
    }

    protected override void Init()
    {
        m_rectTransform = transform as RectTransform;
        m_mainCamera = Camera.main;
        m_Deck.OnDrawPileChanged += UpdateDrawPileCardAmount;
        m_Deck.OnDiscardPileChanged += UpdateDiscardPileCardAmount;
    }

    public HealthBar SpawnHPBar(Fighter fighter)
    {
        HealthBar hpBar;
        if (fighter.CompareTag("Player"))
        {
            hpBar = Instantiate(m_healthBarPrefab, transform);   
        }
        else
        {
            hpBar = Instantiate(m_enemyHealthBarPrefab, transform);
        }
         
        hpBar.Config(fighter);

        RectTransform rectTransform = hpBar.transform as RectTransform;
        rectTransform.anchoredPosition = ConvertWorldPosition(fighter.GetRootPosition() + hpBar.GetOffset(), rectTransform.anchorMin, rectTransform.anchorMax);
#if UNITY_EDITOR
        hpBar.name = "HealthBar " + fighter.name;
#endif
        AddHudObject(fighter, HudObjectType.HEALTHBAR, hpBar.gameObject);
        return hpBar;
    }
    
    public HealthBar SpawnHPBar(IHaveHUD owner)
    {
        if (owner is not IHaveHP HpOwner)
        {
            Debug.Log("ERROR: tried to Spawn HP bard for hud owner that does not have HP");
            return null;
        }
        
        HealthBar hpBar;
        hpBar = Instantiate(m_enemyHealthBarPrefab, transform);
         
        hpBar.Config(HpOwner);

        RectTransform rectTransform = hpBar.transform as RectTransform;
        rectTransform.anchoredPosition = ConvertWorldPosition(owner.GetRootPosition() + hpBar.GetOffset(), rectTransform.anchorMin, rectTransform.anchorMax);
        
        AddHudObject(owner, HudObjectType.HEALTHBAR, hpBar.gameObject);
        return hpBar;
    }
    
    public void RemoveHPBar(Fighter enemy)
    {
        RemoveHudObject(enemy, HudObjectType.HEALTHBAR);
    }

    public MechanicsDisplay SpawnMechanicsDisplay(Fighter fighter, MechanicsList mechanicsList)
    {
        return SpawnMechanicsDisplay(fighter as IHaveHUD, mechanicsList);
    }
    public MechanicsDisplay SpawnMechanicsDisplay(IHaveHUD owner, MechanicsList mechanicsList)
    {
        MechanicsDisplay mechanicsDisplay = Instantiate(m_mechanicsDisplayPrefab, transform);
        mechanicsDisplay.Configure(owner as IHaveMechanics, mechanicsList);
        RectTransform rectTransform = mechanicsDisplay.transform as RectTransform;
        rectTransform.anchoredPosition = ConvertWorldPosition(owner.GetRootPosition() + mechanicsDisplay.GetOffset(), rectTransform.anchorMin, rectTransform.anchorMax);

#if UNITY_EDITOR
        mechanicsDisplay.name = "Mechanics Display " + owner.GetType();
#endif
        AddHudObject(owner, HudObjectType.MECHANICS_DISPLAY, mechanicsDisplay.gameObject);
        return mechanicsDisplay;
    }
    public void RemoveMechanicsDisplay(Fighter fighter)
    {
        RemoveHudObject(fighter, HudObjectType.MECHANICS_DISPLAY);    
    }
    
    public IntentionWidget SpawnEnemyIntentionWidget(BaseEnemy enemy)
    {
        IntentionWidget intentionWidget = Instantiate(m_intentionWidgetPrefab, transform);
        intentionWidget.Config(enemy);

        RectTransform rectTransform = intentionWidget.transform as RectTransform;
        rectTransform.anchoredPosition = ConvertWorldPosition(enemy.GetHeadPosition() + intentionWidget.GetOffset(), rectTransform.anchorMin, rectTransform.anchorMax);
#if UNITY_EDITOR
        intentionWidget.name = "Intention " + enemy.name;
#endif
        AddHudObject(enemy, HudObjectType.INTENTION_WIDGET, intentionWidget.gameObject);
        return intentionWidget;
    }
    public IntentionWidget SpawnEnemyIntentionWidget(IHaveIntention iHaveIntention, Vector3 position)
    {
        IntentionWidget intentionWidget = Instantiate(m_intentionWidgetPrefab, transform);
        intentionWidget.Config(iHaveIntention);

        RectTransform rectTransform = intentionWidget.transform as RectTransform;
        rectTransform.anchoredPosition = ConvertWorldPosition(position + intentionWidget.GetOffset(), rectTransform.anchorMin, rectTransform.anchorMax);
        AddHudObject(iHaveIntention, HudObjectType.INTENTION_WIDGET, intentionWidget.gameObject);
        return intentionWidget;
    }
    public void RemoveEnemyIntentionWidget(Fighter fighter)
    {
        RemoveHudObject(fighter, HudObjectType.INTENTION_WIDGET);
    }
    
    public DamageIndicator SpawnDamageIndicator(Fighter fighter)
    {
        DamageIndicator damageIndicator = Instantiate(m_damageIndicatorPrefab, transform);
        damageIndicator.Config(fighter);
        damageIndicator.transform.position = fighter.GetHeadPosition();
#if UNITY_EDITOR
        damageIndicator.name = "DamageIndicator " + fighter.name;
#endif
        AddHudObject(fighter, HudObjectType.DAMAGE_INDICATOR, damageIndicator.gameObject);
        return damageIndicator;
    }
    
    public DamageIndicator SpawnDamageIndicator(IHaveHUD owner)
    {
        DamageIndicator damageIndicator = Instantiate(m_damageIndicatorPrefab, transform);
        damageIndicator.Config(owner);
        damageIndicator.transform.position = owner.GetHeadPosition();
        AddHudObject(owner, HudObjectType.DAMAGE_INDICATOR, damageIndicator.gameObject);
        return damageIndicator;
    }
    public void RemoveDamageIndicator(Fighter fighter)
    {
        RemoveHudObject(fighter, HudObjectType.DAMAGE_INDICATOR);
    }

    public void AddHudObject(IHaveHUD hudOwner, HudObjectType hudObjectType, GameObject hudObject)
    {
        if (m_hudObjects.ContainsKey(hudOwner))
        {
            m_hudObjects[hudOwner].Add(hudObjectType, hudObject);
        }
        else
        {
            m_hudObjects.Add(hudOwner, new Dictionary<HudObjectType, GameObject>(){ { hudObjectType, hudObject } });
        }
    }

    public void RemoveHudObject(IHaveHUD hudOwner, HudObjectType hudObjectType)
    {
        if (!m_hudObjects.ContainsKey(hudOwner))
        {
            Debug.Log("WARNING: tried to remove hud of non existent fighter");
            return;
        }
        
        if (!m_hudObjects[hudOwner].ContainsKey(hudObjectType))
        {
            Debug.Log($"WARNING: hudOwner did not have hud of type: {hudObjectType.ToString()}");
            return;
        }
        
        Destroy(m_hudObjects[hudOwner][hudObjectType]);
        m_hudObjects[hudOwner].Remove(hudObjectType);
    }


    public void SetEnergyWidgetAnimation(bool setActive)
    {
        m_energyWidget.SetAnimation(setActive);
    }

    public void SetEndTurnWidgetAnimation(bool setActive)
    {
        m_endTurnWidget.SetState(setActive);
    }

    public void UpdateDrawPileCardAmount(int amount)
    {
        m_drawPileCardAmount.text = amount.ToString();
    }

    public void UpdateDiscardPileCardAmount(int amount)
    {
        m_discardPileCardAmount.text = amount.ToString();
    }

    public Vector3 ConvertWorldPosition(Vector3 worldPos)
    {
        Vector3 viewPort = m_mainCamera.WorldToViewportPoint(worldPos);
        Vector3 canvasPos = new Vector3();
        canvasPos.x = viewPort.x * m_rectTransform.rect.width;
        canvasPos.y = viewPort.y * m_rectTransform.rect.height;
        return canvasPos;
    }

    public Vector3 ConvertWorldPosition(Vector3 worldPos, Vector2 anchorMin, Vector2 anchorMax)
    {
        Vector3 viewPort = m_mainCamera.WorldToViewportPoint(worldPos);

        float x = (anchorMin.x + anchorMax.x) / 2f;
        float y = (anchorMin.y + anchorMax.y) / 2f;
        viewPort.x -= x;
        viewPort.y -= y;

        Vector3 canvasPos = new Vector3();
        canvasPos.x = viewPort.x * m_rectTransform.rect.width;
        canvasPos.y = viewPort.y * m_rectTransform.rect.height;

        return canvasPos;
    }

    public void OnDrawPileButtonClicked()
    {
        m_cardDisplayList.AddCards(m_Deck.GetCardPile(CardStorage.DRAW_PILE));
        m_cardDisplayList.gameObject.SetActive(true);
    }
    
    public void OnDiscardPileButtonClicked()
    {
        m_cardDisplayList.AddCards(m_Deck.GetCardPile(CardStorage.DISCARD_PILE));
        m_cardDisplayList.gameObject.SetActive(true);
    }

}
