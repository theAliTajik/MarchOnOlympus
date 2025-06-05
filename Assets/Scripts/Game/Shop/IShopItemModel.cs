
using System;

public interface IShopItemModel
{
    public event Action OnDataChanged;
    public int NumOfPurchases { get; set; }
    public bool Sellable { get; set; }
    public IPrice Price { get; set; }
    
    public bool Purchase();
}
