
using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class EnemySelector : Singleton<EnemySelector>
{
    public event Action<IDamageable> OnTargetSelected;
    public event Action OnNoTargetSelected;

    private bool m_selecting;
    
    
    public void StartSelection()
    {
        m_selecting = true;
        CustomDebug.Log("Started Selection", Categories.Combat.EnemySelector);
    }
    
    public void Update()
    {
        if(!m_selecting) return;
        
        if (!Input.GetMouseButtonDown(0)) return;
        
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(
            new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane)
        ); 
        
        RaycastHit2D hit = Physics2D.Raycast(mouseWorldPosition, Vector2.zero);

        if (hit.collider == null)
        {
            OnNoTargetSelected?.Invoke();
            CustomDebug.Log("No Taget selected", Categories.Combat.EnemySelector);
            m_selecting = false;
            return;
        }
        
        IDamageable target =  hit.collider.gameObject.GetComponentInParent<IDamageable>();
        if (target == null)
        {
            OnNoTargetSelected?.Invoke();
            CustomDebug.Log("No Taget selected", Categories.Combat.EnemySelector);
            m_selecting = false;
            return;
        }
        
        OnTargetSelected?.Invoke(target);
        CustomDebug.Log($"Taget selected: {target.GetType()}", Categories.Combat.EnemySelector);
        m_selecting = false;
    }

    protected override void Init()
    {
        
    }
}
