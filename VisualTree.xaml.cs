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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ProjectManagementTree
{
    /// <summary>
    /// Interaction logic for VisualTreeOrganizer.xaml
    /// </summary>
    public partial class VisualTree : UserControl
    {
        public double NodeMargin
        {
            get { return (double)GetValue(NodeMarginProperty); }
            set { SetValue(NodeMarginProperty, value); }
        }
        public static readonly DependencyProperty NodeMarginProperty =
            DependencyProperty.Register("NodeMargin", typeof(double), typeof(VisualTree), new PropertyMetadata(10d));

        NodeControl head;

        public VisualTree()
        {
            InitializeComponent();
            head = new NodeControl(this);
        }

        public void LayoutTree()
        {
            lines.Children.Clear();
            head.OffsetChildren();
            ModifyChildTransform(head);
        }

        private void ModifyChildTransform(NodeControl ctrl)
        {
            foreach (NodeControl child in ctrl.GetChildren())
            {
                double xPos = ctrl.RenderTransform.Value.OffsetX + child.subTree.offset;
                double yPos = child.Depth * NodeMargin;
                Transform trans = ctrl.RenderTransform;
                TranslateTransform childTrans = new TranslateTransform(xPos, yPos);
                child.RenderTransform = childTrans;


                DrawLine(ctrl, child);
                ModifyChildTransform(child);
            }
        }

        private void DrawLine(NodeControl ctrl, NodeControl child)
        {
            Line line = new Line();
            lines.Children.Add(line);
            line.Stroke = Brushes.Black;
            line.StrokeThickness = 1;

            Transform ctrlTrans = ctrl.RenderTransform;
            Transform childTrans = child.RenderTransform;

            line.X1 = ctrlTrans.Value.OffsetX + ctrl.ActualWidth/2;
            line.Y1 = ctrlTrans.Value.OffsetY + ctrl.ActualHeight;
            line.X2 = childTrans.Value.OffsetX + child.ActualWidth/2;
            line.Y2 = childTrans.Value.OffsetY;

            Console.WriteLine("Drawing line from (" + line.X1 +", "+ line.Y1 + ") to (" + line.X2 +", "+ line.Y2 + ")");
        }

        #region DEBUG
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Log(head);
        }

        public static void Log(NodeControl node)
        {
            Console.WriteLine(CreateLog(node, ""));
        }

        private static string CreateLog(NodeControl node, string output)
        {
            if (node == null)
                return null;

            for (int i = 0; i < node.Depth; i++)
            {
                output += "|  ";
            }
            output += node + ": \n";
            foreach (var item in node.GetChildren())
            {
                output = CreateLog(item, output);
            }
            return output;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            LayoutTree();
        }
#endregion
    }

    public struct SubTree
    {
        public double width;
        public double offset;

        public bool Overlaps(SubTree other)
        {
            return ((offset - width / 2) < (other.offset + other.width / 2)) && ((offset + width / 2) > (other.offset - other.width / 2));
        }

        public static void Test()
        {
            SubTree x = new SubTree() { width = 2, offset = 0 };
            Console.WriteLine("Should evaluate to |True => False|");
            Console.WriteLine(x.Overlaps(new SubTree() {width = 2.0001, offset = 2}));
            Console.WriteLine(x.Overlaps(new SubTree() {width = 2, offset = 2.0001}));
        }
    }
}