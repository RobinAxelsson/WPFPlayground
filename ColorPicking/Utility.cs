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

namespace ColorPicking
{
    public class Utility
    {
        private Grid UtilityGrid;
        
        public Utility(Grid grid)
        {
            UtilityGrid = grid;
        }
        public void RowsColumns(int Rows, int Columns)
        {

        }
        //Random r = new Random();
        //public MainWindow()
        //{
        //    InitializeComponent();
        //    Title = Convert.ToString(DateTime.Now); // Der Titel bekommt den Wert des aktuellen Datum

        //}

        //private void Button_Click(object sender, RoutedEventArgs e)
        //{

        //    MainWindow mainwindow_1 = new MainWindow();
        //    Brush brush = new SolidColorBrush(Color.FromRgb(r.Next(1, 255), r.Next(1, 255), r.Next(1, 233)));
        //    mainwindow_1.txtbox_1.Background = brush;
        //    mainwindow_1.Show();


        //}

    }
}
