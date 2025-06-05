using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;


[CreateAssetMenu(fileName = "GiveHonorAction", menuName = "Events/DialogueActions/GiveHonorAction")]
public class GiveHonorDialogueAction : DialogueAction
{
    [SerializeField] private int m_honorAmount;
    
    public override void Execute(DialogueContex context)
    {
        GameProgress.Instance.Data.Honor += m_honorAmount;
    }
    
    
}
