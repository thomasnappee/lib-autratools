using Unity;

public class BacktestEnvironment
{
    private UnityContainer environment = new();

    private Dictionary<Type, List<string>> entityNameByType = new();

    public BacktestEnvironment()
    {
        environment.AddExtension(new Diagnostic());
    }

    public string RegisterEntityType<I, T>() where T : I
    {
        var id = Register(typeof(I), typeof(T));
        environment.RegisterType<I, T>(id);
        return id;
    }

    public string RegisterEntityInstance<I>(I newObject)
    {
        var id = Register(typeof(I), newObject.GetType());
        environment.RegisterInstance(
            id, 
            newObject);
        return id;
    }

    public T? Get<T>(string id)
    {
        return environment.Resolve<T>(id);
    }

    private string Register(Type interfaceType, Type type)
    {
        string id;
        if (entityNameByType.ContainsKey(interfaceType))
        {
            var list = entityNameByType[interfaceType];
            id = $"{interfaceType}-{type}-{list.Count}";
            list.Add(id);
        }
        else
        {
            id = $"{interfaceType}-{type}-0";
            entityNameByType.Add(interfaceType, new List<string>() { id });
        }

        return id;
    }
}