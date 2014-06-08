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
using System.Windows.Media.Animation;

namespace ModelDanSimulasi
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class WindowDDos : Window
    {
        const double PICTURE_WIDTH = 64;
        const double PICTURE_HEIGHT = 64;
        const double LABEL_HEIGHT = 26;
        const double REFLECTOR_WIDTH = 30;
        const double REFLECTOR_HEIGHT = 30;

        const double X_HACKER = 25;
        const double X_ZOMBIE = 200;
        const double X_REFLECTOR = 350;
        const double X_TARGET = 600;

        private Storyboard DDos;

        private BitmapImage hacker, computer, serverOK, serverX;
        private Random _random;
        private int targetIndex = -1;

        public WindowDDos()
        {
            InitializeComponent();

            hacker = new BitmapImage(new Uri(@"/ModelDanSimulasi;component/Images/user.png", UriKind.Relative));
            computer = new BitmapImage(new Uri(@"/ModelDanSimulasi;component/Images/computer.png", UriKind.Relative));
            serverOK = new BitmapImage(new Uri(@"/ModelDanSimulasi;component/Images/computer_ok.png", UriKind.Relative));
            serverX = new BitmapImage(new Uri(@"/ModelDanSimulasi;component/Images/computer_x.png", UriKind.Relative));

            _random = new Random(17 * DateTime.Now.Millisecond);

            addZombie(5);
            addHacker();
            addTarget(true);
        }

        public double getHeight()
        {
            return this.Height - 38;
        }

        public void addHacker()
        {
            _canvas.Children.Add(new Image
            {
                Name = "hacker",
                Source = hacker,
                Width = PICTURE_WIDTH,
                Height = PICTURE_HEIGHT,
                Margin = new Thickness(X_HACKER, (getHeight() - PICTURE_HEIGHT - LABEL_HEIGHT) / 2, 0, 0),
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Left
            });
            _canvas.Children.Add(new Label
            {
                Name = "labelHacker",
                Width = PICTURE_WIDTH * 2,
                Margin = new Thickness(X_HACKER - (PICTURE_WIDTH / 2), (getHeight() + PICTURE_HEIGHT) / 2, 0, 0),
                HorizontalContentAlignment = HorizontalAlignment.Center,
                Content = _random.Next(0, 255) + "." + _random.Next(0, 255) + "." + _random.Next(0, 255) + "." + _random.Next(0, 255)
            });
        }

        public void addZombie(int n)
        {
            double tinggi = getHeight();
            double total = (PICTURE_HEIGHT + LABEL_HEIGHT) * n;
            if (tinggi < total)
            {
                this.Height = total + 38;
                tinggi = total;
            }
            double hackerPosition = tinggi / 2;
            double sisa = (tinggi - total) / (n + 1);
            double reflector1 = ((PICTURE_HEIGHT + LABEL_HEIGHT) - (REFLECTOR_HEIGHT * 2)) / 3;
            double reflector2 = reflector1 * 2 + REFLECTOR_HEIGHT;
            double zombiePosition;
            for (int i = 0; i < n; i++)
            {
                zombiePosition = (PICTURE_HEIGHT + LABEL_HEIGHT + sisa) * i + sisa;
                _canvas.Children.Add(new Image
                {
                    Name = ("zombieComputer" + i),
                    Source = computer,
                    Width = PICTURE_WIDTH,
                    Height = PICTURE_HEIGHT,
                    Margin = new Thickness(X_ZOMBIE, zombiePosition, 0, 0)
                });
                _canvas.Children.Add(new Label
                {
                    Name = ("labelZombie" + i),
                    Width = PICTURE_WIDTH * 2,
                    Margin = new Thickness(X_ZOMBIE - (PICTURE_WIDTH / 2), (PICTURE_HEIGHT + LABEL_HEIGHT + sisa) * (i + 1) - LABEL_HEIGHT, 0, 0),
                    HorizontalContentAlignment = HorizontalAlignment.Center,
                    Content = _random.Next(0, 255) + "." + _random.Next(0, 255) + "." + _random.Next(0, 255) + "." + _random.Next(0, 255)
                });
                _canvas.Children.Add(new Image
                {
                    Name = ("reflectorComputer" + (2 * i)),
                    Source = computer,
                    Width = REFLECTOR_WIDTH,
                    Height = REFLECTOR_HEIGHT,
                    Margin = new Thickness(X_REFLECTOR, zombiePosition + reflector1, 0, 0)
                });
                _canvas.Children.Add(new Image
                {
                    Name = ("reflectorComputer" + (2 * i + 1)),
                    Source = computer,
                    Width = REFLECTOR_WIDTH,
                    Height = REFLECTOR_HEIGHT,
                    Margin = new Thickness(X_REFLECTOR, zombiePosition + reflector2, 0, 0)
                });
                _canvas.Children.Add(new Line
                {
                    Name = ("lineHackerZombie" + i),
                    X1 = X_HACKER + PICTURE_WIDTH + 10,
                    Y1 = hackerPosition,
                    X2 = X_ZOMBIE - 10,
                    Y2 = (PICTURE_HEIGHT + LABEL_HEIGHT + sisa) * i + sisa + 32,
                    Stroke = Brushes.Black,
                    StrokeThickness = 1
                });
                _canvas.Children.Add(new Line
                {
                    Name = ("lineZombieReflector" + (2 * i)),
                    X1 = X_ZOMBIE + PICTURE_WIDTH + 10,
                    Y1 = zombiePosition + PICTURE_HEIGHT / 2,
                    X2 = X_REFLECTOR - 10,
                    Y2 = zombiePosition + reflector1 + REFLECTOR_HEIGHT / 2,
                    Stroke = Brushes.Black,
                    StrokeThickness = 1
                });
                _canvas.Children.Add(new Line
                {
                    Name = ("lineZombieReflector" + (2 * i  + 1)),
                    X1 = X_ZOMBIE + PICTURE_WIDTH + 10,
                    Y1 = zombiePosition + PICTURE_HEIGHT / 2,
                    X2 = X_REFLECTOR - 10,
                    Y2 = zombiePosition + reflector2 + REFLECTOR_HEIGHT / 2,
                    Stroke = Brushes.Black,
                    StrokeThickness = 1
                });
                _canvas.Children.Add(new Line
                {
                    Name = ("lineReflectorTarget" + (2 * i)),
                    X1 = X_REFLECTOR + REFLECTOR_WIDTH + 10,
                    Y1 = zombiePosition + reflector1 + REFLECTOR_HEIGHT / 2,
                    X2 = X_TARGET - 10,
                    Y2 = hackerPosition,
                    Stroke = Brushes.Black,
                    StrokeThickness = 1
                });
                _canvas.Children.Add(new Line
                {
                    Name = ("lineReflectorTarget" + (2 * i + 1)),
                    X1 = X_REFLECTOR + REFLECTOR_WIDTH + 10,
                    Y1 = zombiePosition + reflector2 + REFLECTOR_HEIGHT / 2,
                    X2 = X_TARGET - 10,
                    Y2 = hackerPosition,
                    Stroke = Brushes.Black,
                    StrokeThickness = 1
                });
            }
        }

        public void addTarget(bool statusOK)
        {
            targetIndex = _canvas.Children.Add(new Image
            {
                Name = "target",
                Source = statusOK ? serverOK : serverX,
                Width = PICTURE_WIDTH,
                Height = PICTURE_HEIGHT,
                Margin = new Thickness(X_TARGET, (getHeight() - PICTURE_HEIGHT - LABEL_HEIGHT) / 2, 0, 0)
            });
            _canvas.Children.Add(new Label
            {
                Name = "labelTarget",
                Width = PICTURE_WIDTH * 2,
                Margin = new Thickness(X_TARGET - (PICTURE_WIDTH / 2), (getHeight() + PICTURE_HEIGHT) / 2, 0, 0),
                HorizontalContentAlignment = HorizontalAlignment.Center,
                Content = _random.Next(0, 255) + "." + _random.Next(0, 255) + "." + _random.Next(0, 255) + "." + _random.Next(0, 255)
            });
        }
    }
}
