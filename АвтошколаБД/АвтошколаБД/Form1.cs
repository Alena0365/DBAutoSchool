using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Data.SqlClient;
namespace АвтошколаБД
{
    public partial class Form1 : Form
    {
        private SqlConnection sqlConnection = null;
        private SqlDataAdapter dataAdapter = null;
        private DataSet dataSet = null;
        private SqlCommandBuilder sqlBuilder = null;
        SqlCommand command = null;
        private bool newRowAdding = false;

        private void views (string str)
        {
            dataAdapter = new SqlDataAdapter(str, sqlConnection);
            dataSet = new DataSet();
            dataAdapter.Fill(dataSet);
            dataGridView1.DataSource = dataSet.Tables[0];
            dataAdapter.Dispose();
            dataSet.Dispose();
        }
        private void procedures(string str)
        {
            command = new SqlCommand(str, sqlConnection);
            if (command.ExecuteNonQuery().ToString() == "-1")
            {
                MessageBox.Show("Ошибка!");
            }
            else
            {
                MessageBox.Show("Успешно!");
            }
            command.Dispose();
        }
        public Form1()
        {
            InitializeComponent();
        }
        
        private void LoadData()
        {
            dataGridView1.Visible = true;
            try
            {
                dataAdapter = new SqlDataAdapter("SELECT *, 'Delete' AS [Действие] FROM v_documents1", sqlConnection);
                sqlBuilder = new SqlCommandBuilder(dataAdapter);
                sqlBuilder.GetInsertCommand();
                sqlBuilder.GetUpdateCommand();
                sqlBuilder.GetDeleteCommand();
                dataSet = new DataSet();
                dataAdapter.Fill(dataSet, "t_documents");
                dataGridView1.DataSource = dataSet.Tables["t_documents"];
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    DataGridViewLinkCell linkCell = new DataGridViewLinkCell();
                    dataGridView1[11, i] = linkCell;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void ReloadData()
        {

            try
            {
                dataSet.Tables["t_documents"].Clear();
                dataAdapter.Fill(dataSet, "t_documents");
                dataGridView1.DataSource = dataSet.Tables["t_documents"];
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    DataGridViewLinkCell linkCell = new DataGridViewLinkCell();
                    dataGridView1[11, i] = linkCell;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["TestBD"].ConnectionString);

            sqlConnection.Open();

            if (sqlConnection.State == ConnectionState.Open)
            {
                MessageBox.Show("Подключение установлено!");
            }
            else
            {
                MessageBox.Show("Ошибка подключения.");

            }
        }

        private void автопаркToolStripMenuItem_Click(object sender, EventArgs e)
        {
            views("select * from v_cars");
            dataGridView1.Visible = true ;
            dataGridView1.AllowUserToAddRows = false;

        }

        private void видЗанятийToolStripMenuItem_Click(object sender, EventArgs e)
        {
            views("select * from v_group_types");
            dataGridView1.Visible = true;
            dataGridView1.AllowUserToAddRows = false;

        }

        private void видКППToolStripMenuItem_Click(object sender, EventArgs e)
        {
            views("select * from v_transmission_types");
            dataGridView1.Visible = true;
            dataGridView1.AllowUserToAddRows = false;

        }

        private void городаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            views("select * from v_cities");
            dataGridView1.Visible = true;
            dataGridView1.AllowUserToAddRows = false;

        }

        private void группыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            views("select * from v_groups");
            dataGridView1.Visible = true;
            dataGridView1.AllowUserToAddRows = false;

        }

        private void категорииОбученияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            views("select * from v_categories");
            dataGridView1.Visible = true;
            dataGridView1.AllowUserToAddRows = false;

        }

