using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using NUnit.Framework;
using Microsoft.Win32;
using System.Diagnostics;
using System.IO;

namespace Calculator
{
    internal partial class Calculator : Form
    {
        private bool m_useCommaSeparators;// Settings
        private AboutBox2 popUp; // This is used for the "About" window

        public Calculator()
        { 
            // Initialize the settings
            m_useCommaSeparators = false;
            popUp = new AboutBox2();
            InitializeComponent();
        }

        // The majority of the System.Windows.Forms.Button functions below simply call the main functions of calcEngine
        private void updateScreen()
        {
            // Updates the text based on the data within the calcEngine
            calcScreen.Text = FormatDisplay(Convert.ToString(CalcEngine.GetDisplay()));
        }

        // When a key is pressed, the number is checked and we pass that value into the calculator engine
        private void number_btn(object sender, EventArgs e)
        {
            Button num = sender as Button;
            if (num != null)
            {
                if (!CalcEngine.m_openParen)
                {
                    status_txt.Text = "";
                }

                int numValue;
                switch (num.Name)
                { // Convert the System.Windows.Forms.Button pressed into a number which is stored in a string
                    case "one":
                        numValue = 1; break;
                    case "two":
                        numValue = 2; break;
                    case "three":
                        numValue = 3; break;
                    case "four":
                        numValue = 4; break;
                    case "five":
                        numValue = 5; break;
                    case "six":
                        numValue = 6; break;
                    case "seven":
                        numValue = 7; break;
                    case "eight":
                        numValue = 8; break;
                    case "nine":
                        numValue = 9; break;
                    default:
                        numValue = 0; break;
                }

                CalcEngine.AppendNum(numValue);
                updateScreen();

                if (CalcEngine.m_operation == null)
                {
                    operation_txt.Text = "";
                }
            }
        }

        // The equals button will make some adjustments to the GUI before updating the screen based on values in calcEngine
        private void equals_btn(object sender, EventArgs e)
        {
            if (sender is System.Windows.Forms.Button)
            {
                // First deal with the notes that may  displayed in the status bar at the bottom
                if (!CalcEngine.m_openParen)
                {
                    status_txt.Text = "";
                }

                if (CalcEngine.m_openParen)
                {
                    status_txt.Text = CalcEngine.equation + CalcEngine.GetDisplay().ToString();
                    paren_txt.Text = "";
                    operation_txt.Text = "";
                }
                // Attempt to solve the math
                if (!CalcEngine.Solve())
                {
                    status_txt.Text = "Sorry, only Chuck Norris can divide by Zero";
                    clear_All.PerformClick();
                    updateScreen();
                }

                updateScreen();
                if (CalcEngine.m_closeParen)
                {
                    paren_txt.Text = "";
                }
            }
        }

        // Tell the calculator engine to clear itself
        private void clear_btn(object sender, EventArgs e)
        {
            if (sender is System.Windows.Forms.Button)
            {
                Button clear_btn = sender as Button;
                switch (clear_btn.Name)
                {
                    case "clear_All":
                        CalcEngine.ClearAll();
                        paren_txt.Text = "";
                        operation_txt.Text = "";
                        status_txt.Text = "";
                        updateScreen();
                        break;
                    case "clear_Entry":
                        CalcEngine.clear();
                        updateScreen();
                        if (!CalcEngine.m_openParen)
                        {
                            status_txt.Text = "";
                        }
                        break;
                }
            }
        }

        // Pass an operation into the calc engine (+, -, *, /) 
        private void operation_btn(object sender, EventArgs e)
        {
            if (sender is Button)
            {
                Button btn_clicked = sender as Button;
                CalcEngine.PrepareOperation(btn_clicked.Name);
                operation_txt.Text = CalcEngine.m_operation ?? "";
                updateScreen();

                if (CalcEngine.m_openParen)
                {
                    status_txt.Text = CalcEngine.equation;
                }
            }
        }

        // Updates status text based on what button was pressed, and updates the engine
        private void memory_btn(object sender, EventArgs e)
        {
            if (sender is System.Windows.Forms.Button && !CalcEngine.m_openParen)
            {
                if (!CalcEngine.m_openParen)
                {
                    status_txt.Text = "";
                }

                Button btnClicked = sender as Button;
                if(btnClicked.Name.Equals("memClear"))
                {
                    status_txt.Text = CalcEngine.m_memory == null ? "Nothing to clear" : "Memory cleared";
                }
                else if(btnClicked.Name.Equals("memRecall"))
                {
                    status_txt.Text = CalcEngine.m_memory == null ? "Nothing to recall" : "Number recalled";
                }
                else if(btnClicked.Name.Equals("memAdd"))
                {
                    if (CalcEngine.m_memory == null)
                    {
                        status_txt.Text = "No number stored in memory";
                    }
                    else
                    {
                        status_txt.Text = "Number added";
                        mem_text.Text = "M";
                    }
                }
                CalcEngine.Memory(btnClicked.Name);
                updateScreen();
            }
        }

