using System;

public class StatConfig
{
    public string name;
    public string label;
    public StatType type;
    public Func<string> getValue;

    public StatConfig(StatType type, string name, string label, Func<string> getFormattedValueMethod)
    {
        this.type = type;
        this.name = name;
        this.label = label;
        getValue = getFormattedValueMethod;
    }
}