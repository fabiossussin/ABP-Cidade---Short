using System;
using System.Collections.Generic;
using System.Linq;

namespace AbpCidades
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("Bem Vindo!");
            var loop = true;

            var tree = new Tree();
            while (loop)
            {
                Console.WriteLine("");
                Console.WriteLine("Ações diponíveis!");
                Console.WriteLine("1) Adicionar Cidade");
                Console.WriteLine("2) Remover Cidade");
                Console.WriteLine("3) Consultar Cidade");
                Console.WriteLine("4) Listar Cidades Crescente");
                Console.WriteLine("5) Listar Cidades Decrescente");
                Console.WriteLine("6) Sair");
                Console.WriteLine("");

                var actionStr = Console.ReadLine();
                if (!int.TryParse(actionStr, out int result))
                    Console.WriteLine("Ação indiponível, digite um número de 1 a 6");
                else
                    switch (result)
                    {
                        case 1:
                            RegisterCity(tree);
                            break;
                        case 2:
                            RemoveCity(tree);
                            break;
                        case 3:
                            GetCity(tree);
                            break;
                        case 4:
                            DataList(tree, DataOrder.Default);
                            break;
                        case 5:
                            DataList(tree, DataOrder.Reverse);
                            break;
                        case 6:
                        default:
                            loop = false;
                            break;

                    }
            }

            Console.WriteLine("");
            Console.WriteLine("Volte Sempre!");
        }

        public static void RegisterCity(Tree tree)
        {
            Console.Write("Cidade: ");
            var cityName = Console.ReadLine();

            if (tree.Root == null)
                tree.Root = new Node(cityName);
            else
                tree.Insert(cityName);
        }

        public static void RemoveCity(Tree tree)
        {
            Console.Write("Nome da Cidade a ser removida: ");
            tree.DeleteCity(tree.Root, Console.ReadLine());
        }

        public static void GetCity(Tree tree)
        {
            Console.Write("Nome da Cidade: ");
            Console.WriteLine(tree.GetCityData(Console.ReadLine(), tree.Root));
        }

        public static void DataList(Tree tree, DataOrder order)
        {
            var result = tree.GetData(order);
            Console.WriteLine(string.IsNullOrEmpty(result) ? "Nenhuma cidade cadastrada" : result);
        }

        public static void PopulationList(Tree tree, DataOrder order)
        {
            var result = tree.GetPopulation(order);
            Console.WriteLine(string.IsNullOrEmpty(result) ? "Nenhuma cidade cadastrada" : result);
        }

        public static void CitiesList(Tree tree)
        {
            Console.Write("Estado das Cidades: ");
            var result = tree.GetCities(Console.ReadLine());
            Console.WriteLine(string.IsNullOrEmpty(result) ? "Nenhuma cidade cadastrada" : result);
        }

        public static void MostPopularCity(Tree tree)
        {
            var result = tree.MostPopularCity();
            Console.WriteLine(string.IsNullOrEmpty(result) ? "Nenhuma cidade cadastrada" : result);
        }

        public class Node
        {
            public Node(string value)
            {
                Value = value;
            }

            public string Value { get; set; }
            public Node Left { get; set; }
            public Node Right { get; set; }
        }

        public class Tree
        {
            public Node Root;
            public void Insert(string value) => Insert(null, value);

            public Tree() { }
            public Tree(string value) => Root = new Node(value);

            public Node DeleteCity(Node root, string key)
            {
                if (root == null)
                    return null;

                if (string.Compare(root.Value, key) < 0)
                    root.Left = DeleteCity(root.Left, key);
                else if (string.Compare(root.Value, key) > 0)
                    root.Right = DeleteCity(root.Right, key);
                else
                {
                    if (root.Left == null && root.Right == null)
                        return null;
                    else if (root.Left == null)
                        root = root.Right;
                    else if (root.Right == null)
                        root = root.Left;
                    else
                    {
                        Node min = MinValue(root.Right);
                        root.Value = min.Value;
                        root.Right = DeleteCity(root.Right, min.Value);
                    }
                }
                return root;
            }

            public string GetCityData(string element, Node root) =>
                root == null ? "Cidade não encontrada" : element == root.Value ?
                root.Value : GetCityData(element, string.Compare(root.Value, element) < 0 ? root.Left : root.Right);

            public string GetData(DataOrder order)
            {
                var result = new List<string>();
                result.AddRange(GetData(Root));
                return string.Join(',', order == DataOrder.Default ? result.OrderBy(x => x) : result.OrderByDescending(x => x));
            }

            public string GetPopulation(DataOrder order)
            {
                var cities = new List<string>();
                cities.AddRange(GetCity(Root));

                return string.Join(", ", order == DataOrder.Default ? cities.OrderBy(x => x).Select(x => x) : cities.OrderByDescending(x => x).Select(x => x));
            }

            public string GetCities(string data)
            {
                var cities = new List<string>();
                cities.AddRange(GetCity(Root));

                return string.Join(',', cities.Where(x => x == data).Select(x => x));
            }

            public string MostPopularCity()
            {
                var cities = new List<string>();
                cities.AddRange(GetCity(Root));

                return "Cidade mais populosa: " + cities.OrderByDescending(x => x).FirstOrDefault();
            }

            private List<string> GetData(Node node)
            {
                var result = new List<string>();
                if (node == null)
                    return result;

                var data = node;
                result.Add(data.Value);
                if (data.Left != null)
                    result.AddRange(GetData(data.Left));
                if (data.Right != null)
                    result.AddRange(GetData(data.Right));

                return result;
            }

            private List<string> GetCity(Node node)
            {
                var result = new List<string>();
                if (node == null)
                    return result;

                var data = node;
                result.Add(data.Value);
                if (data.Left != null)
                    result.AddRange(GetCity(data.Left));
                if (data.Right != null)
                    result.AddRange(GetCity(data.Right));

                return result;
            }

            private static Node MinValue(Node root)
            {
                var minv = root;
                while (root.Left != null)
                {
                    minv = root.Left;
                    root = root.Left;
                }
                return minv;
            }

            private void Insert(Node node, string value)
            {
                if (node == null)
                    node = Root;

                if (string.Compare(node.Value, value) < 0)
                {
                    if (node.Left == null)
                        node.Left = new Node(value);
                    else
                        Insert(node.Left, value);
                }
                else
                {
                    if (node.Right == null)
                        node.Right = new Node(value);
                    else
                        Insert(node.Right, value);
                }
            }

        }

        public enum DataOrder
        {
            Default,
            Reverse
        }
    }
}