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
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenWindesheartDemoApp.Data.Repository
{
    public class StepsRepository : IStepsRepository
    {
        private readonly Database _database;

        public StepsRepository(Database database)
        {
            _database = database;
        }

        public void Add(Step step)
        {
            var query = "INSERT INTO Steps(DateTime, StepCount) VALUES(?,?)";
            var command = _database.Instance.CreateCommand(query, step.DateTime, step.StepCount);
            command.ExecuteNonQuery();
        }

        public IEnumerable<Step> GetAll()
        {
            return _database.Instance.Table<Step>().OrderBy(x => x.DateTime).ToList();
        }

        public DateTime LastAddedDatetime()
        {
            var steps = GetAll().ToArray();
            return steps.Any() ? steps.Last().DateTime.AddMinutes(1) : DateTime.Now.AddYears(-2);
        }

        public void RemoveAll()
        {
            var steps = GetAll();
            foreach (var step in steps)
            {
                var query = "DELETE FROM Steps WHERE Id = ?";
                var command = _database.Instance.CreateCommand(query, step.Id);
                command.ExecuteNonQuery();
            }
        }
    }
}
