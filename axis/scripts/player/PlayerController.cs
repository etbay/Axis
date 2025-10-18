using Godot;
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

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
    private Killzone killzone;
    private bool keyJustHit = false;

    public override void _Ready()
    {
        Killzone.KeyHit += ShowHitRating;
        Pad.KeyHit += ShowHitRating;

        // LR pads must be first child
        leftRightPads = GetChild<Node2D>(0);
        leftPad = leftRightPads.GetChild<Pad>(0);
        rightPad = leftRightPads.GetChild<Pad>(1);
        // TB pads must be second child
        upDownPads = GetChild<Node2D>(1);
        upPad = upDownPads.GetChild<Pad>(0);
        downPad = upDownPads.GetChild<Pad>(1);

        killzone = GetChild<Killzone>(2);

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

    private void MovePads()
    {
        if (Input.IsActionJustPressed("move_pad_up"))
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
        else if (Input.IsActionJustPressed("move_pad_down"))
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

        if (Input.IsActionJustPressed("move_pad_left"))
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
        else if (Input.IsActionJustPressed("move_pad_right"))
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

    private void ShowHitRating(string rating, int points)
    {
        GetChild<GameUserInterface>(3).OnKeyHit(rating, points);
    }
}