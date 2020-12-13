using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace laba2_Books_ADO
{
    public partial class Form1 : Form
    {
        string connectionString = ConfigurationManager.ConnectionStrings["Laba1Connection"].ConnectionString;
        string sql = "SELECT * FROM Book";
        SqlDataAdapter adapter;
        DataSet ds;
        int pSize = 10;
        int pNumber = 0;
        bool isChanges = false;
        bool isNew = false;
        Laba1Entities db = new Laba1Entities();

        public Form1()
        {
            InitializeComponent();

            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            //Запрет добавления новых строк
            dataGridView1.AllowUserToAddRows = false;
            //Запрет редактирования таблицы
            dataGridView1.ReadOnly = true;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                adapter = new SqlDataAdapter(GetSql(), connection);
                ds = new DataSet();
                adapter.Fill(ds, "Book");
                dataGridView1.DataSource = ds.Tables[0];
            }

            dataGridView2.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            dataGridView2.AllowUserToAddRows = false;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                adapter = new SqlDataAdapter(sql, connection);
                adapter.Fill(ds, "Book");
                dataGridView2.DataSource = ds.Tables[0];
                dataGridView2.Columns["BookId"].ReadOnly = true;
            }
            isChangesSave();

            fillAll();

        }

        private void fillSome(ComboBox comboBox, string sql, string table, string displayMember, string valueMember, int dataSource, int mainTable)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                adapter = new SqlDataAdapter(sql, connection);
                adapter.Fill(ds, table);
                comboBox.DataSource = ds.Tables[dataSource];
                comboBox.DisplayMember = displayMember;
                comboBox.ValueMember = valueMember;
                comboBox.DataBindings.Add("SelectedValue", ds.Tables[mainTable], valueMember, true);
            }
        }

        

        private void isChangesSave()
        {
            ds.Tables[0].RowChanged += delegate { isChanges = true; };
            ds.Tables[0].RowDeleted += delegate { isChanges = true; };
            ds.Tables[0].TableNewRow += delegate { isChanges = true; };
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: данная строка кода позволяет загрузить данные в таблицу "laba1DataSet.Provider". При необходимости она может быть перемещена или удалена.
            this.providerTableAdapter.Fill(this.laba1DataSet.Provider);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "laba1DataSet.Genre". При необходимости она может быть перемещена или удалена.
            this.genreTableAdapter.Fill(this.laba1DataSet.Genre);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "laba1DataSet.Publishing". При необходимости она может быть перемещена или удалена.
            this.publishingTableAdapter.Fill(this.laba1DataSet.Publishing);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "laba1DataSet.Author". При необходимости она может быть перемещена или удалена.
            this.authorTableAdapter.Fill(this.laba1DataSet.Author);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "laba1DataSet.Book". При необходимости она может быть перемещена или удалена.
            this.bookTableAdapter.Fill(this.laba1DataSet.Book);

        }

        private string GetSql()
        {
            //Рабоать эта конструкция будет строго для SQL Server 2012 (11.x) и более поздних версий
            return "SELECT * " +
                    "FROM(SELECT *, row_number() OVER(ORDER BY BookId) as Nom FROM Book) as s " +
                    "ORDER BY Nom OFFSET ((" + pNumber + ") * " + pSize + ") " + "ROWS FETCH NEXT " + pSize + "ROWS ONLY";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (pNumber == 0) return;
            pNumber--;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                adapter = new SqlDataAdapter(GetSql(), connection);
                ds.Tables["Book"].Rows.Clear(); //Чистим таблицу Student нашего DataSet
                adapter.Fill(ds, "Book"); //И заполняем по новой конкретно её
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (ds.Tables["Book"].Rows.Count < pSize) return;
            pNumber++;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                adapter = new SqlDataAdapter(GetSql(), connection);
                ds.Tables["Book"].Rows.Clear();
                adapter.Fill(ds, "Book");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                adapter.SelectCommand = new SqlCommand(GetSql(), connection);
                DataRow newRow = ds.Tables[0].NewRow();
                newRow["PublishingId"] = 1;
                newRow["AuthorId"] = 2;
                newRow["GenreId"] = 1;
                newRow["ProviderId"] = 7;
                newRow["Name"] = "Contra spem spero";
                newRow["DateOfPubl"] = "29-07-2001";
                newRow["Numb"] = 5;
                newRow["Price"] = 250;
                newRow["DateProv"] = "27-08-2002";
                ds.Tables[0].Rows.Add(newRow);

                SqlCommandBuilder commandBuilder = new SqlCommandBuilder(adapter);
                adapter.Update(ds.Tables[0]);

                //Перезагружаем данные чтобы обновился StudentId который под автоинкрементом
                ds.Tables[0].Clear();
                adapter.Fill(ds.Tables[0]);

                MessageBox.Show("Вы добавили новую запись в БД!");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            BindingContext[ds, "Book"].EndCurrentEdit();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                adapter.SelectCommand = new SqlCommand(GetSql(), connection);

                //Устанавливаем команду на вставку
                adapter.InsertCommand = new SqlCommand("AddBookNew", connection);
                //Указываем что это будет хранимая процедура
                adapter.InsertCommand.CommandType = CommandType.StoredProcedure;
                //Добавляем параметры
               // adapter.InsertCommand.Parameters.Add(new SqlParameter("@bookId", SqlDbType.Int, 0, "BookId"));
                adapter.InsertCommand.Parameters.Add(new SqlParameter("@publishingId", SqlDbType.Int, 0, "PublishingId"));
                adapter.InsertCommand.Parameters.Add(new SqlParameter("@authorId", SqlDbType.Int, 0, "AuthorId"));
                adapter.InsertCommand.Parameters.Add(new SqlParameter("@genreId", SqlDbType.Int, 0, "GenreId"));
                adapter.InsertCommand.Parameters.Add(new SqlParameter("@providerId", SqlDbType.Int, 0, "ProviderId"));
                adapter.InsertCommand.Parameters.Add(new SqlParameter("@name", SqlDbType.NVarChar, 50, "Name"));
                adapter.InsertCommand.Parameters.Add(new SqlParameter("@dateOfPubl", SqlDbType.Date, 0, "DateOfPubl"));
                adapter.InsertCommand.Parameters.Add(new SqlParameter("@numb", SqlDbType.Int, 0, "Numb"));
                adapter.InsertCommand.Parameters.Add(new SqlParameter("@price", SqlDbType.Money, 0, "Price"));
                adapter.InsertCommand.Parameters.Add(new SqlParameter("@dateProv", SqlDbType.Date, 0, "DateProv"));

                //Добавляем выходной параметр для Id
                SqlParameter parameter = adapter.InsertCommand.Parameters.Add("@bookId", SqlDbType.Int, 0, "BookId");
                parameter.Direction = ParameterDirection.Output;

                //Добавим новую строку в таблицу
                DataRow newRow = ds.Tables[0].NewRow();
                newRow["PublishingId"] = 1;
                newRow["AuthorId"] = 2;
                newRow["GenreId"] = 1;
                newRow["ProviderId"] = 7;
                newRow["Name"] = "Contra spem spero 2.0";
                newRow["DateOfPubl"] = "29-07-2013";
                newRow["Numb"] = 5;
                newRow["Price"] = 250;
                newRow["DateProv"] = "27-08-2020";
                ds.Tables[0].Rows.Add(newRow);

                //По итогу получим StudentId чт и подтвердит использование хранимой процедуры
                adapter.Update(ds.Tables[0]);
                ds.Tables[0].AcceptChanges();

                MessageBox.Show("Вы добавили новую запись процедурой в БД!");
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            //Добавляем новую пустую строку по той же схеме что и в таблице и добавляем её туда
            DataRow row = ds.Tables[0].NewRow();
            ds.Tables[0].Rows.Add(row);

            //Выделяем созданную строку
            dataGridView2.ClearSelection();
            dataGridView2.Rows[dataGridView2.Rows.Count - 1].Selected = true;
            dataGridView2.FirstDisplayedScrollingRowIndex = dataGridView2.Rows.Count - 1;

            isNew = true;
        }

        private void fillNew()
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
        }

        

        private void button6_Click(object sender, EventArgs e)
        {
            //Удаляем выделенные строки из dataGridView2
            foreach (DataGridViewRow row in dataGridView2.SelectedRows)
            {
                dataGridView2.Rows.Remove(row);
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            BindingContext[ds, "Book"].EndCurrentEdit();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                adapter = new SqlDataAdapter(sql, connection);
                SqlCommandBuilder commandBuilder = new SqlCommandBuilder(adapter);
                adapter.InsertCommand = new SqlCommand("AddBookNew", connection);
                adapter.InsertCommand.CommandType = CommandType.StoredProcedure;
                adapter.InsertCommand.Parameters.Add(new SqlParameter("@PublishingId", SqlDbType.Int, 0, "PublishingId"));
                adapter.InsertCommand.Parameters.Add(new SqlParameter("@authorId", SqlDbType.Int, 0, "AuthorId"));
                adapter.InsertCommand.Parameters.Add(new SqlParameter("@genreId", SqlDbType.Int, 0, "GenreId"));
                adapter.InsertCommand.Parameters.Add(new SqlParameter("@providerId", SqlDbType.Int, 0, "ProviderId"));
                adapter.InsertCommand.Parameters.Add(new SqlParameter("@name", SqlDbType.NVarChar, 50, "Name"));
                adapter.InsertCommand.Parameters.Add(new SqlParameter("@dateOfPubl", SqlDbType.Date, 0, "DateOfPubl"));
                adapter.InsertCommand.Parameters.Add(new SqlParameter("@numb", SqlDbType.Int, 0, "Numb"));
                adapter.InsertCommand.Parameters.Add(new SqlParameter("@price", SqlDbType.Money, 0, "Price"));
                adapter.InsertCommand.Parameters.Add(new SqlParameter("@dateProv", SqlDbType.Date, 0, "DateProv"));

                //Добавляем выходной параметр для Id
                SqlParameter parameter = adapter.InsertCommand.Parameters.Add("@bookId", SqlDbType.Int, 0, "BookId");
                parameter.Direction = ParameterDirection.Output;
                
                adapter.Update(ds.Tables[0]);

                MessageBox.Show("You have saved the changes!");
            }
            isChanges = false;
        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (isChanges && MessageBox.Show("Хотите закрыть форму не сохранив изменения?", "Внимание!", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No)
            {
                e.Cancel = true;
            }

        }

        private void fillAll()
        {
            if (isNew)
            {
                fillNew();
                textBox1.DataBindings.Add("Text", ds.Tables[0], "BookId");

                fillSome(comboBox1, "SELECT p.PublishingId, p.Name AS PublName " +
                    "FROM  Publishing AS p ", "Publishing", "PublName", "PublishingId", 1, 0);
                fillSome(comboBox2, "SELECT b.AuthorId, a.Surname + ' ' + a.Name AS AuthorName " +
                    "FROM  Author AS a ", "Author", "AuthorName", "AuthorId", 2, 0);
                fillSome(comboBox3, "SELECT g.GenreId, g.Name AS GenreName " +
                    "FROM Genre AS g ", "Genre", "GenreName", "GenreId", 3, 0);
                fillSome(comboBox4, "SELECT DISTINCT p.ProviderId, p.Surname + ' ' + p.Name AS ProviderName " +
                    "FROM Provider AS p ", "Provider", "ProviderName", "ProviderId", 4, 0);

                textBox2.DataBindings.Add("Text", ds.Tables[0], "Name", true);
                textBox4.DataBindings.Add("Text", ds.Tables[0], "Numb");
                textBox5.DataBindings.Add("Text", ds.Tables[0], "Price");
                textBox3.DataBindings.Add("Text", ds.Tables[0], "DateOfPubl");
                textBox6.DataBindings.Add("Text", ds.Tables[0], "DateProv");
            }
            else
            {
                textBox1.Enabled = false;

                DateTimePicker dateTimePicker1 = new DateTimePicker();
                DateTimePicker dateTimePicker2 = new DateTimePicker();
                textBox1.DataBindings.Add("Text", ds.Tables[0], "BookId");

                fillSome(comboBox1, "SELECT DISTINCT b.PublishingId, p.Name AS PublName " +
                    "FROM Book AS b " +
                    "INNER JOIN Publishing AS p " +
                    "ON b.PublishingId = p.PublishingId", "Publishing", "PublName", "PublishingId", 1, 0);
                fillSome(comboBox2, "SELECT DISTINCT b.AuthorId, a.Surname + ' ' + a.Name AS AuthorName " +
                    "FROM Book AS b " +
                    "INNER JOIN Author AS a " +
                    "ON b.AuthorId = a.AuthorId", "Author", "AuthorName", "AuthorId", 2, 0);
                fillSome(comboBox3, "SELECT DISTINCT b.GenreId, g.Name AS GenreName " +
                    "FROM Book AS b " +
                    "INNER JOIN Genre AS g " +
                    "ON b.GenreId = g.GenreId", "Genre", "GenreName", "GenreId", 3, 0);
                fillSome(comboBox4, "SELECT DISTINCT b.ProviderId, p.Surname + ' ' + p.Name AS ProviderName " +
                    "FROM Book AS b " +
                    "INNER JOIN Provider AS p " +
                    "ON b.ProviderId = p.ProviderId", "Provider", "ProviderName", "ProviderId", 4, 0);

                textBox2.DataBindings.Add("Text", ds.Tables[0], "Name", true);
                textBox4.DataBindings.Add("Text", ds.Tables[0], "Numb");
                textBox5.DataBindings.Add("Text", ds.Tables[0], "Price");
                textBox3.DataBindings.Add("Text", ds.Tables[0], "DateOfPubl");
                textBox6.DataBindings.Add("Text", ds.Tables[0], "DateProv");
            }

        }

        private void button8_Click(object sender, EventArgs e)
        {

        }

        private void bookBindingSource_CurrentChanged(object sender, EventArgs e)
        {

        }

       

        private void toolStripSplitButton1_ButtonClick_1(object sender, EventArgs e)
        {
            this.Validate();
            this.bookBindingSource.EndEdit();
            this.bookTableAdapter.Update(laba1DataSet.Book);
        }

        private void dataGridView4_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button9_Click(object sender, EventArgs e)
        {
            var sql = from b in db.Book
                      join AvgNumbOfOrders in (
                        (from Orders in db.Orders
                         group Orders by new
                         {
                             Orders.BookId
                         } into g
                         select new
                         {
                             g.Key.BookId,
                             Numb = (double?)g.Average(p => p.Numb)
                         })) on new { BookId = b.BookId } equals new { BookId = (int)AvgNumbOfOrders.BookId }
                      join OrderGenre in (
                        (from o in db.Orders
                         group new { o.Book, o } by new
                         {
                             o.Book.GenreId
                         } into g
                         select new
                         {
                             g.Key.GenreId,
                             Numb = (double?)g.Average(p => p.o.Numb)
                         })) on new { GenreId = b.GenreId } equals new { GenreId = (int)OrderGenre.GenreId }
                      where
                        AvgNumbOfOrders.Numb > OrderGenre.Numb
                      select new
                      {
                          b.Name,
                          NumbersInOrders = AvgNumbOfOrders.Numb,
                          GenreName = b.Genre.Name,
                          NumbersInGenre = OrderGenre.Numb
                      };

            dataGridView4.DataSource = sql.ToList();

        }
    }
}
