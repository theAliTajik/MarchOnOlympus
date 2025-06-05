
using UnityEngine;

[CreateAssetMenu(fileName = "GainRandPerkAction", menuName = "Events/DialogueActions/GainRandPerkAction")]
public class GainRandPerkDialogueAction : DialogueAction
{
    public override void Execute(DialogueContex context)
    {
        base.Execute(context);

        PerksDb.PerksInfo randPerkData;
        randPerkData = PerksDb.Instance.GetRandPerk();
        
        if (GameProgress.Instance)
        {
            GameProgress.Instance.Data.PerkIds.Add(randPerkData.ClientID);
            GameProgress.Instance.Save();
        }
    }
}
