using System.Collections.Generic;

public interface IDebugLogger
{
    void Log(string msg, DebugCategory cat, DebugTag tag = DebugTag.NONE);
    void LogWarning(string msg, DebugCategory cat, DebugTag tag = DebugTag.NONE);
    void LogError(string msg, DebugCategory cat, DebugTag tag = DebugTag.NONE);
}