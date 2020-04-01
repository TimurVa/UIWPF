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
using TemporaryWPF.Models;

namespace TemporaryWPF.RectangleControls
{
    /// <summary>
    /// Логика взаимодействия для MovingControl.xaml
    /// </summary>
    public partial class MovingControl : UserControl
    {
        public UIElement Selected { get; set; }
        public BaseRectangleModel BaseRectangleModel { get; private set; }
        public bool captureMouse;
        public Point PrevPos;
        public bool CurrentlyMoving = false;
        public Line line;
        public Point diffPosition;


        public MovingControl()
        {
            InitializeComponent();
        }

        public MovingControl(BaseRectangleModel baseRectangleModel)
        {
            InitializeComponent();
            clear.Children.Add(baseRectangleModel.shape);
            BaseRectangleModel = baseRectangleModel;
            this.Height = (double)BaseRectangleModel.model.Height;
            this.Width = (double)BaseRectangleModel.model.Width;
            Canvas.SetLeft(this, (double)BaseRectangleModel.model.Leftpos);
            Canvas.SetTop(this, (double)BaseRectangleModel.model.Toppos);
        }
        
        public void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Selected = e.OriginalSource as UIElement;
            var ar = LogicalTreeHelper.GetParent(this);
            

           if (!Selected.IsDescendantOf(this.forAdd))
            {
                captureMouse = Selected.CaptureMouse();
                PrevPos = Mouse.GetPosition((IInputElement)ar);
            }
        }
            
        public void OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
                this.BaseRectangleModel.Send();

                OveralldbClass.db.SaveChanges();


            if (captureMouse && Selected != null)
            {
                Selected.ReleaseMouseCapture();
            }
            Selected = null;
        }

        public void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (Selected != null  && !Selected.IsDescendantOf(this.forAdd))
            {
            var ar = LogicalTreeHelper.GetParent(this);
            Point point = Mouse.GetPosition((IInputElement)ar);

                double OffsetX = point.X - PrevPos.X;
                double OffsetY = point.Y - PrevPos.Y;

                double NewX = Canvas.GetLeft(this);
                double NewY = Canvas.GetTop(this);

                NewX += OffsetX;
                NewY += OffsetY;
                Canvas.SetLeft(this, NewX);
                Canvas.SetTop(this, NewY);

                Canvas.SetLeft(this.BaseRectangleModel.shape, NewX);
                Canvas.SetTop(this.BaseRectangleModel.shape, NewY);

                PrevPos = point;
            }
        }
        
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            clear.Children.Remove(this.BaseRectangleModel.shape);
            OveralldbClass.db.Remove(this.BaseRectangleModel.model);
            OveralldbClass.db.SaveChanges();
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            Rectangle rectangle = new Rectangle()
            {
                Width = 10,
                Height = 10,
                Fill = Brushes.White,
                Stroke = Brushes.Black
            };

            Ellipse ellipse = new Ellipse()
            {
                Width = 6,
                Height = 6,
                Fill = Brushes.Black
            };

            line = new Line()
            {
                X1 = diffPosition.X,
                Y1 = diffPosition.Y,
                X2 = diffPosition.X + 50,
                Y2 = diffPosition.Y,
                Stroke = Brushes.Black,
                StrokeThickness = 4,
            };


            Canvas.SetLeft(rectangle, 2);
            Canvas.SetTop(rectangle, 2);
            Canvas.SetLeft(ellipse, 2);
            Canvas.SetTop(ellipse, 2);
            Canvas.SetLeft(line, 2);
            Canvas.SetTop(line, 2);
            forAdd.Children.Add(rectangle);
            forAdd.Children.Add(ellipse);
            forAdd.Children.Add(line);
        }

        private void ForAdd_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Selected = e.OriginalSource as UIElement;


            if (Selected != null)
            {
                captureMouse = Selected.CaptureMouse();
                diffPosition = e.GetPosition(Selected);
                var s = Canvas.GetLeft((UIElement)e.OriginalSource);
                var o = Canvas.GetTop(Selected);
            }

            ///for line
            ///
            /// 
            ///
        }

        private void ForAdd_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (captureMouse)
            {
                Selected.ReleaseMouseCapture();
            }

            /// for line
            
            
            /// 
            Selected = null;
        }

        private void ForAdd_MouseMove(object sender, MouseEventArgs e)
        {
            if (Selected != null)
            {
                if (e.OriginalSource != line)
                {
                    if (Canvas.GetLeft((UIElement)e.OriginalSource) <= this.Width - 11 &&
                        Canvas.GetTop((UIElement)e.OriginalSource) <= this.Height - 11 &&
                        Canvas.GetTop((UIElement)e.OriginalSource) >= 0 &&
                        Canvas.GetLeft((UIElement)e.OriginalSource) >= 0)
                    {
                        Canvas.SetLeft(Selected, e.GetPosition(this.forAdd).X - diffPosition.X);
                        Canvas.SetTop(Selected, e.GetPosition(this.forAdd).Y - diffPosition.Y);
                    }

                    else if (Canvas.GetLeft((UIElement)e.OriginalSource) >= this.Width - 11)
                    {
                        Canvas.SetLeft((UIElement)e.OriginalSource, this.Width - 11);
                        Selected.ReleaseMouseCapture();
                        captureMouse = false;
                        Selected = null;
                    }

                    else if (Canvas.GetTop((UIElement)e.OriginalSource) >= this.Height - 11)
                    {
                        Canvas.SetTop((UIElement)e.OriginalSource, this.Width - 11);
                        Selected.ReleaseMouseCapture();
                        captureMouse = false;
                        Selected = null;
                    }

                    else if (Canvas.GetTop((UIElement)e.OriginalSource) <= 0)
                    {
                        Canvas.SetTop((UIElement)e.OriginalSource, this.Width - 79);
                        Selected.ReleaseMouseCapture();
                        captureMouse = false;
                        Selected = null;
                    }

                    else if (Canvas.GetLeft((UIElement)e.OriginalSource) <= 0)
                    {
                        Canvas.SetLeft((UIElement)e.OriginalSource, this.Width - 79);
                        Selected.ReleaseMouseCapture();
                        captureMouse = false;
                        Selected = null;
                    }
                }
                else if (e.OriginalSource == line)
                {
                    Canvas.SetLeft(line, e.GetPosition(this.forAdd).X - diffPosition.X);
                    Canvas.SetTop(line, e.GetPosition(this.forAdd).Y - diffPosition.Y);
                }
            }
            

            ///for line probably



        }
    }
}

//Canvas.GetLeft((UIElement) e.OriginalSource) <= this.Width - 12
//&& Canvas.GetTop((UIElement) e.OriginalSource) <= this.Height - 12