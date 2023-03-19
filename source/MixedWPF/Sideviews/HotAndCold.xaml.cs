using MixedLibrary.GeneralLogic;
using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MixedWPF
{
    /// <summary>
    /// Interaktionslogik für HotAndCold.xaml
    /// </summary>
    public partial class HotAndCold : Page
    {
        // goal number that has to be reached
        private int _goalNum;

        // number can´t be lower than this bound
        private int _lowerbound = 0;

        // number can´t be larger than this bound
        private int _upperbound = 100;

        // string for TextBlock
        private string _result = "";

        public HotAndCold()
        {
            InitializeComponent();
        }

        // checking for valid input (only numbers) before entering
        private void InputPreview(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        // calculate new bound depending on prevGuess and nextGuess distance to _goalNum
        private void GetNewBound(int prevGuess, int nextGuess)
        {
            int NextUpperbound = 101;
            int NextLowerbound = -1;
            int distanceRadiusBetweenTheGuesses = Math.Abs((prevGuess - nextGuess) / 2);

            // HOT if (the distance of) nextGuess is closer to _goalNum than prevGuess
            if (Math.Abs(_goalNum - nextGuess) < Math.Abs(_goalNum - prevGuess))
            {
                _result += "HOT : " + nextGuess + " ! \n";

                if (prevGuess > nextGuess)
                {
                    NextUpperbound = Math.Abs(nextGuess + distanceRadiusBetweenTheGuesses + 1);
                }
                else if (nextGuess > prevGuess)
                {
                    NextLowerbound = Math.Abs(prevGuess + distanceRadiusBetweenTheGuesses - 1);
                }
            }
            // COLD if (the distance of) prevGuess is closer to _goalNum than nextGuess
            else if (Math.Abs(_goalNum - nextGuess) > Math.Abs(_goalNum - prevGuess))
            {
                _result += "COLD : " + nextGuess + " ! \n";

                if (prevGuess > nextGuess)
                {
                    NextLowerbound = Math.Abs(nextGuess + distanceRadiusBetweenTheGuesses - 1);
                }
                else if (nextGuess > prevGuess)
                {
                    NextUpperbound = Math.Abs(prevGuess + distanceRadiusBetweenTheGuesses + 1);
                }
            }

            // for safety if miscalculation occured
            if (_lowerbound < NextLowerbound && _lowerbound > -1 && _upperbound > NextLowerbound)
            {
                _lowerbound = NextLowerbound;
            }
            else if (_upperbound > NextUpperbound && _upperbound < 101 && _lowerbound < NextUpperbound)
            {
                _upperbound = NextUpperbound;
            }
        }

        // main game logic to help the computer guess the number
        private void ComputerGuess()
        {
            _result = "Your input: " + _goalNum + " ! \n";

            int prevGuess = -1;
            int nextGuess = NumCreator.GetRandomNumWithinBound(_lowerbound, _upperbound);

            if (_goalNum == nextGuess)
            {
                _result += "Guessed at first try: " + nextGuess + " ! \n";
            }
            else
            {

                while (_goalNum != nextGuess)
                {
                    GetNewBound(prevGuess, nextGuess);
                    _result += "Next number between " + _lowerbound + " and " + _upperbound + " .\n";

                    // switch the guesses for next round
                    prevGuess = nextGuess;
                    nextGuess = NumCreator.GetRandomNumWithinBound(_lowerbound, _upperbound);

                    if (_goalNum == nextGuess)
                    {
                        _result += "Amazing you guessed the number: " + nextGuess + " ! \n";
                    }

                    // always set the content of the result TextBlock
                    ResultTxtBlk.Text = _result;

                }
            }
        }

        private void GuessNumber_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            // check first if input valid
            _goalNum = Check.CheckValidNumber(InputTextBox.Text);
            if (_goalNum == int.MinValue)
            {
                MessageBox.Show("Number not valid!");
            }
            else
            {
                if (_goalNum < _lowerbound || _goalNum > _upperbound)
                {
                    MessageBox.Show("Number too large or too small!");
                }
                else
                {
                    // here actual game logic
                    ComputerGuess();
                }
            }

            // reset for next round
            _lowerbound = 0;
            _upperbound = 100;
            _result = "";
        }
    }
}
