using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;

namespace ProjectManagementTree
{
    public class CheckList
    {
        public List<CheckListTask> Tasks { get; } = new List<CheckListTask>();

        public CheckList(List<CheckListTask> tasks)
        {
            Tasks = tasks;
        }

        public float GetCompletion()
        {
            int completedTasks = 0;
            foreach (CheckListTask task in Tasks)
            {
                if (task.Complete)
                    completedTasks++;
            }
            return completedTasks / Tasks.Count;
        }

        public CheckList Clone()
        {
            return new CheckList(new List<CheckListTask>(Tasks));
        }

        public void AddTask(string description)
        {
            CheckListTask task = new CheckListTask();
            task.Description = description;
            Tasks.Add(task);
        }

        public override string ToString()
        {
            string output = "";
            foreach (CheckListTask task in Tasks)
            {
                output += task.ToString() + "\n";
            }
            return output;
        }
    }

    public class CheckListTask
    {
        public Action statusChanged;
        public string Description { get; set; }
        public bool Complete { get; set; }
        List<CheckListTask> subTasks;

        public override string ToString()
        {
            return (Complete? "1: " : "0: ") + Description;
        }
    }
}
