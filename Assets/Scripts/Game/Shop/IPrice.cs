 
public interface IPrice
{
    public int GetPrice();
    public void SetPrice(int price);
    public string GetCurrency();
    
    public bool HasEnoughMoney();
    public bool ReduceCost();
}   