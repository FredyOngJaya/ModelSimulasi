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
        }

        private double getHeight()
        {
            return this.Height - 38;
        }

        private double getWidth()
        {
            return this.Width - 22;
        }
    }
}
