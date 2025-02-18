using Godot;
using System;
using System.Threading.Tasks;

public enum InteractableType { Button, Lever, Dial, Slider}

public partial class FacilityButton : MeshInstance3D
{
    [Export] public InteractableType _IType;
    [Export] public string ButtonTag { get; private set; }
    public event Action<string> Pressed;
    
    [Export] float _distanceToMove = 0.1f;
    [Export] float _timeToFullyMove = 0.05f;

    bool isAnimating = false;
    bool pressed = false;
    bool releaseBuffer = false;


    public override void _Ready()
    {
        GameRoot.AddFacilityButton(this);
    }

    public override void _PhysicsProcess(double delta)
    {
        // Handle releaseBuffer (Only works for button rn)
        if (pressed && !isAnimating && releaseBuffer)
        {
            switch (_IType)
            {
                case InteractableType.Button:
                    EndButtonPress();
                    break;
                case InteractableType.Lever:
                    // Handle lever logic (rotate Z)
                    break;
                case InteractableType.Dial:
                    EndDialRotate();// Handle dial logic (rotate Y)
                    break;
                case InteractableType.Slider:
                    // Handle slider logic (move X or Z)
                    break;
            }
        }
    }

    public void Interact()
    {
        if (pressed || isAnimating) return;
        switch (_IType)
        {
            case InteractableType.Button:
                StartButtonPress();
                break;
            case InteractableType.Lever:
                // Handle lever logic (rotate Z)
                break;
            case InteractableType.Dial:
                StartDialRotate();// Handle dial logic (rotate Y)
                break;
            case InteractableType.Slider:
                // Handle slider logic (move X or Z)
                break;
        }
    }

    public void InteractRelease()
    {
        switch (_IType)
        {
            case InteractableType.Button:
                if (pressed && !isAnimating)
                {
                    EndButtonPress();
                }
                else
                {
                    releaseBuffer = true;
                }
                break;
            case InteractableType.Lever:
                // Handle lever logic (rotate Z)
                break;
            case InteractableType.Dial:
                // Handle dial logic (rotate Y)
                if (pressed && !isAnimating)
                {
                    EndDialRotate();
                }
                else
                {
                    releaseBuffer = true;
                }
                break;
            case InteractableType.Slider:
                // Handle slider logic (move X or Z)
                break;
        }
    }

    #region BUTTON_LOGIC
    private async void StartButtonPress()
    {
        isAnimating = true;
        await MoveYAsync(_distanceToMove, _timeToFullyMove, true);
        isAnimating = false;
        pressed = true;
    }

    private async void EndButtonPress()
    {
        isAnimating = true;
        await MoveYAsync(_distanceToMove, _timeToFullyMove, false);
        isAnimating = false;
        pressed = false;
        releaseBuffer = false;
    }

    private async Task MoveYAsync(float distance, float duration, bool pressDown)
    {
        Vector3 startPos = Position;
        Vector3 targetPos = Vector3.Zero;
        if (pressDown)
            targetPos = startPos - new Vector3(0, distance, 0);
        else
            targetPos = startPos + new Vector3(0, distance, 0);
        float elapsedTime = 0f;

        float deltaTime = (float)GetPhysicsProcessDeltaTime();

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            Position = startPos.Lerp(targetPos, t);
            await ToSignal(GetTree().CreateTimer(deltaTime), "timeout");
            elapsedTime += deltaTime;
        }
        // Ensure the final position is exact.
        Position = targetPos;
    }
    #endregion


    #region DIAL_LOGIC
    private async void StartDialRotate()
    {
        isAnimating = true;
        await RotateYAsync(_distanceToMove, _timeToFullyMove, true);
        isAnimating = false;
        pressed = true;
    }

    private async void EndDialRotate()
    {
        isAnimating = true;
        await RotateYAsync(_distanceToMove, _timeToFullyMove, false);
        isAnimating = false;
        pressed = false;
        releaseBuffer = false;
    }

    private async Task RotateYAsync(float distance, float duration, bool negativeMovement)
    {
        Vector3 startRot = Rotation;
        Vector3 targetRot = Vector3.Zero;
        if (negativeMovement)
            targetRot = startRot - new Vector3(0, Mathf.DegToRad(distance), 0);
        else
            targetRot = startRot + new Vector3(0, Mathf.DegToRad(distance), 0);
        float elapsedTime = 0f;

        float deltaTime = (float)GetPhysicsProcessDeltaTime();

        GD.Print("alo guvnr");

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            Rotation = startRot.Lerp(targetRot, t);
            await ToSignal(GetTree().CreateTimer(deltaTime), "timeout");
            elapsedTime += deltaTime;
        }
        // Ensure the final position is exact.
        Rotation = targetRot;
    }
    #endregion
}
