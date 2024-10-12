using System;
using System.Reflection;

namespace XEvent
{
    /// <summary>
    /// Represents an object that holds a reference to an event handler and can invoke it dynamically.
    /// </summary>
    public class XEventObject
    {
        private string _name;
        private MethodInfo _methodInfo;
        private WeakReference _target;

        /// <summary>
        /// Initializes a new instance of the <see cref="XEventObject"/> class.
        /// </summary>
        /// <param name="name">The name of the event.</param>
        /// <param name="target">The target object to which the method belongs.</param>
        /// <param name="methodInfo">The method to be invoked.</param>
        public XEventObject(string name, object target, MethodInfo methodInfo)
        {
            _name = name;
            _target = new WeakReference(target);
            _methodInfo = methodInfo;
        }

        /// <summary>
        /// Checks whether the target object is still alive.
        /// </summary>
        /// <returns><c>true</c> if the target object is alive; otherwise, <c>false</c>.</returns>
        public bool IsTargetAlive()
        {
            return _target.IsAlive;
        }

        /// <summary>
        /// Checks whether this object is referencing the specified target.
        /// </summary>
        /// <param name="target">The target object to check against.</param>
        /// <returns><c>true</c> if the target matches; otherwise, <c>false</c>.</returns>
        public bool IsTarget(object target)
        {
            return _target.Target == target;
        }

        /// <summary>
        /// Checks if this object is handling the specified method name.
        /// </summary>
        /// <param name="methodName">The name of the method to check.</param>
        /// <returns><c>true</c> if the method name matches; otherwise, <c>false</c>.</returns>
        public bool IsMethod(string methodName)
        {
            return _methodInfo.Name == methodName;
        }

        /// <summary>
        /// Dynamically invokes the method with the specified parameters.
        /// </summary>
        /// <param name="parameters">The parameters to pass to the method.</param>
        public void Invoke(params object[] parameters)
        {
            try
            {
                _methodInfo?.Invoke(_target.Target, parameters);
            }
            catch (Exception ex)
            {
                throw new Exception($"Could not invoke event {_name} to listener method {_methodInfo.Name} from class {_methodInfo.ReflectedType} due to a mismatch of types. " +
                                    $"{_name} has already been defined, and all listeners must have the same parameters. {ex.Message}");
            }
        }
    }
}
