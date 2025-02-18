using Godot;
using System;
using System.Threading.Tasks;


public partial class FacilityButton : MeshInstance3D
{
    [Export] public string ButtonTag { get; private set; }
    public event Action<string> Pressed;
    
    [Export] float _distanceToMove = 0.1f;
    [Export] float _timeToFullyDepress = 0.05f;

    bool isAnimating = false;
    bool pressed = false;
    bool releaseBuffer = false;


    public override void _Ready()
    {
        GameRoot.AddFacilityButton(this);
    }

    public override void _PhysicsProcess(double delta)
    {
        if (pressed && !isAnimating && releaseBuffer)
        {
            EndPress();
        }
    }

    public void Interact()
    {
        if (!pressed && !isAnimating)
        {
            StartPress();
        }
    }

    public void InteractRelease()
    {
        if (pressed && !isAnimating)
        {
            EndPress();
        }
        else
        {
            releaseBuffer = true;
        }
    }

    private async void StartPress()
    {
        isAnimating = true;
        await MoveYAsync(_distanceToMove, _timeToFullyDepress, true);
        isAnimating = false;
        pressed = true;
    }

    private async void EndPress()
    {
        isAnimating = true;
        await MoveYAsync(_distanceToMove, _timeToFullyDepress, false);
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
}
