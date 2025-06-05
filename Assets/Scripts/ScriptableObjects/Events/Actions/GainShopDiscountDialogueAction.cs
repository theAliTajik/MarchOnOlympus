
using Game.ModifiableParam;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "GainShopDiscountAction", menuName = "Events/DialogueActions/GainShopDiscountAction")]
public class GainShopDiscountDialogueAction : DialogueAction
{
    [SerializeField] private int m_shopDiscountPercentage;
    public override void Execute(DialogueContex context)
    {
        base.Execute(context);
        IParamModifier<int> reducePrice = new PercentageModifier(m_shopDiscountPercentage);
        GameProgress.Instance.Data.ShopModifiers.Add(reducePrice);
        GameProgress.Instance.Save();
    }
}
