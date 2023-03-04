namespace MixedLibrary.GeneralLogic
{
    public static class Check
    {
        // Method checks for a valid number! If num valid but negative, it´s converted into positive!
        // else Method returns lowest int value !
        public static int CheckValidNumber(string inputString)
        {
            bool validNumber = false;
            validNumber = int.TryParse(inputString, out int validadedNumber);

            if (validNumber)
            {
                return validadedNumber;
            }
            else
            {
                return int.MinValue;
            }
        }
    }
}