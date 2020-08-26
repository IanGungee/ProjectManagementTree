using System;
using System.Collections.Generic;
using System.Windows.Media.Imaging;

namespace ProjectManagementTree
{
    public class Node : IComparable<Node>
    {
        public event Action ParentChanged;
        public string Name { get; set; }
        public int Priority { get; set; }
        public double Completion { get => _completion; }
        private double _completion;
        CheckList CheckList { get; }
        public Node Parent
        {
            get { return _parent; } set { _parent = value; ParentChanged?.Invoke(); }
        }
        private Node _parent;
        public List<Node> children = new List<Node>();
        public Node(string name = "New Node")
        {
            Name = name;
            CheckList = new CheckList(new List<CheckListTask>());
            CheckList.AddTask("This is just a test task");
        }
        ~Node()
        {
            Console.WriteLine("Node: " + Name + " deconstructed");
        }

        public void AddChild(Node node)
        {
            node.Parent = this;

            if(!children.Contains(node))
                children.Add(node);
        }

        public override string ToString()
        {
            string output = "--- " + Name + " Priority: " +Priority + " Completion: " +Completion + " ---\n";
            return output;
        }

        public int CompareTo(Node other)
        {
            if (other.Priority > Priority)
                return 1;
            else if (other.Priority < Priority)
                return -1;
            else return 0;
        }
    }
}