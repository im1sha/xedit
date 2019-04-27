using System;
using System.Collections.Generic;
using System.Text;

namespace XEdit.Core
{
    /// <summary>
    /// Registers unique instanses
    /// </summary>
    public static class AppDispatcher
    {
        private static List<object> _registeredInstances = new List<object>();

        public static void Register<T>(T instance) where T : class
        {
            if (FindInstance<T>() != null)
            {
                throw new ApplicationException($"{typeof(T)} is already registered");
            }

            _registeredInstances.Add(instance);
        }

        public static T Get<T>() where T : class
        {
            return FindInstance<T>() ?? throw new ApplicationException($"{typeof(T)} is not registered");
        }

        private static T FindInstance<T>() where T : class
        {
            foreach (var item in _registeredInstances)
            {
                if (item.GetType() == typeof(T))
                {
                    return (T)item;
                }
            }
            return null;
        }
    }
}
