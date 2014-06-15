using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Media.Animation;

namespace ModelDanSimulasi
{
    /// <summary>
    /// Interaction logic for WindowMITMA.xaml
    /// </summary>
    public partial class WindowMITMA : Window
    {
        const double PICTURE_WIDTH = 64;
        const double PICTURE_HEIGHT = 64;
        const double LABEL_HEIGHT = 26;

        private BitmapImage hacker, computer;
        private Image A, B, H;

        Line lineHacker;
        Line lineConnect;
        Line lineAHacker;
        Line lineBHacker;
        Label info;

        public WindowMITMA()
        {
            InitializeComponent();

            hacker = new BitmapImage(new Uri(@"/ModelDanSimulasi;component/Images/user.png", UriKind.Relative));
            computer = new BitmapImage(new Uri(@"/ModelDanSimulasi;component/Images/computer.png", UriKind.Relative));

            _canvas.Children.Add(A = new Image
            {
                Source = computer,
                Width = PICTURE_WIDTH,
                Height = PICTURE_HEIGHT,
                Margin = new Thickness(25, (getHeight() - PICTURE_HEIGHT) / 2, 0, 0)
            });
            _canvas.Children.Add(new Label
            {
                Width = PICTURE_WIDTH * 2,
                Margin = new Thickness(25 - (PICTURE_WIDTH / 2), (getHeight() + PICTURE_HEIGHT) / 2, 0, 0),
                HorizontalContentAlignment = HorizontalAlignment.Center,
                Content = "Alice"
            });
            _canvas.Children.Add(B = new Image
            {
                Source = computer,
                Width = PICTURE_WIDTH,
                Height = PICTURE_HEIGHT,
                Margin = new Thickness(getWidth() - PICTURE_HEIGHT - 25, (getHeight() - PICTURE_HEIGHT) / 2, 0, 0)
            });
            _canvas.Children.Add(new Label
            {
                Width = PICTURE_WIDTH * 2,
                Margin = new Thickness(getWidth() - PICTURE_HEIGHT - 25 - (PICTURE_WIDTH / 2), (getHeight() + PICTURE_HEIGHT) / 2, 0, 0),
                HorizontalContentAlignment = HorizontalAlignment.Center,
                Content = "Bob"
            });
            _canvas.Children.Add(H = new Image
            {
                Source = hacker,
                Width = PICTURE_WIDTH,
                Height = PICTURE_HEIGHT,
                Margin = new Thickness((getWidth() - PICTURE_WIDTH) / 2, getHeight() / 10, 0, 0)
            });
            _canvas.Children.Add(new Label
            {
                Width = PICTURE_WIDTH * 2,
                Margin = new Thickness(getWidth() / 2 - PICTURE_WIDTH, getHeight() / 10 + PICTURE_HEIGHT, 0, 0),
                HorizontalContentAlignment = HorizontalAlignment.Center,
                Content = "Hacker"
            });

            _canvas.Children.Add(lineHacker = new Line
            {
                Name = "lineHacker",
                X1 = getWidth() / 2,
                Y1 = getHeight() / 10 + PICTURE_HEIGHT + 30,
                X2 = getWidth() / 2,
                Y2 = getHeight() / 2,
                Stroke = Brushes.Red,
                StrokeThickness = 2,
                Opacity = 0
            });
            _canvas.Children.Add(lineConnect = new Line
            {
                Name = "lineConnect",
                X1 = 25 + PICTURE_WIDTH + 10,
                Y1 = getHeight() / 2,
                X2 = getWidth() - PICTURE_WIDTH - 25 - 10,
                Y2 = getHeight() / 2,
                Stroke = Brushes.Green,
                StrokeThickness = 2,
                Opacity = 0
            });
            _canvas.Children.Add(lineAHacker = new Line
            {
                Name = "lineAHacker",
                X1 = 40 + PICTURE_WIDTH / 2,
                Y1 = getHeight() / 2 - 50,
                X2 = (getWidth() - PICTURE_WIDTH) / 2 - 10,
                Y2 = getHeight() / 10 + 32,
                Stroke = Brushes.Green,
                StrokeThickness = 2,
                Opacity = 0
            });
            _canvas.Children.Add(lineBHacker = new Line
            {
                Name = "lineBHacker",
                X1 = getWidth() - (40 + PICTURE_WIDTH / 2),
                Y1 = getHeight() / 2 - 50,
                X2 = (getWidth() + PICTURE_WIDTH) / 2 + 10,
                Y2 = getHeight() / 10 + 32,
                Stroke = Brushes.Green,
                StrokeThickness = 2,
                Opacity = 0
            });
            _canvas.Children.Add(info = new Label
            {
                Content = "",
                FontSize = 16,
                Margin = new Thickness(20, getHeight() / 2 + 50, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Stretch
            });

            this.RegisterName(lineHacker.Name, lineHacker);
            this.RegisterName(lineAHacker.Name, lineAHacker);
            this.RegisterName(lineBHacker.Name, lineBHacker);
            this.RegisterName(lineConnect.Name, lineConnect);

            Button play;
            _canvas.Children.Add(play = new Button
            {
                Content = "Start",
                Margin = new Thickness(20, 60, 20, 20)
            });
            play.Click += (s, e) =>
            {
                _actions = AnimationSequence().GetEnumerator();
                RunNextAction();
            };
        }

        private double getHeight()
        {
            return this.Height - 38;
        }

        private double getWidth()
        {
            return this.Width - 22;
        }


        IEnumerable<Action<Action>> AnimationSequence()
        {
            yield return message("Koneksi Alice ke Bob");
            yield return ShowOut(lineConnect);
            yield return message("Intercepted by Hacker");
            yield return ShowOut(lineHacker);
            yield return message("Koneksi menjadi Alice - Hacker dan Hacker - Bob");
            yield return ShowOut(lineAHacker);
            yield return ShowOut(lineBHacker);
            yield return hideLine(lineHacker);
            yield return hideLine(lineConnect);
            yield return message("Alice \"Hi Bob, it's Alice. Give me your key\"--> Mallory");
            yield return hideLine(lineAHacker);
            yield return ShowOut(lineAHacker);
            yield return message("Mallory \"Hi Bob, it's Alice. Give me your key\"--> Bob");
            yield return hideLine(lineBHacker);
            yield return ShowOut(lineBHacker);
            yield return message("Mallory <--[Bob's_key] Bob");
            yield return hideLine(lineBHacker);
            yield return ShowOut(lineBHacker);
            yield return message("Alice <--[Fake Bob's_key] Mallory");
            yield return hideLine(lineAHacker);
            yield return ShowOut(lineAHacker);

            for (int i = 0; i < 5; i++)
            {
                // tambah pesan random atao gimana gitu
                StringBuilder mess = new StringBuilder();
                yield return message(mess.ToString());
                yield return hideLine(lineAHacker);
                yield return ShowOut(lineAHacker);
                yield return hideLine(lineBHacker);
                yield return ShowOut(lineBHacker);
                yield return message(mess.ToString());
                yield return hideLine(lineBHacker);
                yield return ShowOut(lineBHacker);
                yield return hideLine(lineAHacker);
                yield return ShowOut(lineAHacker);
            }

            yield return hideLine(lineAHacker);
            yield return hideLine(lineBHacker);
        }

        private IEnumerator<Action<Action>> _actions;

        private void RunNextAction()
        {
            if (_actions.MoveNext())
                _actions.Current(RunNextAction);
        }

        private Action<Action> ShowOut(Line line)
        {
            return completed =>
            {
                DoubleAnimation showOut = new DoubleAnimation
                {
                    From = 0.0,
                    To = 1.0,
                    Duration = TimeSpan.FromSeconds(2),
                    BeginTime = TimeSpan.FromSeconds(1)
                };
                showOut.Completed += (s, e) => completed();
                line.BeginAnimation(Line.OpacityProperty, showOut);
            };
        }

        private Action<Action> hideLine(Line line)
        {
            return completed =>
            {
                DoubleAnimation fade = new DoubleAnimation
                {
                    To = 0.0,
                    Duration = TimeSpan.FromSeconds(0.01)
                };
                fade.Completed += (s, e) => completed();
                line.BeginAnimation(Line.OpacityProperty, fade);
            };
        }

        private Action<Action> message(string pesan)
        {
            info.Content = pesan;
            return completed => { completed(); };
        }
    }
}
