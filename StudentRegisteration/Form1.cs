using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace StudentRegisteration
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Load();
        }

        SqlConnection con = new SqlConnection("Data Source=DESKTOP-QJ64AE6\\SQLEXPRESS; Initial Catalog=StudentDB; User Id=sa; Password=hello12;");
        SqlCommand cmd;
        SqlDataReader data;

        string id, sql;
        bool Mode = true;

        

        //REFRESH DB
        public void Load()
        {
            try
            {
                dataGridView1.Rows.Clear();

                sql = "select * from student_table2";
                cmd = new SqlCommand(sql, con);

                con.Open();

                data = cmd.ExecuteReader();
                while (data.Read())
                {
                    dataGridView1.Rows.Add(data[0], data[1], data[2], data[3]);
                }

                con.Close();
            }

            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }


        }

        //FILLS DATA IN TEXTBOX AFTER CLICKING EDIT
        public void FillData(string id)
        {

            button1.Text = "EDIT";
            sql = "select * from student_table2 where id= '"+ id +"'  ";
            cmd = new SqlCommand(sql, con);

            con.Open();
            data=cmd.ExecuteReader();

            while(data.Read())
            {
                TxtName.Text = data[1].ToString();
                TxtCourse.Text = data[2].ToString();
                TxtFee.Text = data[3].ToString();
            }

            con.Close();
            button1.Text = "SAVE";
        }


        //EDIT AND DELETE
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dataGridView1.Columns["Edit"].Index && e.RowIndex>=0)
            {
                Mode = false;

                

                id=dataGridView1.CurrentRow.Cells[0].Value.ToString();
                FillData(id);
            }

            else if (e.ColumnIndex == dataGridView1.Columns["Delete"].Index && e.RowIndex>=0)
            {
                Mode = false;
                id = dataGridView1.CurrentRow.Cells[0].Value.ToString();

                sql = "DELETE FROM student_table2 WHERE id='" + id + "'";
                cmd=new SqlCommand(sql, con);



                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();

                

                Load();
                MessageBox.Show("Record Deleted.");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {

            TxtName.Clear();
            TxtCourse.Clear();
            TxtFee.Clear();

            TxtName.Focus();
        }


        //SAVE BTN
        private void button1_Click(object sender, EventArgs e)
        {
            string Name = TxtName.Text;
            string Course = TxtCourse.Text;
            string Fee = TxtFee.Text;

            if (Mode)
            {
                sql = "insert into student_table2(name,course,fee) values(@Name,@Course,@Fee)";
                cmd = new SqlCommand(sql, con);

                con.Open();
                cmd.Parameters.AddWithValue("@Name", Name);
                cmd.Parameters.AddWithValue("@Course", Course);
                cmd.Parameters.AddWithValue("@Fee", Fee);

                TxtName.Clear();
                TxtCourse.Clear();
                TxtFee.Clear();

                TxtName.Focus();

                cmd.ExecuteNonQuery();
                MessageBox.Show("Record Added.");


                con.Close();

                Load();

            }
            else
            {
                string name=TxtName.Text;
                string course=TxtCourse.Text;
                string fee=TxtFee.Text;

                sql = "UPDATE student_table2 SET name='" + name + "',course='"+course+"',fee='"+fee+"' WHERE id='"+id+"'";
                cmd=new SqlCommand(sql, con);
                
                
                con.Open();
                cmd.ExecuteNonQuery();

                TxtName.Clear();
                TxtCourse.Clear();
                TxtFee.Clear();

                MessageBox.Show("Record Edited");
                Mode = true;
                
                con.Close();

                Load();


            }

        }


    }
}
