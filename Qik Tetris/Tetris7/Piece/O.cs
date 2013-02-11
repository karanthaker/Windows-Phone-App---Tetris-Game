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
    public class O : PieceBase
    {
        public override void InitPiece()
        {
            Matrix = new int[,]
            {
                {0,0,0,0},
                {0,1,1,0},
                {0,1,1,0},
                {0,0,0,0}
            };

            MaxIndex = 0;
        }

        public override int[,] GetRotate()
        {
            return Matrix;
        }

        public override Color Color
        {
            get { return Helper.GetColor("#880088"); }
        }
    }
}
