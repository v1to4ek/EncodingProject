using Encoding.EncodingHandling;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Encoding
{
    class Program
    {
        static void Main()
        {
            var bitSequence = SelectSequence();

            if (ShouldUseHamming()) bitSequence = RunHammingConsole(bitSequence);

            if (ShouldEncode())
            {
                var encoderType = EncodingSelector.SelectBitEncoder();
                Encoder encoder = new(encoderType);
                encoder.Encode(bitSequence);
            }

            Console.ReadLine();
        }

        static bool ShouldUseHamming()
        {
            string[] answers = ["Да", "Нет"];
            string answer;
            Console.WriteLine("Использовать алгоритм Хэмминга?");
            int i = 0;

            while (true)
            {
                Console.Write(answers[i]);
                ConsoleKeyInfo keyValue = Console.ReadKey(true);
                if (keyValue.Key == ConsoleKey.RightArrow) i++;
                if (keyValue.Key == ConsoleKey.LeftArrow) i++;
                i %= 2;
                if (keyValue.Key == ConsoleKey.Enter)
                {
                    Console.WriteLine();
                    break;
                }
                ClearLine();
            }
            answer = answers[i];

            Console.WriteLine("----------------------------------------------");

            if (answer == "Да") return true;
            else return false;
        }

        static bool ShouldEncode()
        {
            string[] answers = ["Да", "Нет"];
            string answer;
            Console.WriteLine("Кодировать битовую последовательность?");
            int i = 0;

            while (true)
            {
                Console.Write(answers[i]);
                ConsoleKeyInfo keyValue = Console.ReadKey(true);
                if (keyValue.Key == ConsoleKey.RightArrow) i++;
                if (keyValue.Key == ConsoleKey.LeftArrow) i++;
                i %= 2;
                if (keyValue.Key == ConsoleKey.Enter)
                {
                    Console.WriteLine();
                    break;
                }
                ClearLine();
            }
            answer = answers[i];

            Console.WriteLine("----------------------------------------------");

            if (answer == "Да") return true;
            else return false;
        }

        public static List<int> RunHammingConsole(List<int> input)
        {
            Console.WriteLine("Исходные данные:");
            Console.WriteLine(HammingStream.ToBitString(input));
            Console.WriteLine("----------------------------------------------");

            var encoded = HammingStream.Encode(input, out int padding);

            Console.WriteLine("Закодированная строка:");
            Console.WriteLine(HammingStream.ToBitString(encoded));
            Console.WriteLine("----------------------------------------------");
            Console.WriteLine("Проверочные биты (P1 P2 P4 для каждого блока):");
            Console.WriteLine(HammingStream.GetParityBitsString(encoded));
            Console.WriteLine("----------------------------------------------");


            Console.Write("Введите позицию ошибки (0-based): ");
            int errorIndex = int.Parse(Console.ReadLine());
            Console.WriteLine("----------------------------------------------");

            HammingStream.InjectError(encoded, errorIndex);

            Console.WriteLine("Строка с ошибкой:");
            Console.WriteLine(HammingStream.ToBitString(encoded));
            Console.WriteLine("----------------------------------------------");

            var decoded = HammingStream.DecodeAndFix(
                encoded,
                out var errorPositions
            );

            if (padding > 0)
                decoded.RemoveRange(decoded.Count - padding, padding);

            Console.WriteLine("Исправленная строка:");
            Console.WriteLine(HammingStream.ToBitString(decoded));
            Console.WriteLine("----------------------------------------------");

            Console.WriteLine("Найдены ошибки в позициях:");
            Console.WriteLine(errorPositions.Count == 0
                ? "Ошибок нет"
                : string.Join(", ", errorPositions));
            Console.WriteLine("----------------------------------------------");

            return decoded;
        }

        static List<int> SelectSequence()
        {

            string[] types = ["Фиксированная бинарная", "Последовательность символов"];
            string type;
            Console.WriteLine("Выберите тип последовательности:");
            int i = 0;

            while (true)
            {
                Console.Write(types[i]);
                ConsoleKeyInfo keyValue = Console.ReadKey(true);
                if (keyValue.Key == ConsoleKey.RightArrow) i++;
                if (keyValue.Key == ConsoleKey.LeftArrow) i++;
                i %= 2;
                if (keyValue.Key == ConsoleKey.Enter)
                {
                    Console.WriteLine();
                    break;
                }
                ClearLine();
            }
            type = types[i];

            Console.WriteLine("----------------------------------------------");

            switch (type)
            {
                case "Фиксированная бинарная":

                    return CreateBitSequence();

                case "Последовательность символов":

                    var sequenceToEncode = CreateSymbolSequence();
                    return sequenceToEncode.BitSequence.ToList();

                default:

                    throw new Exception("Неизвестный тип последовательности");
            }
        }

        static Sequence CreateSymbolSequence()
        {
            Console.WriteLine("Введите текст для кодирования:");
            string text = Console.ReadLine();
            Console.WriteLine("----------------------------------------------");

            var encoderType = EncodingSelector.SelectSymbolEncoder();
            Sequence sequence = new(text, encoderType);
            Console.WriteLine();
            Console.WriteLine("----------------------------------------------");

            Console.WriteLine("Битовая последовательность:");
            Console.WriteLine(sequence);
            Console.WriteLine("----------------------------------------------");

            Console.WriteLine("Закодированные символы:");
            sequence.PrintEncodedSymbols();
            Console.WriteLine("----------------------------------------------");

            Console.WriteLine("Декодированная последовательность:");
            sequence.PrintDecodedSequence();
            Console.WriteLine("----------------------------------------------");

            return sequence;
        }

        static List<int> CreateBitSequence()
        {
            var bitSequence = "10101010111001110111111000110110";

            if (bitSequence.All(c => c != '0' && c != '1')) throw new ArgumentException("Недопустимый символ в последовательности");

            List<int> bitSequenceList = bitSequence.Select(c => c - '0').ToList();

            Console.Write($"Битовая последовательность: ");

            foreach(var bit in bitSequenceList)
            {
                Console.Write(bit);
            }
            Console.WriteLine();
            Console.WriteLine("----------------------------------------------");
            return bitSequenceList;
        }

        static void ClearLine()
        {
            int currentLineCursor = Console.CursorTop;
            Console.SetCursorPosition(0, currentLineCursor);
            Console.Write(new string(' ', Console.BufferWidth));
            Console.SetCursorPosition(0, currentLineCursor);
        }
    }
}
