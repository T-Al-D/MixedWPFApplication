namespace MixedLibrary.GeneralLogic
{
    public static class NumCreator
    {
        // returns a random number including lower- and upperBound
        public static int GetRandomNumWithinBound(int lowerBound, int upperBound)
        {
            Random random = new();
            int randomNum = random.Next(lowerBound, upperBound + 1);
            return randomNum;
        }
    }
}
