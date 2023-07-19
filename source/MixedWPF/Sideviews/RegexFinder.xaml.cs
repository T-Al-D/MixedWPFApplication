using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
    /// Interaktionslogik für RegexFinder.xaml
    /// </summary>
    public partial class RegexFinder : Page
    {
        public RegexFinder()
        {
            InitializeComponent();
        }

        private void OpenFileBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "Text Files | *.txt";
            fileDialog.Multiselect = false;

            bool? success = fileDialog.ShowDialog();

            // if openening of filedialog was successful, read .txt
            if (success == true)
            {
                DisplayRichTextBox.Document.Blocks.Clear();
                string contents = File.ReadAllText(fileDialog.FileName);
                DisplayRichTextBox.Document.Blocks.Add(new Paragraph(new Run(contents)));
            }
        }

        private void ClearTextBtn_Click(object sender, RoutedEventArgs e)
        {
            DisplayRichTextBox.Document.Blocks.Clear();
        }

        private void ClearMarkingsBtn_Click(object sender, RoutedEventArgs e)
        {
            TextPointer? startPointer = startPointer = DisplayRichTextBox.Document.ContentStart;
            TextPointer? endPointer = endPointer = DisplayRichTextBox.Document.ContentEnd;

            TextRange textRange = new TextRange(startPointer, endPointer);
            Color color = (Color)ColorConverter.ConvertFromString("White");
            textRange.ApplyPropertyValue(TextElement.BackgroundProperty, new SolidColorBrush(color));
        }

        void MarkingTxt(string searchPattern)
        {
            // Create TextPointers to specify the range of content to select.
            TextPointer textPointer = DisplayRichTextBox.Document.ContentStart;
            TextPointer? startPointer = null;
            TextPointer? endPointer = null;

            // TextRange instance which contains Textpointers and color
            TextRange? textRange = null;
            Color color = (Color)ColorConverter.ConvertFromString("#F1B667");

            while (textPointer != null)
            {
                // search for the next "text content" in forward direction (only if TextContext !)
                if (textPointer.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.Text)
                {
                    // find the starting index of any substring that matches "searchPattern"
                    int indexInRun = textPointer.GetTextInRun(LogicalDirection.Forward).IndexOf(searchPattern);

                    if (indexInRun > -1)
                    {
                        // start at the beginning of the word, end at the ending of the word
                        startPointer = textPointer.GetPositionAtOffset(indexInRun);
                        endPointer = textPointer.GetPositionAtOffset(indexInRun + searchPattern.Length);

                        if (startPointer != null && endPointer != null)
                        {
                            // textrange where the text should be marked
                            textRange = new TextRange(startPointer, endPointer);

                            // Apply formating to a caracteristic, for current TextRanges
                            textRange.ApplyPropertyValue(TextElement.BackgroundProperty, new SolidColorBrush(color));
                        }
                    }
                }

                textPointer = textPointer.GetNextContextPosition(LogicalDirection.Forward);
            }
        }

        private void SearchRegexBtn_Click(object sender, RoutedEventArgs e)
        {
            string inputTxt = new TextRange(
                DisplayRichTextBox.Document.ContentStart,
                DisplayRichTextBox.Document.ContentEnd).Text;

            string regularExpression = @"" + RegexInput.Text;

            if (!string.IsNullOrEmpty(regularExpression))
            {
                // MatchCollection with all the matches to the regular expression
                MatchCollection? matches = null;

                // check if the regular expression is valid and if it exists
                try
                {
                    matches = Regex.Matches(inputTxt, regularExpression);
                }
                catch (System.Exception)
                {
                }

                // clear previous markings before appling new
                ClearMarkingsBtn_Click(sender, e);

                // for each match, mark all matches in text
                if (matches != null)
                {
                    // no duplicates allowed (for more speed)
                    // selcted matches are compared based on their values
                    matches.OfType<Match>().Select(m => m.Groups[0].Value).Distinct();

                    foreach (Match match in matches)
                    {
                        MarkingTxt(match.Value);
                        //HighlightText(match);
                    }
                }
            }
        }
    }
}
