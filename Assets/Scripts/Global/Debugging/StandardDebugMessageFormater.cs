
public class StandardDebugMessageFormater : IDebugMessageFormater
{
    private const string m_greyColor = "888888";
    private const string m_yellowColor = "EED202";
    private const string m_redColor = "D0342C";
    
    
    public string Format(string message, DebugCategory category, DebugTag tag = DebugTag.NONE, DebugLevel level = DebugLevel.LOG)
    {
        string catPathFormated = category.FullPath.Replace("Categories.", "");
        string color = m_greyColor;
        string prefix = "";

        switch (level)
        {
            case DebugLevel.WARNING:
                color = m_yellowColor;
                prefix = $"<color=#{color}>[Warning:]</color> ";
                break;
            case DebugLevel.ERROR:
                color = m_redColor;
                prefix = $"<color=#{color}>[Error:]</color> ";
                break;
        }

        if (tag != DebugTag.NONE)
        {
            prefix += $"[{tag.ToString()}] ";
        }
        
        string formated = $"<color=#{color}>[{catPathFormated}]</color> {prefix}<b>{message}</b>";
        return formated;
    }
}
