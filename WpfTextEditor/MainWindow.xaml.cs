using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Configuration;
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
            var fontSize = 0d;
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
            string connectionString = ConfigurationManager.AppSettings["connectionString"];

            using (SqlConnection sql = new SqlConnection(connectionString))
            {
                try
                {
                    if (sql.State == System.Data.ConnectionState.Closed)
                        sql.Open();

                    string query = "SELECT COUNT(1) FROM TestTable";
                    SqlCommand sqlCom = new SqlCommand(query, sql);
                    sqlCom.CommandType = System.Data.CommandType.Text;
                    //sqlCom.Parameters.Add("",);

                    int count = sqlCom.ExecuteScalar() as int? ?? 0;
                    if (count == 0)
                    {
                        query = "INSERT INTO Users(login, password) VALUES (@login, @pass)";
                        sqlCom = new SqlCommand(query, sql) { CommandType = System.Data.CommandType.Text};
                        //sqlCom.Parameters.Add();

                        sqlCom.ExecuteNonQuery();
                        MessageBox.Show("Мы добавили вас в базу данных");
                    } else
                    {
                        MessageBox.Show("Вы успешно авторизовались");
                    }

                }
                catch (Exception exc)
                {
                    MessageBox.Show(exc.Message);
                }
            }
        }

        /// <summary>
        /// Удаления зарегистрированного пользователя
        /// </summary>
        void RemoveLogin()
        {
            var y = AesClass.Decrypt("AQAAANCMnd8BFdERjHoAwE/Cl+sBAAAA9Zo7t1TN7Uia6WE2VeIQ7gQAAAACAAAAAAAQZgAAAAEAACAAAAD33d1YXXcNh8PBD8iq04A/byxbqEfXf6p0KeYyheIzQQAAAAAOgAAAAAIAACAAAAC5q1/NSNJ+2sJKN1mY4X4KZ0jHz2o6XpwOUNRGTkzNIxAAAACzUZasQAH3xV9QnlTTONMTQAAAAC+EdexJy0a3KaoEByiBgcwG/pqiYsLhCxUdbq+DrA0QtRrO0EGfonrOnoK5GSE05+Xm+tiyLlfzN4pNdPN9GWA=");
            MessageBox.Show(y);
        }
    }
}
