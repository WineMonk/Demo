using SkiaSharp;
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
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ForceGuidanceDiagram
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int a = 123;
        public MainWindow()
        {
            InitializeComponent();
        }


        Random random = new Random();
        int radius = 15;

        List<ShapeCircle> ListRoundedCircle = null;
        List<ShapeRelationshipLine> ListShapeRelationshipLine = null;
        int GraphWidth = 200;
        int GraphHeight = 200;
        //CollisionGenerator pCollisionGenerator = null;
        private void generateButton_Click(object sender, RoutedEventArgs e)
        {
            int NodeCount = Convert.ToInt32(nodeNumTextBox.Text);
            int RelCount = Convert.ToInt32(relationNumTextBox.Text);

            this.ucGraphCanvas.Width = (NodeCount * 6) * 4;
            this.ucGraphCanvas.Height = (NodeCount * 6) * 3;

            GraphWidth = Convert.ToInt32(this.ucGraphCanvas.Width) - radius * 2;
            GraphHeight = Convert.ToInt32(this.ucGraphCanvas.Height) - radius * 2;

            bool IsShowRelNode = onlyDisplayIterationNodeCheckBox.IsChecked == true;

            ListRoundedCircle = new List<ShapeCircle>();

            for (int i = 0; i < NodeCount; i++)
            {
                int startX = random.Next(radius, GraphWidth);
                int startY = random.Next(radius, GraphHeight);
                ShapeCircle roundedCircle = new ShapeCircle()
                {
                    DisplayName = string.Format("测试{0}", i),
                    CenterX = startX,
                    CenterY = startY,
                    Width = 2 * radius,
                    Height = 2 * radius
                };
                ListRoundedCircle.Add(roundedCircle);
            }

            Dictionary<string, Tuple<int, int>> dicHas = new Dictionary<string, Tuple<int, int>>();

            for (int i = 0; i < RelCount; i++)
            {
                int StartNodeIndex = random.Next(0, ListRoundedCircle.Count);
                int EndNodeIndex = random.Next(0, ListRoundedCircle.Count);

                if (StartNodeIndex == EndNodeIndex)
                {
                    continue;
                }
                if (StartNodeIndex > EndNodeIndex)
                {
                    int TempIndex = EndNodeIndex;
                    EndNodeIndex = StartNodeIndex;
                    StartNodeIndex = TempIndex;
                }
                string strKey = $"{StartNodeIndex},{EndNodeIndex}";
                if (dicHas.ContainsKey(strKey))
                {
                    continue;
                }
                else
                {
                    dicHas.Add(strKey, new Tuple<int, int>(StartNodeIndex, EndNodeIndex));
                }
            }

            ListShapeRelationshipLine = new List<ShapeRelationshipLine>();
            foreach (var item in dicHas)
            {
                int StartNodeIndex = item.Value.Item1;
                int EndNodeIndex = item.Value.Item2;
                ShapeCircle StartShapeCircle = ListRoundedCircle[StartNodeIndex];
                ShapeCircle EndShapeCircle = ListRoundedCircle[EndNodeIndex];
                ShapeRelationshipLine pShapeRelationshipLine = new ShapeRelationshipLine()
                {
                    StartShapeCircle = StartShapeCircle,
                    EndShapeCircle = EndShapeCircle
                };
                ListShapeRelationshipLine.Add(pShapeRelationshipLine);
            }

            if (IsShowRelNode)
            {
                List<ShapeCircle> listRemoveNode = new List<ShapeCircle>();
                listRemoveNode.AddRange(ListRoundedCircle);
                foreach (var item in dicHas)
                {
                    int StartNodeIndex = item.Value.Item1;
                    int EndNodeIndex = item.Value.Item2;
                    ShapeCircle StartShapeCircle = ListRoundedCircle[StartNodeIndex];
                    if (listRemoveNode.Contains(StartShapeCircle))
                    {
                        listRemoveNode.Remove(StartShapeCircle);
                    }
                    ShapeCircle EndShapeCircle = ListRoundedCircle[EndNodeIndex];
                    if (listRemoveNode.Contains(EndShapeCircle))
                    {
                        listRemoveNode.Remove(EndShapeCircle);
                    }
                }
                foreach (var item in listRemoveNode)
                {
                    ListRoundedCircle.Remove(item);
                }
            }
            IninGraphRelNode(ucGraphCanvas, ListShapeRelationshipLine, ListRoundedCircle);
        }

        private void iterationButton_Click(object sender, RoutedEventArgs e)
        {
            int iterationNum = Convert.ToInt32(iterationNumTextBox.Text);
            for (int i = 0; i < iterationNum; i++)
            {
                GraphForceDirectedAlgorithm graphForceDirectedAlgorithm = new GraphForceDirectedAlgorithm(ListRoundedCircle, ListShapeRelationshipLine, GraphWidth, GraphHeight);
                graphForceDirectedAlgorithm.Collide();
            }
            IninGraphRelNode(ucGraphCanvas, ListShapeRelationshipLine, ListRoundedCircle);
        }

        private void clearButton_Click(object sender, RoutedEventArgs e)
        {
            ucGraphCanvas.Children.Clear();
        }

        void IninGraphRelNode(Canvas canves, List<ShapeRelationshipLine> ListShapeRelationshipLine, List<ShapeCircle> ListRoundedCircle)
        {
            canves.Children.Clear();
            foreach (var item in ListRoundedCircle)
            {
                Ellipse ellipse = new Ellipse();
                ellipse.Fill = new SolidColorBrush(Colors.Violet);
                ellipse.StrokeThickness = 2;
                ellipse.Stroke = new SolidColorBrush(Colors.Purple);
                ellipse.Width = radius * 2;
                ellipse.Height = radius * 2;
                //椭圆对象相对于父容器对象Canvas的位置，左边距、上边距
                Canvas.SetLeft(ellipse, item.CenterX);
                Canvas.SetTop(ellipse, item.CenterY);
                canves.Children.Add(ellipse);
            }

            foreach (var item in ListShapeRelationshipLine)
            {
                Line line = new Line();
                line.Stroke = Brushes.Black;
                line.StrokeThickness = 2.0;
                line.X1 = item.StartShapeCircle.CenterX;
                line.Y1= item.StartShapeCircle.CenterY;
                line.X2 = item.EndShapeCircle.CenterX;
                line.Y2 = item.EndShapeCircle.CenterY;
                Canvas.SetLeft(line, radius);
                Canvas.SetTop(line, radius);
                canves.Children.Add(line);
            }
        }

        private void disorganizeButton_Click(object sender, RoutedEventArgs e)
        {
            if (ListRoundedCircle == null || ListRoundedCircle.Count == 0)
                return;
            for (int i = 0; i < ListRoundedCircle.Count; i++)
            {
                int startX = random.Next(radius, GraphWidth);
                int startY = random.Next(radius, GraphHeight);
                ListRoundedCircle[i].CenterX= startX;
                ListRoundedCircle[i].CenterY= startY;
            }
            IninGraphRelNode(ucGraphCanvas, ListShapeRelationshipLine, ListRoundedCircle);
        }

        private void zoominButton_Click(object sender, RoutedEventArgs e)
        {
            ucGraphCanvas.Width = ucGraphCanvas.Width * 11 / 10;
            ucGraphCanvas.Height = ucGraphCanvas.Height * 11 / 10;
        }

        private void zoomoutButton_Click(object sender, RoutedEventArgs e)
        {
            ucGraphCanvas.Width = ucGraphCanvas.Width * 10 / 11;
            ucGraphCanvas.Height = ucGraphCanvas.Height * 10 / 11;
        }

        private void generateRegularButton_Click(object sender, RoutedEventArgs e)
        {
            int NodeCount = 25;

            this.ucGraphCanvas.Width = (NodeCount * 6) * 4;
            this.ucGraphCanvas.Height = (NodeCount * 6) * 3;

            GraphWidth = Convert.ToInt32(this.ucGraphCanvas.Width) - radius * 2;
            GraphHeight = Convert.ToInt32(this.ucGraphCanvas.Height) - radius * 2;

            bool IsShowRelNode = onlyDisplayIterationNodeCheckBox.IsChecked == true;

            ListRoundedCircle = new List<ShapeCircle>();

            for (int i = 0; i < NodeCount; i++)
            {
                int startX = random.Next(radius, GraphWidth);
                int startY = random.Next(radius, GraphHeight);
                ShapeCircle roundedCircle = new ShapeCircle()
                {
                    DisplayName = string.Format("测试{0}", i),
                    CenterX = startX,
                    CenterY = startY,
                    Width = 2 * radius,
                    Height = 2 * radius
                };
                ListRoundedCircle.Add(roundedCircle);
            }

            ListShapeRelationshipLine = new List<ShapeRelationshipLine>();
            for (int i = 1; i < 7; i++)
                ListShapeRelationshipLine.Add(new ShapeRelationshipLine() { StartShapeCircle = ListRoundedCircle[0], EndShapeCircle = ListRoundedCircle[i] });
            for (int i = 1; i < 7; i++)
                ListShapeRelationshipLine.Add(new ShapeRelationshipLine() { StartShapeCircle = ListRoundedCircle[i], EndShapeCircle = ListRoundedCircle[i + 6] });
            for (int i = 1; i < 7; i++)
                ListShapeRelationshipLine.Add(new ShapeRelationshipLine() { StartShapeCircle = ListRoundedCircle[i], EndShapeCircle = ListRoundedCircle[i + 12] });
            for (int i = 1; i < 7; i++)
                ListShapeRelationshipLine.Add(new ShapeRelationshipLine() { StartShapeCircle = ListRoundedCircle[i], EndShapeCircle = ListRoundedCircle[i + 12] });
            for (int i = 1; i < 7; i++)
                ListShapeRelationshipLine.Add(new ShapeRelationshipLine() { StartShapeCircle = ListRoundedCircle[i], EndShapeCircle = ListRoundedCircle[i + 18] });
            
            IninGraphRelNode(ucGraphCanvas, ListShapeRelationshipLine, ListRoundedCircle);
        }
    }
}
