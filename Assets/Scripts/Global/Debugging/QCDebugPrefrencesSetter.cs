using QFSW.QC;
using UnityEngine;

public static class QCDebugPrefrencesSetter
{
    private static DebugCategory m_selectedCategory;

    [Command("choise-test-of-root")]
    public static void SelectRoot()
    {
        ListChildrenForSelection(Categories.Root);
        m_selectedCategory = Categories.Root;
    }

    private static void SelectCat(DebugCategory cat)
    {
        m_selectedCategory = cat;
        
        CustomDebug.Log($"Selected cat: {m_selectedCategory.Name}. children count: {m_selectedCategory.Children.Count}", Categories.CustomDebug.Root);
        if (m_selectedCategory.Children.Count <= 0) // Give option for cat
        {
            ListOptionForDebugCategory(m_selectedCategory);
        }
        
        // List Children for Selection
        ListChildrenForSelection(m_selectedCategory);
        return;
    }
    
    public static void ListChildrenForSelection(DebugCategory category)
    {
        string msg = "\n";
        int currentNum = 0;
        foreach (var categoryChild in category.Children)
        {
            currentNum++;
            msg += currentNum + ". " + categoryChild.Name + "\n";
        }

        Debug.Log(msg);
    }

    private static void ListOptionForDebugCategory(DebugCategory category)
    {
        CustomDebug.Log($"Giving options for cat", Categories.CustomDebug.Root);
        string msg = "\n";
        int currentNum = 1;

        bool isCategoryActive = DebugPreferences.IsCategoryActive(category);

        if (isCategoryActive)
        {
            msg += currentNum + ". " + "DeActivate" + "\n";
        }
        else
        {
            msg += currentNum + ". " + "Activate" + "\n";
        }

        currentNum++;
        msg += currentNum + ". " + "Tags" + "\n";
        Debug.Log(msg);
    }

    [Command("s")]
    public static void DispatchSelection(int selectNum)
    {
        if (m_selectedCategory == null)
        {
            CustomDebug.Log("Selected cat null", Categories.CustomDebug.Root);
            return;
        }

        if (m_selectedCategory.Children.Count <= 0)
        {
            SelectOptionsOfCurrentCat(selectNum);
        }
        else
        {
            SelectChildOfCurrentCat(selectNum);
        }

    }

    private static void SelectChildOfCurrentCat(int selectNum)
    {
        selectNum--;
        bool isValidSelect = isValidIndex(m_selectedCategory, selectNum);
        if (!isValidSelect)
        {
            CustomDebug.Log($"Input num is Invalid. num {selectNum}", Categories.CustomDebug.Root);
            return;
        }
            
        m_selectedCategory = m_selectedCategory.Children[selectNum];
        
        CustomDebug.Log($"Selected cat: {m_selectedCategory.Name}. children count: {m_selectedCategory.Children.Count}", Categories.CustomDebug.Root);
        if (m_selectedCategory.Children.Count <= 0) // Give option for cat
        {
            ListOptionForDebugCategory(m_selectedCategory);
        }
        
        // List Children for Selection
        ListChildrenForSelection(m_selectedCategory);
        return;
    }

    private static void SelectOptionsOfCurrentCat(int selectNum)
    {
        CustomDebug.Log($"Select options for cat: {m_selectedCategory.Name}", Categories.CustomDebug.Root);

        if (selectNum == 1)
        {
            DebugPreferences.ToggleCategoryActive(m_selectedCategory);
        }

        if (selectNum == 2)
        {
            CustomDebug.Log("Show Tags", Categories.CustomDebug.Root);
        }
    }

    private static bool isValidIndex(DebugCategory category, int index)
    {
        bool hasChildren = category.Children.Count != 0;
        bool isInsideChildren = index < 0 || index >= category.Children.Count;

        return true;
    }
    
    
    [Command("Disable-cyclops-logs")]
    public static void SetCategoryEnabled()
    {
        DebugPreferences.SetCategoryEnabled(Categories.Fighters.Enemies.Cyclops, false);
    }
}

