# Fungi.Input

Simple input manager for MonoGame and other XNA derived frameworks.

## Actions

An action is bound to a button or key, and is either pressed or not pressed.
For example, walking with a D-pad or the arrow keys on a keyboard are usually made out of `WalkLeft` and `WalkRight` actions.

### Examples

#### Binding

```csharp
inputManager.Bind("Punch", Keys.P);    // You can bind keyboard keys...
inputManager.Bind("Punch", Buttons.A); // ... or game pad buttons
```

Note that you can bind multiple keys and buttons to the same action

#### Getting inputs

```csharp
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
```

## Axes

An axis is bound to either a thumb stick on a gamepad or a mouse, and returns a float value.
For example, walking with an analog stick or looking around with the mouse are usually made out of `Move` and `LookX` axes.

### Examples

#### Binding

```csharp
inputManager.BindAxis("Lean", AxisType.LeftThumbStickX); // You can bind thumb sticks...
inputManager.BindAxis("LookHorizontal", AxisType.MouseX); // ... or the mouse
```

Unlike actions you can only bind one axis type to a particular axis

#### Getting inputs

```csharp
if (inputManager.GetAxis("Lean") != 0)
{
	Console.WriteLine($"Leaning: {inputManager.GetAxis("Lean")}"); // Thumb stick axes are always between -1.0 and 1.0
}

if (inputManager.GetAxis("LookHorizontal") != 0)
{
	Console.WriteLine($"Looking Horizontally: {inputManager.GetAxis("LookX")}"); // Mouse axes return their delta position as float, so basically how much it has moved since the last frame
}
```

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

            // Bind actions and axes here (or wherever you feel like)
            inputManager.Bind("Punch", Keys.P); 
            inputManager.BindAxis("Lean", AxisType.LeftThumbStickX);
	    
            // ...
        }


        protected override void Update(GameTime gameTime)
        {
            // ...

            inputManager.UpdateState(gameTime); // Updates the state of all input actions, keeping track of how long the actions have been pressed etc.

            if (inputManager.Pressed("Punch")) // Checks if the action was pressed this tick.
            {
                Console.WriteLine("You punch and punch and punch!");
            }

            Console.WriteLine($"Leaning: {inputManager.GetAxis("Lean")}"); // Thumb stick axes are always between -1.0 and 1.0

            // ...
        }
    }
}

```
