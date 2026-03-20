using System;
using System.Reflection;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class DebugCommandAttribute : Attribute
{
    public string CommandId { get; }
    public string Description { get; }

    public DebugCommandAttribute(string id, string desc)
    {
        CommandId = id.ToLower();
        Description = desc;
    }
}
public class DebugCommandBase
{
    public string commandId;
    public string description;
    public MethodInfo method;
    public object targetInstance;

    public ParameterInfo[] parameters => method.GetParameters();

    public DebugCommandBase(string id, string desc, MethodInfo m, object instance)
    {
        commandId = id;
        description = desc;
        method = m;
        targetInstance = instance;
    }

    public void Invoke(object[] args)
    {
        method.Invoke(targetInstance, args);
    }
}