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
                WalkState(delta);
                break;
            case PlayerState.Jump:
                break;
            case PlayerState.Fall:
                break;
            case PlayerState.Attack:
                break;
            case PlayerState.Dead:
                break;
            default:
                throw new ArgumentOutOfRangeException("State inexistente.");
        }

        GD.Print(_state);

        PlayerBody.ApplyPlayerVelocity();

    }

    private void IdleState()
    {
       PlayerBody.AnimationPlayer.Play("Idle");
       PlayerBody.Idle();
       if (PlayerBody.Direction.X != 0) _state = PlayerState.Walk;

    }

    private void WalkState(double delta)
    {
        PlayerBody.Walk();

        if (PlayerBody.Direction.X == 0) _state = PlayerState.Idle;
    }

}