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
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using RASMAN;
using System.Windows.Controls.Primitives;


namespace VpnStatusWpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
        }
        private void Log (string str)
        {
            uiStatus.Text += "\r\n" + str;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            uiStatus.Text = "Starting P/INVOKE!";

            var rasEntries = RASMAN.RASMAN.RasEnumEntries();

            for (int i=0; i<rasEntries.Length; i++)
            {
                var entry = rasEntries[i];
                Log($"{entry.EntryName}\t{entry.PhoneBookPath}");
            }

            Log("DONE");
        }
    }
}
