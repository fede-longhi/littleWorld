public class GameEvent
{
    public GameEventType type;
    public object data;
}

public enum GameEventType
{
    SELECTED_ENTITY,
    DESELECTED_ENTITY,
}