using UnityEngine;
using System;
using System.Collections.Concurrent;

[RequireComponent(typeof(Animator))]
public class AnimationEventHandler : MonoBehaviour
{
    private ConcurrentDictionary<string, Action> eventDictionary = new ConcurrentDictionary<string, Action>();

    /// <summary>
    /// Subscribes a method to an animation event.
    /// </summary>
    /// <param name="eventName">The name of the event to subscribe to.</param>
    /// <param name="method">The method to call when the event is triggered.</param>
    public void Subscribe(string eventName, Action method)
    {
        string key = eventName.ToLower();
        
        if (!eventDictionary.TryAdd(key, method))
        {
            #if UNITY_EDITOR
            Debug.LogError($"Event '{eventName}' is already subscribed.");
            #endif
        }
    }

    /// <summary>
    /// Unsubscribes a method from an animation event.
    /// </summary>
    /// <param name="eventName">The name of the event to unsubscribe from.</param>
    public void Unsubscribe(string eventName)
    {
        string key = eventName.ToLower();
        
        if (!eventDictionary.TryRemove(key, out _))
        {
            #if UNITY_EDITOR
            Debug.LogError($"Event '{eventName}' is not found and cannot be unsubscribed.");
            #endif
        }
    }

    /// <summary>
    /// Method called by the animation event.
    /// This method should be invoked from an animation event in the Animator component.
    /// </summary>
    /// <param name="eventName">The name of the triggered animation event.</param>
    public void HandleAnimationEvent(string eventName)
    {
        string key = eventName.ToLower();

        if (eventDictionary.TryGetValue(key, out var action))
        {
            action?.Invoke();
        }
        else
        {
            #if UNITY_EDITOR
            Debug.LogWarning($"Event '{eventName}' is not found in the event dictionary. Consider removing it as it's unused.");
            #endif
        }
    }
}