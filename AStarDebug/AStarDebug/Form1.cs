using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace AStarDebug
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox9_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox12_TextChanged(object sender, EventArgs e)
        {

        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void textBox22_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Section_TextChanged(object sender, EventArgs e)
        {

        }

        private void Section_label_Click(object sender, EventArgs e)
        {

        }

        public string[] lines;
        public string BreakPoint;
        public string[] linesCleaner;
        List<string> linesCleaner_new = new List<string>();

        public string current_line;
        public string current_line_no_spaces;
        private void button1_Click_2(object sender, EventArgs e) // Browse
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog
            {
                InitialDirectory = @"D:\",
                Title = "Browse Text Files",

                CheckFileExists = true,
                CheckPathExists = true,

                DefaultExt = "txt",
                Filter = "txt files (*.txt)|*.txt",
                FilterIndex = 2,
                RestoreDirectory = true,

                ReadOnlyChecked = true,
                ShowReadOnly = true
            };

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Browse_line.Text = openFileDialog1.FileName;
            }

            richTextBox1.Text = String.Empty;
            lines = File.ReadAllLines(Browse_line.Text);
            Console.WriteLine("Loading the txt file");
            int count = 0;

            foreach (string line in lines)
            {
                richTextBox1.AppendText(line);
                richTextBox1.AppendText(Environment.NewLine);
            }
            CurrentPrintLine.Text = lines[0];
            this.richTextBox1.Focus();
            this.richTextBox1.SelectionStart = 0;
            this.richTextBox1.SelectionLength = lines[0].Length;
        }

        int offset = 0;
        int iterator = 0;

        //RunToBreakPoint
        private void button1_Click_3(object sender, EventArgs e)
        {
            BreakPoint = BreakPointLine.Text;

            while (lines[NextLine_counter].Contains(BreakPoint) == false)
            {
                CurrentPrintLine.Text = lines[NextLine_counter];
                current_line = lines[NextLine_counter];

                while (iterator < NextLine_counter)
                {
                    offset = offset + (lines[iterator].Length + 1);
                    iterator++;
                }

                this.richTextBox1.Focus();
                this.richTextBox1.SelectionStart = offset;
                this.richTextBox1.SelectionLength = lines[NextLine_counter].Length;

                Start_Dest();
                ExtractThread();
                ExtractSection();
                Extract_i();
                Extract_i_show();

                Extract_j();
                Extract_j_show();

                Color_theCurrentcell();
                Extract_Value();
                Extract_openList();
                Extract_ClosedList();
                f_extract();
                parents_extract();
                Thread_box();
                NextLine_counter++;

                int thread = ExtractThread();

                switch (thread)
                {
                    case '0':
                        richTextBox1.SelectionColor = Color.Black;
                        break;
                    case '1':
                        richTextBox1.SelectionColor = Color.Orange;
                        break;
                    case '2':
                        richTextBox1.SelectionColor = Color.Red;
                        break;
                    case '3':
                        richTextBox1.SelectionColor = Color.Green;
                        break;
                    case '4':
                        richTextBox1.SelectionColor = Color.SkyBlue;
                        break;
                    case '5':
                        richTextBox1.SelectionColor = Color.Pink;
                        break;
                    default:
                        Console.WriteLine("Default case");
                        break;
                }
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            this.richTextBox1.ReadOnly = true;

            
            CurrentPrintLine.Text = lines[NextLine_counter];
            current_line = lines[NextLine_counter];

            while(iterator < NextLine_counter)
            {
                offset = offset + (lines[iterator].Length+1);
                iterator++;
            }

            this.richTextBox1.Focus();
            this.richTextBox1.SelectionStart = offset;
            this.richTextBox1.SelectionLength = lines[NextLine_counter].Length;

            Start_Dest();
            ExtractThread();
            ExtractSection();
            Extract_i();
            Extract_i_show();

            Extract_j();
            Extract_j_show();

            Color_theCurrentcell();
            Extract_Value();
            Extract_openList();
            Extract_ClosedList();
            f_extract();
            parents_extract();
            Thread_box();
            NextLine_counter++;

            int thread = ExtractThread();

            switch (thread)
            {
                case '0':
                    richTextBox1.SelectionColor = Color.Black;
                    break;
                case '1':
                    richTextBox1.SelectionColor = Color.Orange;
                    break;
                case '2':
                    richTextBox1.SelectionColor = Color.Red;
                    break;
                case '3':
                    richTextBox1.SelectionColor = Color.Green;
                    break;
                case '4':
                    richTextBox1.SelectionColor = Color.SkyBlue;
                    break;
                case '5':
                    richTextBox1.SelectionColor = Color.Pink;
                    break;
                default:
                    Console.WriteLine("Default case");
                    break;
            }   
        }

        void Start_Dest()
        {
            for (int i = 0; i < current_line.Length; i++)
            {
                if ((current_line[i] == 'S' && current_line[i+1] == 'o' && current_line[i+2] == 'u' && current_line[i+3] == 'r') || current_line[i] == 'D' && current_line[i + 1] == 'e' && current_line[i + 2] == 's' && current_line[i + 3] == 't')
                {
                    if (current_line[i + 8] == '0' && current_line[i + 10] == '0')
                    {
                        zero_zero.BackColor = pictureBox14.BackColor;
                        Console.WriteLine("0 0");
                    }
                    else if (current_line[i + 8] == '0' && current_line[i + 10] == '1')
                    {
                        zero_one.BackColor = pictureBox14.BackColor;
                        Console.WriteLine("0 1");
                    }
                    else if (current_line[i + 8] == '0' && current_line[i + 10] == '2')
                    {
                        zero_two.BackColor = pictureBox14.BackColor;
                        Console.WriteLine("0 2");
                    }
                    else if (current_line[i + 8] == '0' && current_line[i + 10] == '3')
                    {
                        zero_three.BackColor = pictureBox14.BackColor;
                        Console.WriteLine("0 3");
                    }
                    else if (current_line[i + 8] == '1' && current_line[i + 10] == '0')
                    {
                        one_zero.BackColor = pictureBox14.BackColor;
                        Console.WriteLine("1 0");
                    }
                    else if (current_line[i + 8] == '1' && current_line[i + 10] == '1')
                    {
                        one_one.BackColor = pictureBox14.BackColor;
                        Console.WriteLine("1 1");
                    }
                    else if (current_line[i + 8] == '1' && current_line[i + 10] == '2')
                    {
                        one_two.BackColor = pictureBox14.BackColor;
                        Console.WriteLine("1 2");
                    }
                    else if (current_line[i + 8] == '1' && current_line[i + 10] == '3')
                    {
                        one_three.BackColor = pictureBox14.BackColor;
                        Console.WriteLine("1 3");
                    }
                    else if (current_line[i + 8] == '2' && current_line[i + 10] == '0')
                    {
                        two_zero.BackColor = pictureBox14.BackColor;
                        Console.WriteLine("2 0");
                    }
                    else if (current_line[i + 8] == '2' && current_line[i + 10] == '1')
                    {
                        two_one.BackColor = pictureBox14.BackColor;
                        Console.WriteLine("2 1");
                    }
                    else if (current_line[i + 8] == '2' && current_line[i + 10] == '2')
                    {
                        two_two.BackColor = pictureBox14.BackColor;
                        Console.WriteLine("2 2");
                    }
                    else if (current_line[i + 8] == '2' && current_line[i + 10] == '3')
                    {
                        two_three.BackColor = pictureBox14.BackColor;
                        Console.WriteLine("2 3");
                    }
                    else if (current_line[i + 8] == '3' && current_line[i + 10] == '0')
                    {
                        three_zero.BackColor = pictureBox14.BackColor;
                        Console.WriteLine("3 0");
                    }
                    else if (current_line[i + 8] == '3' && current_line[i + 10] == '1')
                    {
                        three_one.BackColor = pictureBox14.BackColor;
                        Console.WriteLine("3 1");
                    }
                    else if (current_line[i + 8] == '3' && current_line[i + 10] == '2')
                    {
                        three_two.BackColor = pictureBox14.BackColor;
                        Console.WriteLine("3 2");
                    }
                    else if (current_line[i + 8] == '3' && current_line[i + 10] == '3')
                    {
                        three_three.BackColor = pictureBox14.BackColor;
                        Console.WriteLine("3 3");
                    }
                    else
                    {
                        Console.WriteLine(current_line[i + 8].ToString(), current_line[i + 10].ToString());
                        Console.WriteLine("none of them");
                    }
                }
            }
        }

        private void Thread_TextChanged(object sender, EventArgs e)
        {

        }


        char ExtractThread()
        {
            for (int i = 0; i < current_line.Length; i++)
            {
                if (current_line[i] == 'T')
                {
                    if (current_line[i + 1] == 'H')
                    {
                        if (current_line[i + 2] == 'R')
                        {
                            if (current_line[i + 3] == 'E')
                            {
                                if (current_line[i + 4] == 'A')
                                {
                                    if (current_line[i + 5] == 'D')
                                    {
                                        if (current_line[i + 6] == ' ')
                                        {
                                            if (current_line[i + 7] == '=')
                                            {
                                                if (current_line[i + 8] == ' ')
                                                {
                                                    Thread.Text = current_line[i + 9].ToString();

                                                    if (current_line[i] == 'A' && current_line[i + 1] == 'f' && current_line[i + 2] == 't' && current_line[i + 3] == 'e' && current_line[i + 4] == 'r' && current_line[i + 5] == ' ' && current_line[i + 6] == 'a' && current_line[i + 7] == 'd')
                                                    {
                                                        openList.Clear();
                                                        ClosedList.AppendText("(");//(
                                                        ClosedList.AppendText(current_line[i + 47].ToString());//num
                                                        ClosedList.AppendText(",");//. ,
                                                        ClosedList.AppendText(current_line[i + 50].ToString());//num
                                                        ClosedList.AppendText(")");//)
                                                        ClosedList.AppendText(Environment.NewLine);
                                                    }

                                                    return current_line[i + 9];
                                                }

                                            }
                                        }

                                    }
                                }

                            }
                        }
                    }
                }
            }
            return '7';
        }

        void ExtractSection()
        {
            Section.Clear();

            for (int i = 0; i < current_line.Length; i++)
            {
                if (current_line[i] == 'S')
                {
                    if (current_line.Length > i + 1 && current_line[i + 1] == 'E') // handle out of bound exception
                    {
                        if (current_line.Length > i + 2 && current_line[i + 2] == 'C')
                        {
                            if (current_line[i + 3] == 'T')
                            {
                                if (current_line[i + 4] == 'I')
                                {
                                    if (current_line[i + 5] == 'O')
                                    {
                                        if (current_line[i + 6] == 'N')
                                        {
                                            if (current_line[i + 7] == ' ')
                                            {
                                                if (current_line[i + 8] == '=')
                                                {
                                                    if (current_line[i + 9] == ' ')
                                                    {
                                                        if (current_line[i + 10] == 'n')
                                                        {
                                                            Section.AppendText(current_line[i + 10].ToString());
                                                            Section.AppendText(current_line[i + 11].ToString());
                                                            Section.AppendText(current_line[i + 12].ToString());
                                                            Section.AppendText(current_line[i + 13].ToString());
                                                        }
                                                        else if (current_line[i + 10] == 'S')
                                                        {
                                                            Section.AppendText(current_line[i + 21].ToString());

                                                        }
                                                    }
                                                }

                                            }
                                        }

                                    }
                                }

                            }
                        }
                    }
                }
            }
        }

        char Extract_i()
        {
            for (int i = 0; i < current_line.Length; i++)
            {
                if (current_line[i] == ':' && current_line[i + 1] == ' ' && current_line[i + 2] == ' ' && current_line[i + 3] == 'C')
                {
                    //i_textbox.AppendText(current_line[i + 20].ToString());
                    return current_line[i + 20];
                }
            }

            return '9';
        }

        void Extract_i_show()
        {
            for (int i = 0; i < current_line.Length; i++)
            {
                if (current_line[i] == ':' && current_line[i + 1] == ' ' && current_line[i + 2] == ' ' && current_line[i + 3] == 'C')
                {
                    i_textbox.Text = string.Empty;
                    i_textbox.AppendText(current_line[i + 20].ToString());
                }
            }
        }

        char Extract_j()
        {
            for (int i = 0; i < current_line.Length; i++)
            {
                if (current_line[i] == ':' && current_line[i + 1] == ' ' && current_line[i + 2] == ' ' && current_line[i + 3] == 'C')
                {
                    //j_textbox.AppendText(current_line[i + 22].ToString());
                    return current_line[i + 22];
                }
            }
            return '9';
        }

        void Extract_j_show()
        {
            for (int i = 0; i < current_line.Length; i++)
            {
                if (current_line[i] == ':' && current_line[i + 1] == ' ' && current_line[i + 2] == ' ' && current_line[i + 3] == 'C')
                {
                    j_textbox.Text = string.Empty; 
                    j_textbox.AppendText(current_line[i + 22].ToString());
                }
            }
        }

        void Extract_Value()
        {
            Value.Clear();
            for (int i = 0; i < current_line.Length; i++)
            {
                if (current_line[i] == 'V' && current_line[i + 1] == 'A' && current_line[i + 2] == 'L' && current_line[i + 3] == 'U')
                {
                    Value.AppendText(current_line[i + 8].ToString());
                    Value.AppendText(current_line[i + 9].ToString());
                }
            }
        }

        int BeforeErasing = 0;
        int BeforeFirstTime = 0;

        int firstTimeErasing = 1;
        int firstTimeErasing2 = 1;

        int After_Before_Flag1 = 0;
        int After_Before_Flag2 = 0;

        int thread_counter0 = 0;
        int thread_counter1 = 0;
        int thread_counter2 = 0;
        int thread_counter3 = 0;
        int thread_counter4 = 0;
        int thread_counter5 = 0;

        int thread_counter0_del = 0;
        int thread_counter1_del = 0;
        int thread_counter2_del = 0;
        int thread_counter3_del = 0;
        int thread_counter4_del = 0;
        int thread_counter5_del = 0;

        void Extract_openList()
        {
            After_Before_Flag1 = 0;
            After_Before_Flag2 = 0;


            char thread;
            thread = ExtractThread();


            if (thread ==  '0' && thread_counter0 !=0 && thread_counter0_del ==0)
            {
                openList.Text = string.Empty;
                thread_counter0_del++;
            }
            else if(thread == '1' && thread_counter1 !=0 && thread_counter1_del == 0)
            {
                openList.Text = string.Empty;
                thread_counter1_del++;
            }
            else if (thread == '2' && thread_counter2 != 0 && thread_counter2_del == 0)
            {
                openList.Text = string.Empty;
                thread_counter2_del++;
            }
            else if (thread == '3' && thread_counter3 != 0 && thread_counter3_del == 0)
            {
                openList.Text = string.Empty;
                thread_counter3_del++;
            }
            else if (thread == '4' && thread_counter4 != 0 && thread_counter4_del == 0)
            {
                openList.Text = string.Empty;
                thread_counter4_del++;
            }
            else if (thread == '5' && thread_counter5 != 0 && thread_counter5_del == 0)
            {
                openList.Text = string.Empty;
                thread_counter5_del++;
            }
            else
            {
                Console.WriteLine("Not First time!");
            }

            for (int i = 0; i < current_line.Length; i++)
            {
                if (current_line[i] == 'P' && current_line[i + 1] == 'a' && current_line[i + 2] == 'i' && current_line[i + 3] == 'r' && current_line[i + 4] == ' ' && current_line[i + 5] == '(' && current_line[i + 29] == 'B' && current_line[i + 30] == 'e')
                {
                    openList.AppendText(current_line[i + 5].ToString());//(
                    openList.AppendText(current_line[i + 6].ToString());//num
                    openList.AppendText(current_line[i + 7].ToString());//. ,
                    openList.AppendText(current_line[i + 8].ToString());//num
                    openList.AppendText(current_line[i + 9].ToString());//)
                    openList.AppendText(Environment.NewLine);

                    

                    switch (thread)
                    {
                        case '0':
                            Console.WriteLine("Thread 0");
                            if (current_line[i + 6] == '0' && current_line[i + 8] == '0')
                            {
                                zero_zero.BackColor = pictureBox1.BackColor;
                                thread00.Text = string.Empty;
                                thread00.AppendText(current_line[i - 14].ToString());
                                Console.WriteLine("0 0");
                            }
                            else if (current_line[i + 6] == '0' && current_line[i + 8] == '1')
                            {
                                zero_one.BackColor = pictureBox1.BackColor;
                                thread01.Text = string.Empty;
                                thread01.AppendText(current_line[i - 14].ToString());
                                Console.WriteLine("0 1");
                            }
                            else if (current_line[i + 6] == '0' && current_line[i + 8] == '2')
                            {
                                zero_two.BackColor = pictureBox1.BackColor;
                                thread02.Text = string.Empty;
                                thread02.AppendText(current_line[i - 14].ToString());
                                Console.WriteLine("0 2");
                            }
                            else if (current_line[i + 6] == '0' && current_line[i + 8] == '3')
                            {
                                zero_three.BackColor = pictureBox1.BackColor;
                                thread03.Text = string.Empty;
                                thread03.AppendText(current_line[i - 14].ToString());
                                Console.WriteLine("0 3");
                            }
                            else if (current_line[i + 6] == '1' && current_line[i + 8] == '0')
                            {
                                one_zero.BackColor = pictureBox1.BackColor;
                                thread10.Text = string.Empty;
                                thread10.AppendText(current_line[i - 14].ToString());
                                Console.WriteLine("1 0");
                            }
                            else if (current_line[i + 6] == '1' && current_line[i + 8] == '1')
                            {
                                one_one.BackColor = pictureBox1.BackColor;
                                thread11.Text = string.Empty;
                                thread11.AppendText(current_line[i - 14].ToString());
                                Console.WriteLine("1 1");
                            }
                            else if (current_line[i + 6] == '1' && current_line[i + 8] == '2')
                            {
                                one_two.BackColor = pictureBox1.BackColor;
                                thread12.Text = string.Empty;
                                thread12.AppendText(current_line[i - 14].ToString());
                                Console.WriteLine("1 2");
                            }
                            else if (current_line[i + 6] == '1' && current_line[i + 8] == '3')
                            {
                                one_three.BackColor = pictureBox1.BackColor;
                                thread13.Text = string.Empty;
                                thread13.AppendText(current_line[i - 14].ToString());
                                Console.WriteLine("1 3");
                            }
                            else if (current_line[i + 6] == '2' && current_line[i + 8] == '0')
                            {
                                two_zero.BackColor = pictureBox1.BackColor;
                                thread20.Text = string.Empty;
                                thread20.AppendText(current_line[i - 14].ToString());
                                Console.WriteLine("2 0");
                            }
                            else if (current_line[i + 6] == '2' && current_line[i + 8] == '1')
                            {
                                two_one.BackColor = pictureBox1.BackColor;
                                thread21.Text = string.Empty;
                                thread21.AppendText(current_line[i - 14].ToString());
                                Console.WriteLine("2 1");
                            }
                            else if (current_line[i + 6] == '2' && current_line[i + 8] == '2')
                            {
                                two_two.BackColor = pictureBox1.BackColor;
                                thread22.Text = string.Empty;
                                thread22.AppendText(current_line[i - 14].ToString());
                                Console.WriteLine("2 2");
                            }
                            else if (current_line[i + 6] == '2' && current_line[i + 8] == '3')
                            {
                                two_three.BackColor = pictureBox1.BackColor;
                                thread23.Text = string.Empty;
                                thread23.AppendText(current_line[i - 14].ToString());
                                Console.WriteLine("2 3");
                            }
                            else if (current_line[i + 6] == '3' && current_line[i + 8] == '0')
                            {
                                three_zero.BackColor = pictureBox1.BackColor;
                                thread30.Text = string.Empty;
                                thread30.AppendText(current_line[i - 14].ToString());
                                Console.WriteLine("3 0");
                            }
                            else if (current_line[i + 6] == '3' && current_line[i + 8] == '1')
                            {
                                three_one.BackColor = pictureBox1.BackColor;
                                thread31.Text = string.Empty;
                                thread31.AppendText(current_line[i - 14].ToString());
                                Console.WriteLine("3 1");
                            }
                            else if (current_line[i + 6] == '3' && current_line[i + 8] == '2')
                            {
                                three_two.BackColor = pictureBox1.BackColor;
                                thread32.Text = string.Empty;
                                thread32.AppendText(current_line[i - 14].ToString());
                                Console.WriteLine("3 2");
                            }
                            else if (current_line[i + 6] == '3' && current_line[i + 8] == '3')
                            {
                                three_three.BackColor = pictureBox1.BackColor;
                                thread33.Text = string.Empty;
                                thread33.AppendText(current_line[i - 14].ToString());
                                Console.WriteLine("3 3");
                            }
                            else
                            {
                                Console.WriteLine(current_line[i + 6].ToString(), current_line[i + 8].ToString());
                                Console.WriteLine("none of them");
                            }
                            break;
                        case '1':
                            Console.WriteLine("Case 1");
                            if (current_line[i + 6] == '0' && current_line[i + 8] == '0')
                            {
                                zero_zero.BackColor = pictureBox4.BackColor;
                                thread00.Text = string.Empty;
                                thread00.AppendText(current_line[i - 14].ToString());
                                Console.WriteLine("0 0");
                            }
                            else if (current_line[i + 6] == '0' && current_line[i + 8] == '1')
                            {
                                zero_one.BackColor = pictureBox4.BackColor;
                                thread01.Text = string.Empty;
                                thread01.AppendText(current_line[i - 14].ToString());
                                Console.WriteLine("0 1");
                            }
                            else if (current_line[i + 6] == '0' && current_line[i + 8] == '2')
                            {
                                zero_two.BackColor = pictureBox4.BackColor;
                                thread02.Text = string.Empty;
                                thread02.AppendText(current_line[i - 14].ToString());
                                Console.WriteLine("0 2");
                            }
                            else if (current_line[i + 6] == '0' && current_line[i + 8] == '3')
                            {
                                zero_three.BackColor = pictureBox4.BackColor;
                                thread03.Text = string.Empty;
                                thread03.AppendText(current_line[i - 14].ToString());
                                Console.WriteLine("0 3");
                            }
                            else if (current_line[i + 6] == '1' && current_line[i + 8] == '0')
                            {
                                one_zero.BackColor = pictureBox4.BackColor;
                                thread10.Text = string.Empty;
                                thread10.AppendText(current_line[i - 14].ToString());
                                Console.WriteLine("1 0");
                            }
                            else if (current_line[i + 6] == '1' && current_line[i + 8] == '1')
                            {
                                one_one.BackColor = pictureBox4.BackColor;
                                thread11.Text = string.Empty;
                                thread11.AppendText(current_line[i - 14].ToString());
                                Console.WriteLine("1 1");
                            }
                            else if (current_line[i + 6] == '1' && current_line[i + 8] == '2')
                            {
                                one_two.BackColor = pictureBox4.BackColor;
                                thread12.Text = string.Empty;
                                thread12.AppendText(current_line[i - 14].ToString());
                                Console.WriteLine("1 2");
                            }
                            else if (current_line[i + 6] == '1' && current_line[i + 8] == '3')
                            {
                                one_three.BackColor = pictureBox4.BackColor;
                                thread13.Text = string.Empty;
                                thread13.AppendText(current_line[i - 14].ToString());
                                Console.WriteLine("1 3");
                            }
                            else if (current_line[i + 6] == '2' && current_line[i + 8] == '0')
                            {
                                two_zero.BackColor = pictureBox4.BackColor;
                                thread20.Text = string.Empty;
                                thread20.AppendText(current_line[i - 14].ToString());
                                Console.WriteLine("2 0");
                            }
                            else if (current_line[i + 6] == '2' && current_line[i + 8] == '1')
                            {
                                two_one.BackColor = pictureBox4.BackColor;
                                thread21.Text = string.Empty;
                                thread21.AppendText(current_line[i - 14].ToString());
                                Console.WriteLine("2 1");
                            }
                            else if (current_line[i + 6] == '2' && current_line[i + 8] == '2')
                            {
                                two_two.BackColor = pictureBox4.BackColor;
                                thread22.Text = string.Empty;
                                thread22.AppendText(current_line[i - 14].ToString());
                                Console.WriteLine("2 2");
                            }
                            else if (current_line[i + 6] == '2' && current_line[i + 8] == '3')
                            {
                                two_three.BackColor = pictureBox4.BackColor;
                                thread23.Text = string.Empty;
                                thread23.AppendText(current_line[i - 14].ToString());
                                Console.WriteLine("2 3");
                            }
                            else if (current_line[i + 6] == '3' && current_line[i + 8] == '0')
                            {
                                three_zero.BackColor = pictureBox4.BackColor;
                                thread30.Text = string.Empty;
                                thread30.AppendText(current_line[i - 14].ToString());
                                Console.WriteLine("3 0");
                            }
                            else if (current_line[i + 6] == '3' && current_line[i + 8] == '1')
                            {
                                three_one.BackColor = pictureBox4.BackColor;
                                thread31.Text = string.Empty;
                                thread31.AppendText(current_line[i - 14].ToString());
                                Console.WriteLine("3 1");
                            }
                            else if (current_line[i + 6] == '3' && current_line[i + 8] == '2')
                            {
                                three_two.BackColor = pictureBox4.BackColor;
                                thread32.Text = string.Empty;
                                thread32.AppendText(current_line[i - 14].ToString());
                                Console.WriteLine("3 2");
                            }
                            else if (current_line[i + 6] == '3' && current_line[i + 8] == '3')
                            {
                                three_three.BackColor = pictureBox4.BackColor;
                                thread33.Text = string.Empty;
                                thread33.AppendText(current_line[i - 14].ToString());
                                Console.WriteLine("3 3");
                            }
                            else
                            {
                                Console.WriteLine(current_line[i + 6].ToString(), current_line[i + 8].ToString());
                                Console.WriteLine("none of them");
                            }
                            break;
                        case '2':
                            Console.WriteLine("Case 2");
                            if (current_line[i + 6] == '0' && current_line[i + 8] == '0')
                            {
                                zero_zero.BackColor = pictureBox8.BackColor;
                                thread00.Text = string.Empty;
                                thread00.AppendText(current_line[i - 14].ToString());
                                Console.WriteLine("0 0");
                            }
                            else if (current_line[i + 6] == '0' && current_line[i + 8] == '1')
                            {
                                zero_one.BackColor = pictureBox8.BackColor;
                                thread01.Text = string.Empty;
                                thread01.AppendText(current_line[i - 14].ToString());
                                Console.WriteLine("0 1");
                            }
                            else if (current_line[i + 6] == '0' && current_line[i + 8] == '2')
                            {
                                zero_two.BackColor = pictureBox8.BackColor;
                                thread02.Text = string.Empty;
                                thread02.AppendText(current_line[i - 14].ToString());
                                Console.WriteLine("0 2");
                            }
                            else if (current_line[i + 6] == '0' && current_line[i + 8] == '3')
                            {
                                zero_three.BackColor = pictureBox8.BackColor;
                                thread03.Text = string.Empty;
                                thread03.AppendText(current_line[i - 14].ToString());
                                Console.WriteLine("0 3");
                            }
                            else if (current_line[i + 6] == '1' && current_line[i + 8] == '0')
                            {
                                one_zero.BackColor = pictureBox8.BackColor;
                                thread10.Text = string.Empty;
                                thread10.AppendText(current_line[i - 14].ToString());
                                Console.WriteLine("1 0");
                            }
                            else if (current_line[i + 6] == '1' && current_line[i + 8] == '1')
                            {
                                one_one.BackColor = pictureBox8.BackColor;
                                thread11.Text = string.Empty;
                                thread11.AppendText(current_line[i - 14].ToString());
                                Console.WriteLine("1 1");
                            }
                            else if (current_line[i + 6] == '1' && current_line[i + 8] == '2')
                            {
                                one_two.BackColor = pictureBox8.BackColor;
                                thread12.Text = string.Empty;
                                thread12.AppendText(current_line[i - 14].ToString());
                                Console.WriteLine("1 2");
                            }
                            else if (current_line[i + 6] == '1' && current_line[i + 8] == '3')
                            {
                                one_three.BackColor = pictureBox8.BackColor;
                                thread13.Text = string.Empty;
                                thread13.AppendText(current_line[i - 14].ToString());
                                Console.WriteLine("1 3");
                            }
                            else if (current_line[i + 6] == '2' && current_line[i + 8] == '0')
                            {
                                two_zero.BackColor = pictureBox8.BackColor;
                                thread20.Text = string.Empty;
                                thread20.AppendText(current_line[i - 14].ToString());
                                Console.WriteLine("2 0");
                            }
                            else if (current_line[i + 6] == '2' && current_line[i + 8] == '1')
                            {
                                two_one.BackColor = pictureBox8.BackColor;
                                thread21.Text = string.Empty;
                                thread21.AppendText(current_line[i - 14].ToString());
                                Console.WriteLine("2 1");
                            }
                            else if (current_line[i + 6] == '2' && current_line[i + 8] == '2')
                            {
                                two_two.BackColor = pictureBox8.BackColor;
                                thread22.Text = string.Empty;
                                thread22.AppendText(current_line[i - 14].ToString());
                                Console.WriteLine("2 2");
                            }
                            else if (current_line[i + 6] == '2' && current_line[i + 8] == '3')
                            {
                                two_three.BackColor = pictureBox8.BackColor;
                                thread23.Text = string.Empty;
                                thread23.AppendText(current_line[i - 14].ToString());
                                Console.WriteLine("2 3");
                            }
                            else if (current_line[i + 6] == '3' && current_line[i + 8] == '0')
                            {
                                three_zero.BackColor = pictureBox8.BackColor;
                                thread30.Text = string.Empty;
                                thread30.AppendText(current_line[i - 14].ToString());
                                Console.WriteLine("3 0");
                            }
                            else if (current_line[i + 6] == '3' && current_line[i + 8] == '1')
                            {
                                three_one.BackColor = pictureBox8.BackColor;
                                thread31.Text = string.Empty;
                                thread31.AppendText(current_line[i - 14].ToString());
                                Console.WriteLine("3 1");
                            }
                            else if (current_line[i + 6] == '3' && current_line[i + 8] == '2')
                            {
                                three_two.BackColor = pictureBox8.BackColor;
                                thread32.Text = string.Empty;
                                thread32.AppendText(current_line[i - 14].ToString());
                                Console.WriteLine("3 2");
                            }
                            else if (current_line[i + 6] == '3' && current_line[i + 8] == '3')
                            {
                                three_three.BackColor = pictureBox8.BackColor;
                                thread33.Text = string.Empty;
                                thread33.AppendText(current_line[i - 14].ToString());
                                Console.WriteLine("3 3");
                            }
                            else
                            {
                                Console.WriteLine(current_line[i + 6].ToString(), current_line[i + 8].ToString());
                                Console.WriteLine("none of them");
                            }
                            break;
                        case '3':
                            Console.WriteLine("Case 3");
                            if (current_line[i + 6] == '0' && current_line[i + 8] == '0')
                            {
                                zero_zero.BackColor = pictureBox6.BackColor;
                                thread00.Text = string.Empty;
                                thread00.AppendText(current_line[i - 14].ToString());
                                Console.WriteLine("0 0");
                            }
                            else if (current_line[i + 6] == '0' && current_line[i + 8] == '1')
                            {
                                zero_one.BackColor = pictureBox6.BackColor;
                                thread01.Text = string.Empty;
                                thread01.AppendText(current_line[i - 14].ToString());
                                Console.WriteLine("0 1");
                            }
                            else if (current_line[i + 6] == '0' && current_line[i + 8] == '2')
                            {
                                zero_two.BackColor = pictureBox6.BackColor;
                                thread02.Text = string.Empty;
                                thread02.AppendText(current_line[i - 14].ToString());
                                Console.WriteLine("0 2");
                            }
                            else if (current_line[i + 6] == '0' && current_line[i + 8] == '3')
                            {
                                zero_three.BackColor = pictureBox6.BackColor;
                                thread03.Text = string.Empty;
                                thread03.AppendText(current_line[i - 14].ToString());
                                Console.WriteLine("0 3");
                            }
                            else if (current_line[i + 6] == '1' && current_line[i + 8] == '0')
                            {
                                one_zero.BackColor = pictureBox6.BackColor;
                                thread10.Text = string.Empty;
                                thread10.AppendText(current_line[i - 14].ToString());
                                Console.WriteLine("1 0");
                            }
                            else if (current_line[i + 6] == '1' && current_line[i + 8] == '1')
                            {
                                one_one.BackColor = pictureBox6.BackColor;
                                thread11.Text = string.Empty;
                                thread11.AppendText(current_line[i - 14].ToString());
                                Console.WriteLine("1 1");
                            }
                            else if (current_line[i + 6] == '1' && current_line[i + 8] == '2')
                            {
                                one_two.BackColor = pictureBox6.BackColor;
                                thread12.Text = string.Empty;
                                thread12.AppendText(current_line[i - 14].ToString());
                                Console.WriteLine("1 2");
                            }
                            else if (current_line[i + 6] == '1' && current_line[i + 8] == '3')
                            {
                                one_three.BackColor = pictureBox6.BackColor;
                                thread13.Text = string.Empty;
                                thread13.AppendText(current_line[i - 14].ToString());
                                Console.WriteLine("1 3");
                            }
                            else if (current_line[i + 6] == '2' && current_line[i + 8] == '0')
                            {
                                two_zero.BackColor = pictureBox6.BackColor;
                                thread20.Text = string.Empty;
                                thread20.AppendText(current_line[i - 14].ToString());
                                Console.WriteLine("2 0");
                            }
                            else if (current_line[i + 6] == '2' && current_line[i + 8] == '1')
                            {
                                two_one.BackColor = pictureBox6.BackColor;
                                thread21.Text = string.Empty;
                                thread21.AppendText(current_line[i - 14].ToString());
                                Console.WriteLine("2 1");
                            }
                            else if (current_line[i + 6] == '2' && current_line[i + 8] == '2')
                            {
                                two_two.BackColor = pictureBox6.BackColor;
                                thread22.Text = string.Empty;
                                thread22.AppendText(current_line[i - 14].ToString());
                                Console.WriteLine("2 2");
                            }
                            else if (current_line[i + 6] == '2' && current_line[i + 8] == '3')
                            {
                                two_three.BackColor = pictureBox6.BackColor;
                                thread23.Text = string.Empty;
                                thread23.AppendText(current_line[i - 14].ToString());
                                Console.WriteLine("2 3");
                            }
                            else if (current_line[i + 6] == '3' && current_line[i + 8] == '0')
                            {
                                three_zero.BackColor = pictureBox6.BackColor;
                                thread30.Text = string.Empty;
                                thread30.AppendText(current_line[i - 14].ToString());
                                Console.WriteLine("3 0");
                            }
                            else if (current_line[i + 6] == '3' && current_line[i + 8] == '1')
                            {
                                three_one.BackColor = pictureBox6.BackColor;
                                thread31.Text = string.Empty;
                                thread31.AppendText(current_line[i - 14].ToString());
                                Console.WriteLine("3 1");
                            }
                            else if (current_line[i + 6] == '3' && current_line[i + 8] == '2')
                            {
                                three_two.BackColor = pictureBox6.BackColor;
                                thread32.Text = string.Empty;
                                thread32.AppendText(current_line[i - 14].ToString());
                                Console.WriteLine("3 2");
                            }
                            else if (current_line[i + 6] == '3' && current_line[i + 8] == '3')
                            {
                                three_three.BackColor = pictureBox6.BackColor;
                                thread33.Text = string.Empty;
                                thread33.AppendText(current_line[i - 14].ToString());
                                Console.WriteLine("3 3");
                            }
                            else
                            {
                                Console.WriteLine(current_line[i + 6].ToString(), current_line[i + 8].ToString());
                                Console.WriteLine("none of them");
                            }
                            break;
                        case '4':
                            if (current_line[i + 6] == '0' && current_line[i + 8] == '0')
                            {
                                zero_zero.BackColor = pictureBox12.BackColor;
                                thread00.Text = string.Empty;
                                thread00.AppendText(current_line[i - 14].ToString());
                                Console.WriteLine("0 0");
                            }
                            else if (current_line[i + 6] == '0' && current_line[i + 8] == '1')
                            {
                                zero_one.BackColor = pictureBox12.BackColor;
                                thread01.Text = string.Empty;
                                thread01.AppendText(current_line[i - 14].ToString());
                                Console.WriteLine("0 1");
                            }
                            else if (current_line[i + 6] == '0' && current_line[i + 8] == '2')
                            {
                                zero_two.BackColor = pictureBox12.BackColor;
                                thread02.Text = string.Empty;
                                thread02.AppendText(current_line[i - 14].ToString());
                                Console.WriteLine("0 2");
                            }
                            else if (current_line[i + 6] == '0' && current_line[i + 8] == '3')
                            {
                                zero_three.BackColor = pictureBox12.BackColor;
                                thread03.Text = string.Empty;
                                thread03.AppendText(current_line[i - 14].ToString());
                                Console.WriteLine("0 3");
                            }
                            else if (current_line[i + 6] == '1' && current_line[i + 8] == '0')
                            {
                                one_zero.BackColor = pictureBox12.BackColor;
                                thread10.Text = string.Empty;
                                thread10.AppendText(current_line[i - 14].ToString());
                                Console.WriteLine("1 0");
                            }
                            else if (current_line[i + 6] == '1' && current_line[i + 8] == '1')
                            {
                                one_one.BackColor = pictureBox12.BackColor;
                                thread11.Text = string.Empty;
                                thread11.AppendText(current_line[i - 14].ToString());
                                Console.WriteLine("1 1");
                            }
                            else if (current_line[i + 6] == '1' && current_line[i + 8] == '2')
                            {
                                one_two.BackColor = pictureBox12.BackColor;
                                thread12.Text = string.Empty;
                                thread12.AppendText(current_line[i - 14].ToString());
                                Console.WriteLine("1 2");
                            }
                            else if (current_line[i + 6] == '1' && current_line[i + 8] == '3')
                            {
                                one_three.BackColor = pictureBox12.BackColor;
                                thread13.Text = string.Empty;
                                thread13.AppendText(current_line[i - 14].ToString());
                                Console.WriteLine("1 3");
                            }
                            else if (current_line[i + 6] == '2' && current_line[i + 8] == '0')
                            {
                                two_zero.BackColor = pictureBox12.BackColor;
                                thread20.Text = string.Empty;
                                thread20.AppendText(current_line[i - 14].ToString());
                                Console.WriteLine("2 0");
                            }
                            else if (current_line[i + 6] == '2' && current_line[i + 8] == '1')
                            {
                                two_one.BackColor = pictureBox12.BackColor;
                                thread21.Text = string.Empty;
                                thread21.AppendText(current_line[i - 14].ToString());
                                Console.WriteLine("2 1");
                            }
                            else if (current_line[i + 6] == '2' && current_line[i + 8] == '2')
                            {
                                two_two.BackColor = pictureBox12.BackColor;
                                thread22.Text = string.Empty;
                                thread22.AppendText(current_line[i - 14].ToString());
                                Console.WriteLine("2 2");
                            }
                            else if (current_line[i + 6] == '2' && current_line[i + 8] == '3')
                            {
                                two_three.BackColor = pictureBox12.BackColor;
                                thread23.Text = string.Empty;
                                thread23.AppendText(current_line[i - 14].ToString());
                                Console.WriteLine("2 3");
                            }
                            else if (current_line[i + 6] == '3' && current_line[i + 8] == '0')
                            {
                                three_zero.BackColor = pictureBox12.BackColor;
                                thread30.Text = string.Empty;
                                thread30.AppendText(current_line[i - 14].ToString());
                                Console.WriteLine("3 0");
                            }
                            else if (current_line[i + 6] == '3' && current_line[i + 8] == '1')
                            {
                                three_one.BackColor = pictureBox12.BackColor;
                                thread31.Text = string.Empty;
                                thread31.AppendText(current_line[i - 14].ToString());
                                Console.WriteLine("3 1");
                            }
                            else if (current_line[i + 6] == '3' && current_line[i + 8] == '2')
                            {
                                three_two.BackColor = pictureBox12.BackColor;
                                thread32.Text = string.Empty;
                                thread32.AppendText(current_line[i - 14].ToString());
                                Console.WriteLine("3 2");
                            }
                            else if (current_line[i + 6] == '3' && current_line[i + 8] == '3')
                            {
                                three_three.BackColor = pictureBox12.BackColor;
                                thread33.Text = string.Empty;
                                thread33.AppendText(current_line[i - 14].ToString());
                                Console.WriteLine("3 3");
                            }
                            else
                            {
                                Console.WriteLine(current_line[i + 6].ToString(), current_line[i + 8].ToString());
                                Console.WriteLine("none of them");
                            }
                            Console.WriteLine("Case 4");
                            break;
                        case '5':
                            if (current_line[i + 6] == '0' && current_line[i + 8] == '0')
                            {
                                zero_zero.BackColor = pictureBox10.BackColor;
                                thread00.Text = string.Empty;
                                thread00.AppendText(current_line[i - 14].ToString());
                                Console.WriteLine("0 0");
                            }
                            else if (current_line[i + 6] == '0' && current_line[i + 8] == '1')
                            {
                                zero_one.BackColor = pictureBox10.BackColor;
                                thread01.Text = string.Empty;
                                thread01.AppendText(current_line[i - 14].ToString());
                                Console.WriteLine("0 1");
                            }
                            else if (current_line[i + 6] == '0' && current_line[i + 8] == '2')
                            {
                                zero_two.BackColor = pictureBox10.BackColor;
                                thread02.Text = string.Empty;
                                thread02.AppendText(current_line[i - 14].ToString());
                                Console.WriteLine("0 2");
                            }
                            else if (current_line[i + 6] == '0' && current_line[i + 8] == '3')
                            {
                                zero_three.BackColor = pictureBox10.BackColor;
                                thread03.Text = string.Empty;
                                thread03.AppendText(current_line[i - 14].ToString());
                                Console.WriteLine("0 3");
                            }
                            else if (current_line[i + 6] == '1' && current_line[i + 8] == '0')
                            {
                                one_zero.BackColor = pictureBox10.BackColor;
                                thread10.Text = string.Empty;
                                thread10.AppendText(current_line[i - 14].ToString());
                                Console.WriteLine("1 0");
                            }
                            else if (current_line[i + 6] == '1' && current_line[i + 8] == '1')
                            {
                                one_one.BackColor = pictureBox10.BackColor;
                                thread11.Text = string.Empty;
                                thread11.AppendText(current_line[i - 14].ToString());
                                Console.WriteLine("1 1");
                            }
                            else if (current_line[i + 6] == '1' && current_line[i + 8] == '2')
                            {
                                one_two.BackColor = pictureBox10.BackColor;
                                thread12.Text = string.Empty;
                                thread12.AppendText(current_line[i - 14].ToString());
                                Console.WriteLine("1 2");
                            }
                            else if (current_line[i + 6] == '1' && current_line[i + 8] == '3')
                            {
                                one_three.BackColor = pictureBox10.BackColor;
                                thread13.Text = string.Empty;
                                thread13.AppendText(current_line[i - 14].ToString());
                                Console.WriteLine("1 3");
                            }
                            else if (current_line[i + 6] == '2' && current_line[i + 8] == '0')
                            {
                                two_zero.BackColor = pictureBox10.BackColor;
                                thread20.Text = string.Empty;
                                thread20.AppendText(current_line[i - 14].ToString());
                                Console.WriteLine("2 0");
                            }
                            else if (current_line[i + 6] == '2' && current_line[i + 8] == '1')
                            {
                                two_one.BackColor = pictureBox10.BackColor;
                                thread21.Text = string.Empty;
                                thread21.AppendText(current_line[i - 14].ToString());
                                Console.WriteLine("2 1");
                            }
                            else if (current_line[i + 6] == '2' && current_line[i + 8] == '2')
                            {
                                two_two.BackColor = pictureBox10.BackColor;
                                thread22.Text = string.Empty;
                                thread22.AppendText(current_line[i - 14].ToString());
                                Console.WriteLine("2 2");
                            }
                            else if (current_line[i + 6] == '2' && current_line[i + 8] == '3')
                            {
                                two_three.BackColor = pictureBox10.BackColor;
                                thread23.Text = string.Empty;
                                thread23.AppendText(current_line[i - 14].ToString());
                                Console.WriteLine("2 3");
                            }
                            else if (current_line[i + 6] == '3' && current_line[i + 8] == '0')
                            {
                                three_zero.BackColor = pictureBox10.BackColor;
                                thread30.Text = string.Empty;
                                thread30.AppendText(current_line[i - 14].ToString());
                                Console.WriteLine("3 0");
                            }
                            else if (current_line[i + 6] == '3' && current_line[i + 8] == '1')
                            {
                                three_one.BackColor = pictureBox10.BackColor;
                                thread31.Text = string.Empty;
                                thread31.AppendText(current_line[i - 14].ToString());
                                Console.WriteLine("3 1");
                            }
                            else if (current_line[i + 6] == '3' && current_line[i + 8] == '2')
                            {
                                three_two.BackColor = pictureBox10.BackColor;
                                thread32.Text = string.Empty;
                                thread32.AppendText(current_line[i - 14].ToString());
                                Console.WriteLine("3 2");
                            }
                            else if (current_line[i + 6] == '3' && current_line[i + 8] == '3')
                            {
                                three_three.BackColor = pictureBox10.BackColor;
                                thread33.Text = string.Empty;
                                thread33.AppendText(current_line[i - 14].ToString());
                                Console.WriteLine("3 3");
                            }
                            else
                            {
                                Console.WriteLine(current_line[i + 6].ToString(), current_line[i + 8].ToString());
                                Console.WriteLine("none of them");
                            }
                            Console.WriteLine("Case 5");
                            break;
                        default:
                            Console.WriteLine("Default case");
                            break;
                    }
                }
                if (current_line[i] == 'B' && current_line[i + 1] == 'e' && current_line[i + 2] == 'f' && current_line[i + 3] == 'o' && current_line[i + 4] == 'r' && current_line[i + 5] == 'e' && current_line[i + 6] == ' ' && current_line[i + 7] == 'e' && current_line[i + 8] == 'r' && current_line[i + 9] == 'a' && current_line[i + 10] == 's' && current_line[i + 11] == 'i' && current_line[i + 12] == 'n' && current_line[i + 13] == 'g')
                    {
             
                    //BeforeErasing++;
                    After_Before_Flag1++;
                    firstTimeErasing++;
                    if (firstTimeErasing == 0)
                    {
                        Console.WriteLine("openList.Text = string.Empty;");
                        openList.Text = string.Empty;
                    }

                        openList.AppendText(current_line[i + 49].ToString());//(
                        openList.AppendText(current_line[i + 50].ToString());//num
                        openList.AppendText(current_line[i + 51].ToString());//. ,
                        openList.AppendText(current_line[i + 52].ToString());//num
                        openList.AppendText(current_line[i + 53].ToString());//)
                        openList.AppendText(Environment.NewLine);

                        switch (thread)
                        {
                        case '0':
                            thread_counter0++;
                            Console.WriteLine("thread_counter0 = "+ thread_counter0);
                            break;
                        case '1':
                            thread_counter1++;
                            Console.WriteLine("thread_counter1 = " + thread_counter1);
                            break;
                        case '2':
                            thread_counter2++;
                            Console.WriteLine("thread_counter2 = " + thread_counter2);
                            break;
                        case '3':
                            thread_counter3++;
                            Console.WriteLine("thread_counter3 = " + thread_counter3);
                            break;
                        case '4':
                            Console.WriteLine("thread_counter4 = " + thread_counter4);
                            thread_counter4++;
                            break;
                        case '5':
                            Console.WriteLine("thread_counter5 = " + thread_counter5);
                            thread_counter5++;
                            break;
                        default:
                            Console.WriteLine("Default case");
                            break;
                    }


                }
                if (current_line[i] == 'A' && current_line[i + 1] == 'f' && current_line[i + 2] == 't' && current_line[i + 3] == 'e' && current_line[i + 4] == 'r' && current_line[i + 5] == ' ' && current_line[i + 6] == 'e' && current_line[i + 7] == 'r' && current_line[i + 8] == 'a' && current_line[i + 9] == 's' && current_line[i + 10] == 'i' && current_line[i + 11] == 'n' && current_line[i + 12] == 'g')
                    {
                    After_Before_Flag2++;
                    

                    if (firstTimeErasing2 == 0)
                    {
                        openList.Text = string.Empty;
                        if (zero_zero.BackColor == pictureBox1.BackColor || zero_zero.BackColor == pictureBox4.BackColor || zero_zero.BackColor == pictureBox8.BackColor || zero_zero.BackColor == pictureBox6.BackColor || zero_zero.BackColor == pictureBox12.BackColor || zero_zero.BackColor == pictureBox10.BackColor)
                        {
                            c00.BackColor = zero_zero.BackColor;
                            zero_zero.BackColor = textBox1.BackColor;
                        }
                        if (zero_one.BackColor == pictureBox1.BackColor || zero_one.BackColor == pictureBox4.BackColor || zero_one.BackColor == pictureBox8.BackColor || zero_one.BackColor == pictureBox6.BackColor || zero_one.BackColor == pictureBox12.BackColor || zero_one.BackColor == pictureBox10.BackColor)
                        {
                            c01.BackColor = zero_one.BackColor;
                            zero_one.BackColor = textBox1.BackColor;
                        }
                        if (zero_two.BackColor == pictureBox1.BackColor || zero_two.BackColor == pictureBox4.BackColor || zero_two.BackColor == pictureBox8.BackColor || zero_two.BackColor == pictureBox6.BackColor || zero_two.BackColor == pictureBox12.BackColor || zero_two.BackColor == pictureBox10.BackColor)
                        {
                            c02.BackColor = zero_two.BackColor;
                            zero_two.BackColor = textBox1.BackColor;
                        }
                        if (zero_three.BackColor == pictureBox1.BackColor || zero_three.BackColor == pictureBox4.BackColor || zero_three.BackColor == pictureBox8.BackColor || zero_three.BackColor == pictureBox6.BackColor || zero_three.BackColor == pictureBox12.BackColor || zero_three.BackColor == pictureBox1.BackColor)
                        {
                            c03.BackColor = zero_three.BackColor;
                            zero_three.BackColor = textBox1.BackColor;
                        }
                        if (one_zero.BackColor == pictureBox1.BackColor || one_zero.BackColor == pictureBox4.BackColor || one_zero.BackColor == pictureBox8.BackColor || one_zero.BackColor == pictureBox6.BackColor || one_zero.BackColor == pictureBox12.BackColor || one_zero.BackColor == pictureBox10.BackColor)
                        {
                            c10.BackColor = one_zero.BackColor;
                            one_zero.BackColor = textBox1.BackColor;
                        }
                        if (one_one.BackColor == pictureBox1.BackColor || one_one.BackColor == pictureBox4.BackColor || one_one.BackColor == pictureBox8.BackColor || one_one.BackColor == pictureBox6.BackColor || one_one.BackColor == pictureBox12.BackColor || one_one.BackColor == pictureBox10.BackColor)
                        {
                            c11.BackColor = one_one.BackColor;
                            one_one.BackColor = textBox1.BackColor;
                        }
                        if (one_two.BackColor == pictureBox1.BackColor || one_two.BackColor == pictureBox4.BackColor || one_two.BackColor == pictureBox8.BackColor || one_two.BackColor == pictureBox6.BackColor || one_two.BackColor == pictureBox12.BackColor || one_two.BackColor == pictureBox10.BackColor)
                        {
                            c12.BackColor = one_two.BackColor;
                            one_two.BackColor = textBox1.BackColor;
                        }
                        if (one_three.BackColor == pictureBox1.BackColor || one_three.BackColor == pictureBox4.BackColor || one_three.BackColor == pictureBox8.BackColor || one_three.BackColor == pictureBox6.BackColor || one_three.BackColor == pictureBox12.BackColor || one_three.BackColor == pictureBox10.BackColor)
                        {
                            c13.BackColor = one_three.BackColor;
                            one_three.BackColor = textBox1.BackColor;
                        }
                        if (two_zero.BackColor == pictureBox1.BackColor || two_zero.BackColor == pictureBox4.BackColor || two_zero.BackColor == pictureBox8.BackColor || two_zero.BackColor == pictureBox6.BackColor || two_zero.BackColor == pictureBox12.BackColor || two_zero.BackColor == pictureBox10.BackColor)
                        {
                            c20.BackColor = two_zero.BackColor;
                            two_zero.BackColor = textBox1.BackColor;
                        }
                        if (two_one.BackColor == pictureBox1.BackColor || two_one.BackColor == pictureBox4.BackColor || two_one.BackColor == pictureBox8.BackColor || two_one.BackColor == pictureBox6.BackColor || two_one.BackColor == pictureBox12.BackColor || two_one.BackColor == pictureBox10.BackColor)
                        {
                            c21.BackColor = two_one.BackColor;
                            two_one.BackColor = textBox1.BackColor;
                        }
                        if (two_two.BackColor == pictureBox1.BackColor || two_two.BackColor == pictureBox4.BackColor || two_two.BackColor == pictureBox8.BackColor || two_two.BackColor == pictureBox6.BackColor || two_two.BackColor == pictureBox12.BackColor || two_two.BackColor == pictureBox10.BackColor)
                        {
                            c22.BackColor = two_two.BackColor;
                            two_two.BackColor = textBox1.BackColor;
                        }
                        if (two_three.BackColor == pictureBox1.BackColor || two_three.BackColor == pictureBox4.BackColor || two_three.BackColor == pictureBox8.BackColor || two_three.BackColor == pictureBox6.BackColor || two_three.BackColor == pictureBox12.BackColor || two_three.BackColor == pictureBox10.BackColor)
                        {
                            c23.BackColor = two_three.BackColor;
                            two_three.BackColor = textBox1.BackColor;
                        }
                        if (three_zero.BackColor == pictureBox1.BackColor || three_zero.BackColor == pictureBox4.BackColor || three_zero.BackColor == pictureBox8.BackColor || three_zero.BackColor == pictureBox6.BackColor || three_zero.BackColor == pictureBox12.BackColor || three_zero.BackColor == pictureBox10.BackColor)
                        {
                            c30.BackColor = three_zero.BackColor;
                            three_zero.BackColor = textBox1.BackColor;
                        }
                        if (three_one.BackColor == pictureBox1.BackColor || three_one.BackColor == pictureBox4.BackColor || three_one.BackColor == pictureBox8.BackColor || three_one.BackColor == pictureBox6.BackColor || three_one.BackColor == pictureBox12.BackColor || three_one.BackColor == pictureBox10.BackColor)
                        {
                            c31.BackColor = three_one.BackColor;
                            three_one.BackColor = textBox1.BackColor;
                        }
                        if (three_two.BackColor == pictureBox1.BackColor || three_two.BackColor == pictureBox4.BackColor || three_two.BackColor == pictureBox8.BackColor || three_two.BackColor == pictureBox6.BackColor || three_two.BackColor == pictureBox12.BackColor || three_two.BackColor == pictureBox10.BackColor)
                        {
                            c32.BackColor = three_two.BackColor;
                            three_two.BackColor = textBox1.BackColor;
                        }
                        if (three_three.BackColor == pictureBox1.BackColor || three_three.BackColor == pictureBox4.BackColor || three_three.BackColor == pictureBox8.BackColor || three_three.BackColor == pictureBox6.BackColor || three_three.BackColor == pictureBox12.BackColor || three_three.BackColor == pictureBox10.BackColor)
                        {
                            c33.BackColor = three_three.BackColor;
                            three_three.BackColor = textBox1.BackColor;
                        }
                    }

                        openList.AppendText(current_line[i + 48].ToString());//(
                        openList.AppendText(current_line[i + 49].ToString());//num
                        openList.AppendText(current_line[i + 50].ToString());//. ,
                        openList.AppendText(current_line[i + 51].ToString());//num
                        openList.AppendText(current_line[i + 52].ToString());//)

                    if (current_line[i + 49] == '0' && current_line[i + 51] == '0')
                    {
                        zero_zero.BackColor = c00.BackColor;

                    }
                    else if (current_line[i + 49] == '0' && current_line[i + 51] == '1')
                    {
                        zero_one.BackColor = c01.BackColor;
                    }
                    else if (current_line[i + 49] == '0' && current_line[i + 51] == '2')
                    {
                        zero_two.BackColor = c02.BackColor;
                    }
                    else if (current_line[i + 49] == '0' && current_line[i + 51] == '3')
                    {
                        zero_three.BackColor = c03.BackColor;
                    }
                    else if (current_line[i + 49] == '1' && current_line[i + 51] == '0')
                    {
                        one_zero.BackColor = c10.BackColor;
                    }
                    else if (current_line[i + 49] == '1' && current_line[i + 51] == '1')
                    {
                        one_one.BackColor = c11.BackColor;
                    }
                    else if (current_line[i + 49] == '1' && current_line[i + 51] == '2')
                    {
                        one_two.BackColor = c12.BackColor;
                    }
                    else if (current_line[i + 49] == '1' && current_line[i + 51] == '3')
                    {
                        one_three.BackColor = c13.BackColor;
                    }
                    else if (current_line[i + 49] == '2' && current_line[i + 51] == '0')
                    {
                        two_zero.BackColor = c20.BackColor;
                    }
                    else if (current_line[i + 49] == '2' && current_line[i + 51] == '1')
                    {
                        two_one.BackColor = c21.BackColor;
                    }
                    else if (current_line[i + 49] == '2' && current_line[i + 51] == '2')
                    {
                        two_two.BackColor = c22.BackColor;
                    }
                    else if (current_line[i + 49] == '2' && current_line[i + 51] == '3')
                    {
                        two_three.BackColor = c23.BackColor;
                    }
                    else if (current_line[i + 49] == '3' && current_line[i + 51] == '0')
                    {
                        three_zero.BackColor = c30.BackColor;
                    }
                    else if (current_line[i + 49] == '3' && current_line[i + 51] == '1')
                    {
                        three_one.BackColor = c31.BackColor;
                    }
                    else if (current_line[i + 49] == '3' && current_line[i + 51] == '2')
                    {
                        three_two.BackColor = c32.BackColor;
                    }
                    else if (current_line[i + 49] == '3' && current_line[i + 51] == '3')
                    {
                        three_three.BackColor = c33.BackColor;
                    }
                    else
                    {
                        Console.WriteLine(current_line[i + 49].ToString(), current_line[i + 51].ToString());
                        Console.WriteLine("none of them");
                    }

                    openList.AppendText(Environment.NewLine);
                    firstTimeErasing2++;
                }

            }


            if (After_Before_Flag1 == 0)
            {
                firstTimeErasing = 0;
            }

            if (After_Before_Flag2 == 0)
            {
                firstTimeErasing2 = 0;
            }

        }

        int first_iteration = 0;
        int After_Before_Flag = 0;
        void Extract_ClosedList()
        {
            After_Before_Flag = 0;
            for (int i = 0; i < current_line.Length; i++)
            {
                if (current_line[i] == 'A' && current_line[i + 1] == 'f' && current_line[i + 2] == 't' && current_line[i + 3] == 'e' && current_line[i + 4] == 'r' && current_line[i + 5] == ' ' && current_line[i + 6] == 'a' && current_line[i + 7] == 'd')
                {
                        if (first_iteration == 0)
                        {
                            ClosedList.Text = string.Empty;
                        }

                        Console.WriteLine("first_iteration " + first_iteration);
                        ClosedList.AppendText("(");//(
                        ClosedList.AppendText(current_line[i + 47].ToString());//num
                        ClosedList.AppendText(",");//. ,
                        ClosedList.AppendText(current_line[i + 50].ToString());//num
                        ClosedList.AppendText(")");//)
                        ClosedList.AppendText(Environment.NewLine);
                        first_iteration++;
                        After_Before_Flag++;

                        if (current_line[i + 47] == '0' && current_line[i + 50] == '0')
                        {
                            zero_zero.BackColor = textBox1.BackColor;
                            zero_zero.BackColor = pictureBox13.BackColor;
                           // Console.WriteLine("0 0");
                        }
                        else if (current_line[i + 47] == '0' && current_line[i + 50] == '1')
                        {
                            zero_one.BackColor = textBox1.BackColor;
                            zero_one.BackColor = pictureBox13.BackColor;
                            //Console.WriteLine("0 1");
                        }
                        else if (current_line[i + 47] == '0' && current_line[i + 50] == '2')
                        {
                            zero_two.BackColor = textBox1.BackColor;
                            zero_two.BackColor = pictureBox13.BackColor; ;
                            //Console.WriteLine("0 2");
                        }
                        else if (current_line[i + 47] == '0' && current_line[i + 50] == '3')
                        {
                            zero_three.BackColor = textBox1.BackColor;
                            zero_three.BackColor = pictureBox13.BackColor;
                          //  Console.WriteLine("0 3");
                        }
                        else if (current_line[i + 47] == '1' && current_line[i + 50] == '0')
                        {
                            one_zero.BackColor = textBox1.BackColor;
                            one_zero.BackColor = pictureBox13.BackColor;
                           // Console.WriteLine("1 0");
                        }
                        else if (current_line[i + 47] == '1' && current_line[i + 50] == '1')
                        {
                            one_one.BackColor = textBox1.BackColor;
                            one_one.BackColor = pictureBox13.BackColor;
                            //Console.WriteLine("1 1");
                        }
                        else if (current_line[i + 47] == '1' && current_line[i + 50] == '2')
                        {
                            one_two.BackColor = textBox1.BackColor;
                            one_two.BackColor = pictureBox13.BackColor;
                            //Console.WriteLine("1 2");
                        }
                        else if (current_line[i + 47] == '1' && current_line[i + 50] == '3')
                        {
                            one_three.BackColor = textBox1.BackColor;
                            one_three.BackColor = pictureBox13.BackColor;
                           // Console.WriteLine("1 3");
                        }
                        else if (current_line[i + 47] == '2' && current_line[i + 50] == '0')
                        {
                            two_zero.BackColor = textBox1.BackColor;
                            two_zero.BackColor = pictureBox13.BackColor;
                            //Console.WriteLine("2 0");
                        }
                        else if (current_line[i + 47] == '2' && current_line[i + 50] == '1')
                        {
                            two_one.BackColor = textBox1.BackColor;
                            two_one.BackColor = pictureBox13.BackColor;
                          //  Console.WriteLine("2 1");
                        }
                        else if (current_line[i + 47] == '2' && current_line[i + 50] == '2')
                        {
                            two_two.BackColor = textBox1.BackColor;
                            two_two.BackColor = pictureBox13.BackColor;
                          //  Console.WriteLine("2 2");
                        }
                        else if (current_line[i + 47] == '2' && current_line[i + 50] == '3')
                        {
                            two_three.BackColor = textBox1.BackColor;
                            two_three.BackColor = pictureBox13.BackColor;
                           // Console.WriteLine("2 3");
                        }
                        else if (current_line[i + 47] == '3' && current_line[i + 50] == '0')
                        {
                            three_zero.BackColor = textBox1.BackColor;
                            three_zero.BackColor = pictureBox13.BackColor;
                            //Console.WriteLine("3 0");
                        }
                        else if (current_line[i + 47] == '3' && current_line[i + 50] == '1')
                        {
                            three_one.BackColor = textBox1.BackColor;
                            three_one.BackColor = pictureBox13.BackColor;
                            //Console.WriteLine("3 1");
                        }
                        else if (current_line[i + 47] == '3' && current_line[i + 50] == '2')
                        {
                            three_two.BackColor = textBox1.BackColor;
                            three_two.BackColor = pictureBox13.BackColor;
                           // Console.WriteLine("3 2");
                        }
                        else if (current_line[i + 47] == '3' && current_line[i + 50] == '3')
                        {
                            three_three.BackColor = textBox1.BackColor;
                            three_three.BackColor = pictureBox13.BackColor;
                        }
                        else
                        {
                            Console.WriteLine(current_line[i + 47].ToString(), current_line[i + 50].ToString());
                           // Console.WriteLine("none of them");
                        }

                }
                else if (current_line[i] == 'B' && current_line[i + 1] == 'e' && current_line[i + 2] == 'f' && current_line[i + 3] == 'o' && current_line[i + 4] == 'r' && current_line[i + 5] == 'e' && current_line[i + 6] == ' ' && current_line[i + 7] == 'd' && current_line[i + 8] == 'd')
                {
                    if (first_iteration == 0)
                    {
                        ClosedList.Text = string.Empty;
                    }

                    ClosedList.AppendText("(");//(
                    ClosedList.AppendText(current_line[i + 46].ToString());//num
                    ClosedList.AppendText(",");//. ,
                    ClosedList.AppendText(current_line[i + 48].ToString());//num
                    ClosedList.AppendText(")");//)
                    ClosedList.AppendText(Environment.NewLine);
                    first_iteration++;
                    After_Before_Flag++;
                }
            }

            if(After_Before_Flag == 0)
            { 
                Console.WriteLine("first_iteration = 0;");
                first_iteration = 0;
                After_Before_Flag = 0;
             }
        }

        void Color_theCurrentcell()
            {
                char thread;
                thread = ExtractThread();
                int i_cur = Extract_i();
                int j_cur = Extract_j();

               // for (int i = 0; i < current_line.Length; i++)
                //{
                    //if (current_line[i] == ':' && current_line[i + 1] == ' ' && current_line[i + 2] == ' ' && current_line[i + 3] == 'C')
                    //{
                        switch (thread)
                        {
                            case '0':
                                Console.WriteLine("Thread 0");
                                if (i_cur == '0' && j_cur == '0')
                                {
                                    zero_zero.BackColor = pictureBox2.BackColor;
                                    thread00.Clear();
                                    thread00.AppendText(thread.ToString());
                                    Console.WriteLine("0 0");
                                }
                                else if (i_cur == '0' && j_cur == '1')
                                {
                                    zero_one.BackColor = pictureBox2.BackColor;
                                    thread01.Clear();
                                    thread01.AppendText(thread.ToString());
                                    Console.WriteLine("0 1");
                                }
                                else if (i_cur == '0' && j_cur == '2')
                                {
                                    zero_two.BackColor = pictureBox2.BackColor;
                                    thread02.Clear();
                                    thread02.AppendText(thread.ToString());
                                    Console.WriteLine("0 2");
                                }
                                else if (i_cur == '0' && j_cur == '3')
                                {
                                    zero_three.BackColor = pictureBox2.BackColor;
                                    thread03.Clear();
                                    thread03.AppendText(thread.ToString());
                                    Console.WriteLine("0 3");
                                }
                                else if (i_cur == '1' && j_cur == '0')
                                {
                                    one_zero.BackColor = pictureBox2.BackColor;
                                    thread10.Clear();
                                    thread10.AppendText(thread.ToString());
                                    Console.WriteLine("1 0");
                                }
                                else if (i_cur == '1' && j_cur == '1')
                                {
                                    one_one.BackColor = pictureBox2.BackColor;
                                    thread11.Clear();
                                    thread11.AppendText(thread.ToString());
                                    Console.WriteLine("1 1");
                                }
                                else if (i_cur == '1' && j_cur == '2')
                                {
                                    one_two.BackColor = pictureBox2.BackColor;
                                    thread12.Clear();
                                    thread12.AppendText(thread.ToString());
                                    Console.WriteLine("1 2");
                                }
                                else if (i_cur == '1' && j_cur == '3')
                                {
                                    one_three.BackColor = pictureBox2.BackColor;
                                    thread13.Clear();
                                    thread13.AppendText(thread.ToString());
                                    Console.WriteLine("1 3");
                                }
                                else if (i_cur == '2' && j_cur == '0')
                                {
                                    two_zero.BackColor = pictureBox2.BackColor;
                                    thread20.Clear();
                                    thread20.AppendText(thread.ToString());
                                    Console.WriteLine("2 0");
                                }
                                else if (i_cur == '2' && j_cur == '1')
                                {
                                    two_one.BackColor = pictureBox2.BackColor;
                                    thread21.Clear();
                                    thread21.AppendText(thread.ToString());
                                    Console.WriteLine("2 1");
                                }
                                else if (i_cur == '2' && j_cur == '2')
                                {
                                    two_two.BackColor = pictureBox2.BackColor;
                                    thread22.Clear();
                                    thread22.AppendText(thread.ToString());
                                    Console.WriteLine("2 2");
                                }
                                else if (i_cur == '2' && j_cur == '3')
                                {
                                    two_three.BackColor = pictureBox2.BackColor;
                                    thread23.Clear();
                                    thread23.AppendText(thread.ToString());
                                    Console.WriteLine("2 3");
                                }
                                else if (i_cur == '3' && j_cur == '0')
                                {
                                    three_zero.BackColor = pictureBox2.BackColor;
                                    thread30.Clear();
                                    thread30.AppendText(thread.ToString());
                                    Console.WriteLine("3 0");
                                }
                                else if (i_cur == '3' && j_cur == '1')
                                {
                                    three_one.BackColor = pictureBox2.BackColor;
                                    thread31.Clear();
                                    thread31.AppendText(thread.ToString());
                                    Console.WriteLine("3 1");
                                }
                                else if (i_cur == '3' && j_cur == '2')
                                {
                                    three_two.BackColor = pictureBox2.BackColor;
                                    thread32.Clear();
                                    thread32.AppendText(thread.ToString());
                                    Console.WriteLine("3 2");
                                }
                                else if (i_cur == '3' && j_cur == '3')
                                {
                                    three_three.BackColor = pictureBox2.BackColor;
                                    thread33.Clear();
                                    thread33.AppendText(thread.ToString());
                                    Console.WriteLine("3 3");
                                }
                                else
                                {
                                    Console.WriteLine(i_cur.ToString(), j_cur.ToString());
                                    Console.WriteLine("none of them");
                                }
                                break;
                            case '1':
                                Console.WriteLine("Case 1");
                                if (i_cur == '0' && j_cur == '0')
                                {
                                    zero_zero.BackColor = pictureBox3.BackColor;
                                    thread00.Clear();
                                    thread00.AppendText(thread.ToString());
                                    Console.WriteLine("0 0");
                                }
                                else if (i_cur == '0' && j_cur == '1')
                                {
                                    zero_one.BackColor = pictureBox3.BackColor;
                                    thread01.Clear();
                                    thread01.AppendText(thread.ToString());
                                    Console.WriteLine("0 1");
                                }
                                else if (i_cur == '0' && j_cur == '2')
                                {
                                    zero_two.BackColor = pictureBox3.BackColor;
                                    thread02.Clear();
                                    thread02.AppendText(thread.ToString());
                                    Console.WriteLine("0 2");
                                }
                                else if (i_cur == '0' && j_cur == '3')
                                {
                                    zero_three.BackColor = pictureBox3.BackColor;
                                    thread03.Clear();
                                    thread03.AppendText(thread.ToString());
                                    Console.WriteLine("0 3");
                                }
                                else if (i_cur == '1' && j_cur == '0')
                                {
                                    one_zero.BackColor = pictureBox3.BackColor;
                                    thread10.Clear();
                                    thread10.AppendText(thread.ToString());
                                    Console.WriteLine("1 0");
                                }
                                else if (i_cur == '1' && j_cur == '1')
                                {
                                    one_one.BackColor = pictureBox3.BackColor;
                                    thread11.Clear();
                                    thread11.AppendText(thread.ToString());
                                    Console.WriteLine("1 1");
                                }
                                else if (i_cur == '1' && j_cur == '2')
                                {
                                    one_two.BackColor = pictureBox3.BackColor;
                                    thread12.Clear();
                                    thread12.AppendText(thread.ToString());
                                    Console.WriteLine("1 2");
                                }
                                else if (i_cur == '1' && j_cur == '3')
                                {
                                    one_three.BackColor = pictureBox3.BackColor;
                                    thread13.Clear();
                                    thread13.AppendText(thread.ToString());
                                    Console.WriteLine("1 3");
                                }
                                else if (i_cur == '2' && j_cur == '0')
                                {
                                    two_zero.BackColor = pictureBox3.BackColor;
                                    thread20.Clear();
                                    thread20.AppendText(thread.ToString());
                                    Console.WriteLine("2 0");
                                }
                                else if (i_cur == '2' && j_cur == '1')
                                {
                                    two_one.BackColor = pictureBox3.BackColor;
                                    thread21.Clear();
                                    thread21.AppendText(thread.ToString());
                                    Console.WriteLine("2 1");
                                }
                                else if (i_cur == '2' && j_cur == '2')
                                {
                                    two_two.BackColor = pictureBox3.BackColor;
                                    thread22.Clear();
                                    thread22.AppendText(thread.ToString());
                                    Console.WriteLine("2 2");
                                }
                                else if (i_cur == '2' && j_cur == '3')
                                {
                                    two_three.BackColor = pictureBox3.BackColor;
                                    thread23.Clear();
                                    thread23.AppendText(thread.ToString());
                                    Console.WriteLine("2 3");
                                }
                                else if (i_cur == '3' && j_cur == '0')
                                {
                                    three_zero.BackColor = pictureBox3.BackColor;
                                    thread30.Clear();
                                    thread30.AppendText(thread.ToString());
                                    Console.WriteLine("3 0");
                                }
                                else if (i_cur == '3' && j_cur == '1')
                                {
                                    three_one.BackColor = pictureBox3.BackColor;
                                    thread31.Clear();
                                    thread31.AppendText(thread.ToString());
                                    Console.WriteLine("3 1");
                                }
                                else if (i_cur == '3' && j_cur == '2')
                                {
                                    three_two.BackColor = pictureBox3.BackColor;
                                    thread32.Clear();
                                    thread32.AppendText(thread.ToString());
                                    Console.WriteLine("3 2");
                                }
                                else if (i_cur == '3' && j_cur == '3')
                                {
                                    three_three.BackColor = pictureBox3.BackColor;
                                    thread33.Clear();
                                    thread33.AppendText(thread.ToString());
                                    Console.WriteLine("3 3");
                                }
                                else
                                {
                                    Console.WriteLine(i_cur.ToString(), j_cur.ToString());
                                    Console.WriteLine("none of them");
                                }
                                break;
                            case '2':
                                Console.WriteLine("Case 2");
                                if (i_cur == '0' && j_cur == '0')
                                {
                                    zero_zero.BackColor = pictureBox7.BackColor;
                                    thread00.Clear();
                                    thread00.AppendText(thread.ToString());
                                    Console.WriteLine("0 0");
                                }
                                else if (i_cur == '0' && j_cur == '1')
                                {
                                    zero_one.BackColor = pictureBox7.BackColor;
                                    thread01.Clear();
                                    thread01.AppendText(thread.ToString());
                                    Console.WriteLine("0 1");
                                }
                                else if (i_cur == '0' && j_cur == '2')
                                {
                                    zero_two.BackColor = pictureBox7.BackColor;
                                    thread02.Clear();
                                    thread02.AppendText(thread.ToString());
                                    Console.WriteLine("0 2");
                                }
                                else if (i_cur == '0' && j_cur == '3')
                                {
                                    zero_three.BackColor = pictureBox7.BackColor;
                                    thread03.Clear();
                                    thread03.AppendText(thread.ToString());
                                    Console.WriteLine("0 3");
                                }
                                else if (i_cur == '1' && j_cur == '0')
                                {
                                    one_zero.BackColor = pictureBox7.BackColor;
                                    thread10.Clear();
                                    thread10.AppendText(thread.ToString());
                                    Console.WriteLine("1 0");
                                }
                                else if (i_cur == '1' && j_cur == '1')
                                {
                                    one_one.BackColor = pictureBox7.BackColor;
                                    thread11.Clear();
                                    thread11.AppendText(thread.ToString());
                                    Console.WriteLine("1 1");
                                }
                                else if (i_cur == '1' && j_cur == '2')
                                {
                                    one_two.BackColor = pictureBox7.BackColor;
                                    thread12.Clear();
                                    thread12.AppendText(thread.ToString());
                                    Console.WriteLine("1 2");
                                }
                                else if (i_cur == '1' && j_cur == '3')
                                {
                                    one_three.BackColor = pictureBox7.BackColor;
                                    thread13.Clear();
                                    thread13.AppendText(thread.ToString());
                                    Console.WriteLine("1 3");
                                }
                                else if (i_cur == '2' && j_cur == '0')
                                {
                                    two_zero.BackColor = pictureBox7.BackColor;
                                    thread20.Clear();
                                    thread20.AppendText(thread.ToString()); ;
                                    Console.WriteLine("2 0");
                                }
                                else if (i_cur == '2' && j_cur == '1')
                                {
                                    two_one.BackColor = pictureBox7.BackColor;
                                    thread21.Clear();
                                    thread21.AppendText(thread.ToString());
                                    Console.WriteLine("2 1");
                                }
                                else if (i_cur == '2' && j_cur == '2')
                                {
                                    two_two.BackColor = pictureBox7.BackColor;
                                    thread22.Clear();
                                    thread22.AppendText(thread.ToString());
                                    Console.WriteLine("2 2");
                                }
                                else if (i_cur == '2' && j_cur == '3')
                                {
                                    two_three.BackColor = pictureBox7.BackColor;
                                    thread23.Clear();
                                    thread23.AppendText(thread.ToString());
                                    Console.WriteLine("2 3");
                                }
                                else if (i_cur == '3' && j_cur == '0')
                                {
                                    three_zero.BackColor = pictureBox7.BackColor;
                                    thread30.Clear();
                                    thread30.AppendText(thread.ToString());
                                    Console.WriteLine("3 0");
                                }
                                else if (i_cur == '3' && j_cur == '1')
                                {
                                    three_one.BackColor = pictureBox7.BackColor;
                                    thread31.Clear();
                                    thread31.AppendText(thread.ToString());
                                    Console.WriteLine("3 1");
                                }
                                else if (i_cur == '3' && j_cur == '2')
                                {
                                    three_two.BackColor = pictureBox7.BackColor;
                                    thread32.Clear();
                                    thread32.AppendText(thread.ToString());
                                    Console.WriteLine("3 2");
                                }
                                else if (i_cur == '3' && j_cur == '3')
                                {
                                    three_three.BackColor = pictureBox7.BackColor;
                                    thread33.Clear();
                                    thread33.AppendText(thread.ToString());
                                    Console.WriteLine("3 3");
                                }
                                else
                                {
                                    Console.WriteLine(i_cur.ToString(), j_cur.ToString());
                                    Console.WriteLine("none of them");
                                }
                                break;
                            case '3':
                                Console.WriteLine("Case 3");
                                if (i_cur == '0' && j_cur == '0')
                                {
                                    zero_zero.BackColor = pictureBox5.BackColor;
                                    thread00.Clear();
                                    thread00.AppendText(thread.ToString());
                                    Console.WriteLine("0 0");
                                }
                                else if (i_cur == '0' && j_cur == '1')
                                {
                                    zero_one.BackColor = pictureBox5.BackColor;
                                    thread01.Clear();
                                    thread01.AppendText(thread.ToString());
                                    Console.WriteLine("0 1");
                                }
                                else if (i_cur == '0' && j_cur == '2')
                                {
                                    zero_two.BackColor = pictureBox5.BackColor;
                                    thread02.Clear();
                                    thread02.AppendText(thread.ToString());
                                    Console.WriteLine("0 2");
                                }
                                else if (i_cur == '0' && j_cur == '3')
                                {
                                    zero_three.BackColor = pictureBox5.BackColor;
                                    thread03.Clear();
                                    thread03.AppendText(thread.ToString());
                                    Console.WriteLine("0 3");
                                }
                                else if (i_cur == '1' && j_cur == '0')
                                {
                                    one_zero.BackColor = pictureBox5.BackColor;
                                    thread10.Clear();
                                    thread10.AppendText(thread.ToString());
                                    Console.WriteLine("1 0");
                                }
                                else if (i_cur == '1' && j_cur == '1')
                                {
                                    one_one.BackColor = pictureBox5.BackColor;
                                    thread11.Clear();
                                    thread11.AppendText("5");
                                    Console.WriteLine("1 1");
                                }
                                else if (i_cur == '1' && j_cur == '2')
                                {
                                    one_two.BackColor = pictureBox5.BackColor;
                                    thread12.Clear();
                                    thread12.AppendText(thread.ToString());
                                    Console.WriteLine("1 2");
                                }
                                else if (i_cur == '1' && j_cur == '3')
                                {
                                    one_three.BackColor = pictureBox5.BackColor;
                                    thread13.Clear();
                                    thread13.AppendText(thread.ToString());
                                    Console.WriteLine("1 3");
                                }
                                else if (i_cur == '2' && j_cur == '0')
                                {
                                    two_zero.BackColor = pictureBox5.BackColor;
                                    thread20.Clear();
                                    thread20.AppendText(thread.ToString());
                                    Console.WriteLine("2 0");
                                }
                                else if (i_cur == '2' && j_cur == '1')
                                {
                                    two_one.BackColor = pictureBox5.BackColor;
                                    thread21.Clear();
                                    thread21.AppendText(thread.ToString());
                                    Console.WriteLine("2 1");
                                }
                                else if (i_cur == '2' && j_cur == '2')
                                {
                                    two_two.BackColor = pictureBox5.BackColor;
                                    thread22.Clear();
                                    thread22.AppendText(thread.ToString());
                                    Console.WriteLine("2 2");
                                }
                                else if (i_cur == '2' && j_cur == '3')
                                {
                                    two_three.BackColor = pictureBox5.BackColor;
                                    thread23.Clear();
                                    thread23.AppendText(thread.ToString());
                                    Console.WriteLine("2 3");
                                }
                                else if (i_cur == '3' && j_cur == '0')
                                {
                                    three_zero.BackColor = pictureBox5.BackColor;
                                    thread30.Clear();
                                    thread30.AppendText(thread.ToString());
                                    Console.WriteLine("3 0");
                                }
                                else if (i_cur == '3' && j_cur == '1')
                                {
                                    three_one.BackColor = pictureBox5.BackColor;
                                    thread31.Clear();
                                    thread31.AppendText(thread.ToString());
                                    Console.WriteLine("3 1");
                                }
                                else if (i_cur == '3' && j_cur == '2')
                                {
                                    three_two.BackColor = pictureBox5.BackColor;
                                    thread32.Clear();
                                    thread32.AppendText(thread.ToString());
                                    Console.WriteLine("3 2");
                                }
                                else if (i_cur == '3' && j_cur == '3')
                                {
                                    three_three.BackColor = pictureBox5.BackColor;
                                    thread33.Clear();
                                    thread33.AppendText(thread.ToString());
                                    Console.WriteLine("3 3");
                                }
                                else
                                {
                                    Console.WriteLine(i_cur.ToString(), j_cur.ToString());
                                    Console.WriteLine("none of them");
                                }
                                break;
                            case '4':
                                if (i_cur == '0' && j_cur == '0')
                                {
                                    zero_zero.BackColor = pictureBox11.BackColor;
                                    thread00.Clear();
                                    thread00.AppendText(thread.ToString());
                                    Console.WriteLine("0 0");
                                }
                                else if (i_cur == '0' && j_cur == '1')
                                {
                                    zero_one.BackColor = pictureBox11.BackColor;
                                    thread01.Clear();
                                    thread01.AppendText(thread.ToString());
                                    Console.WriteLine("0 1");
                                }
                                else if (i_cur == '0' && j_cur == '2')
                                {
                                    zero_two.BackColor = pictureBox11.BackColor;
                                    thread02.Clear();
                                    thread02.AppendText(thread.ToString());
                                    Console.WriteLine("0 2");
                                }
                                else if (i_cur == '0' && j_cur == '3')
                                {
                                    zero_three.BackColor = pictureBox11.BackColor;
                                    thread03.Clear();
                                    thread03.AppendText(thread.ToString());
                                    Console.WriteLine("0 3");
                                }
                                else if (i_cur == '1' && j_cur == '0')
                                {
                                    one_zero.BackColor = pictureBox11.BackColor;
                                    thread10.Clear();
                                    thread10.AppendText(thread.ToString());
                                    Console.WriteLine("1 0");
                                }
                                else if (i_cur == '1' && j_cur == '1')
                                {
                                    one_one.BackColor = pictureBox11.BackColor;
                                    thread11.Clear();
                                    thread11.AppendText(thread.ToString());
                                    Console.WriteLine("1 1");
                                }
                                else if (i_cur == '1' && j_cur == '2')
                                {
                                    one_two.BackColor = pictureBox11.BackColor;
                                    thread12.Clear();
                                    thread12.AppendText(thread.ToString());
                                    Console.WriteLine("1 2");
                                }
                                else if (i_cur == '1' && j_cur == '3')
                                {
                                    one_three.BackColor = pictureBox11.BackColor;
                                    thread13.Clear();
                                    thread13.AppendText(thread.ToString());
                                    Console.WriteLine("1 3");
                                }
                                else if (i_cur == '2' && j_cur == '0')
                                {
                                    two_zero.BackColor = pictureBox11.BackColor;
                                    thread20.Clear();
                                    thread20.AppendText(thread.ToString());
                                    Console.WriteLine("2 0");
                                }
                                else if (i_cur == '2' && j_cur == '1')
                                {
                                    two_one.BackColor = pictureBox11.BackColor;
                                    thread21.Clear();
                                    thread21.AppendText(thread.ToString());
                                    Console.WriteLine("2 1");
                                }
                                else if (i_cur == '2' && j_cur == '2')
                                {
                                    two_two.BackColor = pictureBox11.BackColor;
                                    thread22.Clear();
                                    thread22.AppendText(thread.ToString());
                                    Console.WriteLine("2 2");
                                }
                                else if (i_cur == '2' && j_cur == '3')
                                {
                                    two_three.BackColor = pictureBox11.BackColor;
                                    thread23.Clear();
                                    thread23.AppendText(thread.ToString());
                                    Console.WriteLine("2 3");
                                }
                                else if (i_cur == '3' && j_cur == '0')
                                {
                                    three_zero.BackColor = pictureBox11.BackColor;
                                    thread30.Clear();
                                    thread30.AppendText(thread.ToString());
                                    Console.WriteLine("3 0");
                                }
                                else if (i_cur == '3' && j_cur == '1')
                                {
                                    three_one.BackColor = pictureBox11.BackColor;
                                    thread31.Clear();
                                    thread31.AppendText(thread.ToString());
                                    Console.WriteLine("3 1");
                                }
                                else if (i_cur == '3' && j_cur == '2')
                                {
                                    three_two.BackColor = pictureBox11.BackColor;
                                    thread32.Clear();
                                    thread32.AppendText(thread.ToString());
                                    Console.WriteLine("3 2");
                                }
                                else if (i_cur == '3' && j_cur == '3')
                                {
                                    three_three.BackColor = pictureBox11.BackColor;
                                    thread33.Clear();
                                    thread33.AppendText(thread.ToString());
                                    Console.WriteLine("3 3");
                                }
                                else
                                {
                                    Console.WriteLine(i_cur.ToString(), j_cur.ToString());
                                    Console.WriteLine("none of them");
                                }
                                Console.WriteLine("Case 4");
                                break;
                            case '5':
                                if (i_cur == '0' && j_cur == '0')
                                {
                                    zero_zero.BackColor = pictureBox9.BackColor;
                                    thread00.Clear();
                                    thread00.AppendText(thread.ToString());
                                    Console.WriteLine("0 0");
                                }
                                else if (i_cur == '0' && j_cur == '1')
                                {
                                    zero_one.BackColor = pictureBox9.BackColor;
                                    thread01.Clear();
                                    thread01.AppendText(thread.ToString());
                                    Console.WriteLine("0 1");
                                }
                                else if (i_cur == '0' && j_cur == '2')
                                {
                                    zero_two.BackColor = pictureBox9.BackColor;
                                    thread02.Clear();
                                    thread02.AppendText(thread.ToString());
                                    Console.WriteLine("0 2");
                                }
                                else if (i_cur == '0' && j_cur == '3')
                                {
                                    zero_three.BackColor = pictureBox9.BackColor;
                                    thread03.Clear();
                                    thread03.AppendText(thread.ToString());
                                    Console.WriteLine("0 3");
                                }
                                else if (i_cur == '1' && j_cur == '0')
                                {
                                    one_zero.BackColor = pictureBox9.BackColor;
                                    thread10.Clear();
                                    thread10.AppendText(thread.ToString());
                                    Console.WriteLine("1 0");
                                }
                                else if (i_cur == '1' && j_cur == '1')
                                {
                                    one_one.BackColor = pictureBox9.BackColor;
                                    thread11.Clear();
                                    thread11.AppendText(thread.ToString());
                                    Console.WriteLine("1 1");

                                }
                                else if (i_cur == '1' && j_cur == '2')
                                {
                                    one_two.BackColor = pictureBox9.BackColor;
                                    thread12.Clear();
                                    thread12.AppendText(thread.ToString());
                                    Console.WriteLine("1 2");
                                }
                                else if (i_cur == '1' && j_cur == '3')
                                {
                                    one_three.BackColor = pictureBox9.BackColor;
                                    thread13.Clear();
                                    thread13.AppendText(thread.ToString());

                                    Console.WriteLine("1 3");
                                }
                                else if (i_cur == '2' && j_cur == '0')
                                {
                                    two_zero.BackColor = pictureBox9.BackColor;
                                    thread20.Clear();
                                    thread20.AppendText(thread.ToString());
                                    Console.WriteLine("2 0");
                                }
                                else if (i_cur == '2' && j_cur == '1')
                                {
                                    two_one.BackColor = pictureBox9.BackColor;
                                    thread21.Clear();
                                    thread21.AppendText(thread.ToString());
                                    Console.WriteLine("2 1");
                                }
                                else if (i_cur == '2' && j_cur == '2')
                                {
                                    two_two.BackColor = pictureBox9.BackColor;
                                    thread22.Clear();
                                    thread22.AppendText(thread.ToString());
                                    Console.WriteLine("2 2");
                                }
                                else if (i_cur == '2' && j_cur == '3')
                                {
                                    two_three.BackColor = pictureBox9.BackColor;
                                    thread23.Clear();
                                    thread23.AppendText(thread.ToString());
                                    Console.WriteLine("2 3");
                                }
                                else if (i_cur == '3' && j_cur == '0')
                                {
                                    three_zero.BackColor = pictureBox9.BackColor;
                                    thread30.Clear();
                                    thread30.AppendText(thread.ToString());
                                    Console.WriteLine("3 0");
                                }
                                else if (i_cur == '3' && j_cur == '1')
                                {
                                    three_one.BackColor = pictureBox9.BackColor;
                                    thread31.Clear();
                                    thread31.AppendText(thread.ToString());
                                    Console.WriteLine("3 1");
                                }
                                else if (i_cur == '3' && j_cur == '2')
                                {
                                    three_two.BackColor = pictureBox9.BackColor;
                                    thread32.Clear();
                                    thread32.AppendText(thread.ToString());
                                    Console.WriteLine("3 2");
                                }
                                else if (i_cur == '3' && j_cur == '3')
                                {
                                    three_three.BackColor = pictureBox9.BackColor;
                                    thread33.Clear();
                                    thread33.AppendText(thread.ToString());
                                    Console.WriteLine("3 3");
                                }
                                else
                                {
                                    Console.WriteLine(i_cur.ToString(), j_cur.ToString());
                                    Console.WriteLine("none of them");
                                }
                                Console.WriteLine("Case 5");
                                break;
                            default:
                                Console.WriteLine("Default case");
                                break;
                        }
                   // }//
               // }//

            }

        void Thread_box()
        {
            for (int i = 0; i < current_line.Length; i++)
            {
                //Console.WriteLine("!!!");
                if (current_line[i] == ':' && current_line[i + 1] == ' ' && current_line[i + 2] == ' ' && current_line[i + 3] == 'C' && current_line[i + 4] == 'u' && current_line[i + 5] == 'r')
                {
                    //Console.WriteLine("!!!AAA!!!");
                    if (current_line[i + 20] == '0' && current_line[i + 22] == '0')
                    {
                        thread00.Text = string.Empty;
                        thread00.AppendText(current_line[i - 12].ToString());
                    }
                    else if (current_line[i + 20] == '0' && current_line[i - 2] == '1')
                    {
                        thread01.Text = string.Empty;
                        thread01.AppendText(current_line[i - 12].ToString());
                        Console.WriteLine("0 1");
                    }
                    else if (current_line[i + 20] == '0' && current_line[i + 22] == '2')
                    {
                        thread02.Text = string.Empty;
                        thread02.AppendText(current_line[i - 12].ToString());
                        Console.WriteLine("0 2");
                    }
                    else if (current_line[i + 20] == '0' && current_line[i + 22] == '3')
                    {
                        thread03.Text = string.Empty;
                        thread03.AppendText(current_line[i - 12].ToString());
                        Console.WriteLine("0 3");
                    }
                    else if (current_line[i + 20] == '1' && current_line[i + 22] == '0')
                    {
                        thread10.Text = string.Empty;
                        thread10.AppendText(current_line[i - 12].ToString());
                        Console.WriteLine("1 0");
                    }
                    else if (current_line[i + 20] == '1' && current_line[i + 22] == '1')
                    {
                        thread11.Text = string.Empty;
                        thread11.AppendText(current_line[i - 12].ToString());
                        Console.WriteLine("AAAA!!!!");
                    }
                    else if (current_line[i + 20] == '1' && current_line[i - 2] == '2')
                    {
                        thread12.Text = string.Empty;
                        thread12.AppendText(current_line[i - 12].ToString());
                        Console.WriteLine("1 2");
                    }
                    else if (current_line[i + 20] == '1' && current_line[i + 22] == '3')
                    {
                        thread13.Text = string.Empty;
                        thread13.AppendText(current_line[i - 12].ToString());
                        Console.WriteLine("1 3");
                    }
                    else if (current_line[i + 20] == '2' && current_line[i + 22] == '0')
                    {
                        thread20.Text = string.Empty;
                        thread20.AppendText(current_line[i - 12].ToString());
                        Console.WriteLine("2 0");
                    }
                    else if (current_line[i + 20] == '2' && current_line[i + 22] == '1')
                    {
                        thread21.Text = string.Empty;
                        thread21.AppendText(current_line[i - 12].ToString());
                        Console.WriteLine("2 1");
                    }
                    else if (current_line[i + 20] == '2' && current_line[i + 22] == '2')
                    {
                        thread22.Text = string.Empty;
                        thread22.AppendText(current_line[i - 12].ToString());
                        Console.WriteLine("2 2");
                    }
                    else if (current_line[i + 20] == '2' && current_line[i + 22] == '3')
                    {
                        thread23.Text = string.Empty;
                        thread23.AppendText(current_line[i - 12].ToString());
                        Console.WriteLine("2 3");
                    }
                    else if (current_line[i + 20] == '3' && current_line[i + 22] == '0')
                    {
                        thread30.Text = string.Empty;
                        thread30.AppendText(current_line[i - 12].ToString());
                        Console.WriteLine("3 0");
                    }
                    else if (current_line[i + 20] == '3' && current_line[i + 22] == '1')
                    {
                        thread31.Text = string.Empty;
                        thread31.AppendText(current_line[i - 12].ToString());
                        Console.WriteLine("3 1");
                    }
                    else if (current_line[i + 20] == '3' && current_line[i + 22] == '2')
                    {
                        thread32.Text = string.Empty;
                        thread32.AppendText(current_line[i - 12].ToString());
                        Console.WriteLine("3 2");
                    }
                    else if (current_line[i + 20] == '3' && current_line[i + 22] == '3')
                    {
                        thread33.Text = string.Empty;
                        thread33.AppendText(current_line[i - 12].ToString());
                        Console.WriteLine("3 3");
                    }
                    else
                    {
                        Console.WriteLine(current_line[i - 5].ToString(), current_line[i - 2].ToString());
                        Console.WriteLine("none of them");
                    }
                }
            }
        }
        void f_extract()
        {
             for (int i = 0; i < current_line.Length; i++)
             {
                if (current_line[i] == '.' && current_line[i + 1] == 'f' && current_line[i + 2] == ' ' && current_line[i + 3] == '=')
                {
                            char[] current_line_c = current_line.ToCharArray();
                            Console.WriteLine("Thread 0");
                            if (current_line[i - 5] == '0' && current_line[i -2] == '0')
                            {
                                f00.Text = string.Empty;
                                f00.AppendText(current_line[i + 5].ToString());
                                f00.AppendText(current_line[i + 6].ToString());
                                f00.AppendText(current_line[i + 7].ToString());
                                Console.WriteLine("0 0");
                            }
                            else if (current_line[i - 5] == '0' && current_line[i - 2] == '1')
                            {
                                f01.Text = string.Empty;
                                f01.AppendText(current_line[i + 5].ToString());
                                f01.AppendText(current_line[i + 6].ToString());
                                f01.AppendText(current_line[i + 7].ToString());
                                Console.WriteLine("0 1");
                            }
                            else if (current_line[i - 5] == '0' && current_line[i - 2] == '2')
                            {
                                f02.Text = string.Empty;
                                f02.AppendText(current_line[i + 5].ToString());
                                f02.AppendText(current_line[i + 6].ToString());
                                f02.AppendText(current_line[i + 7].ToString());
                                Console.WriteLine("0 2");
                            }
                            else if (current_line[i - 5] == '0' && current_line[i - 2] == '3')
                            {
                                f03.Text = string.Empty;
                                f03.AppendText(current_line[i + 5].ToString());
                                f03.AppendText(current_line[i + 6].ToString());
                                f03.AppendText(current_line[i + 7].ToString());
                                Console.WriteLine("0 3");
                            }
                            else if (current_line[i - 5] == '1' && current_line[i - 2] == '0')
                            {
                                f10.Text = string.Empty;
                                f10.AppendText(current_line[i + 5].ToString());
                                f10.AppendText(current_line[i + 6].ToString());
                                f10.AppendText(current_line[i + 7].ToString());
                                Console.WriteLine("1 0");
                            }
                            else if (current_line[i - 5] == '1' && current_line[i - 2] == '1')
                            {
                                f11.Text = string.Empty;
                                f11.AppendText(current_line[i + 5].ToString());
                                f11.AppendText(current_line[i + 6].ToString());
                                f11.AppendText(current_line[i + 7].ToString());
                                Console.WriteLine("1 1");
                            }
                            else if (current_line[i - 5] == '1' && current_line[i - 2] == '2')
                            {
                                f12.Text = string.Empty;
                                f12.AppendText(current_line[i + 5].ToString());
                                f12.AppendText(current_line[i + 6].ToString());
                                f12.AppendText(current_line[i + 7].ToString());
                                Console.WriteLine("1 2");
                            }
                            else if (current_line[i - 5] == '1' && current_line[i - 2] == '3')
                            {
                                f13.Text = string.Empty;
                                f13.AppendText(current_line[i + 5].ToString());
                                f13.AppendText(current_line[i + 6].ToString());
                                f13.AppendText(current_line[i + 7].ToString());
                                Console.WriteLine("1 3");
                            }
                            else if (current_line[i - 5] == '2' && current_line[i - 2] == '0')
                            {
                                f20.Text = string.Empty;
                                f20.AppendText(current_line[i + 5].ToString());
                                f20.AppendText(current_line[i + 6].ToString());
                                f20.AppendText(current_line[i + 7].ToString());
                                Console.WriteLine("2 0");
                            }
                            else if (current_line[i - 5] == '2' && current_line[i - 2] == '1')
                            {
                                f21.Text = string.Empty;
                                f21.AppendText(current_line[i + 5].ToString());
                                f21.AppendText(current_line[i + 6].ToString());
                                f21.AppendText(current_line[i + 7].ToString());
                                Console.WriteLine("2 1");
                            }
                            else if (current_line[i - 5] == '2' && current_line[i - 2] == '2')
                            {
                                f22.Text = string.Empty;
                                f22.AppendText(current_line[i + 5].ToString());
                                f22.AppendText(current_line[i + 6].ToString());
                                f22.AppendText(current_line[i + 7].ToString());
                                Console.WriteLine("2 2");
                            }
                            else if (current_line[i - 5] == '2' && current_line[i - 2] == '3')
                            {
                                f23.Text = string.Empty;
                                f23.AppendText(current_line[i + 5].ToString());
                                f23.AppendText(current_line[i + 6].ToString());
                                f23.AppendText(current_line[i + 7].ToString());
                                Console.WriteLine("2 3");
                            }
                            else if (current_line[i - 5] == '3' && current_line[i - 2] == '0')
                            {
                                f30.Text = string.Empty;
                                f30.AppendText(current_line[i + 5].ToString());
                                f30.AppendText(current_line[i + 6].ToString());
                                f30.AppendText(current_line[i + 7].ToString());
                                Console.WriteLine("3 0");
                            }
                            else if (current_line[i - 5] == '3' && current_line[i - 2] == '1')
                            {
                                f31.Text = string.Empty;
                                f31.AppendText(current_line[i + 5].ToString());
                                f31.AppendText(current_line[i + 6].ToString());
                                f31.AppendText(current_line[i + 7].ToString());
                                Console.WriteLine("3 1");
                            }
                            else if (current_line[i - 5] == '3' && current_line[i - 2] == '2')
                            {
                                f32.Text = string.Empty;
                                f32.AppendText(current_line[i + 5].ToString());
                                f32.AppendText(current_line[i + 6].ToString());
                                f32.AppendText(current_line[i + 7].ToString());
                                Console.WriteLine("3 2");
                            }
                            else if (current_line[i - 5] == '3' && current_line[i - 2] == '3')
                            {
                                f33.Text = string.Empty;
                                f33.AppendText(current_line[i + 5].ToString());
                                f33.AppendText(current_line[i + 6].ToString());
                                f33.AppendText(current_line[i + 7].ToString());
                                Console.WriteLine("3 3");
                            }
                            else
                            {
                                Console.WriteLine(current_line[i - 5].ToString(), current_line[i - 2].ToString());
                                Console.WriteLine("none of them");
                            } 
                }
            }

        }

        void parents_extract()
        {
            for (int i = 0; i < current_line.Length; i++)
            {
                if (current_line[i] == '.' && current_line[i + 1] == 'p' && current_line[i + 2] == 'a' && current_line[i + 3] == 'r')
                {
                    if (current_line[i - 5] == '0' && current_line[i - 2] == '0')
                    {
                        paremt_i00.Text = string.Empty;
                        paremt_i00.AppendText(current_line[i + 24].ToString());

                        paremt_j00.Text = string.Empty;
                        paremt_j00.AppendText(current_line[i + 26].ToString());
                        Console.WriteLine("0 0");
                    }
                    else if (current_line[i - 5] == '0' && current_line[i - 2] == '1')
                    {
                        paremt_i01.Text = string.Empty;
                        paremt_i01.AppendText(current_line[i + 24].ToString());

                        paremt_j01.Text = string.Empty;
                        paremt_j01.AppendText(current_line[i + 26].ToString());
                        Console.WriteLine("0 1");
                    }
                    else if (current_line[i - 5] == '0' && current_line[i - 2] == '2')
                    {
                        paremt_i02.Text = string.Empty;
                        paremt_i02.AppendText(current_line[i + 24].ToString());

                        paremt_j02.Text = string.Empty;
                        paremt_j02.AppendText(current_line[i + 26].ToString());
                        Console.WriteLine("0 2");
                    }
                    else if (current_line[i - 5] == '0' && current_line[i - 2] == '3')
                    {
                        paremt_i03.Text = string.Empty;
                        paremt_i03.AppendText(current_line[i + 24].ToString());

                        paremt_j03.Text = string.Empty;
                        paremt_j03.AppendText(current_line[i + 26].ToString());
                        Console.WriteLine("0 3");
                    }
                    else if (current_line[i - 5] == '1' && current_line[i - 2] == '0')
                    {
                        paremt_i10.Text = string.Empty;
                        paremt_i10.AppendText(current_line[i + 24].ToString());

                        paremt_j10.Text = string.Empty;
                        paremt_j10.AppendText(current_line[i + 26].ToString());
                        Console.WriteLine("1 0");
                    }
                    else if (current_line[i - 5] == '1' && current_line[i - 2] == '1')
                    {
                        paremt_i11.Text = string.Empty;
                        paremt_i11.AppendText(current_line[i + 24].ToString());

                        paremt_j11.Text = string.Empty;
                        paremt_j11.AppendText(current_line[i + 26].ToString());
                        Console.WriteLine("1 1");
                    }
                    else if (current_line[i - 5] == '1' && current_line[i - 2] == '2')
                    {
                        paremt_i12.Text = string.Empty;
                        paremt_i12.AppendText(current_line[i + 24].ToString());

                        paremt_j12.Text = string.Empty;
                        paremt_j12.AppendText(current_line[i + 26].ToString());
                        Console.WriteLine("1 2");
                    }
                    else if (current_line[i - 5] == '1' && current_line[i - 2] == '3')
                    {
                        paremt_i13.Text = string.Empty;
                        paremt_i13.AppendText(current_line[i + 24].ToString());

                        paremt_j13.Text = string.Empty;
                        paremt_j13.AppendText(current_line[i + 26].ToString());
                        Console.WriteLine("1 3");
                    }
                    else if (current_line[i - 5] == '2' && current_line[i - 2] == '0')
                    {
                        paremt_i20.Text = string.Empty;
                        paremt_i20.AppendText(current_line[i + 24].ToString());

                        paremt_j20.Text = string.Empty;
                        paremt_j20.AppendText(current_line[i + 26].ToString());
                        Console.WriteLine("2 0");
                    }
                    else if (current_line[i - 5] == '2' && current_line[i - 2] == '1')
                    {
                        paremt_i21.Text = string.Empty;
                        paremt_i21.AppendText(current_line[i + 24].ToString());

                        paremt_j21.Text = string.Empty;
                        paremt_j21.AppendText(current_line[i + 26].ToString());
                        Console.WriteLine("2 1");
                    }
                    else if (current_line[i - 5] == '2' && current_line[i - 2] == '2')
                    {
                        paremt_i22.Text = string.Empty;
                        paremt_i22.AppendText(current_line[i + 24].ToString());

                        paremt_j22.Text = string.Empty;
                        paremt_j22.AppendText(current_line[i + 26].ToString());
                        Console.WriteLine("2 2");
                    }
                    else if (current_line[i - 5] == '2' && current_line[i - 2] == '3')
                    {
                        paremt_i23.Text = string.Empty;
                        paremt_i23.AppendText(current_line[i + 24].ToString());

                        paremt_j23.Text = string.Empty;
                        paremt_j23.AppendText(current_line[i + 26].ToString()); two_three.BackColor = pictureBox2.BackColor;
                        Console.WriteLine("2 3");
                    }
                    else if (current_line[i - 5] == '3' && current_line[i - 2] == '0')
                    {
                        paremt_i30.Text = string.Empty;
                        paremt_i30.AppendText(current_line[i + 24].ToString());

                        paremt_j30.Text = string.Empty;
                        paremt_j30.AppendText(current_line[i + 26].ToString());
                        Console.WriteLine("3 0");
                    }
                    else if (current_line[i - 5] == '3' && current_line[i - 2] == '1')
                    {
                        paremt_i31.Text = string.Empty;
                        paremt_i31.AppendText(current_line[i + 24].ToString());

                        paremt_j31.Text = string.Empty;
                        paremt_j31.AppendText(current_line[i + 26].ToString());
                        Console.WriteLine("3 1");
                    }
                    else if (current_line[i - 5] == '3' && current_line[i - 2] == '2')
                    {
                        paremt_i32.Text = string.Empty;
                        paremt_i32.AppendText(current_line[i + 24].ToString());

                        paremt_j32.Text = string.Empty;
                        paremt_j32.AppendText(current_line[i + 26].ToString());
                        Console.WriteLine("3 2");
                    }
                    else if (current_line[i - 5] == '3' && current_line[i - 2] == '3')
                    {
                        paremt_i33.Text = string.Empty;
                        paremt_i33.AppendText(current_line[i + 24].ToString());

                        paremt_j33.Text = string.Empty;
                        paremt_j33.AppendText(current_line[i + 26].ToString()); three_three.BackColor = pictureBox2.BackColor;
                        Console.WriteLine("3 3");
                    }
                    else
                    {
                        Console.WriteLine(current_line[i - 5].ToString(), current_line[i - 2].ToString());
                        Console.WriteLine("none of them");
                    }
                }
            }
        }

        void label1_Click_1(object sender, EventArgs e)
        {

        }

        void label7_Click(object sender, EventArgs e)
        {

        }

        private void one_three_TextChanged(object sender, EventArgs e)
        {

        }

        private void one_one_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void textBox73_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox100_TextChanged(object sender, EventArgs e)
        {

        }

        private void richTextBox1_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void richTextBox1_TextChanged_2(object sender, EventArgs e)
        {

        }

        private void ClosedList_TextChanged(object sender, EventArgs e)
        {

        }

        private void Input_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog
            {
                InitialDirectory = @"D:\",
                Title = "Browse Text Files",

                CheckFileExists = true,
                CheckPathExists = true,

                DefaultExt = "cpp",
                Filter = "cpp files (*.cpp)|*.cpp",
                FilterIndex = 2,
                RestoreDirectory = true,

                ReadOnlyChecked = true,
                ShowReadOnly = true
            };

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                LoadFile.Text = openFileDialog1.FileName;
            }

            richTextBox1.Text = String.Empty;
            linesCleaner = File.ReadAllLines(LoadFile.Text);
            string lineCleaner;
            int twoLinesCase = 0;
            for (int i = 0;i< linesCleaner.Length;i++)
            {
                lineCleaner = linesCleaner[i];
                bool IsNull;
                IsNull = string.IsNullOrEmpty(lineCleaner);

                if (IsNull == false )//linesCleaner[0] != "" || lineCleaner[1] == 't' || lineCleaner[0] == '{')
                {
                    //for(int j = 0; j<lineCleaner.Length;j++)
                    //{
                       // if (twoLinesCase == 0)
                       // {
                            //if (lineCleaner.Contains("//") && lineCleaner.Contains("/*dev*/") != false)//lineCleaner[j] == '/' && lineCleaner[j + 1] == '/') ////if there are some selective defines//
                           // {
                           //     twoLinesCase++;
                          //  }
                          //  else
                          //  {
                                if (lineCleaner.Contains("/*dev*/") == false)//lineCleaner[j] == '/' && lineCleaner[j + 1] == '*' && lineCleaner[j + 2] == 'd' && lineCleaner[j + 3] == 'e' && lineCleaner[j + 4] == 'v' && lineCleaner[j + 5] == '*' && lineCleaner[j + 6] == '/')
                                {
                                    linesCleaner_new.Add(lineCleaner);
                                    //lineCleaner = " ";
                                }
                           // }
                       // }
                       /* else
                        {
                            twoLinesCase = 0;
                        }*/
                    //}
                }
                
            }

            /*
              if ( (lineCleaner[0] != '/' && lineCleaner[1] != '*') )
                    {
                        if (lineCleaner[0] == '/' && lineCleaner[1] == '*' && lineCleaner[2] == 'd' && lineCleaner[3] == 'e' && lineCleaner[4] == 'v' && lineCleaner[5] == '*' && lineCleaner[6] == '/')
                        {
                            lineCleaner = " ";
                        }
                        else
                        {

                        }
                    }
             */
            string folder = @"D:\";
            // Filename  
            string fileName = "Cleaned.cpp";
            // Fullpath. You can direct hardcode it if you like.  
            string fullPath = folder + fileName;
            // Write array of strings to a file using WriteAllLines.  
            // If the file does not exists, it will create a new file.  
            // This method automatically opens the file, writes to it, and closes file

            File.WriteAllLines(fullPath, linesCleaner_new);
        }

        private void LoadFile_TextChanged(object sender, EventArgs e)
        {

        }


    }
}

