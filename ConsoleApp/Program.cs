using System;
using System.Text.RegularExpressions;
using FParsec.CSharp;
using Microsoft.FSharp.Core;
using static FParsec.CSharp.PrimitivesCS; // combinator functions
using static FParsec.CSharp.CharParsersCS; // pre-defined parsers

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var str = @"""  hello world!  \"" \n\r   """;
            
            var r = Regex(@"(?:[^\\""]|\\.)*");

            var p = Between('"', r, '"')
                .ParseString(str);

            Console.WriteLine(p.IsOk());

            if (p.IsOk())
            {
                Console.WriteLine(p.Result);
            }
            else
            {
                Console.WriteLine(p.Error);
            }
        }
    }
}