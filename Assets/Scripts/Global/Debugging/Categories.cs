
using System;
using System.Reflection;

public static class Categories
{

    static Categories()
    {
        InitializeAllNestedStatics(typeof(Categories));
    }

    private static void InitializeAllNestedStatics(Type type)
    {
        foreach (var nested in type.GetNestedTypes(BindingFlags.Public | BindingFlags.Static))
        {
            // Access all static fields to force initialization
            foreach (var field in nested.GetFields(BindingFlags.Public | BindingFlags.Static))
            {
                var _ = field.GetValue(null);
            }

            // Recursively initialize deeper nested static classes
            InitializeAllNestedStatics(nested);
        }
    }
    
    public static readonly DebugCategory Root = new("Categories");

    public static class CustomDebug
    {
        public static readonly DebugCategory Root = new("Custom Debug", Categories.Root);
    }

    public static class UI
    {
        public static readonly DebugCategory Root = new("UI", Categories.Root);

        public static readonly DebugCategory MainMenu = new("MainMenu", Root);
    }

    public static class Combat
    {
        public static readonly DebugCategory Root = new("Combat", Categories.Root);
        
        public static readonly DebugCategory Energy = new("Energy", Root);
        public static readonly DebugCategory Cards = new("Cards", Root);
    }
    
    public static class Fighters
    {
        public static readonly DebugCategory Root = new("Fighters", Categories.Root);

        public static class Enemies
        {
            public static readonly DebugCategory Root = new("Enemies", Fighters.Root);
            
            public static readonly DebugCategory Hydra = new("Hydra", Root);
            public static DebugCategory HydraHead = new("HydraHead", Hydra);

            public static DebugCategory Cyclops = new("Cyclops", Root);
            public static DebugCategory CyclopsClub = new("CyclopsClub", Cyclops);
            public static DebugCategory CyclopsEye = new("CyclopsEye", Cyclops);
            
            public static DebugCategory Phoenix = new("Phoenix", Root);
            
            public static DebugCategory Nymphs = new("Nymphs", Root);
        }

        public static class Player
        {
            public static readonly DebugCategory Root = new("Player", Fighters.Root);
        }
    }

    public static class Mechanics
    {
        public static readonly DebugCategory Root = new("Mechanics", Categories.Root);

        public static readonly DebugCategory Acidic = new("Acidic", Root);
        public static readonly DebugCategory AcidicDot = new("AcidicDot", Root);
        public static readonly DebugCategory AcidicDotTwo = new("AcidicDotTwo", Root);
    }

    public static class Perks
    {
        public static readonly DebugCategory Root = new("Perks", Categories.Root);

        public static readonly DebugCategory SaveAndLoad = new("SaveAndLoad", Root);
    }
    
    public static class Sound
    {
        public static readonly DebugCategory Root = new("Sound", Categories.Root);
        
        public static readonly DebugCategory Music = new("Music", Root);
        public static readonly DebugCategory SFX = new("SFX", Root);
    }

    public static class Invent
    {
        public static readonly DebugCategory Root = new("Invent", Categories.Root);
    }
}