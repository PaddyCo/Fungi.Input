using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Fungi.Input
{
    public enum AxisType
    {
        None,
        MouseX,
        MouseY,
        LeftThumbStickX,
        LeftThumbStickY,
        RightThumbStickX,
        RightThumbStickY
    }

    class Axis
    {
        public string Name;
        private AxisType axisType;
        private List<Keys> boundKeys;
        private List<Buttons> boundButtons;
        
        public Axis(string name, AxisType axisType = AxisType.None)
        {
            Name = name;
            this.axisType = axisType;
        }

        public void SetAxisType(AxisType axisType)
        {
            this.axisType = axisType;
        }

        public float GetValue(Vector2 mouseMovementDelta, GamePadState gamePadState)
        {
            switch (axisType)
            {
                case AxisType.LeftThumbStickX:
                    return gamePadState.ThumbSticks.Left.X;
                case AxisType.LeftThumbStickY:
                    return -gamePadState.ThumbSticks.Left.Y;
                case AxisType.RightThumbStickX:
                    return gamePadState.ThumbSticks.Right.X;
                case AxisType.RightThumbStickY:
                    return -gamePadState.ThumbSticks.Right.Y;
                case AxisType.MouseX:
                    return mouseMovementDelta.X;
                case AxisType.MouseY:
                    return mouseMovementDelta.Y;
                default:
                    return 0f;
            }
        }
    }
}
