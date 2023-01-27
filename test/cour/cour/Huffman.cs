using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cour
{
    //общий класс
    public class Node
    {
        //поле частоты
        public double Frequency { get; set; }
        //поле символов
        public char Symbol { get; set; }
        //поле левого эл-та
        public Node Left { get; set; }
        //поле правого эл-та
        public Node Right { get; set; }
        //метод, объявляющий, что этот эл-т - лист
        public bool IsLeaf
        {
            get => Left == null && Right == null;
        }

        public Node(char symbol, double frequency)
        {
            Frequency = frequency;
            Symbol = symbol;
        }
    }
    public class HuffmanCode
    {
        string tex;
        //словарь для хранения значений частот
        public Dictionary<char, int> Frequencies { get;  set; }
        //дерево хаффмана
        public Node HuffmanTree { get;  set; }
        //словарь для хранения значиений кода каждого символа

        public Dictionary<char, string> CodeTable { get;  set; }
       
        private HuffmanCode(string text, Dictionary<char, int> frequencies)
        {
            if (text != null)
            {
                tex = text;
                Frequencies = ComputeFrequencies(text);
                FieldsInitialize();
            }
            else if (frequencies != null)
            {
                Frequencies = frequencies;
                FieldsInitialize();
            }
        }
        // для вывода на интерфейс и тестов 
        public HuffmanCode(string text)
            : this(text, null) { }

        public HuffmanCode(Dictionary<char, int> alphabetFrequencies)
            : this(null, alphabetFrequencies) { }
        //метод кодирования
        public string Encode(string text)
        {
            var encode = new StringBuilder();
            foreach (var symbol in text)
                encode.Append(CodeTable[symbol]);

            return encode.ToString();
        }

        public string Encode()
        {
            if (tex != null)
                return Encode(tex);
            else
                throw new Exception("Текст не был задан!");
        }
        //метод декодирования
        public string Decode(string huffmanCode)
        {
            Node currentNode = HuffmanTree;
            var decode = new StringBuilder();

            foreach (var bit in huffmanCode)
            {
                if (bit == '0')
                    currentNode = currentNode.Left;
                else if (bit == '1')
                    currentNode = currentNode.Right;
                else
                    throw new ArgumentException("Код Хаффмана должен содержать только 0 или 1.");

                if (currentNode.IsLeaf)
                {
                    decode.Append(currentNode.Symbol);
                    currentNode = HuffmanTree;
                }
            }

            return decode.ToString();
        }
        //инициализация полей
        private void FieldsInitialize()
        {
            HuffmanTree = BuildHuffmanTree(Frequencies);
            CodeTable = new Dictionary<char, string>();
            FillCodeTable(HuffmanTree);
        }
        //вычисление частот
        private Dictionary<char, int> ComputeFrequencies(string text)
        {
            var alphabetFrequencies = new Dictionary<char, int>();
            foreach (var symbol in text)
            {
                if (alphabetFrequencies.ContainsKey(symbol))
                    alphabetFrequencies[symbol]++;
                else
                    alphabetFrequencies.Add(symbol, 1);
            }

            return alphabetFrequencies;
        }
        //формироваение древа хаффмана
        private Node BuildHuffmanTree(Dictionary<char, int> alphabetFrequencies)
        {
            List<Node> nodes = ToListNodes(alphabetFrequencies);

            while (nodes.Count > 1)
            {
                List<Node> taken = RemoveTwoMinNodes(nodes);
                Node parent = CreateParentNode(taken);
                nodes.Add(parent);
            }

            return nodes.FirstOrDefault();
        }
        //заполнение таблицы символов
        private void FillCodeTable(Node currentNode, string bitString = "")
        {
            if (currentNode != null)
            {
                if (currentNode.IsLeaf)
                {
                    CodeTable[currentNode.Symbol] = bitString;
                    return;
                }

                FillCodeTable(currentNode.Left, bitString + '0');
                FillCodeTable(currentNode.Right, bitString + '1');
            }
        }
        //перечисление узлов
        private List<Node> ToListNodes(Dictionary<char, int> alphabetFrequencies)
        {
            var nodes = new List<Node>();
            foreach (var pair in alphabetFrequencies)
                nodes.Add(new Node(pair.Key, pair.Value));

            return nodes;
        }
        //удаление узла
        private List<Node> RemoveTwoMinNodes(List<Node> nodes)
        {
            List<Node> orderedNodes = SortAscending(nodes);
            List<Node> taken = orderedNodes.Take(2).ToList();
            nodes.Remove(taken[0]);
            nodes.Remove(taken[1]);

            return taken;
        }
        //сортировка частот по возрастанию
        private List<Node> SortAscending(List<Node> nodes)
            => nodes.OrderBy(node => node.Frequency).ToList<Node>();
        //родительский узел
        private Node CreateParentNode(List<Node> taken)
        {
            return new Node('*', taken[0].Frequency + taken[1].Frequency)
            {
                Left = taken[0],
                Right = taken[1]
            };
        }
    }
    //класс для подсчета энтропии и средней длины сообщения
    public class InformationText
    {
        string tex;
        HuffmanCode huffman;
        Node huffmantree;
        Dictionary<char, double> probabilit;
        Dictionary<char, int> depth;
        //поле алфавита
        public char[] Alphabet { get; }
        //вывод расчетов
        public InformationText(string text)
        {
            tex = text;
            huffman = new HuffmanCode(text);
            Alphabet = huffman.CodeTable.Keys.ToArray();
            probabilit = ToProbabilities(huffman.Frequencies);
            huffmantree = huffman.HuffmanTree;
            depth = new Dictionary<char, int>();
            ComputeTreeSymbolsDepth(huffmantree);
        }
        //расчет энтропии Шеннона
        public double ShannonEntropy()
        {
            double entropy = 0;
            foreach (var probability in probabilit.Values)
                entropy += Shannon(probability);

            return -entropy;
        }
        //средняя длина сообщения
        public double AverageCodingMessageLength()
        {
            double sum = 0;
            foreach (var symbol in Alphabet)
                sum += depth[symbol] * probabilit[symbol];

            return sum;
        }
        //вероятности
        private Dictionary<char, double> ToProbabilities(Dictionary<char, int> frequencies)
        {
            var probabilities = new Dictionary<char, double>();
            foreach (var pair in frequencies)
                probabilities[pair.Key] = (double)pair.Value / tex.Length;

            return probabilities;
        }

        private double Shannon(double probability)
            => probability * Math.Log(probability, 2);
        //Глубина символов дерева вычислений
        private void ComputeTreeSymbolsDepth(Node currentNode, int depthr = 0)
        {
            if (currentNode != null)
            {
                if (currentNode.IsLeaf)
                {
                    depth[currentNode.Symbol] = depthr;
                    return;
                }

                ComputeTreeSymbolsDepth(currentNode.Left, depthr + 1);
                ComputeTreeSymbolsDepth(currentNode.Right, depthr + 1);
            }
        }
    }
}
