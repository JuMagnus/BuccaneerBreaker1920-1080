using System;
using System.Collections.Generic;

namespace BuccaneerBreaker
{
    public static class ServicesLocator
    {
        private static Dictionary<Type, object> _servicesList = new Dictionary<Type, object>();

        public static void Register<T>(T service)
        {
            _servicesList.Add(typeof(T), service);
        }

        public static T Get<T>()
        {

            return (T)_servicesList[typeof(T)];
        }

    }
}