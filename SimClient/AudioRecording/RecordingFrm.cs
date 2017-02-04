using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AudioRecording
{
    public partial class RecordingFrm : Form
    {
        private DateTime startTime;

        private WaveIn waveInSource = null;

        private WaveFileWriter waveFileWriter = null;

        public RecordingFrm()
        {
            InitializeComponent();
            cleanUp();

            waveInSource = new WaveIn();
            //waveInSource.WaveFormat = new WaveFormat(44100, 1);
            waveInSource.WaveFormat = new WaveFormat(8000, 1);
            waveInSource.DataAvailable += waveInSource_DataAvailable;
            waveInSource.RecordingStopped += waveInSource_RecordingStopped;

            waveFileWriter = new WaveFileWriter(@"D:\Test0001.wav", waveInSource.WaveFormat);
            try
            {
                waveInSource.StartRecording();
            }
            catch (Exception)
            {
                waveFileWriter.Dispose();
                waveFileWriter = null;
                waveInSource.Dispose();
                waveInSource = null;
                MessageBox.Show("录音出错，请检查麦克风或话筒等输入设备！");
            }

            startTime = DateTime.Now;
            recordingTimer.Enabled = true;
            recordingTimer.Start();

        }
        void waveInSource_RecordingStopped(object sender, StoppedEventArgs e)
        {
            //throw new NotImplementedException();
            //waveInSource.StopRecording();
            //MessageBox.Show("aaaaaaaaaaaaaaaaaaaaaaa");
            if (waveFileWriter != null)
            {
                waveFileWriter.Flush();
                waveFileWriter.Dispose();
                waveFileWriter = null;
            }
        }

        void waveInSource_DataAvailable(object sender, WaveInEventArgs e)
        {
            //throw new NotImplementedException();
            if (waveFileWriter != null)
            {
                waveFileWriter.Write(e.Buffer, 0, e.BytesRecorded);
                waveFileWriter.Flush();
            }
        }
        public void cleanUp()
        {
            if (waveInSource != null)
            {
                waveInSource.Dispose();
                waveInSource = null;
            }
            finalizeWaveFile();
        }
        private void finalizeWaveFile()
        {
            if (waveFileWriter != null)
            {
                waveFileWriter.Dispose();
                waveFileWriter = null;
            }
        }
        private void recordingTimer_Tick(object sender, EventArgs e)
        {
            TimeSpan ts = DateTime.Now - startTime;
            string str =null;
           
             //ts.Milliseconds
            str = string.Format("{0:D2}:{1:D2}:{2:D2}", ts.Hours, ts.Minutes, ts.Seconds);
            recordingTimeLabel.Text = str;
        }

        private void stopBtn_Click(object sender, EventArgs e)
        {
            recordingTimer.Enabled = false;
            recordingTimer.Stop();
            this.Dispose();
            this.Close();
        }
    }
}
