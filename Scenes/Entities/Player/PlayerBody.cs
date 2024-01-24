using Godot;

namespace Esmera.Scenes.Entities.Player;

public partial class PlayerBody : CharacterBody2D
{
    [Export] public float Speed = 300.0f;
    [Export] public float JumpVelocity = -400.0f;

    public Vector2 Direction;
    public Vector2 PlayerVelocity;

    [Export] public AnimationPlayer AnimationPlayer { get; private set; }
    [Export] public Sprite2D Sprite { get; private set; }

    // Get the gravity from the project settings to be synced with RigidBody nodes.
    public float Gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

    public override void _PhysicsProcess(double delta)
    {
        //PlayerVelocity = Velocity;


        // Add the gravity.
        
        //if (!IsOnFloor())
          //  PlayerVelocity.Y += Gravity * (float)delta;

        // Handle Jump.
        if (Input.IsActionJustPressed("ui_accept") && IsOnFloor())
            PlayerVelocity.Y = JumpVelocity;
        


        // Get the input direction and handle the movement/deceleration.
        // As good practice, you should replace UI actions with custom gameplay actions.
        /*
        if (Direction != Vector2.Zero)
        {
            PlayerVelocity.X = Direction.X * Speed;
        }
        else
        {
            PlayerVelocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
        }
        */

        //SpriteChangeDirection();

        //Velocity = PlayerVelocity;

        //MoveAndSlide();
    }

    public void GrabCurrentVelocityAndCheckGravity(double delta)
    {
        Direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
        
        PlayerVelocity = Velocity;

        if (!IsOnFloor()) PlayerVelocity.Y += Gravity * (float)delta;
    }

    public void ApplyPlayerVelocity()
    {
        SpriteChangeDirection();
        Velocity = PlayerVelocity;
        MoveAndSlide();
    }

    private void SpriteChangeDirection()
    {
        Sprite.FlipH = PlayerVelocity.X switch
        {
            < 0 => PlayerVelocity.X < 0,
            > 0 => false,
            _ => Sprite.FlipH
        };
    }

    public void Walk()
    {
        PlayerVelocity.X = Direction.X * Speed;
    }

    public void Idle()
    {
        PlayerVelocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
    }
}