// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;

namespace Microsoft.DotNet.Cli.Build.Framework
{
    public class AnsiConsole
    {
        private AnsiConsole(TextWriter writer)
        {
            Writer = writer;
    
            OriginalForegroundColor = Console.ForegroundColor;
        }
    
        private int _boldRecursion;
    
        public static AnsiConsole GetOutput()
        {
            return new AnsiConsole(Console.Out);
        }
    
        public static AnsiConsole GetError()
        {
            return new AnsiConsole(Console.Error);
        }
    
        public TextWriter Writer { get; }
    
        public ConsoleColor OriginalForegroundColor { get; }
    
        private void SetColor(ConsoleColor color)
        {
            const int Light = 0x08;
            int c = (int)color;

            Console.ForegroundColor = 
                c < 0 ? color :                                   // unknown, just use it
                _boldRecursion > 0 ? (ConsoleColor)(c | Light) :  // ensure color is light
                (ConsoleColor)(c & ~Light);                       // ensure color is dark
        }
    
        private void SetBold(bool bold)
        {
            _boldRecursion += bold ? 1 : -1;
            if (_boldRecursion > 1 || (_boldRecursion == 1 && !bold))
            {
                return;
            }
            
            // switches on _boldRecursion to handle boldness
            SetColor(Console.ForegroundColor);        
        }

        public void WriteLine(string message)
        {
            Write(message);
            Writer.WriteLine();
        }


        public void Write(string message)
        {
            Writer.Write(message);
        }
    }
}
