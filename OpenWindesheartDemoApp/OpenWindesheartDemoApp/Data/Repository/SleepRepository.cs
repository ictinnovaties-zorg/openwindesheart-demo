/* Copyright 2020 Research group ICT innovations in Health Care, Windesheim University of Applied Sciences

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License. */

using OpenWindesheartDemoApp.Data.Interfaces;
using OpenWindesheartDemoApp.Models;
using System.Collections.Generic;

namespace OpenWindesheartDemoApp.Data.Repository
{
    public class SleepRepository : ISleepRepository
    {
        private readonly Database _database;
        public SleepRepository(Database database)
        {
            _database = database;
        }

        public void Add(Sleep sleep)
        {
            var query = "INSERT INTO Sleep(DateTime, SleepType) VALUES(?,?)";
            var command = _database.Instance.CreateCommand(query, sleep.DateTime, sleep.SleepType);
            command.ExecuteNonQuery();
        }

        public IEnumerable<Sleep> GetAll()
        {
            return _database.Instance.Table<Sleep>().OrderBy(x => x.DateTime).ToList();
        }

        public void RemoveAll()
        {
            var sleeps = GetAll();
            foreach (var sleep in sleeps)
            {
                var query = "DELETE FROM Sleep WHERE Id = ?";
                var command = _database.Instance.CreateCommand(query, sleep.Id);
                command.ExecuteNonQuery();
            }
        }
    }
}