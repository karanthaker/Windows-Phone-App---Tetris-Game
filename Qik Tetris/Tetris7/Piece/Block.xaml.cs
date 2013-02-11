using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Tetris7.Piece
{
    public partial class Block : UserControl
    {
        public Block()
        {
            InitializeComponent();
        }

        public double Top
        {
            set
            {
                rectangle.SetValue(Canvas.TopProperty, value);
            }
        }

        public double Left
        {
            set
            {
                rectangle.SetValue(Canvas.LeftProperty, value);
            }
        }

        public Color? Color
        {
            set
            {
                if (value == null)
                {
                    rectangle.Visibility = Visibility.Collapsed;
                }
                else
                {
                    gradientStop.Color = (Color)value;
                    rectangle.Visibility = Visibility.Visible;
                }
            }
            get
            {
                if (rectangle.Visibility == Visibility.Collapsed)
                    return null;
                else
                    return gradientStop.Color;
            }
        }
    }
}
