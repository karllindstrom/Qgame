using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Student;
class Agent:BaseAgent {
    private Graph graph;

    [STAThread]
    static void Main() {
        Program.Start(new Agent());
    }
    public Agent()
    {
        graph = new Graph(9);
    }
    public override Drag SökNästaDrag(SpelBräde bräde) 
    {
        Spelare jag = bräde.spelare[0];
        Point playerPos = jag.position; //nuvarande position
        Drag drag = new Drag();

        //hämta målnoder
        List<Node> goalNodes = graph.FindGoalNodes(0);

        

        //hitta nästa drag mot mål
        Node startNode = graph.GetNode(playerPos.X, playerPos.Y);
        Node closestGoalNode = null;
        List<Node> path = null;

        //sök efter närmaste målnod
        foreach(var goalNode in goalNodes)
        {
            var currentPath = graph.BFS(startNode, goalNode, bräde);
            if (currentPath != null && (path == null || currentPath.Count < path.Count))
            {
                path = currentPath; //spara kortast väg
                closestGoalNode = goalNode; //spara närmaste målnod
            }
        }

        //om väg hittats till närmaste mål, bestäm nästa drag
        if (path != null && path.Count > 1)
        {
            Node nextNode = path[1];//nästa nod i vägen
            drag.typ = Typ.Flytta;
            drag.point = new Point(nextNode.X, nextNode.Y);//dragets punkt till nästa nod
        }
        else
        {
            //standard-drag om ingen väg finns
            if(jag.antalVäggar > 7)
            {
                drag.typ = Typ.Horisontell;
                drag.point = new Point(23 - jag.antalVäggar * 2, 2);
            }
            else
            {
                drag.typ = Typ.Flytta;
                drag.point = new Point(playerPos.X, playerPos.Y + 1); //standard nedåtflytt
            }
        }
        return drag;

        //Så ni kan kolla om systemet fungerar!
        //Spelare jag = bräde.spelare[0];
        //Point playerPos = jag.position;
        //Drag drag = new Drag();
        //if (jag.position.Y < 4) {
        //    drag.typ = Typ.Flytta;
        //    drag.point = playerPos;
        //    drag.point.Y++;
        //} else if (jag.antalVäggar > 7) {
        //    drag.typ = Typ.Horisontell;
        //    drag.point = new Point(23-jag.antalVäggar*2, 2);
        //} else {
        //    drag.typ = Typ.Flytta;
        //    drag.point = playerPos;
        //    drag.point.Y++;
        //}
        //return drag;
    }
    public override Drag GörOmDrag(SpelBräde bräde, Drag drag) {
        //Om draget ni försökte göra var felaktigt så kommer ni hit
        System.Diagnostics.Debugger.Break();    //Brytpunkt
        return SökNästaDrag(bräde);
    }
}
//enum Typ { Flytta, Horisontell, Vertikal }
//struct Drag {
//    public Typ typ;
//    public Point point;
//}