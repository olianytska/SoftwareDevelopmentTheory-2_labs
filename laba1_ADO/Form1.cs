using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace laba1_ADO
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SqlConnectionStringBuilder connectionStringBuilder = new SqlConnectionStringBuilder();
            connectionStringBuilder["Data Source"] = @"(LocalDb)\LocalDBDemo";
            connectionStringBuilder["Initial Catalog"] = "Laba6";
            connectionStringBuilder["UID"] = textBox1.Text;
            connectionStringBuilder["Password"] = textBox2.Text;
            connectionStringBuilder["Integrated Security"] = true;
            using (SqlConnection connection = new SqlConnection(connectionStringBuilder.ConnectionString))
            {
                try
                {
                    connection.Open();
                    //Вывод на экран информации о подключении к базе данных
                    MessageBox.Show("Удалось успешно подключиться к " + connection.Database);
                    Form2 form = new Form2(connectionStringBuilder.ConnectionString);
                    form.Show();
                    this.Hide();
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message);
                }
            }
        }
    }
}
