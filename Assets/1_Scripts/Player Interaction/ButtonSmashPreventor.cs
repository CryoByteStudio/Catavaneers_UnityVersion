public static class ButtonSmashPreventor
{
    private static int limit = 1;
    public static int Limit => limit;
    public static void SetLimit(int value)
    {
        limit = value;
    }

    public static bool ShouldProceed(ref int currentPressCount)
    {
        currentPressCount++;
        return currentPressCount - 1 < limit;
    }
}