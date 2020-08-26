using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace ProjectManagementTree
{
    public partial class NodeControl : UserControl
    {
        public static Dictionary<Node, NodeControl> nodeControls = new Dictionary<Node, NodeControl>();

        #region DEPENDENCY PROPERTIES
        public string NodeName
        {
            get { return (string)GetValue(NodeNameProperty); }
            set { SetValue(NodeNameProperty, value); }
        }
        public static readonly DependencyProperty NodeNameProperty =
            DependencyProperty.Register("NodeName", typeof(string), typeof(NodeControl));

        public double Completion
        {
            get { return (double)GetValue(CompletionProperty); }
            set { SetValue(CompletionProperty, value); }
        }
        public static readonly DependencyProperty CompletionProperty =
            DependencyProperty.Register("Completion", typeof(double), typeof(NodeControl), new PropertyMetadata(0d));

        public int Priority
        {
            get { return (int)GetValue(PriorityProperty); }
            set { SetValue(PriorityProperty, value); }
        }
        public static readonly DependencyProperty PriorityProperty =
            DependencyProperty.Register("Priority", typeof(int), typeof(NodeControl), new PropertyMetadata(0));
        #endregion

        public Node Node { get; }
        public new NodeControl Parent { get { if (Node.Parent == null) return null; else return nodeControls[Node.Parent]; } }
        public double X {get;set;}
        public double Y { get; set; }
        public bool Displayed { get; private set; }
        public int Depth { get; private set; }
        public Line connectionLine;
        public SubTree subTree;
        private VisualTree visTree;

        internal NodeControl(VisualTree tree)
        {
            Node = new Node();
            Node.ParentChanged += UpdateDepth;
            Binding nameBind = new Binding("Name");
            nameBind.Source = Node;
            nameBind.Mode = BindingMode.TwoWay;
            SetBinding(NodeNameProperty, nameBind);

            Binding priorBind = new Binding("Priority");
            priorBind.Source = Node;
            priorBind.Mode = BindingMode.TwoWay;
            SetBinding(PriorityProperty, priorBind);

            Binding compBind = new Binding("Completion");
            compBind.Source = Node;
            compBind.Mode = BindingMode.OneWay;
            SetBinding(CompletionProperty, compBind);




            Random rand = new Random();
            X = rand.NextDouble() * 100;
            Y = rand.NextDouble() * 100;
            NodeName = System.IO.Path.GetRandomFileName();
            Priority = (int)(rand.NextDouble() * 1000);
            Completion = (int)(rand.NextDouble() * 1000);




            nodeControls.Add(Node, this);
            InitializeComponent();
            visTree = tree;
            visTree.nodes.Children.Add(this);
        }

        private void UpdateDepth()
        {
            Depth = 0;
            Node trav = Node;
            while(trav.Parent != null)
            {
                Depth++;
                trav = trav.Parent;
            }
            NodeName = Depth + " " + Priority;
        }

        public NodeControl[] GetChildren()
        {
            NodeControl[] children = new NodeControl[Node.children.Count];
            for (int i = 0; i < Node.children.Count; i++)
            {
                children[i] = nodeControls[Node.children[i]];
            }
            return children;
        }

        public double GetSubTreeWidth()
        {
            if (Node.children.Count < 2)
                subTree.width = visTree.NodeMargin;
            else subTree.width = Node.children.Count * visTree.NodeMargin;

            return subTree.width;
        }

        public void OffsetChildren()
        {
            double width = 0;
            NodeControl[] children = GetChildren();
            for (int i = 0; i < children.Length; i++)
            {
                children[i].OffsetChildren();
                width += children[i].subTree.width;
            }
            if (width == 0)
                subTree.width = visTree.NodeMargin;
            else
                subTree.width = width;

            double total = 0;
            double middle = subTree.width / 2;
            for (int i = 0; i < children.Length; i++)
            {
                children[i].subTree.offset = total + children[i].subTree.width / 2 - middle;
                total += children[i].subTree.width;
            }
        }

        private void AddChild()
        {
            NodeControl ctrl = new NodeControl(visTree);
            Node.AddChild(ctrl.Node);
            visTree.LayoutTree();
        }

        private void AddParent()
        {
            if (Parent == null)
                return;

            Node.Parent.children.Remove(Node);
            NodeControl newParent = new NodeControl(visTree);
            Node.Parent.AddChild(newParent.Node);
            ChangeParent(newParent);
            visTree.LayoutTree();
        }

        private void DeleteNode()
        {
            if (Node.Parent == null)
                return;

            foreach (NodeControl child in GetChildren())
                child.ChangeParent(Parent, true);

            Node.Parent.children.Remove(Node);
            Node.children = null;
            visTree.nodes.Children.Remove(this);
            nodeControls.Remove(Node);

            visTree.LayoutTree();
        }

        private void ChangeParent(NodeControl newParent, bool removeThisNode = false)
        {
            if (removeThisNode)
            {
                foreach (NodeControl child in GetChildren())
                    child.ChangeParent(Parent);

                newParent.Node.AddChild(Node);
            }
            else
            {
                newParent.Node.AddChild(Node);

                foreach (NodeControl child in GetChildren())
                    child.ChangeParent(this);
            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            AddChild();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            DeleteNode();
        }

        public override string ToString()
        {
            return NodeName + " " + Priority + ": " + GetChildren().Length;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            AddParent();
        }
    }
}
