using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using MySql.Data.MySqlClient;

namespace dbAdmin
{
    class DBConnect
    {
        private MySqlConnection connection;
        private string server;
        private string uid;
        private string password;

        TextBox serveraadr = Application.OpenForms["Form1"].Controls["txtServer"] as TextBox;
        TextBox usid = Application.OpenForms["Form1"].Controls["txtuserid"] as TextBox;
        TextBox upass = Application.OpenForms["Form1"].Controls["txtpwd"] as TextBox;
        ComboBox databases = Application.OpenForms["Form1"].Controls["cbxDatabase"] as ComboBox;
        ComboBox tables = Application.OpenForms["Form1"].Controls["cbxTable"] as ComboBox;
        DataGridView tableviewer = Application.OpenForms["Form1"].Controls["dgTable"] as DataGridView;

        //Constructor
        public DBConnect()
        {
            Initialize();
        }

        //Initialize values
        private void Initialize()
        {
            databases.Items.Clear();

            server = serveraadr.Text;
            uid = usid.Text;
            password = upass.Text;
            string connectionString = "SERVER=" + server + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";

            connection = new MySqlConnection(connectionString);

            if (this.OpenConnection() == true)
            {
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "SHOW DATABASES;";
                MySqlDataReader Reader;
                Reader = command.ExecuteReader();
                while (Reader.Read())
                {
                    string row = "";
                    for (int i = 0; i < Reader.FieldCount; i++)
                        row += Reader.GetValue(i).ToString();
                        databases.Items.Add(row);
                }
                Reader.Close();
            }

        }
        public void DBSelect()
        {
            tables.Items.Clear();
            
            string query = "SHOW TABLES FROM " + databases.Text + ";";

            //create command and assign the query and connection from the constructor
            MySqlCommand cmd = new MySqlCommand(query, connection);

            MySqlDataReader Reader = cmd.ExecuteReader();


            while (Reader.Read())
            {
                string row = "";
                for (int i = 0; i < Reader.FieldCount; i++)
                    row += Reader.GetValue(i).ToString();
                tables.Items.Add(row);
            }

            //close Data Reader
            Reader.Close();
        }

        public void GetTableData()
        {
            tableviewer.Columns.Clear();
            tableviewer.Rows.Clear();

            //string query = "SHOW COLUMNS FROM " + databases.Text + "." + tables.Text + ";";

            string query = "SELECT* FROM " + databases.Text + "." + tables.Text + ";";

            MySqlCommand cmd = new MySqlCommand(query, connection);
            MySqlDataReader Reader = cmd.ExecuteReader();


            while (Reader.Read())
            {
                for (int i = 0; i < Reader.FieldCount; i++)
                {
                    if (tableviewer.ColumnCount != Reader.FieldCount)
                    {
                        tableviewer.Columns.Add(Reader.GetName(i).ToString(), Reader.GetName(i).ToString());
                    }
                }

                var dataentry = tableviewer.Rows.Add();
                string row = "";
                for (int i = 0; i < Reader.FieldCount; i++)
                {
                    row = Reader.GetValue(i).ToString();
                    tableviewer.Rows[dataentry].Cells[i].Value = row;

                }
            }

            //close Data Reader
            Reader.Close();

        }

            //open connection to database
            private bool OpenConnection()
        {
            try
            {
                connection.Open();
                return true;
            }
            catch (MySqlException ex)
            {
                //Checking for connection issues or wrong username or pass - todo: check documentation for case of other issues
                switch (ex.Number)
                {
                    case 0:
                        MessageBox.Show("Unable to Connect");
                        break;

                    case 1045:
                        MessageBox.Show("Wrong Usr/Pass");
                        break;
                }
                return false;
            }
        }

        //Close connection
        private bool CloseConnection()
        {
            try
            {
                connection.Close();
                return true;
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        //Insert statement
        public void Insert()
        {
            string query = "INSERT INTO tableinfo (name, age) VALUES('John Smith', '33')";

            //open connection
            if (this.OpenConnection() == true)
            {
                //create command and assign the query and connection from the constructor
                MySqlCommand cmd = new MySqlCommand(query, connection);

                //Execute command
                cmd.ExecuteNonQuery();

                //close connection
                this.CloseConnection();
            }
        }

        //Update statement
        public void Update()
        {
            string query = "UPDATE tableinfo SET name='Joe', age='22' WHERE name='John Smith'";

            //Open connection
            if (this.OpenConnection() == true)
            {
                //create mysql command
                MySqlCommand cmd = new MySqlCommand();
                //Assign the query using CommandText
                cmd.CommandText = query;
                //Assign the connection using Connection
                cmd.Connection = connection;

                //Execute query
                cmd.ExecuteNonQuery();

                //close connection
                this.CloseConnection();
            }
        }

        //Delete statement
        public void Delete()
        {
            string query = "DELETE FROM tableinfo WHERE name='John Smith'";

            if (this.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.ExecuteNonQuery();
                this.CloseConnection();
            }
        }

        //Select statement
        public List<string>[] Select()
        {
            string query = "SELECT * FROM tableinfo";

            //Create a list to store the result
            List<string>[] list = new List<string>[3];
            list[0] = new List<string>();
            list[1] = new List<string>();
            list[2] = new List<string>();

            //Open connection
            if (this.OpenConnection() == true)
            {
                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();

                //Read the data and store them in the list
                while (dataReader.Read())
                {
                    list[0].Add(dataReader["id"] + "");
                    list[1].Add(dataReader["name"] + "");
                    list[2].Add(dataReader["age"] + "");
                }

                //close Data Reader
                dataReader.Close();

                //close Connection
                this.CloseConnection();

                //return list to be displayed
                return list;
            }
            else
            {
                return list;
            }
        }

        //Count statement
        public int Count()
        {
            string query = "SELECT Count(*) FROM tableinfo";
            int Count = -1;

            //Open Connection
            if (this.OpenConnection() == true)
            {
                //Create Mysql Command
                MySqlCommand cmd = new MySqlCommand(query, connection);

                //ExecuteScalar will return one value
                Count = int.Parse(cmd.ExecuteScalar() + "");

                //close Connection
                this.CloseConnection();

                return Count;
            }
            else
            {
                return Count;
            }
        }
    }
}
