
public static class CustomDebug
{
    private static IDebugLogger m_debugLogger = new UnityLogger();
    
    public static void Log(string msg, DebugCategory cat, DebugTag tag = DebugTag.NONE)
    {
       m_debugLogger.Log(msg, cat, tag);
    }

    public static void LogWarning(string msg, DebugCategory cat, DebugTag tag = DebugTag.NONE)
    {
        m_debugLogger.LogWarning(msg, cat, tag);
    }
    
    public static void LogError(string msg, DebugCategory cat, DebugTag tag = DebugTag.NONE)
    {
        m_debugLogger.LogError(msg, cat, tag);
    }

    public static void SetLogger(IDebugLogger logger)
    {
        m_debugLogger = logger;
    }
}