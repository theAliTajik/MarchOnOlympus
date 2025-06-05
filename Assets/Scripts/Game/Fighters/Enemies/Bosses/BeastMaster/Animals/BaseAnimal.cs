using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BaseAnimal : BaseEnemy
{
    
    protected BeastMaster m_master;

    public void SetMaster(BeastMaster master)
    {
        m_master = master;
    }
    
    public virtual void SpecialMove(Fighter fighter)
    {
    }
}
