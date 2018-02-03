# Fungi.Input

Simple input manager for MonoGame and other XNA derived frameworks.

## How to use

```csharp
// ...
using Fungi.Input;

namespace HelloWorld
{
    public class Game1 : Game
    {
        // ...

        InputManager inputManager;

        public Game1()
        {
            // ...

            inputManager = new InputManager(); // Initialize the Input manager
        }

        protected override void Initialize()
        {
            // ...

            inputManager.Bind("Punch", Keys.P);    // You can bind keyboard keys...
            inputManager.Bind("Punch", Buttons.A); // ...or game pad buttons

            // ...
        }


        protected override void Update(GameTime gameTime)
        {
            // ...

            inputManager.UpdateState(gameTime); // Updates the state of all input actions, keeping track of how long the actions have been pressed etc.

            if (inputManager.JustPressed("Punch")) // Checks if the action was pressed this tick.
            {
                Console.WriteLine("Started charging punch!");
            }

            if (inputManager.JustReleased("Punch")) // Checks if the action was released this tick.
            {
                Console.WriteLine("Unleashing punch!");
            }

            if (inputManager.Pressed("Punch")) // Checks if the button is currently being pressed.
            {
                // The input manager also keeps track of how long the action has been held down for:
                Console.WriteLine($"The punch has been charging for {inputManager.GetPressDuration("Punch")} seconds!");
            }

            if (inputManager.Released("Punch")) // Checks if the button is currently not being pressed.
            {
                Console.WriteLine("Not doing anything!");
            }

            // ...
        }
    }
}

```
