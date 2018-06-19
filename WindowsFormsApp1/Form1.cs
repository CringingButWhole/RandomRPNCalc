using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        #region Variables
        TableLayoutPanel numberPanel = new TableLayoutPanel();
        TextBox InputTextbox = new TextBox();
        TextBox OutputTextbox = new TextBox();
        Button BtnToOutput = new Button();
        Button BtnToInput = new Button();
        Button RPNCalc = new Button();
        GroupBox RdoButtonGroup = new GroupBox();
        String ActiveStack= "ArrayStack";
        Stack<string> ArrayStack = new Stack<string>();
        Stack<string> ListStack = new Stack<string>();
        Stack<string> MylistStack = new Stack<string>();


        string[] ButtonArray = new string[] { "1", "2", "3", "+", "4", "5", "6", "-", "7", "8", "9", "*", "(-)", "0", ",", "/" };
        string[] RdoStackOptions = new string[] { "ArrayStack", "ListStack", "MyListStack" };
        #endregion

        public Form1()
        {
            InitializeComponent();
            CreateInputTextbox();
            CreateOutputTextbox();
            CreateIOButtons();
            CreateLayoutPanel();
            CreateRadioGroup();
        }

        public void CreateInputTextbox()
        {
            this.Controls.Add(InputTextbox);
            InputTextbox.Enabled = false;
            InputTextbox.Multiline = true;
            InputTextbox.Size = new Size(180, 40);
            InputTextbox.Location = new Point(10, 60);
            InputTextbox.Text = "0";
            InputTextbox.Name = "InputTextbox";
            InputTextbox.TextAlign = HorizontalAlignment.Right;
        }

        //Maakt de textbox voor de output
        public void CreateOutputTextbox()
        {
            this.Controls.Add(OutputTextbox);
            OutputTextbox.Enabled = false;
            OutputTextbox.Multiline = true;
            OutputTextbox.Size = new Size(180, 220);
            OutputTextbox.Location = new Point(210, 60);
            OutputTextbox.Text = "";
            OutputTextbox.Name = "OutputTextbox";
        }

        //Maakt de knoppen voor input en output
        public void CreateIOButtons()
        {
            //input (clear voor nu)
            this.Controls.Add(BtnToInput);
            BtnToInput.Name = "c";
            BtnToInput.Text = "c";
            BtnToInput.Size = new Size(20, 20);
            BtnToInput.Location = new Point(190, 60);
            BtnToInput.Click += new EventHandler(ClearText);

            //output
            this.Controls.Add(BtnToOutput);
            BtnToOutput.Name = ">>";
            BtnToOutput.Text = ">>";
            BtnToOutput.Size = new Size(20, 20);
            BtnToOutput.Location = new Point(190, 80);
            BtnToOutput.Click += new EventHandler(ToOutput);

            this.Controls.Add(RPNCalc);
            RPNCalc.Text = "Calculate";
            RPNCalc.Size = new Size(100, 20);
            RPNCalc.Location = new Point(250, 280);
            RPNCalc.Click += new EventHandler(Calculate);

        }

        //Function van ClearText
        public void ClearText(object sender, EventArgs e)
        {
            InputTextbox.Text = "0";
        }

        //Function van ToOutput, Pusht de InputBox naar de desbetreffende Stack + Outputbox
        public void ToOutput(object sender, EventArgs e)
        {
            if (ActiveStack == "ArrayStack")
                ArrayStack.Push(Convert.ToString(InputTextbox.Text));
            if (ActiveStack == "ListStack")
                ListStack.Push(Convert.ToString(InputTextbox.Text));
            if (ActiveStack == "MyListStack")
                MylistStack.Push(Convert.ToString(InputTextbox.Text));
            PrintStack();
                InputTextbox.Text = "0";
        }

        public void Calculate(object sender, EventArgs e)
        {
            Double pop1;
            Double pop2;
            pop1 = Convert.ToDouble(ArrayStack.Pop);
        }

        //Print de Stacks in de OutputBox
        public void PrintStack()
        {
            OutputTextbox.Clear();
            if (ActiveStack == "ArrayStack")
                foreach (var item in ArrayStack)
                    OutputTextbox.Text += item + "\r\n";
            
            if (ActiveStack == "ListStack")
                foreach (var item in ListStack)
                    OutputTextbox.Text += item + "\r\n";
                
            if (ActiveStack == "MyListStack")
                foreach (var item in MylistStack)
                    OutputTextbox.Text += item + "\r\n";       
        }

        //Maakt de layoutpanel voor de nummers en functies
        public void CreateLayoutPanel()
        {
            numberPanel.Location = new Point(10, 100);
            numberPanel.Size = new Size(180, 180);
            numberPanel.RowCount = 4;
            numberPanel.ColumnCount = 4;
            for (int i = 0; i < 16; i++)
            {
                Button NumpadButton = new Button
                {
                    Name = ButtonArray[i],
                    Dock = DockStyle.Fill,
                    Text = ButtonArray[i],
                    Visible = true,
                    Enabled = true,
                    Size = new Size(40, 40)
                };
                this.Controls.Add(NumpadButton);
                numberPanel.Controls.Add(NumpadButton);
                NumpadButton.Click += new EventHandler(ClkNumpadButton);
            }
            this.Controls.Add(numberPanel);
        }

        //Functie van layoutpanel buttons
        public void ClkNumpadButton(object sender, EventArgs e)
        {
            Button clickedButton = sender as Button;
            string InputValue = clickedButton.Name;

            //voor onderscheid tussen de knoppen
            List<string> Numberlist = new List<string>() { "1", "2", "3", "4", "5", "6", "7", "8", "9", "0", "," };
            List<string> Functionlist = new List<string>() { "+", "-", "*", "/", "(-)", "c" };

            //Input is nummer of ','
            if (Numberlist.Contains(InputValue))
            {       //Niet meerdere ',' in input
                if (!(InputTextbox.Text.Contains(",") && InputValue == ","))
                {        // "0," getallen vervangt niet de start met 0 
                    if (InputTextbox.Text.StartsWith("0,"))
                    {
                        InputTextbox.Text += InputValue;
                    }
                    //Bij start met '0' vervangt '0' door de input, maar niet bij ","
                    else if (InputTextbox.Text.StartsWith("0") && InputValue != ",")
                    {
                        InputTextbox.Text = InputValue;
                    }
                    //=normal flow, getal komt er gewoon achter te staan
                    else
                    {
                        InputTextbox.Text += InputValue;
                    }
                }
            }
            //Input is functie
            else if (Functionlist.Contains(InputValue))
            {       //Wat in de input staat *-1
                if (InputValue == "(-)")
                {
                    double KeerMinEen = Convert.ToDouble(InputTextbox.Text) * -1;
                    InputTextbox.Text = Convert.ToString(KeerMinEen);
                }
                
                else
                {
                    if (InputTextbox.Text.EndsWith("+") || InputTextbox.Text.EndsWith("-") || InputTextbox.Text.EndsWith("*") || InputTextbox.Text.EndsWith("/"))
                    {
                       InputTextbox.Text += InputValue;
                    }
                    else
                    {
                        InputTextbox.Text += InputValue;
                    }
                }
            }
        }

        //Maakt de keuze voor de stacks
        public void CreateRadioGroup()
        {
            RdoButtonGroup.Text = "Select Stack type";
            RdoButtonGroup.Location = new Point(10, 280);
            RdoButtonGroup.Size = new Size(180, 80);
            for (int i = 0; i < 3; i++)
            {
                RadioButton RdoStacksButton = new RadioButton
                {
                    Name = RdoStackOptions[i],
                    Text = RdoStackOptions[i],
                    Size = new Size(180, 20),
                    Dock = DockStyle.Bottom,
                };
                //eerste optie is checked
                if (i==0)
                {
                    RdoStacksButton.Checked = true;
                }
                this.Controls.Add(RdoStacksButton);
                RdoStacksButton.CheckedChanged += new EventHandler(this.OnCheckedChanged);
            }
            this.Controls.Add(RdoButtonGroup);
        }

        public void OnCheckedChanged(object sender, System.EventArgs e)
        {
            RadioButton RdoStacksButton = sender as RadioButton;
            ActiveStack = RdoStacksButton.Name;
            PrintStack();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch (keyData)
            {
                case Keys.NumPad0:
                    Button Np0 = this.Controls.Find("0", true).FirstOrDefault() as Button;
                    Np0.PerformClick();
                    break;
                case Keys.NumPad1:
                    Button Np1 = this.Controls.Find("1", true).FirstOrDefault() as Button;
                    Np1.PerformClick();
                    break;
                case Keys.NumPad2:
                    Button Np2 = this.Controls.Find("2", true).FirstOrDefault() as Button;
                    Np2.PerformClick();
                    break;
                case Keys.NumPad3:
                    Button Np3 = this.Controls.Find("3", true).FirstOrDefault() as Button;
                    Np3.PerformClick();
                    break;
                case Keys.NumPad4:
                    Button Np4 = this.Controls.Find("4", true).FirstOrDefault() as Button;
                    Np4.PerformClick();
                    break;
                case Keys.NumPad5:
                    Button Np5 = this.Controls.Find("5", true).FirstOrDefault() as Button;
                    Np5.PerformClick();
                    break;
                case Keys.NumPad6:
                    Button Np6 = this.Controls.Find("6", true).FirstOrDefault() as Button;
                    Np6.PerformClick();
                    break;
                case Keys.NumPad7:
                    Button Np7 = this.Controls.Find("7", true).FirstOrDefault() as Button;
                    Np7.PerformClick();
                    break;
                case Keys.NumPad8:
                    Button Np8 = this.Controls.Find("8", true).FirstOrDefault() as Button;
                    Np8.PerformClick();
                    break;
                case Keys.NumPad9:
                    Button Np9 = this.Controls.Find("9", true).FirstOrDefault() as Button;
                    Np9.PerformClick();
                    break;
                case Keys.Multiply:
                    Button Multiply = this.Controls.Find("*", true).FirstOrDefault() as Button;
                    Multiply.PerformClick();
                    break;
                case Keys.Subtract:
                    Button Substract = this.Controls.Find("/", true).FirstOrDefault() as Button;
                    Substract.PerformClick();
                    break;
                case Keys.OemMinus:
                    Button Minus = this.Controls.Find("-", true).FirstOrDefault() as Button;
                    Minus.PerformClick();
                    break;
                case Keys.Oemplus:
                    Button Plus = this.Controls.Find("+", true).FirstOrDefault() as Button;
                    Plus.PerformClick();
                    break;
                case Keys.Oemcomma:
                    Button Comma = this.Controls.Find(",", true).FirstOrDefault() as Button;
                    Comma.PerformClick();
                    break;
                case Keys.Enter:
                    Button Enter = this.Controls.Find(">>", true).FirstOrDefault() as Button;
                    Enter.PerformClick();
                    break;
                case Keys.Back:
                    Button Backspace = this.Controls.Find("c", true).FirstOrDefault() as Button;
                    Backspace.PerformClick();
                    break;
                case Keys.Delete:
                    Button Del = this.Controls.Find("c", true).FirstOrDefault() as Button;
                    Del.PerformClick();
                    break;
            }
            return base.ProcessCmdKey(ref msg, keyData);
            
        }

    }
}
