using Microsoft.Xna.Framework;
using SharpDX.MediaFoundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Student
{
    public class Graph
    {
        private Node[,] nodes;
        private int size;
        
        public Graph(int size)
        {
            this.size = size;
            nodes = new Node[size, size];
            BuildGraph();
        }

        public void BuildGraph()
        {
            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    nodes[x, y] = new Node(x, y);
                }
            }

            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    AddNeighbors(x, y); 
                }
            }


        }

        private void AddNeighbors(int x, int y)
        {
            //kontrollera gränser
            if( y > 0)
            {
                nodes[x, y].neighbors.Add(nodes[x,y-1]); //ned
            }
            if(y < size - 1)
            {
                nodes[x, y].neighbors.Add(nodes[x, y + 1]); // upp
            }
            if(x > 0)
            {
                nodes[x, y].neighbors.Add(nodes[x-1, y]);//vänster
            }
            if (x < size - 1)
            {
                nodes[x, y].neighbors.Add(nodes[x+1,y]);//höger
            }
        }

        public void WallGraphUpdate(Drag drag)
        {
            if (drag.typ == Typ.Horisontell)
            {
                if (nodes[drag.point.X, drag.point.Y].neighbors.Contains(nodes[drag.point.X, drag.point.Y + 1]))
                {
                    nodes[drag.point.X, drag.point.Y].neighbors.Remove(nodes[drag.point.X, drag.point.Y + 1]);
                    nodes[drag.point.X, drag.point.Y + 1].neighbors.Remove(nodes[drag.point.X, drag.point.Y]);
                }
            }
            else if (drag.typ == Typ.Vertikal)
            {
                if (nodes[drag.point.X, drag.point.Y].neighbors.Contains(nodes[drag.point.X + 1, drag.point.Y]))
                {
                    nodes[drag.point.X, drag.point.Y].neighbors.Remove(nodes[drag.point.X + 1, drag.point.Y]);
                    nodes[drag.point.X + 1, drag.point.Y].neighbors.Remove(nodes[drag.point.X, drag.point.Y]);
                }
            }
        }


        public List<Node> BFS(Node start, Node end, SpelBräde bräde)
        {
            Queue<Node> queue = new Queue<Node>();
            Dictionary<Node, Node> predecessor = new Dictionary<Node, Node>();
            HashSet<Node> visited = new HashSet<Node>();

            queue.Enqueue(start);
            visited.Add(start);

            while (queue.Count > 0)
            {
                Node current = queue.Dequeue();

                if (current == end)
                {
                    return RebuildRoad(predecessor, start, end);
                }

                foreach (Node neighbor in current.neighbors)
                {
                    Point targetPosition = new Point(neighbor.X, neighbor.Y);

                    // Kontrollera om vi kan flytta till granne
                    if (!visited.Contains(neighbor) && CanMoveTo(new Point(current.X, current.Y), targetPosition, bräde))
                    {
                        queue.Enqueue(neighbor);
                        visited.Add(neighbor);
                        predecessor[neighbor] = current;
                    }
                }
            }

            return null; 
        }


        private List<Node> RebuildRoad(Dictionary<Node, Node> predecessor, Node start, Node end)
        {
            List<Node> road = new List<Node>();
            Node current = end;

            while(current != start)
            {
                road.Add(current);
                current = predecessor[current]; 
            }
            road.Add(start);
            road.Reverse(); //eftersom vägen byggs bakifrån

            return road;
        }

        public List<Node> FindGoalNodes(int playerIndex)
        {
            List<Node> goalNodes = new List<Node>();

            
            if (playerIndex == 0) 
            {
                // Lägg till noder i rad 0
                for (int x = 0; x < size; x++)
                {
                    goalNodes.Add(nodes[x, 8]);
                }
            }
            else if (playerIndex == 1) 
            {
                
                for (int x = 0; x < size; x++)
                {
                    goalNodes.Add(nodes[x, size - 1]);
                }
            }

            
            return goalNodes;
        }

        private bool CanMoveTo(Point currentPosition, Point targetPosition, SpelBräde bräde)
        {
            
            if (targetPosition.X < 0 || targetPosition.X >= SpelBräde.N || targetPosition.Y < 0 || targetPosition.Y >= SpelBräde.N)
            {
                return false; // Utanför gränser
            }

            // Kontrollera om det finns en vägg mellan nuvarande och målposition
            if (currentPosition.X == targetPosition.X)
            {
                
                if (currentPosition.Y < targetPosition.Y)
                {
                    // Flytta upp
                    return !bräde.horisontellaVäggar[currentPosition.X, currentPosition.Y];
                }
                else
                {
                    // Flytta ned
                    return !bräde.horisontellaVäggar[targetPosition.X, targetPosition.Y];
                }
            }
            else if (currentPosition.Y == targetPosition.Y)
            {
                
                if (currentPosition.X < targetPosition.X)
                {
                    // Flytta höger
                    return !bräde.vertikalaVäggar[currentPosition.X, currentPosition.Y]; 
                }
                else
                {
                    // Flytta vänster
                    return !bräde.vertikalaVäggar[targetPosition.X, currentPosition.Y]; 
                }
            }

            return false; // Ogiltig rörelse
        }





        public Node GetNode(int x, int y)
        {
            if (x >= 0 && x < size && y >= 0 && y < size)
            {
                return nodes[x, y];
            }
            return null; 
        }

    }

}
