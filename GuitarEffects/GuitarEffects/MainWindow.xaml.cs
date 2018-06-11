using System;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using WPFSoundVisualizationLib;
using System.Windows.Data;

namespace GuitarEffects
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private NAudioEngine soundEngine;
        private float reverbDecayValue = 0.7f;
        private int reverbDelayValue = 50;

        private int compressThresh = -10;
        private double compressGain = 0.5;
        private int compressGate = -40;

        public MainWindow()
        {
            InitializeComponent();

            soundEngine = NAudioEngine.Instance;
            soundEngine.PropertyChanged += NAudioEngine_PropertyChanged;

            UIHelper.Bind(soundEngine, "CanStop", StopButton, Button.IsEnabledProperty);
            UIHelper.Bind(soundEngine, "CanPlay", PlayButton, Button.IsEnabledProperty);
            UIHelper.Bind(soundEngine, "CanPause", PauseButton, Button.IsEnabledProperty);

            waveformTimeline.RegisterSoundPlayer(soundEngine);

            SetDefaults();
        }
        private void SetDefaults()
        {
            decayText.Text = reverbDecayValue.ToString();
            reverbDecay.Value = reverbDecayValue * 100;

            delayText.Text = reverbDelayValue.ToString() + " ms";
            reverbDelay.Value = reverbDelayValue;

            threshText.Text = compressThresh.ToString() + " dB";
            compressorThresh.Value = 50 + (int)compressThresh;

            gainText.Text = compressGain.ToString();
            compressorGain.Value = compressGain * 100;


            gateText.Text = compressGate.ToString() + " dB";
            compressorGate.Value = 60 + (int)compressGate;

        }
        private void ApplyEffectsButton_Click(object sender, RoutedEventArgs e)
        {
            ErrorText.Text = "";

            soundEngine.ClearEffects();

            if (CompressorSwitch.IsChecked == true)
            {
                soundEngine.AddCompressors(this.compressThresh, this.compressGain, this.compressGate);
            }
            if (ReverbSwitch.IsChecked == true)
            {
                soundEngine.AddReverbs(this.reverbDecayValue, this.reverbDelayValue);
            }

            try
            {
                soundEngine.ChangeEffects();
            }
            catch(Exception ex)
            {
                ErrorText.Text = ex.Message;
            }
            
        }

        private void ReverbDecaySlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            this.reverbDecayValue = (float)Math.Round(reverbDecay.Value / 100, 2);
            decayText.Text = this.reverbDecayValue.ToString();
        }

        private void ReverbDelaySlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            this.reverbDelayValue = (int)reverbDelay.Value;
            delayText.Text = this.reverbDelayValue.ToString() + " ms";
        }

        private void CompressorThreshSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            this.compressThresh = (50 - (int)compressorThresh.Value) * -1;
            threshText.Text = this.compressThresh.ToString() + " dB";        
        }

        private void CompressorGainSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            this.compressGain = Math.Round(compressorGain.Value / 100, 2);
            gainText.Text = this.compressGain.ToString();

        }

        private void CompressorGateSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            this.compressGate = (60 - (int)compressorGate.Value) * -1;
            gateText.Text = this.compressGate.ToString() + " dB";

        }

        #region NAudio Engine Events
        private void NAudioEngine_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            NAudioEngine engine = NAudioEngine.Instance;
            switch (e.PropertyName)
            {

                case "ChannelPosition":
                    
                    break;
                default:
                    // Do Nothing
                    break;
            }

        }
        #endregion
        private void OpenFile()
        {
            Microsoft.Win32.OpenFileDialog openDialog = new Microsoft.Win32.OpenFileDialog();
            openDialog.Filter = "(*.mp3;*.wav)|*.mp3;*.wav";
            if (openDialog.ShowDialog() == true)
            {
                ErrorText.Text = "";
                try
                {
                    soundEngine.OpenFile(openDialog.FileName);
                }
                catch (ArgumentException ex)
                {
                    ErrorText.Text = ex.Message;
                }
                
                FileText.Text = openDialog.FileName;
            }
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            if (soundEngine.CanPlay)
                soundEngine.Play();
        }

        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            if (soundEngine.CanPause)
                soundEngine.Pause();
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            if (soundEngine.CanStop)
                soundEngine.Stop();
        }

        private void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFile();
        }


    }
}
