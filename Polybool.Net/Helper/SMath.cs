using System;

namespace PolyBool.Net.Helper
{
    public static class SMath
    {
        public static T Min<T>(T a, T b) where T : IComparable
        {
            return a.CompareTo(b) < 0 ? a : b;
        }

        public static T Max<T>(T a, T b) where T : IComparable
        {
            return a.CompareTo(b) > 0 ? a : b;
        }

        public static T Abs<T>(this T a) where T : struct, IComparable
        {
            return a.CompareTo(default(T)) < 0 ? a.Neg() : a;
        }

        public static T Add<T>(this T a, T b) where T : struct => (a, b) switch
        {
            (int x, int y) => (T)(object)(x + y),
            (long x, long y) => (T)(object)(x + y),
            (float x, float y) => (T)(object)(x + y),
            (double x, double y) => (T)(object)(x + y),
            (decimal x, decimal y) => (T)(object)(x + y),
            _ => default,
        };

        public static T Sub<T>(this T a, T b) where T : struct => (a, b) switch
        {
            (int x, int y) => (T)(object)(x - y),
            (long x, long y) => (T)(object)(x - y),
            (float x, float y) => (T)(object)(x - y),
            (double x, double y) => (T)(object)(x - y),
            (decimal x, decimal y) => (T)(object)(x - y),
            _ => default,
        };

        public static T Mul<T>(this T a, T b) where T : struct => (a, b) switch
        {
            (int x, int y) => (T)(object)(x * y),
            (long x, long y) => (T)(object)(x * y),
            (float x, float y) => (T)(object)(x * y),
            (double x, double y) => (T)(object)(x * y),
            (decimal x, decimal y) => (T)(object)(x * y),
            _ => default,
        };

        public static T Div<T>(this T a, T b) where T : struct => (a, b) switch
        {
            (int x, int y) => (T)(object)(x / y),
            (long x, long y) => (T)(object)(x / y),
            (float x, float y) => (T)(object)(x / y),
            (double x, double y) => (T)(object)(x / y),
            (decimal x, decimal y) => (T)(object)(x / y),
            _ => default,
        };

        public static T Neg<T>(this T a) where T : struct => a switch
        {
            int x => (T)(object)-x,
            long x => (T)(object)-x,
            float x => (T)(object)-x,
            double x => (T)(object)-x,
            decimal x => (T)(object)-x,
            _ => default,
        };
    }
}
