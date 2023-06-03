using System.Collections.Generic;

namespace RekursionTests.Lib
{
    public class Maze
    {
        public int MazeSize { get; set; }
        public List<Node> NodeList { get; set; }

        public Node? StartNode { get; set; }
        public Node? EndNode { get; set; }

        public Maze(int mazeSize)
        {
            MazeSize = mazeSize;
            NodeList = new();

            // fill Nodes with values
            for (int xAxis = 0; xAxis < mazeSize; xAxis++)
            {

                for (int yAxis = 0; yAxis < mazeSize; yAxis++)
                {
                    Node node = new Node();
                    node.XPos = xAxis;
                    node.YPos = yAxis;
                    NodeList.Add(node);

                    // walls around the _maze
                    if (xAxis == mazeSize - 1 || xAxis == 0 || yAxis == 0 || yAxis == mazeSize - 1 ||
                        // walls top left (middle)
                        xAxis == 2 && yAxis == 1 || xAxis == 2 && yAxis == 2 || xAxis == 2 && yAxis == 3 || xAxis == 3 && yAxis == 3 ||
                        // walls bottom
                        xAxis == 2 && yAxis == 5 || xAxis == 3 && yAxis == 5 || xAxis == 4 && yAxis == 5 || xAxis == 3 && yAxis == 6 ||
                        // walls right
                        xAxis == 6 && yAxis == 4 || xAxis == 6 && yAxis == 5 || xAxis == 5 && yAxis == 3 || xAxis == 5 && yAxis == 2 ||
                        xAxis == 6 && yAxis == 3 ||
                        
                        // walls right wall
                        xAxis == 12 && yAxis == 4 || xAxis == 11 && yAxis == 4 || xAxis == 10 && yAxis == 4 ||
                        xAxis == 12 && yAxis == 7 || xAxis == 11 && yAxis == 7 || xAxis == 10 && yAxis == 7 || xAxis == 9 && yAxis == 7 ||
                        xAxis == 12 && yAxis == 10 || xAxis == 11 && yAxis == 10 || xAxis == 10 && yAxis == 10 || xAxis == 9 && yAxis == 10 ||
                        xAxis == 8 && yAxis == 10 ||

                        // walls "+"
                        xAxis == 5 && yAxis == 10 || xAxis == 4 && yAxis == 10 || xAxis == 3 && yAxis == 10 || xAxis == 4 && yAxis == 11 ||
                        xAxis == 4 && yAxis == 9

                        )
                    {
                        node.IsWalkable = false;
                    }
                    else
                    {
                        node.IsWalkable = true;
                    }
                }
            }
        }
    }
}
