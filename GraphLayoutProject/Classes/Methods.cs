using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media.TextFormatting;

namespace GraphLayoutProject.Classes
{
    public static class Methods
    {
        public struct GraphStructure
        {
            public int Identificator;
            public bool Flag;
            public List<GraphStructure> Input;
            public List<GraphStructure> Output;
        }

        public static void UpdateInputs(GraphStructure node, GraphStructure inputNode)
        {
            foreach (var inputs in inputNode.Input)
            {
                if(!node.Input.Contains(inputs)) node.Input.Add(inputs);
            }
            if(node.Output==null) return;
            foreach (var output in node.Output)
            {
                UpdateInputs(output,node);
            }
        }

        public static void CreateGraphByStructure(GraphStructure structure, RectangularGraph graph)
        {
            structure.Flag = true;
            if (structure.Output != null)
            {
                foreach (var node in structure.Output)
                {
                    if (!node.Flag)
                    {
                        RectangularGraph temp = new RectangularGraph(30,30);
                        temp.NextElements = new List<RectangularGraph>();
                        temp.Content.Identificator = node.Identificator;
                        graph.NextElements.Add(temp);
                        CreateGraphByStructure(node,temp);
                    }
                }
            }
            else
            {
                graph.NextElements = null;
            }
        }
        public static RectangularGraph GenerateGraphStructure(int number, int maxLines)
        {
            var nodes = new GraphStructure[number];
            for (int i = 0; i < number; i++)
            {
                nodes[i].Identificator = i;
                nodes[i].Input=new List<GraphStructure>();
                nodes[i].Output=new List<GraphStructure>();
            }
            var random = new Random();
            for (int i = 1; i < number; i++)
            {
           
                var count = random.Next(maxLines);
                for (int j = 0; j < count; j++)
                {
                    bool error;
                    int maxiterations = 0;
                    do
                    {
                        var value = random.Next(1,number);
                        error = (value == i) || (nodes[i].Input.Contains(nodes[value])) || (nodes[i].Output.Contains(nodes[value]));
                        if (!error)
                        {
                            nodes[i].Output.Add(nodes[value]);
                            nodes[value].Input.Add(nodes[i]);
                            UpdateInputs(nodes[value],nodes[i]);
                        }
                        maxiterations++;
                    } while (error && maxiterations<20);
                }
            }
            for(int i=1;i<number;i++)
                if(nodes[i].Input.Count==0) nodes[0].Output.Add(nodes[i]);
            MakeGraphStructureFalse(nodes[0]);
            var result = new RectangularGraph[number];
            for (int i = 0; i < number; i++) result[i] = new RectangularGraph(30,30);

            for (int i = 0; i < number; i++)
            {
                result[i].NextElements = new List<RectangularGraph>();
                result[i].Content.Identificator = i;
                if(nodes[i].Output!=null)
                foreach (var node in nodes[i].Output)
                {
                    result[i].NextElements.Add(result[node.Identificator]);
                }
            }
           
            return result[0];
        }
        public static void MakeGraphStructureFalse(GraphStructure graph)
        {
            graph.Flag = false;
            if (graph.Output == null) return;
            foreach (var nextElement in graph.Output)
            {
                MakeGraphStructureFalse(nextElement);
            }
        }
        public static void MakeGraphFalse(RectangularGraph graph)
        {
            graph.Flag = false;
            if (graph.NextElements == null) return;
            foreach (var nextElement in graph.NextElements)
            {
                MakeGraphFalse(nextElement);
            }
        }

        public static int NumberOfNodes(RectangularGraph graph)
        {
            int result = 1;
            graph.Flag = true;
            if (graph.NextElements == null) return result;
            foreach (var nextElement in graph.NextElements)
            {
                if (nextElement.Flag == false)
                {
                    nextElement.Flag = true;
                    result += NumberOfNodes(nextElement);
                }
            }
            return result;
        }
        

        public static RectangularGraph CreateGraph(Matr<bool> matrix, Rectangle[] rectangles)
        {
            RectangularGraph[] temp = new RectangularGraph[matrix.Columns];
            for (int i = 0; i < matrix.Lines; i++)
            {
                temp[i] = new RectangularGraph(rectangles[i].Width,rectangles[i].Height);
                temp[i].Content.Left = 150+40*(i%3);
                temp[i].Content.Top =150+ 40 * (i/3);
                temp[i].Content.Identificator = i;
            }
            for (int i = 0; i < matrix.Lines; i++)
            {
                temp[i].NextElements = new List<RectangularGraph>();
                for (int j = 0; j < matrix.Lines; j++)
                {
                    if(matrix[i,j]) temp[i].NextElements.Add(temp[j]);
                }
            }
            return temp[0];
        }

