﻿#if TOOLS
using System.Collections.Generic;
using Godot;
using Newtonsoft.Json;
using Yarn.GodotIntegration;
namespace YarnSpinnerGodot.addons.YarnSpinnerGodot.Editor
{
    [Tool]
    public class YarnCompileErrorsPropertyEditor : EditorProperty
    {
        // The main control for editing the property.
        private Label _propertyControl = new Label();
        // An internal value of the property.
        private List<YarnProjectError> _currentValue;

        public delegate void ErrorsUpdatedHandler(Object yarnProject);
        public event ErrorsUpdatedHandler OnErrorsUpdated;
        
        public YarnCompileErrorsPropertyEditor()
        {
            Label = "Project Errors";
            // Add the control as a direct child of EditorProperty node.
            AddChild(_propertyControl);
            // Make sure the control is able to retain the focus.
            AddFocusable(_propertyControl);
            // Setup the initial state and connect to the signal to track changes.
            RefreshControlText();
        }

        public override void UpdateProperty()
        {
            // Read the current value from the property.
            var newValue = (string)GetEditedObject().Get(GetEditedProperty());
            var parsedNewValue = JsonConvert.DeserializeObject<List<YarnProjectError>>(newValue);
            if (parsedNewValue == _currentValue)
            {
                return;
            }
            _currentValue = parsedNewValue;
            RefreshControlText();
            OnErrorsUpdated?.Invoke(GetEditedObject());
        }

        private void RefreshControlText()
        {
            if (_currentValue == null)
            {
                _propertyControl.Text = "";
            }
            else if(_currentValue.Count == 0)
            {
                _propertyControl.Text = "None";
            }
            else
            {
                _propertyControl.Text = $"{_currentValue.Count} error{(_currentValue.Count > 1 ? "s" : "")}";
            }
        }

        public void Refresh()
        {
            EmitChanged(GetEditedProperty(), GetEditedObject().Get(GetEditedProperty()));
        }
    }
#endif
}