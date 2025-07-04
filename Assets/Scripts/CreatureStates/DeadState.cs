public class DeadState : CreatureState
{
    public DeadState(Creature creature) : base(creature) { }

    public override void Enter()
    {
        creature.StopMoving();
    }

    public override void Update() { }
    public override void FixedUpdate() { }

    public override void Exit() { }

    public override string GetName() => "dead";
}