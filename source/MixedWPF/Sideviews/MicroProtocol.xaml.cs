using MixedLibrary.DataAccess;
using MixedLibrary.GeneralLogic;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace MixedWPF
{
    /// <summary>
    /// Interaktionslogik für MicroProtocol.xaml
    /// </summary>
    public partial class MicroProtocol : Page
    {
        public MicroProtocol()
        {
            InitializeComponent();
        }

        private void InputPreview(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        // acess database and Fill the DataGrid with Info
        private void FillDataGrid(List<ProtocolModel> protocols)
        {
            // if no protocols have been recived, create a "warning protocol"
            if (protocols == null || protocols.Count == 0)
            {
                protocols = new List<ProtocolModel>();
                protocols.Add(new ProtocolModel("NO SUCH ID IN DATABASE!", "THIS IS A WARNING, please input valid Id!"));

                ProtocolDataGrid.Foreground = new SolidColorBrush(Colors.DarkRed);
            }
            else
            {
                ProtocolDataGrid.Foreground = new SolidColorBrush(Colors.DarkBlue);
            }

            ProtocolDataGrid.ItemsSource = protocols;

        }

        private List<ProtocolModel> GetAllProtocols()
        {
            List<ProtocolModel> protocols = new List<ProtocolModel>();

            using (SQLiteContext sqliteContext = new SQLiteContext())
            {
                // access Table
                protocols = sqliteContext.Protocols.ToList();
            }

            return protocols;
        }

        private List<ProtocolModel>? GetSpecificProtocol(int value)
        {
            List<ProtocolModel>? protocols = new List<ProtocolModel>();
            ProtocolModel protocol;

            using (SQLiteContext sqliteContext = new SQLiteContext())
            {
                // access Table
                // only one Item in the List
                protocol = sqliteContext.Protocols.Find(value);
            }

            if (protocol == null)
            {
                protocols = null;
            }
            else
            {
                protocols.Add(protocol);
            }

            return protocols;
        }

        private void CreateProtocolBtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (TitleInputTextBox.Text.Length < 3)
            {
                MessageBox.Show("Title is too short!");
            }
            else
            {
                ProtocolModel protocol = new ProtocolModel(TitleInputTextBox.Text, DescriptionInputTextBox.Text);

                using (SQLiteContext sqliteContext = new SQLiteContext())
                {
                    // access Table
                    sqliteContext.Protocols.Add(protocol);

                    // important: save changes !
                    sqliteContext.SaveChanges();
                }

                // update datagrid after operation
                FillDataGrid(GetAllProtocols());
            }

            TitleInputTextBox.Text = "";
            DescriptionInputTextBox.Text = "";
        }

        private void GetProtocolBtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            int value = Check.CheckValidNumber(IdInputTextBox.Text);

            // get all Protocols
            if (value == int.MinValue)
            {
                FillDataGrid(GetAllProtocols());
            }
            // get specific protocol
            else
            {
                FillDataGrid(GetSpecificProtocol(value));
            }

            IdInputTextBox.Text = "";
        }

        private void DeleteProtocolBtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {

            ProtocolModel protocolToDelete = (ProtocolModel)ProtocolDataGrid.SelectedItem;

            if (protocolToDelete == null)
            {
                MessageBox.Show("No Protocol selected!");
            }
            else
            {
                using (SQLiteContext sqliteContext = new SQLiteContext())
                {
                    // access Table
                    sqliteContext.Protocols.Remove(protocolToDelete);

                    // important: save changes !
                    sqliteContext.SaveChanges();
                }
            }

            // update datagrid after operation
            FillDataGrid(GetAllProtocols());
        }
    }
}
