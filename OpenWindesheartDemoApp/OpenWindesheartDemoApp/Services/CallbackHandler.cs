﻿/* Copyright 2020 Research group ICT innovations in Health Care

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
using OpenWindesheartDemoApp.Models;
using OpenWindesheartDemoApp.Resources;
using OpenWindesheartDemoApp.Views;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Xamarin.Forms;

namespace OpenWindesheartDemoApp.Services
{
    public static class CallbackHandler
    {
        public static void OnHeartrateUpdated(HeartrateData heartrate)
        {
            if (heartrate.Heartrate == 0)
                return;
            Globals.HomePageViewModel.Heartrate = heartrate.Heartrate;
        }

        public static void OnBatteryUpdated(BatteryData battery)
        {
            Globals.HomePageViewModel.UpdateBattery(battery);
        }

        public static void OnStepsUpdated(StepData stepsInfo)
        {
            var count = stepsInfo.StepCount;
            Globals.StepsPageViewModel.OnStepsUpdated(count);
        }

        public static async void OnConnect(ConnectionResult result, byte[] secretKey)
        {
            if (result == ConnectionResult.Succeeded)
            {
                try
                {
                    //Sync settings
                    Windesheart.PairedDevice.SetTime(DateTime.Now);
                    Windesheart.PairedDevice.SetDateDisplayFormat(DeviceSettings.DateFormatDMY);
                    Windesheart.PairedDevice.SetLanguage(DeviceSettings.DeviceLanguage);
                    Windesheart.PairedDevice.SetTimeDisplayFormat(DeviceSettings.TimeFormat24Hour);
                    Windesheart.PairedDevice.SetActivateOnLiftWrist(DeviceSettings.WristRaiseDisplay);
                    Windesheart.PairedDevice.SetStepGoal(DeviceSettings.DailyStepsGoal);
                    Windesheart.PairedDevice.EnableFitnessGoalNotification(true);
                    Windesheart.PairedDevice.EnableSleepTracking(true);
                    Windesheart.PairedDevice.SetHeartrateMeasurementInterval(1);

                    Globals.HomePageViewModel.ReadCurrentBattery();
                    Globals.HomePageViewModel.BandNameLabel = Windesheart.PairedDevice.Name;

                    //Callbacks
                    Windesheart.PairedDevice.EnableRealTimeHeartrate(OnHeartrateUpdated);
                    Windesheart.PairedDevice.EnableRealTimeBattery(OnBatteryUpdated);
                    Windesheart.PairedDevice.EnableRealTimeSteps(OnStepsUpdated);
                    Windesheart.PairedDevice.SubscribeToDisconnect(OnDisconnect);

                    Globals.DevicePageViewModel.StatusText = "Connected";
                    Globals.DevicePageViewModel.DeviceList = new ObservableCollection<BLEScanResult>();
                    Device.BeginInvokeOnMainThread(delegate
                    {
                        DevicePage.DisconnectButton.IsEnabled = true;
                    });
                    Globals.DevicePageViewModel.IsLoading = false;
                    Globals.SamplesService.StartFetching();
                    SaveGuid(secretKey);
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                    Debug.WriteLine("Something went wrong while connecting to device, disconnecting...");
                    Windesheart.PairedDevice?.Disconnect();
                    Globals.DevicePageViewModel.IsLoading = false;
                }
            }
            else if (result == ConnectionResult.Failed)
            {
                Debug.WriteLine("Connection failed");
                if (Windesheart.PairedDevice.SecretKey != null && Application.Current.Properties.ContainsKey(Windesheart.PairedDevice.IDevice.Uuid.ToString()))
                {
                    Application.Current.Properties.Remove(Windesheart.PairedDevice.IDevice.Uuid.ToString());
                    Device.BeginInvokeOnMainThread(delegate
                    {
                        Application.Current.MainPage.DisplayAlert("Connecting failed, Please try again", "The secret key of this device has changed since the last time it was connected to this phone. Please try connecting to this device again or try to factory reset the device!", "OK");
                    });
                }
                Windesheart.PairedDevice?.Disconnect();
                Globals.DevicePageViewModel.StatusText = "Disconnected";
                Device.BeginInvokeOnMainThread(delegate
                {
                    DevicePage.DisconnectButton.IsEnabled = true;
                    Globals.HomePageViewModel.IsLoading = false;
                    Globals.HomePageViewModel.EnableDisableButtons(true);
                    Globals.DevicePageViewModel.ScanButtonText = "Scan for devices";
                    DevicePage.ScanButton.IsEnabled = true;
                    DevicePage.ReturnButton.IsVisible = true;
                });
                Globals.DevicePageViewModel.IsLoading = false;
            }
        }


        public static void SaveGuid(byte[] secretKey)
        {
            if (!Application.Current.Properties.ContainsKey(Windesheart.PairedDevice.IDevice.Uuid.ToString()))
            {
                Application.Current.Properties.Add(Windesheart.PairedDevice.IDevice.Uuid.ToString(), secretKey);
                Application.Current.SavePropertiesAsync();
            }
        }

        public static void OnDisconnect(Object obj)
        {
            Globals.DevicePageViewModel.StatusText = "Disconnected";
            Globals.HomePageViewModel.BandNameLabel = "";
            Globals.HomePageViewModel.Battery = 0;
            Globals.HomePageViewModel.BatteryImage = "";
        }
    }
}