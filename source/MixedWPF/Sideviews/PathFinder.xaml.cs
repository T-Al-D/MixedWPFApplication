using RekursionTests.Lib;
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
using static System.Net.Mime.MediaTypeNames;

namespace MixedWPF.Sideviews
{
    /// <summary>
    /// Interaktionslogik für PathFinder.xaml
    /// </summary>
    public partial class PathFinder : Page
    {
        public PathFinder()
        {
            InitializeComponent();
            DrawMaze();
        }

        private Maze _maze = new(14);
        private bool _isAtEndGoal = false;
        private List<Node> _nodeList = new();
        private List<Node> _nodesToBeVisited { get; set; } = new List<Node>();

        List<Node> AllWalkableNotVisitedNeighbours(Node currentNode)
        {

            int xPosition = currentNode.XPos;
            int yPosition = currentNode.YPos;

            // gets all the neighbours from the maze, that fit the criteria
            List<Node> neighbours = _maze.NodeList.Where(node =>
            (node.YPos == yPosition - 1 && node.XPos == xPosition || node.YPos == yPosition + 1 && node.XPos == xPosition ||
             node.YPos == yPosition && node.XPos == xPosition + 1 || node.YPos == yPosition && node.XPos == xPosition - 1)
            && (!node.IsVisited && node.IsWalkable)).ToList();

            return neighbours;
        }

        // Widthsearch
        void FindWayBreadthSearch(List<Node> nodesBreadthSearch)
        {
            List<Node> neighboursForAllNodes = new List<Node>();

            // get the next IsWalkable neighours Nodes 
            foreach (Node node in nodesBreadthSearch)
            {
                // each node counts as IsVisited
                node.IsVisited = true;

                // get the neighbours for each node
                List<Node> neighboursForOneNode = AllWalkableNotVisitedNeighbours(node);

                // add the neighbours of each IsVisited node into the "big" List
                foreach (Node neighbour in neighboursForOneNode)
                {
                    if (!neighboursForAllNodes.Contains(neighbour))
                    {
                        neighboursForAllNodes.Add(neighbour);
                    }
                };

                // remove the neighbour that alrady has been IsVisited !
                neighboursForAllNodes.Remove(node);
            }

            if (neighboursForAllNodes.Count != 0)
            {
                Node? endNode = neighboursForAllNodes.FirstOrDefault(node => node.IsEnd);
                // if one of those neighbours is the End Node -> goal
                if (endNode != null)
                {
                    _isAtEndGoal = true;
                    DrawMaze();
                }
                else if (!_isAtEndGoal)
                {
                    // brute force search (breadth)
                    FindWayBreadthSearch(neighboursForAllNodes);
                }
            }
        }

        // Depthsearch
        void FindWayDepthSearch(Node currentNode)
        {
            // the currentNode counts as IsVisited
            currentNode.IsVisited = true;

            // get the next IsWalkable neighours Nodes 
            List<Node> neighbours = AllWalkableNotVisitedNeighbours(currentNode);

            if (neighbours.Count != 0)
            {
                Node? endNode = neighbours.FirstOrDefault(node => node.IsEnd);
                // if one of those neighbours is the End Node -> goal
                if (endNode != null)
                {
                    _isAtEndGoal = true;
                    DrawMaze();
                }
                // if none of the those nodes are the End Node -> keep searching
                else
                {
                    // brute force search (depth)
                    foreach (Node node in neighbours)
                    {
                        if (_isAtEndGoal)
                            break;
                        node.IsPath = true;
                        FindWayDepthSearch(node);
                        node.IsPath = false;
                    }
                }
            }
        }

        void DrawRectangleOnCanvas(Node node, System.Windows.Media.Color color)
        {
            byte distanceBetweenRect = 21;
            byte sizeOfRect = 20;

            System.Windows.Shapes.Rectangle rect;
            rect = new System.Windows.Shapes.Rectangle();
            // assoziate Tag with rectangle, give tag a mouse event
            if (node.IsWalkable)
            {
                rect.MouseDown += Rect_MouseDown;
            }

            rect.Tag = node;
            rect.Width = sizeOfRect;
            rect.Height = sizeOfRect;
            rect.Stroke = new SolidColorBrush(Colors.Black);
            rect.Fill = new SolidColorBrush(color);
            Canvas.SetLeft(rect, node.XPos * distanceBetweenRect);
            Canvas.SetTop(rect, node.YPos * distanceBetweenRect);
            DisplayCanvas.Children.Add(rect);
        }

        void Rect_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Rectangle rect = ((Rectangle)sender);

            // Check if a Beginning Node and End Node has been selected
            Node node = (Node)((Rectangle)sender).Tag;

            Node? startNode = _maze.NodeList.FirstOrDefault(node => node.IsBeginning);
            Node? endNode = _maze.NodeList.FirstOrDefault(node => node.IsEnd);

            // if there is no start or end, then the next clickt node ist start
            if (startNode == null && endNode == null)
            {
                _maze.StartNode = node;
                node.IsBeginning = true;
                rect.Fill = new SolidColorBrush(Colors.DarkGreen);
                DisplayCanvas.Children.Remove(rect);
                DisplayCanvas.Children.Add(rect);
            }
            // if there is start but no end, the next clicked node is end
            else if (startNode != null && endNode == null && startNode != node)
            {
                _maze.EndNode = node;
                node.IsEnd = true;
                rect.Fill = new SolidColorBrush(Colors.DarkRed);
                DisplayCanvas.Children.Remove(rect);
                DisplayCanvas.Children.Add(rect);
                // start immediately to find a way
                if (_maze.StartNode != null)
                {
                    if (DepthSearchRadioBtn.IsChecked == true)
                    {
                        FindWayDepthSearch(_maze.StartNode);
                    }
                    else if (BreadthSearchRadioBtn.IsChecked == true)
                    {
                        _nodesToBeVisited.Add(_maze.StartNode);
                        FindWayBreadthSearch(_nodesToBeVisited);
                    }
                    
                }
            }
        }

        private void DrawMaze()
        {
            List<Node> nodes = _maze.NodeList;

            foreach (var node in nodes)
            {
                if (node.IsWalkable)
                {
                    if (node.IsBeginning)
                    {
                        DrawRectangleOnCanvas(node, Colors.DarkGreen);
                    }
                    else if (node.IsEnd)
                    {
                        DrawRectangleOnCanvas(node, Colors.DarkRed);
                    }
                    /*else if (node.IsPath)
                    {
                        DrawRectangleOnCanvas(node, Colors.AliceBlue);
                    }*/
                    else if (node.IsVisited)
                    {
                        DrawRectangleOnCanvas(node, Colors.Magenta);
                    }
                    else
                    {
                        DrawRectangleOnCanvas(node, Colors.DarkOrange);
                    }
                }
                else
                {
                    DrawRectangleOnCanvas(node, Colors.DarkBlue);
                }
            }
        }

        void ResetMaze()
        {
            List<Node> nodes = _maze.NodeList;

            foreach (var node in nodes)
            {
                node.IsBeginning = false;
                node.IsEnd = false;
                node.IsVisited = false;
                node.IsPath = false;
                _isAtEndGoal = false;
                _maze.StartNode = null;
                _maze.EndNode = null;
                _nodeList.Clear();
                _nodesToBeVisited.Clear();
            }
        }

        private void RestCanvasBtn_Click(object sender, RoutedEventArgs e)
        {
            ResetMaze();
            DrawMaze();
        }
    }
}
