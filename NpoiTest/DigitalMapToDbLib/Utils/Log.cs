using System;

namespace DigitalMapToDB.DigitalMapParser.Utils
{
    internal class Log
    {
        public static void Err(string tag, string msg)
        {
            Console.WriteLine(tag + ":\t" + msg);
        }
    }
}