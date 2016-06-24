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
using System.Windows.Shapes;

namespace ControlePonto.WPF.window.relatorio
{
    /// <summary>
    /// Interaction logic for RelatorioWindow.xaml
    /// </summary>
    public partial class RelatorioWindow : WindowBase
    {
        public RelatorioWindow(RelatorioViewModel viewModel)
            : base(viewModel)
        {
            InitializeComponent();
        }
    }
}
