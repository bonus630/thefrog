using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace br.com.bonus630.thefrog.Utils
{
    [Flags]
    public enum DeviceCategory
    {
        KEY,
        XBOX,
        PS,
        BIGN,
        Generic
    }
    public static class DeviceDetector
    {
        //| Número | Xbox       | PlayStation  |
        //| ------ | ---------- | ------------ |
        //| 0      | A          | Cross(✕)    |
        //| 1      | B          | Circle(○)    |
        //| 2      | X          | Square(□)    |
        //| 3      | Y          | Triangle(△) |
        //| 4      | LB         | L1           |
        //| 5      | RB         | R1           |
        //| 6      | Back       | Share        |
        //| 7      | Start      | Options      |
        //| 8      | LS(click) | L3            |
        //| 9      | RS(click) | R3            |

        //{DEVICE}_{INPUT}
        //XBOX  // Xbox
        //PS   // PlayStation
        //BIGN  // Nintendo
        //KEY  // Teclado

        //S   // buttonSouth
        //E   // buttonEast
        //W   // buttonWest
        //N   // buttonNorth

        //L1  // leftShoulder
        //R1  // rightShoulder

        //L2  // leftTrigger
        //R2  // rightTrigger

        //L3  // leftStickButton
        //R3  // rightStickButton

        //START  // start
        //SELECT  // select

        //UP  // dpad up
        //DOWN  // dpad down
        //LEFT  // dpad left
        //RIGHT  // dpad right

        //------------------------------------------------------------------------------------------------
        //|Unity(Input System)   |   ABREVIAÇÃO  |   Xbox            |   PlayStation        |   Número   |
        //|--------------------------------------------------------------------------------------------- |
        //|buttonSouth           |    S          |      A            |      Cross(✕)       |      0     |
        //|buttonEast            |    E          |      B            |      Circle(○)       |      1     |
        //|buttonWest            |    W          |      X            |      Square(□)       |      2     |
        //|buttonNorth           |    N          |      Y            |      Triangle(△)    |      3     |
        //|leftShoulder          |    L1         |      LB           |       L1             |      4     |
        //|rightShoulder         |    R1         |      RB           |       R1             |      5     |
        //|leftTrigger           |    L2         |      LT           |       L2             |            |  
        //|rightTrigger          |    R2         |      RT           |       R2             |            |  
        //|startButton           |    START      |      Start        |       Options        |      7     |
        //|selectButton          |    SELECT     |      Back         |       Share          |      6     |
        //|leftStickButton       |    L3         |      LS(click)    |       L3             |            |  
        //|rightStickButton      |    R3         |      RS(click)    |       R3             |            |
        //-----------------------------------------------------------------------------------------------

        static readonly Dictionary<string, string> InputToShort = new()
        {
            { "buttonSouth", "S" },
            { "buttonEast", "E" },
            { "buttonWest", "W" },
            { "buttonNorth", "N" },

            { "leftShoulder", "L1" },
            { "rightShoulder", "R1" },

            { "leftTrigger", "L2" },
            { "rightTrigger", "R2" },

            { "leftStickButton", "L3" },
            { "rightStickButton", "R3" },

            { "startButton", "START" },
            { "selectButton", "SELECT" },

            { "dpad_up", "UP" },
            { "dpad_down", "DOWN" },
            { "dpad_left", "LEFT" },
            { "dpad_right", "RIGHT" },

            { "leftStick", "DPAD" },
          
        };
        public static readonly Dictionary<string, string> Xbox = new()
        {
            { "buttonSouth", "A" },
            { "buttonEast", "B" },
            { "buttonWest", "X" },
            { "buttonNorth", "Y" },

            { "leftShoulder", "LB" },
            { "rightShoulder", "RB" },

            { "leftTrigger", "LT" },
            { "rightTrigger", "RT" },

            { "startButton", "Start" },
            { "selectButton", "Back" },

            { "leftStickButton", "LS" },
            { "rightStickButton", "RS" },

            { "dpad_up", "DpadUp" },
            { "dpad_down", "DpadDown" },
            { "dpad_left", "DpadLeft" },
            { "dpad_right", "DpadRight" }
        };
        public static readonly Dictionary<string, string> PlayStation = new()
        {
            { "buttonSouth", "Cross" },
            { "buttonEast", "Circle" },
            { "buttonWest", "Square" },
            { "buttonNorth", "Triangle" },

            { "leftShoulder", "L1" },
            { "rightShoulder", "R1" },

            { "leftTrigger", "L2" },
            { "rightTrigger", "R2" },

            { "startButton", "Options" },
            { "selectButton", "Share" },

            { "leftStickButton", "L3" },
            { "rightStickButton", "R3" },

            { "dpad_up", "DpadUp" },
            { "dpad_down", "DpadDown" },
            { "dpad_left", "DpadLeft" },
            { "dpad_right", "DpadRight" }
        };
        public static DeviceCategory GetCategory(InputDevice device)
        {
            if (device == null)
                return DeviceCategory.Generic;

            if (device is Keyboard || device is Mouse)
                return DeviceCategory.KEY;

            switch (device.layout)
            {
                case "XInputControllerWindows":
                case "XInputController":
                    return DeviceCategory.XBOX;

                case "DualShockGamepadHID":
                case "DualSenseGamepadHID":
                    return DeviceCategory.PS;

                case "SwitchProControllerHID":
                    return DeviceCategory.BIGN;

                case "Gamepad":
                case "HID":
                case "Joystick":
                    return DeviceCategory.Generic;
            }

            return DeviceCategory.Generic;
        }
        public static string GetBindingNameForDevice(InputAction action, InputDevice device)
        {
            foreach (var binding in action.bindings)
            {
                // ignora bindings compostos (WASD etc)
                if (binding.isComposite || binding.isPartOfComposite)
                    continue;

                // verifica se o binding é compatível com o device
                if (!InputControlPath.Matches(binding.effectivePath, device))
                    continue;

                return InputControlPath.ToHumanReadableString(
                    binding.effectivePath,
                    InputControlPath.HumanReadableStringOptions.OmitDevice
                );
            }

            return null;
        }
        public static string GetControlName(InputAction action, InputDevice device)
        {
            var control = action.controls
                .FirstOrDefault(c => c.device == device);

            if (control == null)
                return null;
            return control.name; // "buttonSouth", "space", etc
        }

        public static string GetReplacedName(DeviceCategory category, string controlName)
        {
            string result = "";
            switch (category)
            {
                case DeviceCategory.XBOX:
                    InputToShort.TryGetValue(controlName, out result);
                    return result;
                case DeviceCategory.PS:
                    InputToShort.TryGetValue(controlName, out result);
                    return result;
            }
            return controlName;
        }
    }
}

