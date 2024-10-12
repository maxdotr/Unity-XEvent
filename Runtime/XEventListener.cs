using System;

namespace XEvent
{
    /// <summary>
    /// An attribute used to mark methods as event listeners for a specific event name.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class XEventListener : Attribute
    {
        /// <summary>
        /// Gets the name of the event this method listens to.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="XEventListener"/> class.
        /// </summary>
        /// <param name="name">The name of the event this method listens to.</param>
        public XEventListener(string name)
        {
            Name = name;
        }
    }
}
