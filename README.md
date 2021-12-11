# toy-parser

![Icon](https://img.icons8.com/ios-glyphs/90/000000/rubber-duck.png)

Parser for toy object oriented language using FParsec



## Example

```scala
class Foo() extends IO() {
  def fibonacci(n: Int): Int = if (n <= 1)
    n
  else
    fibonacci(n - 1) + fibonacci(n - 2);

  def assertEquals(expected: Any, actual: Any, msg: String): Unit = if (expected != actual)
    out(
      "["
        .concat(ms)
        .concat("]")
        .concat("expected: ")
        .concact(expected.toString())
        .concact(" but received: ")
        .concact(actual.toString())
    )
  else
    out("passed!");

  { assertEquals(34, fibonacci(9), "fibonacci") }
}
```
## Syntax

If you are familiar with Scala, then this should be very familiar. This is the CFG in a nutshell:

```regex
// This is a single line comment
/* this is a multiline comment */

<Name>         = [^:" {}=()\n;,*!.<>]+
               ;
             
<Actuals>      = '(' (<Expr> ,)+ <Expr> ')' | '(' <Expr> ')' | '(' ')'
               ;
             
<Arm>          = case null '=>' <Expr> | case <Name> ':' <Name> '=>' <Expr>
               ;
             
<Arms>         = '{' (<Arm> ,)+ <Arm> '}' | '{' <Arm> '}' | '{' '}'
               ;
             
<Expr>         = if '(' <Expr> ')' else <Expr>
               | while '(' <Expr> ')' <Expr>
               | match <Expr> with Arms
               | <Name> <Actuals>
               | var <Name> '=' <Expr>
               | <Name> '=' <Expr>
               | '{' (<Expr> ';')+ <Expr> '}' | '{' <Expr> '}' | '{' '}'
               | [\d]+
               | <Name>
               | null
               | true | false
               | '(' <Expr> ')'
               | native
               | <Expr> '.' <Expr>
               | <Expr> '+' <Expr> | <Expr> '-' <Expr> | <Expr> '*' <Expr> | <Expr> '/' <Expr>
               | <Expr> '<' <Expr> | <Expr> '<=' <Expr>
               | '-' <Expr>
               | '!' <Expr>
               | <Expr> '==' <Expr> | <Expr> '!=' <Expr>
               ;

<Formal>       = <Name> ':' <Name> 
               ;
             
<Formals>      = '(' (<Formal> ';')+ <Formal> ')' | '(' (<Formal> ')' | '(' ')'
               ;

<FunctionDecl> = def <Name> <Formals> : <Name> = <Expr>
               ;

<Feature>      = <Expr> | <FunctionDecl>
               ;
             
<Features>     = '{' (<Feature> ;)+ <Feature> '}' | '{' <Feature> '}' | '{' '}'
               ;
             
<ClassDecl>    = class <Name> <Formals> Features
               | class <Name> <Formals> extends <Name> <Actuals> Features
               | class <Name> <Formals> extends native Features
               ;
```
