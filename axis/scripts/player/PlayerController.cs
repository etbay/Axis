using Godot;
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

public partial class PlayerController : Node2D
{
    public event Action<string, int> KeyHit;
    private int movementDistance;
    private int numMovementsAllowed = 1;
    private int maxDistance;

    private Node2D leftRightPads;
    private Node2D upDownPads;
    private Pad upPad;
    private Pad downPad;
    private Pad leftPad;
    private Pad rightPad;
    private Killzone killzone;
    private bool keyJustHit = false;

    public override void _Ready()
    {
        Killzone.KeyHit += ShowHitRating;
        Pad.KeyHit += ShowHitRating;

        // LR pads must be first child
        leftRightPads = GetNode<Node2D>("LRPads");
        leftPad = GetNode<Pad>("LRPads/LeftPad");
        leftPad.SetColor(GameData.LeftColor);
        rightPad = GetNode<Pad>("LRPads/RightPad");
        rightPad.SetColor(GameData.RightColor);
        // TB pads must be second child
        upDownPads = GetNode<Node2D>("TBPads");
        upPad = GetNode<Pad>("TBPads/TopPad");
        upPad.SetColor(GameData.UpColor);
        downPad = GetNode<Pad>("TBPads/BottomPad");
        downPad.SetColor(GameData.DownColor);

        killzone = GetNode<Killzone>("Killzone");

        movementDistance = GameData.KeyOffsetDistance;
        maxDistance = movementDistance * numMovementsAllowed;
    }

    public override void _ExitTree()
    {
        Killzone.KeyHit -= ShowHitRating;
        Pad.KeyHit -= ShowHitRating;
    }

    public override void _PhysicsProcess(double delta)
    {
        MovePads();
        SelectPads();
    }

    private void MovePads() // TODO: Lock movement if key is being held, not if pad is selected
    {
        if (Input.IsActionJustPressed("move_pad_up") && !leftPad.IsPadSelected && !rightPad.IsPadSelected)
        {
            if (leftRightPads.Position > Vector2.Up * maxDistance)
            {
                leftRightPads.Position += Vector2.Up * movementDistance;
            }
            else
            {
                leftRightPads.Position -= Vector2.Up * movementDistance * 2;
            }
        }
        else if (Input.IsActionJustPressed("move_pad_down") && !leftPad.IsPadSelected && !rightPad.IsPadSelected)
        {
            if (leftRightPads.Position < Vector2.Down * maxDistance)
            {
                leftRightPads.Position += Vector2.Down * movementDistance;
            }
            else
            {
                leftRightPads.Position -= Vector2.Down * movementDistance * 2;
            }
        }

        if (Input.IsActionJustPressed("move_pad_left") && !upPad.IsPadSelected && !downPad.IsPadSelected)
        {
            if (upDownPads.Position > Vector2.Left * maxDistance)
            {
                upDownPads.Position += Vector2.Left * movementDistance;
            }
            else
            {
                upDownPads.Position -= Vector2.Left * movementDistance * 2;
            }
        }
        else if (Input.IsActionJustPressed("move_pad_right") && !upPad.IsPadSelected && !downPad.IsPadSelected)
        {
            if (upDownPads.Position < Vector2.Right * maxDistance)
            {
                upDownPads.Position += Vector2.Right * movementDistance;
            }
            else
            {
                upDownPads.Position -= Vector2.Right * movementDistance * 2;
            }
        }
    }

    private void SelectPads()
    {
        if (Input.IsActionJustPressed("press_pad_up"))
        {
            _ = upPad.Activate();
            upPad.SelectPad();
        }
        else if (Input.IsActionJustReleased("press_pad_up"))
        {
            upPad.DeselectPad();
        }

        if (Input.IsActionJustPressed("press_pad_down"))
        {
            _ = downPad.Activate();
            downPad.SelectPad();
        }
        else if (Input.IsActionJustReleased("press_pad_down"))
        {
            downPad.DeselectPad();
        }

        if (Input.IsActionJustPressed("press_pad_left"))
        {
            _ = leftPad.Activate();
            leftPad.SelectPad();
        }
        else if (Input.IsActionJustReleased("press_pad_left"))
        {
            leftPad.DeselectPad();
        }

        if (Input.IsActionJustPressed("press_pad_right"))
        {
            _ = rightPad.Activate();
            rightPad.SelectPad();
        }
        else if (Input.IsActionJustReleased("press_pad_right"))
        {
            rightPad.DeselectPad();
        }
    }

    private void ShowHitRating(string rating, int points)
    {
        this.KeyHit?.Invoke(rating, points);
    }
}