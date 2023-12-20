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
    /// Логика взаимодействия для BinarySearchTree.xaml
    /// </summary>
    public partial class BinarySearchTreeWindow : Window
    {
        BinarySearchTree bst = new BinarySearchTree();
        public BinarySearchTreeWindow()
        {
            InitializeComponent();
        }

        public void AddButton (object sender, RoutedEventArgs e)
        {
            try
            {
                int input = Convert.ToInt32(Input.Text);
                bst.Insert(input);
            }
            catch {
               MessageBox.Show("Ошибка: введите число");
            }
            DFS.Text = bst.TraverseDFS();
            BFS.Text = bst.TraverseBFS();
        }
        public void FindButton (object sender, RoutedEventArgs e)
        {
            int input = Convert.ToInt32(Input.Text);
            try
            {
                if (bst.Search(input) == true)
                {
                    MessageBox.Show("Число " + input + " найдено в дереве");
                }
                else
                {
                    MessageBox.Show("Число " + input + " не найдено в дереве");
                }
            }
            catch
            {
                MessageBox.Show("Ошибка: введите число");
            }
            DFS.Text = bst.TraverseDFS();
            BFS.Text = bst.TraverseBFS();
        }

        public void DelButton(object sender, RoutedEventArgs e)
        {
            try
            {
                int input = Convert.ToInt32(Input.Text);
                bst.Remove(input);
            }
            catch
            {
                MessageBox.Show("Ошибка: введите число");
            }
            DFS.Text = bst.TraverseDFS();
            BFS.Text = bst.TraverseBFS();
        }



        class BinarySearchTree
            {
                private class Node
                {
                    public int Data;
                    public Node Left;
                    public Node Right;
        
                    public Node(int data)
                    {
                        Data = data;
                        Left = null;
                        Right = null;
                    }
                }
        
                private Node root;
        
                public BinarySearchTree()
                {
                    root = null;
                }
        
                public void Insert(int data)
                {
                    root = Insert(root, data);
                }
        
                private Node Insert(Node root, int data)
                {
                    if (root == null)
                    {
                        root = new Node(data);
                    }
                    else if (data < root.Data)
                    {
                        root.Left = Insert(root.Left, data);
                    }
                    else if (data > root.Data)
                    {
                        root.Right = Insert(root.Right, data);
                    }
        
                    return root;
                }
        
                public void Remove(int data)
                {
                    root = Remove(root, data);
                }
        
                private Node Remove(Node root, int data)
                {
                    if (root == null)
                    {
                        return null;
                    }
                    else if (data < root.Data)
                    {
                        root.Left = Remove(root.Left, data);
                    }
                    else if (data > root.Data)
                    {
                        root.Right = Remove(root.Right, data);
                    }
                    else
                    {
                        if (root.Left == null && root.Right == null)
                        {
                            root = null;
                        }
                        else if (root.Left == null)
                        {
                            root = root.Right;
                        }
                        else if (root.Right == null)
                        {
                            root = root.Left;
                        }
                        else
                        {
                            Node minNode = FindMin(root.Right);
                            root.Data = minNode.Data;
                            root.Right = Remove(root.Right, minNode.Data);
                        }
                    }
        
                    return root;
                }
        
                private Node FindMin(Node node)
                {
                    while (node.Left != null)
                    {
                        node = node.Left;
                    }
        
                    return node;
                }
        
                public bool Search(int data)
                {
                    return Search(root, data);
                }
        
                private bool Search(Node root, int data)
                {
                    if (root == null)
                    {
                        return false;
                    }
                    else if (data == root.Data)
                    {
                        return true;
                    }
                    else if (data < root.Data)
                    {
                        return Search(root.Left, data);
                    }
                    else
                    {
                        return Search(root.Right, data);
                    }
                }
        
                public string TraverseBFS()
                {
                string output = "";
                    if (root == null)
                    {
                        return "";
                    }
        
                    Queue<Node> queue = new Queue<Node>();
                    queue.Enqueue(root);
        
                    while (queue.Count > 0)
                    {
                        Node node = queue.Dequeue();
                        output += (node.Data + " ").ToString();
        
                        if (node.Left != null)
                        {
                            queue.Enqueue(node.Left);
                        }
                        if (node.Right != null)
                        {
                            queue.Enqueue(node.Right);
                        }
                    }
                return output;
                }

            string output = "";
            public string TraverseDFS()
                {

                output = "";
                return TraverseDFS(root);
                }
                private string TraverseDFS(Node root)
                {
                    if (root == null)
                    {
                        return "";
                    }
        
                    output += (root.Data + " ").ToString();
                    TraverseDFS(root.Left);
                    TraverseDFS(root.Right);
                    return output;
                }
            }
        
           
               
            }
    }

        