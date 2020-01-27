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

using OpenWindesheart;
using OpenWindesheart.Models;
using OpenWindesheartDemoApp.Data.Interfaces;
using OpenWindesheartDemoApp.Models;
using OpenWindesheartDemoApp.Resources;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Xamarin.Forms;

namespace OpenWindesheartDemoApp.Services
{
    public class SamplesService
    {
        private readonly IHeartrateRepository _heartrateRepository;
        private readonly IStepsRepository _stepsRepository;
        private readonly ISleepRepository _sleepRepository;
        private DateTime _fetchingStartDate;
        private int _totalSamples = 0;

        public SamplesService(IHeartrateRepository heartrateRepository, IStepsRepository stepsRepository, ISleepRepository sleepRepository)
        {
            _heartrateRepository = heartrateRepository;
            _stepsRepository = stepsRepository;
            _sleepRepository = sleepRepository;
        }

        public void StartFetching()
        {
            Device.BeginInvokeOnMainThread(delegate
            {
                Application.Current.MainPage.Navigation.PopAsync();
                Globals.HomePageViewModel.FetchProgressVisible = true;
                Globals.HomePageViewModel.EnableDisableButtons(false);
                Globals.HomePageViewModel.IsLoading = true;
            });
            _fetchingStartDate = GetLastAddedDateTime();
            Windesheart.PairedDevice.GetSamples(_fetchingStartDate, FillDatabase, ProgressCalculator);
        }

        private void ProgressCalculator(int remainingSamples)
        {
            if (_totalSamples == 0)
            {
                _totalSamples = remainingSamples;
            }
            //Calculates percentage of progression. -10f to leave some space for DB insertion progress indication.
            float calculatedProgress = ((float)_totalSamples - (float)remainingSamples) / (float)_totalSamples;


            //Leave some space on progressbar for DB insertion
            if (calculatedProgress > 0.9f)
            {
                calculatedProgress = 0.9f;
            }

            Device.BeginInvokeOnMainThread(delegate
            {
                Globals.HomePageViewModel.ShowFetchProgress(calculatedProgress);
            });
        }

        private void FillDatabase(List<ActivitySample> samples)
        {
            Debug.WriteLine("Filling DB with samples: " + samples.Count);
            Globals.Database.Instance.BeginTransaction();
            foreach (var sample in samples)
            {
                var datetime = sample.Timestamp;

                AddHeartrate(datetime, sample);
                AddStep(datetime, sample);
                AddSleep(datetime, sample);
            }
            Globals.Database.Instance.Commit();
            Debug.WriteLine("DB filled with samples");
            Device.BeginInvokeOnMainThread(delegate
            {
                Globals.HomePageViewModel.IsLoading = false;
                Globals.HomePageViewModel.EnableDisableButtons(true);
                Globals.HomePageViewModel.ShowFetchProgress(1f);

            });
        }

        private DateTime GetLastAddedDateTime()
        {
            return _stepsRepository.LastAddedDatetime();
        }

        private void AddHeartrate(DateTime datetime, ActivitySample sample)
        {
            var heartRate = new Heartrate(datetime, sample.HeartRate != 255 ? sample.HeartRate : 0);
            _heartrateRepository.Add(heartRate);
        }

        private void AddStep(DateTime datetime, ActivitySample sample)
        {
            var step = new Step(datetime, sample.Steps);
            _stepsRepository.Add(step);
        }

        private void AddSleep(DateTime datetime, ActivitySample sample)
        {
            Sleep sleep;
            switch (sample.Category)
            {
                case 112:
                    sleep = new Sleep(datetime, SleepType.Light);
                    break;
                case 121:
                case 122:
                case 123:
                    sleep = new Sleep(datetime, SleepType.Deep);
                    break;

                default:
                    sleep = new Sleep(datetime, SleepType.Awake);
                    break;
            }
            _sleepRepository.Add(sleep);
        }
    }
}
