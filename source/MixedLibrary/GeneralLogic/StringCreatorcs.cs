namespace MixedLibrary.GeneralLogic
{
    public static class StringCreator
    {
        public static string AddEmptySpaces(string input, int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                input = $"{input} ";
            }
            return input;
        }

        public static string FillStringWithSubStrings(string mainString, string subString, int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                mainString = $"{mainString}{subString}";
            }
            return mainString;
        }
    }
}

