using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace test00
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private DataTable process ;

        private void button1_Click(object sender, EventArgs e)
        {

            process = new DataTable();
           
            process.Columns.Add("processName", typeof(string));
            process.Columns.Add("arrivingTime", typeof(float));
            process.Columns.Add("brustTime", typeof(float));
            DataRow newRow = process.NewRow();

            process.Rows.Add(textBox1.Text, textBox2.Text, textBox3.Text);
            foreach (DataRow item in process.Rows)
            {
                int num = dataGridView1.Rows.Add();
                dataGridView1.Rows[num].Cells[0].Value = item["processName"].ToString();
                dataGridView1.Rows[num].Cells[1].Value = item["arrivingTime"].ToString();
                dataGridView1.Rows[num].Cells[2].Value = item["brustTime"].ToString();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
            for (int i = process.Rows.Count - 1; i >= 0; i--)
            {
                DataRow delRow = process.Rows[i];
                if (delRow["processName"].ToString()==textBox1.Text && delRow["arrivingTime"].ToString() == textBox2.Text && delRow["brustTime"].ToString() == textBox3.Text) {
                    delRow.Delete();
                    dataGridView1.Rows.RemoveAt(i);
                }
            }
            foreach(DataGridViewRow delRow in dataGridView1.SelectedRows)
            {
                DataRow delRowTable = process.Rows[delRow.Index];
                delRowTable.Delete();
                dataGridView1.Rows.RemoveAt(delRow.Index);
            }
            
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }
    }
}
