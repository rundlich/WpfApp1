using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для FanoHuffman.xaml
    /// </summary>
    public partial class FanoHuffman : Window
    {
        public FanoHuffman()
        {
            InitializeComponent();
        }


        List<double> probabilities = new List<double>();

        public void AddButton(object sender, RoutedEventArgs e)
        {
            ShannonFanoOutput.Text = "";
            HuffmanOutput.Text = "";
            double input = Convert.ToDouble(Input.Text);
            try
            {
                if (probabilities.Sum() + input > 1) {
                    MessageBox.Show("Ошибка: сумма всех вероятностей не должна превышать 1");
                }
                else
                {
                    probabilities.Add(Convert.ToDouble(input));
                }  
            }
            catch (Exception) {
                MessageBox.Show("Ошибка: введите число");
            };
            char[] Alphabet = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };


            List<char> symbols = new List<char>();
            for (int i = 0; i < probabilities.Count; i++)
            {
                symbols.Add(Alphabet[i]);
            }




            ShannonFano shannonFano = new ShannonFano();
            Huffman huffman = new Huffman();

            List<ShannonFanoNode> encodedNodes1 = shannonFano.EncodeFano(symbols, probabilities);
            List<HuffmanNode> encodedNodes2 = huffman.EncodeHuffman(symbols, probabilities);
          
            foreach (var i in encodedNodes1)
            {
                ShannonFanoOutput.Text += ($"{i.Symbol}\t{i.Probability}\t{i.Code}\n");
            }
            


            foreach (var i in encodedNodes2)
            {
                HuffmanOutput.Text += ($"{i.Symbol}\t{i.Frequency}\t{i.Probability}\n");
            }
           


        }















        public class ShannonFanoNode
        {
            public char Symbol { get; set; }
            public double Probability { get; set; }
            public string Code { get; set; }
        }

    public class ShannonFano
    {
        public List<ShannonFanoNode> EncodeFano(List<char> symbols, List<double> probabilities)
        {
            List<ShannonFanoNode> nodes = new List<ShannonFanoNode>();

                // Создаем узлы для каждого символа с его вероятностью
                for (int i = 0; i < symbols.Count; i++)
            {
                ShannonFanoNode node = new ShannonFanoNode
                {
                    Symbol = symbols[i],
                    Probability = probabilities[i]
                };
                nodes.Add(node);
            }

            // Сортировка узлов по символу
            nodes.Sort((x, y) => y.Probability.CompareTo(x.Probability));

            // Рекурсивный вызов функции разделения
            Split(nodes, 0, nodes.Count - 1);


            return nodes;
        }

        private void Split(List<ShannonFanoNode> nodes, int start, int end)
        {
            if (start >= end)
                return;

            int mid = GetSplitIndex(nodes, start, end);

            // Присваиваем коды "0" или "1" в зависимости от положения узла в отношении деления
            for (int i = start; i <= end; i++)
            {
                if (i <= mid)
                    nodes[i].Code += "0";
                else
                    nodes[i].Code += "1";
            }

            // Рекурсивно вызываем функцию для левой и правой части
            Split(nodes, start, mid);
            Split(nodes, mid + 1, end);
        }

        private int GetSplitIndex(List<ShannonFanoNode> nodes, int start, int end)
        {
            double sum1 = 0, sum2 = 0;
            double allsum = 0;
            int index1 = -1, index2 = -1;
            for (int i = start; i <= end; i++)
            {
                allsum += nodes[i].Probability;
            }

            for (int i = start; i <= end; i++)
            {
                if (sum1 <= 0.5 * allsum)
                    sum1 += nodes[i].Probability;
                else break;

                index1 = i;
            }

            for (int i = end; i >= start; i--)
            {
                if (sum2 <= 0.5 * allsum)
                    sum2 += nodes[i].Probability;
                else break;

                index2 = i - 1;
            }

            double p1 = Math.Abs(0.5 * allsum - sum1);
            double p2 = Math.Abs(0.5 * allsum - sum2);

            if (p1 < p2)
                return index1;
            else if (p2 < p1)
                return index2;
            else return index1;
        }
    }





    class HuffmanNode
    {
        public char Symbol { get; set; }
        public double Frequency { get; set; }
        public HuffmanNode Left { get; set; }
        public HuffmanNode Right { get; set; }
        public string Probability { get; set; }
    }





    class Huffman
    {
        public List<HuffmanNode> EncodeHuffman(List<char> symbols, List<double> probabilities)
        {
            Dictionary<char, double> symbolFrequencies = new Dictionary<char, double>();

            for (int i = 0; i < symbols.Count; i++)
            {
                symbolFrequencies.Add(symbols[i], probabilities[i]);
            }


            HuffmanNode root = BuildHuffmanTree(symbolFrequencies);
            Dictionary<char, string> codes = GenerateHuffmanCodes(root);

            List<HuffmanNode> result = new List<HuffmanNode>();

            foreach (var i in codes)
            {
                HuffmanNode node = new HuffmanNode();
                node.Symbol = i.Key;
                node.Probability = i.Value;
                result.Add(node);
            }
            result.Sort((x, y) => x.Symbol.CompareTo(y.Symbol));

            probabilities.Sort();
            probabilities.Reverse();
            int count = 0;
            foreach (var i in result)
            {
                i.Frequency = probabilities[count];
                count++;
            }
            return result;
        }









        static HuffmanNode BuildHuffmanTree(Dictionary<char, double> symbolFrequencies)
        {
            SortedList<double, HuffmanNode> nodes = new SortedList<double, HuffmanNode>();

            double epsilon = 0.0001;

            foreach (var symbolFrequency in symbolFrequencies)
            {
                double key = symbolFrequency.Value;
                while (nodes.ContainsKey(key))
                    key += epsilon;

                nodes.Add(key, new HuffmanNode
                {
                    Symbol = symbolFrequency.Key,
                    Frequency = symbolFrequency.Value
                });
            }

            while (nodes.Count > 1)
            {
                HuffmanNode left = nodes.Values[0];
                HuffmanNode right = nodes.Values[1];

                HuffmanNode parent = new HuffmanNode
                {
                    Symbol = '\0',
                    Frequency = left.Frequency + right.Frequency,
                    Left = left,
                    Right = right
                };

                nodes.RemoveAt(0);
                nodes.RemoveAt(0);

                double key = parent.Frequency;
                while (nodes.ContainsKey(key))
                    key += epsilon;

                nodes.Add(key, parent);
            }

            return nodes.Values[0];
        }

        static Dictionary<char, string> GenerateHuffmanCodes(HuffmanNode root)
        {
            Dictionary<char, string> codes = new Dictionary<char, string>();
            GenerateHuffmanCodesRecursive(root, "", codes);
            return codes;
        }

        static void GenerateHuffmanCodesRecursive(HuffmanNode node, string code, Dictionary<char, string> codes)
        {
            if (node.Left == null && node.Right == null) // Если узел является листом, то это символ
            {
                codes.Add(node.Symbol, code);
                return;
            }

            GenerateHuffmanCodesRecursive(node.Left, code + "0", codes);
            GenerateHuffmanCodesRecursive(node.Right, code + "1", codes);
        }
    }




}
}
