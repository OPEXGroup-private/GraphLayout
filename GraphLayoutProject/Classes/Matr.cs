using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphLayoutProject.Classes
{
    public class Matr<T>
    {
        public T[,] Elements;
        public int Lines;
        public int Columns;

        public Matr(int lines, int columns)
        {
            Lines = lines;
            Columns = columns;
            Elements = new T[lines,columns];
        }
        public T this[int line ,int column]
        {
            get { return Elements[line, column]; }
            set { Elements[line, column] = value; }
        }

    }
}
