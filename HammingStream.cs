using System;
using System.Collections.Generic;
using System.Linq;

public static class HammingStream
{
    private const int DataBlockSize = 4;
    private const int CodeBlockSize = 7;

    public static List<int> Encode(List<int> bits, out int padding)
    {
        var data = new List<int>(bits);

        padding = (DataBlockSize - data.Count % DataBlockSize) % DataBlockSize;
        for (int i = 0; i < padding; i++)
            data.Add(0);

        var encoded = new List<int>();

        for (int i = 0; i < data.Count; i += DataBlockSize)
            encoded.AddRange(EncodeBlock(data.GetRange(i, 4)));

        return encoded;
    }

    private static List<int> EncodeBlock(List<int> d)
    {
        int[] b = new int[7];

        b[2] = d[0];
        b[4] = d[1];
        b[5] = d[2];
        b[6] = d[3];

        b[0] = b[2] ^ b[4] ^ b[6];
        b[1] = b[2] ^ b[5] ^ b[6]; 
        b[3] = b[4] ^ b[5] ^ b[6]; 

        return b.ToList();
    }


    public static void InjectError(List<int> encoded, int index)
    {
        encoded[index] ^= 1;
    }


    public static List<int> DecodeAndFix(
        List<int> encoded,
        out List<int> errorPositions)
    {
        errorPositions = new List<int>();
        var decoded = new List<int>();

        for (int i = 0; i < encoded.Count; i += CodeBlockSize)
        {
            var block = encoded.GetRange(i, CodeBlockSize);
            int error = CheckAndFixBlock(block);

            if (error != 0)
                errorPositions.Add(i + error - 1);

            decoded.Add(block[2]);
            decoded.Add(block[4]);
            decoded.Add(block[5]);
            decoded.Add(block[6]);
        }

        return decoded;
    }

    private static int CheckAndFixBlock(List<int> b)
    {
        int s1 = b[0] ^ b[2] ^ b[4] ^ b[6];
        int s2 = b[1] ^ b[2] ^ b[5] ^ b[6];
        int s4 = b[3] ^ b[4] ^ b[5] ^ b[6];

        int pos = s1 * 1 + s2 * 2 + s4 * 4;

        if (pos != 0)
            b[pos - 1] ^= 1;

        return pos;
    }

    public static string ToBitString(List<int> bits)
        => string.Join("", bits);

    public static string GetParityBitsString(List<int> encoded)
    {
        var result = new List<string>();

        for (int i = 0; i < encoded.Count; i += CodeBlockSize)
        {
            result.Add($"[{encoded[i]}{encoded[i + 1]}{encoded[i + 3]}]");
        }

        return string.Join(" ", result);
    }
}
