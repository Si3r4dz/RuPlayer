using System;
using NAudio;
using NAudio.Codecs;
using Id3Lib;
using Mp3Lib;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using WMPLib;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace RuPlayer
{
    public partial class RuPlayer : Form
    {
        //zmienne do poruszania myszą
        int TogMove;
        int MvalX;
        int MvalY;
        int i = 0;
        // obiekt do odtwarzania muzyki
        WindowsMediaPlayer wplayer = new WindowsMediaPlayer();


        public RuPlayer()
        {
            InitializeComponent();
            wplayer.settings.volume = 50;
    }

        //Zamknięcie Programu
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (wyjscie != null)
            {
                wyjscie.Dispose();
                mp3.Dispose();
            }
            this.Close();
        }
        //Poruszanie oknem
        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            TogMove = 1;
            MvalX = e.X;
            MvalY = e.Y;
            

        }

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            TogMove = 0;

        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (TogMove == 1)
            {
                this.SetDesktopLocation(MousePosition.X - MvalX, MousePosition.Y - MvalY);
            }
        }
   


        //Przycisk do minimalizowania okna
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.WindowState=FormWindowState.Minimized;
        }

        
         //Przycisk Play
        private void pictureBox5_Click(object sender, EventArgs e)
        {
            if (wyjscie != null)
            {
                if (wyjscie.PlaybackState == NAudio.Wave.PlaybackState.Playing) wyjscie.Pause();
                else if (wyjscie.PlaybackState == NAudio.Wave.PlaybackState.Paused) wyjscie.Play();
                else if (wyjscie.PlaybackState == NAudio.Wave.PlaybackState.Stopped) wyjscie.Play();
            }
        
            
        }

        //Przycisk Pause
        private void pictureBox4_Click(object sender, EventArgs e)
        {
            wyjscie.Pause();
        }


        //Przycisk Stop
        private void pictureBox3_Click(object sender, EventArgs e)
        {
            if (wyjscie != null)
            {
                wyjscie.Stop();
                wyjscie.Dispose();
                mp3.Position = 0;
            }
        }





        //tablice do przechowywania nazw piosenek oraz ich ścieżek;
        string[] nazwa;

        //Zadeklarowana Lista
        List<string> sciezki = new List<string>();

        NAudio.Wave.BlockAlignReductionStream mp3 = null;
        NAudio.Wave.DirectSoundOut wyjscie = null;


       
        //Ustawianie głośności
        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            wplayer.settings.volume = trackBar1.Value;

        }

       




        private void polskiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DodajPiosenki.Text = "Dodaj piosenki";
            label2.Text = "Album";
            label4.Text = "Artysta";
            label5.Text = "Tytuł";
            button2.Text = "Wycisz";
            opcjeToolStripMenuItem.Text = "Opcje";
        }

        private void angielskiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DodajPiosenki.Text = "Add songs";
            label2.Text = "Album";
            label4.Text = "Artist";
            label5.Text = "Title";
            button2.Text = "Mute";
            opcjeToolStripMenuItem.Text = "Options";

        }

        public void button2_Click(object sender, EventArgs e)
        {
            wplayer.settings.volume = 0;
        }

  
        //Wybranie piosenki do odtwarzania
        public void ListOfSongs_DoubleClick(object sender, EventArgs e)
        {
            if (mp3 != null)
            {
                try {
                    wyjscie.Stop();
                    wyjscie.Dispose();
                    mp3.Dispose();
                    mp3.Close();
                    mp3 = null;
                }
                catch
                {
                    Console.WriteLine("Error");
                }
     
                
            }
            if (ListOfSongs != null)
            {
                
                NAudio.Wave.WaveStream pcm = new NAudio.Wave.Mp3FileReader(sciezki[ListOfSongs.SelectedIndex]);
                mp3 = new NAudio.Wave.BlockAlignReductionStream(pcm);

                wyjscie = new NAudio.Wave.DirectSoundOut();
                wyjscie.Init(mp3);
                
                wyjscie.Play();
                

                //Wypisywanie Informacji o piosence
                Mp3Lib.Mp3File file = new Mp3Lib.Mp3File(sciezki[ListOfSongs.SelectedIndex]);
                try
                {
                    label3.Text = file.TagHandler.Album;
                }
                catch
                {
                    label3.Text = null;
                }
                try
                {
                    label6.Text = file.TagHandler.Artist;
                }
                catch
                {
                    label6.Text = null;
                }
                try
                {
                    label7.Text = file.TagHandler.Title;
                    if (label7.Text == null & label6.Text == null & label3.Text==null)
                    {
                        label7.Text = nazwa[ListOfSongs.SelectedIndex];
                    }
                }
                catch
                {
                    label7.Text = null; // nazwa[ListOfSongs.SelectedIndex];
                }
                try
                {
                    pictureBox6.Image = file.TagHandler.Picture;
                    if (pictureBox6.Image == null)
                    {
                        pictureBox6.Image = Properties.Resources.audio_file_sound_wave_computer_mp3_song_sample_512;
                        pictureBox6.Refresh();
                    }
                }
                catch
                {

                    pictureBox6.Image = Properties.Resources.audio_file_sound_wave_computer_mp3_song_sample_512;
                    pictureBox6.Refresh();
                }
                
                    
                    
                    
               
                    

              
            
            } 
        }



        //Wybór piosenek
        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog okno = new OpenFileDialog();
            okno.Filter = "Plik MP3 (*.mp3)|*.mp3;";
            okno.Multiselect = true;
            if (okno.ShowDialog() == DialogResult.OK)
            {
               
                
                nazwa = okno.SafeFileNames;//Zapisuje nazwy piosenek

                string []sciezka = okno.FileNames; // Zapisuje sciezke piosenek
                for (int i = 0; i < sciezka.Length; i++)
                {
                    sciezki.Add(sciezka[i]);
                }


                //Wyświetla nazwy piosenek w textboxie
                for (i=0 ; i < nazwa.Length ; i++)
                {
                    ListOfSongs.Items.Add(nazwa[i]);
                }


               
            }
            
            
        }


        private void trackBar2_Scroll(object sender, EventArgs e)
        {

          

        }

 

        private void trackBar3_Scroll(object sender, EventArgs e)
        {
            
        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
            Form2 okienko = new Form2();
            okienko.Show();

        }
    }

}
