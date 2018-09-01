using System;
using xxdwunity.util;

namespace xxdwunity.vo
{
    public class Cell
    {
        public int Row { get; set; }    // 行号，0-based
        public int Col { get; set; }    // 列号，0-based

        public Cell()
        {
            this.Row = 0;
            this.Col = 0;
        }
        public Cell(Cell cell)
        {
            this.Row = cell.Row;
            this.Col = cell.Col;
        }
        public Cell(int row, int col)
        {
            this.Row = row;
            this.Col = col;
        }

        public override int GetHashCode()
        {
            return this.Row * Int16.MaxValue + this.Col;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType()) return false;

            Cell c = (Cell)obj;
            return this.Row == c.Row && this.Col == c.Col;
        }  

        public static bool operator ==(Cell lhs, Cell rhs)
        {
            return lhs.Row == rhs.Row && lhs.Col == rhs.Col;
        }

        public static bool operator !=(Cell lhs, Cell rhs)
        {
            return !(lhs == rhs);
        }
    }
}


