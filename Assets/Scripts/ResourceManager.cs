public static class ResourceManager
{
    public static float Sun { get; private set; }

    public static void InitializeSun(float initialSun)
    {
        Sun = initialSun;
    }

    public static void AddSun(float amount)
    {
        Sun += amount;
    }

    public static bool TrySpendSun(float amount)
    {
        if (Sun >= amount)
        {
            Sun -= amount;
            return true;
        }
        return false;
    }
}