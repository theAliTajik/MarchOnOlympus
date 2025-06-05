using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public abstract class BaseCardAction : MonoBehaviour
{
    public virtual void CardDrawn(BaseCardData cardData){}
    
    public virtual void Config(CardDisplay cardDisplay){}
    
    public abstract void Play(BaseCardData cardData, Action finishCallback, Fighter target, CardDisplay cardDisplay);

    public virtual void NotUsed(BaseCardData cardData){}


}

