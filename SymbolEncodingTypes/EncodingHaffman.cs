using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Encoding.SymbolEncodingTypes
{
    public class Node
    {
        public string Symbol  { get; }
        public int Frequency  { get; }
        public Node NodeLeft  { get; }
        public Node NodeRight { get; }
        public bool IsLeaf { get { return NodeLeft == null && NodeRight == null; } }

        public Node(string symbol, int frequency)
        {
            Symbol = symbol;
            Frequency = frequency;
        }

        public Node(int frequency, Node nodeLeft, Node nodeRight)
        {
            Frequency = frequency;
            NodeLeft = nodeLeft;
            NodeRight = nodeRight;
        }
    }


    public class EncodingHaffman : ISymbolEncoder
    {
        public string Name { get { return "Алгоритм Хаффмана"; } }

        private List<Node> _nodes = new();

        private Dictionary<string, List<int>> _encodedSymbols = new();

        public Dictionary<string, int[]> EncodeSymbols(List<string> symbolSequence)
        {
            ClearFields();

            var symbolFrequences = CreateFrequences(symbolSequence);

            foreach(var line in symbolFrequences)
            {
                CreateNode(line.Key, line.Value);
            }

            CreateTree();

            CreateSymbolCodes(_nodes[0],new List<int>());

            return _encodedSymbols
                .ToDictionary(line => line.Key, line => line.Value.ToArray());
        }
        private void ClearFields()
        {
            _nodes.Clear();
            _encodedSymbols.Clear();
        }
        private Dictionary<string, int> CreateFrequences(List<string> symbolSequence)
        {
            return symbolSequence
                .GroupBy(s => s)
                .ToDictionary(symbol => symbol.Key, symbol => symbol.Count());
        }

        private void CreateNode(string key,int value)
        {
            _nodes.Add(new Node(key, value));
        }

        private void CreateTree()
        {
            while(_nodes.Count > 1)
            {
                _nodes = _nodes.OrderBy(instance => instance.Frequency).ToList();

                var left = _nodes[0];
                var right = _nodes[1];
                var totalFrequency = left.Frequency + right.Frequency;

                var parent = new Node(totalFrequency, left, right);

                _nodes.Remove(left);
                _nodes.Remove(right);
                _nodes.Add(parent);
            }
        }

        private void CreateSymbolCodes(Node node, List<int> bits)
        {
            if (node == null) return;

            if (node.IsLeaf)
            {
                if (bits.Count == 0)
                    bits.Add(0);
                _encodedSymbols.Add(node.Symbol, new List<int>(bits));
                return;
            }

            bits.Add(0);
            CreateSymbolCodes(node.NodeLeft, bits);
            bits.RemoveAt(bits.Count - 1);

            bits.Add(1);
            CreateSymbolCodes(node.NodeRight, bits);
            bits.RemoveAt(bits.Count - 1);
        }
    }

}
