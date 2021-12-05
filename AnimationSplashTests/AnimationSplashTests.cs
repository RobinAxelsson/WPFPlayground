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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace AnimationSplashTests
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Start();
        }

        private Canvas MainCanvas;
        private Storyboard MainStoryboard;
        Rectangle PinkRectangle;
        Rectangle Square;
        DispatcherTimer SizeTimer = new DispatcherTimer();
        DispatcherTimer MovementTimer = new DispatcherTimer();

        private void Start()
        {

            Title = "GUI App";
            WindowState = WindowState.Maximized;

            //WindowStartupLocation = WindowStartupLocation.CenterScreen;
            //WindowStyle = WindowStyle.None; //removes the top titlebar
            //Arrange(new Rect(0, 0, DesiredSize.Width, DesiredSize.Height));

            var canvas = new Canvas();
            MainCanvas = canvas;
            Content = canvas;
            canvas.Background = Brushes.Beige;
            canvas.Focusable = true;
            canvas.Focus();
            

            var square = new Rectangle
            {
                Fill = Brushes.DarkBlue,
                Width = 50,
                Height = 50
            };
            Square = square;

            canvas.Children.Add(square);

            var rectangle = new Rectangle
            {
                Fill = Brushes.DeepPink,
                Width = 80,
                Height = 50
            };

            PinkRectangle = rectangle;
            canvas.Children.Add(rectangle);
            Canvas.SetTop(rectangle, 200);
            Canvas.SetLeft(rectangle, 200);

            Loaded += MainWindow_Loaded;
               
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var point = new Point();
            var point2 = PinkRectangle.TranslatePoint(point, MainCanvas);
            double canvasWidth = MainCanvas.ActualWidth;
            double canvasHeight = MainCanvas.ActualHeight;
            var sb = new Storyboard();
            var slideAnimation = new ThicknessAnimation
            {
                Duration = new Duration(TimeSpan.FromSeconds(5)),
                From = new Thickness(canvasWidth - 80, 0, 0, 0),
                To = new Thickness(0),
                DecelerationRatio = 0.5f,
                AccelerationRatio = 0.5f,
                AutoReverse = true,
                RepeatBehavior = RepeatBehavior.Forever
            };
            var heightAnimation = new DoubleAnimation
            {
                Duration = new Duration(TimeSpan.FromSeconds(5)),
                From = 100,
                To = 200,
                AutoReverse = true,
                RepeatBehavior = RepeatBehavior.Forever,
                DecelerationRatio = 0.5f,
                AccelerationRatio = 0.5f
            };
            var opacityAnimation = new DoubleAnimation
            {
                Duration = new Duration(TimeSpan.FromSeconds(5)),
                From = 1,
                To = 0,
                AutoReverse = true,
                RepeatBehavior = RepeatBehavior.Forever,
                DecelerationRatio = 0.5f,
                AccelerationRatio = 0.5f
            };
            var colorAnimation = new ColorAnimation
            {
                Duration = new Duration(TimeSpan.FromMilliseconds(100)),
                From = Colors.Red,
                To = Colors.Blue,
                AutoReverse = true,
                RepeatBehavior = RepeatBehavior.Forever,
                DecelerationRatio = 0.5f,
                AccelerationRatio = 0.5f
            };
            

            Storyboard.SetTargetProperty(opacityAnimation, new PropertyPath("Opacity"));
            Storyboard.SetTargetProperty(slideAnimation, new PropertyPath("Margin"));
            Storyboard.SetTargetProperty(heightAnimation, new PropertyPath("Height"));
            Storyboard.SetTargetProperty(colorAnimation, new PropertyPath("(Rectangle.Fill).Color"));

            sb.Children.Add(slideAnimation);
            sb.Children.Add(heightAnimation);
            sb.Children.Add(opacityAnimation);
            sb.Children.Add(colorAnimation);

            sb.Begin(Square);
            sb.Begin(PinkRectangle);

        }
    }
}
