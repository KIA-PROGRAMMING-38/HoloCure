using System.Collections.Generic;

namespace StringLiterals
{
    public static class NumLiteral
    {
        private static readonly Dictionary<int, string> _container = new();
        public static string GetNumString(int num)
        {
            if (false == _container.ContainsKey(num))
            {
                _container.Add(num, $"{num}");
            }

            return _container[num];
        }
        private static readonly string[] _timeContainer = { "00", "01", "02", "03", "04", "05", "06", "07", "08", "09" };
        public static string GetTimeNumString(int num)
        {
            if (num < 10)
            {
                return _timeContainer[num];
            }

            return GetNumString(num);
        }
    }
}
