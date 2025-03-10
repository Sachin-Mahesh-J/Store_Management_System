using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Retail_Managment_WPF_UI.Services
{
    public static class CommonButtons
    {

        public static void closebutton(Window window)
        {
            window.Close();
        }

        public static void minimizebutton(Window window)
        {
            window.WindowState = WindowState.Minimized;
        }

        public static void maximizebutton(Window window, Button maximizeButton)
        {
            if (window.WindowState == WindowState.Maximized)
            {
                window.WindowState = WindowState.Normal;
                maximizeButton.Content = "□";
            }
            else
            {
                window.WindowState = WindowState.Maximized;
                maximizeButton.Content = "❐";
            }
        }

        public static void dragmove(Window window,MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                window.DragMove();
            }
        }

    }
}

