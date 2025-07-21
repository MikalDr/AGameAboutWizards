using Godot;
using System;

public partial class Spell : CharacterBody2D
{

  [Export] protected float SpellSpeedMax;
  [Export] protected float SpellAcceleration;
  
  protected AnimatedSprite2D Animated;
  protected Area2D Hitbox;
  protected Area2D Hurtbox;
  protected AudioStreamPlayer2D SpellAudio;
  protected bool HasHit;
  protected Func<double, bool> IsEffectOver = _ => true;
  
  protected virtual void Setup()
  {
    SpriteSetup();
    HitboxSetup();
    HurtBoxSetup();
    SpellAudioSetup();
  }

  public void CastSpell(Direction direction)
  {
    Setup();
    Rotation = (float)direction.GetRadians();
    GD.Print("Setup completed");
  }

  protected void SpriteSetup()
  {
    Animated = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
    if (Animated == null) throw new Exception("AnimatedSprite2D is null");
  }

  protected void SpellAudioSetup()
  {
    SpellAudio = GetNode<AudioStreamPlayer2D>("SpellAudio");
    if (SpellAudio == null) throw new Exception("SpellAudio is null");
  }

  protected void HurtBoxSetup()
  {
    
    Hurtbox = GetNode<Area2D>("Hurtbox");
    if (Hurtbox == null) throw new Exception("Hurtbox is null");
    Hurtbox.AreaEntered += OnHurtboxEnter;
    Hurtbox.AreaExited += OnHurtboxExit;
  }

  protected void HitboxSetup()
  {
    Hitbox = GetNode<Area2D>("Hitbox");
    if (Hitbox == null) throw new Exception("Hitbox is null");
    Hitbox.AreaEntered += OnHitboxEnter;
    Hitbox.AreaExited += OnHitboxExit;
  }

  public override void _Process(double delta)
  {
    OnEffectOver(delta);
    Velocity *= (float)(delta * SpellAcceleration);
    Velocity = Velocity.Clamp(SpellSpeedMax, SpellSpeedMax);
  }

  protected void OnEffectOver(double delta)
  {
    if (!HasHit) return;
    var r = IsEffectOver(delta);
    GD.Print("Uh oh", r);
    if (r) QueueFree();
  }

  protected void SafelyDequeSelf()
  {
    HasHit = true;
    Animated.Visible = false;
    Hitbox.QueueFree();
    Hurtbox.QueueFree();
  }

  protected virtual void OnHurtboxEnter(Area2D area)
  {
    
  }

  protected virtual void OnHurtboxExit(Area2D area)
  {
    
  }

  protected virtual void OnHitboxEnter(Area2D area)
  {
    
  }

  protected virtual void OnHitboxExit(Area2D area)
  {
    
  }
}
