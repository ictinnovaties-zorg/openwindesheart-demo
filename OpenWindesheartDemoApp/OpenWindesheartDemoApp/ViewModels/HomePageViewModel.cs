/* Copyright 2020 Research group ICT innovations in Health Care

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License. */

using OpenWindesheart;
using OpenWindesheart.Models;
using OpenWindesheartDemoApp.Resources;
using OpenWindesheartDemoApp.Views;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace OpenWindesheartDemoApp.ViewModels
{
    public class HomePageViewModel : INotifyPropertyChanged
    {
        private int _battery;
        private int _heartrate;
        private string _batteryImage = "";
        private bool _isLoading;
        private string _bandnameLabel;
        private float _fetchProgress;
        private bool _fetchProgressVisible;
        public event PropertyChangedEventHandler PropertyChanged;

        public HomePageViewModel()
        {
            if (Windesheart.PairedDevice != null)
            {
                if (Windesheart.PairedDevice.IsConnected())
                {
                    ReadCurrentBattery();
                    BandNameLabel = Windesheart.PairedDevice.Name;
                }
            }
        }

        public async Task ReadCurrentBattery()
        {
            //catch!!
            var battery = await Windesheart.PairedDevice.GetBattery();
            UpdateBattery(battery);
        }

        public void UpdateBattery(BatteryData battery)
        {
            if (battery.Percentage == 0)
            {
                BatteryImage = "";
                return;
            }

            Battery = battery.Percentage;
            if (battery.Status == BatteryStatus.Charging)
            {
                BatteryImage = "BatteryCharging.png";
                return;
            }

            if (battery.Percentage >= 0 && battery.Percentage < 26)
            {
                BatteryImage = "BatteryQuart.png";
            }
            else if (battery.Percentage >= 26 && battery.Percentage < 51)
            {
                BatteryImage = "BatteryHalf.png";
            }
            else if (battery.Percentage >= 51 && battery.Percentage < 76)
            {
                BatteryImage = "BatteryThreeQuarts.png";
            }
            else if (battery.Percentage >= 76)
            {
                BatteryImage = "BatteryFull.png";
            }
        }


        private void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                _isLoading = value;
                OnPropertyChanged();
            }
        }

        public string BatteryImage
        {
            get => _batteryImage;
            set
            {
                _batteryImage = value;
                OnPropertyChanged();
            }
        }

        public int Battery
        {
            get => _battery;
            set
            {
                _battery = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(DisplayBattery));
            }
        }
        public int Heartrate
        {
            get => _heartrate;
            set
            {
                _heartrate = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(DisplayHeartRate));
            }
        }

        public string BandNameLabel
        {
            get => _bandnameLabel;
            set
            {
                _bandnameLabel = value;
                OnPropertyChanged();
            }
        }

        public float FetchProgress
        {
            get => _fetchProgress;
            set
            {
                _fetchProgress = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(DisplayFetchProgress));


            }
        }

        public bool FetchProgressVisible
        {
            get => _fetchProgressVisible;
            set
            {
                _fetchProgressVisible = value;
                OnPropertyChanged();
            }
        }
        public string DisplayHeartRate => Heartrate != 0 ? $"Last Heartbeat: {Heartrate.ToString()}" : "";

        public string DisplayBattery => Battery != 0 ? $"{Battery.ToString()}%" : "";

        public float DisplayFetchProgress => FetchProgress != 0 ? FetchProgress : 0;

        public async void AboutButtonClicked(object sender, EventArgs args)
        {
            EnableDisableButtons(false);

            IsLoading = true;
            await Application.Current.MainPage.Navigation.PushAsync(new AboutPage());
            IsLoading = false;
            EnableDisableButtons(true);

        }
        public async void SettingsButtonClicked(object sender, EventArgs args)
        {
            EnableDisableButtons(false);
            IsLoading = true;
            await Application.Current.MainPage.Navigation.PushAsync(new SettingsPage()
            {
                BindingContext = Globals.SettingsPageViewModel
            });
            IsLoading = false;
            EnableDisableButtons(true);
        }

        public async void StepsButtonClicked(object sender, EventArgs args)
        {
            EnableDisableButtons(false);
            IsLoading = true;
            await Application.Current.MainPage.Navigation.PushAsync(new StepsPage()
            {
                BindingContext = Globals.StepsPageViewModel
            });
            IsLoading = false;
            EnableDisableButtons(true);
        }

        public async void HeartrateButtonClicked(object sender, EventArgs args)
        {
            EnableDisableButtons(false);
            IsLoading = true;
            await Application.Current.MainPage.Navigation.PushAsync(new HeartratePage()
            {
                BindingContext = Globals.HeartratePageViewModel
            });
            IsLoading = false;
            EnableDisableButtons(true);


        }

        public async void SleepButtonClicked(object sender, EventArgs args)
        {
            EnableDisableButtons(false);

            IsLoading = true;
            await Application.Current.MainPage.Navigation.PushAsync(new SleepPage()
            {
                BindingContext = Globals.SleepPageViewModel
            });
            IsLoading = false;
            EnableDisableButtons(true);

        }

        public async void DeviceButtonClicked(object sender, EventArgs args)
        {
            EnableDisableButtons(false);

            IsLoading = true;
            await Application.Current.MainPage.Navigation.PushAsync(new DevicePage()
            {
                BindingContext = Globals.DevicePageViewModel
            });
            IsLoading = false;
            EnableDisableButtons(true);


        }

        public void EnableDisableButtons(bool enable)
        {
            HomePage.SleepButton.IsEnabled = enable;
            HomePage.AboutButton.IsEnabled = enable;
            HomePage.SettingsButton.IsEnabled = enable;
            HomePage.StepsButton.IsEnabled = enable;
            HomePage.HeartrateButton.IsEnabled = enable;
            HomePage.DeviceButton.IsEnabled = enable;
        }

        public void ShowFetchProgress(float progress)
        {
            FetchProgress = progress;
            FetchProgressVisible = progress != 1f;
        }
    }
}
