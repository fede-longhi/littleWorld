public class CreatureEvent
{
    public CreatureEventType type;
    public Creature creature;
    public object data;
}

public enum CreatureEventType
{
    DEATH,
    BORN
}