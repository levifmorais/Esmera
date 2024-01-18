using Godot;

namespace Esmera.Scenes.Entities.Player;

public partial class PlayerBody : CharacterBody2D
{
    public const float Speed = 300.0f;
    public const float JumpVelocity = -400.0f;

    [Export] public AnimationPlayer AnimationPlayer { get; private set; }
    [Export] public Sprite2D Sprite { get; private set; }

    // Get the gravity from the project settings to be synced with RigidBody nodes.
    public float Gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

    public override void _PhysicsProcess(double delta)
    {
        Vector2 velocity = Velocity;

        // Add the gravity.
        if (!IsOnFloor())
            velocity.Y += Gravity * (float)delta;

        // Handle Jump.
        if (Input.IsActionJustPressed("ui_accept") && IsOnFloor())
            velocity.Y = JumpVelocity;

        // Get the input direction and handle the movement/deceleration.
        // As good practice, you should replace UI actions with custom gameplay actions.
        Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
        if (direction != Vector2.Zero)
        {
            AnimationPlayer.Play("Walk");
            velocity.X = direction.X * Speed;
        }
        else
        {
            AnimationPlayer.Play("Idle");
            velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
        }

        Sprite.FlipH = velocity.X switch
        {
            < 0 => velocity.X < 0,
            > 0 => false,
            _ => Sprite.FlipH
        };

        Velocity = velocity;
        MoveAndSlide();
    }
}