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

namespace MoveShapesAroundExample
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Start();
        }

        private Point clickPosition;
        private TranslateTransform OriginTT;
        private List<Shape> Shapes;
        private Canvas MyCanvas;
        private ScrollViewer MyScrollViewer;
        private void Start()
        {

            Title = "GUI App";
            WindowState = WindowState.Maximized;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            var canvas = new Canvas
            {
                Background = Brushes.Beige,
                Focusable = true,
                AllowDrop = true
            };
            MyCanvas = canvas;
            Content = canvas;

            var originTT = new TranslateTransform();
            OriginTT = originTT;

            var scrollViewer = new ScrollViewer
            {
                Background = Brushes.Black,
                Focusable = true,
                Width = 200,
                Height = 200,
                IsEnabled = true,
                AllowDrop = true
            };
            MyScrollViewer = scrollViewer;
            canvas.Children.Add(scrollViewer);
            Canvas.SetLeft(scrollViewer, 10);
            Canvas.SetTop(scrollViewer, 50);


            var scrollViewer2 = new ScrollViewer
            {
                Background = Brushes.Black,
                Focusable = true,
                Width = 200,
                Height = 200,
                IsEnabled = true,
                AllowDrop = true
            };

            canvas.Children.Add(scrollViewer2);
            Canvas.SetLeft(scrollViewer2, 500);
            Canvas.SetTop(scrollViewer2, 50);

            scrollViewer.Drop += viewerDrop;
            scrollViewer2.Drop += viewerDrop;

            var rectangle1 = new Rectangle
            {
                Width = 100,
                Height = 100,
                Fill = Brushes.White
            };
            canvas.Children.Add(rectangle1);
            
            var rectangle2 = new Rectangle
            {
                Width = 100,
                Height = 100,
                Fill = Brushes.Blue
            };
            canvas.Children.Add(rectangle2);

            var shapes = new List<Shape> { rectangle1, rectangle2 };
            Shapes = shapes;

            foreach (var shape in shapes)
            {                
                shape.MouseLeftButtonDown += Shape_MouseLeftButtonDown;
            }            
        }

        private void Shape_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var shape = (Shape)sender;
            DataObject dataObj = new DataObject(shape.Fill);
            DragDrop.DoDragDrop(shape, dataObj, DragDropEffects.Move);
        }

        private void viewerDrop(object sender, DragEventArgs e)
        {
            var viewer = (ContentControl)sender;
            SolidColorBrush brush = (SolidColorBrush)e.Data.GetData(typeof(SolidColorBrush));
            viewer.Background = brush;
          
        }
    }
}
