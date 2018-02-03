using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Fungi.Input
{
    public class InputManager
    {
        private List<Action> actions;
        private List<Axis> axes;
        private PlayerIndex playerIndex;
        private Vector2? mouseLockPosition;

        private KeyboardState currentKeyboardState;
        private MouseState currentMouseState;
        private Vector2 currentMouseDeltaPosition;
        private GamePadState currentGamePadState;
        private Dictionary<string, float> currentActionPressDurations;
        private List<string> actionsPressedThisUpdate;
        private List<string> actionsReleasedThisUpdate;
        private Vector2 mousePositionLastUpdate;

        /// <summary>
        /// Initializes the input manager for one player.
        /// </summary>
        /// <param name="playerIndex">The index of the player, defaults to One</param>
        public InputManager(PlayerIndex playerIndex = PlayerIndex.One)
        {
            this.playerIndex = playerIndex;
            actions = new List<Action>();
            axes = new List<Axis>();
            currentActionPressDurations = new Dictionary<string, float>();
        }

        /// <summary>
        /// Binds a keyboard key to an action.
        /// </summary>
        /// <param name="actionName">The name of the action</param>
        /// <param name="key">The keyboard key</param>
        public void Bind(string actionName, Keys key)
        {
            GetAction(actionName).Bind(key);
        }

        /// <summary>
        /// Binds a game pad button to an action.
        /// </summary>
        /// <param name="actionName">The name of the action</param>
        /// <param name="button">The game pad button</param>
        public void Bind(string actionName, Buttons button)
        {
            GetAction(actionName).Bind(button);
        }

        public void BindAxis(string axisName, AxisType axisType)
        {
            GetAxisBind(axisName).SetAxisType(axisType);
        }

        /// <summary>
        /// This gets the latest keyboard and game pad state, run this every tick before checking inputs!
        /// </summary>
        public void UpdateState(GameTime gameTime)
        {
            // Get the current state
            currentKeyboardState = Keyboard.GetState();
            currentGamePadState = GamePad.GetState(playerIndex);
            currentMouseState = Mouse.GetState();

            // Clear just pressed/released lists
            actionsPressedThisUpdate = new List<string>();
            actionsReleasedThisUpdate = new List<string>();

            var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            foreach (var action in actions)
            {
                if (action.IsPressed(currentKeyboardState, currentGamePadState))
                {
                    if (currentActionPressDurations.ContainsKey(action.Name))
                    {
                        currentActionPressDurations[action.Name] += deltaTime;
                    }
                    else
                    {
                        actionsPressedThisUpdate.Add(action.Name);
                        currentActionPressDurations.Add(action.Name, deltaTime);
                    }
                }
                else if (currentActionPressDurations.ContainsKey(action.Name))
                {
                    actionsReleasedThisUpdate.Add(action.Name);
                    currentActionPressDurations.Remove(action.Name);
                }
            }

            // Calculate mouse delta position
            if (mousePositionLastUpdate != null)
            {
                currentMouseDeltaPosition = currentMouseState.Position.ToVector2() - mousePositionLastUpdate;
            }

            if (mouseLockPosition.HasValue)
            {
                Mouse.SetPosition((int)mouseLockPosition.Value.X, (int)mouseLockPosition.Value.Y);
                mousePositionLastUpdate = mouseLockPosition.Value;
            }
            else
            {
                mousePositionLastUpdate = currentMouseState.Position.ToVector2();
            }

        }


        public void LockMouse(Vector2 mousePosition)
        {
            mouseLockPosition = mousePosition;
        }

        public void UnlockMouse()
        {
            mouseLockPosition = null;
        }


        /// <summary>
        /// Gets the press duration of an action in seconds.
        /// </summary>
        /// <param name="actionName">The name of the action</param>
        /// <returns>The press duration in milliseconds</returns>
        public float GetPressDuration(string actionName)
        {
            return Pressed(actionName) ? currentActionPressDurations[actionName] : 0f;
        }

        /// <summary>
        /// Checks if the action is currently pressed
        /// </summary>
        /// <param name="actionName">The name of the action</param>
        /// <returns>True if pressed down, false if not</returns>
        public bool Pressed(string actionName)
        {
            return currentActionPressDurations.ContainsKey(actionName);
        }
        
        /// <summary>
        /// Checks if the action was pressed this tick
        /// </summary>
        /// <param name="actionName">The name of the action</param>
        /// <returns>True if it was pressed down this tick, false if not</returns>
        public bool JustPressed(string actionName)
        {
            return actionsPressedThisUpdate.Contains(actionName);
        }

        /// <summary>
        /// Check if the action is currently released
        /// </summary>
        /// <param name="actionName">The name of the action</param>
        /// <returns>True if released, false if not</returns>
        public bool Released(string actionName)
        {
            return !Pressed(actionName);
        }

        /// <summary>
        /// Checks if the action was released this tick
        /// </summary>
        /// <param name="actionName">The name of the action</param>
        /// <returns>True if it was released this tick, false if not</returns>
        public bool JustReleased(string actionName)
        {
            return actionsReleasedThisUpdate.Contains(actionName);
        }

        public float GetAxis(string axisName)
        {
            return GetAxisBind(axisName).GetValue(currentMouseDeltaPosition, currentGamePadState);
        }

        /// <summary>
        /// This will get the action with the specified name, and if none exist, simply create a new one.
        /// </summary>
        /// <param name="actionName">The name of the action</param>
        /// <returns>The action</returns>
        private Action GetAction(string actionName)
        {
            var action = actions.Find((a) => a.Name == actionName);

            if (action == null)
            {
                action = new Action(actionName);
                actions.Add(action);
            }

            return action;
        }

        private Axis GetAxisBind(string axisName)
        {
            var axis = axes.Find((a) => a.Name == axisName);

            if (axis == null)
            {
                axis = new Axis(axisName);
                axes.Add(axis);
            }

            return axis;
        }
    }
}
