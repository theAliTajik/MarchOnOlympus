public static class MenuEventBus
{
    public static event System.Action<string> OnDeckSelected;
    public static void SendOnDeckSelected(string deckName)
    {
        OnDeckSelected?.Invoke(deckName);
    }
    
    public static event System.Action<string> OnCharacterSelected;
    public static void SendOnCharacterSelected(string characterName)
    {
        OnCharacterSelected?.Invoke(characterName);
    }
}