        // If a settings button is hit, pass its value into menu_Actions
        private void menu_btn(object sender, EventArgs e)
        {
            if (sender is ToolStripMenuItem)
            {
                if (!CalcEngine.m_openParen)
                {
                    status_txt.Text = "";
                }

                ToolStripMenuItem menuItem = sender as ToolStripMenuItem;
                menu_Actions(menuItem.Name);
                updateScreen();
            }
        }

        // Pass this trig buttons value into the calcEngine
        private void trig_btn(object sender, EventArgs e)
        {
            if (sender is Button)
            {
                if (!CalcEngine.m_openParen)
                {
                    status_txt.Text = "";
                }

                Button btnClicked = sender as Button;
                CalcEngine.trig_fcns(btnClicked.Name);
                updateScreen();
            }
        }

        // Toggles the display mode for angles
        private void toggleMode_btn(object sender, EventArgs e)
        {
            if (!CalcEngine.m_openParen)
            {
                status_txt.Text = "";
            }
            if (sender is Button)
            {
                Button radioBtn = sender as Button;
                ToggleMode(radioBtn.Name);
                updateScreen();
            }
        }
        
        // Updates the engine angle display mode based on the provided string
        private void ToggleMode(string n)
        {
            switch (n)
            {
                case "radian_btn":
                    if (radian_btn.Checked)
                    {
                        radian_btn.Checked = true;
                        degrees_btn.Checked = false;
                        CalcEngine.m_useRadians = true;
                        status_txt.Text = "Radian Mode";
                    } break;
                case "degrees_btn":
                    if (degrees_btn.Checked)
                    {
                        degrees_btn.Checked = true;
                        radian_btn.Checked = false;
                        CalcEngine.m_useRadians = false;
                        status_txt.Text = "Degree Mode";
                    }
                    break;
            }
        }

        // Performs some misc function on the calcEngine based on the button that was pressed
        // (Again, everything here is merely updating the screen, but the calcEngine variable holds the true state of things)
        private void other_btn(object sender, EventArgs e)
        {
            status_txt.Text = "";
            if (sender is Button)
            {
                Button btnClicked = sender as Button;
                if (btnClicked.Name.Equals("close_paren"))
                {
                    if (CalcEngine.m_openParen)
                    {
                        status_txt.Text = CalcEngine.equation + CalcEngine.GetDisplay().ToString();
                        paren_txt.Text = "";
                        operation_txt.Text = "";
                    }
                    else
                    {
                        status_txt.Text = "You most open the parentheses first";
                    }
                }
                else if (btnClicked.Name.Equals("open_paren"))
                {
                    if (CalcEngine.m_closeParen)
                    {
                        paren_txt.Text = "(...)";
                        operation_txt.Text = "";
                    }
                    else
                    {
                        status_txt.Text = "Parentheses already opened";
                    }
                }
                else if (btnClicked.Name.Equals("percent"))
                {
                    if (CalcEngine.m_openParen)
                    {
                        status_txt.Text = "% is disabled for parenthetical operations";
                    }
                }
                else if (btnClicked.Name.Equals("inverse"))
                {
                    if (Convert.ToDouble(CalcEngine.input) == 0)
                    {
                        status_txt.Text = "Can't inverse zero";
                    }
                }
                else if (btnClicked.Name.Equals("backspace"))
                {
                    if (CalcEngine.input.Equals("."))
                    {
                        status_txt.Text = "Nothing to Backspace";
                    }
                }
                else if (btnClicked.Name.Equals("decimal_btn"))
                {
                    btnClicked.Name = "decimal";
                }
                else if (btnClicked.Name.Equals("sqrt"))
                {
                    if (CalcEngine.GetDisplay()<0)
                    {
                        status_txt.Text = "Invalid operation attempted";
                    }
                }

                CalcEngine.other_fcns(btnClicked.Name);
                updateScreen();
            }
        }

        // Other functions possible here
        private void calcScreen_TextChanged(object sender, EventArgs e)
        {

        }

        // Show the about menu
        private void about_Popup(object sender, EventArgs e)
        {
            popUp.ShowDialog();
        }

        // Keyboard shortcuts
        private void GetKey(char c)
        {
            switch (c)
            {
                case '0':
                    zero.PerformClick();
                    break;
                case '1':
                    one.PerformClick();
                    break;
                case '2':
                    two.PerformClick();
                    break;
                case '3':
                    three.PerformClick();
                    break;
                case '4':
                    four.PerformClick();
                    break;
                case '5':
                    five.PerformClick();
                    break;
                case '6':
                    six.PerformClick();
                    break;
                case '7':
                    seven.PerformClick();
                    break;
                case '8':
                    eight.PerformClick();
                    break;
                case '9':
                    nine.PerformClick();
                    break;
                case '=':
                    add.PerformClick();
                    break;
                case '+':
                    add.PerformClick();
                    break;
                case '-':
                    subtract.PerformClick();
                    break;
                case '/':
                    divide.PerformClick();
                    break;
                case '*':
                    multiply.PerformClick();
                    break;
                case '\b':
                    backspace.PerformClick();
                    break;
                case '.':
                    decimal_btn.PerformClick();
                    break;
            }
        }

