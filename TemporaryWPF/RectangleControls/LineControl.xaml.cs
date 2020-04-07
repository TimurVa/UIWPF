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
    /// Логика взаимодействия для LineControl.xaml
    /// </summary>
    public partial class LineControl : UserControl
    {
        public static event EventHandler ValueChangedEvent;


        public UIElement Selected { get; set; }
        public Point diffPosition;
        public BaseRectangleModel _baseRectangleModel { get; set; }
        public Line line;


        private Point _StartPoint;
        private Point _EndPoint;

        public Point StartPoint
        {
            get
            {
                return _StartPoint;
            }
            set
            {
                _StartPoint = value;
                // _StartPoint.Connections.Add(this);
                OnValueChanged();
            }
        }

        public Point EndPoint
        {
            get
            {
                return _EndPoint;
            }
            set
            {
                _EndPoint = value;
                // _EndPoint.Connections.Add(this);
                OnValueChanged();
            }
        }


        //public Connector _startPoint;
        //public Connector _endPoint;

        //public Connector StartPoint
        //{
        //    get
        //    {
        //        return _startPoint;
        //    }
        //    set
        //    {
        //        if (_startPoint != value)
        //        {
        //            if (_startPoint != null)
        //            {
        //                _startPoint.PropertyChanged -= this.UpdateLine_MouseMove;
        //                _startPoint.Connections.Remove(this);
        //            }

        //            _startPoint = value;

        //            if (_startPoint != null)
        //            {
        //                _startPoint.Connections.Add(this);
        //                _startPoint.PropertyChanged += this.UpdateLine_MouseMove;
        //            }

        //           // UpdatePathGeometry();
        //        }
        //    }
        //}

        //public Connector EndPoint
        //{
        //    get { return _endPoint; }
        //    set
        //    {
        //        if (_endPoint != value)
        //        {
        //            if (_endPoint != null)
        //            {
        //                _endPoint.PropertyChanged -= this.UpdateLine_MouseMove;
        //                _endPoint.Connections.Remove(this);
        //            }

        //            _endPoint = value;

        //            if (_endPoint != null)
        //            {
        //                _endPoint.Connections.Add(this);
        //                _endPoint.PropertyChanged += this.UpdateLine_MouseMove;
        //            }
        //           // UpdatePathGeometry();
        //        }
        //    }
        //}


        public LineControl()
        {
            InitializeComponent();
        }

        public LineControl(BaseRectangleModel baseRectangleModel)
        {
            InitializeComponent();
            _baseRectangleModel = baseRectangleModel;

            this.Height = (double)_baseRectangleModel.model.Height / 4;
            this.Width = (double)_baseRectangleModel.model.Width / 4;
            Canvas.SetLeft(this, (double)baseRectangleModel.model.Width - this.Width);
            Canvas.SetTop(this, ((double)baseRectangleModel.model.Height - this.Height) / 2);
           
        }

        protected virtual void OnValueChanged()
        {
            ValueChangedEvent?.Invoke(this, EventArgs.Empty); //reg event
        }

        private void Lcgrid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            
            var ar = LogicalTreeHelper.GetParent(this);
            Selected = e.OriginalSource as UIElement;


            if (e.OriginalSource == lcellipse)
            {
               // StartPoint = Mouse.GetPosition(this.lcgrid);

                line = new Line()
                {
                    X1 = Mouse.GetPosition(this.lcgrid).X,
                    Y1 = Mouse.GetPosition(this.lcgrid).Y,
                    Stroke = Brushes.Black,
                    StrokeThickness = 2
                };
                line.CaptureMouse();
                diffPosition = e.GetPosition(line);
                lcgrid.Children.Add(line);



                //UIElement element = sender as UIElement;
                //if (element == null)
                //    return;
                //DataObject datObj = new DataObject(this);
                //DragDrop.DoDragDrop(element, datObj, DragDropEffects.Move);
            }

            if (Selected != null)
            {
                Selected.CaptureMouse();
                diffPosition = e.GetPosition((IInputElement)ar);
            }
        }

        private void Lcgrid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
                if (Selected.CaptureMouse())
                {
                   // OnValueChanged();
                    Selected.ReleaseMouseCapture();
                }
                Selected = null;
            //line.X2 = Mouse.GetPosition(this).X + 100;
            //line.Y2 = Mouse.GetPosition(this).Y + 100;
        }

        private void Lcgrid_MouseMove(object sender, MouseEventArgs e)
        {
            if (Selected != null)
            {
                if (e.OriginalSource != line && e.OriginalSource != lcellipse)
                {
                    var ar = LogicalTreeHelper.GetParent(this);
                    Point point = Mouse.GetPosition((IInputElement)ar);

                    double OffsetX = point.X - diffPosition.X;
                    double OffsetY = point.Y - diffPosition.Y;

                    double NewX = Canvas.GetLeft(this);
                    double NewY = Canvas.GetTop(this);

                    NewX += OffsetX;
                    NewY += OffsetY;
                    Canvas.SetLeft(this, NewX);
                    Canvas.SetTop(this, NewY);

                    // Canvas.SetLeft(this.lcgrid, NewX);
                    //  Canvas.SetTop(this.lcgrid, NewY);
                    LineControl.ValueChangedEvent += this.UpdateLine_MouseMove;

                    StartPoint = Mouse.GetPosition(this);
                    
                    diffPosition = point;

                }
                else if (e.OriginalSource == lcellipse && Selected != null)
                {
                    EndPoint = Mouse.GetPosition(this);

                    line.X2 = EndPoint.X;
                    line.Y2 = EndPoint.Y;
                }
            }
        }

        public void UpdateLine_MouseMove(object sender, EventArgs e)
        {
            if (line != null)
            {
                line.X2 = EndPoint.X;
                line.Y2 = EndPoint.Y;
            }
            
        }

        //delete line
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
           // delete logic here
        }

        private void UserControl_Drop(object sender, DragEventArgs e)
        {
            FrameworkElement elem = sender as FrameworkElement;
            if (null == elem)
                return;
            IDataObject data = e.Data;
            if (!data.GetDataPresent(typeof(Line)))
                return;
            Line node = data.GetData(typeof(Line)) as Line;
            if (null == node)
                return;

        }
    }
}
