using MP3_Player.cs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Un4seen.Bass;

namespace MP3_Player
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Vars.Link = this;
            BassLike.InitBass(BassLike.HZ);
            Vars.SetInputFormats();
            timer2.Enabled = true;
        }
               

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void butOpen_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            string[] tmp = openFileDialog1.FileNames;
            for(int i = 0; i < tmp.Length; i++)
            {
                Vars.Files.Add(tmp[i]);
                TagModel TM = new TagModel(tmp[i]);
                playlist.Items.Add(TM.Artist + " - " + TM.Title + " (" + TimeSpan.FromSeconds(TM.Duration).ToString("mm':'ss") + ")");
            }
            playlist.SelectedIndex = Vars.CurrentTrackNumber = 0;
        }

        private void btnPlay_Click(object sender, EventArgs e)
        {
            if((playlist.Items.Count != 0) && (playlist.SelectedIndex != -1))
            {
                if (!Vars.play)
                {
                    string current = Vars.Files[playlist.SelectedIndex];
                    Vars.CurrentTrackNumber = playlist.SelectedIndex;
                    BassLike.Play(current, BassLike.Volume);
                    label1.Text = TimeSpan.FromSeconds(BassLike.GetPosOfStream(BassLike.Stream)).ToString("mm':'ss");
                    label2.Text = TimeSpan.FromSeconds(BassLike.GetTimeOfStream(BassLike.Stream)).ToString("mm':'ss");
                    if ((Vars.startedNew))
                    {
                        TagModel TM = new TagModel(current);
                        label3.Text = (TM.Artist + " - " + TM.Title + "   ");
                    }
                    Vars.startedNew = false;
                    slTime.Maximum = BassLike.GetTimeOfStream(BassLike.Stream);
                    slTime.Value = BassLike.GetPosOfStream(BassLike.Stream);
                    timer1.Enabled = true;
                    btnPlay.BackgroundImage = Properties.Resources.player_pause_5536;
                    Vars.play = true;
                }
                else
                {
                    BassLike.Pause();
                    Vars.play = false;
                    btnPlay.BackgroundImage = Properties.Resources.player_play_1976;
                }
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            BassLike.Stop();
            timer1.Enabled = false;
            Vars.play = false;
            btnPlay.BackgroundImage = Properties.Resources.player_play_1976;
            slTime.Value = 0;
            label1.Text = "00:00";
            label3.Text = "";
            Vars.mixed = false;

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            label1.Text = TimeSpan.FromSeconds(BassLike.GetPosOfStream(BassLike.Stream)).ToString("mm':'ss");
            slTime.Value = BassLike.GetPosOfStream(BassLike.Stream);
            
            if (BassLike.AutoNextTrack())
            {
                playlist.SelectedIndex = Vars.CurrentTrackNumber;
                label1.Text = TimeSpan.FromSeconds(BassLike.GetPosOfStream(BassLike.Stream)).ToString("mm':'ss");
                label2.Text = TimeSpan.FromSeconds(BassLike.GetTimeOfStream(BassLike.Stream)).ToString("mm':'ss");
                TagModel TM = new TagModel(Vars.Files[Vars.CurrentTrackNumber]);
                label3.Text = (TM.Artist + " - " + TM.Title + "   ");
                slTime.Maximum = BassLike.GetTimeOfStream(BassLike.Stream);
                slTime.Value = BassLike.GetPosOfStream(BassLike.Stream);
            }
            
            
            if(BassLike.EndPlayList)
            {
                btnStop_Click(this, new EventArgs());
                playlist.SelectedIndex = Vars.CurrentTrackNumber = 0;
                BassLike.EndPlayList = false;
                label2.Text = "00:00";
                label3.Text = "";
            }

           
            if (label3.Text != "")
            {
                string symb;
                string tmp = label3.Text;
                symb = tmp.Substring(0, 1);
                label3.Text = (tmp.Remove(0, 1)+symb);
            }
        }

        private void slTime_Scroll(object sender, ScrollEventArgs e)
        {
            BassLike.SetPosOfScroll(BassLike.Stream, slTime.Value);
        }

        private void slVol_Scroll(object sender, ScrollEventArgs e)
        {
            BassLike.SetVolumeToStream(BassLike.Stream, slVol.Value);
            Vars.vol = true;
            pictureBox1.Image = Properties.Resources._1498657581_VolumeNormalBlue;
        }

        private void playlist_DoubleClick(object sender, EventArgs e)
        {
            if (playlist.Items.Count > 0)
            {
                BassLike.Stop();
                string current = Vars.Files[playlist.SelectedIndex];
                Vars.CurrentTrackNumber = playlist.SelectedIndex;
                BassLike.Play(current, BassLike.Volume);
                label1.Text = TimeSpan.FromSeconds(BassLike.GetPosOfStream(BassLike.Stream)).ToString("mm':'ss");
                label2.Text = TimeSpan.FromSeconds(BassLike.GetTimeOfStream(BassLike.Stream)).ToString("mm':'ss");
                TagModel TM = new TagModel(current);
                label3.Text = (TM.Artist + " - " + TM.Title + "   ");
                slTime.Maximum = BassLike.GetTimeOfStream(BassLike.Stream);
                slTime.Value = BassLike.GetPosOfStream(BassLike.Stream);
                timer1.Enabled = true;
                btnPlay.BackgroundImage = Properties.Resources.player_pause_5536;
                Vars.play = true;
                Vars.mixed = false;
            }
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            if ((Vars.CurrentTrackNumber > 0) && (!Vars.mixed) && (playlist.Items.Count > 0))
            {
                BassLike.Stop();
                btnPlay.BackgroundImage = Properties.Resources.player_pause_5536;
                Vars.play = true;
                timer1.Enabled = true;
                BassLike.ToPrevTrack();
                playlist.SelectedIndex = Vars.CurrentTrackNumber;
                label1.Text = TimeSpan.FromSeconds(BassLike.GetPosOfStream(BassLike.Stream)).ToString("mm':'ss");
                label2.Text = TimeSpan.FromSeconds(BassLike.GetTimeOfStream(BassLike.Stream)).ToString("mm':'ss");
                TagModel TM = new TagModel(Vars.Files[Vars.CurrentTrackNumber]);
                label3.Text = (TM.Artist + " - " + TM.Title + "   ");
                slTime.Maximum = BassLike.GetTimeOfStream(BassLike.Stream);
                slTime.Value = BassLike.GetPosOfStream(BassLike.Stream);
                Vars.startedNew = false;
            }    
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (Vars.mixed)
            {
                BassLike.Stop();
                playlist.SelectedIndex = Vars.CurrentTrackNumber = 0;
                BassLike.Play(Vars.Files[0], BassLike.Volume);
                btnPlay.BackgroundImage = Properties.Resources.player_pause_5536;
                Vars.play = true;
                timer1.Enabled = true;
                label1.Text = TimeSpan.FromSeconds(BassLike.GetPosOfStream(BassLike.Stream)).ToString("mm':'ss");
                label2.Text = TimeSpan.FromSeconds(BassLike.GetTimeOfStream(BassLike.Stream)).ToString("mm':'ss");
                TagModel TM = new TagModel(Vars.Files[Vars.CurrentTrackNumber]);
                label3.Text = (TM.Artist + " - " + TM.Title + "   ");
                Vars.mixed = false;
            }
            else
            {
                BassLike.Stop();
                btnPlay.BackgroundImage = Properties.Resources.player_pause_5536;
                Vars.play = true;
                timer1.Enabled = true;
                if (BassLike.ToNextTrack())
                {
                    playlist.SelectedIndex = Vars.CurrentTrackNumber;
                    label1.Text = TimeSpan.FromSeconds(BassLike.GetPosOfStream(BassLike.Stream)).ToString("mm':'ss");
                    label2.Text = TimeSpan.FromSeconds(BassLike.GetTimeOfStream(BassLike.Stream)).ToString("mm':'ss");
                    TagModel TM = new TagModel(Vars.Files[Vars.CurrentTrackNumber]);
                    label3.Text = (TM.Artist + " - " + TM.Title + "   ");
                    slTime.Maximum = BassLike.GetTimeOfStream(BassLike.Stream);
                    slTime.Value = BassLike.GetPosOfStream(BassLike.Stream);
                }

                if (BassLike.EndPlayList)
                {
                    btnStop_Click(this, new EventArgs());
                    if (playlist.Items.Count != 0)
                        playlist.SelectedIndex = Vars.CurrentTrackNumber = 0;
                    BassLike.EndPlayList = false;
                    label2.Text = "00:00";
                    label3.Text = "";
                }
            }
            Vars.startedNew = false;

        }

        private void playlist_DragDrop(object sender, DragEventArgs e)
        {
            string[] tmp = (string[])e.Data.GetData(DataFormats.FileDrop);
            for (int i = 0; i < tmp.Length; i++)
            {
                Vars.Files.Add(tmp[i]);
                TagModel TM = new TagModel(tmp[i]);
                playlist.Items.Add(TM.Artist + " - " + TM.Title + " (" + TimeSpan.FromSeconds(TM.Duration).ToString("mm':'ss") + ")");
            }
            playlist.SelectedIndex = Vars.CurrentTrackNumber = 0;
        }

        private void playlist_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop, false) == true)
            {
                e.Effect = DragDropEffects.All;
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            label4.Text = slVol.Value.ToString();
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if ((playlist.Items.Count != 0) && (playlist.SelectedIndex != -1))
            {
                int del = playlist.SelectedIndex;
                if (del == Vars.CurrentTrackNumber)
                {
                    btnStop_Click(this, new EventArgs());
                    label2.Text = "00:00";
                    label3.Text = "";
                    Vars.curr = true;
                }
                playlist.Items.Remove(playlist.SelectedItem);
                Vars.Files.RemoveAt(del);
                if (Vars.curr)
                {
                    if (del < playlist.Items.Count)
                    {
                        playlist.SelectedIndex = del;
                    }
                    else
                    {
                        playlist.SelectedIndex = del - 1;
                        Vars.CurrentTrackNumber--;
                    }   
                }
                else
                {
                    if (del < Vars.CurrentTrackNumber)
                        Vars.CurrentTrackNumber--;
                    playlist.SelectedIndex = Vars.CurrentTrackNumber;
                }
                    

            }
            Vars.curr = false;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            btnStop_Click(this, new EventArgs());
            label2.Text = "00:00";
            label3.Text = "";
            playlist.Items.Clear();
            Vars.Files.Clear();
        }

        private void btnMode_Click(object sender, EventArgs e)
        {
            if (!Vars.repeat)
            {
                Vars.repeat = true;
                btnMode.BackgroundImage = Properties.Resources._1498696044_repeat;
            }
            else
            {
                Vars.repeat = false;
                btnMode.BackgroundImage = Properties.Resources._1498696071_share_alt;
            }
            btnMode_MouseHover(this, new EventArgs());
        }

        private void btnMix_Click(object sender, EventArgs e)
        {
            if (playlist.Items.Count > 1)
            {
                Random rnd = new Random();
                int j;
                string tmp;
                for (int i = 0; i < playlist.Items.Count; i++)
                {
                    j = rnd.Next(0, playlist.Items.Count - 1);
                    tmp = playlist.Items[j].ToString();
                    playlist.Items[j] = playlist.Items[i];
                    playlist.Items[i] = tmp;
                    tmp = Vars.Files[j];
                    Vars.Files[j] = Vars.Files[i];
                    Vars.Files[i] = tmp;
                }
                Vars.mixed = true;
                playlist.SelectedIndex = 0;
            }
        }
        
        private void butOpen_MouseHover(object sender, EventArgs e)
        {
            toolTip1.SetToolTip(this.butOpen, "Add to playlist");
        }

        private void btnRemove_MouseHover(object sender, EventArgs e)
        {
            toolTip1.SetToolTip(this.btnRemove, "Remove from playlist");
        }

        private void btnClear_MouseHover(object sender, EventArgs e)
        {
            toolTip1.SetToolTip(this.btnClear, "Clear the playlist");
        }

        private void btnMix_MouseHover(object sender, EventArgs e)
        {
            toolTip1.SetToolTip(this.btnMix, "Mix the playlist");
        }

        private void btnMode_MouseHover(object sender, EventArgs e)
        {
            if (Vars.repeat)
                toolTip1.SetToolTip(this.btnMode, "Playback mode: Repeat");
            else
                toolTip1.SetToolTip(this.btnMode, "Playback mode: In order");
        }

        private void btnUp_MouseHover(object sender, EventArgs e)
        {
            toolTip1.SetToolTip(this.btnUp, "Up track in the playlist");
        }

        private void btnDown_MouseHover(object sender, EventArgs e)
        {
            toolTip1.SetToolTip(this.btnMix, "Down track in the playlist");
        }

        private void playlist_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Delete)
            {
                btnRemove_Click(this, new EventArgs());
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if(Vars.vol)
            {
                Vars.vol = false;
                Vars.VolBuf = BassLike.Volume;
                BassLike.Volume = 0;
                slVol.Value = BassLike.Volume;
                BassLike.SetVolumeToStream(BassLike.Stream, slVol.Value);
                pictureBox1.Image = Properties.Resources._1498657581_NoVol1;
            }
            else
            {
                Vars.vol = true;
                BassLike.Volume = Vars.VolBuf;
                slVol.Value = BassLike.Volume;
                BassLike.SetVolumeToStream(BassLike.Stream, slVol.Value);
                pictureBox1.Image = Properties.Resources._1498657581_VolumeNormalBlue;
            }
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            if((playlist.Items.Count != 0) && (playlist.SelectedIndex > 0))
            {
                int i = playlist.SelectedIndex;
                string tmp = playlist.Items[i - 1].ToString();
                playlist.Items[i - 1] = playlist.Items[i];
                playlist.Items[i] = tmp;
                tmp = Vars.Files[i - 1];
                Vars.Files[i - 1] = Vars.Files[i];
                Vars.Files[i] = tmp;
                Vars.mixed = true;
                playlist.SelectedIndex = i - 1;
            }
        }

        private void btnDown_Click(object sender, EventArgs e)
        {
            if ((playlist.Items.Count != 0) && (playlist.SelectedIndex < (playlist.Items.Count -1)))
            {
                int i = playlist.SelectedIndex;
                string tmp = playlist.Items[i + 1].ToString();
                playlist.Items[i + 1] = playlist.Items[i];
                playlist.Items[i] = tmp;
                tmp = Vars.Files[i + 1];
                Vars.Files[i + 1] = Vars.Files[i];
                Vars.Files[i] = tmp;
                Vars.mixed = true;
                playlist.SelectedIndex = i + 1;
            }
        }
        
    }
}
