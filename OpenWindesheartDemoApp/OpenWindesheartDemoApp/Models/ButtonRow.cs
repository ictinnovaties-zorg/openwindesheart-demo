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

using System.Collections.Generic;
using Xamarin.Forms;

namespace OpenWindesheartDemoApp.Models
{
    public class ButtonRow
    {
        private readonly List<Button> _buttons;
        private int _selectedIndex;

        public ButtonRow(List<Button> buttons)
        {
            _buttons = buttons;
            _selectedIndex = buttons.Count - 1;
        }

        public bool ToNext()
        {
            //If there is another button left
            if (_selectedIndex + 1 < _buttons.Count)
            {
                //If there was an old button
                if (_selectedIndex >= 0)
                {
                    _buttons[_selectedIndex].IsEnabled = true; //enable old button
                }

                _selectedIndex++;

                //If there is a new button
                if (_selectedIndex >= 0)
                {
                    _buttons[_selectedIndex].IsEnabled = false; //disable new button
                }
                return true;
            }
            return false;
        }

        public bool ToPrevious()
        {
            //If old index is 0 or above
            if (_selectedIndex >= 0)
            {
                _buttons[_selectedIndex].IsEnabled = true; //enable old button
                _selectedIndex--;

                //If new index is 0 or above
                if (_selectedIndex >= 0)
                {
                    _buttons[_selectedIndex].IsEnabled = false; //disable new button
                }
                return true;
            }
            else
            {
                _selectedIndex--;
                return false;
            }
        }

        public bool SwitchTo(Button b)
        {
            //If that button exists, get the index
            int index = _buttons.FindIndex(button => button.Equals(b));
            if (index >= 0)
            {
                if (_selectedIndex >= 0)
                {
                    _buttons[_selectedIndex].IsEnabled = true; //enable old button
                }

                _selectedIndex = index;
                _buttons[_selectedIndex].IsEnabled = false; //disable new button
                return true;
            }
            return false;
        }
    }
}
