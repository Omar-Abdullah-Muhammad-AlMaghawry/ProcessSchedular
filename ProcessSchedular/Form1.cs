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
            float arr = 0, bru = 0, pri = 0; 
            process = new DataTable();
           
            process.Columns.Add("processName", typeof(string));
            process.Columns.Add("arrivingTime", typeof(string));
            process.Columns.Add("burstTime", typeof(string));
            process.Columns.Add("Priority", typeof(string));
            bool canConvertArr = float.TryParse(textBox2.Text, out arr);
            bool canConvertBru = float.TryParse(textBox3.Text, out bru);
            bool canConvertPri = float.TryParse(textBox4.Text, out pri);
            if (canConvertArr == true && canConvertBru == true && canConvertPri == true)
                process.Rows.Add(textBox1.Text, textBox2.Text, textBox3.Text, textBox4.Text);
            else
                MessageBox.Show("You have to enter decimal values for Arriving , Burst Time ,Priority","Error Entry",MessageBoxButtons.OK,MessageBoxIcon.Error);

            DataRow newRow = process.NewRow();
           
            
            foreach (DataRow item in process.Rows)
            {
                int num = dataGridView1.Rows.Add();
                dataGridView1.Rows[num].Cells[0].Value = item["processName"].ToString();
                dataGridView1.Rows[num].Cells[1].Value = item["arrivingTime"].ToString();
                dataGridView1.Rows[num].Cells[2].Value = item["burstTime"].ToString();
                dataGridView1.Rows[num].Cells[3].Value = item["Priority"].ToString();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
            
            for (int i = process.Rows.Count - 1; i >= 0; i--)
            {
                DataRow delRow = process.Rows[i];
                if (delRow["processName"].ToString()==textBox1.Text && delRow["arrivingTime"].ToString() == textBox2.Text && delRow["burstTime"].ToString() == textBox3.Text && delRow["Priority"].ToString() == textBox4.Text) {
                    delRow.Delete();
                    dataGridView1.Rows.RemoveAt(i);
                }
            }
            foreach(DataGridViewRow delRow in dataGridView1.SelectedRows)
            {
                //DataRow delRowTable = process.Rows[delRow.Index];
                //process.Rows[delRow.Index].Delete();
                dataGridView1.Rows.RemoveAt(delRow.Index);
            }
            
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }
    }
}
