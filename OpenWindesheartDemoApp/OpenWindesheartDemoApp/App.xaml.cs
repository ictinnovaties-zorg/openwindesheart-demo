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

using FormsControls.Base;
using OpenWindesheartDemoApp.Data;
using OpenWindesheartDemoApp.Data.Repository;
using OpenWindesheartDemoApp.Resources;
using OpenWindesheartDemoApp.Views;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;

namespace OpenWindesheartDemoApp
{
    public partial class App
    {
        public App()
        {
            InitializeComponent();
            var database = new Database();
            Globals.BuildGlobals(new HeartrateRepository(database), new SleepRepository(database), new StepsRepository(database), database);
            //database.EmptyDatabase();
            MainPage = new AnimationNavigationPage(new HomePage());
        }

        protected override void OnStart()
        {
            //Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }

        public static async void RequestLocationPermission()
        {
            var permissionStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Location);
            if (permissionStatus != PermissionStatus.Granted)
            {
                await CrossPermissions.Current.RequestPermissionsAsync(Permission.Location);
            }
        }
    }
}
