using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
                Date = DateTime.Now.Date
            });
            CodeTextBox.Text = "";
        }

        private void CodeTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            //codesList.Add(new Code
            //{
            //    CodeString = CodeTextBox.Text,
            //    Date = DateTime.Now.Date
            //});
            //CodeTextBox.Text = "";
            Console.WriteLine(codesList.Count);
        }
    }
}
