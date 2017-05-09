using Common;
using Newtonsoft.Json;
using System;
using System.Windows.Forms;

namespace CardOperationSystem
{
    public partial class frmCardNetSet : Form
    {
        private int icdev;
        frmMain pf;
        public frmCardNetSet(frmMain pf)
        {
            InitializeComponent();
            this.icdev = pf.icdev;
            this.pf = pf;
        }

        private void frmCardNetSet_Load(object sender, EventArgs e)
        {
            clear();
        }

        private void clear()
        {
            SerialNumber_old = "";
            this.lbCardType.Text = InfoSys.StrUnknown;
            this.lbSerialNumber.Text = InfoSys.StrUnknown;
            this.lbState.Text = InfoSys.StrState;

            this.txtIP.Text = "";
            this.txtPort.Text = "";
            this.ckbIsDomain.Checked = false;
            this.txtAPN.Text = "";
            this.txtUserName.Text = "";
            this.txtPassword.Text = "";

            //add by kqz 2017 -5-9 10:49
            this.txtIP2.Text = "";
            this.txtPort2.Text = "";
            this.ckbIsDomain2.Checked = false;
            this.txtApn2.Text = "";
            this.txtUserName2.Text = "";
            this.txtPassword2.Text = "";

            this.txtIP3.Text = "";
            this.txtPort3.Text = "";
            this.ckbIsDomain3.Checked = false;
            this.txtApn3.Text = "";
            this.txtUserName3.Text = "";
            this.txtPassword3.Text = "";
            //end add
        }

