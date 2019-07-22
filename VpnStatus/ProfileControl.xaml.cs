using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Networking.Vpn;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace VpnStatus
{
    public sealed partial class ProfileControl : UserControl
    {
        public ProfileControl()
        {
            this.InitializeComponent();
            this.Loaded += ProfileControl_Loaded;
        }
        private void Log(string str)
        {
            System.Diagnostics.Debug.WriteLine(str);
        }

        IVpnProfile Profile = null;
        private void ProfileControl_Loaded(object sender, RoutedEventArgs e)
        {
            Profile = this.DataContext as IVpnProfile;

            uiName.Text = Profile.ProfileName;

            SetupNativeProfile(Profile as VpnNativeProfile);
            SetupPluginProfile(Profile as VpnPlugInProfile);
        }
        //
        //
        // Summary:
        //     The profile is disconnected.
        //Disconnected = 0,
        //
        // Summary:
        //     The profile is in the process of disconnecting.
        //Disconnecting = 1,
        //
        // Summary:
        //     The profile is connected.
        //Connected = 2,
        //
        // Summary:
        //     The profile is in the process of connecting.
        //Connecting = 3
        string[] StatusIcons = new string[]
        {
            "DIS",
            "ding",
            "CON",
            "cing",
        };

        private void SetupNativeProfile(VpnNativeProfile profile)
        {
            if (profile == null) return;
            try
            {
                var connectionStatus = profile.ConnectionStatus;
                uiStatus.Text = StatusIcons[(int)connectionStatus];
                Log($"{profile.ConnectionStatus}");
            }
            catch (Exception ex)
            {
                Log($"ERROR: can't get connection status {ex.Message} :-(");
            }
        }
        private void SetupPluginProfile (VpnPlugInProfile profile)
        {
            if (profile == null) return;
            try
            {
                var connectionStatus = profile.ConnectionStatus;
                uiStatus.Text = StatusIcons[(int)connectionStatus];
                Log($"{profile.ConnectionStatus}");
            }
            catch (Exception ex)
            {
                Log($"ERROR: can't get connection status {ex.Message} :-(");
            }
        }
    }
}
