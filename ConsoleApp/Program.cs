using System;
using System.IO;
using Core;
using FParsec.CSharp;

namespace ConsoleApp
{
    class Program
    {   
        static void Main(string[] args)
        {
            var text = File.ReadAllText("basic.toy");

            var reply = Parser.Classes().ParseString(text);
            
            Console.WriteLine(reply.IsOk());
            Console.WriteLine(reply.Result);
        }
    }
}