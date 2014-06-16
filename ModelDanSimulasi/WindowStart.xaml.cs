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
    /// Interaction logic for WindowStart.xaml
    /// </summary>
    public partial class WindowStart : Window
    {
        public WindowStart()
        {
            InitializeComponent();

            buttonDDos.Click += (s, e) =>
            {
                Window ddos = new WindowDDos();
                ddos.ShowDialog();
            };

            buttonMITMA.Click += (s, e) =>
            {
                Window mitma = new WindowMITMA();
                mitma.ShowDialog();
            };

            buttonMailBomb.Click += (s, e) =>
            {
                Window mailBomb = new WindowMailBomb();
                mailBomb.ShowDialog();
            };
        }
    }
}
