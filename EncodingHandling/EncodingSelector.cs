using Encoding.BitEncodingTypes;
using Encoding.SymbolEncodingTypes;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Encoding.EncodingHandling
{
    static class EncodingSelector
    {
        private static List<IBitEncoder> _bitEncoders = new();
        private static List<ISymbolEncoder> _symbolEncoders = new();

        static EncodingSelector()
        {
            LoadEncoders();
        }

        private static void LoadEncoders()
        {
            var bitEncoderType = typeof(IBitEncoder);
            var symbolEncoderType = typeof(ISymbolEncoder);
            var types = Assembly.GetExecutingAssembly().GetTypes();

            foreach (var type in types)
            {
                if (bitEncoderType.IsAssignableFrom(type) && !type.IsInterface) _bitEncoders.Add((IBitEncoder)Activator.CreateInstance(type));
                if (symbolEncoderType.IsAssignableFrom(type) && !type.IsInterface) _symbolEncoders.Add((ISymbolEncoder)Activator.CreateInstance(type));
            }
        }

        public static IBitEncoder SelectBitEncoder()
        {
            int index = 0;
            Console.WriteLine("Тип кодирования битовой последовательности:");

            while (true)
            {
                ClearLine();
                Console.Write(_bitEncoders[index].Name);
                ConsoleKeyInfo keyValue = Console.ReadKey(true);

                if (keyValue.Key == ConsoleKey.RightArrow) index++;
                if (keyValue.Key == ConsoleKey.LeftArrow) index--;

                if (index < 0) index = _bitEncoders.Count - 1;
                if (index >= _bitEncoders.Count) index = 0;

                if (keyValue.Key == ConsoleKey.Enter) break;
            }
            return _bitEncoders[index];
        }

        public static ISymbolEncoder SelectSymbolEncoder()
        {
            int index = 0;
            Console.WriteLine("Тип кодирования символьной последовательности:");

            while (true)
            {
                ClearLine();
                Console.Write(_symbolEncoders[index].Name);
                ConsoleKeyInfo keyValue = Console.ReadKey(true);

                if (keyValue.Key == ConsoleKey.RightArrow) index++;
                if (keyValue.Key == ConsoleKey.LeftArrow) index++;

                if (index < 0) index = _symbolEncoders.Count - 1;
                if (index >= _symbolEncoders.Count) index = 0;

                if (keyValue.Key == ConsoleKey.Enter) break;
            }
            return _symbolEncoders[index];
        }

        private static void ClearLine()
        {
            int currentLineCursor = Console.CursorTop;
            Console.SetCursorPosition(0, currentLineCursor);
            Console.Write(new string(' ', Console.BufferWidth));
            Console.SetCursorPosition(0, currentLineCursor);
        }
    }
}
