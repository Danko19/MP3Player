using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MP3_Player.cs
{
    public static class Vars

    {
        public static Form1 Link;

        public static List<string> Files = new List<string>();

        public static bool play = false;

        public static bool repeat = false;

        public static bool startedNew = false;

        public static bool mixed = false;

        public static bool mouseLClick = false;

        public static bool vol = true;

        public static int VolBuf;

        public static bool curr = false;

        public static string AppPath = AppDomain.CurrentDomain.BaseDirectory;

        public static int CurrentTrackNumber;

        public static string GetFileName(string file)
        {
            string[] tmp = file.Split('\\');
            return tmp[tmp.Length - 1];
        }

        public static void SetInputFormats()
        {
            Link.openFileDialog1.Filter = "Все форматы|*.mp3; *.wav"
                + "|MP3|*.mp3"
                + "|WAV|*.wav";
        }
    }
}
