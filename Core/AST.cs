using System;
using Core.Interfaces;

namespace System.Runtime.CompilerServices
{
    internal static class IsExternalInit {}
}

namespace Core
{
    public record Token
    {
        // ReSharper disable once UnusedMember.Global
        public Guid Id => Guid.NewGuid();
    }

    #region Misc
    
    public record CommentToken(string Text) : Token;

    #endregion

    #region Singular

    public record AssignToken(string Variable, Token Body) : Token;

    public record WhileToken(Token Condition, Token Body) : Token;
    
    public record CondToken(Token Condition, Token IfToken, Token ElseToken) : Token;

    public record VarDeclToken(string Variable, string Type, Token Body) : Token;

    public record FunctionDeclToken(string Name, Formals Formals, Token Body) : Token;
    
    public record BlockToken(Tokens Tokens) : Token;

    public record FunctionCallToken(Token Receiver, Token Actuals) : Token;

    public record NegateToken(Token Token) : Token;

    public record NotToken(Token Token) : Token;
    
    public record AddToken(Token Left, Token Right) : Token;
    
    public record SubtractToken(Token Left, Token Right) : Token;
    
    public record DivideToken(Token Left, Token Right) : Token;
    
    public record MultiplyToken(Token Left, Token Right) : Token;

    public record AtomicToken(IConvertible Value) : Token;

    public record VariableToken(string Variable) : Token;
    
    public record InstantiationToken(string Class, Tokens Actuals) : Token;

    public record Formal(string Name, string Type) : Token;

    public record ClassToken(string Name, Formals Formals, string Inherits, Tokens Actuals, Tokens Features);

    #endregion
    
    #region SequenceToken
    
    public record Formals(IValueCollection<Formal> Inner) : Token;

    public record Tokens(IValueCollection<Token> Inner) : Token;

    #endregion
}