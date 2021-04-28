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
        private DataTable process = null;
        Graphics pObject = null;
        static Brush blue = new SolidBrush(Color.Blue);
        static Pen bluePen = new Pen(blue, 4);
        static Brush black = new SolidBrush(Color.Black);
        static Brush White = new SolidBrush(Color.White);
        static Pen blackPen = new Pen(black, 3);
        static Pen whitePen = new Pen(White, 5);
        // Font font = new Font(Font.FontFamily, 4);
        static float startX = 0, startY = 0;
        static float height = 0;
        static string nameOfProcess = "";
        float inc = 0;
        float width = 0;
        PaintEventArgs e;
        object sender;
        public Form1()
        {
            InitializeComponent();
            startX = 14;
            startY = 14;
            height = 25;
        }

        private bool isChanged(float w)
        {
            bool x;
            float oldValue = 0;
            if (w != oldValue)
            {
                x = true;

            }
            else
                x = false;

            oldValue = w;
            return x;
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            float arr = 0, bur = 0, pri = 0,Q=0;
            process = new DataTable();

            process.Columns.Add("processName", typeof(string));
            process.Columns.Add("arrivingTime", typeof(string));
            process.Columns.Add("burstTime", typeof(string));
            process.Columns.Add("Priority", typeof(string));

            process.Columns.Add("waitingTime", typeof(string));
            process.Columns.Add("turnaroundTime", typeof(string));
            bool canConvertArr = float.TryParse(textBox2.Text, out arr);
            bool canConvertBru = float.TryParse(textBox3.Text, out bur);
            bool canConvertPri = float.TryParse(textBox4.Text, out pri);
            bool canConvertQ = float.TryParse(textBox5.Text, out Q );
            if (comboBox1.Text == "Preemptive Priority" || comboBox1.Text == "Non Preemptive Priority")
            {
                if (canConvertArr == true && canConvertBru == true && canConvertPri == true)
                    process.Rows.Add(textBox1.Text, float.Parse(textBox2.Text), float.Parse(textBox3.Text), float.Parse(textBox4.Text));
                else
                    MessageBox.Show("You have to enter decimal values for Arriving , Burst Time ,Priority", "Error Entry", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (comboBox1.Text == "Round Robin")
            {
                if (canConvertArr == true && canConvertBru == true && canConvertQ == true)
                    process.Rows.Add(textBox1.Text, float.Parse(textBox2.Text), float.Parse(textBox3.Text), 0);
                else
                    MessageBox.Show("You have to enter decimal values for Arriving , Burst Time ,Quantum", "Error Entry", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
           else {
                if (canConvertArr == true && canConvertBru == true)
                    process.Rows.Add(textBox1.Text, float.Parse(textBox2.Text), float.Parse(textBox3.Text),0);
                else
                    MessageBox.Show("You have to enter decimal values for Arriving , Burst Time", "Error Entry", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void button2_Click_1(object sender, EventArgs e)
        {


            /*  for (int i = process.Rows.Count - 1; i >= 0; i--)
              {
                  DataRow delRow = process.Rows[i];
                  if (delRow["processName"].ToString()==textBox1.Text && delRow["arrivingTime"].ToString() == textBox2.Text && delRow["burstTime"].ToString() == textBox3.Text && delRow["Priority"].ToString() == textBox4.Text) {
                      delRow.Delete();
                      dataGridView1.Rows.RemoveAt(i);
                  }
              }*/
            foreach (DataGridViewRow delRow in dataGridView1.SelectedRows)
            {
                //DataRow delRowTable = process.Rows[delRow.Index];
                //process.Rows[delRow.Index].Delete();
                dataGridView1.Rows.RemoveAt(delRow.Index);
            }

        }
        private void pSort()
        {

            float burstTimeSum = 0;
            float averageWaitingTime = 0;
            float totalNoOfProcess = dataGridView1.Rows.Count;
            var arrDataOr = new List<List<string>> { };
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
                arrDataOr.Insert(i, new List<string> { (string)dataGridView1.Rows[i].Cells[0].Value, (string)dataGridView1.Rows[i].Cells[1].Value, (string)dataGridView1.Rows[i].Cells[2].Value, (string)dataGridView1.Rows[i].Cells[3].Value, i.ToString(), });
            var arrData = new List<List<string>> { };
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                int noOfProcess = 0;

                //who is arriving time <burstTime
                for (int j = 0; j < arrDataOr.Count; j++)
                    if (float.Parse((string)arrDataOr[j][1]) <= burstTimeSum)
                    {

                        arrData.Insert(noOfProcess, new List<string> { (string)arrDataOr[j][0], (string)arrDataOr[j][1], (string)arrDataOr[j][2], (string)arrDataOr[j][3], (string)arrDataOr[j][4] });
                        //              arrData[noOfProcess]=dataGridView1.Rows[j];

                        noOfProcess += 1;
                    }
                //  if (j == arrDataOr.Count) j--;

                if (noOfProcess == 1)
                {
                    // TO DO ... show the process
                    width = float.Parse(arrData[noOfProcess - 1][2]) * 4;
                    nameOfProcess = arrData[noOfProcess - 1][0];
                    dataGridView2_Paint(sender, e);
                    dataGridView2.Update();
                    averageWaitingTime += (burstTimeSum - float.Parse((string)arrData[noOfProcess - 1][1])) / totalNoOfProcess;
                    burstTimeSum += float.Parse((string)arrData[noOfProcess - 1][2]);

                    int delIndex = int.Parse(arrData[noOfProcess - 1][4]);
                    arrDataOr.RemoveAt(delIndex);
                    for (int c = delIndex; c < arrDataOr.Count; c++)
                    {
                        arrDataOr[c][4] = (int.Parse(arrDataOr[c][4]) - 1).ToString();
                    }
                }
                else if (noOfProcess > 1)
                {
                    //who is most priority by less num
                    float min = float.Parse((string)arrData[0][3]);
                    List<string> minRow = arrData[0];
                    for (int k = 0; k < noOfProcess; k++)
                    {
                        if (float.Parse(arrData[k][3]) < min)
                        {
                            min = float.Parse((string)arrData[k][3]);
                            minRow = arrData[k];
                        }
                    }
                    // TO DO .. Show the minRow 
                    width = float.Parse((string)minRow[2]) * 4;
                    nameOfProcess = minRow[0];
                    dataGridView2_Paint(sender, e);
                    dataGridView2.Update();
                    averageWaitingTime += (burstTimeSum - float.Parse((string)minRow[1])) / totalNoOfProcess;
                    burstTimeSum += float.Parse((string)minRow[2]);

                    int delIndex = int.Parse(minRow[4]);
                    arrDataOr.RemoveAt(delIndex);
                    for (int c = delIndex; c < arrDataOr.Count; c++)
                    {
                        arrDataOr[c][4] = (int.Parse(arrDataOr[c][4]) - 1).ToString();
                    }
                }

            }

            textBox6.Text = (averageWaitingTime).ToString();







        }

        //  for (int i = 0; i < process.Rows.Count ; i++)
        // {
        // if(process.Rows[i]["arrivingTime"]==0)
        //  DataRow temp = process.Rows[0]["Priority"];
        //   process.Rows[0]["Priority"]=process.Rows[i]["Priority"];
        //    process.Rows[i]["Priority"]=temp;           
        //     DataRow topRow = process.Rows[i];
        //    for (int j = i; j < process.Rows.Count ; j++)
        //   {
        // DataRow roundRow = process.Rows[j];
        //     if (process.Rows[j]["Priority"]<process.Rows[i]["Priority"] && process.Rows[i]["arrivingTime"] != 0&&process.Rows[j]["arrivingTime"] != 0) {
        //           DataRow temp = process.Rows[i]["Priority"];
        //         process.Rows[i]["Priority"]=process.Rows[j]["Priority"];
        //       process.Rows[j]["Priority"]=temp;
        //}
        // }
        // }

        /* private void roundrobin1(DataTable processes, int quantum)
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
                             //Console.WriteLine(process["processName"] + " " + Convert.ToString(time) + " " + Convert.ToString(time + Convert.ToInt32(process["burstTime"])));
                             width = float.Parse(process["burstTime"].ToString()) * 4;
                             nameOfProcess = process["processName"].ToString();
                             dataGridView2_Paint(sender, e);
                             dataGridView2.Update();
                             time += Convert.ToInt32(process["burstTime"]);
                             process["waitingTime"] = Convert.ToString(Convert.ToInt32(process["waitingTime"]) - Convert.ToInt32(process["burstTime"]));
                             process["burstTime"] = Convert.ToString(Convert.ToInt32(process["waitingTime"]) * -1);
                             process["turnaroundTime"] = Convert.ToString(time - Convert.ToInt32(process["arrivingTime"]));
                             process["waitingTime"] = Convert.ToString(Convert.ToInt32(process["turnaroundTime"]) + Convert.ToInt32(process["waitingTime"]));
                             done += 1;
                             madeProgress = true;
                         }
                         else
                         {
                             //  Console.WriteLine(process["processName"] + " " + Convert.ToString(time) + " " + Convert.ToString(time + quantum));
                             width = float.Parse(quantum.ToString()) * 4;
                             nameOfProcess = process["processName"].ToString();
                             dataGridView2_Paint(sender, e);
                             dataGridView2.Update();
                             time += quantum;
                             process["burstTime"] = Convert.ToString(Convert.ToInt32(process["burstTime"]) - quantum);
                             process["waitingTime"] = Convert.ToString(Convert.ToInt32(process["waitingTime"]) - quantum);
                             madeProgress = true;
                         }
                     }
                 }
                 time = (madeProgress) ? time : time + quantum;
             }
         }*/
        private void roundrobin(DataGridView processes, int quantum)
        {
            //TODO implement return of average waiting and turnaround times
            int time = 0;
            int done = 0;
            int processes_no = processes.Rows.Count;
            bool madeProgress = false;
            var waitingTime = new string[processes_no];
            for (int i = 0; i < processes_no; i++)
            {
                waitingTime[i] = (0).ToString();
            }
            while (done != processes_no)
            {
                madeProgress = false;
                for (int i = 0; i < processes_no; i++)
                {
                    DataGridViewRow process = processes.Rows[i];
                    /*
                     * 0 -> process name
                     * 1 -> arrival time
                     * 2 -> burst time
                     * 3 -> priority
					 * 4 -> waiting time
                    */

                    if (int.Parse(waitingTime[i]) > 0)
                    {
                        madeProgress = true;
                        continue;
                    }
                    if (Convert.ToInt32(process.Cells[1].Value) <= time && Convert.ToInt32(process.Cells[2].Value) > 0)
                    {
                        if (Convert.ToInt32(process.Cells[2].Value) <= quantum)
                        {
                            //Console.WriteLine(process["processName"] + " " + Convert.ToString(time) + " " + Convert.ToString(time + Convert.ToInt32(process["burstTime"])));
                            /*  if(quantum<10)
                                   width = float.Parse(process.Cells[2].Value.ToString()) * 4;
                              else*/
                            width = float.Parse(process.Cells[2].Value.ToString()) * 4;
                            nameOfProcess = process.Cells[0].Value.ToString();
                            dataGridView2_Paint(sender, e);
                            dataGridView2.Update();
                            time += Convert.ToInt32(process.Cells[2].Value);
                            waitingTime[i] = Convert.ToString(Convert.ToInt32(waitingTime[i]) - Convert.ToInt32(process.Cells[2].Value));
                            process.Cells[2].Value = Convert.ToString(Convert.ToInt32(waitingTime[i]) * -1);
                            //process["turnaroundTime"] = Convert.ToString(time - Convert.ToInt32(process["arrivingTime"]));
                            waitingTime[i] = Convert.ToString(time - Convert.ToInt32(process.Cells[1].Value) + Convert.ToInt32(waitingTime[i]));
                            done += 1;
                            madeProgress = true;
                        }
                        else
                        {
                            //  Console.WriteLine(process["processName"] + " " + Convert.ToString(time) + " " + Convert.ToString(time + quantum));
                            /*          if (quantum < 10)
                                          width = float.Parse(process.Cells[2].Value.ToString()) * 4;
                                      else*/
                            width = float.Parse(quantum.ToString()) * 4;
                            nameOfProcess = process.Cells[0].Value.ToString();
                            dataGridView2_Paint(sender, e);
                            dataGridView2.Update();
                            time += quantum;
                            process.Cells[2].Value = Convert.ToString(Convert.ToInt32(process.Cells[2].Value) - quantum);
                            waitingTime[i] = Convert.ToString(Convert.ToInt32(waitingTime[i]) - quantum);
                            madeProgress = true;
                        }
                    }
                }
                time = (madeProgress) ? time : time + quantum;
            }

            int total_waiting_time = 0;

            for (int i = 0; i < processes_no; i++)
            {
                DataGridViewRow process = processes.Rows[i];
                total_waiting_time += Convert.ToInt32(waitingTime[i]);
            }

            textBox6.Text = (total_waiting_time / processes_no).ToString();
        }
        private void FCFS()
        {
            int count = dataGridView1.Rows.Count;
            int sum = 0;
            double totalWaiting = 0;
            dataGridView1.Sort(new NaturalSortComparer());
            for (int i = 0; i < count; i++)
            {
                //graphics.DrawString($"{ dataGridView1[0, i].Value.ToString().ToUpper()}", new Font(FontFamily.GenericSansSerif, 13), new SolidBrush(Color.Black), new Point((width * i) + 15, (panel1.Height / 3) + 5));
                int runTime = (int.Parse(dataGridView1[1, i].Value.ToString()) > sum) ? int.Parse(dataGridView1[1, i].Value.ToString()) : sum;
                sum += int.Parse(dataGridView1[2, i].Value.ToString()) + ((int.Parse(dataGridView1[1, i].Value.ToString()) < sum) ? 0 : (int.Parse(dataGridView1[1, i].Value.ToString()) - sum));
                width = float.Parse(dataGridView1[2, i].Value.ToString()) * 4;
                nameOfProcess = dataGridView1[0, i].Value.ToString();
                dataGridView2_Paint(sender, e);
                dataGridView2.Update();
                //  Console.WriteLine(sum);
                totalWaiting += runTime - int.Parse(dataGridView1[1, i].Value.ToString());
            }
            if (totalWaiting > 0)
            {
                totalWaiting /= count;
                textBox6.Text = totalWaiting.ToString();
            }
            //   graphics.DrawString($"{runTime}", new Font(FontFamily.GenericSerif, 8), new SolidBrush(Color.White), new Point((width * i) + 5, (panel1.Height / 3) + height));
            //  graphics.DrawString($"{sum}", new Font(FontFamily.GenericSerif, 8), new SolidBrush(Color.Black), new Point((width * i) + width - 10, (panel1.Height / 3) + height));
        }
        static void Swap(ref string x, ref string y)
        {
            string tempswap = x;
            x = y;
            y = tempswap;
        }
        private void non_premitiveSort()
        {
            int no_processes = dataGridView1.Rows.Count;
            string[,] array = new string[no_processes, 7];
            //read from table and put in a private array

            for (int i = 0; i < no_processes; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (j == 0)
                        array[i, j] = dataGridView1.Rows[i].Cells[0].Value.ToString();
                    else if (j == 1)
                        array[i, j] = dataGridView1.Rows[i].Cells[1].Value.ToString();
                    else if (j == 2)
                        array[i, j] = dataGridView1.Rows[i].Cells[2].Value.ToString();


                }
            }
           // int no_processes = dataGridView1.Rows.Count;

            //sort according to the arrival time
            for (int i = 0; i < no_processes; i++)
            {
                for (int j = 0; j < no_processes - i - 1; j++)
                {
                    if (float.Parse(array[j, 1]) > float.Parse(array[j + 1, 1]))
                    {
                        for (int k = 0; k < 4; k++)
                        {
                            Swap(ref (array[j, k]), ref (array[j + 1, k]));
                        }
                    }
                }
            }


            //sort according to the butst time
            float temp;
            bool flag = false;
            int val = 0;
            for (int i = 0; i < no_processes - 1; i++)
            {
                if (float.Parse(array[i, 1]) == float.Parse(array[i + 1, 1]) && float.Parse(array[i, 1]) == 0)
                {
                    flag = true;
                }
            }
            if (flag == false)
            {
                array[0, 3] = (float.Parse(array[0, 1]) + float.Parse(array[0, 2])).ToString();
                for (int i = 1; i < no_processes; i++)
                {

                    temp = float.Parse(array[i - 1, 3]);
                    float low = float.Parse(array[i, 2]);//brust time

                    for (int j = i; j < no_processes; j++) //to loop rows from 1 to num-1 to find the next Shortest job
                    {
                        if (temp >= float.Parse(array[j, 1]) && low >= float.Parse(array[j, 2])) //arrivaltime smaller than completionTime of cuurent process
                        {
                            low = float.Parse(array[j, 2]);
                            val = j;
                        }
                    }
                    array[val, 3] =( temp + float.Parse(array[val, 2])).ToString();
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
                        temp = float.Parse(array[i, 3]);
                        float low = float.Parse(array[i, 2]);//brust time
                        for (int j = i; j < no_processes; j++) //to loop rows from 1 to num-1 to find the next Shortest job
                        {
                            if (temp >= float.Parse(array[j, 1] )&& low >= float.Parse(array[j, 2])) //arrivaltime smaller than completionTime of cuurent process
                            {
                                low = float.Parse(array[j, 2]);
                                val = j;
                            }
                        }
                        array[val, 3] = (temp + float.Parse(array[val, 2])).ToString();
                        for (int k = 0; k < 4; k++)
                        {
                            Swap(ref (array[val, k]), ref (array[i, k])); //fuction to swap row if needed
                        }
                    }


                    else
                    {
                        temp = float.Parse(array[i - 1, 3]);
                        float low = float.Parse(array[i, 2]);//brust time
                        for (int j = i; j < no_processes; j++) //to loop rows from 1 to num-1 to find the next Shortest job
                        {
                            if (temp >= float.Parse(array[j, 1]) && low >= float.Parse(array[j, 2])) //arrivaltime smaller than completionTime of cuurent process
                            {
                                low = float.Parse(array[j, 2]);
                                val = j;
                            }
                        }
                        array[val, 3] = (temp + float.Parse(array[val, 2])).ToString();
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
                array[i, 4] = (float.Parse(array[i, 3]) - float.Parse(array[i, 1])).ToString();
            }
            //put coloumn 5 for the waiting time
            for (int i = 0; i < no_processes; i++)
            {
                array[i, 5] = (float.Parse(array[i, 4]) - float.Parse(array[i, 2])).ToString();
            }
            //put coloumn 6 for the start time
            for (int i = 0; i < no_processes; i++)
            {
                array[i, 6] =(float.Parse(array[i, 3]) - float.Parse(array[i, 2])).ToString();
            }

            //calculate total waiting time
            float waiting_time = 0;
            for (int i = 0; i < no_processes; i++)
            {
                waiting_time = waiting_time + float.Parse(array[i, 5]);
            }

            for (int i = 0; i < no_processes; i++)
            {
                for (int j = 0; j < 6; j++)
                    Console.WriteLine(array[i, j]);
            }


            float average_waiting_time = (float)waiting_time / no_processes;
            Console.WriteLine(average_waiting_time);
            for (int i = 0; i < no_processes; i++)
            {
                width = float.Parse(array[i , 2] )* 4;
                nameOfProcess = array[i, 0];
                dataGridView2_Paint(sender, e);
                dataGridView2.Update();
            }

                textBox6.Text = Convert.ToString(average_waiting_time);
        }

        
    
  

    private void button3_Click_1(object sender, EventArgs e)
        {
            startX = 15;
            inc = 0;
            width = 0;
          //  pObject.Clear(Color.Gray);//
            dataGridView2.Refresh();//

            if (comboBox1.Text == "Non Preemptive Priority")
                pSort();
            else if (comboBox1.Text == "Round Robin")
                roundrobin(dataGridView1, int.Parse(textBox5.Text));
            else if (comboBox1.Text == "FCFS")
                FCFS();
            else if (comboBox1.Text == "Non Preemptive SJF")
                non_premitiveSort();

            // dataGridView2.Update();


        }

        private void drawRect(float w,string name)
        {
            Font fontb = new Font(Font.FontFamily, 6);
            Font fontw = new Font(Font.FontFamily, 8);
            pObject.DrawRectangle(blackPen, startX + inc, startY, w, height);
            pObject.FillRectangle(blue, startX + inc, startY, w, height);
           // if (inc != 0)
            pObject.DrawString((inc/4).ToString(), fontb, black, startX + inc-3 , startY + (float)26);
            pObject.DrawString(name, fontw, White, (startX + inc-4) + (width/2) , startY + height/2);
            inc += w;
            if (inc != 0)
            {
               
                pObject.DrawString((inc / 4).ToString(),fontb, black, startX + inc-3, startY + (float)26);

            }
        }

      




      
        private void Form1_Load(object sender, EventArgs e)
        {
            textBox1.Enabled = false;
            textBox2.Enabled = false;
            textBox3.Enabled = false;
            textBox4.Enabled = false;
            textBox5.Enabled = false;
            button3.Enabled = false;
        }

        private void comboBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            textBox1.Enabled = true;
            textBox2.Enabled = true;
            textBox3.Enabled = true;
            textBox4.Enabled = true;
        //  textBoxEnabled = false;


            if (comboBox1.Text == "")
            {
                button3.Enabled = false;
            }
            else if (comboBox1.Text == "Preemptive Priority" || comboBox1.Text == "Non Preemptive Priority")
            {
                textBox4.Enabled = true;
                textBox4.ReadOnly = false;
                button3.Enabled = true;
                textBox5.Enabled = false;
                textBox5.ReadOnly = true;
            }
            else if (comboBox1.Text == "Round Robin")
            {
                textBox4.Enabled = false;
                textBox4.ReadOnly = true;
                textBox5.Enabled = true;
                textBox5.ReadOnly = false;
                textBox5.Text = "";
                button3.Enabled = true;
            }
            else
            {
                button3.Enabled = true;
                textBox4.Enabled = false;
                textBox4.ReadOnly = true;
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    dataGridView1.Rows[i].Cells[3].Value = "";
                }
                textBox4.Text = "";
                textBox5.Enabled = false;
                textBox5.ReadOnly = true;
                textBox5.Text = "";

            }

        }

        private void dataGridView2_Paint(object sender, PaintEventArgs e)
        {

            float w = 0;
            pObject = dataGridView2.CreateGraphics();

            drawRect(width,nameOfProcess);
              
           
            w = width;
        }
    }
}
