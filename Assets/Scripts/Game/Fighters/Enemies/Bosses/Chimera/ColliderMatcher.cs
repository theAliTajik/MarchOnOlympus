
using System;
using UnityEngine;

public interface IColliderMatcher
{
    public bool IsMyCollider(Collider2D collider);
}

[Serializable]
public class ColliderMatcher : IColliderMatcher
{
    [SerializeField] private Collider2D m_collider;
    
    public Collider2D Collider => m_collider;
    
    public bool IsMyCollider(Collider2D targetedCollider)
    {
        if (targetedCollider == null)
        {
            Debug.Log("ERROR: Null collider given");
            return false;
        }

        if (targetedCollider == m_collider)
        {
            return true;
        }
        
        return false;
    }
}
