using System.Collections.Generic;
using UnityEngine;

namespace Util
{
    public static class TimeStore
    {
        private static readonly Dictionary<float, WaitForSeconds> _container = new Dictionary<float, WaitForSeconds>();
        public static WaitForSeconds GetWaitForSeconds(float seconds)
        {
            if (false == _container.ContainsKey(seconds))
            {
                _container.Add(seconds, new WaitForSeconds(seconds));
            }

            return _container[seconds];
        }
    }
}