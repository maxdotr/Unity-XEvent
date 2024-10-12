using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace XEvent
{
    /// <summary>
    /// Manages the registration and firing of events, allowing methods to be dynamically invoked based on event names.
    /// </summary>
    public static class XEventManager
    {
        private static Dictionary<string, List<XEventObject>> subscribers = new Dictionary<string, List<XEventObject>>();

        /// <summary>
        /// Registers a specific method by its name for a given MonoBehaviour instance.
        /// </summary>
        /// <param name="monoBehaviour">The MonoBehaviour instance containing the method.</param>
        /// <param name="methodName">The name of the method to register.</param>
        public static void RegisterMethod(MonoBehaviour monoBehaviour, string methodName)
        {
            MethodInfo method = monoBehaviour.GetType().GetMethod(methodName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            if (method != null)
            {
                var xEventAttr = method.GetCustomAttribute<XEventListener>();
                if (xEventAttr != null)
                {
                    XEventObject xEventObject = new XEventObject(xEventAttr.Name, monoBehaviour, method);
                    RegisterEventByName(xEventAttr.Name, xEventObject);
                }
            }
            else
            {
                throw new Exception($"Method {methodName} not found on {monoBehaviour.GetType().Name}");
            }
        }

        /// <summary>
        /// Registers all methods of a monobehaviour where the method has an XEvent attribute.
        /// </summary>
        /// <param name="monoBehaviour">The MonoBehaviour instance containing the method.</param>
        public static void RegisterMethods(MonoBehaviour monoBehaviour)
        {
            Debug.Log($"Registering methods for: {monoBehaviour.GetType().Name}");

            MethodInfo[] methods = monoBehaviour.GetType().GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            foreach (var method in methods)
            {
                var xEventAttr = method.GetCustomAttribute<XEventListener>();
                if (xEventAttr != null)
                {
                    Debug.Log($"Found method with XEventListener: {method.Name}, Event Name: {xEventAttr.Name}");

                    XEventObject xEventObject = new XEventObject(xEventAttr.Name, monoBehaviour, method);
                    RegisterEventByName(xEventAttr.Name, xEventObject);
                }
            }
        }

        /// <summary>
        /// Registers a method to an event by its name.
        /// </summary>
        /// <param name="name">The name of the event.</param>
        /// <param name="xEventObject">The XEventObject representing the method to be invoked when the event is fired.</param>
        public static void RegisterEventByName(string name, XEventObject xEventObject)
        {
            if (!subscribers.ContainsKey(name))
            {
                subscribers[name] = new List<XEventObject>();
            }

            subscribers[name].Add(xEventObject);
        }

        /// <summary>
        /// Unregisters all methods for a specific MonoBehaviour instance.
        /// </summary>
        /// <param name="monoBehaviour">The MonoBehaviour instance whose methods will be unregistered.</param>
        public static void UnregisterMethods(MonoBehaviour monoBehaviour)
        {
            foreach (var eventSubscribers in subscribers.Values)
            {
                eventSubscribers.RemoveAll(subscriber => subscriber.IsTarget(monoBehaviour));
            }
        }

        /// <summary>
        /// Unregisters a specific method by its name for a MonoBehaviour instance.
        /// </summary>
        /// <param name="monoBehaviour">The MonoBehaviour instance containing the method.</param>
        /// <param name="methodName">The name of the method to unregister.</param>
        public static void UnregisterMethod(MonoBehaviour monoBehaviour, string methodName)
        {
            foreach (var eventSubscribers in subscribers.Values)
            {
                eventSubscribers.RemoveAll(subscriber => subscriber.IsTarget(monoBehaviour) && subscriber.IsMethod(methodName));
            }
        }

        /// <summary>
        /// Fires an event by its name, invoking all registered subscribers with the provided parameters.
        /// </summary>
        /// <param name="eventName">The name of the event to fire.</param>
        /// <param name="parameters">The parameters to pass to the event listeners.</param>
        public static void Fire(string eventName, params object[] parameters)
        {
            if (subscribers.ContainsKey(eventName))
            {
                var eventSubscribers = subscribers[eventName];
                for (int i = eventSubscribers.Count - 1; i >= 0; i--)
                {
                    var subscriber = eventSubscribers[i];

                    if (subscriber.IsTargetAlive())
                    {
                        subscriber.Invoke(parameters);
                    }
                    else
                    {
                        eventSubscribers.RemoveAt(i);
                    }
                }
            }
        }
    }
}
