using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PrivateIslandDragDrop
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Start();
        }

        Label PrizeTitelLabel;
        StackPanel IslandStackpanel;
        StackPanel RightPanel;
        Image SelectedIsland;
        ScrollViewer ScrollViewer1;
        List<Image> Islands = new List<Image>();
        Grid MainGrid = new Grid();
        Canvas MyCanvas = new Canvas();
        Point clickPosition;
        bool IsInsidePanel;
        Point IslandPosition;
        Point Opoint = new Point(0, 0);
        private void Start()
        {
            Title = "Private Islands";
            WindowState = WindowState.Maximized;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;


            var rootCanvas = new Canvas();
            rootCanvas.Name = "MyCanvas";
            MyCanvas = rootCanvas;
            Content = rootCanvas;

            var mainGrid = new Grid
            {
                ShowGridLines = true,
                Focusable = true,
                Margin = new Thickness(5),
                Name = "MainGrid"
            };

            MainGrid = mainGrid;
            rootCanvas.Children.Add(mainGrid);
            Canvas.SetLeft(mainGrid, 0);
            Canvas.SetTop(mainGrid, 0);

            ImageSource source = new BitmapImage(new Uri("background.jpg", UriKind.Relative));

            ImageBrush imageBrush = new ImageBrush { ImageSource = source };
            ImageBrush imageBrushTitle = new ImageBrush { ImageSource = source };
            rootCanvas.Background = imageBrush;

            ColumnsAndRows(mainGrid, columns: 2, rows: 3);
            mainGrid.RowDefinitions[0].Height = GridLength.Auto;
            mainGrid.RowDefinitions[1].Height = new GridLength(900);
            mainGrid.ColumnDefinitions[0].Width = GridLength.Auto;
            mainGrid.ColumnDefinitions[1].Width = GridLength.Auto;

            imageBrush.Opacity = 0.8;

            var title = new Label
            {
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
            scrollViewer1.Name = "scrollarnIslands";
            ScrollViewer1 = scrollViewer1;
            mainGrid.Children.Add(scrollViewer1);
            Grid.SetColumn(scrollViewer1, 0);
            Grid.SetRow(scrollViewer1, 1);

            var mainImagesStack = new StackPanel { Orientation = Orientation.Vertical, Background = Brushes.Beige };
            mainImagesStack.Name = "IslandsStackpanel";
            scrollViewer1.Content = mainImagesStack;
            IslandStackpanel = mainImagesStack;
            ScrollViewer.SetVerticalScrollBarVisibility(scrollViewer1, ScrollBarVisibility.Hidden);

            var prizeTitelLabel = new Label
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(10, 0, 0, 0),
                Content = "",
                FontSize = 20
            };
            PrizeTitelLabel = prizeTitelLabel;

            mainGrid.Children.Add(prizeTitelLabel);
            Grid.SetColumn(prizeTitelLabel, 0);
            Grid.SetRow(prizeTitelLabel, 2);

            Image island1 = CreateImage("island1.png");
            Image island2 = CreateImage("island2.png");
            Image island3 = CreateImage("island3.png");

            Islands.Add(island1);
            Islands.Add(island2);
            Islands.Add(island3);
            mainImagesStack.Children.Add(island1);
            mainImagesStack.Children.Add(island2);
            mainImagesStack.Children.Add(island3);

            var rightPanel = new StackPanel
            {
                Orientation = Orientation.Vertical,
                Background = Brushes.Black,
                Focusable = true,
                Name = "RightPanel",
                Width = 200,
                AllowDrop = true
            };
            RightPanel = rightPanel;
            
            mainGrid.Children.Add(rightPanel);
            Grid.SetColumn(rightPanel, 1);
            Grid.SetRow(rightPanel, 1);

            foreach (var island in Islands)
            {
                island.MouseLeftButtonDown += CustomDragAndDrop_MouseLeftButtonDown;
            }

        }

        private void CustomDragAndDrop_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            clickPosition = e.GetPosition(this);
            var island = new Image();
            island = (Image)sender;
            var canvas = MyCanvas;
            Point islandPosition = island.PointToScreen(Opoint);
            IslandPosition = islandPosition;

            ImageSource source = new BitmapImage(new Uri(island.Source.ToString()));
            Image dragIsl = new Image{Source = source};

            canvas.Children.Add(dragIsl);
            Canvas.SetLeft(dragIsl, clickPosition.X-dragIsl.Width);
            Canvas.SetTop(dragIsl, clickPosition.Y-200);
            SelectedIsland = island;
            dragIsl.MouseMove += CustomDragAndDrop_MouseMove;
        }
        private void CustomDragAndDrop_MouseMove(object sender, MouseEventArgs e)
        {
            var island = sender as Image;
     
            Point currentPosition = e.GetPosition(this);
            Point rightPanelPosition = RightPanel.PointToScreen(Opoint);
            Point rightPanelEndPosition = new Point(rightPanelPosition.X + RightPanel.ActualWidth, rightPanelPosition.Y + RightPanel.ActualHeight);

            var transform = island.RenderTransform as TranslateTransform ?? new TranslateTransform();
            transform.X = currentPosition.X - clickPosition.X;
            transform.Y = currentPosition.Y - clickPosition.Y;
            island.RenderTransform = new TranslateTransform(transform.X, transform.Y);

     
            bool isInsideXrange = currentPosition.X >= rightPanelPosition.X && currentPosition.X <= rightPanelEndPosition.X;
            bool isInsideYrange = currentPosition.Y >= rightPanelPosition.Y && currentPosition.Y <= rightPanelEndPosition.Y;
            bool isInsidePanel = isInsideXrange && isInsideYrange;
            IsInsidePanel = isInsidePanel;

            island.MouseLeftButtonUp += CustomDragAndDrop_MouseLeftButtonUp;

        }
        private void CustomDragAndDrop_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var island = sender as Image;
            if (island == null) return;
            MyCanvas.Children.Remove(island);

            if (IsInsidePanel)
            {
                var parent = (StackPanel)SelectedIsland.Parent;
                parent.Children.Remove(SelectedIsland);
                RightPanel.Children.Add(SelectedIsland);
            }
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
        public RowDefinition AddRow(Grid grid)
        {
            var rowDefinition = new RowDefinition();
            rowDefinition.Height = new GridLength(50);
            grid.RowDefinitions.Add(rowDefinition);
            return rowDefinition;
        }
        public ColumnDefinition AddColumn(Grid grid)
        {
            var columnDefinition = new ColumnDefinition();
            columnDefinition.Width = new GridLength(50);
            grid.ColumnDefinitions.Add(columnDefinition);
            return columnDefinition;
        }
        public void ColumnsAndRows(Grid grid, int columns, int rows)
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
