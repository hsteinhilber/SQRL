using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace SQRL
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            this.AssemblyInfo.Text = String.Format("Product: {0}\nCompany: {1}\nCopyright: {2}\nTrademark: {3}\n" + 
                                                   "Title: {4}\nVersion: {5}\nFile Version: {6}\n" +
                                                   "Info Version: {7}\n", 
                                         SQRL.AssemblyInfo.Product,
                                         SQRL.AssemblyInfo.Company,
                                         SQRL.AssemblyInfo.Copyright,
                                         SQRL.AssemblyInfo.Trademark,
                                         SQRL.AssemblyInfo.Title,
                                         SQRL.AssemblyInfo.Version.ToString(),
                                         SQRL.AssemblyInfo.FileVersion,
                                         SQRL.AssemblyInfo.InformationalVersion);        
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException("The exception handling page is not yet fully implemented. This is just a test.");
        }
    }
}
