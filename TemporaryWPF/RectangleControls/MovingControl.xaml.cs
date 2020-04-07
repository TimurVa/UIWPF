using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public Ellipse ellipse;
        public Rectangle rectangle;
        public ObservableCollection<LineControl> _lineControl;

      //  public static event EventHandler ValueChangedEvent;


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



            _lineControl = new ObservableCollection<LineControl>();
            _lineControl.CollectionChanged += _lineControl_CollectionChanged;
        }

        //private Point _StartPoint;
        //private Point _EndPoint;

        //public Point StartPoint
        //{
        //    get
        //    {
        //        return _StartPoint;
        //    }
        //    set
        //    {
        //        _StartPoint = value;
        //        // _StartPoint.Connections.Add(this);
        //        OnValueChanged();
        //    }
        //}

        //public Point EndPoint
        //{
        //    get
        //    {
        //        return _EndPoint;
        //    }
        //    set
        //    {
        //        _EndPoint = value;
        //        // _EndPoint.Connections.Add(this);
        //        OnValueChanged();
        //    }
        //}

        //protected virtual void OnValueChanged()
        //{
        //    ValueChangedEvent?.Invoke(this, EventArgs.Empty); 
        //}


        private void _lineControl_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case System.Collections.Specialized.NotifyCollectionChangedAction.Add:
                    {
                        this.forAdd.Children.Add((LineControl)e.NewItems[0]);
                    }
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Remove:
                    {
                        this.forAdd.Children.Remove(((LineControl)e.NewItems[0]));
                    }
                    break;
            }
        }

        //public enum RecBorder
        //{
        //    L, R, T, B, BR, BL, TR, TL, None
        //}

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

                var o = this._lineControl.Where(x => x.lcgrid == e.OriginalSource)
                    .FirstOrDefault();

                double OffsetX = point.X - PrevPos.X;
                double OffsetY = point.Y - PrevPos.Y;

                double NewX = Canvas.GetLeft(this);
                double NewY = Canvas.GetTop(this);

                NewX += OffsetX;
                NewY += OffsetY;
                Canvas.SetLeft(this, NewX);
                Canvas.SetTop(this, NewY);


              //  EndPoint = Mouse.GetPosition(o.line);

                Canvas.SetLeft(this.BaseRectangleModel.shape, NewX);
                Canvas.SetTop(this.BaseRectangleModel.shape, NewY);

                PrevPos = point;
            }
        }
        //delete
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
                clear.Children.Remove(this.BaseRectangleModel.shape);
                OveralldbClass.db.Remove(this.BaseRectangleModel.model);
                OveralldbClass.db.SaveChanges();
        }
        //add
        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            _lineControl.Add(new LineControl(BaseRectangleModel));
        }

        //private void ForAdd_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        //{
        //    Selected = e.OriginalSource as UIElement;

        //    if (e.OriginalSource == ellipse)
        //    {
        //        line = new Line()
        //        {
        //            X1 = Canvas.GetLeft(Selected),
        //            Y1 = Canvas.GetTop(Selected),
        //            Stroke = Brushes.Black,
        //            StrokeThickness = 2
        //        };
        //        line.CaptureMouse();
        //        diffPosition = e.GetPosition(line);
        //        forAdd.Children.Add(line);
        //    }

        //    if (Selected != null)
        //    {
        //        captureMouse = Selected.CaptureMouse();
        //        diffPosition = e.GetPosition(Selected);
        //        var s = Canvas.GetLeft((UIElement)e.OriginalSource);
        //        var o = Canvas.GetTop(Selected);
        //    }
        //    ///for line
        //    ///
        //    /// 
        //    ///
        //}

        //private void ForAdd_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        //{
        //    if (captureMouse)
        //    {
        //        Selected.ReleaseMouseCapture();
        //    }
        //    /// for line

        //    /// 
        //    Selected = null;
        //}

        //private void ForAdd_MouseMove(object sender, MouseEventArgs e)
        //{
        //    if (Selected != null)
        //    {
        //        if (e.OriginalSource != line && e.OriginalSource != ellipse)
        //        {
        //            if (Canvas.GetLeft((UIElement)e.OriginalSource) <= this.Width &&
        //                Canvas.GetTop((UIElement)e.OriginalSource) <= this.Height &&
        //                Canvas.GetTop((UIElement)e.OriginalSource) >= 0 &&
        //                Canvas.GetLeft((UIElement)e.OriginalSource) >= 0)
        //            {
        //                Canvas.SetLeft(Selected, e.GetPosition(this.forAdd).X - diffPosition.X);
        //                Canvas.SetTop(Selected, e.GetPosition(this.forAdd).Y - diffPosition.Y);

        //                Canvas.SetLeft(ellipse, e.GetPosition(this.forAdd).X - diffPosition.X + (rectangle.Width / 2));
        //                Canvas.SetTop(ellipse, e.GetPosition(this.forAdd).Y - diffPosition.Y + (rectangle.Height / 4));
        //                if (line != null)
        //                {

        //                    Canvas.SetLeft(line, e.GetPosition(this.forAdd).X - diffPosition.X);
        //                    Canvas.SetTop(line, e.GetPosition(this.forAdd).Y - diffPosition.Y);
        //                }
        //            }

        //            else if (Canvas.GetLeft((UIElement)e.OriginalSource) >= this.Width - 11)
        //            {
        //                Canvas.SetLeft((UIElement)e.OriginalSource, this.Width - rectangle.Height);
        //                Canvas.SetLeft(ellipse, this.Width - rectangle.Height);
        //                Selected.ReleaseMouseCapture();
        //                captureMouse = false;
        //                Selected = null;
        //            }

        //            else if (Canvas.GetTop((UIElement)e.OriginalSource) >= this.Height - 11)
        //            {
        //                Canvas.SetTop((UIElement)e.OriginalSource, this.Width - rectangle.Height);
        //                Canvas.SetTop(ellipse, this.Width - rectangle.Height);
        //                Selected.ReleaseMouseCapture();
        //                captureMouse = false;
        //                Selected = null;
        //            }

        //            else if (Canvas.GetTop((UIElement)e.OriginalSource) <= 0)
        //            {
        //                Canvas.SetTop((UIElement)e.OriginalSource, this.Width - 79);
        //                Canvas.SetTop(ellipse, this.Width - rectangle.Height);
        //                Selected.ReleaseMouseCapture();
        //                captureMouse = false;
        //                Selected = null;
        //            }

        //            else if (Canvas.GetLeft((UIElement)e.OriginalSource) <= 0)
        //            {
        //                Canvas.SetLeft((UIElement)e.OriginalSource, this.Width - 79);
        //                Canvas.SetLeft(ellipse, this.Width - rectangle.Height);
        //                Selected.ReleaseMouseCapture();
        //                captureMouse = false;
        //                Selected = null;
        //            }
        //        }
        //        else if (e.OriginalSource == ellipse)
        //        {
        //            line.X2 = Mouse.GetPosition(this.forAdd).X;
        //            line.Y2 = Mouse.GetPosition(this.forAdd).Y;
        //            //   Canvas.SetZIndex(line, 10000);
        //        }
        //    }
        //    /for line probably


        //    /
        //}
    }
}

//Canvas.GetLeft((UIElement) e.OriginalSource) <= this.Width - 12
//&& Canvas.GetTop((UIElement) e.OriginalSource) <= this.Height - 12