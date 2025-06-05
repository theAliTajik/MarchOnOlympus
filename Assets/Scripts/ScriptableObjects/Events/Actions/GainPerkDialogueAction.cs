using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;


[CreateAssetMenu(fileName = "GainPerkAction", menuName = "Events/DialogueActions/GainPerkAction")]
public class GainPerkDialogueAction : DialogueAction
{
    [SerializeField] private string m_perkId;

    public override void Execute(DialogueContex context)
    {
        if (string.IsNullOrEmpty(m_perkId))
        {
            Debug.Log("string was null or empty");
            return;
        }

        GameProgress.Instance.Data.PerkIds.Add(m_perkId);
        GameProgress.Instance.Save();
        Debug.Log("added perk: " + m_perkId);
    }

}
