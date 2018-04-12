﻿using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Threading;

namespace QuickDataTool
{
    public class Logging : INotifyPropertyChanged
    {
        private static Logging _uniqueInstance;
        private static readonly object _threadLock = new object();

        private Logging()
        {

        }

        public static Logging GetInstance()
        {
            if (_uniqueInstance == null)
                lock (_threadLock)
                    _uniqueInstance = new Logging();
            return _uniqueInstance;
        }

        public void ClearLogs(System.Windows.Controls.ListBox listBox)
        {
            logCollection = new ObservableCollection<string>();
            Refresh();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public void Refresh()
        {
            OnPropertyChanged(null);
        }

        private ObservableCollection<string> logCollection = new ObservableCollection<string>();

        public ObservableCollection<string> LogCollection
        {
            get { return logCollection; }
        }

        public delegate void IncrementDelegate(string log); // Delegated increment

        public void Increment(string log) // Standard increment
        {
            logCollection.Add(log);
            Refresh();
        }

        public void Increment(Dispatcher dispatcher, string log) // Dispatched increment
        {
            dispatcher.Invoke(new IncrementDelegate(Increment), log);
        }
    }
}
