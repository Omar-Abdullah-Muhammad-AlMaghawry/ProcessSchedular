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
            process.Columns.Add("waitingTime", typeof(string));
            process.Columns.Add("turnaroundTime", typeof(string));
            bool canConvertArr = float.TryParse(textBox2.Text, out arr);
            bool canConvertBru = float.TryParse(textBox3.Text, out bru);
            bool canConvertPri = float.TryParse(textBox4.Text, out pri);
            if (canConvertArr == true && canConvertBru == true && canConvertPri == true)
                process.Rows.Add(textBox1.Text, textBox2.Text, textBox3.Text, textBox4.Text, "0", "0");
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

        static void roundrobin(DataTable processes, int quantum)
        {
            //TODO implement return of average waiting and turnaround times
            int time = 0;
            int done = 0;
            int processes_no = processes.Rows.Count;
            bool madeProgress = false;

            while (done != processes_no)
            {
                madeProgress = false;
                for (int i = 0; i < processes_no; i++)
                {
                    DataRow process = processes.Rows[i];

                    if (Convert.ToInt32(process["arrivingTime"]) <= time && Convert.ToInt32(process["burstTime"]) > 0)
                    {
                        if (Convert.ToInt32(process["burstTime"]) <= quantum)
                        {
                            Console.WriteLine(process["processName"] + " " + Convert.ToString(time) + " " + Convert.ToString(time + Convert.ToInt32(process["burstTime"])));
                            time += Convert.ToInt32(process["burstTime"]);
                            process["burstTime"] ="0";
                            process["turnaroundTime"] = Convert.ToString(time - Convert.ToInt32(process["arrivingTime"]));
                            process["waitingTime"] = Convert.ToString(Convert.ToInt32(process["turnaroundTime"]) + Convert.ToInt32(process["waitingTime"]));
                            done += 1;
                            madeProgress = true;
                        }
                        else
                        {
                            Console.WriteLine(process["processName"] + " " + Convert.ToString(time) + " " + Convert.ToString(time + quantum));
                            time += quantum;
                            process["burstTime"] = Convert.ToString(Convert.ToInt32(process["burstTime"]) - quantum);
                            process["waitingTime"] = Convert.ToString(Convert.ToInt32(process["waitingTime"]) - quantum);
                            madeProgress = true;
                        }
                    }
                }
                time = (madeProgress) ? time : time + quantum;
            }
        }
    }
}
