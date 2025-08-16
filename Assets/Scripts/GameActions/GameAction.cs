using UnityEngine;

public enum GameActionType
{
    CREATE_CREATURE,
    SELECT_ENTITY
}

public abstract class GameAction
{
    public GameActionType type;
    public abstract void Execute(Vector2 mousePosition);
}