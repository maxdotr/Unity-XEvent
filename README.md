# XEvent

## Overview

The **XEvent** system provides a dynamic event management system for Unity, built using reflection to register, unregister, and invoke event listeners at runtime. It offers greater flexibility over Unity's standard event system, making it more suitable for cases where events need to be dynamically managed or where looser coupling between components is required.

## Key Features
- **Dynamic Registration**: Methods are registered for events at runtime using reflection, providing flexibility in when and how methods respond to events.
- **Fine-Grained Control**: Specific methods can be dynamically registered and unregistered without having to recompile the project.
- **Flexible Event Invocation**: Events are fired by name, decoupling method signatures from specific event invocations.
- **Attribute-Based Event Binding**: Methods are marked with `[XEventListener]` attributes to specify which events they respond to.

## How to Use XEvent

### Step 1: Mark Methods with `[XEventListener]` Attribute

To start using the **XEvent** system, annotate methods with the `[XEventListener]` attribute, specifying the name of the event they should respond to. Here’s an example:

```csharp
[XEventListener("EventA")]
public void OnEventA() {
    Debug.Log("Event A triggered!");
}

[XEventListener("EventB")]
public void OnEventB(int value) {
    Debug.Log($"Event B triggered with value: {value}");
}
```

In the above example:
- `OnEventA` responds to an event named `"EventA"`.
- `OnEventB` responds to an event named `"EventB"` and expects an integer parameter.

### Step 2: Register Methods

You can register methods in two ways:
1. **Automatically Register All Methods**: Call `XEventManager.RegisterMethods()` to register all methods in a `MonoBehaviour` that have the `[XEventListener]` attribute.

```csharp
XEventManager.RegisterMethods(this);
```

2. **Manually Register Specific Methods**: Call `XEventManager.RegisterMethod()` to register specific methods by name.

```csharp
XEventManager.RegisterMethod(this, "OnEventA");
```

### Step 3: Fire Events

To trigger an event, use the `XEventManager.Fire()` method. This will invoke all registered methods that listen to the event.

```csharp
XEventManager.Fire("EventA"); // Triggers OnEventA
XEventManager.Fire("EventB", 42); // Triggers OnEventB with a value of 42
```

### Step 4: Unregister Methods

You can dynamically unregister methods when they are no longer needed, either by removing specific methods or unregistering all methods for a particular `MonoBehaviour`.

- **Unregister a Specific Method**:
```csharp
XEventManager.UnregisterMethod(this, "OnEventA");
```

- **Unregister All Methods**:
```csharp
XEventManager.UnregisterMethods(this);
```

### Differences from Unity’s Standard Event System

The **XEvent** system differs from Unity’s built-in event system (`UnityEvent` and delegates) in several key areas:

| Feature        | XEvent System                                                   | Unity's Event System (UnityEvent, Delegates)   |
|:---------------|:----------------------------------------------------------------|:-----------------------------------------------|
| Event Binding  | Attribute-based (`[XEventListener]`)                            | Explicit delegate or `UnityEvent.AddListener`  |
| Reflection     | Uses reflection to discover methods at runtime                  | No reflection, manual registration             |
| Registration   | Dynamic, registers methods at runtime                           | Static, pre-defined during development         |
| Invocation     | Fired by event name, decoupled from method                      | Direct invocation via delegate or event        |
| Unregistration | Dynamic, can unregister methods at runtime                      | Requires manual removal with `RemoveListener`  |
| Flexibility    | More flexible but incurs performance overhead due to reflection | More performant but less flexible              |
| Performance    | Moderate reflection overhead                                    | Minimal overhead, faster execution             |
## How Reflection Enables XEvent

Reflection is the key enabler of the dynamic nature of **XEvent**. It allows the system to discover methods and attributes at runtime, providing the following benefits:

- **Method Discovery**: Reflection is used to discover methods marked with the `[XEventListener]` attribute. This means that new methods can be added without changing the core logic of the event system, making it highly flexible.
- **Dynamic Method Invocation**: The `MethodInfo.Invoke()` method allows the system to call event listeners without needing explicit references to them in the code. This decouples event firing from method invocation, enabling more modular and scalable code.

Here’s how reflection is used in **XEvent**:
```csharp
MethodInfo method = monoBehaviour.GetType().GetMethod("OnEventA", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
if (method != null) {
    method.Invoke(monoBehaviour, null);
}
```
In this example, the method `OnEventA` is discovered at runtime and invoked. This allows for dynamic behavior and flexibility.

## Use Cases for XEvent

The **XEvent** system is particularly useful in the following scenarios:
- **Dynamic Event Systems**: If your game or application needs to manage events that are registered or unregistered at runtime, **XEvent** is a powerful tool to handle these dynamic scenarios.
- **Modular Game Systems**: For large, modular systems where components interact loosely, **XEvent** decouples components and simplifies their interactions through events.
- **Runtime Event Management**: You can manage a large number of events without boilerplate code. Reflection simplifies the discovery and invocation of event listeners.
