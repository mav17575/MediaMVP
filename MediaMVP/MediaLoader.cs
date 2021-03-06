﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MediaMVP
{
    class MediaLoader : INotifyPropertyChanged
    {
        public String path;
        private bool volumeV;
        public bool VolumeV
        {
            get { return volumeV; }
            set { volumeV = value; OnPropertyChanged(); }
        }
        private bool speedV;
        public bool SpeedV
        {
            get { return speedV; }
            set { speedV = value; OnPropertyChanged(); }
        }
        public ASCIIEncoding enc = new System.Text.ASCIIEncoding();
        public MediaLoader()
        {
            //GetMedia("");
            // Media = GetMediaENum("C:/Users/Granit/Desktop/KONFIG", new List<string> { ".mp3",".mp4"});
            /*FreeExtensions = new ObservableCollection<Extension>();
            UsedExtensions = new ObservableCollection<Extension>();
            UsedExtensions.Add(new Extension(".mp3"));
            UsedExtensions.Add(new Extension(".mp4"));*/
            usedextensions = new ObservableCollection<Extension>() { };
            extensions = new ObservableCollection<Extension>() { };
            UsedExtensions.Add(new Extension(".mp3"));
            UsedExtensions.Add(new Extension(".mp4"));
            UsedExtensions.Add(new Extension(".asf"));
            UsedExtensions.Add(new Extension(".wma"));
            UsedExtensions.Add(new Extension(".wmv"));
            UsedExtensions.Add(new Extension(".aac"));
            UsedExtensions.Add(new Extension(".3gp"));
            UsedExtensions.Add(new Extension(".3g2"));
            UsedExtensions.Add(new Extension(".m4a"));
            UsedExtensions.Add(new Extension(".m4b"));
            UsedExtensions.Add(new Extension(".mpg"));
            UsedExtensions.Add(new Extension(".mpeg"));
            UsedExtensions.Add(new Extension(".divx"));
            UsedExtensions.Add(new Extension(".xvid"));
            UsedExtensions.Add(new Extension(".h264"));
            String se = "";
            String fe = "All Media Files (";
            String te = "";
            foreach (Extension ext in UsedExtensions)
            {
                te += "*" + ext.Name+";";
                se += (ext.Name.Remove(0, 1)) + " (*" + ext.Name + ")|" + "*" + ext.Name + "|";
            }
            int c = te.Count();
            te = te.Remove(c - 1, 1);
            fe += te + ")|";
            c = se.Count();
            extString = fe+te+"|"+se.Remove(c - 1, 1);
            sources = new ObservableDictionary<string,ObservableCollection<Media>>();
            path = "../../Settings/Settings.config";
            String[] settings = File.ReadAllLines(path);
            String df = settings.Length>0 ? settings[0]:null;
            ObservableCollection<Media> o =null;
            if (df != null) o = GetMediaENum(df,usedextensions);
            sources.Add("Path", o ?? new ObservableCollection<Media>());
        }

        public static String extString;

        private ObservableDictionary<string,ObservableCollection<Media>> sources;
        public ObservableDictionary<string,ObservableCollection<Media>> Sources
        {
            get { return sources; }
            set { sources = value; OnPropertyChanged(); }
        }

        private ObservableCollection<Extension> freeextensions;
        public ObservableCollection<Extension> FreeExtensions
        {
            get { return freeextensions; }
            set { freeextensions = value; OnPropertyChanged(); }
        }

        private ObservableCollection<Extension> usedextensions;
        public ObservableCollection<Extension> UsedExtensions
        {
            get { return usedextensions; }
            set { usedextensions = value; OnPropertyChanged(); }
        }

        private ObservableCollection<Extension> extensions;
        public ObservableCollection<Extension> Extensions
        {
            get { return extensions; }
            set { extensions = value; OnPropertyChanged(); }
        }

        private ObservableCollection<Media> media;
        public ObservableCollection<Media> Media
        {
            get { return media; }
            set { media = GetMedia(value,UsedExtensions); OnPropertyChanged(); }
        }

        private Media cmedia;
        public Media CMedia
        {
            get { return cmedia; }
            set { cmedia = value; OnPropertyChanged(); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
       
        public static ObservableCollection<Media> GetMediaENum(String path, ObservableCollection<Extension> ext)
        {
            /*var myFiles = Directory.EnumerateFiles(path, "*.*", SearchOption.AllDirectories)
                 .Where(s => ext.Contains(Path.GetExtension(s)));
            */ObservableCollection<Media> res = new ObservableCollection<Media>(){ };
            try
            {

                foreach (Extension fileExtension in ext)
                {
                    foreach (String file in Directory.EnumerateFiles(path, "*.*", SearchOption.AllDirectories))
                    {
                        if (fileExtension.Name.Contains(Path.GetExtension(file))) res.Add(new Media(file));
                    }
                }
                return res;
            }
            catch (Exception e)
            {
                return res;
            }
        }

        public static ObservableCollection<Media> GetFiles(String path)
        {
            /*var myFiles = Directory.EnumerateFiles(path, "*.*", SearchOption.AllDirectories)
                 .Where(s => ext.Contains(Path.GetExtension(s)));
            */
            if (path == null) return null;
            ObservableCollection<Media> res = new ObservableCollection<Media>() { };
                foreach (String file in Directory.EnumerateFiles(path, "*.*", SearchOption.AllDirectories))
                {
                  res.Add(new Media(file));
                }
            return res;
        }

        public static ObservableCollection<Media> GetMedia(ObservableCollection<Media> files,ObservableCollection<Extension> ext)
        {
            if (files == null) return null;
            ObservableCollection<Media> res = new ObservableCollection<Media>() { };
            foreach (Extension fileExtension in ext)
            {
                if(fileExtension.Selected)
                foreach (Media m in files)
                {
                    if (fileExtension.Name.Contains(Path.GetExtension(m.MPath))) res.Add(m);
                }
            }
            return res;
        }

        public void LoadExtensions()
        {
            Extensions.Clear();
            foreach (Extension e in UsedExtensions)
            {
                if(e.Selected)Extensions.Add(e);
            }
        }

        public void ReloadMedia(ObservableCollection<Media> m)
        {
            Media = m;
        }


        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

