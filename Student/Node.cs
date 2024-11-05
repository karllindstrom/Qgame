using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Student
{
    public class Node
    {
        public int X {  get; set; }
        public int Y { get; set; }

        public List<Node> neighbors { get; set; }

        public Node(int x, int y)
        {
            X = x;
            Y = y;

            neighbors = new List<Node>();
        }
    }
}
