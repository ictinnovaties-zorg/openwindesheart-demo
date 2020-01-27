﻿/* Copyright 2020 Research group ICT innovations in Health Care, Windesheim University of Applied Sciences

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License. */

using Microcharts;
using OpenWindesheartDemoApp.Models;
using OpenWindesheartDemoApp.Resources;
using OpenWindesheartDemoApp.Views;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.Forms;
using Entry = Microcharts.Entry;

namespace OpenWindesheartDemoApp.ViewModels
{
    public class SleepPageViewModel : INotifyPropertyChanged
    {
        public static IEnumerable<Sleep> SleepInfo = new List<Sleep>();
        public DateTime StartDate { get; }
        public DateTime SelectedDate;
        public event PropertyChangedEventHandler PropertyChanged;

        public string AwakeColor = "#ffffff";
        public string LightColor = "#1281ff";
        public string DeepColor = "#002bba";

        private ButtonRow _buttonRow;
        private Chart _chart;
        private bool _isLoading;

        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                _isLoading = value;
                OnPropertyChanged();
            }
        }

        public Chart Chart
        {
            get => _chart;
            set
            {
                _chart = value;
                OnPropertyChanged();
            }
        }

        public async void OnAppearing()
        {
            //Get all sleep data from DB
            SleepInfo = Globals.SleepRepository.GetAll();

            if (!SleepInfo.Any())
            {
                await Application.Current.MainPage.DisplayAlert("No data", "Unfortunately, no sleep-data was found.", "Ok");
            }

            //Init buttons on bottom
            List<Button> dayButtons = new List<Button>
            {
                SleepPage.Day1Button,
                SleepPage.Day2Button,
                SleepPage.Day3Button,
                SleepPage.Day4Button,
                SleepPage.Day5Button,
                SleepPage.Day6Button,
                SleepPage.TodayButton
            };
            _buttonRow = new ButtonRow(dayButtons);

            //Switch to today
            TodayBtnClick(SleepPage.TodayButton, new EventArgs());

            //Update chart in other thread
            IsLoading = true;
            await Task.Run(() =>
            {
                var data = GetData();
                Device.BeginInvokeOnMainThread(() =>
                {
                    UpdateChart(data);
                });
            });
        }

        void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public SleepPageViewModel()
        {
            StartDate = DateTime.Today;
            SelectedDate = StartDate;
        }

        private async Task UpdateInfo()
        {
            IsLoading = true;

            if (SelectedDate == StartDate)
            {
                SleepPage.CurrentDayLabel.Text = "Today";
            }
            else if (SelectedDate >= StartDate.AddDays(-6))
            {
                SleepPage.CurrentDayLabel.Text = SelectedDate.DayOfWeek.ToString();
            }
            else
            {
                SleepPage.CurrentDayLabel.Text = SelectedDate.ToString("dd/MM/yyyy");
            }

            //Update chart in other thread
            await Task.Run(() =>
            {
                var data = GetData();
                Device.BeginInvokeOnMainThread(() =>
                {
                    UpdateChart(data);
                });
            });
        }

        private List<Sleep> GetCurrentSleep()
        {
            return SleepInfo.Where(s => s.DateTime.Year == SelectedDate.Year &&
            s.DateTime.Month == SelectedDate.Month &&
            s.DateTime > SelectedDate.AddHours(-4) &&
            s.DateTime < SelectedDate.AddHours(12)).
            OrderBy(x => x.DateTime).ToList();
        }

        private List<Entry> GetData()
        {
            List<Sleep> sleepData = GetCurrentSleep();
            List<Entry> entries = new List<Entry>();

            //For each hour
            for (int i = 20; i < 36; i++)
            {
                int hour = i;
                if (i >= 24) hour -= 24;

                //Get sleep data for that hour
                List<Sleep> data = sleepData.Where(x => x.DateTime.Hour == hour).ToList();

                for (int j = 0; j < 60; j++)
                {
                    if (data.ElementAtOrDefault(j) != null)
                    {
                        switch (data[j].SleepType)
                        {
                            case SleepType.Awake:
                                Entry awakeEntry = new Entry(1) { Color = SKColor.Parse(AwakeColor) };
                                entries.Add(awakeEntry);
                                break;
                            case SleepType.Light:
                                Entry lightEntry = new Entry(1) { Color = SKColor.Parse(LightColor) };
                                entries.Add(lightEntry);
                                break;
                            case SleepType.Deep:
                                Entry deepEntry = new Entry(1) { Color = SKColor.Parse(DeepColor) };
                                entries.Add(deepEntry);
                                break;
                        }
                    }
                    else
                    {
                        Entry entry = new Entry(1) { Color = SKColor.Parse(AwakeColor) };
                        entries.Add(entry);
                    }
                }
            }
            return entries;
        }

        public void UpdateChart(List<Entry> entries)
        {
            Chart = new BarChart
            {
                Entries = entries,
                BackgroundColor = SKColors.Transparent,
                Margin = 0
            };
            IsLoading = false;
        }

        public async void PreviousDayBtnClick(object sender, EventArgs args)
        {
            Debug.WriteLine("Previous day clicked!");

            //You can always go back
            _buttonRow.ToPrevious();
            SelectedDate = SelectedDate.AddDays(-1);
            await UpdateInfo();
        }

        public async void NextDayBtnClick(object sender, EventArgs args)
        {
            //If already today, you cant go next
            if (_buttonRow.ToNext())
            {
                SelectedDate = SelectedDate.AddDays(1);
                await UpdateInfo();
            }
            else
            {
                Application.Current.MainPage.DisplayAlert("Sleep", "Unfortunately, you can't foresee the future.", "Ok");
            }
        }

        public async void TodayBtnClick(object sender, EventArgs args)
        {
            SelectedDate = StartDate;
            if (_buttonRow.SwitchTo(sender as Button))
            {
                await UpdateInfo();
            }
        }

        public async void Day6BtnClick(object sender, EventArgs args)
        {
            SelectedDate = StartDate.AddDays(-1);
            if (_buttonRow.SwitchTo(sender as Button))
            {
                await UpdateInfo();
            }
        }

        public async void Day5BtnClick(object sender, EventArgs args)
        {
            SelectedDate = StartDate.AddDays(-2);
            if (_buttonRow.SwitchTo(sender as Button))
            {
                await UpdateInfo();
            }
        }

        public async void Day4BtnClick(object sender, EventArgs args)
        {
            SelectedDate = StartDate.AddDays(-3);
            if (_buttonRow.SwitchTo(sender as Button))
            {
                await UpdateInfo();
            }
        }

        public async void Day3BtnClick(object sender, EventArgs args)
        {
            SelectedDate = StartDate.AddDays(-4);
            if (_buttonRow.SwitchTo(sender as Button))
            {
                await UpdateInfo();
            }
        }

        public async void Day2BtnClick(object sender, EventArgs args)
        {
            SelectedDate = StartDate.AddDays(-5);
            if (_buttonRow.SwitchTo(sender as Button))
            {
                await UpdateInfo();
            }
        }

        public async void Day1BtnClick(object sender, EventArgs args)
        {
            SelectedDate = StartDate.AddDays(-6);
            if (_buttonRow.SwitchTo(sender as Button))
            {
                await UpdateInfo();
            }
        }
    }
}
