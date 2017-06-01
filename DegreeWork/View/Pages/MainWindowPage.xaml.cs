using DataBaseStruct;
using DegreeWork.Instances;
using DegreeWork.Model.Instances;
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
using static DataBaseStruct.Model;

namespace DegreeWork
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindowPage : Page
    {
        private const int SIZE = 1;
        private const int SHIFT = SIZE / 2;

        DB_ConnectionContext _Context;
        private Point prev;
        private SolidColorBrush color = new SolidColorBrush(Colors.Black);
        private bool isPaint = false;
        private Line line;
        private Line line2;
        private Line line3;
        private Line line4;
        private Line pointLine;
        bool _IsRoom = true;
        bool _IsPoint = false;
        int _projectId;

        public MainWindowPage(DB_ConnectionContext context, int projectId)
        {
            InitializeComponent();
            _Context = context;
            _projectId = projectId;

            List<Room> rooms = _Context.Rooms.Where(c => c.Scheme.ProjectNumber.ProjectId.Equals(_projectId)).ToList();
            
            if (rooms != null )
            {
                if (rooms.Count > 0)
                {
                    foreach (Room room in rooms)
                    {
                        int coord_X1 = room.Coord_X1;
                        int coord_Y1 = room.Coord_Y1;

                        int coord_X2 = room.Coord_X2;
                        int coord_Y2 = room.Coord_Y2;

                        int coord_X3 = room.Coord_X3;
                        int coord_Y3 = room.Coord_Y3;

                        int coord_X4 = room.Coord_X4;
                        int coord_Y4 = room.Coord_Y4;

                        line = new Line
                        {
                            Stroke = color,
                            StrokeThickness = SIZE,
                            X1 = coord_X1,
                            Y1 = coord_Y1,
                            X2 = coord_X2,
                            Y2 = coord_Y2,
                            StrokeStartLineCap = PenLineCap.Round,
                            StrokeEndLineCap = PenLineCap.Round
                        };

                        line2 = new Line
                        {
                            Stroke = color,
                            StrokeThickness = SIZE,
                            X1 = coord_X1,
                            Y1 = coord_Y1,
                            X2 = coord_X3,
                            Y2 = coord_Y3,
                            StrokeStartLineCap = PenLineCap.Round,
                            StrokeEndLineCap = PenLineCap.Round
                        };

                        line3 = new Line
                        {
                            Stroke = color,
                            StrokeThickness = SIZE,
                            X1 = coord_X3,
                            Y1 = coord_Y3,
                            X2 = coord_X4,
                            Y2 = coord_Y4,
                            StrokeStartLineCap = PenLineCap.Round,
                            StrokeEndLineCap = PenLineCap.Round
                        };

                        line4 = new Line
                        {
                            Stroke = color,
                            StrokeThickness = SIZE,
                            X1 = coord_X2,
                            Y1 = coord_Y2,
                            X2 = coord_X4,
                            Y2 = coord_Y4,
                            StrokeStartLineCap = PenLineCap.Round,
                            StrokeEndLineCap = PenLineCap.Round
                        };

                        CanvasAreaForSchemeOfRoom.Children.Add(line);
                        CanvasAreaForSchemeOfRoom.Children.Add(line2);
                        CanvasAreaForSchemeOfRoom.Children.Add(line3);
                        CanvasAreaForSchemeOfRoom.Children.Add(line4);
                    }
                }
            }
        }
        
        private void MyIP_MouseMove(object sender, MouseEventArgs e)
        {
            if (_IsRoom)
            {
                if (!isPaint) { return; }

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
        }
    
        private void MyIP_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (_IsRoom)
            {
                if (isPaint) { return; }

                isPaint = true;
                prev = Mouse.GetPosition(CanvasAreaForSchemeOfRoom);
                var dot = new Ellipse { Width = SIZE, Height = SIZE, Fill = color };
                dot.SetValue(Canvas.LeftProperty, prev.X - SHIFT);
                dot.SetValue(Canvas.TopProperty, prev.Y - SHIFT);
                CanvasAreaForSchemeOfRoom.Children.Add(dot);
            }
        }
    
        private void Window_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (_IsRoom)
            {
                if (line != null && line2 != null)
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
                }

                line = null;
                line2 = null;
                line3 = null;
                line4 = null;
                isPaint = false;
            }

            if (_IsPoint)
            {
                var point = Mouse.GetPosition(CanvasAreaForSchemeOfRoom);
                pointLine = new Line
                {
                    Stroke = new SolidColorBrush
                    {
                        Color = Colors.Red,
                    },
                    StrokeThickness = 10,
                    X1 = point.X,
                    Y1 = point.Y,
                    X2 = point.X + 1,
                    Y2 = point.Y + 1,
                    StrokeStartLineCap = PenLineCap.Round,
                    StrokeEndLineCap = PenLineCap.Round,
                  
                };
                ControlPointInst controlPoint = new ControlPointInst(
                    Convert.ToInt32(pointLine.X1), 
                    Convert.ToInt32(pointLine.Y1));

                SingleSpaceParams.getInstance().ControlPoints.Add(controlPoint);
                CanvasAreaForSchemeOfRoom.Children.Add(pointLine);
            }
        }

        private void CanvasAreaForSchemeOfRoom_Initialized(object sender, EventArgs e)
        {
            Canvas canvas = (Canvas)sender;
            canvas.Width = SingleSpaceParams.getInstance().Width;
            canvas.Height = SingleSpaceParams.getInstance().Height;
            
        }

        private void RoomEnableButton_Click(object sender, RoutedEventArgs e)
        {
            SolidColorBrush pointEbableBrush = new SolidColorBrush
            {
                Color = Colors.White
            };
            PointEnableButton.Background = pointEbableBrush;

            SolidColorBrush roomEnabledBrush = new SolidColorBrush
            {
                Color = Colors.LightBlue
            };
            RoomEnableButton.Background = roomEnabledBrush;

            _IsRoom = true;
            _IsPoint = false;
        }

        private void PointEnableButton_Click(object sender, RoutedEventArgs e)
        {
            SolidColorBrush pointEbableBrush = new SolidColorBrush
            {
                Color = Colors.LightBlue
            };
            PointEnableButton.Background = pointEbableBrush;

            SolidColorBrush roomEnabledBrush = new SolidColorBrush
            {
                Color = Colors.White
            };
            RoomEnableButton.Background = roomEnabledBrush;

            _IsRoom = false;
            _IsPoint = true;
        }

        private void StartAcommButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SchemeOfBuilding scheme = _Context.Schemes.Where(c => c.ProjectNumber.ProjectId.Equals(_projectId)).First();

                foreach (ControlPointInst cp in SingleSpaceParams.getInstance().ControlPoints)
                {
                    ControlPoint point = new ControlPoint();
                    point.Coord_X = cp.X1;
                    point.Coord_Y = cp.Y1;
                    scheme.Point.Add(point);
                }
                //_Context.Schemes.Add(scheme);

                foreach (RectangleRoom rm in SingleSpaceParams.getInstance().Rooms)
                {
                    Room room = new Room();
                    room.Coord_X1 = rm.X1;
                    room.Coord_Y1 = rm.Y1;

                    room.Coord_X2 = rm.X2;
                    room.Coord_Y2 = rm.Y2;

                    room.Coord_X3 = rm.X3;
                    room.Coord_Y3 = rm.Y3;

                    room.Coord_X4 = rm.X4;
                    room.Coord_Y4 = rm.Y4;
                    room.Scheme = scheme;
                    _Context.Rooms.Add(room);
                
                   // scheme.Rooms.Add(room);
                    
                }
               
                _Context.SaveChanges();
            }
            catch(Exception ex)
            {
                MessageBox.Show("Ошибка обновления базы данных!");
            }
        }
    }
}
