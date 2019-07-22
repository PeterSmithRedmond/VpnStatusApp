using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Networking.Vpn;
using Windows.Storage.Provider;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace VpnStatus
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public ObservableCollection<IVpnProfile> Profiles { get; } = new ObservableCollection<IVpnProfile>();
        public MainPage()
        {
            this.InitializeComponent();
            this.Loaded += MainPage_Loaded;
        }
        private void Log (string str)
        {
            uiStatus.Text += str + "\r\n";
        }
        VpnManagementAgent Agent = null; 
        private async void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = this;
            await LoadVpn();
        }

        private async void OnRetry(object sender, RoutedEventArgs e)
        {
            await LoadVpn();
        }

        private async Task LoadVpn()
        {
            Profiles.Clear();
            Agent = new VpnManagementAgent();
            var profiles = await Agent.GetProfilesAsync();
            foreach (var profile in profiles)
            {
                Log($"PROFILE: {profile.ProfileName}");
                Profiles.Add(profile);
            }

        }
    }
}
