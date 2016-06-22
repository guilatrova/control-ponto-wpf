﻿using ControlePonto.WPF.window.feriado;
using ControlePonto.WPF.window.ponto;
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

namespace ControlePonto.WPF.window.administracao
{
    /// <summary>
    /// Interaction logic for PainelControleWindow.xaml
    /// </summary>
    public partial class PainelControleWindow : WindowBase
    {
        public PainelControleWindow(PainelControleViewModel viewModel) : base(viewModel)
        {
            InitializeComponent();
        }

        protected override void viewRequested(object sender, framework.ViewRequestEventArgs e)
        {
            switch (e.RequestCode)
            {
                case PainelControleViewModel.VIEW_FERIADO:
                    FeriadoWindowFactory.criarCadastroFeriadoWindow().ShowDialog();
                    break;

                case PainelControleViewModel.VIEW_FOLGA:
                    PontoWindowFactory.criarFolgaWindow().ShowDialog();
                    break;

                default:
                    base.viewRequested(sender, e);
                    break;
            }

        }
    }
}
