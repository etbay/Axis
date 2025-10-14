using Godot;
using System;
using System.Runtime.CompilerServices;

public partial class PlayerController : Node2D
{
    private int movementDistance;
    private int numMovementsAllowed = 1;
    private int maxDistance;

    private Node2D leftRightPads;
    private Node2D upDownPads;
    private Pad upPad;
    private Pad downPad;
    private Pad leftPad;
    private Pad rightPad;

    public override void _Ready()
    {
        // LR pads must be first child
        leftRightPads = GetChild<Node2D>(0);
        leftPad = leftRightPads.GetChild<Pad>(0);
        rightPad = leftRightPads.GetChild<Pad>(1);
        // TB pads must be second child
        upDownPads = GetChild<Node2D>(1);
        upPad = upDownPads.GetChild<Pad>(0);
        downPad = upDownPads.GetChild<Pad>(1);

        movementDistance = GameData.KeyOffsetDistance;
        maxDistance = movementDistance * numMovementsAllowed;
    }

    public override void _PhysicsProcess(double delta)
    {
        MovePads();
        SelectPads();
    }

    private void MovePads()
    {
        if (Input.IsActionJustPressed("move_pad_up") && leftRightPads.Position > Vector2.Up * maxDistance)
        {
            leftRightPads.Position += Vector2.Up * movementDistance;
        }
        else if (Input.IsActionJustPressed("move_pad_down") && leftRightPads.Position < Vector2.Down * maxDistance)
        {
            leftRightPads.Position += Vector2.Down * movementDistance;
        }

        if (Input.IsActionJustPressed("move_pad_left") && upDownPads.Position > Vector2.Left * maxDistance)
        {
            upDownPads.Position += Vector2.Left * movementDistance;
        }
        else if (Input.IsActionJustPressed("move_pad_right") && upDownPads.Position < Vector2.Right * maxDistance)
        {
            upDownPads.Position += Vector2.Right * movementDistance;
        }
    }
    
    private void SelectPads()
    {
        if (Input.IsActionJustPressed("press_pad_up"))
        {
            _ = upPad.Activate();
            //_ = upPad.ChangeColor(GameData.UpColor);
        }

        if (Input.IsActionJustPressed("press_pad_down"))
        {
            _ = downPad.Activate();
            //_ = downPad.ChangeColor(GameData.DownColor);
        }

        if (Input.IsActionJustPressed("press_pad_left"))
        {
            _ = leftPad.Activate();
            //_ = leftPad.ChangeColor(GameData.LeftColor);
        }

        if (Input.IsActionJustPressed("press_pad_right"))
        {
            _ = rightPad.Activate();
            //_ = rightPad.ChangeColor(GameData.RightColor);
        }
    }
}