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
using System.Windows.Threading;

namespace ModelDanSimulasi
{
    /// <summary>
    /// Interaction logic for WindowMailBomb.xaml
    /// </summary>
    public partial class WindowMailBomb : Window
    {
        const double PICTURE_WIDTH = 64;
        const double PICTURE_HEIGHT = 64;
        const double LABEL_HEIGHT = 26;

        const double X_HACKER = 15;
        const double X_ZOMBIE = 150;
        const double X_TARGET = 500;
        const double X_ETC = 600;

        private BitmapImage hacker, computer, serverOK, serverX, email;
        private Random _random;
        private Image imageTarget;
        private Label labelTarget;
        private ListBox listBoxMail;
        private TextBox textBoxNZombie;
        private TextBox textBoxDiskSize;
        private TextBox textBoxInfoServer;
        private Slider sliderSpeed;
        private Button buttonSimulate;

        private DispatcherTimer timerBomb;

        private List<string> listIpZombie = new List<string>();
        private List<double> listYZombie = new List<double>();

        private int countBasic;
        private int usedSize;
        private int diskSize;

        public WindowMailBomb()
        {
            InitializeComponent();

            hacker = new BitmapImage(new Uri(@"/ModelDanSimulasi;component/Images/user.png", UriKind.Relative));
            computer = new BitmapImage(new Uri(@"/ModelDanSimulasi;component/Images/computer.png", UriKind.Relative));
            serverOK = new BitmapImage(new Uri(@"/ModelDanSimulasi;component/Images/computer_ok.png", UriKind.Relative));
            serverX = new BitmapImage(new Uri(@"/ModelDanSimulasi;component/Images/computer_x.png", UriKind.Relative));
            email = new BitmapImage(new Uri(@"/ModelDanSimulasi;component/Images/mail_cancel.png", UriKind.Relative));

            _random = new Random(17 * DateTime.Now.Millisecond);

            _canvas.Children.Add(textBoxInfoServer = new TextBox
            {
                Name = "infoRAM",
                Margin = new Thickness(X_ETC, 155, 0, 0),
                Height = 20,
                Width = 325,
                IsReadOnly = true
            });

            _canvas.Children.Add(listBoxMail = new ListBox
            {
                Margin = new Thickness(X_ETC, 180, 20, 20),
                Height = getHeight() - 180,
                Width = 325
            });

            // Slider
            Label speed;
            _canvas.Children.Add(speed = new Label
            {
                Content = "Speed (500ms)",
                Margin = new Thickness(X_ETC, 5, 0, 0)
            });
            _canvas.Children.Add(sliderSpeed = new Slider
            {
                Name = "sliderSpeed",
                Orientation = Orientation.Horizontal,
                Margin = new Thickness(X_ETC, 30, 0, 0),
                Width = 200,
                BorderBrush = Brushes.Black,
                BorderThickness = new Thickness(0.5),
                Maximum = 2000,
                Minimum = 50,
                Value = 500,
                SmallChange = 10,
                LargeChange = 10
            });
            _canvas.Children.Add(new Label
            {
                Content = "Faster",
                Margin = new Thickness(X_ETC, 50, 0, 0)
            });
            _canvas.Children.Add(new Label
            {
                Content = "Slower",
                Margin = new Thickness(X_ETC + 155, 50, 0, 0)
            });

            // N Zombie
            _canvas.Children.Add(new Label
            {
                Content = "N Zombie",
                Margin = new Thickness(X_ETC, 75, 0, 0)
            });
            _canvas.Children.Add(textBoxNZombie = new TextBox
            {
                Name = "textBoxNZombie",
                Margin = new Thickness(X_ETC + 100, 75, 0, 0),
                Width = 100
            });

            // Server RAM
            _canvas.Children.Add(new Label
            {
                Content = "Disk Size(MB)",
                Margin = new Thickness(X_ETC, 100, 0, 0)
            });
            _canvas.Children.Add(textBoxDiskSize = new TextBox
            {
                Name = "textBoxDiskSize",
                Margin = new Thickness(X_ETC + 100, 100, 0, 0),
                Width = 100
            });

            timerBomb = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(sliderSpeed.Value)
            };

