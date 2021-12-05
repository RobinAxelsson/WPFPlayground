using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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
    public partial class MainWindow : Window
    {
        
        Random random = new Random();
        ScrollViewer Root;
        double ColorWidth = 60;
        double ColorHeight = 60;
        int columns = 5;
        int rows = 5;
        public static string TextfilesPath { get; } = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + "\\TextFiles\\";
        public MainWindow()
        {
            InitializeComponent();
            Start();
        }

        private void Start()
        {
            Title = "ColorButtons";
            SizeToContent = SizeToContent.WidthAndHeight;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;

            ScrollViewer root = new ScrollViewer();
            root.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            Content = root;
            Root = root;

            var grid = new Grid();
            root.Content = grid;
            grid.Margin = new Thickness(5);
            grid.ShowGridLines = true;
            ColumnsAndRows(grid, 1, 2);

            int iButtonGrid = 0;
            grid.RowDefinitions[iButtonGrid].Height = GridLength.Auto;
            var colorButtonGrid = new Grid();
            //colorButtonGrid.ShowGridLines = true;
            ColorButtons(colorButtonGrid, columns, rows, ColorWidth, ColorHeight);          
            grid.Children.Add(colorButtonGrid);
            Grid.SetColumn(colorButtonGrid, iButtonGrid);
            Grid.SetRow(colorButtonGrid, iButtonGrid);

            var txtBox = new TextBox();
            txtBox.Text = "Color-content:";
            grid.Children.Add(txtBox);
            Grid.SetRow(txtBox, 1);

            var buttons = colorButtonGrid.Children;
            var button1 = (Button)buttons[0];
            var color1 = (SolidColorBrush)button1.Background;

            foreach (Button button in buttons)
            {
                button.Click += Button_Click;
                button.MouseEnter += Button_MouseEnter;
            }
            
        }


        private void Button_MouseEnter(object sender, MouseEventArgs e)
        {
            var grid = (Grid)Root.Content;
            var TxtBox = (TextBox)grid.Children[1];
            var button = (Button)sender;
            var color = (SolidColorBrush)button.Background;
            var R = color.Color.R;
            var G = color.Color.G;
            var B = color.Color.B;

            TxtBox.Text = $"R{R}, G{G}, B{B}";            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var grid = (Grid)Root.Content;
            var TxtBox = (TextBox)grid.Children[1];
            var colorButtonGrid = (Grid)grid.Children[0];

            File.AppendAllText(TextfilesPath + "colorcode.csv", TxtBox.Text + Environment.NewLine);
            colorButtonGrid.Children.Clear();
            ColorButtons(colorButtonGrid, columns, rows, ColorWidth, ColorHeight);

            foreach (Button b in colorButtonGrid.Children)
            {
                b.Click += Button_Click;
                b.MouseEnter += Button_MouseEnter;
            }
        }


        
        public void ColorButtons(Grid grid, int columns, int rows, double btnWidth = 100, double btnHeight = 100)
        {
            ColumnsAndRows(grid, columns, rows);
            for (int x = 0; x < columns; x++)
            {
                for (int y = 0; y < rows; y++)
                {
                    var button = AddButton("", btnWidth, btnHeight);
                    grid.Children.Add(button);
                    Grid.SetColumn(button, x);
                    Grid.SetRow(button, y);                    
                }
            }
        }
        public Button AddButton(string content = "", double width = 100, double height = 100)
        {
            Button button = new Button
            {
                Content = content,
                //Margin = new Thickness(5),
                //Padding = new Thickness(5),
                Height = height,
                Width = width,
                Background = RandomColor()
            };

            return button;
        }
        public RowDefinition AddRow(Grid grid)
        {
            var rowDefinition = new RowDefinition();
            rowDefinition.Height = GridLength.Auto;
            grid.RowDefinitions.Add(rowDefinition);
            return rowDefinition;
        }
        public ColumnDefinition AddColumn(Grid grid)
        {
            var columnDefinition = new ColumnDefinition();
            columnDefinition.Width = GridLength.Auto;
            grid.ColumnDefinitions.Add(columnDefinition);
            return columnDefinition;
        }
        private Brush RandomColor()
        {            
            return new SolidColorBrush(Color.FromRgb((byte)random.Next(1, 255), (byte)random.Next(1, 255), (byte)random.Next(1, 233)));
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
