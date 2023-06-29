using UnityEngine;

public class LabelAttribute : PropertyAttribute
{
    private readonly string name = "";

    public string Name { get { return name; } }

    public LabelAttribute(string name)
    {
        this.name = name;
    }
}