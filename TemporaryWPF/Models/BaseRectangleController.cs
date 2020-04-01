using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace TemporaryWPF.Models
{
    public class BaseRectangleController
    {
       // public Shape rectangle; 
        public BaseRectangleController()
        {
            //rectangle = model;

            Shape rectangle = new Rectangle()
            {
                Width = 10,
                Height = 10,
                Fill = Brushes.White,
                Stroke = Brushes.Black
            };

           //var o = rectangle as UIElement;
        }
    }
}
