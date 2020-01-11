using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace dbAdmin
{ 
    public partial class Form1 : Form
    {
        private DBConnect dbConnect;

        public Form1()
        {
            InitializeComponent();
        }

        private void cmdConnect_Click(object sender, EventArgs e)
        {
            dbConnect = new DBConnect();

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void cbxDatabase_SelectedIndexChanged(object sender, EventArgs e)
        {
            dbConnect.DBSelect();
        }

        private void cbxTable_SelectedIndexChanged(object sender, EventArgs e)
        {
            dbConnect.GetTableData();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }

}
