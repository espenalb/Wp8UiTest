using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Wp8UiTest.Resources;

namespace Wp8UiTest
{
    public partial class MainPage : PhoneApplicationPage
    {
        private int _clickCounter;
        private int _tapCounter;
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            var peer = new ButtonAutomationPeer(Button1);
            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
            TextBlock.Text = "";
            foreach (var control in new UIElement[] { Button1, TextBox, TextBlock, Panorama, PanoramaItem, Pivot, PivotItem })
            {
                TextBlock.Text += control.GetType().Name + " supports:";
                foreach (var pattern in from PatternInterface pattern in Enum.GetValues(typeof(PatternInterface)) let invoker = peer.GetPattern(pattern) where invoker != null select pattern.ToString())
                {
                    TextBlock.Text += pattern;
                }
                TextBlock.Text += ",";
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ((Button)sender).Content = "Clicked " + _clickCounter;
            _clickCounter++;

        }


        private void Button_Tap(object sender, RoutedEventArgs e)
        {
            ((Button)sender).Content = "Tapped " + _tapCounter;
            _tapCounter++;

        }

        private void OnAutomateClicked(object sender, RoutedEventArgs e)
        {
            InvokeButtons();
        }

        private void InvokeButtons()
        {
            foreach (var button in new[] { Button1, Button2 })
            {
                var peer = new ButtonAutomationPeer(button);
                var invokeProv = peer.GetPattern(PatternInterface.Invoke) as IInvokeProvider;
                if (invokeProv != null)
                {
                    invokeProv.Invoke();
                }
            }
        }

        private void OnClickInWorker(object sender, RoutedEventArgs e)
        {
            var worker = new BackgroundWorker();
            worker.DoWork += (o, workAargs) =>
            {
                // We need to invoke the automation peer on the UI thread:
                var dispatcher = Deployment.Current.Dispatcher;
                dispatcher.BeginInvoke(InvokeButtons);
            };
            worker.RunWorkerAsync();
        }

        // Sample code for building a localized ApplicationBar
        //private void BuildLocalizedApplicationBar()
        //{
        //    // Set the page's ApplicationBar to a new instance of ApplicationBar.
        //    ApplicationBar = new ApplicationBar();

        //    // Create a new button and set the text value to the localized string from AppResources.
        //    ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
        //    appBarButton.Text = AppResources.AppBarButtonText;
        //    ApplicationBar.Buttons.Add(appBarButton);

        //    // Create a new menu item with the localized string from AppResources.
        //    ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
        //    ApplicationBar.MenuItems.Add(appBarMenuItem);
        //}
    }
}