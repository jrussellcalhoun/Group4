using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Threading;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Data;

namespace WordGame.Tools
{
    public class ControlScaler : INotifyPropertyChanged
    {
        public double ScreenWidth { get; private set; }
        public double ScreenHeight { get; private set; }

        public object[] Ratios
        {
            get { return new object[] { "0.05", "0.1", "0.15", "0.2", "0.25", "0.3", "0.35", "0.4", "0.45", "0.5", "0.55", "0.6", "0.65", "0.7", "0.75", "0.8", "0.85", "0.9", "0.95" }; }
        }

        /// <summary>
        /// Static helper method that returns ratio * SystemParameters.MaximizedPrimaryScreenWidth if the provide ratio is greater than zero and less than 1.
        /// Otherwise, it returns SystemParameters.MaximizedPrimaryScreenWidth.
        /// </summary>
        /// <param name="ratio">The ratio of the screen width (between 0.0 and 1.0) to multiply the MaximizedPrimaryScreenWidth system parameter by.</param>
        /// <returns></returns>
        public double GetScreenWidthRatio(double ratio)
        {
            return ratio < 0 ? ScreenWidth : ratio > 1 ? ScreenWidth : ratio * ScreenWidth;
        }

        /// <summary>
        /// Static helper method that returns ratio * SystemParameters.MaximizedPrimaryScreenHeight if the provide ratio is greater than zero and less than 1.
        /// Otherwise, it returns SystemParameters.MaximizedPrimaryScreenHeight.
        /// </summary>
        /// <param name="ratio">The ratio of the screen height (between 0.0 and 1.0) to multiply the MaximizedPrimaryScreenHeight system parameter by.</param>
        /// <returns></returns>
        public double GetScreenHeightRatio(double ratio)
        {
            return ratio < 0 ? ScreenHeight : ratio > 1 ? ScreenHeight : ratio * ScreenHeight;
        }

        public double GetControlRatio(double size, double ratio)
        {
            return size * ratio;
        }

        public ControlScaler() 
        {
            ScreenWidth = SystemParameters.MaximizedPrimaryScreenWidth;
            ScreenHeight = SystemParameters.MaximizedPrimaryScreenHeight;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            // If we have any listeners to the PropertyChanged event. This utilizes reflection to update the UI with our logic properties.
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}