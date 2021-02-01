using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Kursach
{
    public partial class Form_main : Form
    {
        public Form_main()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
           
        }

        private void query_to_dataGrid(string query)
        {
            string connectionString =
                "datasource=" + textBox_DBIP.Text + 
                ";port=" + textBox_DBport.Text + 
                ";username=" + textBox_DB_user.Text + 
                ";password=" + textBox_DB_pass.Text + 
                ";database=hotel;";

            try
            {
                MySqlConnection databaseConnection = new MySqlConnection(connectionString);
                MySqlDataAdapter dataAdapter = new MySqlDataAdapter(query, databaseConnection);

                databaseConnection.Open();

                DataSet ds = new DataSet();
                dataAdapter.Fill(ds, "guest");
                dataGridView1.DataSource = ds.Tables["guest"];
                databaseConnection.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button_manual_query_Click(object sender, EventArgs e)
        {
            query_to_dataGrid(Convert.ToString(textBox_manual_DB_request.Text));
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            string date = dateTimePicker1.Value.ToString("yyyy-MM-dd");
            string query =
                "UPDATE room SET room.status = 'booked' WHERE room.room_number IN (" +
                "SELECT check_in.room_id FROM check_in " +
                "WHERE date_in > '" + date +
                "'); "
            +
                "UPDATE room SET room.status = 'checked-in' WHERE room.room_number IN (" +
                "SELECT check_in.room_id FROM check_in " +
                "WHERE date_in <= '" + date + "' AND date_out > '" + date +
                "'); "
            +   
                "UPDATE room SET room.status = 'free' WHERE room.room_number IN (" +
                "SELECT check_in.room_id FROM check_in " +
                "WHERE date_out <= '" + date +
                "');";
            query_to_dataGrid(query);
        }

        private void button_get_all_guest_Click(object sender, EventArgs e)
        {
            query_to_dataGrid("SELECT * FROM guest;");
        }

        private void button_get_all_room_Click(object sender, EventArgs e)
        {
            query_to_dataGrid("SELECT * FROM room;");
        }

        private void button_get_all_chech_in_Click(object sender, EventArgs e)
        {
            query_to_dataGrid("SELECT * FROM check_in;");
        }

        private void button_search_Click(object sender, EventArgs e)
        {
            string table_name = comboBox_columns_for_search.Text.Split(new char[] { '.' })[0];
            string column_name = comboBox_columns_for_search.Text.Split(new char[] { '.' })[1];
            string query = "SELECT " + "*" + " FROM " + table_name + " WHERE " + column_name + " LIKE '%" + textBox_mask_for_search.Text + "%';";
            query_to_dataGrid(query);
        }

        private void button_search_for_free_room_Click(object sender, EventArgs e)
        {
            string query = 
                "SELECT room.room_number, room.capacity, room.type, room.price, room.status FROM room " +
                "WHERE room.capacity >= " + Convert.ToString(comboBox_seaarch_for_free_room_capacity.Text) + 
                " AND room.type = '" + Convert.ToString(comboBox_search_for_free_room_type.Text) + 
                "' AND room.status = 'free'; ";
            query_to_dataGrid(query);
        }

        private void button_insert_Click(object sender, EventArgs e)
        {
            if (textBox_insert_value1.Text == "")
                textBox_insert_value1.Text = "NULL";
            if (textBox_insert_value2.Text == "")
                textBox_insert_value2.Text = "NULL";
            if (textBox_insert_value3.Text == "")
                textBox_insert_value3.Text = "NULL";
            if (textBox_insert_value4.Text == "")
                textBox_insert_value4.Text = "NULL";
            if (textBox_insert_value5.Text == "")
                textBox_insert_value5.Text = "NULL";
            if (textBox_insert_comment.Text == "")
                textBox_insert_comment.Text = "";

            string query = "";

            if (comboBox_insert_table.Text == "check_in")
            {
                query += "INSERT INTO check_in(guest_id, room_id, date_in, date_out, comment) VALUES(" + textBox_insert_value2.Text + 
                    ", " + textBox_insert_value3.Text + ", '" + textBox_insert_value4.Text + "', '" + textBox_insert_value5.Text +
                    "', '" + textBox_insert_comment.Text + "');";
            }

            else if (comboBox_insert_table.Text == "guest")
            {
                query += "INSERT INTO guest VALUES(" + textBox_insert_value1.Text + ", '" + textBox_insert_value2.Text + 
                    "', '" + textBox_insert_value3.Text + "', '" + textBox_insert_value4.Text + "', '" + textBox_insert_value5.Text + 
                    "', '" + textBox_insert_comment.Text + "');";
            }

            else if (comboBox_insert_table.Text == "room")
            {
                query += "INSERT INTO room(room_number, capacity, type, price) VALUES(" + textBox_insert_value1.Text + 
                    ", " + textBox_insert_value2.Text + ", '" + textBox_insert_value3.Text + "', " + textBox_insert_value4.Text + ");";
            }

            query_to_dataGrid(query);

        }

        private void comboBox_insert_table_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox_insert_table.Text == "check_in")
            {
                label_insert_value1.Visible = false;
                textBox_insert_value1.Visible = false;
                label_insert_value2.Text = "guest_id";
                label_insert_value3.Text = "room_id";
                label_insert_value4.Text = "date-in";
                label_insert_value5.Text = "date_out";
                label_insert_comment.Visible = true;
                textBox_insert_comment.Visible = true;
            }

            else if (comboBox_insert_table.Text == "guest")
            {
                label_insert_value1.Visible = true;
                textBox_insert_value1.Visible = true;
                label_insert_value1.Text = "passport_id";
                label_insert_value2.Text = "last_name";
                label_insert_value3.Text = "first_name";
                label_insert_value4.Text = "middle_name";
                label_insert_value5.Text = "birth_date";
                label_insert_comment.Visible = true;
                textBox_insert_comment.Visible = true;
            }

            else if (comboBox_insert_table.Text == "room")
            {
                label_insert_value1.Visible = true;
                textBox_insert_value1.Visible = true;
                label_insert_value1.Text = "room_number";
                label_insert_value2.Text = "capacity";
                label_insert_value3.Text = "type";
                label_insert_value4.Text = "price";
                label_insert_value5.Text = "status";
                label_insert_comment.Visible = false;
                textBox_insert_comment.Visible = false;
            }
        }

        private void comboBox_update_table_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox_update_table.Text == "check_in")
            {
                label_update_value1.Text = "check_in_id";
                label_update_value2.Text = "guest_id";
                label_update_value3.Text = "room_id";
                label_update_value4.Text = "date-in";
                label_update_value5.Text = "date_out";
                label_update_comment.Visible = true;
                textBox_update_comment.Visible = true;
            }

            else if (comboBox_update_table.Text == "guest")
            {
                label_update_value1.Text = "passport_id";
                label_update_value2.Text = "last_name";
                label_update_value3.Text = "first_name";
                label_update_value4.Text = "middle_name";
                label_update_value5.Text = "birth_date";
                label_update_comment.Visible = true;
                textBox_update_comment.Visible = true;
            }

            else if (comboBox_update_table.Text == "room")
            {
                label_update_value1.Text = "room_number";
                label_update_value2.Text = "capacity";
                label_update_value3.Text = "type";
                label_update_value4.Text = "price";
                label_update_value5.Text = "status";
                label_update_comment.Visible = false;
                textBox_update_comment.Visible = false;
            }
        }

        private void button_update_Click(object sender, EventArgs e)
        {
            if (textBox_update_value1.Text == "")
            {
                label_update_isKeyDefined.Text = "NO KEY";
                return;
            }
            else label_update_isKeyDefined.Text = "";

            string query = "UPDATE " + comboBox_update_table.Text + " SET ";

            if (comboBox_update_table.Text == "check_in")
            {
                if (textBox_update_value2.Text != "")
                {
                    query += label_update_value2.Text + " = " + textBox_update_value2.Text + ", ";
                }

                if (textBox_update_value3.Text != "")
                {
                    query += label_update_value3.Text + " = " + textBox_update_value3.Text + ", ";
                }

                if (textBox_update_value4.Text != "")
                {
                    query += label_update_value4.Text + " = '" + textBox_update_value4.Text + "', ";
                }

                if (textBox_update_value5.Text != "")
                {
                    query += label_update_value5.Text + " = '" + textBox_update_value5.Text + "', ";
                }

                if (textBox_update_comment.Text != "")
                {
                    query += label_update_comment.Text + " = '" + textBox_update_comment.Text + "'";
                }
            }

            else if (comboBox_update_table.Text == "guest")
            {
                if (textBox_update_value2.Text != "")
                {
                    query += label_update_value2.Text + " = '" + textBox_update_value2.Text + "', ";
                }

                if (textBox_update_value3.Text != "")
                {
                    query += label_update_value3.Text + " = '" + textBox_update_value3.Text + "', ";
                }

                if (textBox_update_value4.Text != "")
                {
                    query += label_update_value4.Text + " = '" + textBox_update_value4.Text + "', ";
                }

                if (textBox_update_value5.Text != "")
                {
                    query += label_update_value5.Text + " = '" + textBox_update_value5.Text + "', ";
                }

                if (textBox_update_comment.Text != "")
                {
                    query += label_update_comment.Text + " = '" + textBox_update_comment.Text + "'";
                }
            }

            else if (comboBox_update_table.Text == "room")
            {
                if (textBox_update_value2.Text != "")
                {
                    query += label_update_value2.Text + " = " + textBox_update_value2.Text + ", ";
                }

                if (textBox_update_value3.Text != "")
                {
                    query += label_update_value3.Text + " = '" + textBox_update_value3.Text + "', ";
                }

                if (textBox_update_value4.Text != "")
                {
                    query += label_update_value4.Text + " = " + textBox_update_value4.Text + ", ";
                }

                if (textBox_update_value5.Text != "")
                {
                    query += label_update_value5.Text + " = '" + textBox_update_value5.Text + "'";
                }
            }
            
            query = query.TrimEnd(new char[] { ' ', ',' });

            query += " WHERE " + label_update_value1.Text + " = " + textBox_update_value1.Text + ";";

            query_to_dataGrid(query);

        }

        private void comboBox_delete_table_row_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox_delete_table_row.Text == "check_in")
            {
                label_delete_value1.Text = "check_in_id";
            }

            else if (comboBox_delete_table_row.Text == "guest")
            {
                label_delete_value1.Text = "passport_id";
            }

            else if (comboBox_delete_table_row.Text == "room")
            {
                label_delete_value1.Text = "room_number";
                
            }
        }

        private void button_delete_row_Click(object sender, EventArgs e)
        {
            if (textBox_delete_value1.Text == "")
            {
                label_delete_isKeyDefined.Text = "NO KEY";
                return;
            }
            else label_delete_isKeyDefined.Text = "";

            string query = "DELETE FROM " + comboBox_delete_table_row.Text + " WHERE " + label_delete_value1.Text + " = " + textBox_delete_value1.Text + ";";

            query_to_dataGrid(query);
        }

        private void button_calculate_price_Click(object sender, EventArgs e)
        {
            string connectionString = 
                "datasource=" + textBox_DBIP.Text + 
                ";port=" + textBox_DBport.Text + 
                ";username=" + textBox_DB_user.Text + 
                ";password=" + textBox_DB_pass.Text + 
                ";database=hotel;";

            string query = "SELECT " +
                "IF((SELECT COUNT(check_in.check_in_id) FROM check_in WHERE check_in.guest_id = (SELECT check_in.guest_id FROM check_in WHERE check_in.check_in_id = "
                + textBox_calculate_price_check_in_id.Text + ") )> 5, 0.95, 1) * (check_in.date_out - check_in.date_in) *" +
                " (SELECT room.price FROM room WHERE room.room_number = check_in.room_id) FROM check_in WHERE check_in.check_in_id = " 
                + textBox_calculate_price_check_in_id.Text + ";";

            MySqlConnection databaseConnection = new MySqlConnection(connectionString);
            MySqlCommand commandToDB = new MySqlCommand(query, databaseConnection);
            commandToDB.CommandTimeout = 60;
            MySqlDataReader reader;

            try
            {
                databaseConnection.Open();
                reader = commandToDB.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                        textBox_calculate_price_tp_pay.Text = reader.GetString(0);
                }
                else textBox_calculate_price_tp_pay.Text = "ERROR";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
