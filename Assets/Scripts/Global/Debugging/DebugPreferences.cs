
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class DebugPreferences
{
    private static Dictionary<string, CategoryPreference> m_preferences;
    
    private static readonly string FilePath = 
        Path.Combine(UnityEngine.Application.persistentDataPath, "debug_prefs.json");
    
    
    static DebugPreferences()
    {
        Load();
    }

    public static bool IsCategoryActive(DebugCategory category)
    {
        return GetCategoryPreference(category)?.IsActive() ?? true;
    }

    public static bool IsTagEnabled(DebugCategory category, DebugTag tag)
    {
        CategoryPreference pref = GetCategoryPreference(category);
        bool isprefNull = pref == null;
        if (isprefNull)
        {
            // Debug.Log("Pref was null");
            return true;
        }
        
        bool hasTag = pref.DisabledTags.Contains(tag);
        
        // Debug.Log($"checking tag en. pref nul: {isprefNull}. tag: {tag.ToString()} has tag: {hasTag}");
        return !isprefNull && !hasTag; 
    }

    public static void ToggleCategoryActive(DebugCategory category)
    {
        bool isActive = IsCategoryActive(category);
        
        SetCategoryEnabled(category, !isActive);
    }

    public static void SetCategoryEnabled(DebugCategory category, bool enabled)
    {
        CategoryPreference pref = GetOrCreatePreference(category);
        pref.Enabled = enabled;
        Save();
    }

    public static void SetTagEnabled(DebugCategory category, DebugTag tag, bool enabled)
    {
        CategoryPreference pref = GetOrCreatePreference(category);
        if (!enabled) pref.DisabledTags.Add(tag);
        else pref.DisabledTags.Remove(tag);
        Save();
    }

    private static CategoryPreference? GetCategoryPreference(DebugCategory category)
    {
        m_preferences.TryGetValue(category.FullPath, out var pref);
        return pref;
    }

    private static CategoryPreference GetOrCreatePreference(DebugCategory category)
    {
        if (!m_preferences.TryGetValue(category.FullPath, out var pref))
        {
            pref = new CategoryPreference();
            m_preferences[category.FullPath] = pref;
            
            if(category.Parent != null)
                pref.Parent = GetOrCreatePreference(category.Parent);
        }
        return pref;
    }

    private static void Load()
    {
        m_preferences = JsonHelper.LoadAdvanced<Dictionary<string, CategoryPreference>>(FilePath);
        if (m_preferences == null)
        {
            m_preferences = new();
            CustomDebug.Log("Preferences are not loaded.", Categories.CustomDebug.Root);
        }
    }

    private static void Save()
    {
        JsonHelper.SaveAdvanced(m_preferences, FilePath);
    }
}

public class CategoryPreference
{
    public CategoryPreference Parent;
    public bool Enabled { get; set; } = true;
    public HashSet<DebugTag> DisabledTags { get; set; } = new();

    public bool IsActive()
    {
        if(Parent != null) return Parent.IsActive() && Enabled;

        return Enabled;
    }
}