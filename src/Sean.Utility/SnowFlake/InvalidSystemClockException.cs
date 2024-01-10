using System;

namespace Sean.Utility.SnowFlake
{
    public class InvalidSystemClockException : Exception
    {
        public InvalidSystemClockException(string message) : base(message) { }
    }
}