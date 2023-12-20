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
using static WpfApp1.CompressionAlgorithms;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для CompressionAlgorithms.xaml
    /// </summary>
    public partial class CompressionAlgorithms : Window
    {
        public CompressionAlgorithms()
        {
            InitializeComponent();
        }

        public void CompressButton(object sender, RoutedEventArgs e)
        {

            string input = Input.Text;
            LZ77field.Text = "";
            LZ78field.Text = "";
            LZSSfield.Text = "";

            List<output> compressedData1 = LZ77(input);
            foreach (var item in compressedData1)
            {
                LZ77field.Text += "<" + item.offset + "," + item.length + ",\'" + item.symbol + "\'>\n";
            }

            List<output> compressedData2 = LZ78(input);
            foreach (var item in compressedData2)
            {
                LZ78field.Text += "<" + item.offset + ",\'" + item.symbol + "\'>\n";
            }

            List<output> compressedData3 = LZSS(input);
            foreach (var item in compressedData3)
            {
                if (item.offset + item.length != 0)
                {
                    LZSSfield.Text += "1<" + item.offset + " " + item.length + ">\n";
                }
                else
                {
                    LZSSfield.Text += "0'" + item.symbol + "\'\n";
                }

            }


        }



        public class output
        {
            public int offset;
            public int length;
            public string symbol;
        }

        static public List<output> LZ77(string input)
        {
            try
            {
                List<output> Output = new List<output>();
                string bufferer = null;
                int CoddingPosition = 1;


                bufferer = input.Substring(0, 1);
                output first = new output();

                first.symbol = input.Substring(0, 1);
                first.length = 0;
                first.offset = 0;

                Output.Add(first);
                while (CoddingPosition < input.Length)
                {

                    int l;

                    for (l = Math.Min(bufferer.Length, input.Length - CoddingPosition); l >= 1; l--)
                    {

                        string substring = input.Substring(CoddingPosition, l);
                        int index = bufferer.IndexOf(substring);

                        if (index != -1)
                        {
                            index = 8 - bufferer.Length + index;
                            output i = new output();
                            i.offset = index;
                            i.length = l;

                            if (CoddingPosition + l > input.Length - 1)
                            {

                                if (bufferer.Length >= 8)
                                {
                                    bufferer = bufferer.Remove(0, substring.Length);
                                }
                                i.offset = 0;
                                i.length = 0;
                                i.symbol = input.Substring(CoddingPosition, 1);
                                bufferer = bufferer + i.symbol;
                                Output.Add(i);
                                CoddingPosition = CoddingPosition + l + 1;
                                break;
                            }

                            else
                                i.symbol = input.Substring(CoddingPosition + l, 1);
                            if (bufferer.Length + substring.Length >= 8)
                            {
                                if (bufferer.Length == 8)
                                {
                                    bufferer = bufferer.Remove(0, i.symbol.Length + substring.Length);
                                }
                                else
                                {

                                    bufferer = bufferer.Remove(0, (i.symbol.Length + substring.Length) - bufferer.Length);


                                }
                            }
                            CoddingPosition = CoddingPosition + l + 1;
                            bufferer = bufferer + substring + i.symbol;
                            Output.Add(i);
                            break;

                        }
                    }
                    if (l == 0)
                    {
                        output i = new output();
                        i.symbol = input.Substring(CoddingPosition, 1);
                        i.length = 0;
                        i.offset = 0;
                        if (bufferer.Length >= 8)
                        {
                            bufferer = bufferer.Remove(0, 1);
                        }
                        Output.Add(i);
                        bufferer = bufferer + input.Substring(CoddingPosition, 1);
                        CoddingPosition = CoddingPosition + 1;
                    }




                }
                return Output;
            }
            catch
            {
                List<output> Output = new List<output>();
                output i = new output();
                i.symbol = "";
                i.length = 0;
                i.offset = 0;
                return Output;
            }
        }


        static public List<output> LZ78(string input)
        {

            try
            {
            string bufferer = null;
            List<output> output = new List<output>();
            Dictionary<string, int> dictinory = new Dictionary<string, int>();
            int CoddingPosition = 2;
            bufferer = bufferer + input.Substring(0, 1);
            dictinory.Add(input.Substring(0, 1), 1);
            output one = new output();
            one.symbol = input.Substring(0, 1);
            one.offset = 0;
            output.Add(one);
            while (CoddingPosition < input.Length + 1)
            {
                int l;
                for (l = Math.Min(bufferer.Length, input.Length - (CoddingPosition - 1)); l >= 1; l--)
                {
                    string i = input.Substring(CoddingPosition - 1, l);
                    if (dictinory.ContainsKey(i))
                    {
                        if (CoddingPosition > input.Length - 1)
                        {
                            output chare = new output();
                            int b = dictinory[i];
                            chare.offset = b;
                            output.Add(chare);
                            dictinory.Add(" ", l + 1);
                            CoddingPosition = CoddingPosition + i.Length;
                            break;
                        }
                        else
                        {
                            int b = dictinory[i];
                            output chare = new output();
                            chare.symbol = input.Substring(CoddingPosition - 1 + l, 1);
                            chare.offset = b;
                            output.Add(chare);
                            dictinory.Add(i + input.Substring(CoddingPosition - 1 + l, 1), CoddingPosition - 1);
                            bufferer = bufferer + i + input.Substring(CoddingPosition - 1 + l, 1);
                            CoddingPosition = CoddingPosition + i.Length + 1;
                            break;
                        }


                    }
                }
                if (l == 0)
                {
                    output chare = new output();
                    dictinory.Add(input.Substring(CoddingPosition - 1, 1), CoddingPosition);
                    chare.symbol = input.Substring(CoddingPosition - 1, 1);
                    chare.offset = 0;
                    output.Add(chare);
                    bufferer = bufferer + input.Substring(CoddingPosition - 1, 1);
                    CoddingPosition = CoddingPosition + 1;

                }

            }
            return output;
        }
             catch
            {
                List<output> Output = new List<output>();
                output i = new output();
                i.symbol = "";
                i.length = 0;
                i.offset = 0;
                return Output;
            }
        }

        static public List<output> LZSS(string input)
        {

            List<output> Output = new List<output>();
            string buffer = null;
            int CoddingPosition = 1;


            buffer = buffer + input.Substring(0, 1);
            output first = new output();

            first.symbol = input.Substring(0, 1);
            first.length = 0;
            first.offset = 0;

            Output.Add(first);
            while (CoddingPosition < input.Length)
            {

                int l;

                for (l = Math.Min(buffer.Length, input.Length - CoddingPosition); l >= 1; l--)
                {

                    string substring = input.Substring(CoddingPosition, l);
                    int index = buffer.IndexOf(substring);
                    if (index != -1)
                    {
                        index = 8 - buffer.Length + index;
                        output i = new output();
                        i.offset = index;
                        i.length = l;
                        if (CoddingPosition + l > input.Length - 1)
                        {
                            if (buffer.Length + substring.Length >= 8)
                            {
                                buffer = buffer.Remove(0, substring.Length);
                            }

                            buffer = buffer + i.symbol;
                            Output.Add(i);
                            CoddingPosition = CoddingPosition + l;
                            break;
                        }
                        else
                            i.symbol = input.Substring(CoddingPosition + l, 1);
                        if (buffer.Length + substring.Length >= 8)
                        {
                            buffer = buffer.Remove(0, substring.Length);
                        }
                        CoddingPosition = CoddingPosition + l;
                        buffer = buffer + substring;
                        Output.Add(i);
                        break;

                    }
                }
                if (l == 0)
                {
                    output i = new output();
                    i.symbol = input.Substring(CoddingPosition, 1);
                    i.length = 0;
                    i.offset = 0;
                    if (buffer.Length >= 8)
                    {
                        buffer = buffer.Remove(0, 1);
                    }
                    Output.Add(i);
                    buffer = buffer + input.Substring(CoddingPosition, 1);
                    CoddingPosition = CoddingPosition + 1;
                }


            }
            return Output;
        }

    }



}

