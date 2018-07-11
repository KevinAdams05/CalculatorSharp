using System;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Diagnostics;
using System.IO;
// ReSharper disable SpecifyACultureInStringConversionExplicitly
// ReSharper disable LocalizableElement

namespace Calculator
{
    internal partial class Calculator : Form
    {
        private bool UseCommaSeparators;// Settings
        private AboutBox2 popUp; // This is used for the "About" window

        public Calculator()
        { 
            // Initialize the settings
            UseCommaSeparators = false;
            popUp = new AboutBox2();
            InitializeComponent();
        }

        /// <summary>
        /// The majority of the System.Windows.Forms.Button functions below simply call the main functions of calcEngine
        /// </summary>
        private void UpdateScreen()
        {
            // Updates the text based on the data within the calcEngine
            calcScreen.Text = FormatDisplay(Convert.ToString(CalcEngine.GetDisplay()));
        }

        /// <summary>
        /// When a key is pressed, the number is checked and we pass that value into the calculator engine
        /// </summary>
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
                UpdateScreen();

                if (CalcEngine.m_operation == null)
                {
                    operation_txt.Text = "";
                }
            }
        }

        /// <summary>
        /// The equals button will make some adjustments to the GUI before updating the screen based on values in calcEngine
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void equals_btn(object sender, EventArgs e)
        {
            if (sender is Button)
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
                    status_txt.Text = "Can't divide by Zero";
                    clear_All.PerformClick();
                    UpdateScreen();
                }

                UpdateScreen();

                if (CalcEngine.m_closeParen)
                {
                    paren_txt.Text = "";
                }
            }
        }

        /// <summary>
        /// Tell the calculator engine to clear itself
        /// </summary>
        private void clear_btn(object sender, EventArgs e)
        {
            if (sender is Button)
            {
                Button clearBtn = sender as Button;
                switch (clearBtn.Name)
                {
                    case "clear_All":
                        CalcEngine.ClearAll();
                        paren_txt.Text = string.Empty;
                        operation_txt.Text = string.Empty;
                        status_txt.Text = string.Empty;
                        UpdateScreen();
                        break;
                    case "clear_Entry":
                        CalcEngine.clear();
                        UpdateScreen();
                        if (!CalcEngine.m_openParen)
                        {
                            status_txt.Text = string.Empty;
                        }
                        break;
                }
            }
        }

        /// <summary>
        /// Pass an operation into the calc engine (+, -, *, /) 
        /// </summary>
        private void operation_btn(object sender, EventArgs e)
        {
            if (sender is Button)
            {
                Button btnClicked = sender as Button;
                CalcEngine.PrepareOperation(btnClicked.Name);
                operation_txt.Text = CalcEngine.m_operation ?? string.Empty;
                UpdateScreen();

                if (CalcEngine.m_openParen)
                {
                    status_txt.Text = CalcEngine.equation;
                }
            }
        }

        /// <summary>
        /// Updates status text based on what button was pressed, and updates the engine
        /// </summary>
        private void memory_btn(object sender, EventArgs e)
        {
            if (sender is Button && !CalcEngine.m_openParen)
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
                UpdateScreen();
            }
        }

        /// <summary>
        /// If a settings button is hit, pass its value into menu_Actions
        /// </summary>
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
                UpdateScreen();
            }
        }

        /// <summary>
        /// Pass this trig buttons value into the calcEngine
        /// </summary>
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
                UpdateScreen();
            }
        }

        /// <summary>
        /// Toggles the display mode for angles
        /// </summary>
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
                UpdateScreen();
            }
        }

        /// <summary>
        /// Updates the engine angle display mode based on the provided string
        /// </summary>
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
                    }
                    break;
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

        /// <summary>
        /// Performs some misc function on the calcEngine based on the button that was pressed
        /// (Again, everything here is merely updating the screen, but the calcEngine variable holds the true state of things)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                UpdateScreen();
            }
        }

        /// <summary>
        /// Other functions possible here
        /// </summary>
        private void calcScreen_TextChanged(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Show the about menu
        /// </summary>
        private void about_Popup(object sender, EventArgs e)
        {
            popUp.ShowDialog();
        }

        /// <summary>
        /// Keyboard shortcuts
        /// </summary>
        /// <param name="c">input character</param>
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

        /// <summary>
        /// Performs an action based on the given key value
        /// </summary>
        /// <param name="k">input key</param>
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

        /// <summary>
        /// Deal with keyboard press events
        /// </summary>
        private void Calculator_KeyPress(object sender, KeyPressEventArgs e)
        {
            GetKey(e.KeyChar);
        }

        /// <summary>
        /// Deal with keyboard press events
        /// </summary>
        private void Calculator_KeyDown(object sender, KeyEventArgs e)
        {
            GetKey(e.KeyCode);
        }

        /// <summary>
        /// Format the display based on if commas are on or not
        /// </summary>
        private string FormatDisplay(String tempstr)
        {
            string dec = "";
            int totalCommas;
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

            if (UseCommaSeparators && Convert.ToDouble(tempstr) < Math.Pow(10, 19))
            {
                if (tempstr.Length > 3)
                {
                    totalCommas = (tempstr.Length - (tempstr.Length % 3)) / 3;
                    if (tempstr.Length % 3 == 0)
                    {
                        totalCommas--;
                    }

                    int pos = tempstr.Length - 3;

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

        /// <summary>
        /// Toggle comma separators
        /// </summary>>
        private void menu_Actions(string n)
        {
            switch (n)
            {
                case "digitGroup":
                    if (digitGroup.CheckState.Equals(CheckState.Checked))
                    {
                        UseCommaSeparators = false;
                        digitGroup.CheckState = CheckState.Unchecked;
                        status_txt.Text = "Comma Separators Off";
                        UpdateScreen();
                    }
                    else
                    {
                        UseCommaSeparators = true;
                        digitGroup.CheckState = CheckState.Checked;
                        status_txt.Text = "Comma Seperators On";
                        UpdateScreen();
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
                    UpdateScreen();
                    break;
            }
        }

        /// <summary>
        /// Used to get the internet browser path for showing the documentation
        /// </summary>
        /// <returns>path to the browser</returns>
        private static string GetDefaultBrowserPath()
        {
            RegistryKey registryKey = Registry.ClassesRoot.OpenSubKey(@"htmlfile\shell\open\command", false);

            // get default browser path
            if (registryKey != null)
            {
                return ((string) registryKey.GetValue(null, null)).Split('"')[1];
            }

            throw new Exception("Could not find path to default web browser");
        }

        /// <summary>
        /// Used to show the documentation in the user's browser
        /// </summary>
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