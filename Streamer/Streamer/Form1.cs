using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NAudio.CoreAudioApi;
using NAudio.Wave;


namespace Streamer
{
    public partial class Form1 : Form
    {
        WaveFileWriter writer;
        WaveInEvent waveIn;

        public Form1()
        {
            InitializeComponent();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);

            waveIn?.Dispose();
            writer?.Dispose();
        }

        private void StartRecording()
        {
            waveIn = new WaveInEvent();
            writer = new WaveFileWriter(Path.Combine(Directory.GetCurrentDirectory(), "recording.wav"), waveIn.WaveFormat);
            waveIn.DataAvailable += (o, e) =>
            {
                writer.Write(e.Buffer, 0, e.BytesRecorded);
                if (writer.Position > waveIn.WaveFormat.AverageBytesPerSecond * 30)
                {
                    waveIn.StopRecording();
                }
            };

            waveIn.RecordingStopped += (o, e) =>
            {
                writer?.Dispose();
                writer = null;

            };
            
            waveIn.StartRecording();
            
            btnStart.Enabled = false;

        }

   

        private void WaveIn_DataAvailable(object sender, WaveInEventArgs e)
        {
           
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            StartRecording();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            waveIn?.StopRecording();
            btnStart.Enabled = true;
        }
    }
}
