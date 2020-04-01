using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using TemporaryWPF.PostgreSchem;

namespace TemporaryWPF.Models
{
    public class BaseRectangleModel
    {
        public Shapes model;
        public Shape shape;

        public BaseRectangleModel(Shapes mod)
        {
            model = mod;
            SolidColorBrush customcolor = new SolidColorBrush
            {
                Color = Color.FromArgb(255, 70, 130, 180)
            };

            shape = new Rectangle()
            {
                Width = (double)model.Width,
                Height = (double)model.Height,
                Fill = customcolor
            };

            Canvas.SetLeft(shape, (double)model.Leftpos);
            Canvas.SetTop(shape, (double)model.Toppos);
        }

        public void Send()
        {
            model.Toppos = (int)Canvas.GetTop(shape);
            model.Leftpos = (int)Canvas.GetLeft(shape);
            model.Width = (int)shape.Width;
            model.Height = (int)shape.Height;
        }
    }
}
