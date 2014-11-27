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

using System.Net.NetworkInformation;
using MySql.Data.MySqlClient;

namespace Ratomir
{
	/// <summary>
	/// Interaction logic for Racun.xaml
	/// </summary>
	public partial class Racun : Window
	{
        static string connString = "Server=localhost;Database=parking;Uid=root;";
        MySqlConnection konekcija = new MySqlConnection(connString);

        int mjesto;

		public Racun(int indexMjesta)
		{
			this.InitializeComponent();

            mjesto = indexMjesta;
		}

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        private void btn7_Click(object sender, RoutedEventArgs e)
        {
            pbSifra.Password += "7";
        }

        private void btn8_Click(object sender, RoutedEventArgs e)
        {
            pbSifra.Password += "8";
        }

        private void btn9_Click(object sender, RoutedEventArgs e)
        {
            pbSifra.Password += "9";
        }

        private void btn4_Click(object sender, RoutedEventArgs e)
        {
            pbSifra.Password += "4";
        }

        private void btn5_Click(object sender, RoutedEventArgs e)
        {
            pbSifra.Password += "5";
        }

        private void btn6_Click(object sender, RoutedEventArgs e)
        {
            pbSifra.Password += "6";
        }

        private void btn1_Click(object sender, RoutedEventArgs e)
        {
            pbSifra.Password += "1";
        }

        private void btn2_Click(object sender, RoutedEventArgs e)
        {
            pbSifra.Password += "2";
        }

        private void btn3_Click(object sender, RoutedEventArgs e)
        {
            pbSifra.Password += "3";
        }

        private void btn0_Click(object sender, RoutedEventArgs e)
        {
            pbSifra.Password += "0";
        }

        private void btnPotvrdi_Click(object sender, RoutedEventArgs e)
        {
            bool pronasao = false;
            konekcija.Open();

            using(MySqlCommand komanda = new MySqlCommand("SELECT sifra FROM parking WHERE zauzeto = 1 AND brojMjesta = " + (mjesto+1) + ";", konekcija))
            using(MySqlDataReader citac = komanda.ExecuteReader())
            {
                if (citac.Read())
                {
                    if (citac["sifra"].ToString() == pbSifra.Password)
                    {
                        pronasao = true;
                    }
                }
            }

            using (MySqlCommand komanda = new MySqlCommand("UPDATE parking SET zauzeto = 0, vrijemeDolaska='00:00:00', datumDolaska=0000-00-00,sifra='' WHERE brojMjesta = " + (mjesto + 1) + ";", konekcija))
            {
                if (pronasao)
                {
                    komanda.ExecuteNonQuery();
                    this.DialogResult = true;
                }
                else
                {
                    this.DialogResult = false;
                    MessageBox.Show("Unjeli ste pogrešnu šifru ili rezervisano mjesto sa šifrom ne postoji u bazi.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            konekcija.Close();
            this.Close();
        }

        private void btnOdustani_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void btnC_Click(object sender, RoutedEventArgs e)
        {
            if (pbSifra.Password.Length > 0)
            {
                pbSifra.Password = pbSifra.Password.Remove(pbSifra.Password.Length - 1);
            }
        }

        private void btnUndo_Click(object sender, RoutedEventArgs e)
        {
            pbSifra.Password = "";
        }
	}
}