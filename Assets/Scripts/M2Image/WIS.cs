using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using U3d = UnityEngine;

namespace M2Image
{
    /// <summary>
    /// WIS文件读取工具类
    /// <para>wis文件图片都为8位灰度图</para>
    /// </summary>
    internal sealed class WIS : IDisposable
    {
        /// <summary>
        /// 图片数量
        /// </summary>
        internal int ImageCount { get; private set; }
        /// <summary>
        /// 色深度
        /// </summary>
        internal int ColorCount { get; private set; }

        /// <summary>
        /// 图片数据起始位置
        /// </summary>
        private int[] OffsetList;
        /// <summary>
        /// 图片数据长度
        /// </summary>
        private int[] LengthList;
        /// <summary>
        /// 图片描述对象
        /// </summary>
        internal M2ImageInfo[] ImageInfos { get; private set; }
        /// <summary>
        /// WIL文件指针
        /// </summary>
        private FileStream FS_wis;
        /// <summary>
        /// 是否成功初始化
        /// </summary>
        internal bool Loaded { get; private set; }

        /// <summary>
        /// 文件指针读取锁
        /// </summary>
        private Object wis_locker = new Object();

        internal WIS(String wisPath)
        {
            Loaded = false;
            ColorCount = 8;
            FS_wis = new FileStream(wisPath, FileMode.Open, FileAccess.Read);
            List<int> listOffset = new List<int>();
            List<int> listLength = new List<int>();
            int currentOffset = 0;
            FS_wis.Position = FS_wis.Length - 12;
            using (BinaryReader rwis = new BinaryReader(FS_wis))
            {
                // 从文件末尾开始读取图片数据描述信息
                // 一组描述信息包括12个字节(3个int值)，依次为图片数据起始位置(相对于文件)、图片数据大小(包括基本信息)、保留
                // 使用两个List保存offsetList和lengthList
                do {
                    currentOffset = rwis.ReadInt32();
                    listOffset.Add(currentOffset);
                    listLength.Add(rwis.ReadInt32());
                    FS_wis.Position -= 12;
                } while (currentOffset > 512);
                listOffset.Reverse();
                OffsetList = listOffset.ToArray();
                listLength.Reverse();
                LengthList = listLength.ToArray();
                ImageCount = OffsetList.Length;
                // 读取图片信息
                ImageInfos = new M2ImageInfo[ImageCount];
                for (int i = 0; i < ImageCount; ++i)
                {
                    // 读取图片信息
                    M2ImageInfo ii = new M2ImageInfo();
                    FS_wis.Position = OffsetList[i] + 4;
                    ii.Width = rwis.ReadUInt16();
                    ii.Height = rwis.ReadUInt16();
                    ii.OffsetX = rwis.ReadInt16();
                    ii.OffsetY = rwis.ReadInt16();
                    ImageInfos[i] = ii;
                }
            }
            Loaded = true;
        }

        /// <summary>
        /// 获取某个索引的图片
        /// </summary>
        /// <param name="index">图片索引，从0开始</param>
        /// <returns>对应图片数据</returns>
        internal U3d.Texture2D this[uint index]
        {
            get
            {
                M2ImageInfo ii = ImageInfos[index];
                U3d.Texture2D result = new U3d.Texture2D(ii.Width, ii.Height);
                byte[] pixels = null;
                lock (wis_locker)
                {
                    FS_wis.Position = OffsetList[index];
                    using (BinaryReader rwis = new BinaryReader(FS_wis))
                    {
                        // 是否压缩(RLE)
                        byte compressFlag = rwis.ReadByte();
                        FS_wis.Position += 11;
                        pixels = rwis.ReadBytes(LengthList[index] - 12);
                        if (compressFlag == 1)
                        {
                            // 需要解压
                            pixels = unpack(pixels, pixels.Length);
                        }
                    }
                }
                int p_index = 0;
                for (int h = 0; h < ii.Height; ++h)
                    for (int w = 0; w < ii.Width; ++w)
                    {
                        float[] pallete = Delphi.PALLETE[pixels[p_index++] & 0xff];
						result.SetPixel(w, ii.Height - h, new U3d.Color(pallete[1], pallete[2], pallete[3], pallete[0]));
					}
				result.Apply ();
                return result;
            }
        }

        private byte[] unpack(byte[] packed, int unpackLength)
        {
            int srcLength = packed.Length; // 压缩后数据大小
            byte[] result = new byte[unpackLength]; // 解压后数据
            int srcIndex = 0; // 当前解压的字节索引
            int dstIndex = 0; // 解压过程还原出的字节索引
            // 解压过程为逐字节进行(字节应转为1-256)
            // 如果当前字节非0则表示将以下一个字节数据填充当前字节个字节位置
            // 如果当前字节为0且下一个字节不为0则表示从下下个字节开始到下一个字节长度都没有压缩，直接复制到目标数组
            // 如果当前字节为0且下一个字节也为0则可能是脏数据，不予处理
            // XX YY 表示以YY填充XX个字节
            // 00 XX YY ZZ ... 表示从YY开始XX个字节是未被压缩的，直接复制出来即可
            while (srcLength > 0 && unpackLength > 0)
            {
                int length = packed[srcIndex++] & 0xff; // 取出第一个标志位
                int value = packed[srcIndex++] & 0xff; // 取出第二个标志位
                srcLength -= 2;
                /*if(value == 0 && length == 0) {
                    // 脏数据
                    continue;
                } else */
                if (length != 0)
                {
                    // 需要解压缩
                    unpackLength -= length;
                    for (int i = 0; i < length; ++i)
                    {
                        result[dstIndex++] = (byte)value;
                    }
                }
                else if (value != 0)
                {
                    srcLength -= value;
                    unpackLength -= value;
                    Array.Copy(packed, srcIndex, result, dstIndex, value);
                    dstIndex += value;
                    srcIndex += value;
                }
            }
            return result;
        }

        public void Dispose()
        {
            lock (wis_locker)
            {
                OffsetList = null;
                ImageInfos = null;
                Loaded = false;
                if (FS_wis != null)
                {
                    FS_wis.Dispose();
                }
            }
        }
    }
}