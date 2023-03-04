using MixedLibrary.GeneralLogic;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MixedWPF
{
    /// <summary>
    /// Interaktionslogik für Diamond.xaml
    /// </summary>
    public partial class Diamond : Page
    {
        public Diamond()
        {
            InitializeComponent();
        }

        // checking for valid input (only numbers) before entering
        private void InputPreview(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private string BuildDiamond(int maxHeight)
        {
            string diamondString = "";
            int currentEmptySpaces = maxHeight;
            for (int i = 0; i < maxHeight + 1; i++)
            {
                diamondString = StringCreator.AddEmptySpaces(diamondString, currentEmptySpaces);
                diamondString = StringCreator.FillStringWithSubStrings(diamondString, " *", i);
                diamondString = diamondString + "\n";
                currentEmptySpaces--;
            }

            currentEmptySpaces = 1;
            for (int i = maxHeight - 1; i > 0; i--)
            {
                diamondString = StringCreator.AddEmptySpaces(diamondString, currentEmptySpaces);
                diamondString = StringCreator.FillStringWithSubStrings(diamondString, " *", i);
                diamondString = diamondString + "\n";
                currentEmptySpaces++;
            }
            return diamondString;
        }

        // draw a diamond, check for potential mistake
        private void DrawDiamondBtn_Click(object sender, RoutedEventArgs e)
        {
            int numInput = Check.CheckValidNumber(InputTextBox.Text);
            string diamondString = "";

            if (numInput == int.MinValue)
            {
                MessageBox.Show("Input is not a number!");
            }
            else
            {
                if (numInput > 42)
                {
                    MessageBox.Show("Number is larger than 42 !");
                }
                else
                {
                    // if Box IS checked: number is toatal size 
                    if (checkBox.IsChecked == true)
                    {
                        if (numInput % 2 == 0)
                        {
                            MessageBox.Show("Only uneven Numbers!");
                        }
                        else
                        {
                            diamondString = BuildDiamond((numInput/2) + 1);
                        }
                    }
                    // if Box NOT checked: number is size of top half, size of largest row
                    else
                    {
                        diamondString = BuildDiamond(numInput);
                    }
                }
                ResultTxtBlk.Text = diamondString;
            }
        }

        private void InputTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {

        }
    }
}
