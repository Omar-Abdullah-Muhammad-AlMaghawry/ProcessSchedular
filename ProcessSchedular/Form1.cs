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
        private DataTable process =null;
        Graphics pObject = null;
        static  Brush blue = new SolidBrush(Color.Blue);
        static Pen bluePen = new Pen(blue, 4);
        static Brush black = new SolidBrush(Color.Black);
        static Brush White = new SolidBrush(Color.White);
        static Pen blackPen = new Pen(black, 3);
        static Pen whitePen = new Pen(White, 5);
        // Font font = new Font(Font.FontFamily, 4);
        static float startX = 0 , startY =0;
        static float height = 0 ;
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

        private bool isChanged(float w )
        {
            bool x;
            float oldValue = 0;
            if (w != oldValue)
            {
                x = true;
                
            }
            else
                x= false;

            oldValue = w;
            return x;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            float arr = 0, bur = 0, pri = 0; 
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
            if (canConvertArr == true && canConvertBru == true && canConvertPri == true)
                process.Rows.Add(textBox1.Text, float.Parse (textBox2.Text), float.Parse(textBox3.Text), float.Parse(textBox4.Text),"0","0");
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
        private void pSort()
        {
            label6.Text = dataGridView1.Rows.Count.ToString();
           float burstTimeSum = 0;
          
            var arrDataOr = new List<List<string>> { };
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
                arrDataOr.Insert(i, new List<string> {(string)dataGridView1.Rows[i].Cells[0].Value, (string)dataGridView1.Rows[i].Cells[1].Value, (string)dataGridView1.Rows[i].Cells[2].Value, (string)dataGridView1.Rows[i].Cells[3].Value, i.ToString(), });
            var arrData = new List<List<string>> { };
            for (int i = 0 ; i< dataGridView1.Rows.Count; i++) { 
            int noOfProcess =0;
                
                //who is arriving time <burstTime
                for (int j = 0; j < arrDataOr.Count; j++)
                if(float.Parse((string)arrDataOr[j][1]) <=burstTimeSum)
                {

                        arrData.Insert(noOfProcess,new List<string> { (string)arrDataOr[j][0], (string)arrDataOr[j][1], (string)arrDataOr[j][2], (string)arrDataOr[j][3], (string)arrDataOr[j][4] });
          //              arrData[noOfProcess]=dataGridView1.Rows[j];

                    noOfProcess+=1;
                }
              //  if (j == arrDataOr.Count) j--;
                   
            if(noOfProcess == 1)
            {
                    // TO DO ... show the process
                    width = float.Parse(arrData[noOfProcess - 1][2])*4;
                    nameOfProcess = arrData[noOfProcess - 1][0];
                    dataGridView2_Paint( sender,  e);
                    dataGridView2.Update();
                    burstTimeSum += float.Parse((string)arrData[noOfProcess - 1][2]);
                    
                    int delIndex = int.Parse(arrData[noOfProcess - 1][4]);
                    arrDataOr.RemoveAt(delIndex);
                    for(int c = delIndex; c < arrDataOr.Count; c++)
                    {
                        arrDataOr[c][4]= (int.Parse(arrDataOr[c][4])-1).ToString();
                    }
            }
            else if (noOfProcess > 1)
            {
                //who is most priority by less num
                float min = float.Parse((string)arrData[0][3]);
                List<string> minRow = arrData[0];
                for (int k = 0 ; k<noOfProcess;k++)
                {
                    if(float.Parse(arrData[k][3])<min)
                    {   min = float.Parse((string)arrData[k][3]);
                        minRow=arrData[k] ;   
                    }
                }
                    // TO DO .. Show the minRow 
                    width = float.Parse((string)minRow[2])*4;
                    nameOfProcess = minRow[0];
                    dataGridView2_Paint(sender, e);
                    dataGridView2.Update();
                    burstTimeSum += float.Parse((string)minRow[2]);
                    
                    int delIndex = int.Parse(minRow[4]);
                    arrDataOr.RemoveAt(delIndex);
                    for (int c = delIndex; c < arrDataOr.Count; c++)
                    {
                        arrDataOr[c][4] = (int.Parse(arrDataOr[c][4]) - 1).ToString();
                    }
                }   

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
        }
        private void roundrobin(DataTable processes, int quantum)
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
        }
        private void button3_Click(object sender, EventArgs e)
        {
            startX = 15;
            inc = 0;
            width = 0;
            //  dataGridView2.Refresh();
            if (comboBox1.Text == "Non Preemptive Priority")
                pSort();
            else if (comboBox1.Text == "Round Robin")
                roundrobin(process, 10);
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
        
      
        private void dataGridView2_Paint(object sender, PaintEventArgs e)
        {

            float w = 0;
            pObject = dataGridView2.CreateGraphics();

            drawRect(width,nameOfProcess);
              
           
            w = width;
        }
    }
}
