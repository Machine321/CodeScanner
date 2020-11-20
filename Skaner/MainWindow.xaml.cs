﻿using CsvHelper;
using Microsoft.Win32;
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
        ListCollectionView collectionView;
        String dbPath;

        public MainWindow()
        {
            codesList = new ObservableCollection<Code>();
            collectionView = new ListCollectionView(codesList);
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            CodesDataGrid.DataContext = collectionView;
            CodeTextBox.Focus();
            DateToFilter.SelectedDateChanged += this.DateToFilter_SelectedDateChanged;
        }

        private void DateToFilter_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            collectionView.Filter = (el) =>
            {
                Code emp = el as Code;
                if (emp.Date == DateToFilter.SelectedDate)
                    return true;
                return false;
            };
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

            collectionView.Filter = null;
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

            collectionView.Filter = null;
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
                    collectionView.Filter = null;
                    CodesDataGrid.ScrollIntoView(codesList[codesList.Count - 1]);
                    CodesDataGrid.SelectedItem = CodesDataGrid.Items[CodesDataGrid.Items.Count - 1];
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            collectionView.Filter = null;
            CodesDataGrid.ScrollIntoView(codesList[codesList.Count - 1]);
            CodesDataGrid.SelectedItem = CodesDataGrid.Items[CodesDataGrid.Items.Count - 1];
        }

        private void ExportButton_Click(object sender, RoutedEventArgs e)
        {
            var sb = new StringBuilder();
            foreach (Code code in CodesDataGrid.Items)
            {
                sb.Append(code.CodeString);
                sb.AppendLine();
            }
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            if (saveFileDialog.ShowDialog() == true)
                File.WriteAllText(saveFileDialog.FileName, sb.ToString());

        }
    }
}
