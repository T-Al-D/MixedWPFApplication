using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MixedWPF.Sideviews
{
    /// <summary>
    /// Interaktionslogik für PiApproximation.xaml
    /// </summary>
    public partial class PiApproximation : Page
    {
        // choose globalradius dependent on canvans size
        public int globalRadius = 100;

        public PiApproximation()
        {
            InitializeComponent();
        }

        // preview for the input
        private new void PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var textBox = sender as TextBox;
            e.Handled = Regex.IsMatch(e.Text, "[^0-9]+"); //regex that matches disallowed text
            return;
        }

        // cast/parse string to int
        private ulong StringToNum(string text)
        {
            bool success = ulong.TryParse(text, out ulong inputNumber);

            if (success)
            {
                primaryResultTextBlock.Text = "Parse successfull \n";
            }
            else
            {
                primaryResultTextBlock.Text = "conversion from int to string failed \n";
            }

            return inputNumber;
        }

        public double CalcPossiblePi(uint steps)
        {
            double radius = globalRadius;
            double quaderSurface = 0;
            double stepLength = radius / steps;
            double radiusSquare = radius * radius;

            for (int counter = 1; counter <= steps; counter++)
            {
                // lower side of the triangle
                double currentTriangleWidth = counter * stepLength;

                // temporary calculated surface
                double currentsurface = 0;

                // only feasable if triangleWidth is NOT larger than radius
                if (currentTriangleWidth < radius)
                {
                    // radius = triangleDiagonal, stepLength = currentRectangle
                    double heightSquare = radiusSquare - (currentTriangleWidth * currentTriangleWidth);
                    double currentRectangleHeight = Math.Sqrt(heightSquare);

                    currentsurface = currentRectangleHeight * stepLength;
                }

                // calculate all the rectangles together
                quaderSurface += currentsurface;
            }

            double calculatedPi = (quaderSurface * 4) / (globalRadius * globalRadius);
            return calculatedPi;
        }

        private bool CompareDecimalPlaces(ulong decimalPlace, double possiblePi, double delta)
        {

            bool value = false;

            // epsilon is the difference between PI_CONST and the piCutOff
            double epsilon = 3.1415926535897932 - possiblePi;
            // if epsilon is smaller than the allowed difference between the decimalPlaces
            if (epsilon < delta)
            {
                value = true;
            }

            return value;
        }

        private double CutOffDecimal(double decimalPlace, double num)
        {
            double truncateNum = Math.Pow(10, decimalPlace);
            return Math.Truncate(num * truncateNum) / truncateNum;
        }

        private (ulong, ulong, double, string) CalcNecessaryStepsToDecimalPlace(ulong decimalPlace, ulong acceptedDifference)
        {
            // Gets a NumberFormatInfo associated with the en-US culture.
            NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
            nfi.CurrencyDecimalSeparator = ",";
            nfi.CurrencyGroupSeparator = ".";
            nfi.CurrencySymbol = "";

            // delta is the decimalPlace difference that is allowed
            // example epsilon = 3,1415 - 3,1   = 0.0415 -> 1st decimalPlace
            // example epsilon = 3,1415 - 3,14  = 0.0015 -> 2nd decimalPlace
            // example epsilon = 3,1415 - 3,141 = 0.0005 -> 3rd decimalplace;

            double cutOffNum = CutOffDecimal((decimalPlace), 3.1415926535897932);
            double delta = 3.1415926535897932 - cutOffNum;

            // important for phase 1: iterative increase
            ulong leap = (ulong)Math.Pow(10, decimalPlace);

            // important for phase 2: smaller iterative increase / decrease
            ulong approximation = decimalPlace - 1;

            // bounds to calculate Pi with approximation
            ulong upperBound = ulong.MaxValue;
            ulong lowerBound = leap;

            // nessecarry for 1.Phase (don´t reset to beginning)
            ulong previousLowerBound = leap;

            // output possible PI
            double possiblePi = 0;

            // booleans
            bool decimalPlacesCorrect = false;

            // string for result
            string resultString = "";

            // toatalSteps made
            ulong totalSteps = 0;

            while (true)
            {
                // calculate Pi with the amount oft lowerBound
                possiblePi = CalcPossiblePi((uint)lowerBound);
                decimalPlacesCorrect = CompareDecimalPlaces(decimalPlace, possiblePi, delta);

                if (upperBound == ulong.MaxValue)
                {
                    // if the decimalPlaces don´t match and the upperBound is unkown
                    // increase lowerBound
                    if (!decimalPlacesCorrect)
                    {
                        previousLowerBound = lowerBound;
                        lowerBound += leap;
                    }
                    else
                    {
                        // if decimalPlaces match but upperBound not set,
                        // set upperBound with the current "lowerBound" and set lowerBound to the previousLowerBound
                        upperBound = lowerBound;
                        lowerBound = previousLowerBound;
                    }
                }
                else
                {
                    ulong currentDifference = upperBound - lowerBound;
                    ulong approximationLeap = (ulong)Math.Pow(10, approximation);

                    resultString += "current Low: " + Convert.ToDecimal(lowerBound).ToString("C0", nfi)
                        + ", current High: " + Convert.ToDecimal(upperBound).ToString("C0", nfi)
                        + ", currentApprox. : 10^" + approximation
                        + ", totalsteps: " + totalSteps
                        + "\n";

                    if (!decimalPlacesCorrect)
                    {
                        // set lowerBound                       
                        previousLowerBound = lowerBound;
                        lowerBound += approximationLeap;
                    }
                    else
                    {
                        // if bound within accepted range, break!
                        if (currentDifference <= acceptedDifference)
                        {
                            break;
                        }
                        else
                        {
                            // if decimalPlaces are Correct -> number reached or above
                            upperBound = lowerBound;
                            lowerBound = previousLowerBound;
                            approximation -= 1;

                            // if the difference lowerBound and upperBound is only 1, this means max precision has been reached
                            if (approximation == ulong.MaxValue || (lowerBound + 1) == upperBound || lowerBound == upperBound)
                            {
                                resultString += "current Low: " + Convert.ToDecimal(lowerBound).ToString("C0", nfi)
                                    + ", current High: " + Convert.ToDecimal(upperBound).ToString("C0", nfi)
                                    + ", currentApprox. : 10^X"
                                    + ", totalsteps: " + totalSteps
                                    + "\n";

                                resultString += "Maximum precision reached !\n";
                                break;
                            }
                        }
                    }
                }

                totalSteps++;
            }

            // secondaryResultTextBox.Text = resultString;
            return (lowerBound, upperBound, possiblePi, resultString);
        }

        private async Task<(ulong, ulong, double, string)> executeProgramAsync(ulong inputNum, ulong precisionNum)
        {
            return await Task.Run(() => CalcNecessaryStepsToDecimalPlace(inputNum, precisionNum));
        }

        private async void actionButton_Click(object sender, RoutedEventArgs e)
        {
            // clear results
            primaryResultTextBlock.Text = "";
            secondaryResultTextBox.Text = "";
            ulong inputNum = StringToNum(decimalDigitInput.Text);
            ulong precisionNum = StringToNum(precisionDigitInput.Text);

            if (decimalDigitInput.Text == "" || precisionDigitInput.Text == "")
            {
                MessageBox.Show("One or muliple input(s) empty", "Alert", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else if(inputNum > 9)
            {
                MessageBox.Show("decimalPlace choosen to large !", "Alert", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {

                // disable button, else new calculation gets queued
                actionButton.IsEnabled = false;
                clearButton.IsEnabled = false;

                // start stopwatch and progressbar
                progressBar.IsIndeterminate = true;
                var watch = System.Diagnostics.Stopwatch.StartNew();
                // execute program (asyncronously)
                // CalcNecessaryStepsToDecimalPlace(inputNum, precisionNum);
                var items = await executeProgramAsync(inputNum, precisionNum);
                // stop stopwatch and progressbar
                watch.Stop();

                // get values
                var elapsedMs = watch.ElapsedMilliseconds;
                ulong lowerBound = items.Item1;
                ulong upperBound = items.Item2;
                double possiblePi = items.Item3;
                secondaryResultTextBox.Text = items.Item4;

                progressBar.IsIndeterminate = false;
                // allow interaction with buttons
                actionButton.IsEnabled = true;
                clearButton.IsEnabled = true;

                // display the results
                primaryResultTextBlock.Text = "lowerBound: " + lowerBound + "! upperBound :" + upperBound + "!\n";
                primaryResultTextBlock.Text += "elapsed milliseconds: " + elapsedMs + " ms or " + (elapsedMs / 1000) + " seconds !\n";
                primaryResultTextBlock.Text += "possible PI: " + possiblePi;
            }
        }

        private void clearButton_Click(object sender, RoutedEventArgs e)
        {
            // clear everything
            primaryResultTextBlock.Text = "";
            secondaryResultTextBox.Text = "";
            decimalDigitInput.Text = "";
            precisionDigitInput.Text = "";
        }
    }
}
