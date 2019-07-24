using System;
using System.Runtime.InteropServices;

/// <summary>
/// All of the classes needed to get VPN Phonebook entries and expose them to C#
/// </summary>
namespace RASMAN
{
    /// <summary>
    /// https://docs.microsoft.com/en-us/previous-versions/windows/desktop/legacy/aa376725(v=vs.85)
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 4, CharSet = CharSet.Unicode)]
    public unsafe struct RASCONNW
    {
        public void Init()
        {
            Size = (UInt32)Marshal.SizeOf(this.GetType());
            dwFlags = 0;
        }

        public UInt32 Size;
        public IntPtr hrasconn;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = RASMAN.RAS_MaxEntryName + 1)]
        public string EntryName;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = RASMAN.RAS_MaxDeviceType + 1)]
        public string DeviceType;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = RASMAN.RAS_MaxDeviceName + 1)]
        public string DeviceName;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = RASMAN.MAX_PATH + 1)]
        public string PhoneBook;

        UInt32 dwSubEntry;
        Guid guidEntry;
        UInt32 dwFlags;
        UInt64 luid;
        Guid guidCorrelationId;
    }   

    /// <summary>
    /// https://docs.microsoft.com/en-us/windows/win32/api/ras/nf-ras-rasenumentriesw
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 4, CharSet = CharSet.Unicode)]
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
    /// <summary>
    /// RASMAN API entries for P/Invoke; includes a number of common constants.
    /// </summary>
    public static class RASMAN
    {
        public const int MAX_PATH = 260; // set where?
        public const int RAS_MaxDeviceName = 128;
        public const int RAS_MaxDeviceType = 16;
        public const int RAS_MaxEntryName = 256;

        [DllImport("RasAPI32.dll", CharSet = CharSet.Unicode)]
        public static extern UInt32 RasEnumConnectionsW([In, Out] RASCONNW[] array, ref UInt32 arrayByteSize, out UInt32 arrayCountWritten);
        public static RASCONNW[] RasEnumConnections()
        {
            int entrySize = Marshal.SizeOf(typeof(RASCONNW));

            UInt32 status = 0;
            UInt32 arrayByteSize = (UInt32)(entrySize*0);
            UInt32 arrayCount = 0;
            RASCONNW[] rasArray = null;
            do
            {
                arrayCount = (UInt32)(arrayByteSize / entrySize);
                rasArray = new RASCONNW[arrayCount];
                for (int i = 0; i < arrayCount; i++)
                {
                    rasArray[i].Init();
                }
                status = RASMAN.RasEnumConnectionsW(rasArray.Length > 0 ? rasArray : null, ref arrayByteSize, out arrayCount);
            }
            while (status == RASERROR.ERROR_TOO_SMALL); // 603==too small
            if (status != 0) throw new Exception($"ERROR: {System.Reflection.MethodBase.GetCurrentMethod().Name} status {status}");
            return rasArray;
        }


        [DllImport("RasAPI32.dll", CharSet = CharSet.Unicode)]
        public static extern UInt32 RasEnumEntriesW(string reserved1, string phonebook, [In, Out] RASENTRYNAMEW[] array, ref UInt32 arrayByteSize, out UInt32 arrayCountWritten);

        public static RASENTRYNAMEW[] RasEnumEntries()
        {
            int entrySize = Marshal.SizeOf(typeof(RASENTRYNAMEW));

            UInt32 status = 0;
            UInt32 arrayByteSize = (UInt32)(entrySize * 0);
            UInt32 arrayCount = 0;
            RASENTRYNAMEW[] rasArray = null;
            do
            {
                arrayCount = (UInt32)(arrayByteSize / entrySize);
                rasArray = new RASENTRYNAMEW[arrayCount];
                for (int i = 0; i < arrayCount; i++)
                {
                    rasArray[i].Init();
                }
                status = RASMAN.RasEnumEntriesW(null, null, rasArray.Length > 0 ? rasArray : null, ref arrayByteSize, out arrayCount);
            }
            while (status == RASERROR.ERROR_TOO_SMALL); // 603==too small
            if (status != 0) throw new Exception($"ERROR: {System.Reflection.MethodBase.GetCurrentMethod().Name} status {status}");
            return rasArray;
        }
    };

    /// <summary>
    /// Commonly needed RAS error definitions.
    /// </summary>
    public static class RASERROR
    {
        public const int RASBASE = 600;
        public const int ERROR_TOO_SMALL = RASBASE + 3; // 603
        public const int ERROR_INVALID_SIZE = RASBASE + 32; // 632
    }

}