using System;
using System.Text.RegularExpressions;
using Core;
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
            var text = @"
class NoExplicitSuper0() { }
class NoExplicitSuper1(var x : Int) { }
class NoExplicitSuper2(var x : Int, var x : Int) { }
class NoExplicitSuper3(var x : Int, var x : Int, var x : Int) { }

class ExplicitSuper0() extends Super(x+1,""x"",if (x) x else x) { }
class ExplicitSuper1(var x : Int) extends Super({x}) { }
class ExplicitSuper2(var x : Int, var x : Int) extends Super(x) { }
class ExplicitSuper3(var x : Int, var x : Int, var x : Int) extends Super() { }
";

            var reply = Parser.Classes().ParseString(text);
            
            Console.WriteLine(reply.IsOk());
            
        }
    }
}