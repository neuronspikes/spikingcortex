using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

using System.Windows.Data;
using System.Windows.Documents;

using System.Windows.Input;
using System.Threading;



using SpikingNeurons;

namespace CortexViewer
{
    /// <summary>
    /// Interaction logic for FabricViewer.xaml
    /// </summary>
    public partial class FabricViewer : Window
    {

        Simulation simulation;
       
        public FabricViewer()
        {
            InitializeComponent();

            // pre initialization
            simulation = new Simulation(Dispatcher);

  
            // bind moving image sources
            fabricInputImage.Source = simulation.input.Bitmap;
            fabricOutputImage.Source = simulation.output.Bitmap;
            fabricSetImage.Source = simulation.fabric.Bitmap;
        }

        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
      
        }

        private void fabricInputImage_ImageFailed(object sender, ExceptionRoutedEventArgs e)
        {

        }

        private void fabricInputImage_ImageFailed_1(object sender, ExceptionRoutedEventArgs e)
        {

        }

        private void Window_MouseMove_1(object sender, MouseEventArgs e)
        {
            // bind moving image sources
            fabricInputImage.Source = simulation.input.Bitmap;
            fabricOutputImage.Source = simulation.output.Bitmap;
            fabricSetImage.Source = simulation.fabric.Bitmap;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            simulation.stop = true;
        }
    }
}
