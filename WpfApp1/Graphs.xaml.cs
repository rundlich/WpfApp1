using System;
using System.Collections.Generic;
using System.Linq;
using System.Printing;
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
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>

    public partial class Graphs : Window
    {
        List<Edge> Graph = new List<Edge>();

        public Graphs()
        {
            InitializeComponent();
        }

        public void AddButton(object sender, RoutedEventArgs e)
        {

            Edge R = new Edge();
            R.begin = InputBegin.Text;
            R.end = InputEnd.Text;
            Graph.Add(R);
            EnteredGrapghs.Text += string.Format($" {R.begin}  : {R.end}\n");
            Dictionary<string, List<string>> DirectedAdjacencyDictionary = DirectedAdjacencyList(Graph);
            Dictionary<string, List<string>> AdjacencyDictionary = AdjacencyList(Graph);
            OutputDirectedAdjacencyList.Text = null;
            foreach (var Q in DirectedAdjacencyDictionary)
            {
                string Edge = "";
                foreach (var G in Q.Value)
                {
                    Edge += G.ToString();
                    Edge += ",";
                }
                OutputDirectedAdjacencyList.Text += string.Format($" {Q.Key} :  {(Edge)}\n");
            }

            OutputDirectedAdjacencyMatrix.Text = null;
            {
                string Edge = "";
                foreach (var G in DirectedAdjacencyDictionary.Keys)
                {
                    Edge += G.ToString();
                    Edge += "   ";
                }
                OutputDirectedAdjacencyMatrix.Text += string.Format(Edge);

                foreach (var G in DirectedAdjacencyDictionary)
                {
                    string row = G.Key;
                    foreach (var column in DirectedAdjacencyDictionary.Keys)
                    {
                        if (G.Key == column)
                        {
                            row = row + 0 + "   ";
                        }
                        else
                        {
                            if (G.Value.Contains(column))
                            {
                                row = row + 1 + "   ";
                            }
                            else
                            {
                                row = row + 0 + "   ";
                            }
                        }
                    }
                    OutputDirectedAdjacencyMatrix.Text += "\n" + string.Format(row);
                }


            }



            OutputDirectedIncidenceMatrix.Text = "";

            string GraphsList = " ";
            foreach (var G in Graph)
            {
                GraphsList += G.Name() + "  ";
            }
            OutputDirectedIncidenceMatrix.Text += GraphsList;
            foreach (var Vertex in DirectedAdjacencyDictionary.Keys)
            {
                string row = Vertex;
                foreach (var G in Graph)
                {
                    if (Vertex == G.begin)
                    {
                        row = row + 1 + "  ";
                    }
                    else if (Vertex == G.end)
                    {
                        row = row + -1 + "  ";
                    }
                    else if (Vertex == G.begin && Vertex == G.end)
                    {
                        row = row + 1 + "  ";
                    }
                    else
                        row = row + 0 + "  ";
                }
                OutputDirectedIncidenceMatrix.Text += "\n" + row;
            }





            OutputAdjacencyList.Text = null;
            foreach (var Q in AdjacencyDictionary)
            {
                string Edge = "";
                foreach (var G in Q.Value)
                {
                    Edge += G.ToString();
                    Edge += ",";
                }
                OutputAdjacencyList.Text += string.Format($" {Q.Key} :  {(Edge)}\n");
            }

            OutputAdjacencyMatrix.Text = null;
            {
                string Edge = "";
                foreach (var G in AdjacencyDictionary.Keys)
                {
                    Edge += G.ToString();
                    Edge += "   ";
                }
                OutputAdjacencyMatrix.Text += string.Format(Edge);

                foreach (var G in AdjacencyDictionary)
                {
                    string row = G.Key;
                    foreach (var column in AdjacencyDictionary.Keys)
                    {
                        if (G.Key == column)
                        {
                            row = row + 0 + "   ";
                        }
                        else
                        {
                            if (G.Value.Contains(column))
                            {
                                row = row + 1 + "   ";
                            }
                            else
                            {
                                row = row + 0 + "   ";
                            }
                        }
                    }
                    OutputAdjacencyMatrix.Text += "\n" + string.Format(row);
                }


            }






            OutputIncidenceMatrix.Text = "";
            GraphsList = " ";
            foreach (var G in Graph)
            {
                GraphsList += G.Name() + "  ";
            }
            OutputIncidenceMatrix.Text += GraphsList;
            foreach (var Vertex in AdjacencyDictionary.Keys)
            {
                string row = Vertex;
                foreach (var G in Graph)
                {
                    if (Vertex == R.begin || Vertex == R.end)
                    {
                        row = row + 1 + "  ";
                    }
                    else
                    {
                        row = row + 0 + "  ";
                    }
                }
                OutputIncidenceMatrix.Text += "\n" + row;
            }


        }





       

        static Dictionary<string, List<string>> DirectedAdjacencyList(List<Edge> Graph)// Направленный список смежности
        {
            Dictionary<string, List<string>> AdjacencyList = new Dictionary<string, List<string>>();

            foreach (var R in Graph)
            {

                if (!AdjacencyList.ContainsKey(R.begin))
                {
                    List<string> GraphsListR = new List<string>();
                    GraphsListR.Add(R.end);
                    AdjacencyList.Add(R.begin, GraphsListR);


                }
                else
                {
                    List<string> GraphsListR = AdjacencyList[R.begin];
                    GraphsListR.Add(R.end);

                }


            }
            foreach (var R in Graph)
            {

                if (!AdjacencyList.ContainsKey(R.end))
                {
                    List<string> GraphsListR = new List<string>();

                    AdjacencyList.Add(R.end, GraphsListR);
                }


            }
            return AdjacencyList;
        }

        static Dictionary<string, List<string>> AdjacencyList(List<Edge> Graph)// Ненаправленный список смежности
        {
            Dictionary<string, List<string>> AdjacencyList = new Dictionary<string, List<string>>();

            foreach (var R in Graph)
            {

                if (!AdjacencyList.ContainsKey(R.begin))
                {
                    List<string> GraphsListR = new List<string>();
                    GraphsListR.Add(R.end);
                    AdjacencyList.Add(R.begin, GraphsListR);


                }
                else
                {
                    List<string> GraphsListR = AdjacencyList[R.begin];
                    GraphsListR.Add(R.end);

                }
            }
            foreach (var R in Graph)
            {

                if (!AdjacencyList.ContainsKey(R.end))
                {
                    List<string> GraphsListR = new List<string>();
                    GraphsListR.Add(R.begin);
                    AdjacencyList.Add(R.end, GraphsListR);


                }
                else
                {
                    List<string> GraphsListR = AdjacencyList[R.end];
                    GraphsListR.Add(R.begin);

                }
            }
            return AdjacencyList;
        }
        
        static void IncidenceMatrix(Dictionary<string, List<string>> AdjacencyList, List<Edge> input)// Неориентированная матрица инцидентности
        {
            string GraphsList = " ";
            foreach (var R in input)
            {
                GraphsList = GraphsList + R.Name() + " ";
            }
            Console.WriteLine("Матрица инцидентности не ориентированный");
            Console.WriteLine(GraphsList);
            foreach (var Vertex in AdjacencyList.Keys)
            {
                string row = Vertex;
                foreach (var R in input)
                {
                    if (Vertex == R.begin || Vertex == R.end)
                    {
                        row = row + 1 + "  ";
                    }
                    else
                    {
                        row = row + 0 + "  ";
                    }
                }
                Console.WriteLine(row);
            }

        }
        static void DirectedIncidenceMatrix(Dictionary<string, List<string>> AdjacencyList, List<Edge> input)// Ориентированная матрица инцидентности
        {
            string GraphsList = " ";
            foreach (var R in input)
            {
                GraphsList = GraphsList + R.Name() + " ";
            }
            Console.WriteLine("Матрица инцидентности  ориентированная");
            Console.WriteLine(GraphsList);
            foreach (var Vertex in AdjacencyList.Keys)
            {
                string row = Vertex;
                foreach (var R in input)
                {
                    if (Vertex == R.begin)
                    {
                        row = row + 1 + "  ";
                    }
                    else if (Vertex == R.end)
                    {
                        row = row + -1 + "  ";
                    }
                    else if (Vertex == R.begin && Vertex == R.end)
                    {
                        row = row + 1 + "  ";
                    }
                    else
                        row = row + 0 + "  ";
                }
                Console.WriteLine(row);
            }

        }
       
        public struct Edge //Структура ребра
        {
            public string begin;

            public string end;


            public void PrintR()
            {
                //string OutputDirected = ($" {begin}  : {end}").ToString();
            }
            public string Name()
            {
                return begin + end;
            }
        }
    }
}


    
    