        string SerialNumber_old = "";
        string IsDomain_old = "";
        private void button1_Click(object sender, EventArgs e)
        {
            clear();

            pf.BeginLog(InfoSys.CardTypeStrNetSet, InfoSys.MethodReadCard);

            if (!pf.SeedIC())
            {
                this.lbCardType.Text = InfoSys.StrSeekFailure;
                pf.EndLog(InfoSys.CardTypeStrNetSet, InfoSys.MethodReadCard);
                return;
            }

            this.lbSerialNumber.Text = pf.getSnr().ToString("X");

            int mode = 4; //以B密码认证
            int sec = 1; //扇区
            int block = 0;
            string key = pf.getKeyB(); //读卡密码
            string result_ReadIC = "";
            string result_AuthIC = "";

            int len_IP = 0;
            int len_Port = 0;
            int len_APN = 0;
            int len_UserName = 0;
            int len_Password = 0;
            //start add by kqz 2017-5-9 10:52
            int len_IP2 = 0;
            int len_Port2= 0;
            int len_APN2 = 0;
            int len_UserName2 = 0;
            int len_Password2 = 0;

            int len_IP3 = 0;
            int len_Port3 = 0;
            int len_APN3 = 0;
            int len_UserName3 = 0;
            int len_Password3 = 0;
            //end add

            //读取扇区1内容
            sec = 1;
            //认证卡密码B
            result_AuthIC = CardCommon.AuthIC(icdev, mode, sec, key);
            pf.AuthLog(InfoSys.CardTypeStrNetSet, InfoSys.MethodReadCard, sec, result_AuthIC);
            if (result_AuthIC == InfoSys.StrAuthSuccess)
            {
                //读数据块0，数据块0为卡类型（1字节）、是否域名访问（1字节）、有效IP地址长度（1字节）、有效端口号长度（1字节）
                //APN名称长度（1字节）、APN用户名长度（1字节）、APN密码长度（1字节）                
                block = 0;
                result_ReadIC = CardCommon.ReadIC(icdev, sec, block);
                if (result_ReadIC.Length == 32)
                {
                    this.lbCardType.Text = result_ReadIC.Substring(0, 2);
                    if (this.lbCardType.Text != InfoSys.CardTypeNetSet)
                    {
                        this.lbState.Text = "非" + InfoSys.CardTypeStrNetSet + "，" + InfoSys.StrCannotRead;
                        pf.Log(this.lbState.Text);
                        pf.EndLog(InfoSys.CardTypeStrNetSet, InfoSys.MethodReadCard);
                        return;
                    }

                    IsDomain_old = result_ReadIC.Substring(2, 2);
                    this.ckbIsDomain.Checked = IsDomain_old == "01";

                    len_IP = int.Parse(result_ReadIC.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
                    len_Port = int.Parse(result_ReadIC.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);
                    len_APN = int.Parse(result_ReadIC.Substring(8, 2), System.Globalization.NumberStyles.HexNumber);
                    len_UserName = int.Parse(result_ReadIC.Substring(10, 2), System.Globalization.NumberStyles.HexNumber);
                    len_Password = int.Parse(result_ReadIC.Substring(12, 2), System.Globalization.NumberStyles.HexNumber);
                }
                else
                {
                    this.lbState.Text = InfoSys.InfoReadFailure(sec, block, InfoSys.CardTypeStrNetSet, InfoSys.StrReadFailure);
                    pf.Log(this.lbState.Text);
                    pf.EndLog(InfoSys.CardTypeStrNetSet, InfoSys.MethodReadCard);
                    return;
                }

                //读数据块1，IP地址或域名地址
                block = 1;
                result_ReadIC = CardCommon.ReadIC(icdev, sec, block);
                if (result_ReadIC.Length == 32)
                {
                    if (len_IP > 0)
                    {
                        string v = result_ReadIC.Substring(0, len_IP * 2);
                        this.txtIP.Text = HexStringUtility.ByteArrayToStr(HexStringUtility.HexStringToByteArray(v));
                    }
                }
                else
                {
                    this.lbState.Text = InfoSys.InfoReadFailure(sec, block, InfoSys.CardTypeStrNetSet, InfoSys.StrReadFailure);
                    pf.Log(this.lbState.Text);
                    pf.EndLog(InfoSys.CardTypeStrNetSet, InfoSys.MethodReadCard);
                    return;
                }

                //读数据块2，端口号（5字节）
                block = 2;
                result_ReadIC = CardCommon.ReadIC(icdev, sec, block);
                if (result_ReadIC.Length == 32)
                {
                    if (len_Port > 0)
                    {
                        string v = result_ReadIC.Substring(0, len_Port * 2);
                        this.txtPort.Text = HexStringUtility.ByteArrayToStr(HexStringUtility.HexStringToByteArray(v));
                    }
                }
                else
                {
                    this.lbState.Text = InfoSys.InfoReadFailure(sec, block, InfoSys.CardTypeStrNetSet, InfoSys.StrReadFailure);
                    pf.Log(this.lbState.Text);
                    pf.EndLog(InfoSys.CardTypeStrNetSet, InfoSys.MethodReadCard);
                    return;
                }
            }

            //读取扇区2内容
            sec = 2;
            //认证卡密码B
            result_AuthIC = CardCommon.AuthIC(icdev, mode, sec, key);
            pf.AuthLog(InfoSys.CardTypeStrNetSet, InfoSys.MethodReadCard, sec, result_AuthIC);
            if (result_AuthIC == InfoSys.StrAuthSuccess)
            {
                //读数据块0，APN名称                
                block = 0;
                result_ReadIC = CardCommon.ReadIC(icdev, sec, block);
                if (result_ReadIC.Length == 32)
                {
                    if (len_APN > 0)
                    {
                        string v = result_ReadIC.Substring(0, len_APN * 2);
                        this.txtAPN.Text = HexStringUtility.ByteArrayToStr(HexStringUtility.HexStringToByteArray(v));
                    }
                }
                else
                {
                    this.lbState.Text = InfoSys.InfoReadFailure(sec, block, InfoSys.CardTypeStrNetSet, InfoSys.StrReadFailure);
                    pf.Log(this.lbState.Text);
                    pf.EndLog(InfoSys.CardTypeStrNetSet, InfoSys.MethodReadCard);
                    return;
                }

                //读数据块1，用户名
                block = 1;
                result_ReadIC = CardCommon.ReadIC(icdev, sec, block);
                if (result_ReadIC.Length == 32)
                {
                    if (len_UserName > 0)
                    {
                        string v = result_ReadIC.Substring(0, len_UserName * 2);
                        this.txtUserName.Text = HexStringUtility.ByteArrayToStr(HexStringUtility.HexStringToByteArray(v));
                    }
                }
                else
                {
                    this.lbState.Text = InfoSys.InfoReadFailure(sec, block, InfoSys.CardTypeStrNetSet, InfoSys.StrReadFailure);
                    pf.Log(this.lbState.Text);
                    pf.EndLog(InfoSys.CardTypeStrNetSet, InfoSys.MethodReadCard);
                    return;
                }

                //读数据块2，密码
                block = 2;
                result_ReadIC = CardCommon.ReadIC(icdev, sec, block);
                if (result_ReadIC.Length == 32)
                {
                    if (len_Password > 0)
                    {
                        string v = result_ReadIC.Substring(0, len_Password * 2);
                        this.txtPassword.Text = HexStringUtility.ByteArrayToStr(HexStringUtility.HexStringToByteArray(v));
                    }
                }
                else
                {
                    this.lbState.Text = InfoSys.InfoReadFailure(sec, block, InfoSys.CardTypeStrNetSet, InfoSys.StrReadFailure);
                    pf.Log(this.lbState.Text);
                    pf.EndLog(InfoSys.CardTypeStrNetSet, InfoSys.MethodReadCard);
                    return;
                }
            }
            //start add by kqz 2017-5-9 10:53
            //读取扇区1内容
            sec = 3;
            //认证卡密码B
            result_AuthIC = CardCommon.AuthIC(icdev, mode, sec, key);
            pf.AuthLog(InfoSys.CardTypeStrNetSet, InfoSys.MethodReadCard, sec, result_AuthIC);
            if (result_AuthIC == InfoSys.StrAuthSuccess)
            {
                //读数据块0，数据块0为卡类型（1字节）、是否域名访问（1字节）、有效IP地址长度（1字节）、有效端口号长度（1字节）
                //APN名称长度（1字节）、APN用户名长度（1字节）、APN密码长度（1字节）                
                block = 0;
                result_ReadIC = CardCommon.ReadIC(icdev, sec, block);
                if (result_ReadIC.Length == 32)
                {
                    this.lbCardType.Text = result_ReadIC.Substring(0, 2);
                    if (this.lbCardType.Text != InfoSys.CardTypeNetSet)
                    {
                        this.lbState.Text = "非" + InfoSys.CardTypeStrNetSet + "，" + InfoSys.StrCannotRead;
                        pf.Log(this.lbState.Text);
                        pf.EndLog(InfoSys.CardTypeStrNetSet, InfoSys.MethodReadCard);
                        return;
                    }

                    IsDomain_old = result_ReadIC.Substring(2, 2);
                    this.ckbIsDomain2.Checked = IsDomain_old == "01";

                    len_IP2 = int.Parse(result_ReadIC.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
                    len_Port2 = int.Parse(result_ReadIC.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);
                    len_APN2 = int.Parse(result_ReadIC.Substring(8, 2), System.Globalization.NumberStyles.HexNumber);
                    len_UserName2 = int.Parse(result_ReadIC.Substring(10, 2), System.Globalization.NumberStyles.HexNumber);
                    len_Password2 = int.Parse(result_ReadIC.Substring(12, 2), System.Globalization.NumberStyles.HexNumber);
                }
                else
                {
                    this.lbState.Text = InfoSys.InfoReadFailure(sec, block, InfoSys.CardTypeStrNetSet, InfoSys.StrReadFailure);
                    pf.Log(this.lbState.Text);
                    pf.EndLog(InfoSys.CardTypeStrNetSet, InfoSys.MethodReadCard);
                    return;
                }

                //读数据块1，IP地址或域名地址
                block = 1;
                result_ReadIC = CardCommon.ReadIC(icdev, sec, block);
                if (result_ReadIC.Length == 32)
                {
                    if (len_IP2 > 0)
                    {
                        string v = result_ReadIC.Substring(0, len_IP2 * 2);
                        this.txtIP2.Text = HexStringUtility.ByteArrayToStr(HexStringUtility.HexStringToByteArray(v));
                    }
                }
                else
                {
                    this.lbState.Text = InfoSys.InfoReadFailure(sec, block, InfoSys.CardTypeStrNetSet, InfoSys.StrReadFailure);
                    pf.Log(this.lbState.Text);
                    pf.EndLog(InfoSys.CardTypeStrNetSet, InfoSys.MethodReadCard);
                    return;
                }

                //读数据块2，端口号（5字节）
                block = 2;
                result_ReadIC = CardCommon.ReadIC(icdev, sec, block);
                if (result_ReadIC.Length == 32)
                {
                    if (len_Port2 > 0)
                    {
                        string v = result_ReadIC.Substring(0, len_Port2 * 2);
                        this.txtPort2.Text = HexStringUtility.ByteArrayToStr(HexStringUtility.HexStringToByteArray(v));
                    }
                }
                else
                {
                    this.lbState.Text = InfoSys.InfoReadFailure(sec, block, InfoSys.CardTypeStrNetSet, InfoSys.StrReadFailure);
                    pf.Log(this.lbState.Text);
                    pf.EndLog(InfoSys.CardTypeStrNetSet, InfoSys.MethodReadCard);
                    return;
                }
            }

            //读取扇区2内容
            sec = 4;
            //认证卡密码B
            result_AuthIC = CardCommon.AuthIC(icdev, mode, sec, key);
            pf.AuthLog(InfoSys.CardTypeStrNetSet, InfoSys.MethodReadCard, sec, result_AuthIC);
            if (result_AuthIC == InfoSys.StrAuthSuccess)
            {
                //读数据块0，APN名称                
                block = 0;
                result_ReadIC = CardCommon.ReadIC(icdev, sec, block);
                if (result_ReadIC.Length == 32)
                {
                    if (len_APN2 > 0)
                    {
                        string v = result_ReadIC.Substring(0, len_APN2 * 2);
                        this.txtApn2.Text = HexStringUtility.ByteArrayToStr(HexStringUtility.HexStringToByteArray(v));
                    }
                }
                else
                {
                    this.lbState.Text = InfoSys.InfoReadFailure(sec, block, InfoSys.CardTypeStrNetSet, InfoSys.StrReadFailure);
                    pf.Log(this.lbState.Text);
                    pf.EndLog(InfoSys.CardTypeStrNetSet, InfoSys.MethodReadCard);
                    return;
                }

                //读数据块1，用户名
                block = 1;
                result_ReadIC = CardCommon.ReadIC(icdev, sec, block);
                if (result_ReadIC.Length == 32)
                {
                    if (len_UserName2 > 0)
                    {
                        string v = result_ReadIC.Substring(0, len_UserName2 * 2);
                        this.txtUserName2.Text = HexStringUtility.ByteArrayToStr(HexStringUtility.HexStringToByteArray(v));
                    }
                }
                else
                {
                    this.lbState.Text = InfoSys.InfoReadFailure(sec, block, InfoSys.CardTypeStrNetSet, InfoSys.StrReadFailure);
                    pf.Log(this.lbState.Text);
                    pf.EndLog(InfoSys.CardTypeStrNetSet, InfoSys.MethodReadCard);
                    return;
                }

                //读数据块2，密码
                block = 2;
                result_ReadIC = CardCommon.ReadIC(icdev, sec, block);
                if (result_ReadIC.Length == 32)
                {
                    if (len_Password2 > 0)
                    {
                        string v = result_ReadIC.Substring(0, len_Password2 * 2);
                        this.txtPassword2.Text = HexStringUtility.ByteArrayToStr(HexStringUtility.HexStringToByteArray(v));
                    }
                }
                else
                {
                    this.lbState.Text = InfoSys.InfoReadFailure(sec, block, InfoSys.CardTypeStrNetSet, InfoSys.StrReadFailure);
                    pf.Log(this.lbState.Text);
                    pf.EndLog(InfoSys.CardTypeStrNetSet, InfoSys.MethodReadCard);
                    return;
                }
            }

            //读取扇区1内容
            sec =5;
            //认证卡密码B
            result_AuthIC = CardCommon.AuthIC(icdev, mode, sec, key);
            pf.AuthLog(InfoSys.CardTypeStrNetSet, InfoSys.MethodReadCard, sec, result_AuthIC);
            if (result_AuthIC == InfoSys.StrAuthSuccess)
            {
                //读数据块0，数据块0为卡类型（1字节）、是否域名访问（1字节）、有效IP地址长度（1字节）、有效端口号长度（1字节）
                //APN名称长度（1字节）、APN用户名长度（1字节）、APN密码长度（1字节）                
                block = 0;
                result_ReadIC = CardCommon.ReadIC(icdev, sec, block);
                if (result_ReadIC.Length == 32)
                {
                    this.lbCardType.Text = result_ReadIC.Substring(0, 2);
                    if (this.lbCardType.Text != InfoSys.CardTypeNetSet)
                    {
                        this.lbState.Text = "非" + InfoSys.CardTypeStrNetSet + "，" + InfoSys.StrCannotRead;
                        pf.Log(this.lbState.Text);
                        pf.EndLog(InfoSys.CardTypeStrNetSet, InfoSys.MethodReadCard);
                        return;
                    }

                    IsDomain_old = result_ReadIC.Substring(2, 2);
                    this.ckbIsDomain3.Checked = IsDomain_old == "01";

                    len_IP3 = int.Parse(result_ReadIC.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
                    len_Port3 = int.Parse(result_ReadIC.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);
                    len_APN3 = int.Parse(result_ReadIC.Substring(8, 2), System.Globalization.NumberStyles.HexNumber);
                    len_UserName3 = int.Parse(result_ReadIC.Substring(10, 2), System.Globalization.NumberStyles.HexNumber);
                    len_Password3 = int.Parse(result_ReadIC.Substring(12, 2), System.Globalization.NumberStyles.HexNumber);
                }
                else
                {
                    this.lbState.Text = InfoSys.InfoReadFailure(sec, block, InfoSys.CardTypeStrNetSet, InfoSys.StrReadFailure);
                    pf.Log(this.lbState.Text);
                    pf.EndLog(InfoSys.CardTypeStrNetSet, InfoSys.MethodReadCard);
                    return;
                }

                //读数据块1，IP地址或域名地址
                block = 1;
                result_ReadIC = CardCommon.ReadIC(icdev, sec, block);
                if (result_ReadIC.Length == 32)
                {
                    if (len_IP3 > 0)
                    {
                        string v = result_ReadIC.Substring(0, len_IP3 * 2);
                        this.txtIP3.Text = HexStringUtility.ByteArrayToStr(HexStringUtility.HexStringToByteArray(v));
                    }
                }
                else
                {
                    this.lbState.Text = InfoSys.InfoReadFailure(sec, block, InfoSys.CardTypeStrNetSet, InfoSys.StrReadFailure);
                    pf.Log(this.lbState.Text);
                    pf.EndLog(InfoSys.CardTypeStrNetSet, InfoSys.MethodReadCard);
                    return;
                }

                //读数据块2，端口号（5字节）
                block = 2;
                result_ReadIC = CardCommon.ReadIC(icdev, sec, block);
                if (result_ReadIC.Length == 32)
                {
                    if (len_Port3 > 0)
                    {
                        string v = result_ReadIC.Substring(0, len_Port3 * 2);
                        this.txtPort3.Text = HexStringUtility.ByteArrayToStr(HexStringUtility.HexStringToByteArray(v));
                    }
                }
                else
                {
                    this.lbState.Text = InfoSys.InfoReadFailure(sec, block, InfoSys.CardTypeStrNetSet, InfoSys.StrReadFailure);
                    pf.Log(this.lbState.Text);
                    pf.EndLog(InfoSys.CardTypeStrNetSet, InfoSys.MethodReadCard);
                    return;
                }
            }

            //读取扇区2内容
            sec = 6;
            //认证卡密码B
            result_AuthIC = CardCommon.AuthIC(icdev, mode, sec, key);
            pf.AuthLog(InfoSys.CardTypeStrNetSet, InfoSys.MethodReadCard, sec, result_AuthIC);
            if (result_AuthIC == InfoSys.StrAuthSuccess)
            {
                //读数据块0，APN名称                
                block = 0;
                result_ReadIC = CardCommon.ReadIC(icdev, sec, block);
                if (result_ReadIC.Length == 32)
                {
                    if (len_APN3 > 0)
                    {
                        string v = result_ReadIC.Substring(0, len_APN3 * 2);
                        this.txtApn3.Text = HexStringUtility.ByteArrayToStr(HexStringUtility.HexStringToByteArray(v));
                    }
                }
                else
                {
                    this.lbState.Text = InfoSys.InfoReadFailure(sec, block, InfoSys.CardTypeStrNetSet, InfoSys.StrReadFailure);
                    pf.Log(this.lbState.Text);
                    pf.EndLog(InfoSys.CardTypeStrNetSet, InfoSys.MethodReadCard);
                    return;
                }

                //读数据块1，用户名
                block = 1;
                result_ReadIC = CardCommon.ReadIC(icdev, sec, block);
                if (result_ReadIC.Length == 32)
                {
                    if (len_UserName3 > 0)
                    {
                        string v = result_ReadIC.Substring(0, len_UserName3 * 2);
                        this.txtUserName3.Text = HexStringUtility.ByteArrayToStr(HexStringUtility.HexStringToByteArray(v));
                    }
                }
                else
                {
                    this.lbState.Text = InfoSys.InfoReadFailure(sec, block, InfoSys.CardTypeStrNetSet, InfoSys.StrReadFailure);
                    pf.Log(this.lbState.Text);
                    pf.EndLog(InfoSys.CardTypeStrNetSet, InfoSys.MethodReadCard);
                    return;
                }

                //读数据块2，密码
                block = 2;
                result_ReadIC = CardCommon.ReadIC(icdev, sec, block);
                if (result_ReadIC.Length == 32)
                {
                    if (len_Password3 > 0)
                    {
                        string v = result_ReadIC.Substring(0, len_Password3 * 2);
                        this.txtPassword3.Text = HexStringUtility.ByteArrayToStr(HexStringUtility.HexStringToByteArray(v));
                    }
                }
                else
                {
                    this.lbState.Text = InfoSys.InfoReadFailure(sec, block, InfoSys.CardTypeStrNetSet, InfoSys.StrReadFailure);
                    pf.Log(this.lbState.Text);
                    pf.EndLog(InfoSys.CardTypeStrNetSet, InfoSys.MethodReadCard);
                    return;
                }
            }
            //end add

            if (this.lbCardType.Text == InfoSys.CardTypeNetSet)
            {
                SerialNumber_old = this.lbSerialNumber.Text.Trim();
                this.lbState.Text = InfoSys.StrReadSuccess;
            }
            else
            {
                this.lbState.Text = "非" + InfoSys.CardTypeStrNetSet + "，" + InfoSys.StrCannotRead;
                MessageBox.Show(this.lbState.Text);
            }
            pf.EndLog(InfoSys.CardTypeNetSet, InfoSys.MethodReadCard);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定" + InfoSys.CardTypeStrNetSet + InfoSys.MethodOpenCard + "？",
                InfoSys.CardTypeStrNetSet + InfoSys.MethodOpenCard, MessageBoxButtons.YesNo,
                MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
            {
                return;
            }

            pf.BeginLog(InfoSys.CardTypeStrNetSet, InfoSys.MethodOpenCard);

            if (!CheckValue())
            {
                pf.EndLog(InfoSys.CardTypeStrNetSet, InfoSys.MethodOpenCard);
                return;
            }

            if (!pf.SeedIC())
            {
                this.lbCardType.Text = InfoSys.StrSeekFailure;
                pf.EndLog(InfoSys.CardTypeStrNetSet, InfoSys.MethodOpenCard);
                return;
            }

            this.lbSerialNumber.Text = pf.getSnr().ToString("X");
            
            //保存远程服务器数据库
            string str = DataTransfer.OpenCardNetSet(this.lbSerialNumber.Text, IP, Port, IsDomain, APN, UserName, Password);
            JavaScriptObject result = (JavaScriptObject)JavaScriptConvert.DeserializeObject(str);
            if (!bool.Parse(result["Result"].ToString()))
            {
                string txt = result["Message"].ToString();
                MessageBox.Show(txt);
                pf.Log(txt);
                pf.EndLog(InfoSys.CardTypeStrNetSet, InfoSys.MethodOpenCard);
                return;
            }
            
            int mode = 4; //以B密码认证
            int sec = 1; //扇区
            int block = 0;
            string key = pf.getKeyA(); //读卡密码
            string keyNew = pf.getKeyB(); //读卡密码
            string result_WriteIC = "";
            string result_AuthIC = "";

            string hex_IP = HexStringUtility.StrToHexString(IP);
            string hex_Port = HexStringUtility.StrToHexString(Port);
            string hex_APN = HexStringUtility.StrToHexString(APN);
            string hex_UserName = HexStringUtility.StrToHexString(UserName);
            string hex_Password = HexStringUtility.StrToHexString(Password);

            string len_IP = (hex_IP.Length / 2).ToString("X").PadLeft(2, '0');
            string len_Port = (hex_Port.Length / 2).ToString("X").PadLeft(2, '0');
            string len_APN = (hex_APN.Length / 2).ToString("X").PadLeft(2, '0');
            string len_UserName = (hex_UserName.Length / 2).ToString("X").PadLeft(2, '0');
            string len_Password = (hex_Password.Length / 2).ToString("X").PadLeft(2, '0');

            //start add by kqz 2017-5-9 10:40
            string hex_IP2 = HexStringUtility.StrToHexString(IP2);
            string hex_Port2 = HexStringUtility.StrToHexString(Port2);
            string hex_APN2 = HexStringUtility.StrToHexString(APN2);
            string hex_UserName2 = HexStringUtility.StrToHexString(UserName2);
            string hex_Password2 = HexStringUtility.StrToHexString(Password2);

            string len_IP2 = (hex_IP2.Length / 2).ToString("X").PadLeft(2, '0');
            string len_Port2 = (hex_Port2.Length / 2).ToString("X").PadLeft(2, '0');
            string len_APN2 = (hex_APN2.Length / 2).ToString("X").PadLeft(2, '0');
            string len_UserName2 = (hex_UserName2.Length / 2).ToString("X").PadLeft(2, '0');
            string len_Password2 = (hex_Password2.Length / 2).ToString("X").PadLeft(2, '0');

            string hex_IP3 = HexStringUtility.StrToHexString(IP3);
            string hex_Port3 = HexStringUtility.StrToHexString(Port3);
            string hex_APN3 = HexStringUtility.StrToHexString(APN3);
            string hex_UserName3 = HexStringUtility.StrToHexString(UserName3);
            string hex_Password3 = HexStringUtility.StrToHexString(Password3);

            string len_IP3 = (hex_IP3.Length / 2).ToString("X").PadLeft(2, '0');
            string len_Port3 = (hex_Port3.Length / 2).ToString("X").PadLeft(2, '0');
            string len_APN3 = (hex_APN3.Length / 2).ToString("X").PadLeft(2, '0');
            string len_UserName3 = (hex_UserName3.Length / 2).ToString("X").PadLeft(2, '0');
            string len_Password3 = (hex_Password3.Length / 2).ToString("X").PadLeft(2, '0');
            //end add

            //设置扇区1内容
            sec = 1;
            //认证卡密码B
            result_AuthIC = CardCommon.AuthIC(icdev, mode, sec, key);
            pf.AuthLog(InfoSys.CardTypeStrNetSet, InfoSys.MethodOpenCard, sec, result_AuthIC);
            if (result_AuthIC == InfoSys.StrAuthSuccess)
            {
                //写数据块0，数据块0为卡类型（1字节）、是否域名访问（1字节）、有效IP地址长度（1字节）、有效端口号长度（1字节）
                //APN名称长度（1字节）、APN用户名长度（1字节）、APN密码长度（1字节）
                block = 0;
                result_WriteIC = CardCommon.WriteIC(icdev, sec, block, (InfoSys.CardTypeNetSet + IsDomain +
                    len_IP + len_Port + len_APN + len_UserName + len_Password).PadRight(32, '0'));
                if (result_WriteIC != "")
                {
                    this.lbState.Text = InfoSys.InfoWriteFailure(sec, block, InfoSys.CardTypeStrNetSet, InfoSys.StrOpenFailure);
                    pf.Log(this.lbState.Text);
                    pf.EndLog(InfoSys.CardTypeStrNetSet, InfoSys.MethodOpenCard);
                    return;
                }

                //写数据块1，IP地址或域名地址
                block = 1;
                result_WriteIC = CardCommon.WriteIC(icdev, sec, block, hex_IP.PadRight(32, '0'));
                if (result_WriteIC != "")
                {
                    this.lbState.Text = InfoSys.InfoWriteFailure(sec, block, InfoSys.CardTypeStrNetSet, InfoSys.StrOpenFailure);
                    pf.Log(this.lbState.Text);
                    pf.EndLog(InfoSys.CardTypeStrNetSet, InfoSys.MethodOpenCard);
                    return;
                }

                //写数据块2，端口号（5字节）
                block = 2;
                result_WriteIC = CardCommon.WriteIC(icdev, sec, block, hex_Port.PadRight(32, '0'));
                if (result_WriteIC != "")
                {
                    this.lbState.Text = InfoSys.InfoWriteFailure(sec, block, InfoSys.CardTypeStrNetSet, InfoSys.StrOpenFailure);
                    pf.Log(this.lbState.Text);
                    pf.EndLog(InfoSys.CardTypeStrNetSet, InfoSys.MethodOpenCard);
                    return;
                }
            }
            else
            {
                this.lbState.Text = InfoSys.StrCannotOpen + InfoSys.StrOpenFailure;
                pf.Log(this.lbState.Text);
                pf.EndLog(InfoSys.CardTypeStrNetSet, InfoSys.MethodOpenCard);
                return;
            }

            //设置扇区2内容
            sec = 2;
            //认证卡密码B
            result_AuthIC = CardCommon.AuthIC(icdev, mode, sec, key);
            pf.AuthLog(InfoSys.CardTypeStrNetSet, InfoSys.MethodOpenCard, sec, result_AuthIC);
            if (result_AuthIC == InfoSys.StrAuthSuccess)
            {
                //写数据块0，APN名称
                block = 0;
                result_WriteIC = CardCommon.WriteIC(icdev, sec, block, hex_APN.PadRight(32, '0'));
                if (result_WriteIC != "")
                {
                    this.lbState.Text = InfoSys.InfoWriteFailure(sec, block, InfoSys.CardTypeStrNetSet, InfoSys.StrOpenFailure);
                    pf.Log(this.lbState.Text);
                    pf.EndLog(InfoSys.CardTypeStrNetSet, InfoSys.MethodOpenCard);
                    return;
                }

                //写数据块1，用户名
                block = 1;
                result_WriteIC = CardCommon.WriteIC(icdev, sec, block, hex_UserName.PadRight(32, '0'));
                if (result_WriteIC != "")
                {
                    this.lbState.Text = InfoSys.InfoWriteFailure(sec, block, InfoSys.CardTypeStrNetSet, InfoSys.StrOpenFailure);
                    pf.Log(this.lbState.Text);
                    pf.EndLog(InfoSys.CardTypeStrNetSet, InfoSys.MethodOpenCard);
                    return;
                }

                //写数据块2，密码
                block = 2;
                result_WriteIC = CardCommon.WriteIC(icdev, sec, block, hex_Password.PadRight(32, '0'));
                if (result_WriteIC != "")
                {
                    this.lbState.Text = InfoSys.InfoWriteFailure(sec, block, InfoSys.CardTypeStrNetSet, InfoSys.StrOpenFailure);
                    pf.Log(this.lbState.Text);
                    pf.EndLog(InfoSys.CardTypeStrNetSet, InfoSys.MethodOpenCard);
                    return;
                }
            }

            //start add by kqz 2017-5-9 10:42 
            //设置扇区1内容
            sec = 3;
            //认证卡密码B
            result_AuthIC = CardCommon.AuthIC(icdev, mode, sec, key);
            pf.AuthLog(InfoSys.CardTypeStrNetSet, InfoSys.MethodOpenCard, sec, result_AuthIC);
            if (result_AuthIC == InfoSys.StrAuthSuccess)
            {
                //写数据块0，数据块0为卡类型（1字节）、是否域名访问（1字节）、有效IP地址长度（1字节）、有效端口号长度（1字节）
                //APN名称长度（1字节）、APN用户名长度（1字节）、APN密码长度（1字节）
                block = 0;
                result_WriteIC = CardCommon.WriteIC(icdev, sec, block, (InfoSys.CardTypeNetSet + IsDomain2 +
                    len_IP2 + len_Port2 + len_APN2 + len_UserName2 + len_Password2).PadRight(32, '0'));
                if (result_WriteIC != "")
                {
                    this.lbState.Text = InfoSys.InfoWriteFailure(sec, block, InfoSys.CardTypeStrNetSet, InfoSys.StrOpenFailure);
                    pf.Log(this.lbState.Text);
                    pf.EndLog(InfoSys.CardTypeStrNetSet, InfoSys.MethodOpenCard);
                    return;
                }

                //写数据块1，IP地址或域名地址
                block = 1;
                result_WriteIC = CardCommon.WriteIC(icdev, sec, block, hex_IP2.PadRight(32, '0'));
                if (result_WriteIC != "")
                {
                    this.lbState.Text = InfoSys.InfoWriteFailure(sec, block, InfoSys.CardTypeStrNetSet, InfoSys.StrOpenFailure);
                    pf.Log(this.lbState.Text);
                    pf.EndLog(InfoSys.CardTypeStrNetSet, InfoSys.MethodOpenCard);
                    return;
                }

                //写数据块2，端口号（5字节）
                block = 2;
                result_WriteIC = CardCommon.WriteIC(icdev, sec, block, hex_Port2.PadRight(32, '0'));
                if (result_WriteIC != "")
                {
                    this.lbState.Text = InfoSys.InfoWriteFailure(sec, block, InfoSys.CardTypeStrNetSet, InfoSys.StrOpenFailure);
                    pf.Log(this.lbState.Text);
                    pf.EndLog(InfoSys.CardTypeStrNetSet, InfoSys.MethodOpenCard);
                    return;
                }
            }
            else
            {
                this.lbState.Text = InfoSys.StrCannotOpen + InfoSys.StrOpenFailure;
                pf.Log(this.lbState.Text);
                pf.EndLog(InfoSys.CardTypeStrNetSet, InfoSys.MethodOpenCard);
                return;
            }

            //设置扇区2内容
            sec = 4;
            //认证卡密码B
            result_AuthIC = CardCommon.AuthIC(icdev, mode, sec, key);
            pf.AuthLog(InfoSys.CardTypeStrNetSet, InfoSys.MethodOpenCard, sec, result_AuthIC);
            if (result_AuthIC == InfoSys.StrAuthSuccess)
            {
                //写数据块0，APN名称
                block = 0;
                result_WriteIC = CardCommon.WriteIC(icdev, sec, block, hex_APN2.PadRight(32, '0'));
                if (result_WriteIC != "")
                {
                    this.lbState.Text = InfoSys.InfoWriteFailure(sec, block, InfoSys.CardTypeStrNetSet, InfoSys.StrOpenFailure);
                    pf.Log(this.lbState.Text);
                    pf.EndLog(InfoSys.CardTypeStrNetSet, InfoSys.MethodOpenCard);
                    return;
                }

                //写数据块1，用户名
                block = 1;
                result_WriteIC = CardCommon.WriteIC(icdev, sec, block, hex_UserName2.PadRight(32, '0'));
                if (result_WriteIC != "")
                {
                    this.lbState.Text = InfoSys.InfoWriteFailure(sec, block, InfoSys.CardTypeStrNetSet, InfoSys.StrOpenFailure);
                    pf.Log(this.lbState.Text);
                    pf.EndLog(InfoSys.CardTypeStrNetSet, InfoSys.MethodOpenCard);
                    return;
                }

                //写数据块2，密码
                block = 2;
                result_WriteIC = CardCommon.WriteIC(icdev, sec, block, hex_Password2.PadRight(32, '0'));
                if (result_WriteIC != "")
                {
                    this.lbState.Text = InfoSys.InfoWriteFailure(sec, block, InfoSys.CardTypeStrNetSet, InfoSys.StrOpenFailure);
                    pf.Log(this.lbState.Text);
                    pf.EndLog(InfoSys.CardTypeStrNetSet, InfoSys.MethodOpenCard);
                    return;
                }
            }

            //设置扇区1内容
            sec = 5;
            //认证卡密码B
            result_AuthIC = CardCommon.AuthIC(icdev, mode, sec, key);
            pf.AuthLog(InfoSys.CardTypeStrNetSet, InfoSys.MethodOpenCard, sec, result_AuthIC);
            if (result_AuthIC == InfoSys.StrAuthSuccess)
            {
                //写数据块0，数据块0为卡类型（1字节）、是否域名访问（1字节）、有效IP地址长度（1字节）、有效端口号长度（1字节）
                //APN名称长度（1字节）、APN用户名长度（1字节）、APN密码长度（1字节）
                block = 0;
                result_WriteIC = CardCommon.WriteIC(icdev, sec, block, (InfoSys.CardTypeNetSet + IsDomain3 +
                    len_IP3 + len_Port3 + len_APN3 + len_UserName3 + len_Password3).PadRight(32, '0'));
                if (result_WriteIC != "")
                {
                    this.lbState.Text = InfoSys.InfoWriteFailure(sec, block, InfoSys.CardTypeStrNetSet, InfoSys.StrOpenFailure);
                    pf.Log(this.lbState.Text);
                    pf.EndLog(InfoSys.CardTypeStrNetSet, InfoSys.MethodOpenCard);
                    return;
                }

                //写数据块1，IP地址或域名地址
                block = 1;
                result_WriteIC = CardCommon.WriteIC(icdev, sec, block, hex_IP3.PadRight(32, '0'));
                if (result_WriteIC != "")
                {
                    this.lbState.Text = InfoSys.InfoWriteFailure(sec, block, InfoSys.CardTypeStrNetSet, InfoSys.StrOpenFailure);
                    pf.Log(this.lbState.Text);
                    pf.EndLog(InfoSys.CardTypeStrNetSet, InfoSys.MethodOpenCard);
                    return;
                }

                //写数据块2，端口号（5字节）
                block = 2;
                result_WriteIC = CardCommon.WriteIC(icdev, sec, block, hex_Port3.PadRight(32, '0'));
                if (result_WriteIC != "")
                {
                    this.lbState.Text = InfoSys.InfoWriteFailure(sec, block, InfoSys.CardTypeStrNetSet, InfoSys.StrOpenFailure);
                    pf.Log(this.lbState.Text);
                    pf.EndLog(InfoSys.CardTypeStrNetSet, InfoSys.MethodOpenCard);
                    return;
                }
            }
            else
            {
                this.lbState.Text = InfoSys.StrCannotOpen + InfoSys.StrOpenFailure;
                pf.Log(this.lbState.Text);
                pf.EndLog(InfoSys.CardTypeStrNetSet, InfoSys.MethodOpenCard);
                return;
            }

            //设置扇区2内容
            sec =6;
            //认证卡密码B
            result_AuthIC = CardCommon.AuthIC(icdev, mode, sec, key);
            pf.AuthLog(InfoSys.CardTypeStrNetSet, InfoSys.MethodOpenCard, sec, result_AuthIC);
            if (result_AuthIC == InfoSys.StrAuthSuccess)
            {
                //写数据块0，APN名称
                block = 0;
                result_WriteIC = CardCommon.WriteIC(icdev, sec, block, hex_APN3.PadRight(32, '0'));
                if (result_WriteIC != "")
                {
                    this.lbState.Text = InfoSys.InfoWriteFailure(sec, block, InfoSys.CardTypeStrNetSet, InfoSys.StrOpenFailure);
                    pf.Log(this.lbState.Text);
                    pf.EndLog(InfoSys.CardTypeStrNetSet, InfoSys.MethodOpenCard);
                    return;
                }

                //写数据块1，用户名
                block = 1;
                result_WriteIC = CardCommon.WriteIC(icdev, sec, block, hex_UserName3.PadRight(32, '0'));
                if (result_WriteIC != "")
                {
                    this.lbState.Text = InfoSys.InfoWriteFailure(sec, block, InfoSys.CardTypeStrNetSet, InfoSys.StrOpenFailure);
                    pf.Log(this.lbState.Text);
                    pf.EndLog(InfoSys.CardTypeStrNetSet, InfoSys.MethodOpenCard);
                    return;
                }

                //写数据块2，密码
                block = 2;
                result_WriteIC = CardCommon.WriteIC(icdev, sec, block, hex_Password3.PadRight(32, '0'));
                if (result_WriteIC != "")
                {
                    this.lbState.Text = InfoSys.InfoWriteFailure(sec, block, InfoSys.CardTypeStrNetSet, InfoSys.StrOpenFailure);
                    pf.Log(this.lbState.Text);
                    pf.EndLog(InfoSys.CardTypeStrNetSet, InfoSys.MethodOpenCard);
                    return;
                }
            }


            //end add 

            for (int i = 0; i < pf.getSize(); i++)
            {
                result_AuthIC = CardCommon.AuthIC(icdev, mode, i, key);
                if (result_AuthIC == InfoSys.StrAuthSuccess)
                {
                    //写数据块3，密码eeeeeeeeeeee
                    block = 3;
                    CardCommon.WritePWD(icdev, i, block, keyNew, InfoSys.KeyControl, keyNew);
                }
            }

            SerialNumber_old = "";
            this.lbCardType.Text = InfoSys.CardTypeNetSet;
            this.lbState.Text = InfoSys.StrOpenSuccess;
            pf.Log(this.lbState.Text);
            pf.EndLog(InfoSys.CardTypeStrNetSet, InfoSys.MethodOpenCard);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (SerialNumber_old == "")
            {
                MessageBox.Show("请先读卡再修改卡！");
                return;
            }

            if (MessageBox.Show("确定" + InfoSys.CardTypeStrNetSet + InfoSys.MethodModifyCard + "？",
                InfoSys.CardTypeStrNetSet + InfoSys.MethodModifyCard, MessageBoxButtons.YesNo,
                MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
            {
                return;
            }

            pf.BeginLog(InfoSys.CardTypeStrNetSet, InfoSys.MethodModifyCard);

            if (!CheckValue())
            {
                pf.EndLog(InfoSys.CardTypeStrNetSet, InfoSys.MethodModifyCard);
                return;
            }

            if (!pf.SeedIC())
            {
                this.lbCardType.Text = InfoSys.StrSeekFailure;
                pf.EndLog(InfoSys.CardTypeStrNetSet, InfoSys.MethodModifyCard);
                return;
            }

            this.lbSerialNumber.Text = pf.getSnr().ToString("X");

            if (SerialNumber_old != this.lbSerialNumber.Text)
            {
                MessageBox.Show("请重新读卡再修改卡！");
                pf.EndLog(InfoSys.CardTypeStrNetSet, InfoSys.MethodModifyCard);
                return;
            }

            //保存远程服务器数据库
            string str = DataTransfer.ModifyCardNetSet(this.lbSerialNumber.Text, IP, Port, IsDomain, APN, UserName, Password);
            JavaScriptObject result = (JavaScriptObject)JavaScriptConvert.DeserializeObject(str);
            if (!bool.Parse(result["Result"].ToString()))
            {
                string txt = result["Message"].ToString();
                MessageBox.Show(txt);
                pf.Log(txt);
                pf.EndLog(InfoSys.CardTypeStrNetSet, InfoSys.MethodOpenCard);
                return;
            }

            int mode = 4; //以B密码认证
            int sec = 1; //扇区
            int block = 0;
            string key = pf.getKeyB(); //读卡密码
            string result_WriteIC = "";
            string result_AuthIC = "";

            string hex_IP = HexStringUtility.StrToHexString(IP);
            string hex_Port = HexStringUtility.StrToHexString(Port);
            string hex_APN = HexStringUtility.StrToHexString(APN);
            string hex_UserName = HexStringUtility.StrToHexString(UserName);
            string hex_Password = HexStringUtility.StrToHexString(Password);

            string len_IP = (hex_IP.Length / 2).ToString("X").PadLeft(2, '0');
            string len_Port = (hex_Port.Length / 2).ToString("X").PadLeft(2, '0');
            string len_APN = (hex_APN.Length / 2).ToString("X").PadLeft(2, '0');
            string len_UserName = (hex_UserName.Length / 2).ToString("X").PadLeft(2, '0');
            string len_Password = (hex_Password.Length / 2).ToString("X").PadLeft(2, '0');

            //start add by kqz 2017-5-9 10:40
            string hex_IP2 = HexStringUtility.StrToHexString(IP2);
            string hex_Port2 = HexStringUtility.StrToHexString(Port2);
            string hex_APN2 = HexStringUtility.StrToHexString(APN2);
            string hex_UserName2 = HexStringUtility.StrToHexString(UserName2);
            string hex_Password2 = HexStringUtility.StrToHexString(Password2);

            string len_IP2 = (hex_IP2.Length / 2).ToString("X").PadLeft(2, '0');
            string len_Port2 = (hex_Port2.Length / 2).ToString("X").PadLeft(2, '0');
            string len_APN2 = (hex_APN2.Length / 2).ToString("X").PadLeft(2, '0');
            string len_UserName2 = (hex_UserName2.Length / 2).ToString("X").PadLeft(2, '0');
            string len_Password2 = (hex_Password2.Length / 2).ToString("X").PadLeft(2, '0');

            string hex_IP3 = HexStringUtility.StrToHexString(IP3);
            string hex_Port3 = HexStringUtility.StrToHexString(Port3);
            string hex_APN3 = HexStringUtility.StrToHexString(APN3);
            string hex_UserName3 = HexStringUtility.StrToHexString(UserName3);
            string hex_Password3 = HexStringUtility.StrToHexString(Password3);

            string len_IP3 = (hex_IP3.Length / 2).ToString("X").PadLeft(2, '0');
            string len_Port3 = (hex_Port3.Length / 2).ToString("X").PadLeft(2, '0');
            string len_APN3 = (hex_APN3.Length / 2).ToString("X").PadLeft(2, '0');
            string len_UserName3 = (hex_UserName3.Length / 2).ToString("X").PadLeft(2, '0');
            string len_Password3 = (hex_Password3.Length / 2).ToString("X").PadLeft(2, '0');
            //end add

            //设置扇区1内容
            sec = 1;
            //认证卡密码B
            result_AuthIC = CardCommon.AuthIC(icdev, mode, sec, key);
            pf.AuthLog(InfoSys.CardTypeStrNetSet, InfoSys.MethodModifyCard, sec, result_AuthIC);
            if (result_AuthIC == InfoSys.StrAuthSuccess)
            {
                //写数据块0，数据块0为卡类型（1字节）、是否域名访问（1字节）、有效IP地址长度（1字节）、有效端口号长度（1字节）
                //APN名称长度（1字节）、APN用户名长度（1字节）、APN密码长度（1字节）
                block = 0;
                result_WriteIC = CardCommon.WriteIC(icdev, sec, block, (InfoSys.CardTypeNetSet + IsDomain +
                    len_IP + len_Port + len_APN + len_UserName + len_Password).PadRight(32, '0'));
                if (result_WriteIC != "")
                {
                    this.lbState.Text = InfoSys.InfoWriteFailure(sec, block, InfoSys.CardTypeStrNetSet, InfoSys.StrModifyFailure);
                    pf.Log(this.lbState.Text);
                    pf.EndLog(InfoSys.CardTypeStrNetSet, InfoSys.MethodModifyCard);
                    return;
                }

                //写数据块1，IP地址或域名地址
                block = 1;
                result_WriteIC = CardCommon.WriteIC(icdev, sec, block, hex_IP.PadRight(32, '0'));
                if (result_WriteIC != "")
                {
                    this.lbState.Text = InfoSys.InfoWriteFailure(sec, block, InfoSys.CardTypeStrNetSet, InfoSys.StrModifyFailure);
                    pf.Log(this.lbState.Text);
                    pf.EndLog(InfoSys.CardTypeStrNetSet, InfoSys.MethodModifyCard);
                    return;
                }

                //写数据块2，端口号（5字节）
                block = 2;
                result_WriteIC = CardCommon.WriteIC(icdev, sec, block, hex_Port.PadRight(32, '0'));
                if (result_WriteIC != "")
                {
                    this.lbState.Text = InfoSys.InfoWriteFailure(sec, block, InfoSys.CardTypeStrNetSet, InfoSys.StrModifyFailure);
                    pf.Log(this.lbState.Text);
                    pf.EndLog(InfoSys.CardTypeStrNetSet, InfoSys.MethodModifyCard);
                    return;
                }
            }

            //设置扇区2内容
            sec = 2;
            //认证卡密码B
            result_AuthIC = CardCommon.AuthIC(icdev, mode, sec, key);
            pf.AuthLog(InfoSys.CardTypeStrNetSet, InfoSys.MethodOpenCard, sec, result_AuthIC);
            if (result_AuthIC == InfoSys.StrAuthSuccess)
            {
                //写数据块0，APN名称
                block = 0;
                result_WriteIC = CardCommon.WriteIC(icdev, sec, block, hex_APN.PadRight(32, '0'));
                if (result_WriteIC != "")
                {
                    this.lbState.Text = InfoSys.InfoWriteFailure(sec, block, InfoSys.CardTypeStrNetSet, InfoSys.StrModifyFailure);
                    pf.Log(this.lbState.Text);
                    pf.EndLog(InfoSys.CardTypeStrNetSet, InfoSys.MethodModifyCard);
                    return;
                }

                //写数据块1，用户名
                block = 1;
                result_WriteIC = CardCommon.WriteIC(icdev, sec, block, hex_UserName.PadRight(32, '0'));
                if (result_WriteIC != "")
                {
                    this.lbState.Text = InfoSys.InfoWriteFailure(sec, block, InfoSys.CardTypeStrNetSet, InfoSys.StrModifyFailure);
                    pf.Log(this.lbState.Text);
                    pf.EndLog(InfoSys.CardTypeStrNetSet, InfoSys.MethodModifyCard);
                    return;
                }

                //写数据块2，密码
                block = 2;
                result_WriteIC = CardCommon.WriteIC(icdev, sec, block, hex_Password.PadRight(32, '0'));
                if (result_WriteIC != "")
                {
                    this.lbState.Text = InfoSys.InfoWriteFailure(sec, block, InfoSys.CardTypeStrNetSet, InfoSys.StrModifyFailure);
                    pf.Log(this.lbState.Text);
                    pf.EndLog(InfoSys.CardTypeStrNetSet, InfoSys.MethodModifyCard);
                    return;
                }
            }


            //start add by kqz 2017-5-9 10:42 
            //设置扇区1内容
            sec = 3;
            //认证卡密码B
            result_AuthIC = CardCommon.AuthIC(icdev, mode, sec, key);
            pf.AuthLog(InfoSys.CardTypeStrNetSet, InfoSys.MethodOpenCard, sec, result_AuthIC);
            if (result_AuthIC == InfoSys.StrAuthSuccess)
            {
                //写数据块0，数据块0为卡类型（1字节）、是否域名访问（1字节）、有效IP地址长度（1字节）、有效端口号长度（1字节）
                //APN名称长度（1字节）、APN用户名长度（1字节）、APN密码长度（1字节）
                block = 0;
                result_WriteIC = CardCommon.WriteIC(icdev, sec, block, (InfoSys.CardTypeNetSet + IsDomain2 +
                    len_IP2 + len_Port2 + len_APN2 + len_UserName2 + len_Password2).PadRight(32, '0'));
                if (result_WriteIC != "")
                {
                    this.lbState.Text = InfoSys.InfoWriteFailure(sec, block, InfoSys.CardTypeStrNetSet, InfoSys.StrOpenFailure);
                    pf.Log(this.lbState.Text);
                    pf.EndLog(InfoSys.CardTypeStrNetSet, InfoSys.MethodOpenCard);
                    return;
                }

                //写数据块1，IP地址或域名地址
                block = 1;
                result_WriteIC = CardCommon.WriteIC(icdev, sec, block, hex_IP2.PadRight(32, '0'));
                if (result_WriteIC != "")
                {
                    this.lbState.Text = InfoSys.InfoWriteFailure(sec, block, InfoSys.CardTypeStrNetSet, InfoSys.StrOpenFailure);
                    pf.Log(this.lbState.Text);
                    pf.EndLog(InfoSys.CardTypeStrNetSet, InfoSys.MethodOpenCard);
                    return;
                }

                //写数据块2，端口号（5字节）
                block = 2;
                result_WriteIC = CardCommon.WriteIC(icdev, sec, block, hex_Port2.PadRight(32, '0'));
                if (result_WriteIC != "")
                {
                    this.lbState.Text = InfoSys.InfoWriteFailure(sec, block, InfoSys.CardTypeStrNetSet, InfoSys.StrOpenFailure);
                    pf.Log(this.lbState.Text);
                    pf.EndLog(InfoSys.CardTypeStrNetSet, InfoSys.MethodOpenCard);
                    return;
                }
            }
            else
            {
                this.lbState.Text = InfoSys.StrCannotOpen + InfoSys.StrOpenFailure;
                pf.Log(this.lbState.Text);
                pf.EndLog(InfoSys.CardTypeStrNetSet, InfoSys.MethodOpenCard);
                return;
            }

            //设置扇区2内容
            sec = 4;
            //认证卡密码B
            result_AuthIC = CardCommon.AuthIC(icdev, mode, sec, key);
            pf.AuthLog(InfoSys.CardTypeStrNetSet, InfoSys.MethodOpenCard, sec, result_AuthIC);
            if (result_AuthIC == InfoSys.StrAuthSuccess)
            {
                //写数据块0，APN名称
                block = 0;
                result_WriteIC = CardCommon.WriteIC(icdev, sec, block, hex_APN2.PadRight(32, '0'));
                if (result_WriteIC != "")
                {
                    this.lbState.Text = InfoSys.InfoWriteFailure(sec, block, InfoSys.CardTypeStrNetSet, InfoSys.StrOpenFailure);
                    pf.Log(this.lbState.Text);
                    pf.EndLog(InfoSys.CardTypeStrNetSet, InfoSys.MethodOpenCard);
                    return;
                }

                //写数据块1，用户名
                block = 1;
                result_WriteIC = CardCommon.WriteIC(icdev, sec, block, hex_UserName2.PadRight(32, '0'));
                if (result_WriteIC != "")
                {
                    this.lbState.Text = InfoSys.InfoWriteFailure(sec, block, InfoSys.CardTypeStrNetSet, InfoSys.StrOpenFailure);
                    pf.Log(this.lbState.Text);
                    pf.EndLog(InfoSys.CardTypeStrNetSet, InfoSys.MethodOpenCard);
                    return;
                }

                //写数据块2，密码
                block = 2;
                result_WriteIC = CardCommon.WriteIC(icdev, sec, block, hex_Password2.PadRight(32, '0'));
                if (result_WriteIC != "")
                {
                    this.lbState.Text = InfoSys.InfoWriteFailure(sec, block, InfoSys.CardTypeStrNetSet, InfoSys.StrOpenFailure);
                    pf.Log(this.lbState.Text);
                    pf.EndLog(InfoSys.CardTypeStrNetSet, InfoSys.MethodOpenCard);
                    return;
                }
            }

            //设置扇区1内容
            sec = 5;
            //认证卡密码B
            result_AuthIC = CardCommon.AuthIC(icdev, mode, sec, key);
            pf.AuthLog(InfoSys.CardTypeStrNetSet, InfoSys.MethodOpenCard, sec, result_AuthIC);
            if (result_AuthIC == InfoSys.StrAuthSuccess)
            {
                //写数据块0，数据块0为卡类型（1字节）、是否域名访问（1字节）、有效IP地址长度（1字节）、有效端口号长度（1字节）
                //APN名称长度（1字节）、APN用户名长度（1字节）、APN密码长度（1字节）
                block = 0;
                result_WriteIC = CardCommon.WriteIC(icdev, sec, block, (InfoSys.CardTypeNetSet + IsDomain3 +
                    len_IP3 + len_Port3 + len_APN3 + len_UserName3 + len_Password3).PadRight(32, '0'));
                if (result_WriteIC != "")
                {
                    this.lbState.Text = InfoSys.InfoWriteFailure(sec, block, InfoSys.CardTypeStrNetSet, InfoSys.StrOpenFailure);
                    pf.Log(this.lbState.Text);
                    pf.EndLog(InfoSys.CardTypeStrNetSet, InfoSys.MethodOpenCard);
                    return;
                }

                //写数据块1，IP地址或域名地址
                block = 1;
                result_WriteIC = CardCommon.WriteIC(icdev, sec, block, hex_IP3.PadRight(32, '0'));
                if (result_WriteIC != "")
                {
                    this.lbState.Text = InfoSys.InfoWriteFailure(sec, block, InfoSys.CardTypeStrNetSet, InfoSys.StrOpenFailure);
                    pf.Log(this.lbState.Text);
                    pf.EndLog(InfoSys.CardTypeStrNetSet, InfoSys.MethodOpenCard);
                    return;
                }

                //写数据块2，端口号（5字节）
                block = 2;
                result_WriteIC = CardCommon.WriteIC(icdev, sec, block, hex_Port3.PadRight(32, '0'));
                if (result_WriteIC != "")
                {
                    this.lbState.Text = InfoSys.InfoWriteFailure(sec, block, InfoSys.CardTypeStrNetSet, InfoSys.StrOpenFailure);
                    pf.Log(this.lbState.Text);
                    pf.EndLog(InfoSys.CardTypeStrNetSet, InfoSys.MethodOpenCard);
                    return;
                }
            }
            else
            {
                this.lbState.Text = InfoSys.StrCannotOpen + InfoSys.StrOpenFailure;
                pf.Log(this.lbState.Text);
                pf.EndLog(InfoSys.CardTypeStrNetSet, InfoSys.MethodOpenCard);
                return;
            }

            //设置扇区2内容
            sec = 6;
            //认证卡密码B
            result_AuthIC = CardCommon.AuthIC(icdev, mode, sec, key);
            pf.AuthLog(InfoSys.CardTypeStrNetSet, InfoSys.MethodOpenCard, sec, result_AuthIC);
            if (result_AuthIC == InfoSys.StrAuthSuccess)
            {
                //写数据块0，APN名称
                block = 0;
                result_WriteIC = CardCommon.WriteIC(icdev, sec, block, hex_APN3.PadRight(32, '0'));
                if (result_WriteIC != "")
                {
                    this.lbState.Text = InfoSys.InfoWriteFailure(sec, block, InfoSys.CardTypeStrNetSet, InfoSys.StrOpenFailure);
                    pf.Log(this.lbState.Text);
                    pf.EndLog(InfoSys.CardTypeStrNetSet, InfoSys.MethodOpenCard);
                    return;
                }

                //写数据块1，用户名
                block = 1;
                result_WriteIC = CardCommon.WriteIC(icdev, sec, block, hex_UserName3.PadRight(32, '0'));
                if (result_WriteIC != "")
                {
                    this.lbState.Text = InfoSys.InfoWriteFailure(sec, block, InfoSys.CardTypeStrNetSet, InfoSys.StrOpenFailure);
                    pf.Log(this.lbState.Text);
                    pf.EndLog(InfoSys.CardTypeStrNetSet, InfoSys.MethodOpenCard);
                    return;
                }

                //写数据块2，密码
                block = 2;
                result_WriteIC = CardCommon.WriteIC(icdev, sec, block, hex_Password3.PadRight(32, '0'));
                if (result_WriteIC != "")
                {
                    this.lbState.Text = InfoSys.InfoWriteFailure(sec, block, InfoSys.CardTypeStrNetSet, InfoSys.StrOpenFailure);
                    pf.Log(this.lbState.Text);
                    pf.EndLog(InfoSys.CardTypeStrNetSet, InfoSys.MethodOpenCard);
                    return;
                }
            }


            //end add 

            SerialNumber_old = "";
            this.lbCardType.Text = InfoSys.CardTypeNetSet;
            this.lbState.Text = InfoSys.StrModifySuccess;
            pf.Log(this.lbState.Text);
            pf.EndLog(InfoSys.CardTypeStrNetSet, InfoSys.MethodModifyCard);
        }

        string IP = "";
        string Port = "";
        string IsDomain = "";
        string APN = "";
        string UserName = "";
        string Password = "";

        string IP2 = "";
        string Port2 = "";
        string IsDomain2 = "";
        string APN2 = "";
        string UserName2 = "";
        string Password2 = "";
        //start add by kqz 2017-5-9 10:36
        string IP3 = "";
        string Port3 = "";
        string IsDomain3 = "";
        string APN3 = "";
        string UserName3 = "";
        string Password3 = "";
        //end add
        private bool CheckValue()
        {
            IP = this.txtIP.Text.Trim();
            if (IP.Length < 1 || IP.Length > 16)
            {
                MessageBox.Show("IP地址或域名最长16个英文字符，不能为空");
                return false;
            }

            Port = this.txtPort.Text.Trim();
            if (!Tools.CheckValue(Port, "int", 0, 0, 1, 65535))
            {
                MessageBox.Show("端口好范围为 1-65535");
                return false;
            }

            IsDomain = this.ckbIsDomain.Checked ? "01" : "00";

            APN = this.txtAPN.Text.Trim();
            if (APN.Length > 16)
            {
                MessageBox.Show("APN名称最长16个英文字符");
                return false;
            }

            UserName = this.txtUserName.Text.Trim();
            if (UserName.Length > 16)
            {
                MessageBox.Show("用户名最长16个英文字符");
                return false;
            }

            Password = this.txtPassword.Text.Trim();
            if (Password.Length > 16)
            {
                MessageBox.Show("密码最长16个英文字符");
                return false;
            }

            //start add by kqz 2017-5-9 10:36
            IP2 = this.txtIP2.Text.Trim();
            if (IP2.Length < 1 || IP2.Length > 16)
            {
                MessageBox.Show("IP地址或域名2最长16个英文字符，不能为空");
                return false;
            }

            Port2 = this.txtPort2.Text.Trim();
            if (!Tools.CheckValue(Port2, "int", 0, 0, 1, 65535))
            {
                MessageBox.Show("端口好范围为 1-65535");
                return false;
            }

            IsDomain2 = this.ckbIsDomain2.Checked ? "01" : "00";

            APN2 = this.txtApn2.Text.Trim();
            if (APN2.Length > 16)
            {
                MessageBox.Show("APN2名称最长16个英文字符");
                return false;
            }

            UserName2 = this.txtUserName2.Text.Trim();
            if (UserName2.Length > 16)
            {
                MessageBox.Show("用户名2最长16个英文字符");
                return false;
            }

            Password2 = this.txtPassword2.Text.Trim();
            if (Password2.Length > 16)
            {
                MessageBox.Show("密码2最长16个英文字符");
                return false;
            }

            IP3 = this.txtIP3.Text.Trim();
            if (IP3.Length < 1 || IP3.Length > 16)
            {
                MessageBox.Show("IP地址或域名3最长16个英文字符，不能为空");
                return false;
            }

            Port3 = this.txtPort3.Text.Trim();
            if (!Tools.CheckValue(Port3, "int", 0, 0, 1, 65535))
            {
                MessageBox.Show("端口好范围为 1-65535");
                return false;
            }

            IsDomain3 = this.ckbIsDomain3.Checked ? "01" : "00";

            APN3 = this.txtApn3.Text.Trim();
            if (APN3.Length > 16)
            {
                MessageBox.Show("APN名称3最长16个英文字符");
                return false;
            }

            UserName3 = this.txtUserName3.Text.Trim();
            if (UserName3.Length > 16)
            {
                MessageBox.Show("用户名3最长16个英文字符");
                return false;
            }

            Password3 = this.txtPassword3.Text.Trim();
            if (Password3.Length > 16)
            {
                MessageBox.Show("密码3最长16个英文字符");
                return false;
            }
            //end add;

            return true;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (SerialNumber_old == "")
            {
                MessageBox.Show("请先读卡再注销卡！");
                return;
            }

            if (this.lbCardType.Text != InfoSys.CardTypeNetSet)
            {
                MessageBox.Show("非" + InfoSys.CardTypeStrNetSet + "无法注销！");
                return;
            }

            if (MessageBox.Show("确定" + InfoSys.CardTypeStrNetSet + InfoSys.MethodCancelCard + "？",
                InfoSys.CardTypeStrNetSet + InfoSys.MethodCancelCard, MessageBoxButtons.YesNo,
                MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
            {
                return;
            }

            pf.BeginLog(InfoSys.CardTypeStrNetSet, InfoSys.MethodCancelCard);

            if (!pf.SeedIC())
            {
                this.lbCardType.Text = InfoSys.StrSeekFailure;
                pf.EndLog(InfoSys.CardTypeStrNetSet, InfoSys.MethodCancelCard);
                return;
            }

            this.lbSerialNumber.Text = pf.getSnr().ToString("X");

            if (SerialNumber_old != this.lbSerialNumber.Text)
            {
                MessageBox.Show("请重新读卡再注销卡！");
                pf.EndLog(InfoSys.CardTypeStrNetSet, InfoSys.MethodCancelCard);
                return;
            }

            //保存远程服务器数据库
            string str = DataTransfer.CancelCardNetSet(this.lbSerialNumber.Text);
            JavaScriptObject result = (JavaScriptObject)JavaScriptConvert.DeserializeObject(str);
            if (!bool.Parse(result["Result"].ToString()))
            {
                string txt = result["Message"].ToString();
                MessageBox.Show(txt);
                pf.Log(txt);
                pf.EndLog(InfoSys.CardTypeStrNetSet, InfoSys.MethodCancelCard);
                return;
            }

            int mode = 0; //以A密码认证
            string key = pf.getKeyB(); //读卡密码
            string keyOld = pf.getKeyA(); //读卡密码
            string result_WriteIC = "";
            string result_AuthIC = "";

            for (int i = 0; i < pf.getSize(); i++)
            {
                result_AuthIC = CardCommon.AuthIC(icdev, mode, i, key);
                pf.AuthLog(InfoSys.CardTypeStrNetSet, InfoSys.MethodCancelCard, i, result_AuthIC);
                if (result_AuthIC != InfoSys.StrAuthSuccess)
                {
                    continue;
                }
                else
                {
                    int begin = 0;
                    if (i == 0)
                    {
                        begin = 1;
                    }

                    for (int j = begin; j < 3; j++)
                    {
                        result_WriteIC = CardCommon.WriteIC(icdev, i, j, "".PadRight(32, '0'));
                        pf.Log("注销卡扇区：" + i + " 数据块：" + j + " 结果：" + (result_WriteIC == "" ? "成功" : result_WriteIC));
                    }

                    {
                        int block = 3;
                        result_WriteIC = CardCommon.WriteIC(icdev, i, block, keyOld + InfoSys.KeyControl + keyOld);
                        pf.Log("注销卡写密码扇区：" + i + " 数据块：" + block + " 结果：" + (result_WriteIC == "" ? "成功" : result_WriteIC));
                    }
                }
            }

            SerialNumber_old = "";
            this.lbState.Text = InfoSys.MethodCancelCard + "结束";
            pf.EndLog(InfoSys.CardTypeStrNetSet, InfoSys.MethodCancelCard);
        }
    }
}
