using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
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
                    string sql = "SELECT * FROM Instruments";
                    SqlCommand command = new SqlCommand(sql, connection);
                    SqlDataReader reader = command.ExecuteReader();

                    int i = 0;
                    while (reader.Read())
                    {
                        if (i == stringN)
                        {
                            int id = reader.GetInt32(0);
                            string filename = reader.GetString(1);
                            string mark = reader.GetString(2);
                            string name = reader.GetString(3);
                            byte[] data = (byte[])reader.GetValue(4);

                            textBox1.Text = id.ToString();
                            textBox2.Text = filename;
                            textBox3.Text = mark;
                            textBox4.Text = name;

                            //Пишем изображение в pictureBox1
                            using (MemoryStream inStream = new MemoryStream())
                            {
                                inStream.Write(data, 0, data.Length);
                                Image image = Image.FromStream(inStream);
                                pictureBox1.Image = new Bitmap(image);
                            }
                            break;
                        }
                        else i++;
                    }
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
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string sql = "SELECT Name FROM Instruments";
                    SqlCommand command = new SqlCommand(sql, connection);
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        comboBox1.Items.Add(reader.GetString(0));
                    }
                }
                catch (Exception exception)
                {
                    MessageBox.Show("Не получилось прочитать из БД. " + exception.Message);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog open_dialog = new OpenFileDialog(); //Создание диалогового окна для выбора файла
            open_dialog.Filter = "Image Files(*.BMP;*.JPG;*.GIF;*.PNG)|*.BMP;*.JPG;*.GIF;*.PNG|All files (*.*)|*.*"; //Формат загружаемого файла
            if (open_dialog.ShowDialog() == DialogResult.OK) //Если в окне была нажата кнопка "ОК"
            {
                try
                {
                    textBox2.Text = open_dialog.SafeFileName;
                    pictureBox1.Image = new Bitmap(open_dialog.FileName);
                    pictureBox1.Invalidate();
                }
                catch
                {
                    DialogResult rezult = MessageBox.Show("Невозможно открыть выбранный файл", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand();
                    command.Connection = connection;
                    command.CommandText = @"INSERT INTO Instruments VALUES ( @NameFile, @Mark, @Name, @ImageData)";
                    //command.Parameters.Add("@Id", SqlDbType.Int);
                    command.Parameters.Add("@NameFile", SqlDbType.NVarChar, 50);
                    command.Parameters.Add("@Mark", SqlDbType.NVarChar, 50);
                    command.Parameters.Add("@Name", SqlDbType.NVarChar, 50);
                    command.Parameters.Add("@ImageData", SqlDbType.Image, 1000000);

                    //Передаем данные в команду через параметры
                    ImageConverter converter = new ImageConverter();
                    //command.Parameters["@Id"].Value = textBox1.Text;
                    command.Parameters["@NameFile"].Value = textBox2.Text;
                    command.Parameters["@Mark"].Value = textBox3.Text;
                    command.Parameters["@Name"].Value = textBox4.Text;
                    command.Parameters["@ImageData"].Value = (byte[])converter.ConvertTo(pictureBox1.Image, typeof(byte[]));
                    command.ExecuteNonQuery();
                    MessageBox.Show("You have added new item!");
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message);
                }
            }
            CreateList();
            comboBox1.SelectedIndex = comboBox1.Items.Count - 1;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex >= 0)
            {
                ReadLine(comboBox1.SelectedIndex);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 0) comboBox1.SelectedIndex = comboBox1.Items.Count - 1;
            else comboBox1.SelectedIndex--;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == comboBox1.Items.Count - 1) comboBox1.SelectedIndex = 0;
            else comboBox1.SelectedIndex++;
        }

        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
            Form form = Application.OpenForms[0];
            form.Close();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            button1_Click(sender, e);
            flag = true;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (flag)
            {
                button2_Click(sender, e);
                flag = false;
            }
            else
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        SqlCommand command = new SqlCommand();
                        command.Connection = connection;
                        command.CommandText = @"UPDATE Instruments SET NameFile = @NameFile, Mark = @Mark, Name = @Name, ImageData = @ImageData WHERE Id = @Id";
                        command.Parameters.Add("@Id", SqlDbType.Int);
                        command.Parameters.Add("@NameFile", SqlDbType.NVarChar, 50);
                        command.Parameters.Add("@Mark", SqlDbType.NVarChar, 50);
                        command.Parameters.Add("@Name", SqlDbType.NVarChar, 50);
                        command.Parameters.Add("@ImageData", SqlDbType.Image, 1000000);

                        //Передаем данные в команду через параметры
                        ImageConverter converter = new ImageConverter();
                        command.Parameters["@Id"].Value = textBox1.Text;
                        command.Parameters["@NameFile"].Value = textBox2.Text;
                        command.Parameters["@Mark"].Value = textBox3.Text;
                        command.Parameters["@Name"].Value = textBox4.Text;
                        command.Parameters["@ImageData"].Value = (byte[])converter.ConvertTo(pictureBox1.Image, typeof(byte[]));
                        command.ExecuteNonQuery();

                        //Обновляем список на форме
                        int id = comboBox1.SelectedIndex;
                        CreateList();
                        comboBox1.SelectedIndex = id;
                    }
                    catch (Exception exception)
                    {
                        MessageBox.Show("Не получилось сохранить изменения в БД. " + exception.Message);
                    }
                }
            }

        }

        private void button8_Click(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand();
                    command.Connection = connection;
                    command.CommandText = @"DELETE Instruments WHERE Id = @Id";
                    command.Parameters.Add("@Id", SqlDbType.Int);

                    //Передаем данные в команду через параметры
                    ImageConverter converter = new ImageConverter();
                    command.Parameters["@Id"].Value = textBox1.Text;
                    command.ExecuteNonQuery();

                    //Обновляем список на форме
                    CreateList();
                    comboBox1.SelectedIndex = comboBox1.Items.Count - 1;
                }
                catch (Exception exception)
                {
                    MessageBox.Show("Не получилось удалить из БД. " + exception.Message);
                }
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog open_dialog = new OpenFileDialog(); //Создание диалогового окна для выбора файла
            open_dialog.Filter = "Image Files(*.BMP;*.JPG;*.GIF;*.PNG)|*.BMP;*.JPG;*.GIF;*.PNG|All files (*.*)|*.*"; //Формат загружаемого файла
            if (open_dialog.ShowDialog() == DialogResult.OK) //Если в окне была нажата кнопка "ОК"
            {
                try
                {
                    textBox2.Text = open_dialog.SafeFileName;
                    pictureBox1.Image = new Bitmap(open_dialog.FileName);
                    pictureBox1.Invalidate();
                }
                catch
                {
                    DialogResult rezult = MessageBox.Show("Невозможно открыть выбранный файл", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (flag)
            {
                button2_Click(sender, e);
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
                        command.CommandText = @"UPDATE Instruments SET NameFile = @NameFile, Mark = @Mark, Name = @Name, ImageData = @ImageData WHERE Id = @Id";
                        command.Parameters.Add("@Id", SqlDbType.Int);
                        command.Parameters.Add("@NameFile", SqlDbType.NVarChar, 50);
                        command.Parameters.Add("@Mark", SqlDbType.NVarChar, 50);
                        command.Parameters.Add("@Name", SqlDbType.NVarChar, 50);
                        command.Parameters.Add("@ImageData", SqlDbType.Image, 1000000);

                        //Передаем данные в команду через параметры
                        ImageConverter converter = new ImageConverter();
                        command.Parameters["@Id"].Value = textBox1.Text;
                        command.Parameters["@NameFile"].Value = textBox2.Text;
                        command.Parameters["@Mark"].Value = textBox3.Text;
                        command.Parameters["@Name"].Value = textBox4.Text;
                        command.Parameters["@ImageData"].Value = (byte[])converter.ConvertTo(pictureBox1.Image, typeof(byte[]));
                        command.ExecuteNonQuery();

                        //Обновляем список на форме
                        comboBox1.Items.Clear();
                        command.CommandText = "SELECT Name FROM Instruments";
                        SqlDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            comboBox1.Items.Add(reader.GetString(0));
                        }
                        reader.Close();

                        //Подтверждаем транзакцию
                        transaction.Commit();
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

        private void button10_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog(); //Создание диалогового окна для выбора имени сохраняемого файла
            saveFileDialog.Filter = "JPEG(*.jpg;*.jpeg;*.jpe;*.jfif)|*.jpg;*.jpeg;*.jpe;*.jfif|PNG(*.png)|*.png|GIF(*.gif)|*.gif|BMP(*.bmp)|*.bmp|All files (*.*)|*.*";
            if (saveFileDialog.ShowDialog() == DialogResult.OK) //Если в окне была нажата кнопка "ОК"
            {
                try
                {
                    ImageConverter converter = new ImageConverter();
                    using (System.IO.FileStream fs = new System.IO.FileStream(saveFileDialog.FileName, FileMode.OpenOrCreate))
                    {
                        byte[] mas = (byte[])converter.ConvertTo(pictureBox1.Image, typeof(byte[]));
                        fs.Write(mas, 0, mas.Length);
                    }
                }
                catch (Exception exception)
                {
                    MessageBox.Show("Не получилось сохранить файл на диск. " + exception);
                }
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string sql = "SELECT COUNT(*) FROM Instruments";
                    SqlCommand command = new SqlCommand(sql, connection);
                    MessageBox.Show($"В БД есть {command.ExecuteScalar()} картинок");
                }
                catch (Exception exception)
                {
                    MessageBox.Show("Не получилось прочитать вернуть значение. " + exception.Message);
                }
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            string sqlExpression = "AddInstrument"; //Имя хранимой процедуры
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(sqlExpression, connection);
                    //Указываем, что команда представляет хранимую процедуру
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    //Задаём параметры
                    command.Parameters.Add(new SqlParameter("@NameFile", textBox2.Text));
                    command.Parameters.Add(new SqlParameter("@Mark", textBox3.Text));
                    command.Parameters.Add(new SqlParameter("@Name", textBox4.Text));
                    ImageConverter converter = new ImageConverter();
                    command.Parameters.Add(new SqlParameter("@ImageData", (byte[])converter.ConvertTo(pictureBox1.Image, typeof(byte[]))));

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
