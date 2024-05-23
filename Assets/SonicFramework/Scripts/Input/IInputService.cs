namespace SonicFramework
{
    public interface IInputService
    {
        GameInput gameInput { get; set;  }
        bool WasPressed(InputButton button);
        bool WasReleased(InputButton button);
        bool IsPressed(InputButton button);
        bool IsAnyButtonPressed();
    }
}