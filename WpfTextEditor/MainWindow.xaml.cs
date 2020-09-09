using Microsoft.Win32;
using System;
using System.Collections.Generic;
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

namespace WpfTextEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        //Клик по элементу меню
        private void Menu_Click(object sender, RoutedEventArgs e)
        {
            var menuItem = sender as MenuItem;
            if (menuItem == null) return;
            //Выбор шрифта
            if (menuItem.Tag?.Equals("FontButton") ?? false)
            {
                mainTextArea.FontFamily = menuItem.FontFamily;
            }
            //Сохранение файла
            if (menuItem == miSaveFile)
            {
                var saveDialog = new SaveFileDialog();
                saveDialog.ShowDialog();


            }
            //Открытие файла
            if (menuItem == miOpenFile)
            {
                var openDialog = new OpenFileDialog();
                openDialog.ShowDialog();
            }
            //Выход
            if (menuItem == miQuit) this.Close();
        }

        private void Toolbar_Click(object sender, RoutedEventArgs e)
        {

        }

        private void selectFontSize_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (mainTextArea == null) return;
            var selectedItem = selectFontSize.SelectedValue as ComboBoxItem;
            var fontSize = 0d;
            if (Double.TryParse(selectedItem?.Content.ToString(), out fontSize)) 
                mainTextArea.FontSize = fontSize;
            mainTextArea.Focus();
        }
    }
}
