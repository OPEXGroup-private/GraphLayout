using System;
using System.Collections.Generic;
using System.Globalization;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using GraphLayoutProject.Classes;
using Rectangle = System.Windows.Shapes.Rectangle;

namespace GraphLayoutProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {

            InitializeComponent();
        }
        private void Clean() => GraphCanvas.Children.Clear();

        private void Draw(RectangularGraph graph)
        {
            var textblock = new TextBlock {Text = graph.Content.Identificator.ToString()};
            Canvas.SetLeft(textblock, graph.Content.Left);
            Canvas.SetTop(textblock, graph.Content.Top);
            var rectangle = new System.Windows.Shapes.Rectangle
            {
                
                Width = graph.Content.Width,
                Height = graph.Content.Height,
                Stroke = Brushes.LightBlue,
                StrokeThickness = 2,
                Tag = graph.Content
            };
            Canvas.SetLeft(rectangle,graph.Content.Left);
            Canvas.SetTop(rectangle, graph.Content.Top);
            GraphCanvas.Children.Add(rectangle);
            GraphCanvas.Children.Add(textblock);
            if (graph.NextElements==null) return;
            foreach (var node in graph.NextElements)
            {
              Draw(node);
            }
        }

        private void DrawLines(RectangularGraph graph)
        {
            if (graph.NextElements == null) return;
            foreach (var node in graph.NextElements)
            {
    
                var line = new Line
                {
                    
                    Stroke = Brushes.LightSteelBlue,
                    StrokeThickness = 2,
                    X1 = graph.Content.Left + graph.Content.Width,
                    X2 = node.Content.Left,
                    Y1 = graph.Content.Top + graph.Content.Height/2,
                    Y2 = node.Content.Top + node.Content.Height / 2,
                };
                GraphCanvas.Children.Add(line);
                DrawLines(node);
            }
          


      
        }

        private void Button_OnClick(object sender, RoutedEventArgs e)
        {
            GraphCanvas.Children.Clear();
            var graph = Methods.GenerateGraphStructure(30,10);
            Methods.MakeGraphFalse(graph);
            Methods.ChangeNodesPosition(graph);
            Draw(graph);
            DrawLines(graph);


        }
    }
}
