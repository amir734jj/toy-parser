# toy-parser

Parser for variation of toy object oriented language

```scala
class Foo() extends IO() {
  def fibonacci(x: Int): Int = if (n <= 1) n else fibonacci(n - 1) + fibonacci(n - 2);

  def assertEquals(expected: Int, actual: Int): Unit = if (expected != actual)
    out(
      "expected: "
        .concact(expected.toString())
        .concact(" but received: ")
        .concact(actual.toString())
    )
  else
    out("passed!");

  { assertEquals(34, fibonacci(9)) }
}
```
