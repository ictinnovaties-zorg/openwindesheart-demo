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

using OpenWindesheartDemoApp.Data;
using OpenWindesheartDemoApp.Data.Interfaces;
using OpenWindesheartDemoApp.Services;
using OpenWindesheartDemoApp.ViewModels;
using System.Collections.Generic;
using Xamarin.Forms;

namespace OpenWindesheartDemoApp.Resources
{
    //General class for the App
    public static class Globals
    {
        public static HeartRatePageViewModel HeartratePageViewModel;
        public static SamplesService SamplesService { get; private set; }
        public static DevicePageViewModel DevicePageViewModel;
        public static HomePageViewModel HomePageViewModel;
        public static SettingsPageViewModel SettingsPageViewModel;
        public static StepsPageViewModel StepsPageViewModel;
        public static SleepPageViewModel SleepPageViewModel;

        public static double ScreenHeight { get; set; }
        public static double ScreenWidth { get; set; }
        public static Color PrimaryColor { get; set; } = Color.FromHex("#96d1ff");
        public static Color SecondaryColor { get; set; } = Color.FromHex("#53b1ff");
        public static Color LightTextColor { get; set; } = Color.FromHex("#999999");
        public static IStepsRepository StepsRepository { get; set; }
        public static ISleepRepository SleepRepository { get; set; }
        public static IHeartrateRepository HeartrateRepository { get; set; }
        public static Dictionary<string, string> LanguageDictionary;
        public static Database Database;

        public static void BuildGlobals(IHeartrateRepository heartrateRepository, ISleepRepository sleepRepository, IStepsRepository stepsRepository, Database database)
        {
            StepsRepository = stepsRepository;
            SleepRepository = sleepRepository;
            HeartrateRepository = heartrateRepository;
            Database = database;

            HeartratePageViewModel = new HeartRatePageViewModel(HeartrateRepository);
            SamplesService = new SamplesService(HeartrateRepository, StepsRepository, SleepRepository);
            StepsPageViewModel = new StepsPageViewModel();
            SettingsPageViewModel = new SettingsPageViewModel();
            SleepPageViewModel = new SleepPageViewModel();
            DevicePageViewModel = new DevicePageViewModel();
            HomePageViewModel = new HomePageViewModel();

            LanguageDictionary = new Dictionary<string, string>
            {
                {"Nederlands", "nl-NL"},
                {"English", "en-EN"},
                {"Deutsch", "de-DE"}
            };
        }
    }
}