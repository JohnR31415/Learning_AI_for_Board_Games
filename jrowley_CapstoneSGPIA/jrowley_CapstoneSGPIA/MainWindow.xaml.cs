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

namespace jrowley_CapstoneSGPIA
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<TerritoryToken> tokens;
        public MainWindow()
        {
            InitializeComponent();
            addTokens();
        }

        public void addTokens()
        {
            tokens = new List<TerritoryToken>();
            for (int i = 0; i < tokenEllipses.Items.Count; i++)
            {
                tokens.Add(new TerritoryToken((Ellipse)tokenEllipses.Items[i], (Label)tokenNames.Items[i], (Label)tokenArmies.Items[i]));
                Grid.Children.Add(tokens[i].ellipse);
                Grid.Children.Add(tokens[i].name);
                Grid.Children.Add(tokens[i].armies);
            }
        }

        public List<TerritoryToken> getTokens()
        {
            return tokens;
        }
    }
}
