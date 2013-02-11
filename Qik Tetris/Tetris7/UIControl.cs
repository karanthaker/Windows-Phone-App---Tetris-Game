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
using System.ComponentModel;
using Tetris7.Piece;
using System.Windows.Threading;
using System.Collections.Generic;

namespace Tetris7
{
    public class UIControl : INotifyPropertyChanged
    {
        public Block[,] Container { get; set; }
        public Block[,] NextContainer { get; set; }
        public GameStatus GameStatus { get; set; }

        private int _rows = 20;
        private int _columns = 10;
        private int _positionX = 3;
        private int _positionY = 0;

        private List<PieceBase> _pieces;

        private PieceBase _currentPiece;
        private PieceBase _nextPiece;

        private int _initSpeed = 400;
        private int _levelSpeed = 50;

        private DispatcherTimer _timer;

        public UIControl()
        {
            _pieces = new List<PieceBase>() { new I(), new L(), new L2(), new N(), new N2(), new O(), new T() };

            Container = new Block[_rows, _columns];
            for (int i = 0; i < _rows; i++)
            {
                for (int j = 0; j < _columns; j++)
                {
                    var block = new Block();
                    block.Top = i * block.rectangle.ActualHeight;
                    block.Left = j * block.rectangle.ActualWidth;
                    block.Color = null;

                    Container[i, j] = block;
                }
            }

            NextContainer = new Block[4, 4];
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    var block = new Block();
                    block.Top = i * block.rectangle.ActualHeight;
                    block.Left = j * block.rectangle.ActualWidth;
                    block.Color = null;

                    NextContainer[i, j] = block;
                }
            }

            CreatePiece();
            AddPiece(0, 0);

            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromMilliseconds(_initSpeed);
            _timer.Tick += new EventHandler(_timer_Tick);

            GameStatus = GameStatus.Ready;
        }

        public void Play()
        {
            GameStatus = GameStatus.Play;
            _timer.Start();
        }

        public void Pause()
        {
            GameStatus = GameStatus.Pause;
            _timer.Stop();
        }

        private void CreatePiece()
        {
            for (int x = 0; x < _columns; x++)
            {
                if (Container[0, x].Color != null)
                {
                    OnGameOver(null);
                    break;
                }
            }

            Random random = new Random();
            _currentPiece = _nextPiece == null ? _pieces[random.Next(0, 7)] : _nextPiece;
            _nextPiece = _pieces[random.Next(0, 7)];

            _positionX = 3;
            _positionY = 0;

            SetNextContainerUI();
        }

        private void _timer_Tick(object sender, EventArgs e)
        {
            MoveToDown();
        }

        public void MoveToLeft()
        {
            if (GameStatus != GameStatus.Play) return;

            if (!IsBoundary(_currentPiece.Matrix, -1, 0))
            {
                RemovePiece();
                AddPiece(-1, 0);
            }
        }

        public void MoveToRight()
        {
            if (GameStatus != GameStatus.Play) return;

            if (!IsBoundary(_currentPiece.Matrix, 1, 0))
            {
                RemovePiece();
                AddPiece(1, 0);
            }
        }

        public void MoveToDown()
        {
            if (GameStatus != GameStatus.Play) return;

            if (!IsBoundary(_currentPiece.Matrix, 0, 1))
            {
                RemovePiece();
                AddPiece(0, 1);
            }
            else
            {
                RemoveRow();
                CreatePiece();

                Score++;
            }
        }

        public void Rotate()
        {
            if (GameStatus != GameStatus.Play) return;

            if (!IsBoundary(_currentPiece.GetRotate(), 0, 0))
            {
                RemovePiece();
                _currentPiece.Rotate();
                AddPiece(0, 0);
            }
        }

        public void Clear()
        {
            for (int x = 0; x < _columns; x++)
            {
                for (int y = 0; y < _rows; y++)
                {
                    Container[y, x].Color = null;
                }
            }
        }

        private bool IsBoundary(int[,] matrix, int offsetX, int offsetY)
        {
            RemovePiece();

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (matrix[i, j] == 1)
                    {
                        if (j + _positionX + offsetX > _columns - 1
                            || i + _positionY + offsetY > _rows - 1
                            || j + _positionX + offsetX < 0
                            || Container[i + _positionY + offsetY, j + _positionX + offsetX].Color != null)
                        {
                            AddPiece(0, 0);
                            return true;
                        }
                    }
                }
            }

            AddPiece(0, 0);
            return false;
        }

        private void SetNextContainerUI()
        {
            foreach (Block block in NextContainer)
            {
                block.Color = null;
            }

            for (int x = 0; x < 4; x++)
            {
                for (int y = 0; y < 4; y++)
                {
                    if (_nextPiece.Matrix[x, y] == 1)
                    {
                        NextContainer[x, y].Color = _nextPiece.Color;
                    }
                }
            }
        }

        private void RemovePiece()
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (_currentPiece.Matrix[i, j] == 1)
                    {
                        Container[i + _positionY, j + _positionX].Color = null;
                    }
                }
            }
        }

        private void AddPiece(int offsetX, int offsetY)
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (_currentPiece.Matrix[i, j] == 1)
                    {
                        Container[i + _positionY + offsetY, j + _positionX + offsetX].Color = _currentPiece.Color;
                    }
                }
            }

            _positionX += offsetX;
            _positionY += offsetY;
        }

        private void RemoveRow()
        {
            int removeRowCount = 0;

            for (int y = 0; y < _rows; y++)
            {
                bool isLine = true;

                for (int x = 0; x < _columns; x++)
                {
                    if (Container[y, x].Color == null)
                    {
                        isLine = false;
                        break;
                    }
                }

                if (isLine)
                {
                    removeRowCount++;

                    for (int x = 0; x < _columns; x++)
                    {
                        Container[y, x].Color = null;
                    }

                    for (int i = y; i > 0; i--)
                    {
                        for (int x = 0; x < _columns; x++)
                        {
                            Container[i, x].Color = Container[i - 1, x].Color;
                        }
                    }
                }
            }

            if (removeRowCount > 0)
                Score += 10 * (int)Math.Pow(2, removeRowCount);

            RemoveRowCount += removeRowCount;

            Level = (int)Math.Sqrt(RemoveRowCount / 5);

            _timer.Interval = TimeSpan.FromMilliseconds(_initSpeed - _levelSpeed * Level > _levelSpeed ? _initSpeed - _levelSpeed * Level : _levelSpeed);
        }

        private int _score = 0;
        public int Score
        {
            get { return _score; }
            set
            {
                _score = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Score"));
                }
            }
        }

        private int _removeRowCount = 0;
        public int RemoveRowCount
        {
            get { return _removeRowCount; }
            set
            {
                _removeRowCount = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("RemoveRowCount"));
                }
            }
        }

        private int _level = 0;
        public int Level
        {
            get { return _level; }
            set
            {
                _level = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Level"));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public event EventHandler GameOver;
        private void OnGameOver(EventArgs e)
        {
            GameStatus = GameStatus.Over;
            _timer.Interval = TimeSpan.FromMilliseconds(_initSpeed);
            _timer.Stop();

            EventHandler handler = GameOver;
            if (handler != null)
                handler(this, e);
        }
    }
}
