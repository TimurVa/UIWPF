using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TemporaryWPF.Models;
using TemporaryWPF.PostgreSchem;
using TemporaryWPF.RectangleControls;

namespace TemporaryWPF
{

    public partial class MainWindow : Window
    {
        public ObservableCollection<MovingControl> movingControls;
        public Line line;

        public MainWindow()
        {
            InitializeComponent();


            movingControls = new ObservableCollection<MovingControl>();
            movingControls.CollectionChanged += MovingControls_CollectionChanged;

            foreach (var k in OveralldbClass.db.Shapes)
            {
                movingControls.Add(new MovingControl(new BaseRectangleModel(k)));
            }
        }

        private void MovingControls_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)

        {
            switch (e.Action)
            {
                case System.Collections.Specialized.NotifyCollectionChangedAction.Add:
                    {
                        area.Children.Add((MovingControl)e.NewItems[0]);
                    }
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Remove:
                    {
                        area.Children.Remove(((MovingControl)e.NewItems[0]));
                    }
                    break;
            }
        }

        public void NewRectangle(object sender, RoutedEventArgs e)
        {
            AddNewRectangle();
        }

        public void AddNewRectangle()
        {
            Shapes model = new Shapes()
            {
                Toppos = 10,
                Leftpos = -40,
                Width = 80,
                Height = 80,
            };
            OveralldbClass.db.Add(model);
            OveralldbClass.db.SaveChanges();

            BaseRectangleModel baseRectangleModel = new BaseRectangleModel(model);
            movingControls.Add(new MovingControl(baseRectangleModel));
        }

        //public void AddNewControlle()
        //{
        //    Shape rectangle = new Rectangle()
        //    {
        //        Width = 20,
        //        Height = 20,
        //        Fill = Brushes.White,
        //        Stroke = Brushes.Black
        //    };
            
        //    BaseRectangleController baseRectangleController = new BaseRectangleController(rectangle);
        //    movingControls.Add(new MovingControl(baseRectangleController));
        //}

    }

}
