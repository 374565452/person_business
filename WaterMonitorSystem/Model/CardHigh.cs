using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Maticsoft.Model
{
    /// <summary>
    /// 高级用户卡
    /// </summary>
    [Serializable]
    [DataContract]
    public class CardHigh
    {
        /// <summary>
        /// Id
        /// </summary>	
        [DataMember]
        public long Id { get; set; }

        /// <summary>
        /// IC卡序列号
        /// </summary>	
        [DataMember]
        public string SerialNumber { get; set; }

        /// <summary>
        /// 开卡操作人id
        /// </summary>	
        [DataMember]
        public long OpenUserId { get; set; }

        /// <summary>
        /// 开卡操作人名字
        /// </summary>	
        [DataMember]
        public string OpenUserName { get; set; }

        /// <summary>
        /// 开卡地址
        /// </summary>	
        [DataMember]
        public string OpenAddress { get; set; }

        /// <summary>
        /// 开卡时间
        /// </summary>	
        [DataMember]
        public DateTime OpenTime { get; set; }
    }
}
