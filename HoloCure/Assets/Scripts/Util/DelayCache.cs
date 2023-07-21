using System.Collections.Generic;
using UnityEngine;

namespace Util
{
    public static class DelayCache
    {
        private static readonly Dictionary<float, WaitForSeconds> _container = new();
        private static readonly Dictionary<float, WaitForSecondsRealtime> _unscaledContainer = new();
        public static WaitForSeconds GetWaitForSeconds(float time)
        {
            if (false == _container.ContainsKey(time))
            {
                _container.Add(time, new WaitForSeconds(time));
            }

            return _container[time];
        }
        public static WaitForSecondsRealtime GetUnscaledWaitForSeconds(float time)
        {
            if (false == _unscaledContainer.ContainsKey(time))
            {
                _unscaledContainer.Add(time, new WaitForSecondsRealtime(time));
            }

            return _unscaledContainer[time];
        }
    }
}