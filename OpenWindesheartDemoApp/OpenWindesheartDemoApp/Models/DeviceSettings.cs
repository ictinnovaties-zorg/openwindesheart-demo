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

using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace OpenWindesheartDemoApp.Models
{
    public class DeviceSettings
    {
        private static ISettings AppSettings => CrossSettings.Current;

        public static bool WristRaiseDisplay
        {
            get => AppSettings.GetValueOrDefault(nameof(WristRaiseDisplay), false);
            set => AppSettings.AddOrUpdateValue(nameof(WristRaiseDisplay), value);
        }

        public static string DeviceLanguage
        {
            get => AppSettings.GetValueOrDefault(nameof(DeviceLanguage), "en-EN");
            set => AppSettings.AddOrUpdateValue(nameof(DeviceLanguage), value);
        }

        public static bool DateFormatDMY
        {
            get => AppSettings.GetValueOrDefault(nameof(DateFormatDMY), true);
            set => AppSettings.AddOrUpdateValue(nameof(DateFormatDMY), value);
        }

        public static bool TimeFormat24Hour
        {
            get => AppSettings.GetValueOrDefault(nameof(TimeFormat24Hour), true);
            set => AppSettings.AddOrUpdateValue(nameof(TimeFormat24Hour), value);
        }

        public static int DailyStepsGoal
        {
            get => AppSettings.GetValueOrDefault(nameof(DailyStepsGoal), 10000);
            set => AppSettings.AddOrUpdateValue(nameof(DailyStepsGoal), value);
        }
    }
}
