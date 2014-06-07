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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ModelDanSimulasi
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class WindowDDos : Window
    {
        Canvas _canvas;
        BitmapImage hacker, computer, serverOK, serverX;
        Random _random;

        public WindowDDos()
        {
            InitializeComponent();

            hacker = new BitmapImage(new Uri(@"/ModelDanSimulasi;component/Images/user.png", UriKind.Relative));
            computer = new BitmapImage(new Uri(@"/ModelDanSimulasi;component/Images/computer.png", UriKind.Relative));
            serverOK = new BitmapImage(new Uri(@"/ModelDanSimulasi;component/Images/computer_ok.png", UriKind.Relative));
            serverX = new BitmapImage(new Uri(@"/ModelDanSimulasi;component/Images/computer_x.png", UriKind.Relative));

            _canvas = new Canvas();
            _random = new Random(17 * DateTime.Now.Millisecond);

            this.Content = _canvas;

            addZombie(4);
            addHacker();
            addTarget(false);
        }

        public void addHacker()
        {
            _canvas.Children.Add(new Image
            {
                Source = hacker,
                Width = 64,
                Height = 64,
                Margin = new Thickness(10, (this.Height - 102) / 2, 0, 0),
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Left
            });
        }

        public void addZombie(int n)
        {
            double hackerPosition = (this.Height - 38) / 2;
            double tinggi = this.Height - 38;
            double total = 64.0 * n;
            if (tinggi < total)
            {
                this.Height = total + 38;
                tinggi = total;
            }
            double sisa = (tinggi - total) / (n + 1);
            for (int i = 0; i < n; i++)
            {
                _canvas.Children.Add(new Image
                {
                    Name = ("zombie" + i),
                    Source = computer,
                    Width = 64,
                    Height = 64,
                    Margin = new Thickness(200, (64 + sisa) * i + sisa, 0, 0)
                });
                _canvas.Children.Add(new Label
                {
                    Name = ("label" + i),
                    Width = 100,
                    Margin = new Thickness(190, (64 + sisa) * (i + 1), 0, 0),
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Content = "IP " + _random.Next(0, 255) + "." + _random.Next(0, 255) + "." + _random.Next(0, 255) + "." + _random.Next(0, 255)
                });
                _canvas.Children.Add(new Line
                {
                    Name = ("line" + i),
                    X1 = 90,
                    Y1 = hackerPosition,
                    X2 = 190,
                    Y2 = (64 + sisa) * i + sisa + 32,
                    Stroke = Brushes.Black,
                    StrokeThickness = 2
                });
            }
        }

        public void addTarget(bool statusOK)
        {
            _canvas.Children.Add(new Image
            {
                Source = statusOK ? serverOK : serverX,
                Width = 64,
                Height = 64,
                Margin = new Thickness(650, (this.Height - 102) / 2, 0, 0)
            });
        }
    }
}
