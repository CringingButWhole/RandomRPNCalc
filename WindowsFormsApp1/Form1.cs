using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
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
        GroupBox RdoButtonGroup = new GroupBox();
        String ActiveStack= "ArrayStack";
        Stack<Int32> ArrayStack = new Stack<int>();
        Stack<Int32> ListStack = new Stack<int>();
        Stack<Int32> MylistStack = new Stack<int>();
        Button NumpadButton;


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
            BtnToInput.Text = "c";
            BtnToInput.Size = new Size(20, 20);
            BtnToInput.Location = new Point(190, 60);
            BtnToInput.Click += new EventHandler(ClearText);

            //output
            this.Controls.Add(BtnToOutput);
            BtnToOutput.Text = ">>";
            BtnToOutput.Size = new Size(20, 20);
            BtnToOutput.Location = new Point(190, 80);
            BtnToOutput.Click += new EventHandler(ToOutput);
            this.AcceptButton = BtnToOutput;
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
                ArrayStack.Push(Convert.ToInt32(InputTextbox.Text));
            if (ActiveStack == "ListStack")
                ListStack.Push(Convert.ToInt32(InputTextbox.Text));
            if (ActiveStack == "MyListStack")
                MylistStack.Push(Convert.ToInt32(InputTextbox.Text));
            PrintStack();
                InputTextbox.Text = "0";
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

                //else if (dingen met stacks)
                //{
                //      voer functie uit
                //}
                else
                {

                }
            }
        }

        //Maakt de keuze voor de stacks
        public void CreateRadioGroup()
        {
            RdoButtonGroup.Text = "Select Stack type";
            RdoButtonGroup.Location = new Point(10, 280);
            RdoButtonGroup.Size = new Size(180, 80);
            RdoButtonGroup.Enabled = true;
            RdoButtonGroup.Visible = true;
            for (int i = 0; i < 3; i++)
            {
                RadioButton RdoStacksButton = new RadioButton
                {
                    Name = RdoStackOptions[i],
                    Text = RdoStackOptions[i],
                    Size = new Size(180, 20),
                    Dock = DockStyle.Bottom,
                    Visible = true,
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
                    System.Windows.Forms.MessageBox.Show("00000000");
                    this.Controls.Find("0", true).FirstOrDefault();
                    this.NumpadButton.PerformClick();
                    break;
                case Keys.NumPad1:
                    //System.Windows.Forms.MessageBox.Show("00000001");
                    InputTextbox.Text += "1";
                    break;
                case Keys.NumPad2:
                    System.Windows.Forms.MessageBox.Show("00000002");
                    break;
                case Keys.NumPad3:
                    System.Windows.Forms.MessageBox.Show("00000003");
                    break;
                case Keys.NumPad4:
                    System.Windows.Forms.MessageBox.Show("00000004");
                    break;
                case Keys.NumPad5:
                    System.Windows.Forms.MessageBox.Show("00000005");
                    break;
                case Keys.NumPad6:
                    System.Windows.Forms.MessageBox.Show("00000006");
                    break;
                case Keys.NumPad7:
                    System.Windows.Forms.MessageBox.Show("00000007");
                    break;
                case Keys.NumPad8:
                    System.Windows.Forms.MessageBox.Show("00000008");
                    break;
                case Keys.NumPad9:
                    System.Windows.Forms.MessageBox.Show("00000009");
                    break;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}
