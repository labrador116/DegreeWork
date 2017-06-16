using DataBaseStruct;
using DegreeWork.ChromosomeModel;
using DegreeWork.Container;
using DegreeWork.Instances;
using DegreeWork.Model.Instances;
using DegreeWork.Service;
using DegreeWork.SpaceParam;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    public partial class MainWindowPage : Window
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
        List<ModelsOfModules> _Models;
        BackgroundWorker _BackgroundWorker;
        Chromosome _Chromosome;
        Point _Point;

        public MainWindowPage(DB_ConnectionContext context, int projectId)
        {
            InitializeComponent();
            _Context = context;
            _projectId = projectId;
            _BackgroundWorker = ((BackgroundWorker)this.FindResource("backgroundWorker"));
            InitializeAllObjects();
            if (SingleSpaceParams.getInstance().SumOfChromosomeInPopulation == 0)
            {
                SingleSpaceParams.getInstance().SumOfChromosomeInPopulation = 8;
            }
            if (SingleSpaceParams.getInstance().PropabilityOfMutation == 0)
            {
                SingleSpaceParams.getInstance().PropabilityOfMutation = 0.5;
            }

            SingleSpaceParams.getInstance().TheBestResolve = 1;
            SingleSpaceParams.getInstance().ModulesRadius.Clear();
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
                
                _Point = Mouse.GetPosition(CanvasAreaForSchemeOfRoom);
                line = new Line
                {
                    Stroke = color,
                    StrokeThickness = SIZE,
                    X1 = prev.X,
                    Y1 = prev.Y,
                    X2 = _Point.X,
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
                    Y2 = _Point.Y,
                    StrokeStartLineCap = PenLineCap.Round,
                    StrokeEndLineCap = PenLineCap.Round
                };

                line3 = new Line
                {
                    Stroke = color,
                    StrokeThickness = SIZE,
                    X1 = prev.X,
                    Y1 = _Point.Y,
                    X2 = _Point.X,
                    Y2 = _Point.Y,
                    StrokeStartLineCap = PenLineCap.Round,
                    StrokeEndLineCap = PenLineCap.Round
                };

                line4 = new Line
                {
                    Stroke = color,
                    StrokeThickness = SIZE,
                    X1 = _Point.X,
                    Y1 = prev.Y,
                    X2 = _Point.X,
                    Y2 = _Point.Y,
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

                widthRoomTextBlock.Visibility = Visibility.Visible;
                widthRoomTextBox.Visibility = Visibility.Visible;
                lengthRoomTextBlock.Visibility = Visibility.Visible;
                lengthRoomTextBox.Visibility = Visibility.Visible;
                setLengthAndWidthButton.Visibility = Visibility.Visible;

                pressEnterLabel.Visibility = Visibility.Visible;
                CanvasAreaForSchemeOfRoom.Focusable = true;
                CanvasAreaForSchemeOfRoom.Focus();
                
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
                   // SingleSpaceParams.getInstance().Rooms.Add(room);
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
            if (SingleSpaceParams.getInstance().ModulesRadius.Count > 0)
            {
                try
                {
                    SchemeOfBuilding scheme = _Context.Schemes.Where(c => c.ProjectNumber.ProjectId.Equals(_projectId)).First();

                    foreach (ControlPointInst cp in SingleSpaceParams.getInstance().ControlPoints)
                    {
                        ControlPoint point = new ControlPoint();
                        point.Coord_X = cp.X1;
                        point.Coord_Y = cp.Y1;

                        bool isExist = false;

                        foreach (ControlPoint c in scheme.Point)
                        {
                            if(c.Coord_X==point.Coord_X && c.Coord_Y == point.Coord_Y)
                            {
                                isExist = true;
                            }
                        }

                        if (!isExist)
                        {
                            scheme.Point.Add(point);
                        }
                    }

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

                        bool isExist = false;

                        foreach(Room r in scheme.Rooms)
                        {
                            if(r.Coord_X1 == room.Coord_X1
                                && r.Coord_Y1 == room.Coord_Y1
                                && r.Coord_X4 == room.Coord_X4
                                && r.Coord_Y4 == room.Coord_Y4)
                            {
                                isExist = true;
                            }
                        }

                        if (!isExist)
                        {
                            scheme.Rooms.Add(room);
                        }
                    }
                    _Context.SaveChanges();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка обновления базы данных!");
                }

                geneticAlgProgressBar.Visibility = Visibility.Visible;
                progressTextStateLabel.Visibility = Visibility.Visible;

               
                StartAcommButton.IsEnabled = false;

                ExecuteService service = new ExecuteService(SingleSpaceParams.getInstance().ModulesRadius, _BackgroundWorker);
                _BackgroundWorker.RunWorkerAsync(service);
            }
        }

        private void CanvasAreaForSchemeOfRoom_KeyUp(object sender, KeyEventArgs e)
        {
            if (_IsRoom)
            {
                if (e.Key == Key.Enter)
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
                    pressEnterLabel.Visibility = Visibility.Collapsed;

                }

                if (e.Key == Key.Escape)
                {
                    if (line != null && line2 != null && line3 != null && line4 != null)
                    {
                        CanvasAreaForSchemeOfRoom.Children.Remove(line);
                        CanvasAreaForSchemeOfRoom.Children.Remove(line2);
                        CanvasAreaForSchemeOfRoom.Children.Remove(line3);
                        CanvasAreaForSchemeOfRoom.Children.Remove(line4);
                    }
                    isPaint = false;
                }
            }
        }
    
        private void selectWiFimodelsButton_Click(object sender, RoutedEventArgs e)
        {
            ChooseModelOfModule dialogChose = new ChooseModelOfModule(_Models);
            dialogChose.SendSelected += DialogChose_SendSelected;
            dialogChose.ShowDialog();
        }

        private void DialogChose_SendSelected(ModelsOfModules module)
        {
            SingleSpaceParams.getInstance().ModulesRadius.Add(int.Parse(module.ModelRadius));
            selectedModelsOfModulesLabel.Text += "\n\t" + module.ModelName;
        }

        private void probabilityOfMutationTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!Char.IsDigit(e.Text, 0) && e.Text!=",") e.Handled = true;
        }

        private void sumOfChromosome_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!Char.IsDigit(e.Text, 0)) e.Handled = true;
        }

        private void InitializeAllObjects()
        {
            SchemeOfBuilding scheme=null;
            try
            {
                 scheme = _Context.Schemes.Where(c => c.ProjectNumber.ProjectId.Equals(_projectId)).First();
            }
            catch
            {
                MessageBox.Show("Ошибка загрузки данных из базы данных");
            }
            List<Room> rooms = null;
            if (scheme.Rooms != null)
            {
                if (scheme.Rooms.Count() > 0)
                {
                    rooms = scheme.Rooms;

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

                        RectangleRoom rectRoom = new RectangleRoom();
                        rectRoom.X1 = room.Coord_X1;
                        rectRoom.Y1 = room.Coord_Y1;
                        rectRoom.X2 = room.Coord_X2;
                        rectRoom.Y2 = room.Coord_Y2;
                        rectRoom.X3 = room.Coord_X3;
                        rectRoom.Y3 = room.Coord_Y3;
                        rectRoom.X4 = room.Coord_X4;
                        rectRoom.Y4 = room.Coord_Y4;
                        SingleSpaceParams.getInstance().Rooms.Add(rectRoom);
                    }
                }
            }

            if (scheme.Point != null)
            {
                List<ControlPoint> points = null;
                if (scheme.Point.Count > 0)
                {
                    points = scheme.Point;

                    foreach (ControlPoint point in points)
                    {
                        pointLine = new Line
                        {
                            Stroke = new SolidColorBrush
                            {
                                Color = Colors.Red,
                            },
                            StrokeThickness = 10,
                            X1 = point.Coord_X,
                            Y1 = point.Coord_Y,
                            X2 = point.Coord_X + 1,
                            Y2 = point.Coord_Y + 1,
                            StrokeStartLineCap = PenLineCap.Round,
                            StrokeEndLineCap = PenLineCap.Round,
                        };
                        CanvasAreaForSchemeOfRoom.Children.Add(pointLine);

                        ControlPointInst controlPoint = new ControlPointInst(
                    Convert.ToInt32(point.Coord_X),
                    Convert.ToInt32(point.Coord_Y));

                        SingleSpaceParams.getInstance().ControlPoints.Add(controlPoint);
                    }
                }
            }
            
            if (scheme.Positions != null)
            {
                try
                {
                    List<InstallationPosition> positions = scheme.Positions;

                    foreach (InstallationPosition pos in positions)
                    {
                        PlacmentOfModules placement = pos.Placment.First();
                        ModelsOfModules model = _Context.Models.Where(mod => mod.ModuleId == placement.ModelId).First();

                        EllipseGeometry el = new EllipseGeometry
                        {
                            Center = new Point(pos.Coord_X, pos.Coord_Y),
                            RadiusX = double.Parse(model.ModelRadius),
                            RadiusY = double.Parse(model.ModelRadius)
                        };
                        Path path = new Path();
                        path.Data = el;
                        path.StrokeThickness = 2;
                        path.Opacity = 0.5;
                        path.Fill = new SolidColorBrush
                        {
                            Color = Colors.LightBlue,
                            Opacity = 0.7
                        };
                        path.Stroke = new SolidColorBrush
                        {
                            Color = Colors.Red
                        };
                        CanvasAreaForSchemeOfRoom.Children.Add(path);
                    }
                }
                catch
                {
                    MessageBox.Show("Ошибка загрузки данных из базы данных");
                }
            }
            _Models = _Context.Models.ToList();
        }

        private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            ExecuteService service = (ExecuteService)e.Argument;
            e.Result= service.Start();
        }
        private void BackgroundWorker_ProgressChanged_1(object sender, ProgressChangedEventArgs e)
        {
            geneticAlgProgressBar.Value = e.ProgressPercentage;
            progressTextStateLabel.Content = e.UserState;
        }

        private void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
             _Chromosome = (Chromosome)e.Result;
            double resultSum = 0;

            foreach (Gene gene in _Chromosome.Container)
            {
                EllipseGeometry el = new EllipseGeometry
                {
                    Center = new Point(gene.OX, gene.OY),
                    RadiusX = gene.Radius,
                    RadiusY=gene.Radius
                };
                Path path = new Path();
                path.Data = el;
                path.StrokeThickness = 2;
                path.Opacity = 0.5;
                path.Fill = new SolidColorBrush
                {
                    Color = Colors.LightBlue,
                    Opacity = 0.7
                };
                path.Stroke = new SolidColorBrush
                {
                    Color = Colors.Red
                };
                resultSum += gene.CoverageOfArea;
                CanvasAreaForSchemeOfRoom.Children.Add(path);
            }

            percentResultTexBox.Visibility = Visibility.Visible;
            percentResultTexBox.Text = "Результат покрытия: " + resultSum ;
        }

        private void saveMenuItem_Click(object sender, RoutedEventArgs e)
        {
            SchemeOfBuilding scheme = _Context.Schemes.Where(c => c.ProjectNumber.ProjectId.Equals(_projectId)).First();
            List<ModelsOfModules> models = _Context.Models.ToList();

            try
            {
                if (SingleSpaceParams.getInstance().ControlPoints.Count > 0)
                        {
                            foreach (ControlPointInst cp in SingleSpaceParams.getInstance().ControlPoints)
                            {
                                ControlPoint point = new ControlPoint();
                                point.Coord_X = cp.X1;
                                point.Coord_Y = cp.Y1;

                                bool isExist = false;

                                foreach (ControlPoint c in scheme.Point)
                                {
                                    if (c.Coord_X == point.Coord_X && c.Coord_Y == point.Coord_Y)
                                    {
                                        isExist = true;
                                    }
                                }

                                if (!isExist)
                                {
                                    scheme.Point.Add(point);
                                }
                            }
                        }

                        if (SingleSpaceParams.getInstance().Rooms.Count > 0)
                        {
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

                                bool isExist = false;

                                foreach (Room r in scheme.Rooms)
                                {
                                    if (r.Coord_X1 == room.Coord_X1
                                        && r.Coord_Y1 == room.Coord_Y1
                                        && r.Coord_X4 == room.Coord_X4
                                        && r.Coord_Y4 == room.Coord_Y4)
                                    {
                                        isExist = true;
                                    }
                                }

                                if (!isExist)
                                {
                                    scheme.Rooms.Add(room);
                                }
                            }
                        }
                        
                if (_Chromosome != null)
                {
                    foreach (Gene gene in _Chromosome.Container)
                    {
                        PlacmentOfModules pm = new PlacmentOfModules();

                        pm.ModelId = (from m in models where m.ModelRadius.Equals(gene.Radius.ToString()) select m).First().ModuleId;

                        InstallationPosition position = new InstallationPosition();
                        position.Coord_X = gene.OX;
                        position.Coord_Y = gene.OY;
                        position.Placment = new List<PlacmentOfModules>();
                        position.Placment.Add(pm);

                        scheme.Positions.Add(position);
                    }
                }
                _Context.SaveChanges();
            }
            catch
            {
                MessageBox.Show("Ошибка! Данные не были сохранены в базу данных.");
            }
            MessageBox.Show("Результат сохранен.");
        }

        private void deleteAll_Click(object sender, RoutedEventArgs e)
        {
            CanvasAreaForSchemeOfRoom.Children.Clear();
            try
            {
                SchemeOfBuilding scheme = _Context.Schemes.Where(c => c.ProjectNumber.ProjectId.Equals(_projectId)).First();
                List<ControlPoint> points = new List<ControlPoint>(scheme.Point);

                if (points.Count > 0)
                {
                    foreach (ControlPoint point in points)
                    {
                        _Context.Points.Remove(point);
                    }
                }

                List<Room> rooms = new List<Room>(scheme.Rooms);
                if (rooms.Count > 0)
                {
                    foreach (Room room in rooms)
                    {
                        _Context.Rooms.Remove(room);
                    }
                }

                List<InstallationPosition> positions = new List<InstallationPosition>(scheme.Positions);
                if (positions.Count > 0)
                {
                    foreach (InstallationPosition position in positions)
                    {
                        _Context.InstallationPositions.Remove(position);
                    }
                }

                SingleSpaceParams.getInstance().Rooms.Clear();
                SingleSpaceParams.getInstance().ControlPoints.Clear();
                _Context.SaveChanges();
            }
            catch
            {
                MessageBox.Show("Ошибка! Данные не были изменены.");
            }
        }

        private void getOut_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void createNewProjectMenuIItem_Click(object sender, RoutedEventArgs e)
        {
            this.Content = new CreateProjectPage();
        }

        private void OpenExistProjectMenuitem_Click(object sender, RoutedEventArgs e)
        {
            this.Content = new OpenExistProjectPage();
        }

        private void deleteCurrentProjectItemView_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Project project = _Context.Projects.Where(s => s.ProjectId.Equals(_projectId)).First();

                List<SchemeOfBuilding> scheme = project.Scheme;

                if (scheme != null)
                {
                    _Context.Schemes.Remove(scheme.ElementAt(0));
                }

                if (project != null)
                {
                    _Context.Projects.Remove(project);
                }

                _Context.SaveChanges();
                this.Close();
            }
            catch
            {
                MessageBox.Show("Ошибка! Данные не были удалены.");
            }
        }

        private void setSumOfChromosomesMenuItem_Click(object sender, RoutedEventArgs e)
        {
            new SettingSumOfPopulation().ShowDialog();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            new SettingProbabilityOfMutation().ShowDialog();
        }

        private void FuncByPopulationRelationship_Click(object sender, RoutedEventArgs e)
        {
            if (SingleSpaceParams.getInstance().RatingByOrder.Count > 0)
            {
                new ResultCharts(SingleSpaceParams.getInstance().RatingByOrder).Show();
            }
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            
        }

        private void setLengthAndWidthButton_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrEmpty(widthRoomTextBox.Text) && !String.IsNullOrEmpty(lengthRoomTextBox.Text))
            {
                _Point.X = prev.X + double.Parse(widthRoomTextBox.Text);
                _Point.Y = prev.Y + double.Parse(lengthRoomTextBox.Text);

                if (line != null && line2 != null && line3 != null && line4 != null)
                {
                    CanvasAreaForSchemeOfRoom.Children.Remove(line);
                    CanvasAreaForSchemeOfRoom.Children.Remove(line2);
                    CanvasAreaForSchemeOfRoom.Children.Remove(line3);
                    CanvasAreaForSchemeOfRoom.Children.Remove(line4);
                }

                line = new Line
                {
                    Stroke = color,
                    StrokeThickness = SIZE,
                    X1 = prev.X,
                    Y1 = prev.Y,
                    X2 = _Point.X,
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
                    Y2 = _Point.Y,
                    StrokeStartLineCap = PenLineCap.Round,
                    StrokeEndLineCap = PenLineCap.Round
                };

                line3 = new Line
                {
                    Stroke = color,
                    StrokeThickness = SIZE,
                    X1 = prev.X,
                    Y1 = _Point.Y,
                    X2 = _Point.X,
                    Y2 = _Point.Y,
                    StrokeStartLineCap = PenLineCap.Round,
                    StrokeEndLineCap = PenLineCap.Round
                };

                line4 = new Line
                {
                    Stroke = color,
                    StrokeThickness = SIZE,
                    X1 = _Point.X,
                    Y1 = prev.Y,
                    X2 = _Point.X,
                    Y2 = _Point.Y,
                    StrokeStartLineCap = PenLineCap.Round,
                    StrokeEndLineCap = PenLineCap.Round
                };

                CanvasAreaForSchemeOfRoom.Children.Add(line);
                CanvasAreaForSchemeOfRoom.Children.Add(line2);
                CanvasAreaForSchemeOfRoom.Children.Add(line3);
                CanvasAreaForSchemeOfRoom.Children.Add(line4);
            }
        }

        private void widthRoomTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!Char.IsDigit(e.Text, 0) && e.Text != ",") e.Handled = true;
        }

        private void lengthRoomTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!Char.IsDigit(e.Text, 0) && e.Text != ",") e.Handled = true;
        }

        private void win_KeyUp(object sender, KeyEventArgs e)
        {
            if (_IsRoom)
            {
                if (e.Key == Key.Enter)
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
                    pressEnterLabel.Visibility = Visibility.Collapsed;

                }

                if (e.Key == Key.Escape)
                {
                    if (line != null && line2 != null && line3 != null && line4 != null)
                    {
                        CanvasAreaForSchemeOfRoom.Children.Remove(line);
                        CanvasAreaForSchemeOfRoom.Children.Remove(line2);
                        CanvasAreaForSchemeOfRoom.Children.Remove(line3);
                        CanvasAreaForSchemeOfRoom.Children.Remove(line4);
                    }
                    isPaint = false;
                }
            }
        }
    }
}
