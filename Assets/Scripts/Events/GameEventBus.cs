using System;
using UnityEngine;

public static class GameEventBus
{
    public static event Action<GameEvent> OnGameEvent;
    public static event Action<GameEvent> OnSelectedEntity;
    public static event Action<GameEvent> OnDeselectedEntity;

    public static void Raise(GameEvent evt)
    {
        OnGameEvent?.Invoke(evt);
        if (evt.type == GameEventType.SELECTED_ENTITY)
        {
            OnSelectedEntity?.Invoke(evt);
        }
        else if (evt.type == GameEventType.DESELECTED_ENTITY)
        {
            OnDeselectedEntity?.Invoke(evt);
        }
    }
}