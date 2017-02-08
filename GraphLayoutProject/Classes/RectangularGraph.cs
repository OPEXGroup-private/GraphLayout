using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphLayoutProject.Classes
{
    public class RectangularGraph
    {
        public static int FormWidth = 800;
        public static int FormHeigth = 640;
        public Rectangle Content;
        public List<RectangularGraph> NextElements { get; set; }
        public bool Flag { get; set; }
        public RectangularGraph(int width, int height, List<RectangularGraph> nextElements)
        {
            NextElements = nextElements;
            Content = new Rectangle(width, height);
            Flag = false;
        }
        public RectangularGraph(int width, int height)
        {
            Content = new Rectangle(width, height);
            Flag = false;
        }

   

    }
}
