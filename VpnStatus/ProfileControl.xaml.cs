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

        IVpnProfile Profile = null;
        private void ProfileControl_Loaded(object sender, RoutedEventArgs e)
        {
            Profile = this.DataContext as IVpnProfile;

            uiName.Text = Profile.ProfileName;
        }
    }
}
