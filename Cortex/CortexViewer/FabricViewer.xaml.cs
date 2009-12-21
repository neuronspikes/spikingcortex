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

            // Load model
            simulation = new Simulation(this);

            // bind views
            fabricInputImage.Source = simulation.inputPicture.Bitmap;
            fabricOutputImage.Source = simulation.outputPicture.Bitmap;

            // initialize sliders to effective values
            ExitationLeakSlider.Value = simulation.udpInput.ExitationLeak;
            InhibitionLeakSlider.Value = simulation.udpInput.InhibitionLeak;
            SpikeEffectSlider.Value = simulation.udpInput.SpikeWeight;
            IntervalSlider.Value = simulation.interval;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            simulation.stop = true;
        }

        private void IntervalSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (simulation != null)
            {
                simulation.interval = ((int)e.NewValue);
                IntervalLabel.Content="Interval = " + simulation.interval + "ms";
            }
        }

        private void ExitationLeakSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            double leak=1.0/((double)e.NewValue);
            if (simulation != null)
            {
                simulation.udpInput.ExitationLeak = leak;
                ExitationLeakLabel.Content = "Exitation retention =" + leak.ToString("F");
            }
        }

        private void InhibitionLeakSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            double leak = 1.0 / ((double)e.NewValue);
            if (simulation != null)
            {
                simulation.udpInput.InhibitionLeak = leak;
                this.InhibitionLeakLabel.Content = "Inhibition retention =" + leak.ToString("F");
            }
        }

        private void ExitationSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (simulation != null)
            {
                simulation.udpInput.SpikeWeight = ((double)e.NewValue);
                ExitationText.Content = "Spike weight =" + ((double)e.NewValue).ToString("F");
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
