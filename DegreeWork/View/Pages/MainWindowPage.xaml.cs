using DegreeWork.Instances;
using DegreeWork.SpaceParam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DegreeWork
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindowPage : Page
    {
        private const int SIZE = 1;
        private const int SHIFT = SIZE / 2;

        private Point prev;
        private SolidColorBrush color = new SolidColorBrush(Colors.Black);
        private bool isPaint = false;
        private Line line;
        private Line line2;
        private Line line3;
        private Line line4;

        public MainWindowPage()
        {
            InitializeComponent();
        }
    
        private void MyIP_MouseMove(object sender, MouseEventArgs e)
        {
            if (!isPaint) return;
           if (line != null && line2 != null && line3 != null && line4 != null)
            {
               CanvasAreaForSchemeOfRoom.Children.Remove(line);
               CanvasAreaForSchemeOfRoom.Children.Remove(line2);
               CanvasAreaForSchemeOfRoom.Children.Remove(line3);
               CanvasAreaForSchemeOfRoom.Children.Remove(line4);
            }

            var point = Mouse.GetPosition(CanvasAreaForSchemeOfRoom);
            line = new Line
            {
                Stroke = color,
                StrokeThickness = SIZE,
                X1 = prev.X,
                Y1 = prev.Y,
                X2 = point.X,
                Y2 = prev.Y,
                StrokeStartLineCap = PenLineCap.Round,
                StrokeEndLineCap = PenLineCap.Round
            };

            line2 = new Line
            {
                Stroke = color,
                StrokeThickness = SIZE,
                X1 = prev.X,
                Y1 = prev.Y,
                X2 = prev.X,
                Y2 = point.Y,
                StrokeStartLineCap = PenLineCap.Round,
                StrokeEndLineCap = PenLineCap.Round
            };

            line3 = new Line
            {
                Stroke = color,
                StrokeThickness = SIZE,
                X1 = prev.X,
                Y1 = point.Y,
                X2 = point.X,
                Y2 = point.Y,
                StrokeStartLineCap = PenLineCap.Round,
                StrokeEndLineCap = PenLineCap.Round
            };

            line4 = new Line
            {
                Stroke = color,
                StrokeThickness = SIZE,
                X1 = point.X,
                Y1 = prev.Y,
                X2 = point.X,
                Y2 = point.Y,
                StrokeStartLineCap = PenLineCap.Round,
                StrokeEndLineCap = PenLineCap.Round
            };

            CanvasAreaForSchemeOfRoom.Children.Add(line);
            CanvasAreaForSchemeOfRoom.Children.Add(line2);
            CanvasAreaForSchemeOfRoom.Children.Add(line3);
            CanvasAreaForSchemeOfRoom.Children.Add(line4);
        }
    
        private void MyIP_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (isPaint) return;
            isPaint = true;
            prev = Mouse.GetPosition(CanvasAreaForSchemeOfRoom);
            var dot = new Ellipse { Width = SIZE, Height = SIZE, Fill = color };
            dot.SetValue(Canvas.LeftProperty, prev.X - SHIFT);
            dot.SetValue(Canvas.TopProperty, prev.Y - SHIFT);
            CanvasAreaForSchemeOfRoom.Children.Add(dot);
        }
    
        private void Window_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            RectangleRoom room = new RectangleRoom
            {
                X1 = Convert.ToInt32(line.X1),
                Y1 = Convert.ToInt32(line.Y1),

                X2 = Convert.ToInt32(line.X2),
                Y2 = Convert.ToInt32(line.Y2),

                X3 = Convert.ToInt32(line2.X2),
                Y3 = Convert.ToInt32(line2.Y2),

                X4 = Convert.ToInt32(line4.X2),
                Y4 = Convert.ToInt32(line4.Y2)
            };
            SingleSpaceParams.getInstance().Rooms.Add(room); 

            line = null;
            line2 = null;
            line3 = null;
            line4 = null;
            isPaint = false;
        }
    }
}
