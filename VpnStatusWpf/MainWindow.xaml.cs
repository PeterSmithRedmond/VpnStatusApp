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

            try
            {
                var rasConnections = RASMAN.RASMAN.RasEnumConnections();
                Log("CONNECTIONS");
                for (int i = 0; i < rasConnections.Length; i++)
                {
                    var item = rasConnections[i];
                    Log($"{item.EntryName}\t{item.PhoneBook}\t{item.DeviceType}\t{item.DeviceName}");

                    // Get the connection stats
                    _RAS_STATS stats = new _RAS_STATS();
                    stats.Init();
                    UInt32 status = RASMAN.RASMAN.RasGetConnectionStatistics(item.hrasconn, out stats);
                    Log($"    Recv: {stats.BytesRcved}bytes = {stats.BytesRcved/1024}k = {stats.BytesRcved / (1024*1204)}m");
                    Log($"    Xmit: {stats.BytesXmited}");
                    Log($"    Duration: {stats.ConnectDuration}");
                    Log($"    BPS: {stats.Bps}");
                }
                Log("\n\n");

                var rasEntries = RASMAN.RASMAN.RasEnumEntries();
                Log("ENTRIES");
                for (int i = 0; i < rasEntries.Length; i++)
                {
                    var item = rasEntries[i];
                    Log($"{item.EntryName}\t{item.PhoneBookPath}");
                }
                Log("\n\n");
            }
            catch (Exception ex)
            {
                Log(ex.Message);
            }

            Log("DONE");
        }
    }
}
