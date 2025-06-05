
using System;
using Unity.VisualScripting;

public interface IShopItemView
{
    public event Action<IShopItemModel> OnItemClicked;
    public void Display(IShopItemModel model);
}       