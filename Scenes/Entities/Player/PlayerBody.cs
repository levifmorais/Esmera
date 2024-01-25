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

    // Pegamos a gravidade das configuracoes para serem iguais em todos os lugares (RigidBody, CharacterBody... etc).
    public float Gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

    public override void _PhysicsProcess(double delta)
    {
        //GD.Print(PlayerVelocity);
    }

    public void GrabCurrentVelocityAndCheckGravity(double delta)
    {
        Direction = Input.GetVector("left", "right", "jump", "ui_down");
        
        PlayerVelocity = Velocity;

        if (!IsOnFloor())
        {
            PlayerVelocity.Y += Gravity * (float)delta;
        }
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

    public void Move()
    {
        PlayerVelocity.X = Direction.X * Speed;
    }

    public void Idle()
    {
        PlayerVelocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
    }

    public void Jump()
    {
        PlayerVelocity.Y = JumpVelocity;
    }
}