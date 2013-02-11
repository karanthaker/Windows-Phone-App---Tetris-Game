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
using Microsoft.Phone.Controls;
using System.IO.IsolatedStorage;
using Tetris7.Piece;
using Microsoft.Phone.Tasks;
using Microsoft.Devices;

namespace Tetris7
{
    public partial class MainPage : PhoneApplicationPage
    {
        UIControl _control;
        List<ScoreObject> scoreList = new List<ScoreObject>();

        public MainPage()
        {
            InitializeComponent();
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (IsolatedStorageSettings.ApplicationSettings.Contains("scoreList"))
            {
                scoreList = IsolatedStorageSettings.ApplicationSettings["scoreList"] as List<ScoreObject>;

                if (scoreList != null)
                {
                    var tempList =
                    from score in scoreList
                    where score.Score > 0
                    orderby score.Score descending
                    select score;

                    lboScore.ItemsSource = null;
                    lboScore.ItemsSource = tempList.ToList(); ;
                }
            }

            this.Focus();

            _control = new UIControl();
            _control.GameOver += new EventHandler(_control_GameOver);
            this.DataContext = _control;

            foreach (Block block in _control.Container)
            {
                canvasBox.Children.Add(block);
            }

            foreach (Block block in _control.NextContainer)
            {
                canvasBoxPrev.Children.Add(block);
//                canvasBoxPrev1.Children.Add(block);
            }
        }


        private void rate_click(object sender, RoutedEventArgs e)
        {
            // pop up the link to rate and review the app
 //           MarketplaceDetailTask Details = new MarketplaceDetailTask();
//            Details.ContentIdentifier = "1b4be913-e6c5-4af8-9cb3-afb102eda038";
            MarketplaceReviewTask review = new MarketplaceReviewTask();
            review.Show(); 

        }

        public void more_apps_dev(object sender, RoutedEventArgs e)
        {   
            MarketplaceSearchTask marketplaceSearchTask = new MarketplaceSearchTask();
            marketplaceSearchTask.SearchTerms = "Karan Thaker";
            marketplaceSearchTask.Show();

        }




        void _control_GameOver(object sender, EventArgs e)
        {
            gameOver.Visibility = Visibility.Visible;
            play.Content = "start";

            int count = Convert.ToInt32(lblScore.Text);
            count++;
            scoreList.Add(new ScoreObject { Score = count, Date = "Date: " + DateTime.Now.ToString() });

            if (IsolatedStorageSettings.ApplicationSettings.Contains("scoreList"))
                IsolatedStorageSettings.ApplicationSettings["scoreList"] = scoreList;
            else
                IsolatedStorageSettings.ApplicationSettings.Add("scoreList", scoreList);

            if (scoreList != null)
            {
                var tempList =
                from score in scoreList
                where score.Score > 0
                orderby score.Score descending
                select score;

                lboScore.ItemsSource = null;
                lboScore.ItemsSource = tempList.ToList(); ;
            }
        }

        private void uc_KeyDown(object sender, KeyEventArgs e)
        {
            VibrateController vibrate = VibrateController.Default;
            vibrate.Start(TimeSpan.FromMilliseconds(25));


            
            if (_control.GameStatus != GameStatus.Play) return;

            if (e.Key == Key.Left)
            {
                _control.MoveToLeft();
            }
            else if (e.Key == Key.Right)
            {
                _control.MoveToRight();
            }
            else if (e.Key == Key.Up)
            {
                _control.Rotate();
            }
            else if (e.Key == Key.Down)
            {
                _control.MoveToDown();
            }
        }

        private void start_Click(object sender, RoutedEventArgs e)
        {
            VibrateController vibrate = VibrateController.Default;
            vibrate.Start(TimeSpan.FromMilliseconds(25));


            if (play.Content.ToString() == "start")
            {
                if (_control.GameStatus == GameStatus.Over)
                {
                    _control.Clear();
                    gameOver.Visibility = Visibility.Collapsed;
                    _control.Score = 0;
                    _control.Level = 0;
                    _control.RemoveRowCount = 0;
                }

                _control.Play();
                play.Content = "pause";
            }
            else
            {
                _control.Pause();
                play.Content = "start";
            }
        }


        private void score_Clear(object sender, RoutedEventArgs e)
        {
            VibrateController vibrate = VibrateController.Default;
            vibrate.Start(TimeSpan.FromMilliseconds(25));

            _control.Clear();
            _control.Score = 0;
            _control.Level = 0;
            _control.RemoveRowCount = 0;
            _control.Play();
            play.Content = "pause";
            

        }


        private void btnLeft_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            VibrateController vibrate = VibrateController.Default;
            vibrate.Start(TimeSpan.FromMilliseconds(25));
            _control.MoveToLeft();
        }

        private void btnDown_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            VibrateController vibrate = VibrateController.Default;
            vibrate.Start(TimeSpan.FromMilliseconds(25));
            _control.MoveToDown();
        }

        private void btnRight_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            VibrateController vibrate = VibrateController.Default;
            vibrate.Start(TimeSpan.FromMilliseconds(25));
            _control.MoveToRight();
        }

        private void btnChange_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            VibrateController vibrate = VibrateController.Default;
            vibrate.Start(TimeSpan.FromMilliseconds(25));
            _control.Rotate();
        }

        private void lboScore_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            lboScore.SelectedIndex = -1;
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }
    }

    public class ScoreObject
    {
        public int Score { get; set; }
        public string Date { get; set; }
    }
}
