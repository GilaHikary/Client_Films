using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.IO;
using System.Threading;
using Vlc.DotNet.Forms;

using Vlc.DotNet.Core;

using Vlc.DotNet.Core.Interops.Signatures;

namespace Client_Films
{
    public partial class Movie : DevExpress.XtraEditors.XtraForm
    {
     
        static OneFilm One;
        static bool f = false;
        static Film fiilm;
        public Movie(OneFilm oneFilm, Film film)
        {
            fiilm = film;
            InitializeComponent();
            One = oneFilm;
        }

        private void Movie_Load(object sender, EventArgs e)
        {
            vlcControl1.Play(new Uri("http://localhost:5000/api/Film/Movie?id="+fiilm.FId));
            
        }

        private void vlcControl1_VlcLibDirectoryNeeded(object sender, Vlc.DotNet.Forms.VlcLibDirectoryNeededEventArgs e)
        {
            var currentDirectory = Environment.CurrentDirectory;
            // Default installation path of VideoLAN.LibVLC.Windows
            e.VlcLibDirectory =
                new DirectoryInfo(Path.Combine(currentDirectory, "libvlc", IntPtr.Size == 4 ? "win-x86" : "win-x64"));

        }

        private void button1_Click(object sender, EventArgs e)
        {
            vlcControl1.Pause();
        }

        private void vlcControl1_Click(object sender, EventArgs e)
        {

        }

        private void vlcControl1_LengthChanged(object sender, Vlc.DotNet.Core.VlcMediaPlayerLengthChangedEventArgs e)
        {
            if(!f)
            label3.Invoke(new Action(() => label3.Text = TimeSpan.FromMilliseconds(e.NewLength).ToString(@"hh\:mm\:ss")));
        }

        private void vlcControl1_TimeChanged(object sender, VlcMediaPlayerTimeChangedEventArgs e)
        {
            if(!f)
            label1.Invoke(new Action(() => label1.Text = TimeSpan.FromMilliseconds(e.NewTime).ToString(@"hh\:mm\:ss")));
        }

        private void Movie_FormClosed(object sender, FormClosedEventArgs e)
        {
            f = true;
            One.Show();

        }
    }
    }
