
public struct InventActionViewData
{
    public InventActionViewData(InventFinisher finisher)
    {
        Id = finisher.ID;
        var action = finisher.GetInventAction();

        Active = false;
        ToolTip = "";
        if (action != null)
        {
            Active = true;
            ToolTip = action.ToolTip;
        }
    }
    
    public string Id;
    public string ToolTip;
    public bool Active;
}
