using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace test00
{






    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
        }

        private DataTable process;

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
            if (comboBox1.Text != "Preemptive Priority" && comboBox1.Text != "Non Preemptive Priority")
            {
                if (canConvertArr == true && canConvertBru == true)
                    process.Rows.Add(textBox1.Text, textBox2.Text, textBox3.Text, textBox4.Text);
                else
                    MessageBox.Show("You have to enter decimal values for Arriving , Burst Time ", "Error Entry", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            else
            {
                if (canConvertArr == true && canConvertBru == true && canConvertPri == true)
                    process.Rows.Add(textBox1.Text, textBox2.Text, textBox3.Text, textBox4.Text);
                else
                    MessageBox.Show("You have to enter decimal values for Arriving , Burst Time ,Priority", "Error Entry", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


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

            if (dataGridView1.Rows.Count == 0) return;
            for (int i = process.Rows.Count - 1; i >= 0; i--)
            {
                DataRow delRow = process.Rows[i];
                if (delRow["processName"].ToString() == textBox1.Text && delRow["arrivingTime"].ToString() == textBox2.Text && delRow["burstTime"].ToString() == textBox3.Text && delRow["Priority"].ToString() == textBox4.Text)
                {
                    delRow.Delete();
                    dataGridView1.Rows.RemoveAt(i);
                }
            }
            foreach (DataGridViewRow delRow in dataGridView1.SelectedRows)
            {
                //DataRow delRowTable = process.Rows[delRow.Index];
                //process.Rows[delRow.Index].Delete();
                dataGridView1.Rows.RemoveAt(delRow.Index);
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count <= 0) return;

            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (comboBox1.Text != "Preemptive Priority" && comboBox1.Text != "Non Preemptive Priority")
                {
                    if (dataGridView1[0, i].Value == null || dataGridView1[1, i].Value == null || dataGridView1[2, i].Value == null)
                    {
                        
                        MessageBox.Show("Please input Valid Data Values");
                        return;
                    }
                    
                }
                else
                {
                    if (dataGridView1[0, i].Value == null || dataGridView1[1, i].Value == null || dataGridView1[2, i].Value == null || dataGridView1[3, i].Value == null)
                    {
                        
                        MessageBox.Show("Please input Valid Data Values");
                        return;
                    }
                    
                }

            }
            panel1.Refresh();




        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            int width = 40, height = 30;
            var graphics = e.Graphics;
            int count = dataGridView1.Rows.Count;


            for (int i = 0; i < count; i++)
            {

                graphics.FillRectangle(new SolidBrush(Color.LightGray), new Rectangle((width * i) + 5, (panel1.Height / 3), width, height));
                graphics.DrawRectangle(new Pen(Color.Gray), new Rectangle((width * i) + 5, (panel1.Height / 3), width, height));
                graphics.DrawString($"{ dataGridView1[0, i].Value.ToString().ToUpper()}", new Font(FontFamily.GenericSansSerif, 13), new SolidBrush(Color.Black), new Point((width * i) + 15, (panel1.Height / 3) + 5));
                graphics.DrawString($"{ dataGridView1[1, i].Value.ToString()}", new Font(FontFamily.GenericSerif, 8), new SolidBrush(Color.White), new Point((width * i) + 5, (panel1.Height / 3) + height));

            }

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            button1.Enabled = true;
            button2.Enabled = true;
            button3.Enabled = true;

            if (comboBox1.Text != "Preemptive Priority" && comboBox1.Text != "Non Preemptive Priority")
                {
                    textBox4.Enabled = false;
                    dataGridView1.Columns[3].ReadOnly = true;

                    for (int i = 0; i < dataGridView1.Rows.Count; i++)
                        dataGridView1.Rows[i].Cells[3].Value = "";
                    textBox4.Text = "";
                }
                else
                {
                    textBox4.Enabled = true;
                    dataGridView1.Columns[3].ReadOnly = false;
                }
            
            
           
        }

        private void comboBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            button1.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = false;
        }
    }
}
