using System;
using UnityEngine;

public static class CreatureEventBus
{
    public static event Action<CreatureEvent> OnCreatureEvent;

    public static void Raise(CreatureEvent evt)
    {
        OnCreatureEvent?.Invoke(evt);
    }
}