using UnityEngine;
using XEvent;

public class XEventDemo : MonoBehaviour
{
    // This method listens to "EventA" and will be invoked when "EventA" is fired.
    [XEventListener("EventA")]
    public void OnEventA()
    {
        Debug.Log("OnEventA: Event A was fired!");
    }

    // This method listens to "EventB" and will be invoked when "EventB" is fired.
    [XEventListener("EventB")]
    public void OnEventB(int value)
    {
        Debug.Log($"OnEventB: Event B was fired with value: {value}");
    }

    // This method is used to show how to dynamically register methods
    void Start()
    {
        // Registering all methods with XEventListener attributes for this MonoBehaviour instance
        XEventManager.RegisterMethods(this);

        // Fire the events to test the initial registration
        Debug.Log("Firing EventA and EventB with initial registration...");
        XEventManager.Fire("EventA");
        XEventManager.Fire("EventB", 42);

        // Unregistering the "OnEventA" method dynamically
        XEventManager.UnregisterMethod(this, "OnEventA");

        // Fire the events again to show that OnEventA is no longer invoked
        Debug.Log("Firing EventA and EventB after unregistering OnEventA...");
        XEventManager.Fire("EventA"); // This should not invoke OnEventA
        XEventManager.Fire("EventB", 100); // This will still invoke OnEventB

        // Registering only "OnEventA" method dynamically
        XEventManager.RegisterMethod(this, "OnEventA");

        // Fire the events again after re-registering OnEventA
        Debug.Log("Firing EventA and EventB after re-registering OnEventA...");
        XEventManager.Fire("EventA"); // Now this should invoke OnEventA again
        XEventManager.Fire("EventB", 200); // This will still invoke OnEventB
    }

    // Unregister all methods when the object is destroyed to prevent dangling references
    void OnDestroy()
    {
        XEventManager.UnregisterMethods(this);
        Debug.Log("XEventDemo destroyed and unregistered from all events.");
    }
}
