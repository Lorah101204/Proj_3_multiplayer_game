using UnityEngine.InputSystem;

public class SimpleInputActions
{
    public InputAction Move;
    public InputAction Jump;
    public InputAction Crouch;

    public SimpleInputActions()
    {
        Move = new InputAction("Move", binding: "<Gamepad>/leftStick");
        Move.AddCompositeBinding("2DVector")
            .With("Left", "<Keyboard>/a")
            .With("Right", "<Keyboard>/d")
            .With("Up", "<Keyboard>/w")
            .With("Down", "<Keyboard>/s");

        Jump = new InputAction("Jump", binding: "<Keyboard>/space");
        Jump.AddBinding("<Gamepad>/buttonSouth");

        Crouch = new InputAction("Crouch", binding: "<Keyboard>/s");
        Crouch.AddBinding("<Gamepad>/leftTrigger");
    }

    public void Enable()
    {
        Move.Enable();
        Jump.Enable();
        Crouch.Enable();
    }

    public void Disable()
    {
        Move.Disable();
        Jump.Disable();
        Crouch.Disable();
    }
}
