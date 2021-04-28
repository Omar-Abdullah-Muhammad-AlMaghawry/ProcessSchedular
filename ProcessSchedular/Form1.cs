using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;
using System.Text;
using System.Collections;
using System.ComponentModel;





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
            if (!textBox1.Text.All(char.IsDigit))
            {
                MessageBox.Show("You have to enter decimal values for Process name ", "Error Entry", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
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
                dataGridView1.Rows[num].Cells[0].Value = item["processName"].ToString().ToUpper();
                dataGridView1.Rows[num].Cells[1].Value = item["arrivingTime"].ToString();
                dataGridView1.Rows[num].Cells[2].Value = item["burstTime"].ToString();
                dataGridView1.Rows[num].Cells[3].Value = item["Priority"].ToString();
            }
            dataGridView1.Sort(new NaturalSortComparer());
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

            int no_processes = dataGridView1.Rows.Count;
            Console.WriteLine(Convert.ToInt32(no_processes));
            int[,] array = new int[no_processes, 7];
            //read from table and put in a private array
            
            for (int i = 0; i < no_processes; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (j == 0)
                        array[i, j] = Convert.ToInt32(dataGridView1.Rows[i].Cells[0].Value);
                    else if (j == 1)
                        array[i, j] = Convert.ToInt32(dataGridView1.Rows[i].Cells[1].Value);
                    else if (j == 2)
                        array[i, j] = Convert.ToInt32(dataGridView1.Rows[i].Cells[2].Value);


                }
            }

            int width = 40, height = 30;
            var graphics = e.Graphics;
            int count = dataGridView1.Rows.Count;
            int sum = 0;
            double totalWaiting = 0;
            dataGridView1.Sort(new NaturalSortComparer());
            for (int i = 0; i < count; i++)
            {
 
                graphics.FillRectangle(new SolidBrush(Color.LightGray), new Rectangle((width * i) + 5, (panel1.Height / 3), width, height));
                graphics.DrawRectangle(new Pen(Color.Gray), new Rectangle((width * i) + 5, (panel1.Height / 3), width, height));
                if (comboBox1.Text == "FCFS")
                {
                    FCFS(width, height, graphics, ref sum, ref totalWaiting, i);

                }
                else if (comboBox1.Text == "Non Preemptive SJF")
                {
                    if (i == 0)
                    {
                        non_premitiveSort(array);

                        graphics.DrawString($"{ array[i, 0].ToString().ToUpper()}", new Font(FontFamily.GenericSansSerif, 13), new SolidBrush(Color.Black), new Point((width * i) + 15, (panel1.Height / 3) + 5));


                        graphics.DrawString($"{array[i, 6]}", new Font(FontFamily.GenericSerif, 8), new SolidBrush(Color.White), new Point((width * i) + 5, (panel1.Height / 3) + height));
                        graphics.DrawString($"{array[i, 3]}", new Font(FontFamily.GenericSerif, 8), new SolidBrush(Color.Black), new Point((width * i) + width - 10, (panel1.Height / 3) + height));


                    }
                    else
                    {
                        graphics.DrawString($"{ array[i, 0].ToString().ToUpper()}", new Font(FontFamily.GenericSansSerif, 13), new SolidBrush(Color.Black), new Point((width * i) + 15, (panel1.Height / 3) + 5));


                        graphics.DrawString($"{array[i, 6]}", new Font(FontFamily.GenericSerif, 8), new SolidBrush(Color.White), new Point((width * i) + 5, (panel1.Height / 3) + height));
                        graphics.DrawString($"{array[i, 3]}", new Font(FontFamily.GenericSerif, 8), new SolidBrush(Color.Black), new Point((width * i) + width - 10, (panel1.Height / 3) + height));
                    }
                }
                else
                {

                    /*graphics.DrawString($"{ dataGridView1[0, i].Value.ToString().ToUpper()}", new Font(FontFamily.GenericSansSerif, 13), new SolidBrush(Color.Black), new Point((width * i) + 15, (panel1.Height / 3) + 5));
                    graphics.DrawString($"{ dataGridView1[1, i].Value.ToString()}", new Font(FontFamily.GenericSerif, 8), new SolidBrush(Color.White), new Point((width * i) + 5, (panel1.Height / 3) + height));*/
                }
            }
            if(totalWaiting > 0)
            {
                totalWaiting /= count;
                textBox5.Text = totalWaiting.ToString();
            }
            

        }

        private void FCFS(int width, int height, Graphics graphics, ref int sum, ref double totalWaiting, int i)
        {
            graphics.DrawString($"{ dataGridView1[0, i].Value.ToString().ToUpper()}", new Font(FontFamily.GenericSansSerif, 13), new SolidBrush(Color.Black), new Point((width * i) + 15, (panel1.Height / 3) + 5));
            int runTime = (int.Parse(dataGridView1[1, i].Value.ToString()) > sum) ? int.Parse(dataGridView1[1, i].Value.ToString()) : sum;
            sum += int.Parse(dataGridView1[2, i].Value.ToString()) + ((int.Parse(dataGridView1[1, i].Value.ToString()) < sum) ? 0 : (int.Parse(dataGridView1[1, i].Value.ToString()) - sum));
            Console.WriteLine(sum);
            totalWaiting += runTime - int.Parse(dataGridView1[1, i].Value.ToString());
            graphics.DrawString($"{runTime}", new Font(FontFamily.GenericSerif, 8), new SolidBrush(Color.White), new Point((width * i) + 5, (panel1.Height / 3) + height));
            graphics.DrawString($"{sum}", new Font(FontFamily.GenericSerif, 8), new SolidBrush(Color.Black), new Point((width * i) + width - 10, (panel1.Height / 3) + height));
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            button1.Enabled = true;
            button2.Enabled = true;
            button3.Enabled = true;
            textBox1.Enabled = true;
            textBox2.Enabled = true;
            textBox3.Enabled = true;
            textBox4.Enabled = true;

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
            textBox1.Enabled = false;
            textBox2.Enabled = false;
            textBox3.Enabled = false;
            textBox4.Enabled = false;
        }

        static void Swap(ref int x, ref int y)
        {
            int tempswap = x;
            x = y;
            y = tempswap;
        }
        private void non_premitiveSort(int[,] array)
        {
            int no_processes = dataGridView1.Rows.Count;

            //sort according to the arrival time
            for (int i = 0; i < no_processes; i++)
            {
                for (int j = 0; j < no_processes - i - 1; j++)
                {
                    if (array[j, 1] > array[j + 1, 1])
                    {
                        for (int k = 0; k < 4; k++)
                        {
                            Swap(ref (array[j, k]), ref (array[j + 1, k]));
                        }
                    }
                }
            }

         
            //sort according to the butst time
            int temp;
            bool flag = false;
            int val = 0;
            for (int i = 0; i < no_processes - 1; i++)
            {
                if (array[i, 1] == array[i + 1, 1] && array[i, 1] == 0)
                {
                    flag = true;
                }
            }
            if (flag == false)
            {
                array[0, 3] = array[0, 1] + array[0, 2];
                for (int i = 1; i < no_processes; i++)
                {

                    temp = array[i - 1, 3];
                    int low = array[i, 2];//brust time
                    for (int j = i; j < no_processes; j++) //to loop rows from 1 to num-1 to find the next Shortest job
                    {
                        if (temp >= array[j, 1] && low >= array[j, 2]) //arrivaltime smaller than completionTime of cuurent process
                        {
                            low = array[j, 2];
                            val = j;
                        }
                    }
                    array[val, 3] = temp + array[val, 2];
                    for (int k = 0; k < 4; k++)
                    {
                        Swap(ref (array[val, k]), ref (array[i, k])); //fuction to swap row if needed
                    }
                }

            }

            else
            {

                for (int i = 0; i < no_processes; i++)
                {
                    if (i == 0)
                    {
                        temp = array[i, 3];
                        decimal low = array[i, 2];//brust time
                        for (int j = i; j < no_processes; j++) //to loop rows from 1 to num-1 to find the next Shortest job
                        {
                            if (temp >= array[j, 1] && low >= array[j, 2]) //arrivaltime smaller than completionTime of cuurent process
                            {
                                low = array[j, 2];
                                val = j;
                            }
                        }
                        array[val, 3] = temp + array[val, 2];
                        for (int k = 0; k < 4; k++)
                        {
                            Swap(ref (array[val, k]), ref (array[i, k])); //fuction to swap row if needed
                        }
                    }


                    else
                    {
                        temp = array[i - 1, 3];
                        decimal low = array[i, 2];//brust time
                        for (int j = i; j < no_processes; j++) //to loop rows from 1 to num-1 to find the next Shortest job
                        {
                            if (temp >= array[j, 1] && low >= array[j, 2]) //arrivaltime smaller than completionTime of cuurent process
                            {
                                low = array[j, 2];
                                val = j;
                            }
                        }
                        array[val, 3] = temp + array[val, 2];
                        for (int k = 0; k < 4; k++)
                        {
                            Swap(ref (array[val, k]), ref (array[i, k])); //fuction to swap row if needed
                        }
                    }
                }
            }
            //put the coloum 4 for the turnaround
            for (int i = 0; i < no_processes; i++)
            {
                array[i, 4] = array[i, 3] - array[i, 1];
            }
            //put coloumn 5 for the waiting time
            for (int i = 0; i < no_processes; i++)
            {
                array[i, 5] = array[i, 4] - array[i, 2];
            }
            //put coloumn 6 for the start time
            for (int i = 0; i < no_processes; i++)
            {
                array[i, 6] = array[i, 3] - array[i, 2];
            }

            //calculate total waiting time
            decimal waiting_time = 0;
            for (int i = 0; i < no_processes; i++)
            {
                waiting_time = waiting_time + array[i, 5];
            }

            for (int i = 0; i < no_processes; i++)
            {
                for (int j = 0; j < 6; j++)
                    Console.WriteLine(array[i, j]);
            }


            float average_waiting_time = (float)waiting_time / no_processes;
            Console.WriteLine(average_waiting_time);
            textBox5.Text = Convert.ToString(average_waiting_time);
        }



    }

}
