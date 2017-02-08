using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphLayoutProject.Classes
{
    public class Rectangle
    {
        public int Identificator { get; set; }
        public Rectangle(int width, int height)
        {
            Width = width;
            Height = height;
        }
        public int Width { get; set; }

        public int Height { get; set; }

        public int Left { get; set; } = 0;

        public int Top { get; set; } = 0;
        public static int CompareByIdentificator(Rectangle rectangle1, Rectangle rectangle2)
        {
            return rectangle1.Identificator - rectangle2.Identificator;
        }
    };
}
