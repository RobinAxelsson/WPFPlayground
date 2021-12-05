using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
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
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PrivateIsland
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Start();
        }

        Label PrizeTitelLabel;
        Random random = new Random();
        int multiply = 2;
        StackPanel StaredPanel = new StackPanel();
        StackPanel MainImageStack;
        Image SelectedIsland;
        ScrollViewer ScrollViewer1;
        List<Image> Islands = new List<Image>();
        Grid MainGrid = new Grid();
        Canvas Root = new Canvas();
        Storyboard SBoard;
        Button AddBtn;
        private void Start()
        {
            Title = "Private Islands";
            WindowState = WindowState.Maximized;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;


            var rootCanvas = new Canvas();
            Root = rootCanvas;
            Content = rootCanvas;

            var mainGrid = new Grid();
            MainGrid.ShowGridLines = true;
            mainGrid.Margin = new Thickness(5);
            MainGrid = mainGrid;
            rootCanvas.Children.Add(mainGrid);
            Canvas.SetLeft(mainGrid, 0);
            Canvas.SetTop(mainGrid, 0);

            ImageSource source = new BitmapImage(new Uri("background.jpg", UriKind.Relative));

            ImageBrush imageBrush = new ImageBrush { ImageSource = source};
            ImageBrush imageBrushTitle = new ImageBrush { ImageSource = source };
            rootCanvas.Background = imageBrush;
            
            ColumnsAndRows(mainGrid, columns: 2, rows: 3);
            mainGrid.RowDefinitions[0].Height = GridLength.Auto;
            mainGrid.RowDefinitions[1].Height = new GridLength(900);
            mainGrid.ColumnDefinitions[0].Width = GridLength.Auto;
            mainGrid.ColumnDefinitions[1].Width = GridLength.Auto;

            imageBrush.Opacity = 0.8;

            var title = new Label {
                HorizontalContentAlignment = HorizontalAlignment.Center,
                Content = "Private Islands",
                VerticalContentAlignment = VerticalAlignment.Center,
                FontSize = 40,
                Foreground = imageBrushTitle

            };
            mainGrid.Children.Add(title);
            Grid.SetColumn(title, 0);
            Grid.SetRow(title, 0);           

            var scrollViewer1 = new ScrollViewer();
            ScrollViewer1 = scrollViewer1;
            mainGrid.Children.Add(scrollViewer1);
            Grid.SetColumn(scrollViewer1, 0);
            Grid.SetRow(scrollViewer1, 1);

            
            var mainImagesStack = new StackPanel { Orientation = Orientation.Vertical };
        
            scrollViewer1.Content = mainImagesStack;
            MainImageStack = mainImagesStack;
            ScrollViewer.SetVerticalScrollBarVisibility(scrollViewer1, ScrollBarVisibility.Hidden);

            var prizeTitelLabel = new Label {
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(10,0,0,0),
                Content = "",
                FontSize = 20
            };
            PrizeTitelLabel = prizeTitelLabel;

            mainGrid.Children.Add(prizeTitelLabel);
            Grid.SetColumn(prizeTitelLabel, 0);
            Grid.SetRow(prizeTitelLabel, 2);

            Image island1 = CreateImage("island1.png");
            island1.Tag = "Virgin Islands: Lady Island, Prize 100 000 000 $";
            Image island2 = CreateImage("island2.png");
            island2.Tag = "Virgin Islands: Daddy Island, Prize 200 000 000 $";
            Image island3 = CreateImage("island3.png");
            island3.Tag = "Virgin Islands: Sister Island, Prize 150 000 000 $";

       
            Islands.Add(island1);
            Islands.Add(island2);
            Islands.Add(island3);

            //island1.MouseLeftButtonDown += Island1_MouseLeftButtonDown;

            foreach (var island in Islands)
            {
                island.MouseEnter += Island_MouseEnter;
                island.MouseLeftButtonDown += Island_MouseLeftButtonDown;
            }

            mainImagesStack.Children.Add(island1);
            mainImagesStack.Children.Add(island2);
            mainImagesStack.Children.Add(island3);

            var rightPanel = new StackPanel { Orientation = Orientation.Vertical };
            mainGrid.Children.Add(rightPanel);
            Grid.SetColumn(rightPanel, 1);
            Grid.SetRow(rightPanel, 1);

            var buttonPanel = new StackPanel { Orientation = Orientation.Vertical };
            rightPanel.Children.Add(buttonPanel);
            //Grid.SetColumn(buttonPanel, 1);
            //Grid.SetRow(buttonPanel, 1);

            var focusBtn = AddButton("Focus");


            var addBtn = AddButton("Add");
            AddBtn = addBtn;
            var starBtn = AddButton("Star");

            buttonPanel.Children.Add(focusBtn);
            buttonPanel.Children.Add(addBtn);
            buttonPanel.Children.Add(starBtn);

            var scrollViewer2 = new ScrollViewer
            {
                Margin = new Thickness(10)
            };
            rightPanel.Children.Add(scrollViewer2);
            ScrollViewer.SetVerticalScrollBarVisibility(scrollViewer2, ScrollBarVisibility.Hidden);

            var staredPanel = new StackPanel
            {
                MaxWidth = 100
            };
            scrollViewer2.Content = staredPanel;
            StaredPanel = staredPanel;

            starBtn.Click += StarBtn_Click;
            addBtn.Click += AddBtn_Click;
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            var point = new Point(0, 0);
            var point2 = SelectedIsland.TranslatePoint(point, MainGrid);
            var canvas = Root;

            ImageSource source = new BitmapImage(new Uri(SelectedIsland.Source.ToString()));
            Image boughtIsland = new Image
            {
                Source = source,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Stretch = Stretch.Fill
            };


            canvas.Children.Add(boughtIsland);
            Canvas.SetLeft(SelectedIsland, point2.X);
            Canvas.SetTop(SelectedIsland, point.Y);

            SelectedIsland.Opacity = 0;

            var point3 = AddBtn.TranslatePoint(point, MainGrid);

            var sb = new Storyboard();
            SBoard = sb;
            var slideAnimation = new ThicknessAnimation
            {
                Duration = new Duration(TimeSpan.FromMilliseconds(500)),
                To = new Thickness(point3.X, point3.Y, 0, 0),
                From = new Thickness(0),
                DecelerationRatio = 0.5f,
                AccelerationRatio = 0.5f,
            };

            sb.Completed += Sb_Completed;
            Storyboard.SetTargetProperty(slideAnimation, new PropertyPath("Margin"));
            sb.Children.Add(slideAnimation);
            sb.Begin(boughtIsland);


            var scaleTransform = new ScaleTransform(scaleX: 1, scaleY: 1);
            boughtIsland.RenderTransform = scaleTransform;


            var scaleAnimation = new DoubleAnimation
            {
                From = 1,
                To = 0.1,
                Duration = new Duration(TimeSpan.FromMilliseconds(500)),
                AccelerationRatio = 0.5
            };

            scaleTransform.BeginAnimation(ScaleTransform.ScaleYProperty, scaleAnimation);
            scaleTransform.BeginAnimation(ScaleTransform.ScaleXProperty, scaleAnimation);
        }


        private void Sb_Completed(object sender, EventArgs e)
        {
            MainImageStack.Children.Remove(SelectedIsland);
        }

        private void StaredImage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var miniIsl = (Image)sender;
            var island = Islands.Find(isl => isl.Source.ToString() == miniIsl.Source.ToString());
            Islands.ForEach(isl => isl.Opacity = 1.0);
            Keyboard.Focus(island);
           
        }

        private void StarBtn_Click(object sender, RoutedEventArgs e)
        {

            if (SelectedIsland != null)
            {
                foreach (Image mini in StaredPanel.Children)
                {
                    if (mini.Source.ToString() == SelectedIsland.Source.ToString())
                    {
                        return;
                    }
                }
                ImageSource source = new BitmapImage(new Uri(SelectedIsland.Source.ToString()));
                Image staredImage = new Image
                {
                    Source = source,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    Stretch = Stretch.Fill
                };

                StaredPanel.Children.Add(staredImage);
                staredImage.MouseLeftButtonDown += StaredImage_MouseLeftButtonDown;      
            }
        }
        private void Island_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
        
            var island = (Image)sender;
            SelectedIsland = island;
            island.Opacity = 1.0;
            var fadedIslands = new List<Image>();
            fadedIslands.AddRange(Islands);
            fadedIslands.Remove(island);
            foreach (var isl in fadedIslands)
            {
                isl.Opacity = 0.5;
            }
                if (e.ClickCount == 2)
                {
                    Islands.ForEach(isl => isl.Opacity = 0.5);

                    var textBlock = new TextBlock
                    {
                        HorizontalAlignment = HorizontalAlignment.Stretch,
                        Text = File.ReadAllText("island1.csv"),
                        Background = Brushes.Transparent,
                        Foreground = Brushes.White,
                        Width = island.ActualWidth-30,
                        TextWrapping = TextWrapping.Wrap,
                        FontSize = 16,
                    };

                    ScrollViewer1.IsEnabled = false;
                    var tempStackPanel = new StackPanel
                    {
                        Margin = new Thickness(20,20,20,0)
                    };

                    MainGrid.Children.Add(tempStackPanel);
                    Grid.SetColumn(tempStackPanel, 0);
                    Grid.SetRow(tempStackPanel, 1);

                    tempStackPanel.Children.Add(textBlock);
                textBlock.MouseLeftButtonDown += TextBlock_MouseLeftButtonDown;
                }
        }

        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ScrollViewer1.IsEnabled = true;
            Islands.ForEach(isl => isl.Opacity = 1.0);
            var textBlock = (TextBlock)sender;
            var stackPanel = (StackPanel)textBlock.Parent;
            stackPanel.Children.Remove(textBlock);
        }

        private void Island_MouseEnter(object sender, MouseEventArgs e)
        {
            PrizeTitelLabel.Content = ((Image)sender).Tag;
        }

        private Image CreateImage(string filePath)
        {
            ImageSource source = new BitmapImage(new Uri(filePath, UriKind.Relative));

            Image image = new Image
            {
                Source = source,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(5),
                Focusable = true,
                RenderTransformOrigin = new Point(0.5, 0.5)
            };
            // A small rendering tweak to ensure maximum visual appeal.
            RenderOptions.SetBitmapScalingMode(image, BitmapScalingMode.HighQuality);
            return image;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            var grid = (Grid)button.Parent;
            grid.Children.Remove(button);

            for (int i = 0; i < multiply; i++)
            {
                button = AddButton("Don't\nPress!");
                grid.Children.Add(button);
                Grid.SetRow(button, random.Next(0, 10));
                Grid.SetColumn(button, random.Next(0, 10));
                button.Click += Button_Click;
            }
            multiply++;
        }

        public void ColorButtons(Grid grid, int columns, int rows, double btnWidth = 100, double btnHeight = 100)
        {
            ColumnsAndRows(grid, columns, rows);
            for (int x = 0; x < columns; x++)
            {
                for (int y = 0; y < rows; y++)
                {
                    var button = AddButton("");
                    grid.Children.Add(button);
                    Grid.SetColumn(button, x);
                    Grid.SetRow(button, y);
                }
            }
        }
        public Button AddButton(string content = "")
        {
            Button button = new Button
            {
                Content = content,
                Margin = new Thickness(10),
                Padding = new Thickness(30, 10, 30, 10),
                Background = Brushes.White,
                FontSize = 20,
                
            };

            return button;
        }
        public static RowDefinition AddRow(Grid grid)
        {
            var rowDefinition = new RowDefinition();
            rowDefinition.Height = new GridLength(50);
            grid.RowDefinitions.Add(rowDefinition);
            return rowDefinition;
        }
        public static ColumnDefinition AddColumn(Grid grid)
        {
            var columnDefinition = new ColumnDefinition();
            columnDefinition.Width = new GridLength(50);
            grid.ColumnDefinitions.Add(columnDefinition);
            return columnDefinition;
        }
        public static void ColumnsAndRows(Grid grid, int columns, int rows)
        {
            for (int i = 0; i < columns; i++)
            {
                AddColumn(grid);
            }
            for (int i = 0; i < rows; i++)
            {
                AddRow(grid);
            }
        }
    }
}
