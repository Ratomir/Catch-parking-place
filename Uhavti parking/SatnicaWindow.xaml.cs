using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Uhavti_parking
{
	/// <summary>
	/// Interaction logic for SatnicaWindow.xaml
	/// </summary>
	public partial class SatnicaWindow : Window
	{
		public SatnicaWindow()
		{
			this.InitializeComponent();
			
			// Insert code required on object creation below this point.
		}

        private void btnRezervisi_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }

        private void btnOdustani_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
	}
}