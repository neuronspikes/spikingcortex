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
            fabricInputImage.Source = simulation.inputPicture.Bitmap;
            fabricOutputImage.Source = simulation.outputPicture.Bitmap;

            ExitationLeakSlider.Value = simulation.fab.ExitationLeak;
            InhibitionLeakSlider.Value = simulation.fab.InhibitionLeak;
            SpikeEffectSlider.Value = simulation.udpInput.SpikeWeight;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            simulation.stop = true;
        }

        private void IntervalSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if(simulation != null)simulation.interval = ((int)e.NewValue);
        }

        private void ExitationLeakSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            double leak=1.0/((double)e.NewValue);
            if (simulation != null)
            {
                simulation.fab.ExitationLeak = leak;
                LeakText.Content = "Exitation leak = "+leak;
            }
        }

        private void InhibitionLeakSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            double leak = 1.0 / ((double)e.NewValue);
            if (simulation != null)
            {
                simulation.fab.InhibitionLeak = leak;
                LeakText.Content = "inhibition leak = " + leak;
            }
        }

        private void ExitationSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (simulation != null)
            {
                simulation.udpInput.SpikeWeight = ((double)e.NewValue);
                ExitationText.Content = "spike weight = "+e.NewValue;
            }
        }


        private void LearnConceptsCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            simulation.fab.CanLearnNewConcepts = true;
        }

        private void LearnConceptsCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            simulation.fab.CanLearnNewConcepts = false;
        }

        private void FeedBackCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            simulation.fab.CanDevelopFeedback = true;
        }

        private void FeedBackCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            simulation.fab.CanDevelopFeedback = false;
        }

        private void LearnConcurrencyCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            simulation.fab.CanDevelopConcurrency = true;
        }

        private void LearnConcurrencyCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            simulation.fab.CanDevelopConcurrency = false;
        }
    }
}
