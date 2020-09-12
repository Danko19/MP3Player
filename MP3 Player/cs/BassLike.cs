﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Un4seen.Bass;

namespace MP3_Player.cs
{
    public static class BassLike
    {
        public static int HZ = 44100;
        public static bool InitDefaultDevice;
        public static int Stream;
        public static int Volume = 100;
        private static bool isStoped = true;
        public static bool EndPlayList;
       
        

        public static bool InitBass(int hz)
        {
            if (!InitDefaultDevice)
            {
                InitDefaultDevice = Bass.BASS_Init(-1, hz, BASSInit.BASS_DEVICE_DEFAULT, IntPtr.Zero);
                if(InitDefaultDevice)
                {
                    Bass.BASS_PluginLoad(Vars.AppPath + @"Plugins\xmp-wv.dll");   
                }
            }
                return InitDefaultDevice;
        }

        public static void Play(string filename, int vol)
        {
            if (Bass.BASS_ChannelIsActive(Stream) != BASSActive.BASS_ACTIVE_PAUSED)
            {
                Stop();
                Vars.startedNew = true;                
                if (InitBass(HZ))
                {
                    Stream = Bass.BASS_StreamCreateFile(filename, 0, 0, BASSFlag.BASS_DEFAULT);
                    if (Stream != 0)
                    {
                        Volume = vol;
                        Bass.BASS_ChannelSetAttribute(Stream, BASSAttribute.BASS_ATTRIB_VOL, Volume / 100F);
                        Bass.BASS_ChannelPlay(Stream, false);
                    }
                }
            }
            else
                Bass.BASS_ChannelPlay(Stream, false);
            isStoped = false;
        }

        public static void Pause()
        {
            if(Bass.BASS_ChannelIsActive(Stream) == BASSActive.BASS_ACTIVE_PLAYING)
                Bass.BASS_ChannelPause(Stream);
        }

        public static void Stop()
        {
            Bass.BASS_ChannelStop(Stream);
            Bass.BASS_StreamFree(Stream);
            isStoped = true;
        }

        public static int GetTimeOfStream(int stream)
        {
            long TimeBytes = Bass.BASS_ChannelGetLength(stream);
            double Time = Bass.BASS_ChannelBytes2Seconds(stream, TimeBytes);
            return (int)Time;
        }
        public static int GetPosOfStream(int stream)
        {
            long pos = Bass.BASS_ChannelGetPosition(stream);
            int posSec = (int)Bass.BASS_ChannelBytes2Seconds(stream, pos);
            return posSec;
        }

        public static void SetPosOfScroll(int stream, int pos)
        {
            Bass.BASS_ChannelSetPosition(stream, (double)pos);
        }

        public static void SetVolumeToStream(int stream, int vol)
        {
            Volume = vol;
            Bass.BASS_ChannelSetAttribute(stream, BASSAttribute.BASS_ATTRIB_VOL, Volume / 100F);
        }

        public static bool AutoNextTrack()
        {
            if((Bass.BASS_ChannelIsActive(Stream) == BASSActive.BASS_ACTIVE_STOPPED) && (!isStoped))
            {
                if (Vars.repeat)
                {
                    RepeatTrack();
                    return true;
                }

                else
                {
                    if (Vars.mixed)
                    {
                        Vars.CurrentTrackNumber = 0;
                        Play(Vars.Files[Vars.CurrentTrackNumber], Volume);                        
                        Vars.mixed = false;
                        return true;
                    }
                    else
                    { 
                        if (Vars.Files.Count > Vars.CurrentTrackNumber + 1)
                        {
                            Play(Vars.Files[++Vars.CurrentTrackNumber], Volume);
                            EndPlayList = false;
                            return true;
                        }
                        else
                            EndPlayList = true;
                    }
                }
            }
            return false;
        }

        public static bool ToNextTrack()
        {
            if (Vars.Files.Count > Vars.CurrentTrackNumber + 1)
            {
                Play(Vars.Files[++Vars.CurrentTrackNumber], Volume);
                EndPlayList = false;
                return true;
            }
            else
                EndPlayList = true;
            return false;
        }

        public static void ToPrevTrack()
        {
            Play(Vars.Files[--Vars.CurrentTrackNumber], Volume);                           
        }

        public static void RepeatTrack()
        {
            Play(Vars.Files[Vars.CurrentTrackNumber], Volume);
        }
    }
}
