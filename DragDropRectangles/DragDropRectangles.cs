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

namespace DragDropRectangles
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Start();
        }
        protected bool isDragging;
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
            var canvas = new Canvas();
            MyCanvas = canvas;
            canvas.Background = Brushes.Beige;
            Content = canvas;

            var originTT = new TranslateTransform();
            OriginTT = originTT;

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
            Canvas.SetLeft(rectangle2, 500);

            var shapes = new List<Shape> { rectangle1, rectangle2 };
            Shapes = shapes;

            foreach (var shape in shapes)
            {
                shape.MouseLeftButtonDown += Canvas_MouseLeftButtonDown;                
                shape.MouseLeftButtonUp += Canvas_MouseLeftButtonUp;
                shape.MouseMove += Canvas_MouseMove;
            }

        }

        private void Canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var shape = sender as Shape;

            if (sender == null) return;

            OriginTT = shape.RenderTransform as TranslateTransform ?? new TranslateTransform();
            isDragging = true;
            clickPosition = e.GetPosition(this);
            shape.CaptureMouse();

        }
        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            var obj = sender as FrameworkElement;

            var draggableControl = obj;
            if (isDragging && draggableControl != null)
            {
                Point currentPosition = e.GetPosition(this);
                var transform = draggableControl.RenderTransform as TranslateTransform ?? new TranslateTransform();
                transform.X = OriginTT.X + (currentPosition.X - clickPosition.X);
                transform.Y = OriginTT.Y + (currentPosition.Y - clickPosition.Y);
                draggableControl.RenderTransform = new TranslateTransform(transform.X, transform.Y);
            }

        }
        private void Canvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var obj = sender as FrameworkElement;
            var parent = (FrameworkElement)obj.Parent;

            isDragging = false;
            var draggable = obj;
            if (draggable == null) return;
            draggable.ReleaseMouseCapture();
        }
    }
}