        public static Rectangle[] GetRectangles(RectangularGraph graph)
        {
            List<Rectangle> temp = new List<Rectangle>();
            temp.Add(graph.Content);
            graph.Flag = true;
            if (graph.NextElements == null)
            {
                temp.Sort(Rectangle.CompareByIdentificator);
                return temp.ToArray();
            }
            foreach (var nextElement in graph.NextElements)
            {
                if (nextElement.Flag == false)
                {
                    nextElement.Flag = true;
                    temp.AddRange(GetRectangles(nextElement));
                }
            }
            temp.Sort(Rectangle.CompareByIdentificator);
            return temp.ToArray();
        }

        public static void GetNetworkMatrix(RectangularGraph graph, Matr<bool> matrix)
        {
            if (graph.NextElements == null) return;
            foreach (var node in graph.NextElements)
            {
                matrix[graph.Content.Identificator, node.Content.Identificator] = true;
                if (node.Flag == false)
                {
                    node.Flag = true;
                    GetNetworkMatrix(node,matrix);
                }
           
            }

        }

        public static int MaxPathNode(bool[] flags, int[] paths)
        {
            bool Error = true;
            int k = 0;
            for (int i = 0; i < flags.Length; i++)
            {
                if (flags[i] == false)
                {
                    k = i;
                    Error = false;
                    break;
                }
            }
            if (Error) return -1;
            for(int i=k;i<flags.Length;i++) 
                if(!flags[i])
                    if (paths[i] > paths[k]) k = i;
            return k;
        }
        public static int MinPathNode(bool[] flags, int[] paths)
        {
            bool Error = true;
            int k = 0;
            for (int i = 0; i < flags.Length; i++)
            {
                if (flags[i] == false)
                {
                    k = i;
                    Error = false;
                    break;
                }
            }
            if (Error) return -1;
            for (int i = k; i < flags.Length; i++)
                if (!flags[i])
                    if (paths[i] < paths[k]) k = i;
            return k;
        }
        public static bool[] Sinks(Matr<bool> networkMatr)
        {
            bool[] result = new bool[networkMatr.Lines];
            for (int i = 0; i < networkMatr.Lines;i++)
            {
                result[i] = true;
                for (int j=0;j<networkMatr.Lines;j++)
                    if (networkMatr[i,j])
                    {
                        result[i] = false;
                        break;
                    }
            }
            return result;
        }

        public static void Mask(Matr<bool> networkMatr, int node, bool[] mask)
        {
            mask[node] = true;
            for(int i=0;i<networkMatr.Lines;i++) 
                if(networkMatr[node,i]) Mask(networkMatr,i,mask);
        }
        public static int[] InvDijkstra(Matr<bool> networkMatr, int node)
        {
            int[] paths = new int[networkMatr.Lines];
            bool[] mask = new bool[networkMatr.Lines];
            bool[] flags = new bool[paths.Length];
            for (int i = 0; i < paths.Length; i++)
            {
                flags[i] = false;
                paths[i] = -1;
                mask[i] = false;
            }
            paths[node] = 0;
            Mask(networkMatr,node,mask);
            for(int i=0;i<paths.Length;i++)
                if (!mask[i]) flags[i] = true;
            int nodeIterator;
            do
            {
                nodeIterator = MaxPathNode(flags, paths);
                if (nodeIterator != -1)
                {
                    flags[nodeIterator] = true;
                    for (int i = 0; i < paths.Length; i++)
                            if (networkMatr[nodeIterator, i])
                            {
                                if (paths[i] < paths[nodeIterator] + 1) paths[i] = paths[nodeIterator] + 1;
                            }
                }
            } while (nodeIterator != -1);
            return paths;

        }

        public static int[] Bellman(Matr<bool> networkMatr, int node)
        {
            int[] paths = new int[networkMatr.Lines];
            bool[] mask = new bool[networkMatr.Lines];
            bool[] flags = new bool[paths.Length];
            for (int i = 0; i < paths.Length; i++)
            {
                flags[i] = false;
                paths[i] = Int32.MaxValue;
                mask[i] = false;
            }
            paths[node] = 0;
            Mask(networkMatr, node, mask);
            for (int i = 0; i < paths.Length; i++)
                if (!mask[i]) flags[i] = true;
            int nodeIterator;
            do
            {
                nodeIterator = MinPathNode(flags, paths);
                if (nodeIterator != -1)
                {
                    flags[nodeIterator] = true;
                    for (int i = 0; i < paths.Length; i++)
                        if (networkMatr[nodeIterator, i])
                        {
                            if (paths[nodeIterator] < Int32.MaxValue)
                                if (paths[i] > paths[nodeIterator] - 1) paths[i] = paths[nodeIterator] - 1;
                        }
                }
            } while (nodeIterator != -1);
            for(int i=0;i<paths.Length;i++)
                if (paths[i] == Int32.MaxValue) paths[i] = -1;
                else paths[i] = paths[i]*(-1);
            return paths;
        }

