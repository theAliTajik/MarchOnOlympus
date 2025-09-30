
public interface IDebugMessageFormater
{
    public string Format(string message, DebugCategory category, DebugTag tag = DebugTag.NONE, DebugLevel level = DebugLevel.LOG);
}
