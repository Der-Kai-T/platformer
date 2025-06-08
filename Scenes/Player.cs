using Godot;
using System;

public partial class Player : CharacterBody2D
{
    private const float Speed = 800.0f;
    private const float JumpVelocity = -1200.0f;
    private double _airTime;
    private float _wallPushBack = 1000f;
    private bool _isWallJumping;
    
    private AnimatedSprite2D _sprite;

    public override void _Ready()
    {
        _sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
    }
    

    public override void _PhysicsProcess(double delta)
    {
        UpdateDebug();
        Vector2 velocity = Velocity;
      

        if (IsOnFloor() || IsOnWall())
        {
            _airTime = 0;
            _isWallJumping = false;
        }

        // Add the gravity.
        if (!IsOnFloor())
        {
            _airTime += delta;
            // velocity += GetGravity() * (float)delta * (Velocity.Y > 0 ? 5 : 1);
            velocity += GetGravity() * (float)delta + GetGravity() * (float)_airTime * 0.05f;
        }

        // Handle Jump.
        if (Input.IsActionJustPressed("ui_accept") && (IsOnFloor() || IsOnWall()))
        {
            velocity.Y = JumpVelocity;
            if (IsOnWall())
            {
                velocity.X = GetWallNormal().X * _wallPushBack;
                GD.Print("WallJump", velocity.X);
                _isWallJumping = true;
            }
        }

        // Get the input direction and handle the movement/deceleration.
        // As good practice, you should replace UI actions with custom gameplay actions.

        if (!_isWallJumping)
        {
            Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");


            if (direction != Vector2.Zero)
            {
                velocity.X = direction.X * Speed;
            }
            else
            {
                if (IsOnFloor())
                {
                    velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
                }
                else
                {
                    velocity.X = Mathf.MoveToward(Velocity.X, 0, (float)Speed/100);
                }
            }
        }

        GD.Print(velocity.X);
        Velocity = velocity;
        MoveAndSlide();

        if (Velocity == Vector2.Zero && _sprite.Animation != "IDLE")
        {
            _sprite.Play("IDLE");
        }
        
        if (Velocity.Y < 0 && _sprite.Animation != "JUMP")
        {
            _sprite.Play("JUMP");
        }

        if (IsOnFloor() && Velocity != Vector2.Zero && _sprite.Animation != "WALK")
        {
            _sprite.Play("WALK");
        }

        _sprite.FlipH = Velocity.X < 0;
        
    }

    private void UpdateDebug()
    {
        GetNode<ColorRect>("Camera2D/OnFloor").Color = IsOnFloor() ? new Color(0, 1, 0) : new Color(1, 0, 0);
        GetNode<ColorRect>("Camera2D/OnWall").Color = IsOnWall() ? new Color(0, 1, 0) : new Color(1, 0, 0);
        GetNode<Label>("Camera2D/Velocity").Text = Velocity.ToString();
        GetNode<Label>("Camera2D/Position").Text = Position.ToString();
        GetNode<Label>("Camera2D/Animation").Text = _sprite.Animation;
    }
}