        public static int[] SetLevels(Matr<bool> networkMatr)
        {
            int[] result = new int[networkMatr.Lines];
            var sinks = Sinks(networkMatr);
            for (int i = 0; i < result.Length; i++)
            {
                var minDist = Int32.MaxValue;
                var distanses = Bellman(networkMatr, i);
                for (int j=0;j<result.Length;j++) 
                    if(sinks[j])
                        if (minDist > distanses[j] && distanses[j]!=-1) minDist = distanses[j];
                result[i] = minDist;
            }
            return result;
        }

        public static int[] Barycenter(Matr<bool> networkMatr, int[] levels)
        {
            var result = new int[networkMatr.Lines];
            var barycenters = new double[networkMatr.Lines];
            int maxLevel = levels[0];
            bool[] flags = new bool[networkMatr.Lines];
            for (int i = 0; i < networkMatr.Lines; i++)
            {
                flags[i] = false;
                if (maxLevel < levels[i]) maxLevel = levels[i];
            }
            for (int i = 0; i <= maxLevel; i++)
            {
                int k = 0;
                if (i == 0)
                {
                    for (int j = 0; j < networkMatr.Lines; j++)
                    {
                        if (levels[j] == i) barycenters[j] = 1;
                    }
                }
                else
                {
                    for (int j = 0; j < networkMatr.Lines; j++)
                    {
                        if (levels[j] == i)
                        {
                            int sum = 0;
                            int count = 0;
                            for(int m=0;m<networkMatr.Lines;m++)
                                if (levels[m] == i - 1 && networkMatr[j, m])
                                {
                                    sum += result[m];
                                    count++;
                                }
                            barycenters[j] = ((double) sum)/count;
                        }
                    }
                }

                for (int j = 0; j < networkMatr.Lines; j++)
                {
                    if (levels[j] == i)
                    {
                        result[j] = k;
                        k++;
                    }
                }

                for (int j1 = 0; j1 < networkMatr.Lines; j1++)
                {
                    if (levels[j1] == i)
                    {
                        for (int j2 = 0; j2 < j1; j2++)
                        {
                            if (levels[j2] == i)
                            {
                                int tempMin, tempMax;
                                if (result[j1] > result[j2])
                                {
                                    tempMin = result[j2];
                                    tempMax = result[j1];
                                }
                                else
                                {
                                    tempMin = result[j1];
                                    tempMax = result[j2];
                                }
                                if (barycenters[j1] > barycenters[j2])
                                {
                                    result[j1] = tempMax;
                                    result[j2] = tempMin;
                                }
                                else
                                {
                                    result[j2] = tempMax;
                                    result[j1] = tempMin;
                                }
                            }
                        }
                    }
                }

            }
         
            
            return result;
        }

        public static int Maximum(int[] array)
        {
            var result = array[0];
            foreach (int t in array)
                if (result < t) result = t;
            return result;
        }


        public static void ChangeNodesPosition(RectangularGraph graph)
        {
            var numberNodes = NumberOfNodes(graph);
            MakeGraphFalse(graph);
            var matrix = new Matr<bool>(numberNodes,numberNodes);
            GetNetworkMatrix(graph,matrix);
            MakeGraphFalse(graph);
            var levels = SetLevels(matrix);
            var horizontalOrder = Barycenter(matrix, levels);
            var maxLevel = Maximum(levels);

            var maxHeights = new int[maxLevel + 1];
            var rectangles = GetRectangles(graph);
            for (int i = 0; i <= maxLevel; i++)
            {
                maxHeights[i] = 0;
                var maxWidth = 0;
                var count=0;
                for (int j = 0; j < numberNodes; j++)
                {
                    if (levels[j] == i)
                    {
                        count++;
                        if (maxHeights[i]< rectangles[j].Height) maxHeights[i] = rectangles[j].Height;
                        if (maxWidth < rectangles[j].Width) maxWidth = rectangles[j].Width;
                    }
                      
                }
                int horizontalShift = (RectangularGraph.FormWidth - count*maxWidth)/(count + 1);
                for (int j = 0; j < numberNodes; j++)
                {
                    if (levels[j] == i)
                    {
                        rectangles[j].Left = horizontalShift*(horizontalOrder[j] + 1) + horizontalOrder[j] * maxWidth;
                    }

                }
            }
            var maxHeight = Maximum(maxHeights);
            var verticalShift = (RectangularGraph.FormHeigth - (maxLevel+1)*maxHeight)/(maxLevel +2);
            for (int i = 0; i <= maxLevel; i++)
            {
                for (int j = 0; j < numberNodes; j++)
                {
                    if (levels[j] == i)
                    {
                        rectangles[j].Top = verticalShift * (i + 1) + i * maxHeight;
                    }

                }
            }
            graph = CreateGraph(matrix, rectangles);
        }

    }
}
