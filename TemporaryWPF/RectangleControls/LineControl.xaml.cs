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
        public UIElement Selected { get; set; }
        public Point diffPosition;
        public BaseRectangleModel _baseRectangleModel { get; set; }
        public Line line;

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

            // LCCanvas.Children.Add(rectangle);
            //  LCCanvas.Children.Add(ellipse);

        }

        private void Lcgrid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var ar = LogicalTreeHelper.GetParent(this);
            Selected = e.OriginalSource as UIElement;

            if (e.OriginalSource == lcellipse)
            {
                line = new Line()
                {
                    X1 = Mouse.GetPosition(this.lcgrid).X,
                    Y1 = Mouse.GetPosition(this.lcgrid).Y,
                    Stroke = Brushes.Black,
                    StrokeThickness = 2
                };
                line.CaptureMouse();
                Canvas.SetZIndex(line, 100);
                diffPosition = e.GetPosition(line);
                lcgrid.Children.Add(line);
            }

            if (Selected != null)
            {
                Selected.CaptureMouse();
                diffPosition = e.GetPosition((IInputElement)ar);
            }
        }

        private void Lcgrid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (Selected != null)
            {
                if (Selected.CaptureMouse())
                {
                    Selected.ReleaseMouseCapture();
                }
                Selected = null;
            }
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

                    Canvas.SetLeft(this.lcgrid, NewX);
                    Canvas.SetTop(this.lcgrid, NewY);

                    diffPosition = point;
                }
                else if (e.OriginalSource == lcellipse && Selected != null)
                {
                    line.X2 = Mouse.GetPosition(this.lcgrid).X;
                    line.Y2 = Mouse.GetPosition(this.lcgrid).Y;
                }

            }

        }
    }
}
