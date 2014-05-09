using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace jrowley_CapstoneSGPIA
{
    public class TerritoryToken : System.Windows.UIElement
    {
        public Ellipse ellipse;
        public Label name;
        public Label armies;
        public string strName;

        public TerritoryToken(Ellipse elipse, Label name, Label armies)
        {
            this.ellipse = elipse;
            this.name = name;
            this.armies = armies;
        }
        public TerritoryToken() { }
    }
}
