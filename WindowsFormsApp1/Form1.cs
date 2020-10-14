using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        // серверы для лаб (*_*)//
        //==============================================================================================================================================
        //static string DB = "host=62.149.24.91;Initial Catalog = maindb;User Id = dbadmin;Password=superadmin;CharSet=UTF8;Connect Timeout = 100";
        //"Server=localhost;Database=database;Uid=username;Pwd=password;"
        static string DB = "server=62.149.24.91;database=maindb;Uid=nopass;";
        //==============================================================================================================================================
        
        public Form1()
        {
            InitializeComponent();
        }
        public void clear_fields() 
        {
            game_IDTextBox.Text = "";
            nameTextBox.Text = "";
            descriptionTextBox.Text = "";
            type_IDTextBox.Text = "";
        }
        public void display_data()// моментальное отображение даных таблици)
        {
            MySqlConnection conn_for_db = new MySqlConnection(DB);
            conn_for_db.Open();
            
            MySqlCommand cmd = conn_for_db.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select * from GAME;";
            cmd.ExecuteScalar();
            DataTable dta = new DataTable();
            MySqlDataAdapter dataapdt = new MySqlDataAdapter(cmd);
            dataapdt.Fill(dta);
            dataGridView1.DataSource = dta;
            
            conn_for_db.Close();

            clear_fields();
        }
        static List<object> data = new List<object> { };
        public void spell_for_mysql(string sql_request) // delete_update_insert 
        {
            MySqlConnection conn_for_db = new MySqlConnection(DB);
            conn_for_db.Open();
            MySqlCommand command = new MySqlCommand(sql_request, conn_for_db);
            command.ExecuteNonQuery();
            conn_for_db.Close();  
        }
        public void spell_for_mysql_search(object command)  // подключаешся к SQL и выполняешь действие которые тебе нужны (по сути переменная  СOMMAND , это код на SQL)
        {                                       // а суть метода в том чтобы закинуть заклинание SQL на Windows form 
            MySqlConnection conn_for_db = new MySqlConnection(DB);
            conn_for_db.Open();
            MySqlCommand cmd = conn_for_db.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = Convert.ToString(command);

            cmd.ExecuteScalar();
            conn_for_db.Close();
            DataTable dt = new DataTable();
            MySqlDataAdapter da = new MySqlDataAdapter(cmd);
            da.Fill(dt);

            dataGridView1.DataSource = dt;

            clear_fields();
        }
            private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'azart_companyDataSet.Game' table. You can move, or remove it, as needed.
            // this.gameTableAdapter.Fill(this.azart_companyDataSet.Game);
            display_data();
        }

        private void gameBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
          //  this.gameBindingSource.EndEdit();
          //  this.tableAdapterManager.UpdateAll(this.azart_companyDataSet);

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {     
            if (radioButton1.Checked)// Search 
            {
                if (checkBox1.Checked && (checkBox2.Checked == false))
                {
                    spell_for_mysql_search($"SELECT * FROM GAME WHERE (game_id = {game_IDTextBox.Text})");
                    //пошук за ID
                }
                else if (checkBox1.Checked == false && checkBox2.Checked)
                {
                    spell_for_mysql_search($"SELECT * FROM GAME WHERE (game_type = {type_IDTextBox.Text})");
                    //пошук за типом гри
                }
                else if (checkBox1.Checked && checkBox2.Checked)
                {
                    spell_for_mysql_search($"Select * from GAME where ((game_id =  {game_IDTextBox.Text}) AND (game_type = {type_IDTextBox.Text}))"); 
                    //пошук за ID та Типом 
                }

            }

            if (radioButton2.Checked) //Insert 
            {
                spell_for_mysql("Insert into GAME(game_id, game_name, game_description, game_type) Values('" + game_IDTextBox.Text + "','" + nameTextBox.Text + "','" + descriptionTextBox.Text + "','" + type_IDTextBox.Text + "')");
                display_data();
            }

            if (radioButton3.Checked) //Update            
            {
                if (checkBox1.Checked && (checkBox2.Checked == false))
                {
                    spell_for_mysql($"update GAME set game_id = {game_IDTextBox.Text}, game_name = '{nameTextBox.Text}', game_description = '{descriptionTextBox.Text}', game_type = {type_IDTextBox.Text} where game_id = {game_IDTextBox.Text}");
                    display_data();
                }
                else if (checkBox1.Checked == false && checkBox2.Checked)
                {
                    spell_for_mysql($"update GAME set game_id = {game_IDTextBox.Text}, game_name = '{nameTextBox.Text}', game_description = '{descriptionTextBox.Text}', game_type = {type_IDTextBox.Text} where game_type = {type_IDTextBox.Text}");
                    display_data();
                    //update GAME set game_id = {game_IDTextBox.Text}, game_name = '{nameTextBox.Text}', game_description = '{descriptionTextBox.Text}', game_type = {type_IDTextBox.Text} where game_type = {game_IDTextBox.Text}";
                }
                else if (checkBox1.Checked && checkBox2.Checked)
                {
                    spell_for_mysql($"update GAME set game_id = {game_IDTextBox.Text}, game_name = '{nameTextBox.Text}', game_description = '{descriptionTextBox.Text}', game_type = {type_IDTextBox.Text}  where (( game_type = {type_IDTextBox.Text} ) AND (game_id = {game_IDTextBox.Text}))");
                    display_data();
                }
            }

            if (radioButton4.Checked) //Delete
            {
                if (checkBox1.Checked && (checkBox2.Checked == false))
                {
                    spell_for_mysql($"delete from GAME where game_id = {game_IDTextBox.Text};");
                    display_data();
                }
                else if (checkBox1.Checked == false && checkBox2.Checked)
                {
                    spell_for_mysql($"delete from GAME where game_type = {type_IDTextBox.Text};");
                    display_data();
                }
                else if (checkBox1.Checked && checkBox2.Checked)
                {
                    spell_for_mysql($"delete from GAME where (game_id = {game_IDTextBox.Text}) AND (game_type = {type_IDTextBox.Text});");
                    display_data();
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }
    }
}
