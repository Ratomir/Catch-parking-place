using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using MySql.Data.MySqlClient;
using Ratomir;
using Microsoft.Expression.Controls;

namespace Uhavti_parking
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static string connString = "Server=localhost;Database=parking;Uid=root;";
        MySqlConnection konekcija = new MySqlConnection(connString);

        public MainWindow()
        {
            InitializeComponent();

            for (int i = 1; i <= 65; i++)
            {
                ParkingMjesto mjesto = new ParkingMjesto();
                mjesto.tbBrojMjesta.Text = "" + i;
                mjesto.TabIndex = i;
                plbParking.Items.Add(mjesto);
            }

            konekcija.Open();

            //using (MySqlCommand komada = new MySqlCommand())
            //{
            //    komada.Connection = konekcija;

            //    for (int i = 32; i <= 65; i++)
            //    {
            //        komada.CommandText = "INSERT INTO parking (brojMjesta, zauzeto) VALUES ('" + i + "', '0');";
            //        komada.ExecuteNonQuery();
            //    }
            //}

            using (MySqlCommand komanda = new MySqlCommand("SELECT * FROM parking WHERE zauzeto = 1;", konekcija))
            using (MySqlDataReader citac = komanda.ExecuteReader())
            {
                while (citac.Read())
                {
                    ((ParkingMjesto)(plbParking.Items[(int)citac["brojMjesta"] - 1])).tbVrijeme.Text = citac["vrijemeDolaska"].ToString();
                    ((ParkingMjesto)(plbParking.Items[(int)citac["brojMjesta"] - 1])).tbDatum.Text = citac["datumDolaska"].ToString();

                    RezervisiMjesto((int)citac["brojMjesta"] - 1);
                }
            }

            konekcija.Close();
        }

        private void RezervisiMjesto(int index)
        {
            ((ParkingMjesto)(plbParking.Items[index])).tbZauzeto.Text = "Zauzeto";

            ((ParkingMjesto)(plbParking.Items[index])).tbVrijeme.Padding = new Thickness(0, 1, 0, 0);
            ((ParkingMjesto)(plbParking.Items[index])).borGranica.BorderBrush = Brushes.Red;
            ((ParkingMjesto)(plbParking.Items[index])).borGranica.Background = Brushes.Red;
            ((ParkingMjesto)(plbParking.Items[index])).tbZauzeto.Text = "Zauzeto";
            ((ParkingMjesto)(plbParking.Items[index])).imgSlika.Source = new BitmapImage(new Uri("Zauzeto.jpg", UriKind.Relative));
        }

        private void plbParking_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (plbParking.SelectedIndex >= 0)
            {
                if (((ParkingMjesto)(plbParking.SelectedItem)).tbZauzeto.Text == "Slobodno")
                {
                    SatnicaWindow satnica = new SatnicaWindow();

                    if (satnica.ShowDialog() == true)
                    {
                        konekcija.Open();

                        Random random = new Random();
                        string sifra = random.Next(1000, 9999).ToString();

                        using (MySqlCommand komanda = new MySqlCommand("UPDATE parking SET zauzeto = 1, vrijemeDolaska = CURTIME(), datumDolaska = CURDATE(), sifra = " + sifra + " WHERE brojMjesta = " + (plbParking.SelectedIndex + 1) + ";", konekcija))
                        {
                            komanda.ExecuteNonQuery();

                            ((ParkingMjesto)(plbParking.Items[plbParking.SelectedIndex])).tbVrijeme.Text = DateTime.Now.ToString("HH:mm:ss");
                            ((ParkingMjesto)(plbParking.Items[plbParking.SelectedIndex])).tbDatum.Text = DateTime.Now.ToString("dd-MMM-yy");

                            RezervisiMjesto(plbParking.SelectedIndex);
                        }

                        konekcija.Close();

                        Rezervacija rezervacija = new Rezervacija(plbParking.SelectedIndex + 1, sifra, (ParkingMjesto)(plbParking.Items[plbParking.SelectedIndex]));

                        /*
                         * Glupo!!!!!!!!
                         * */
                        if (rezervacija.ShowDialog() == true)
                        { }
                        else
                        { }
                    }
                }
                else
                {
                    Racun racun = new Racun(plbParking.SelectedIndex);

                    if (racun.ShowDialog() == true)
                    {
                        /*
                         * Pitati profesora za konvertovanje datuma.
                         * */
                        DateTime datumDolaska = new DateTime();
                        datumDolaska = DateTime.ParseExact("14-May-27 " + ((ParkingMjesto)(plbParking.Items[plbParking.SelectedIndex])).tbVrijeme.Text, "yy-MMM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);

                        int index = plbParking.SelectedIndex;

                        string vrijeme = ((ParkingMjesto)(plbParking.Items[index])).tbVrijeme.Text;
                        string datum = ((ParkingMjesto)(plbParking.Items[index])).tbDatum.Text;
                        plbParking.Items[plbParking.SelectedIndex] = new ParkingMjesto();
                        ((ParkingMjesto)(plbParking.Items[index])).tbBrojMjesta.Text = "" + (index + 1);
                        plbParking.UpdateLayout();

                        MessageBox.Show("Ukupna cijena koju morate da platite za rezervaciju \nod " + vrijeme + " " + datum + "\ndo " + DateTime.Now.ToString("HH:mm:ss dd-MMM-yy") + "\nje " + Math.Round((DateTime.Now - datumDolaska).TotalSeconds * (1 / 3600), 2).ToString() + " KM.\nNa izlazu će Vas sačekati račun.\nHvala Vam što koristite naš parking servis.", "Račun", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }

                plbParking.SelectedIndex = -1;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
