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

using OpenWindesheartDemoApp.Models;
using OpenWindesheartDemoApp.Resources;
using SQLite;
using System.IO;

namespace OpenWindesheartDemoApp.Data
{
    public class Database
    {
        private string _dbPath;

        public SQLiteConnection Instance;

        public Database()
        {
            CreateDatabase();
        }

        public void EmptyDatabase()
        {
            //Transaction for emptying DB-data
            Instance.BeginTransaction();
            Globals.HeartrateRepository.RemoveAll();
            Globals.StepsRepository.RemoveAll();
            Globals.SleepRepository.RemoveAll();
            Instance.Commit();
        }

        private void CreateDatabase()
        {
            //Set DbPath
            _dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal),
                "WindesHeart.db");

            //Set Database
            Instance = new SQLiteConnection(_dbPath);

            //Create the tables if not existing
            Instance.CreateTable<Heartrate>();
            Instance.CreateTable<Step>();
            Instance.CreateTable<Sleep>();
        }
    }
}
