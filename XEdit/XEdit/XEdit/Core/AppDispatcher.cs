using System;
using System.Collections.Generic;
using System.Text;

namespace XEdit.Core
{
    public static class AppDispatcher
    {
        private static List<object> _registeredInstances = new List<object>();

        public static void Register<T>() where T : class, new()
        {
            if (FindInstance<T>() != null)
            {
                throw new ApplicationException($"{typeof(T)} is already registered");
            }

            _registeredInstances.Add(new T());
        }

        public static T Get<T>() where T : class, new()
        {
            return FindInstance<T>() ?? throw new ApplicationException($"{typeof(T)} is not registered");
        }

        private static T FindInstance<T>() where T : class, new()
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
