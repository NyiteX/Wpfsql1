using System;
using System.Collections.Generic;
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
using System.Data.SqlClient;

namespace Wpfsql1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary> 
    public partial class MainWindow : Window
    {
        string str = @"Data Source = WIN-U669V8L9R5E; Initial Catalog = Pract; Trusted_Connection=True";
   
        public MainWindow()
        {
            InitializeComponent();
            tb1.Text = "";
        }

        async void Save()
        {
            try
            {
                if (tb1.Text.Length > 0)
                {
                    using (SqlConnection connection = new SqlConnection(str))
                    {
                        await connection.OpenAsync();
                        SqlCommand command = new SqlCommand();

                        command.CommandText = "INSERT INTO Zametka VALUES('" + tb1.Text + "')";
                        command.Connection = connection;

                        await command.ExecuteNonQueryAsync();
                        MessageBox.Show("dobavleno");
                        tb1.Text = "";
                    }
                }
                else
                    MessageBox.Show("texta net");
            }
            catch(Exception e) { MessageBox.Show(e.Message); }
        }

        async void Delete()
        {
            try
            {
                if (tb1.Text.Length > 0)
                {
                    using (SqlConnection connection = new SqlConnection(str))
                    {
                        await connection.OpenAsync();
                        SqlCommand command = new SqlCommand();

                        command.CommandText = "DELETE FROM Zametka WHERE ID = '" + tb1.Text + "'";
                        command.Connection = connection;

                        await command.ExecuteNonQueryAsync();
                        MessageBox.Show("delete uspex");
                        tb1.Text = "";
                    }
                }
                else
                    MessageBox.Show("texta net");
            }
            catch (Exception e) { MessageBox.Show(e.Message); }
        }
        async void Load()
        {
            using (SqlConnection connection = new SqlConnection(str))
            {
                await connection.OpenAsync();

                SqlCommand command = new SqlCommand("SELECT * FROM Zametka GROUP BY Names,ID", connection);
                SqlDataReader reader = await command.ExecuteReaderAsync();

                if (reader.HasRows)
                {
                    tb1.Text = reader.GetName(0) + "\t" + reader.GetName(1) + "\n";
                    tb1.Text += "---------------------\n";
                    while (await reader.ReadAsync())
                    {
                        tb1.Text += reader.GetValue(0) + "\t";
                        tb1.Text += reader.GetValue(1) + "\n";
                    }
                    //await reader.CloseAsync();
                }
                else
                MessageBox.Show("zametok net");
            }
        }

        async void Search()
        {
            using (SqlConnection connection = new SqlConnection(str))
            {
                await connection.OpenAsync();

                SqlCommand command = new SqlCommand("SELECT * FROM Zametka WHERE Names = '" + tb1.Text +"'", connection);
                SqlDataReader reader = await command.ExecuteReaderAsync();

                if (reader.HasRows)
                {
                    MessageBox.Show("naideno");
                    tb1.Text = reader.GetName(0) + "\t" + reader.GetName(1)+"\n";
                    tb1.Text += "---------------------\n";
                    while (await reader.ReadAsync())
                    {
                        object id = reader.GetValue(1);

                        tb1.Text += reader.GetValue(0) + "\t";
                        tb1.Text += reader.GetValue(1) + "\n";
                    }
                    //await reader.CloseAsync();
                }
                else
                    MessageBox.Show("zametok net");
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Load();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Save();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Delete();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            Search();
        }
    }
}
