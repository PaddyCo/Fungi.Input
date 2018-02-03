using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace Fungi.Input
{
    class Action
    {
        public string Name;

        private List<Keys> boundKeys;
        private List<Buttons> boundButtons;

        public Action(string name)
        {
            Name = name;
            boundKeys = new List<Keys>();
            boundButtons = new List<Buttons>();
        }

        public void Bind(Keys key)
        {
            if (!boundKeys.Contains(key))
            {
                boundKeys.Add(key);
            }
        }

        public void Bind(Buttons button)
        {
            if (!boundButtons.Contains(button))
            {
                boundButtons.Add(button);
            }
        }

        public bool IsPressed(KeyboardState keyboardState, GamePadState gamePadState)
        {
            foreach (var key in boundKeys)
            {
                if (keyboardState.IsKeyDown(key))
                {
                    return true;
                }
            }
            
            foreach (var button in boundButtons)
            {
                if (gamePadState.IsButtonDown(button))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
