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

using SQLite;
using System;

namespace OpenWindesheartDemoApp.Models
{
    [Table("Steps")]
    public class Step
    {
        public Step() { } //Needed for DB

        public Step(DateTime datetime, int stepCount)
        {
            DateTime = datetime;
            StepCount = stepCount;
        }
        [PrimaryKey, AutoIncrement, Column("Id")]
        public int Id { get; set; }

        [Column("DateTime")]
        public DateTime DateTime { get; set; }

        [Column("StepCount")]
        public int StepCount { get; set; }
    }
}
