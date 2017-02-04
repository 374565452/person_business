using NAudio.CoreAudioApi;
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
    public partial class AudioRecordFrm : Form
    {
        private Dictionary<String, String> pList = new Dictionary<String, String>();
        public AudioRecordFrm()
        {
            InitializeComponent();
        }
        public bool checkMicrophone()
        {
            MMDeviceEnumerator deviceEnum = new MMDeviceEnumerator();
            List<MMDevice> devices = deviceEnum.EnumerateAudioEndPoints(DataFlow.Capture, DeviceState.Active).ToList();
            if (devices == null || devices.Count <= 0)
            {
                return false;
            }
            return true;
        }
        private void startRecordBtn_Click(object sender, EventArgs e)
        {
            if (!checkMicrophone())
            {
                MessageBox.Show("未检测到录音设备，请插入麦克风或话筒等输入设备！");
                return;
            }
            RecordingFrm recordingFrm = new RecordingFrm();
            recordingFrm.FormClosing += recordingFrm_FormClosing;
            recordingFrm.ShowDialog();
        }

        void recordingFrm_FormClosing(object sender, FormClosingEventArgs e)
        {
            //throw new NotImplementedException();
            RecordFlagModel model = (sender as RecordingFrm).RecordFlagModel;
            if (model.Flag == true)
            {
                pList.Add(model.RecprdFileName, model.RecordFileFullPath);
                this.listBox1.Items.Add(model.RecprdFileName);
            }
        }

        private void delBtn_Click(object sender, EventArgs e)
        {
            //Form1 form = new Form1();
            //form.ShowDialog();
        }
    }
}
