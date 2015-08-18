using Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Gui.Controls
{
    /// <summary>
    /// Interaction logic for Console.xaml
    /// </summary>
    public partial class Console : UserControl
    {
        private LogString myLogger = LogString.GetLogString("Log");

        public Console()
        {
            InitializeComponent();

            myLogger.OnLogUpdate += new LogString.LogUpdateDelegate(this.LogUpdate);
            ConsoleBox.TextWrapping = TextWrapping.Wrap;
            ConsoleBox.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;

        }

        private delegate void UpdateDelegate();

        private void LogUpdate()
        {
            try
            {
                ConsoleBox.AppendText(myLogger.GetLastLog);
                ConsoleBox.ScrollToEnd();
            }
            catch (Exception e) { }
 
            //ConsoleBox.Text = myLogger.GetLog;
        }
    }
}
