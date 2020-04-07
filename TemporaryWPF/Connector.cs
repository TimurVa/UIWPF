using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TemporaryWPF.RectangleControls;

//collect points? 
//Not used 
namespace TemporaryWPF
{
   public class Connector
    {
        private List<LineControl> connections;
        public List<LineControl> Connections
        {
            get
            {
                if (connections == null)
                    connections = new List<LineControl>();
                return connections;
            }
        }

        public event EventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
