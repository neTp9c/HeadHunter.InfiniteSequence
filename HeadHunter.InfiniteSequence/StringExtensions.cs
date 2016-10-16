using System;
namespace HeadHunter.InfiniteSequence
{
    static class StringExtensions
    {
        public static string Right(this string value, int length)
        {
            return value.Substring(value.Length - length);
        }
    }
}
