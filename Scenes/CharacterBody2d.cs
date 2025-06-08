using Godot;
using System;

public partial class CharacterBody2d : CharacterBody2D
{
    [Export]
    public int Speed { get; set; } = 800;
    
    [Export]
    public int Gravity { get; set; } = 4000;
    
    [Export]
    public int JumpForce { get; set; } = 3000;


    public override void _PhysicsProcess(double delta)
    {
        //get Left-Right-Movement
        var horizontalInput = Input.GetActionStrength("right") - Input.GetActionStrength("left");

        if (IsOnFloor())
        {
            if (Input.IsActionJustPressed("jump"))
            {
                Velocity = new Vector2(Velocity.X, -JumpForce);
                MoveAndSlide();
            }
            
            
        }
        
        //apply gravity
            Velocity = new Vector2(horizontalInput * Speed, Velocity.Y  + Gravity * (float) delta);

            MoveAndSlide();

    }

    public void _HorizontalMovement()
    {
        
    }
}
