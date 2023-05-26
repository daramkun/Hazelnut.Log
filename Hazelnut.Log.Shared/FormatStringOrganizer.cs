using System.Runtime.CompilerServices;
using Hazelnut.Log.Utils;

namespace Hazelnut.Log;

internal class FormatStringOrganizer
{
    private class Node { }
    private class TextNode : Node
    {
        public readonly string Text;
        
        public TextNode(string text)
        {
            Text = text;
        }
    }

    private class VariableNode : Node
    {
        public readonly string VariableName;
        public readonly string? FormatString;
        
        public VariableNode(string variableName, string? formatString)
        {
            VariableName = variableName;
            FormatString = formatString;
        }
    }

    private enum ParseState
    {
        Text,
        VariableName,
        FormatString,
    }

    private readonly Node[] _nodes;

    public FormatStringOrganizer(string str)
    {
        using var reader = new StringReader(str);

        var nodes = new List<Node>();

        var state = ParseState.Text;
        using var builder = new Cysharp.Text.Utf16ValueStringBuilder(true);
        var tempStr = string.Empty;

        int iCh;
        while ((iCh = reader.Read()) != -1)
        {
            var ch = (char)iCh;

            switch (state)
            {
                case ParseState.Text:
                    if (ch == '$' && reader.Peek() == '{')
                    {
                        reader.Read();
                        if (builder.Length > 0)
                        {
                            nodes.Add(new TextNode(builder.ToString()));
                            builder.Clear();
                        }

                        state = ParseState.VariableName;
                    }
                    else
                        builder.Append(ch);
                    break;
                case ParseState.VariableName:
                    switch (ch)
                    {
                        case ':' when builder.Length == 0:
                        case '}' when builder.Length == 0:
                            throw new InvalidOperationException("variable name is empty.");
                        
                        case ':':
                            tempStr = builder.ToString();
                            builder.Clear();
                            state = ParseState.FormatString;
                            break;
                        
                        case '}':
                            nodes.Add(new VariableNode(builder.ToString(), string.Empty));
                            builder.Clear();
                            state = ParseState.Text;
                            break;
                        
                        case '\\':
                            var nextCh = reader.Read();
                            if (nextCh == -1)
                                throw new InvalidOperationException("end of string.");
                            ch = (char)nextCh;
                            if (char.IsSymbol(ch) && ch != '_')
                                throw new InvalidOperationException("special characters no allowed to variable name.");
                            builder.Append(ch);
                            break;
                        
                        default:
                            if (char.IsSymbol(ch) && ch != '_')
                                throw new InvalidOperationException("special characters no allowed to variable name.");
                            builder.Append(ch);
                            break;
                    }

                    break;
                case ParseState.FormatString:
                    switch (ch)
                    {
                        case '}':
                            nodes.Add(new VariableNode(tempStr, builder.Length > 0 ? builder.ToString() : null));
                            builder.Clear();
                            state = ParseState.Text;
                            break;
                        
                        case '\\':
                            var nextCh = reader.Read();
                            if (nextCh == -1)
                                throw new InvalidOperationException("end of string.");
                            ch = (char)nextCh;
                            builder.Append(ch);
                            break;
                        
                        default:
                            builder.Append(ch);
                            break;
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(str));
            }
        }
        
        if (state == ParseState.Text && builder.Length > 0)
            nodes.Add(new TextNode(builder.ToString()));
        else if (state is ParseState.VariableName or ParseState.FormatString && builder.Length > 0)
            throw new InvalidOperationException("end of string.");

        _nodes = nodes.ToArray();
    }

    public string Format(Variables variables, LogLevel logLevel, string message)
    {
        if (_nodes is [TextNode onlyTextNode])
            return onlyTextNode.Text;
        
        using var builder = new Cysharp.Text.Utf16ValueStringBuilder(true);
        foreach (var node in _nodes)
        {
            switch (node)
            {
                case TextNode textNode: builder.Append(textNode.Text); break;
                case VariableNode { VariableName: "Message" }: builder.Append(message); break;
                case VariableNode { VariableName: "LogType" }: builder.Append(logLevel); break;
                case VariableNode { VariableName: "ShortLogType" }: builder.Append(GetShortLogType(logLevel)); break;
                case VariableNode variableNode:
                {
                    var text = variables.GetVariable(variableNode.VariableName, variableNode.FormatString);
                    if (text != null)
                        builder.Append(text);
                    break;
                }
            }
        }

        return builder.ToString();
    }

    public string Format(Variables variables)
    {
        if (_nodes is [TextNode onlyTextNode])
            return onlyTextNode.Text;
        
        using var builder = new Cysharp.Text.Utf16ValueStringBuilder(true);
        foreach (var node in _nodes)
        {
            switch (node)
            {
                case TextNode textNode: builder.Append(textNode.Text); break;
                case VariableNode variableNode:
                {
                    var text = variables.GetVariable(variableNode.VariableName, variableNode.FormatString);
                    if (text != null)
                        builder.Append(text);
                    break;
                }
            }
        }

        return builder.ToString();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static char GetShortLogType(LogLevel logLevel) =>
        logLevel switch
        {
            LogLevel.Debug => 'D',
            LogLevel.Information => 'I',
            LogLevel.Warning => 'W',
            LogLevel.Error => 'E',
            LogLevel.Fatal => 'F',
            LogLevel.Notice => 'N',
            _ => throw new ArgumentOutOfRangeException(nameof(logLevel))
        };
}