using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace laba1_ADO
{
    public partial class Form2 : Form
    {
        private string connectionString;
        public bool flag = false;
        public Form2(string str)
        {
            InitializeComponent();

            connectionString = str;
            CreateList();
            comboBox1.SelectedIndex = 0;
        }

        private void ReadLine(int stringN)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string sql = "SELECT BookId, Name, DateOfPubl, Numb, Price, DateProv FROM Book";
                    SqlCommand command = new SqlCommand(sql, connection);
                    SqlDataReader reader = command.ExecuteReader();

                    int i = 0;
                    while (reader.Read())
                    {
                        if (i == stringN)
                        {
                            int id = reader.GetInt32(0);
                            string name = reader.GetString(1);
                            DateTime dateOfPubl = reader.GetDateTime(2);
                            int numb = reader.GetInt32(3);
                            decimal price = reader.GetDecimal(4);
                            DateTime dateProv = reader.GetDateTime(5);

                            textBox1.Text = id.ToString();
                            textBox2.Text = name;
                            textBox3.Text = dateOfPubl.ToString();
                            textBox4.Text = numb.ToString();
                            textBox5.Text = price.ToString();
                            textBox6.Text = dateProv.ToString();
                            break;
                        }
                        else i++;
                    }
                    connection.Close();

                    //Publishing
                    connection.Open();
                    string sqlPubl = "SELECT p.Name FROM Publishing AS p " +
                        "INNER JOIN Book as b " +
                        "ON p.PublishingId = b.PublishingId";
                    SqlCommand commandPubl = new SqlCommand(sqlPubl, connection);
                    SqlDataReader readerPubl = commandPubl.ExecuteReader();

                    int i1 = 0;
                    while (readerPubl.Read())
                    {
                        if (i1 == stringN)
                        {
                            string name = readerPubl.GetString(0);

                            comboBox2.Text = name;

                            break;
                        }
                        else i1++;
                    }
                    connection.Close();

                    //Author
                    connection.Open();
                    string sqlA = "SELECT a.Surname FROM Author AS a " +
                        "INNER JOIN Book as b " +
                        "ON a.AuthorId = b.AuthorId";
                    SqlCommand commandA = new SqlCommand(sqlA, connection);
                    SqlDataReader readerA = commandA.ExecuteReader();

                    int i2 = 0;
                    while (readerA.Read())
                    {
                        if (i2 == stringN)
                        {

                            comboBox3.Text = readerA.GetString(0);

                            break;
                        }
                        else i2++;
                    }
                    connection.Close();

                    //Genre
                    connection.Open();
                    string sqlG = "SELECT g.Name FROM Genre AS g " +
                        "INNER JOIN Book AS b " +
                        "ON g.GenreId = b.GenreId";
                    SqlCommand commandG = new SqlCommand(sqlG, connection);
                    SqlDataReader readerG = commandG.ExecuteReader();

                    int i3 = 0;
                    while (readerG.Read())
                    {
                        if (i3 == stringN)
                        {
                            string name = readerG.GetString(0);

                            comboBox4.Text = name;

                            break;
                        }
                        else i3++;
                    }
                    connection.Close();


                    //Provider
                    connection.Open();
                    string sqlPr = "SELECT p.Surname FROM Provider AS p " +
                        "INNER JOIN Book AS b " +
                        "ON p.ProviderId = b.ProviderId";
                    SqlCommand commandPr = new SqlCommand(sqlPr, connection);
                    SqlDataReader readerPr = commandPr.ExecuteReader();

                    int i4 = 0;
                    while (readerPr.Read())
                    {
                        if (i4 == stringN)
                        {
                            comboBox5.Text = readerPr.GetString(0);

                            break;
                        }
                        else i4++;
                    }
                    connection.Close();
                }
                catch (Exception exception)
                {
                    MessageBox.Show("Не получилось прочитать из БД. " + exception.Message);
                }
            }
        }


        private void CreateList()
        {
            comboBox1.Items.Clear();
            comboBox2.Items.Clear();
            comboBox3.Items.Clear();
            comboBox4.Items.Clear();
            comboBox5.Items.Clear();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    //All
                    connection.Open();
                    string sql = "SELECT Name FROM Book";
                    SqlCommand command = new SqlCommand(sql, connection);
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        comboBox1.Items.Add(reader.GetString(0));
                    }
                    connection.Close();

                    //Publishing
                    connection.Open();
                    string sqlPubl = "SELECT Name FROM Publishing";
                    SqlCommand commandPubl = new SqlCommand(sqlPubl, connection);
                    SqlDataReader readerPubl = commandPubl.ExecuteReader();

                    while (readerPubl.Read())
                    {
                        comboBox2.Items.Add(readerPubl.GetString(0));
                    }
                    connection.Close();

                    //Author
                    connection.Open();
                    string sqlA = "SELECT Surname FROM Author";
                    SqlCommand commandA = new SqlCommand(sqlA, connection);
                    SqlDataReader readerA = commandA.ExecuteReader();

                    while (readerA.Read())
                    {
                        comboBox3.Items.Add(readerA.GetString(0));
                    }
                    connection.Close();
                    //Genre
                    connection.Open();
                    string sqlG = "SELECT Name FROM Genre";
                    SqlCommand commandG = new SqlCommand(sqlG, connection);
                    SqlDataReader readerG = commandG.ExecuteReader();

                    while (readerG.Read())
                    {
                        comboBox4.Items.Add(readerG.GetString(0));
                    }
                    connection.Close();
                    //Provider
                    connection.Open();
                    string sqlPr = "SELECT Surname FROM Provider";
                    SqlCommand commandPr = new SqlCommand(sqlPr, connection);
                    SqlDataReader readerPr = commandPr.ExecuteReader();

                    while (readerPr.Read())
                    {
                        comboBox5.Items.Add(readerPr.GetString(0));
                    }
                    connection.Close();
                }
                catch (Exception exception)
                {
                    MessageBox.Show("Не получилось прочитать из БД. " + exception.Message);
                }
            }
        }

        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
            Form form = Application.OpenForms[0];
            form.Close();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex >= 0)
            {
                ReadLine(comboBox1.SelectedIndex);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 0) comboBox1.SelectedIndex = comboBox1.Items.Count - 1;
            else comboBox1.SelectedIndex--;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == comboBox1.Items.Count - 1) comboBox1.SelectedIndex = 0;
            else comboBox1.SelectedIndex++;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            textBox6.Text = "";
            comboBox1.Text = "";
            comboBox2.Text = "";
            comboBox3.Text = "";
            comboBox4.Text = "";
            comboBox5.Text = "";
            //button1_Click(sender, e);
            flag = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DateTimePicker dateTimePicker = new DateTimePicker();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                try
                {
                    if (flag == true)
                    {
                        string sql = "INSERT INTO Book ([PublishingId], [AuthorId], [GenreId], [ProviderId], [Name], [DateOfPubl], [Numb], [Price], [DateProv]) " +
                                $"VALUES ('{comboBox2.SelectedIndex + 1}', " +
                                $"'{comboBox3.SelectedIndex + 1}', " +
                                $"'{comboBox4.SelectedIndex + 1}', " +
                                $"'{comboBox5.SelectedIndex + 1}', " +
                                $"N'{textBox2.Text}', '{dateTimePicker.Value.ToString("dd-MM-yyyy")}', " +
                                $"{Int32.Parse(textBox4.Text)}, " +
                                $"{Convert.ToDecimal(textBox5.Text)}, " +
                                $"'{dateTimePicker.Value.ToString("dd-MM-yyyy")}');";
                        SqlCommand command = new SqlCommand(sql, connection);
                        command.ExecuteNonQuery();
                        MessageBox.Show("Вы добавили новую запись в БД!");
                    }
                    else if (flag == false)
                    {
                        string sql = "UPDATE Book SET " +
                            $"PublishingId = '{comboBox2.SelectedIndex + 1}', " +
                            $"AuthorId = '{comboBox3.SelectedIndex + 1}', " +
                            $"GenreId = '{comboBox4.SelectedIndex + 1}', " +
                            $"ProviderId = '{comboBox5.SelectedIndex + 1}', " +
                            $"Name = N'{textBox2.Text}', " +
                            $"DateOfPubl = '{dateTimePicker.Value.ToString("dd-MM-yyyy")}', " +
                            $"Numb = {Int32.Parse(textBox4.Text)}, " +
                            $"Price = {Convert.ToDecimal(textBox5.Text)}, " +
                            $"DateProv = '{dateTimePicker.Value.ToString("dd-MM-yyyy")}' " +
                            $"WHERE BookId = {textBox1.Text};";
                        SqlCommand command = new SqlCommand(sql, connection);
                        command.ExecuteNonQuery();
                        MessageBox.Show("Вы исправили запись в БД!");
                        CreateList();
                        comboBox1.SelectedIndex = comboBox1.Items.Count - 1;
                    } 
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message);
                }
                finally
                {
                    connection.Close();
                }
            }    
        }

        private void button5_Click(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                try
                {
                    string sql = $"DELETE Book WHERE BookId = {textBox1.Text};";
                    SqlCommand command = new SqlCommand(sql, connection);
                    command.ExecuteNonQuery();
                    MessageBox.Show("Вы успешно удалили запись с БД!");
                    CreateList();
                    comboBox1.SelectedIndex = comboBox1.Items.Count - 1;
                }
                catch (Exception exception)
                {
                    MessageBox.Show("Не получилось удалить из БД. " + exception.Message);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            DateTimePicker dateTimePicker = new DateTimePicker();
            if (flag)
            {
                button1_Click(sender, e);
                flag = false;
            }
            else
            {
                int id = comboBox1.SelectedIndex;
                string[] list = new string[comboBox1.Items.Count];
                for (int i = 0; i < comboBox1.Items.Count; i++)
                {
                    list[i] = comboBox1.Items[i].ToString();
                }
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlTransaction transaction = connection.BeginTransaction();
                    SqlCommand command = new SqlCommand();
                    command.Transaction = transaction;
                    try
                    {
                        command.Connection = connection;
                        command.CommandText = "UPDATE Book SET " +
                            @"PublishingId = @PublishingId, " +
                            @"AuthorId = @AuthorId, " +
                            @"GenreId = @GenreId, " +
                            @"ProviderId = @ProviderId, " +
                            @"Name = @Name, " +
                            @"DateOfPubl = @DateOfPubl, " +
                            @"Numb = @Numb, " +
                            @"Price = @Price, " +
                            @"DateProv = @DateProv " +
                            @"WHERE BookId = @BookId";

                        command.Parameters.Add("@BookId", SqlDbType.Int);
                        command.Parameters.Add("@PublishingId", SqlDbType.Int);
                        command.Parameters.Add("@AuthorId", SqlDbType.Int);
                        command.Parameters.Add("@GenreId", SqlDbType.Int);
                        command.Parameters.Add("@ProviderId", SqlDbType.Int);
                        command.Parameters.Add("@Name", SqlDbType.NVarChar, 50);
                        command.Parameters.Add("@DateOfPubl", SqlDbType.Date);
                        command.Parameters.Add("@Numb", SqlDbType.Int);
                        command.Parameters.Add("@Price", SqlDbType.Money);
                        command.Parameters.Add("@DateProv", SqlDbType.Date);

                        command.Parameters["@BookId"].Value = textBox1.Text;
                        command.Parameters["@PublishingId"].Value = comboBox2.SelectedIndex + 1;
                        command.Parameters["@AuthorId"].Value = comboBox3.SelectedIndex + 1;
                        command.Parameters["@GenreId"].Value = comboBox4.SelectedIndex + 1;
                        command.Parameters["@ProviderId"].Value = comboBox5.SelectedIndex + 1;
                        command.Parameters["@Name"].Value = textBox2.Text;
                        command.Parameters["@DateOfPubl"].Value = dateTimePicker.Value.ToString("dd-MM-yyyy");
                        command.Parameters["@Numb"].Value = Int32.Parse(textBox4.Text);
                        command.Parameters["@Price"].Value = Convert.ToDecimal(textBox5.Text);
                        command.Parameters["@DateProv"].Value = dateTimePicker.Value.ToString("dd-MM-yyyy");

                        command.ExecuteNonQuery();

                        //Обновляем список на форме
                        comboBox1.Items.Clear();
                        comboBox2.Items.Clear();
                        comboBox3.Items.Clear();
                        comboBox4.Items.Clear();
                        comboBox5.Items.Clear();
                        command.CommandText = "SELECT Name FROM Book;";
                        SqlDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            comboBox1.Items.Add(reader.GetString(0));
                        }
                        reader.Close();

                        //Подтверждаем транзакцию
                        transaction.Commit();
                        MessageBox.Show("Transaction is correct!");
                    }
                    catch (Exception exception)
                    {
                        MessageBox.Show("Не получилось сделать одновременно 2 транзакции. " + exception);
                        transaction.Rollback();
                        comboBox1.Items.AddRange(list);
                    }
                    comboBox1.SelectedIndex = id;
                }
            }

        }

        private void button7_Click(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string sql = "SELECT Genre.Name, Book.DateProv, " +
                        "COUNT(BookId) " +
                        "FROM Book " +
                        "INNER JOIN Genre " +
                        "ON Book.GenreId = Genre.GenreId " +
                        "GROUP BY Genre.Name, Book.DateProv " +
                        "HAVING COUNT(BookId) > 1";
                    SqlCommand command = new SqlCommand(sql, connection);
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows) // если есть данные
                    {
                        // выводим названия столбцов
                        //MessageBox.Show($"{reader.GetName(0)}\t{reader.GetName(1)}\t{reader.GetName(2)}");

                        while (reader.Read()) // построчно считываем данные
                        {
                            object name = reader.GetValue(0);
                            object date = reader.GetValue(1);
                            object numb = reader.GetValue(2);

                            MessageBox.Show($"Genre = {name} \tDate = {date} \tNumbers = {numb}");
                        }
                    }
                    reader.Close();
                }
                catch (Exception exception)
                {
                    MessageBox.Show("Не получилось прочитать вернуть значение. " + exception.Message);
                }
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            DateTimePicker dateTimePicker = new DateTimePicker();
            string sqlExpression = "AddBook"; //Имя хранимой процедуры
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(sqlExpression, connection);
                    //Указываем, что команда представляет хранимую процедуру
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    //Задаём параметры
                    command.Parameters.Add(new SqlParameter("@PublishingId", comboBox2.SelectedIndex + 1));
                    command.Parameters.Add(new SqlParameter("@AuthorId", comboBox3.SelectedIndex + 1));
                    command.Parameters.Add(new SqlParameter("@GenreId", comboBox4.SelectedIndex + 1));
                    command.Parameters.Add(new SqlParameter("@ProviderId", comboBox5.SelectedIndex + 1));
                    command.Parameters.Add(new SqlParameter("@Name", textBox2.Text));
                    command.Parameters.Add(new SqlParameter("@DateOfPubl", dateTimePicker.Value.ToString("dd-MM-yyyy")));
                    command.Parameters.Add(new SqlParameter("@Numb", Int32.Parse(textBox4.Text)));
                    command.Parameters.Add(new SqlParameter("@Price", Convert.ToDecimal(textBox5.Text)));
                    command.Parameters.Add(new SqlParameter("@DateProv", dateTimePicker.Value.ToString("dd-MM-yyyy")));

                    var result = command.ExecuteScalar();
                    //Если нам не надо возвращать id
                    //var result = command.ExecuteNonQuery();
                    MessageBox.Show("Id добавленного объекта: " + result);
                }
                catch (Exception exception)
                {
                    MessageBox.Show("Не получилось запустить процедуру добавления изображения. " + exception.Message);
                }
            }
        }
    }
}
