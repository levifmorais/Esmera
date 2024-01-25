using System;
using Godot;

namespace Esmera.Scenes.Entities.Player;

public enum PlayerState
{
    Idle,
    Walk,
    Jump,
    Fall,
    Attack,
    Dead
}

public partial class Player : Node2D
{
    private PlayerState _state = PlayerState.Idle; // Player state ao iniciar o jogo.

    [Export] public PlayerBody PlayerBody;

    public override void _Ready()
    {
    }

    public override void _PhysicsProcess(double delta)
    {
        PlayerBody.GrabCurrentVelocityAndCheckGravity(delta);

        switch (_state)
        {
            case PlayerState.Idle:
                IdleState();
                break;
            case PlayerState.Walk:
                WalkState();
                break;
            case PlayerState.Jump:
                JumpState();
                break;
            case PlayerState.Fall:
                FallState();
                break;
            case PlayerState.Attack:
                break;
            case PlayerState.Dead:
                break;
            default:
                throw new ArgumentOutOfRangeException("State inexistente.");
        }

        PlayerBody.ApplyPlayerVelocity();

        //GD.Print(_state);
        GD.Print(PlayerBody.IsOnFloor());
    }

    public override void _Process(double delta)
    {

        if (!PlayerBody.IsOnFloor() && PlayerBody.PlayerVelocity.Y > 0) _state = PlayerState.Fall;

    }

    private void IdleState()
    {
       PlayerBody.AnimationPlayer.Play("Idle");
       PlayerBody.Idle();
       if (PlayerBody.Direction.X != 0) _state = PlayerState.Walk;
       CheckIfCanJump();
    }

    private void WalkState()
    {
        PlayerBody.AnimationPlayer.Play("Walk");
        PlayerBody.Move();

        if (PlayerBody.Direction.X == 0) _state = PlayerState.Idle;
        CheckIfCanJump();
    }

    private void JumpState()
    {
        PlayerBody.AnimationPlayer.Play("Jump");
        if (PlayerBody.IsOnFloor()) PlayerBody.Jump();

        PlayerBody.Move();
        
        if (PlayerBody.IsOnFloor() && PlayerBody.PlayerVelocity.Y == 0) 
            _state = PlayerState.Idle;
    }

    private void CheckIfCanJump()
    {
        if (Input.IsActionJustPressed("jump") && PlayerBody.IsOnFloor()) _state = PlayerState.Jump;
    }

    private void FallState()
    {
        PlayerBody.AnimationPlayer.Play("Fall");

        PlayerBody.Move();

        if (PlayerBody.IsOnFloor()) _state = PlayerState.Idle;
    }
}