using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

using System.Windows.Data;
using System.Windows.Documents;

using System.Windows.Input;




using SpikingNeurons;

namespace CortexViewer
{
    /// <summary>
    /// Interaction logic for FabricViewer.xaml
    /// </summary>
    public partial class FabricViewer : Window
    {
        PictureNeuronStates input, output, fabric;
        
        public FabricViewer()
        {
            InitializeComponent();

            input = new PictureNeuronStates(10, 200, null);
            output = new PictureNeuronStates(10, 200, null);
            fabric = new PictureNeuronStates(10, 200, null);
            
            fabricInputImage.Source=input.Bitmap;
            fabricOutputImage.Source = output.Bitmap;
            fabricSetImage.Source = fabric.Bitmap;
        }

        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
           // if(input!= null)this.fabric.updateImage();
        }

        private void fabricInputImage_ImageFailed(object sender, ExceptionRoutedEventArgs e)
        {

        }

        private void fabricInputImage_ImageFailed_1(object sender, ExceptionRoutedEventArgs e)
        {

        }

        private void Window_MouseMove_1(object sender, MouseEventArgs e)
        {
            if (input != null) this.fabric.updateImage();
        }
    }
}
