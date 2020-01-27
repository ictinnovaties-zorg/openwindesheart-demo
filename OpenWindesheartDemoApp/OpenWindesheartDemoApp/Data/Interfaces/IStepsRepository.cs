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

using OpenWindesheartDemoApp.Models;
using System;
using System.Collections.Generic;

namespace OpenWindesheartDemoApp.Data.Interfaces
{
    public interface IStepsRepository
    {
        IEnumerable<Step> GetAll();
        void Add(Step step);
        void RemoveAll();
        DateTime LastAddedDatetime();
    }
}
