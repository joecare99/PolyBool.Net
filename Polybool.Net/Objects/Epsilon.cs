namespace PolyBool.Net.Objects
{
    public static class Epsilon
    {
        public static decimal Eps { get; set; } = 0.00001m;
    }

    public static class Epsilon<T> where T : struct
    {
        public static T Eps { get; set; } = (T)(object)0.00001;
    }
}