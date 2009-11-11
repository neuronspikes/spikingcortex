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
            simulation = new Simulation(this);

  
            // bind moving image sources
 //           fabricInputImage.Source = simulation.inputPicture.Bitmap;
            this.fabricSetImage.Source = simulation.fabricPicture.Bitmap;
 //           fabricOutputImage.Source = simulation.outputPicture.Bitmap;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            simulation.stop = true;
        }
    }
}
