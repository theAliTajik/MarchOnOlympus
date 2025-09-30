using System.Collections.Generic;

public class DebugCategory
{
    public string Name { get; }
    public DebugCategory Parent { get; }
    public bool Enabled { get; set; } = true;
    public List<DebugCategory> Children { get; } = new();

    public DebugCategory(string name, DebugCategory parent = null)
    {
        // Name = parent == null ? name : $"{parent.Name}.{name}";
        Name = name;
        Parent = parent;

        Parent?.Children.Add(this);
    }

    // Active only if this AND all parents are enabled
    public bool IsActive(){
        if (Parent == null) return Enabled;
        
        return Parent.IsActive() && Enabled;
    }
    
    public string FullPath => Parent == null ? Name : $"{Parent.FullPath}.{Name}";

    public override string ToString() => Name;
}