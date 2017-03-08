using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
//using System.Windows.Data;
//using System.Windows.Documents;
//using System.Windows.Input;
//using System.Windows.Media;
//using System.Windows.Media.Imaging;
//using System.Windows.Navigation;
//using System.Windows.Shapes;

namespace Stratego {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();

            PopulateButtonDict();
            PopulateGroupBoxDict();
            PopulateListboxesDict();
            
            Component.Board.GameState c = (Component.Board.GameState)Entity.BoardEntity.Instance.components["GameState"];
            c.currentGameState.Enter();
        }

        private void ButtonClicked(object sender, RoutedEventArgs e) {
            Button b = (Button)sender;
            Component.Board.GameState c = (Component.Board.GameState)Entity.BoardEntity.Instance.components["GameState"];
            c.currentGameState.Execute(Int32.Parse(b.Tag.ToString()));
        }
    }
}
