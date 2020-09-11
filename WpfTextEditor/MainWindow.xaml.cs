using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
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
    public partial class MainWindow : Window
    {
        private static string stringConnection = "Data Source=wpl22.hosting.reg.ru;Initial Catalog=u0790449_BurovAV;Persist Security Info=True;User ID=u0790449_BurovAV;Password=Freedom88";

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
                mainTextArea.FontFamily = menuItem.FontFamily;
            //Новый файл
            if (menuItem == miNewFile)
                mainTextArea.Text = "";

            //Сохранение файла
            if (menuItem == miSaveFile)
            {
                var saveDialog = new SaveFileDialog() { Filter = "Текстовый файл .txt|*.txt" };
                saveDialog.ShowDialog();
                if (saveDialog.FileName == string.Empty) {
                    MessageBox.Show("Файл не был сохранен", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return; 
                }
                using (Stream s = File.Open(saveDialog.FileName, FileMode.OpenOrCreate))
                    using (StreamWriter sw = new StreamWriter(s))
                        sw.Write(mainTextArea.Text);
            }

            //Открытие файла
            if (menuItem == miOpenFile)
            {
                var openDialog = new OpenFileDialog();
                openDialog.ShowDialog();

                if (openDialog.FileName == string.Empty)
                {
                    MessageBox.Show("Файл не был открыт", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                Stream s;
                if ((s = openDialog.OpenFile())!= null)
                {
                    var fileName = openDialog.FileName;
                    var fileText = File.ReadAllText(fileName);
                    mainTextArea.Text = fileText;
                }
            }

            //Выход
            if (menuItem == miQuit) this.Close();
        }

        private void Toolbar_Click(object sender, RoutedEventArgs e)
        {
            //Сохранить файл
            if (sender == tbSave)
                Menu_Click(miSaveFile, null);
            if (sender == tbOpen)
                Menu_Click(miOpenFile, null);
        }

        private void selectFontSize_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (mainTextArea == null) return;
            var selectedItem = selectFontSize.SelectedValue as ComboBoxItem;
            var fontSize = 14d;
            if (Double.TryParse(selectedItem?.Content.ToString(), out fontSize)) 
                mainTextArea.FontSize = fontSize;
            mainTextArea.Focus();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (sender == registerButton)
                Registration();
            if (sender == removeButton)
                RemoveLogin();
        }

        /// <summary>
        /// Регистрация пользователя
        /// </summary>
        void Registration()
        {
            var login = loginInput.Text.ToLower();
            var pass = passwordInput.Text;
            SqlCommand sqlCom = new SqlCommand($"SELECT * FROM WpfTextEditor_logins WHERE " +
                $"login='{login}' AND password='{pass}'");
            var dataTable = BurovSecure.ExecuteDataTable(sqlCom);

            if (dataTable.Rows.Count == 0)
            {
                sqlCom = new SqlCommand($"INSERT INTO WpfTextEditor_logins (login, password) VALUES ('{login}', '{pass}')");
                BurovSecure.ExecuteNonQuery(sqlCom);
                MessageBox.Show("Мы добавили вас базу данных.");
            } else
            {
                MessageBox.Show("Вы успешно авторизованы.");
            }

        }

        /// <summary>
        /// Удаления зарегистрированного пользователя
        /// </summary>
        void RemoveLogin()
        {
            var login = loginInput.Text.ToLower();
            var pass = passwordInput.Text;
            SqlCommand sqlCom = new SqlCommand($"SELECT * FROM WpfTextEditor_logins WHERE " +
                $"login='{login}' AND password='{pass}'");
            var dataTable = BurovSecure.ExecuteDataTable(sqlCom);

            if (dataTable.Rows.Count != 0)
            {
                sqlCom = new SqlCommand($"DELETE FROM WpfTextEditor_logins WHERE " +
                    $"login='{login}' AND password='{pass}'");
                BurovSecure.ExecuteNonQuery(sqlCom);
                MessageBox.Show("Учетная запись удалена.");
            }
            else
            {
                MessageBox.Show("Не найдена пара логин/пароль.");
            }
        }
    }
}
