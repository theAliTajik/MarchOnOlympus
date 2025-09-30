
using UnityEngine;

public class UnityLogger : IDebugLogger
{
    private IDebugMessageFormater m_msgFormater = new StandardDebugMessageFormater();
    private bool tagEnabled;
    private bool catEnabled;
    
    public void Log(string msg, DebugCategory cat, DebugTag tag = DebugTag.NONE)
    {
        CheckConditions(cat, tag);
        
        // Debug.Log($"tag en: {tagEnabled}, cat en: {catEnabled}, msg: {msg}");
        
        if (tagEnabled && catEnabled)
        {
            string formated = m_msgFormater.Format(msg, cat, tag, DebugLevel.LOG);
            UnityEngine.Debug.Log(formated);
        }
    }

    public void LogWarning(string msg, DebugCategory cat, DebugTag tag = DebugTag.NONE)
    {
        CheckConditions(cat, tag);
        
        if (tagEnabled && catEnabled)
        {
            string formated = m_msgFormater.Format(msg, cat, tag, DebugLevel.WARNING);
            UnityEngine.Debug.Log(formated);
        }
    }
    
    public void LogError(string msg, DebugCategory cat, DebugTag tag = DebugTag.NONE)
    {
        CheckConditions(cat, tag);
        
        if (tagEnabled && catEnabled)
        {
            string formated = m_msgFormater.Format(msg, cat, tag, DebugLevel.ERROR);
            UnityEngine.Debug.Log(formated);
        }
    }

    private void CheckConditions(DebugCategory cat, DebugTag tag)
    {
        tagEnabled = DebugPreferences.IsTagEnabled(cat, tag);
        catEnabled = DebugPreferences.IsCategoryActive(cat);
    }
}
