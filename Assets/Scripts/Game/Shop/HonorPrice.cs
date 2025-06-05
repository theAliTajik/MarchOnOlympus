
public class HonorPrice : IPrice
{
    private const string m_currency = "Honor";
    private int m_price;
    
    public int GetPrice()
    {
        return m_price;
    }

    public void SetPrice(int price)
    {
        m_price = price;
    }

    public string GetCurrency()
    {
        return m_currency;
    }

    public bool HasEnoughMoney()
    {
        return GameProgress.Instance.Data.Honor >= m_price;
    }

    public bool ReduceCost()
    {
        if (HasEnoughMoney())
        {
            GameProgress.Instance.Data.Honor -= m_price;
            return true;
        }
        
        return false;
    }
    
}
