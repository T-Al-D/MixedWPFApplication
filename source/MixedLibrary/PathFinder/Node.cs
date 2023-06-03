using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RekursionTests.Lib
{
    public class Node
    {
        public int XPos { get; set; }
        public int YPos { get; set; }
        public bool IsPath { get; set; } = false;
        public bool IsVisited { get; set; } = false;
        public bool IsWalkable { get; set; } = false;
        public bool IsBeginning { get; set; } = false;
        public bool IsEnd { get; set; } = false;
    }
}
