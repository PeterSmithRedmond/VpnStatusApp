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

namespace RASMAN
{
    [StructLayout(LayoutKind.Sequential, Pack =4, CharSet = CharSet.Unicode)]
    public unsafe struct RASENTRYNAMEW
    {
        public void Init()
        {
            Size = (UInt32)Marshal.SizeOf(typeof(RASENTRYNAMEW));
            dwFlags = 0;
        }
        public UInt32 Size;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = RASMAN.RAS_MaxEntryName + 1)]
        public string EntryName;
        //public fixed char EntryName[RASMAN.RAS_MaxEntryName + 1];
        public UInt32 dwFlags;
        //public fixed char szPhonebookPath[RASMAN.MAX_PATH + 1];
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = RASMAN.MAX_PATH + 1)]
        public string PhoneBookPath;
    }
    public static class RASMAN
    {
        public const int MAX_PATH = 260; // set where?
        public const int RAS_MaxEntryName = 256;

        [DllImport("RasAPI32.dll", CharSet = CharSet.Unicode)]
        public static extern UInt32 RasEnumEntriesW(string reserved1, string phonebook, [In,Out] RASENTRYNAMEW[] array, ref UInt32 arrayByteSize, out UInt32 arrayCountWritten );
    };

    public static class RASERROR
    {
        public const int RASBASE=600;
        public const int ERROR_TOO_SMALL = RASBASE + 3; // 603
        public const int ERROR_INVALID_SIZE = RASBASE + 32; // 632
    }

}
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

            int entrySize = Marshal.SizeOf(typeof(RASENTRYNAMEW));

            UInt32 status = 0;
            UInt32 arrayByteSize = (UInt32)(entrySize * 1);
            UInt32 arrayCount = 0;
            do
            {
                arrayCount = (UInt32)(arrayByteSize / entrySize);
                RASENTRYNAMEW[] array = new RASENTRYNAMEW[arrayCount];
                for (int i = 0; i < arrayCount; i++)
                {
                    array[i].Init();
                }
                status = RASMAN.RASMAN.RasEnumEntriesW(null, null, array, ref arrayByteSize, out arrayCount);
            }
            while (status == RASMAN.RASERROR.ERROR_TOO_SMALL); // 603==too small

            Log("DONE");
        }
    }
}