        // Performs an action based on the given key value
        private void GetKey(Keys k)
        {
            switch (k)
            {
                case Keys.Enter:
                    equals.PerformClick();
                    break;
                case Keys.Escape:
                    clear_All.PerformClick();
                    break;
                case Keys.Delete:
                    clear_Entry.PerformClick();
                    break;
                case Keys.F9:
                    switchSign.PerformClick();
                    break;
            }
        }

        // Deal with keyboard press events
        private void Calculator_KeyPress(object sender, KeyPressEventArgs e)
        {
            GetKey(e.KeyChar);
        }

        private void Calculator_KeyDown(object sender, KeyEventArgs e)
        {
            GetKey(e.KeyCode);
        }

        //Format the display based on if commas are on or not
        private string FormatDisplay(String tempstr)
        {
            string dec = "";
            int totalCommas = 0;
            int pos = 0;
            bool addNegative = false;

            if (tempstr.StartsWith("-"))
            {
                tempstr = tempstr.Remove(0, 1);
                addNegative = true;
            }

            if (tempstr.IndexOf(".") > -1)
            {
                dec = tempstr.Substring(tempstr.IndexOf("."), tempstr.Length - tempstr.IndexOf("."));
                tempstr = tempstr.Remove(tempstr.IndexOf("."), tempstr.Length - tempstr.IndexOf("."));
            }

            if (m_useCommaSeparators && Convert.ToDouble(tempstr) < Math.Pow(10, 19))
            {
                if (tempstr.Length > 3)
                {
                    totalCommas = (tempstr.Length - (tempstr.Length % 3)) / 3;
                    if (tempstr.Length % 3 == 0)
                    {
                        totalCommas--;
                    }
                    pos = tempstr.Length - 3;

                    while (totalCommas > 0)
                    {
                        tempstr = tempstr.Insert(pos, ",");
                        pos -= 3;
                        totalCommas--;
                    }
                }
            }

            tempstr += "" + dec;
            if (tempstr.IndexOf(".") == -1)
            {
                tempstr = tempstr + ".";
            }

            if (tempstr.IndexOf(".") == 0)
            {
                tempstr.Insert(0, "0");
            }
            else if (tempstr.IndexOf(".") == tempstr.Length - 2 && tempstr.LastIndexOf("0") == tempstr.Length - 1)
            {
                tempstr = tempstr.Remove(tempstr.Length - 1);
            }

            if (addNegative)
            {
                tempstr = tempstr.Insert(0, "-");
            }

            return tempstr;
        }

        // Toggle comma separators
        private void menu_Actions(string n)
        {
            switch (n)
            {
                case "digitGroup":
                    if (digitGroup.CheckState.Equals(CheckState.Checked))
                    {
                        m_useCommaSeparators = false;
                        digitGroup.CheckState = CheckState.Unchecked;
                        status_txt.Text = "Comma Separators Off";
                        updateScreen();
                    }
                    else
                    {
                        m_useCommaSeparators = true;
                        digitGroup.CheckState = CheckState.Checked;
                        status_txt.Text = "Comma Seperators On";
                        updateScreen();
                    }
                    break;
                case "copy_btn":
                    Clipboard.SetDataObject(CalcEngine.input, true);
                    break;
                case "paste_btn":
                    try
                    {
                        Convert.ToDouble(Clipboard.GetDataObject().GetData(DataFormats.Text).ToString());
                    }
                    catch
                    {
                        status_txt.Text = "Invalid number in Clipboard";
                        break;
                    }

                    CalcEngine.input = Clipboard.GetDataObject().GetData(DataFormats.Text).ToString();
                    updateScreen();
                    break;

            }
        }

        // Used to get the internet browser path for showing the documentation
        private static string GetDefaultBrowserPath()
        {
            string key = @"htmlfile\shell\open\command";
            RegistryKey registryKey = Registry.ClassesRoot.OpenSubKey(key, false);
            // get default browser path
            return ((string) registryKey.GetValue(null, null)).Split('"')[1];
        }

        // Used to show the documentation in the user's browser
        private void HelpPopup(object sender, EventArgs e)
        {
            string defaultBrowserPath = GetDefaultBrowserPath();
            try
            {
                // launch default browser
                Process.Start(defaultBrowserPath, Path.Combine(Application.StartupPath,@"Documentation.htm"));
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message);
            }
        }
    }
}