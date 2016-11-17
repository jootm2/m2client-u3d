using System;
using System.Collections.Generic;
using System.Text;

namespace M2Image
{
    /// <summary>
    /// M2图片信息
    /// </summary>
    public sealed class M2ImageInfo
    {
        internal M2ImageInfo(){}

        /// <summary>
        /// 图片(像素)宽度
        /// </summary>
        public ushort Width { get; internal set; }
        /// <summary>
        /// 图片(像素)高度
        /// </summary>
        public ushort Height { get; internal set; }
        /// <summary>
        /// 横向(像素)偏移量
        /// </summary>
        public short OffsetX { get; internal set; }
        /// <summary>
        /// 纵向(像素)偏移量
        /// </summary>
        public short OffsetY { get; internal set; }
    }
}