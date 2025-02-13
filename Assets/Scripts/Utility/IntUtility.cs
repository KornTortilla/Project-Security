public static class IntUtility
{
    public static int Wrap(int currentInt, int max, int min = 0)
    {
        int range = max - min + 1;

        if (currentInt < min) return currentInt + range;
        if (currentInt > max) return currentInt - range;

        return currentInt;
    }
}