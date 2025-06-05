using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class MechanicsTester : MonoBehaviour
{
    public Fighter Player;
    public Fighter Enemy;
    
    public FighterHP PlayerHP;
    public FighterHP EnemyHP;
    
    public MechanicsDisplay PlayerMechanicsDisplay;
    public MechanicsDisplay EnemyMechanicsDisplay;
    
    public TMP_Text PlayerHPText;
    public TMP_Text EnemyHPText;
    
    public TMP_Dropdown m_selectedMechanics;
    
    private BaseMechanic m_baseMechanic = null;
    private Fighter m_FighterToApplyTo = null;

    private void Start()
    {
        m_FighterToApplyTo = Player;
        // Manager.AddMechanic(Player, MechanicType.STRENGTH, new StrenghtMechanic(12, Player));
        // Manager.AddMechanic(Player, MechanicType.DEXTERITY, new DexterityMechanic(15, Player));
        // Manager.AddMechanic(Player, MechanicType.BLOCK, new BlockMechanic(5, Player));
        // Manager.AddMechanic(Player, MechanicType.FORTIFIED, new FortifiedMechanic(Player));
        // Manager.AddMechanic(Enemy, MechanicType.STRENGTH, new StrenghtMechanic(5, Enemy));
        
        
        // PlayerMechanicsDisplay.Configure(Manager.GetMechanicsList(Player));
        // EnemyMechanicsDisplay.Configure(Manager.GetMechanicsList(Enemy));

    }

    public void AddSelectedMechanic()
    {
        
    }

    private void Update()
    {
        // update display
        if (PlayerHP != null && EnemyHP != null)
        {
            PlayerHPText.text = PlayerHP.Current.ToString();
            EnemyHPText.text = EnemyHP.Current.ToString();    
        }
        

        
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Debug.Log("enemy damaged player");
            Player.TakeDamage(5, Enemy, true);
        }
        
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Debug.Log("Player damaged Enemy");
            Enemy.TakeDamage(5, Player, true);
        }
        
        SetMechanicBasedOnKeyInput();

        if (m_baseMechanic == null)
        {
            return;
        }
        
        if (Input.GetKeyDown(KeyCode.Plus) || Input.GetKeyDown(KeyCode.Equals))
        {
            MechanicsManager.Instance.AddMechanic(m_baseMechanic);    
        }

        if (Input.GetKeyDown(KeyCode.Minus))
        {
            MechanicsManager.Instance.ReduceMechanic(m_FighterToApplyTo, m_baseMechanic.GetMechanicType(), 5);    
        }
        
    }

    private void SetMechanicBasedOnKeyInput()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            m_FighterToApplyTo = Enemy;
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            m_FighterToApplyTo = Player;
        }
        
        if (Input.GetKeyDown(KeyCode.S))
        {
            m_baseMechanic = new StrenghtMechanic(5, m_FighterToApplyTo);
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            m_baseMechanic = new BlockMechanic(5, m_FighterToApplyTo);
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            m_baseMechanic = new FortifiedMechanic(1, m_FighterToApplyTo);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            m_baseMechanic = new DexterityMechanic(5, m_FighterToApplyTo);
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            m_baseMechanic = new ThornsMechanic(5, m_FighterToApplyTo);
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            m_baseMechanic = new FrenzyMechanic(5, m_FighterToApplyTo);
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            m_baseMechanic = new ImpaleMechanic(5, m_FighterToApplyTo);
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            m_baseMechanic = new BleedMechanic(5, m_FighterToApplyTo);
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            m_baseMechanic = new BurnMechanic(5, m_FighterToApplyTo);
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            m_baseMechanic = new DazeMechanic(5, m_FighterToApplyTo);
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            m_baseMechanic = new StunMechanic(1, m_FighterToApplyTo);
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            m_baseMechanic = new VulnerableMechanic(5, m_FighterToApplyTo);
        }
    }
}
