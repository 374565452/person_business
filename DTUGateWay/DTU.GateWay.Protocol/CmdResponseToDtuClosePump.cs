﻿using Common;
using System;
using System.Collections.Generic;

namespace DTU.GateWay.Protocol
{
    public class CmdResponseToDtuClosePump : BaseMessage
    {
        public CmdResponseToDtuClosePump()
        {
            BeginChar = BaseProtocol.BeginChar;
            EndChar = BaseProtocol.EndChar;

            //AFN = 0x54;
            AFN = (byte)BaseProtocol.AFN.ToDtuClosePump;
            //ControlField = 0xB3;
            ControlField = (byte)BaseProtocol.ControlField.FromDtu;
        }

        public CmdResponseToDtuClosePump(BaseMessage bm)
        {
            BeginChar = BaseProtocol.BeginChar;
            EndChar = BaseProtocol.EndChar;

            //AFN = 0x54;
            AFN = (byte)BaseProtocol.AFN.ToDtuClosePump;
            //ControlField = 0xB3;
            ControlField = (byte)BaseProtocol.ControlField.FromDtu;

            this.RawDataStr = bm.RawDataStr;
            this.RawDataChar = bm.RawDataChar;
            this.Length = bm.Length;
            this.AddressField = bm.AddressField;
            this.StationType = bm.StationType;
            this.StationCode = bm.StationCode;
            this.UserData = bm.UserData;
            this.UserDataBytes = bm.UserDataBytes;
            this.CC = bm.CC;
            this.IsPW = bm.IsPW;
            this.PW = bm.PW;
            this.IsTP = bm.IsTP;
            this.TP = bm.TP;
        }

        /// <summary>
        /// 应答结果，0 失败，1 成功
        /// </summary>
        public byte Result
        {
            set;
            get;
        }

        public override byte[] WriteMsg()
        {
            string data = HexStringUtility.ByteArrayToHexString(new byte[] { Result });

            IsPW = false;
            PW = "";
            IsTP = false;
            TP = "";

            UserData = data;

            UserDataBytes = HexStringUtility.HexStringToByteArray(UserData);

            return WriteMsg2();
        }

        public override string ReadMsg()
        {
            try
            {
                string data = UserData;
                Result = HexStringUtility.HexStringToByteArray(UserData)[0];
                return "";
            }
            catch (Exception ex)
            {
                if (ShowLog)
                    logHelper.Error(ex.Message + Environment.NewLine + "获取远程关泵响应信息出错" + " " + RawDataStr);
                return "获取远程关泵响应信息出错";
            }
        }
    }
}
