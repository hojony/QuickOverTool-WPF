﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;

namespace QuickDataTool
{
    public partial class MainWindow : Window
    {
        public string About
        {
            get
            {
                return "QuickDataTool is made by xyx0826.\n" +
                    "This project can be found at:\n" +
                    "https://github.com/xyx0826/QuickOverTool-WPF/tree/datatool. \n\n" +
                    "Thanks dynaomi, zingballyhoo, SombraOW and Js41637 for making OverTool toolchain, the ultimate toolset for extracting Overwatch game assets.\n\n" +
                    "If you need a GUI for OverTool-ing pre-1.14 game, please consider checking out Yernemm's OverTool GUI.\n" +
                    "You can find Yernemm's GUI at:\n" +
                    "https://yernemm.xyz/projects/OverToolGUI.";

            }
            set { }
        }

        public string Warning
        {
            get
            {
                return "Warning:\n" +
                    "New builds may be UNSTABLE.\n" +
                    "They may contain bugs, or be totally broken.\n" +
                    "Please only update when you know what will happen.\n\n";
            }
            set { }
        }

        public string[] DTInfo
        {
            get
            {
                return Networking.GetDTInfo();
            }
            set { }
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }

        private void DownloadNewDataTool(object sender, RequestNavigateEventArgs e)
        {
            string zipPath = ".\\datatool_" + Networking.GetDTInfo()[0] + ".zip";
            // Download new build from appveyor
            uistring.UpdateButton = "Downloading...";
            using (WebClient wc = new WebClient())
            {
                wc.DownloadProgressChanged += wc_DownloadProgressChanged;
                wc.DownloadFileAsync(new Uri(e.Uri.AbsoluteUri), zipPath);
                wc.DownloadFileCompleted += new AsyncCompletedEventHandler(Unzip);
            }
        }

        private void Unzip(object sender, AsyncCompletedEventArgs e)
        {
            string zipPath = ".\\datatool_" + Networking.GetDTInfo()[0] + ".zip";
            // Read zip content and remove old build
            List<string> files = new List<string>();
            ZipArchive zip = ZipFile.OpenRead(zipPath);
            foreach (ZipArchiveEntry file in zip.Entries)
            {
                File.Delete(".\\" + file.Name);
            }
            try
            {
                ZipFile.ExtractToDirectory(zipPath, ".\\");
            }
            catch { }
            uistring.UpdateButton = "Update successful.";
        }

        private void wc_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            uistring.DownloadProgress = e.ProgressPercentage;
            uistring.Rebind(null);
        }
    }
}