        private void предметыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            views("select * from v_lesson_types");
            dataGridView1.Visible = true;
            dataGridView1.AllowUserToAddRows = false;

        }

        private void преподавателиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            views("select * from v_teachers");
            dataGridView1.Visible = true;
            dataGridView1.AllowUserToAddRows = false;

        }

        private void студентыToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.ColumnIndex == 11)
                {
                    string task = dataGridView1.Rows[e.RowIndex].Cells[11].Value.ToString();
                    if (task == "Delete")
                    {
                        if(MessageBox.Show("Удалить эту строку?", "Удаление", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                        == DialogResult.Yes)
                        {
                            int rowIndex = e.RowIndex;
                            dataGridView1.Rows.RemoveAt(rowIndex);
                            dataSet.Tables["t_documents"].Rows[rowIndex].Delete();
                            dataAdapter.Update(dataSet, "t_documents");
                        }
                    }
                    else if( task == "Insert")
                    {
                        int rowIndex = dataGridView1.Rows.Count - 2;
                        DataRow row = dataSet.Tables["t_documents"].NewRow();
                        
                        row["Студент"] = dataGridView1.Rows[rowIndex].Cells["Студент"].Value;
                        row["Группа"] = dataGridView1.Rows[rowIndex].Cells["Группа"].Value;
                        row["Преподаватель"] = dataGridView1.Rows[rowIndex].Cells["Преподаватель"].Value;
                        row["Машина"] = dataGridView1.Rows[rowIndex].Cells["Машина"].Value;
                        row["Категория"] = dataGridView1.Rows[rowIndex].Cells["Категория"].Value;
                        row["Часы"] = dataGridView1.Rows[rowIndex].Cells["Часы"].Value;
                        row["Дата"] = dataGridView1.Rows[rowIndex].Cells["Дата"].Value;
                        dataSet.Tables["t_documents"].Rows.Add(row);
                        dataSet.Tables["t_documents"].Rows.RemoveAt(dataSet.Tables["t_documents"].Rows.Count - 1);
                        dataGridView1.Rows.RemoveAt(dataGridView1.Rows.Count - 2);
                        dataGridView1.Rows[e.RowIndex].Cells[11].Value = "Delete";
                        dataAdapter.Update(dataSet, "t_documents");
                        newRowAdding = false;
                    }
                    else if (task == "Update")
                    {
                        int r = e.RowIndex;
                        dataSet.Tables["t_documents"].Rows[r]["Студент"] = dataGridView1.Rows[r].Cells["Студент"].Value;
                        dataSet.Tables["t_documents"].Rows[r]["Группа"] = dataGridView1.Rows[r].Cells["Группа"].Value;
                        dataSet.Tables["t_documents"].Rows[r]["Преподаватель"] = dataGridView1.Rows[r].Cells["Преподаватель"].Value;
                        dataSet.Tables["t_documents"].Rows[r]["Машина"] = dataGridView1.Rows[r].Cells["Машина"].Value;
                        dataSet.Tables["t_documents"].Rows[r]["Категория"] = dataGridView1.Rows[r].Cells["Категория"].Value;
                        dataSet.Tables["t_documents"].Rows[r]["Часы"] = dataGridView1.Rows[r].Cells["Часы"].Value;
                        dataAdapter.Update(dataSet, "t_documents");
                        dataGridView1.Rows[e.RowIndex].Cells[11].Value = "Delete";
                    }
                    ReloadData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void add_student_Click(object sender, EventArgs e)
        {
            procedures($"EXECUTE p_document @new_student = N'{textBox1.Text}', @new_group = N'{textBox2.Text}', @new_teacher = N'{textBox3.Text}', @new_car = N'{textBox4.Text}', @new_category = N'{textBox5.Text}', @new_volume_hour = N'{textBox6.Text}';");
            LoadData();
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
            textBox6.Clear();
            textBox8.Clear();    


        }

        private void button2_Click(object sender, EventArgs e)
        {
            views("select * from v_documents");
            dataGridView1.Visible = true;
            dataGridView1.AllowUserToAddRows = false;
        }
       

        private void главнаяСтраницаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dataGridView1.Visible = false;
            tabControl1.Visible = false;
        }

        private void обновитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
           
            ReloadData();
        }

        private void dataGridView1_UserAddedRow(object sender, DataGridViewRowEventArgs e)
        {
            try
            {
                if (newRowAdding == false)
                {
                    newRowAdding = true;
                    int lastRow = dataGridView1.Rows.Count - 2;
                    DataGridViewRow row = dataGridView1.Rows[lastRow];
                    DataGridViewLinkCell linkCell = new DataGridViewLinkCell();
                    dataGridView1[11, lastRow] = linkCell;
                    row.Cells["Действие"].Value = "Insert";
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (newRowAdding == false)
                {
                    int rowIndex = dataGridView1.SelectedCells[0].RowIndex;
                    DataGridViewRow editingRow = dataGridView1.Rows[rowIndex];
                    DataGridViewLinkCell linkCell = new DataGridViewLinkCell();
                    dataGridView1[11, rowIndex] = linkCell;
                    editingRow.Cells["Действие"].Value = "Update";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LoadData();
            dataGridView1.AllowUserToAddRows = true;
        }

        private void просмотрToolStripMenuItem_Click(object sender, EventArgs e)
        {
            views("select * from v_students");
            dataGridView1.Visible = true;
            dataGridView1.AllowUserToAddRows = false;
        }

        private void добавитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tabControl1.Visible = true;
            dataGridView1.Visible = false;
            tabControl1.SelectedTab = tabPage1;
        }

        private void textBox11_TextChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            procedures($"EXECUTE p_students @new_student_name_1 = N'{textBox7.Text}', @new_student_name_2 = N'{textBox9.Text}', @new_student_name_3 = N'{textBox10.Text}', @new_student_passport = N'{textBox11.Text}', @new_student_city = N'{textBox12.Text}';");
            textBox7.Clear();
            textBox12.Clear();
            textBox9.Clear();
            textBox10.Clear();
            textBox11.Clear();
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {

        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }
    }
}
