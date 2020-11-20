using CsvHelper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
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

namespace Skaner
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    /// 

    public partial class MainWindow : Window
    {
        ObservableCollection<Code> codesList;
        String dbPath;

        public MainWindow()
        {
            codesList = new ObservableCollection<Code>();

            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            CodesDataGrid.DataContext = codesList;
            CodeTextBox.Focus();
        }

        private void CodeTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter || e.Key != Key.Return) return;

            e.Handled = true;
            codesList.Add(new Code {
                CodeString = CodeTextBox.Text,
                Date = DateTime.Today
            });
            CodeTextBox.Text = "";
            //CodesDataGrid.SelectedIndex = CodesDataGrid.Items.Count - 1;
            CodesDataGrid.ScrollIntoView(codesList[codesList.Count - 1]);
            CodesDataGrid.SelectedItem = CodesDataGrid.Items[CodesDataGrid.Items.Count - 1];
        }

        private void CodeTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            codesList.Add(new Code
            {
                CodeString = CodeTextBox.Text,
                Date = DateTime.Now.Date
            });
            CodeTextBox.Text = "";
            //CodesDataGrid.SelectedIndex = CodesDataGrid.Items.Count - 1;
            CodesDataGrid.ScrollIntoView(codesList[codesList.Count - 1]);
            CodesDataGrid.SelectedItem = CodesDataGrid.Items[CodesDataGrid.Items.Count - 1];
        }

        private void SaveDbButton_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.DefaultExt = ".csv";
            dlg.Filter = "CSV Files (*.csv)|*.csv";

            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                dbPath = dlg.FileName;
                using (var reader = new StreamReader(dbPath))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    var records = csv.GetRecords<Code>();
                    codesList.Clear();
                    foreach(Code item in records)
                        codesList.Add(new Code
                        {
                            CodeString = item.CodeString,
                            Date = item.Date
                        });
                }
            }
        }

        private void WriteDbButton_Click(object sender, RoutedEventArgs e)
        {
            if (dbPath != null)
            {
                using (var writer = new StreamWriter(dbPath))
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    csv.WriteRecords(codesList);
                }
            }
            else
                MessageBox.Show("Brak wskazanego pliku bazy!", "UWAGA!");
        }
    }
}
