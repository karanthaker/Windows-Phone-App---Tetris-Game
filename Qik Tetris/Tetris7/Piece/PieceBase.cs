using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Tetris7.Piece
{
    public abstract class PieceBase
    {
        public PieceBase()
        {
            InitPiece();
        }
        public int[,] Matrix { get; set; }


        private int _index = 0;

        public int MaxIndex { get; set; }


        public abstract void InitPiece();

 
        public abstract int[,] GetRotate();

 
        public abstract Color Color { get; }


        public int GetNextIndex()
        {
            int nextIndex = _index >= MaxIndex ? 0 : _index + 1;

            return nextIndex;
        }

 
        public void Rotate()
        {
            Matrix = GetRotate();

            _index = GetNextIndex();
        }
    }
}
