
using UnityEngine;

[CreateAssetMenu(fileName = "RemoveRandPerkAction", menuName = "Events/DialogueActions/RemoveRandPerkAction")]
public class RemoveRandPerkDialogueAction : DialogueAction
{
    public override void Execute(DialogueContex context)
    {
        base.Execute(context);
        if (GameProgress.Instance)
        {
            int randIndex = UnityEngine.Random.Range(0, GameProgress.Instance.Data.PerkIds.Count);
            GameProgress.Instance.Data.PerkIds.RemoveAt(randIndex);
            GameProgress.Instance.Save();
        }
    }
}
