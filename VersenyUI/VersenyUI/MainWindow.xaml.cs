using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace VersenyUI
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.Width = System.Convert.ToInt16(System.Math.Round(System.Windows.SystemParameters.FullPrimaryScreenWidth * 0.665));
            this.Height = System.Convert.ToInt16(System.Math.Round(System.Windows.SystemParameters.FullPrimaryScreenHeight * 0.665));



        }
    }
}
