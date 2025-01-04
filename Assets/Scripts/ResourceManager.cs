public static class ResourceManager
{
    public static float Sun { get; private set; }
    public static float Reactives { get; private set; }


    public static void InitializeResources(float initialSun, float initialReactives)
    {
        Sun = initialSun;
        Reactives = initialReactives;
        EventBus.RaiseOnSunChange(Sun);
        EventBus.RaiseOnReactiveChange(Reactives);
    }
    public static void AddSun(float amount)
    {
        Sun += amount;
        EventBus.RaiseOnSunChange(Sun);
    }
    public static void AddReactives(float amount)
    {
        Reactives += amount;
        EventBus.RaiseOnReactiveChange(Reactives);
    }

    public static bool TrySpendSun(float amount)
    {
        if (Sun >= amount)
        {
            Sun -= amount;
            EventBus.RaiseOnSunChange(Sun);
            return true;
        }
        return false;
    }

    public static bool TrySpendReactives(float amount)
    {
        if (Reactives >= amount)
        {
            Reactives -= amount;
            EventBus.RaiseOnReactiveChange(Reactives);
            return true;
        }
        return false;
    }
}