using System;
using System.Collections.Generic;
using System.Text;
using MOIS;

namespace GlueEngine.Core
{
    public class InputManager
    {
        private MOIS.InputManager input;
        private Keyboard keyboard;
        private Mouse mouse;

        public Keyboard Keyboard
        {
            get
            {
                return this.keyboard;
            }
        }

        public Mouse Mouse
        {
            get
            {
                return this.mouse;
            }
        }

        public InputManager()
        {
        }

        public bool Initiliase(IntPtr windowHandle)
        {
            this.input = MOIS.InputManager.CreateInputSystem((uint)windowHandle.ToInt32());
            
            //Create all devices (We only catch joystick exceptions here, as, most people have Key/Mouse)
            this.keyboard = (Keyboard)input.CreateInputObject(MOIS.Type.OISKeyboard, true);
            this.mouse = (Mouse)input.CreateInputObject(MOIS.Type.OISMouse, true);
            return true;
        }

        public void Dispose()
        {
            this.input.DestroyInputObject(keyboard);
            this.input.DestroyInputObject(mouse);
            MOIS.InputManager.DestroyInputSystem(input);
        }

        public void Update(float deltaTime)
        {
            this.keyboard.Capture();
            this.mouse.Capture();
        }

        public bool IsKeyDown(KeyCode keyCode)
        {
            return keyboard.IsKeyDown(keyCode);
        }
    }
}