            timerBomb.Tick += (s, e) =>
            {
                int idx, n = _random.Next(2, listIpZombie.Count);
                int mailSize;
                for (int i = 0; i < n; i++)
                {
                    idx = _random.Next(listIpZombie.Count);
                    mailSize = 1 << _random.Next(5, 11);
                    usedSize += mailSize;
                    listBoxMail.Items.Add("Mail from " + listIpZombie[idx] + ", size = " + mailSize + "kB");
                    Image mail = new Image
                    {
                        Source = email,
                        Width = 24,
                        Height = 24,
                        Margin = new Thickness(0, 0, 0, 0)
                    };
                    _canvas.Children.Add(mail);
                    double xNow = X_ZOMBIE + PICTURE_WIDTH + 10, yNow = listYZombie[idx];
                    Canvas.SetLeft(mail, xNow);
                    Canvas.SetTop(mail, yNow);
                    double duration = sliderSpeed.Value;
                    double xDiff = (X_TARGET - xNow - 30) * 16 / duration;
                    double yDiff = (getHeight() / 2 - yNow - 20) * 16 / duration;
                    double t = 0;
                    DispatcherTimer timerMail = new DispatcherTimer
                    {
                        Interval = TimeSpan.FromMilliseconds(16)
                    };
                    timerMail.Tick += (sender, ev) =>
                    {
                        t += 16;
                        xNow += xDiff;
                        yNow += yDiff;
                        Canvas.SetLeft(mail, xNow);
                        Canvas.SetTop(mail, yNow);
                        if (t >= duration)
                        {
                            mail.Visibility = Visibility.Hidden;
                            timerMail.Stop();
                        }
                    };
                    timerMail.Start();
                }
                textBoxInfoServer.Text = "Disk = " + usedSize + "kB / " + diskSize + "kB (" + Math.Round(((double)usedSize * 100 / diskSize), 2) + " %)";
                if (usedSize >= diskSize)
                {
                    timerBomb.Stop();
                    imageTarget.Source = serverX;
                    labelTarget.Background = Brushes.Red;
                    buttonSimulate.IsEnabled = true;
                }
            };

            sliderSpeed.ValueChanged += (s, e) =>
            {
                speed.Content = "Speed (" + sliderSpeed.Value + "ms)";
                timerBomb.Interval = TimeSpan.FromMilliseconds(sliderSpeed.Value);
            };

            countBasic = _canvas.Children.Add(buttonSimulate = new Button
            {
                Name = "buttonSimulate",
                Content = "DDos",
                Margin = new Thickness(X_ETC, 125, 20, 20)
            });

            countBasic++;

            buttonSimulate.Click += (s, e) => { simulasi(); };
        }

        private void simulasi()
        {
            if (countBasic < _canvas.Children.Count)
            {
                _canvas.Children.RemoveRange(countBasic, _canvas.Children.Count - countBasic);
            }

            textBoxInfoServer.Clear();

            int nZombie;

            try
            {
                if (int.TryParse(textBoxNZombie.Text, out nZombie) && nZombie < 9 && nZombie > 4)
                {
                    if (int.TryParse(textBoxDiskSize.Text, out diskSize))
                    {
                        usedSize += _random.Next(diskSize * 1024, diskSize * 2048) / 1024;
                        diskSize *= 1024;
                        this.Height = 630;
                        textBoxInfoServer.Text = "Disk = " + usedSize + "kB / " + diskSize + "kB (" + Math.Round(((double)usedSize * 100 / diskSize), 2) + " %)";
                        addZombie(nZombie);
                        addHacker();
                        addTarget(true);
                        buttonSimulate.IsEnabled = false;
                        timerBomb.Start();
                    }
                    else
                    {
                        throw new Exception("Cek ukuran Disk");
                    }
                }
                else
                {
                    throw new Exception("Jumlah komputer zombie antara 5 - 8");
                }
            }
            catch (Exception e)
            {
                _canvas.Children.Add(new Label
                {
                    Content = e.Message,
                    Margin = new Thickness(20, 20, 20, 20),
                    FontSize = 16,
                    Foreground = Brushes.Red
                });
                buttonSimulate.IsEnabled = true;
            }
        }

        private double getHeight()
        {
            return this.Height - 38;
        }

        private string getRandomIP()
        {
            return _random.Next(0, 255) + "." + _random.Next(0, 255) + "." + _random.Next(0, 255) + "." + _random.Next(0, 255);
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
                Content = getRandomIP()
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
            double zombiePosition;
            string ip;
            Line line;
            for (int i = 0; i < n; i++)
            {
                zombiePosition = (PICTURE_HEIGHT + LABEL_HEIGHT + sisa) * i + sisa;
                ip = getRandomIP();
                listIpZombie.Add(ip);
                listYZombie.Add(zombiePosition + 10);
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
                    Content = ip
                });

                line = new Line
                {
                    Name = ("lineHackerZombie" + i),
                    X1 = X_HACKER + PICTURE_WIDTH + 10,
                    Y1 = hackerPosition,
                    X2 = X_ZOMBIE - 10,
                    Y2 = (PICTURE_HEIGHT + LABEL_HEIGHT + sisa) * i + sisa + 32,
                    Stroke = Brushes.Green,
                    StrokeThickness = 2
                };
                _canvas.Children.Add(line);
            }
        }

        public void addTarget(bool statusOK)
        {
            _canvas.Children.Add(imageTarget = new Image
            {
                Name = "target",
                Source = statusOK ? serverOK : serverX,
                Width = PICTURE_WIDTH,
                Height = PICTURE_HEIGHT,
                Margin = new Thickness(X_TARGET, (getHeight() - PICTURE_HEIGHT - LABEL_HEIGHT) / 2, 0, 0)
            });
            _canvas.Children.Add(labelTarget = new Label
            {
                Name = "labelTarget",
                Width = PICTURE_WIDTH * 2,
                Margin = new Thickness(X_TARGET - (PICTURE_WIDTH / 2), (getHeight() + PICTURE_HEIGHT) / 2, 0, 0),
                HorizontalContentAlignment = HorizontalAlignment.Center,
                Background = Brushes.Green,
                Foreground = Brushes.White,
                Content = getRandomIP()
            });
        }
    }
}
