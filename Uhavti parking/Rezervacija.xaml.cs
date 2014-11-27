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
	/// Interaction logic for Rezervacija.xaml
	/// </summary>
	public partial class Rezervacija : Window
	{
        string pravaSifra = null;

		public Rezervacija(int broj, string sifra, ParkingMjesto rezervisanoMjesto)
		{
			this.InitializeComponent();

            tbBrojMjesta.Text = broj.ToString();
            txtSifra.Text = "⓰⓰⓰⓰";

            ParkingMjesto mjesto = new ParkingMjesto();
            mjesto.tbDatum = rezervisanoMjesto.tbDatum;
            mjesto.tbVrijeme = rezervisanoMjesto.tbVrijeme;
            mjesto.tbZauzeto = rezervisanoMjesto.tbZauzeto;
            mjesto.imgSlika = rezervisanoMjesto.imgSlika;
            mjesto.borGranica = rezervisanoMjesto.borGranica;
            mjesto.Background = rezervisanoMjesto.Background;

            mjesto.Margin = new Thickness(5, 5, 0, 0);

            pravaSifra = sifra;

            gridGlavni.Children.Add(mjesto);

            this.UpdateLayout();
		}

        private void btnPrikaziSifru_MouseEnter(object sender, MouseEventArgs e)
        {
            txtSifra.Text = pravaSifra;
        }

        private void btnPrikaziSifru_MouseLeave(object sender, MouseEventArgs e)
        {
            txtSifra.Text = "⓰⓰⓰⓰";
        }


	}
}