using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using U3d = UnityEngine;

namespace M2Image
{
    /// <summary>
    /// WIL(与WIX配套)文件读取工具类
    /// </summary>
    internal sealed class WIL : IDisposable
    {
        /// <summary>
        /// 图片数量
        /// </summary>
        internal int ImageCount { get; private set; }
        /// <summary>
        /// 色深度
        /// </summary>
        internal int ColorCount { get; private set; }
        /* 版本标识 */
        private int VerFlag;
         
        /// <summary>
        /// 图片数据起始位置
        /// </summary>
        private int[] OffsetList;
        /// <summary>
        /// 图片描述对象
        /// </summary>
        internal M2ImageInfo[] ImageInfos { get; private set; }
        /// <summary>
        /// WIL文件指针
        /// </summary>
        private FileStream FS_wil;
		/// <summary>
		/// WIL随机读取器
		/// </summary>
		private BinaryReader BR_wil;
        /// <summary>
        /// 是否成功初始化
        /// </summary>
        internal bool Loaded { get; private set; }

        /// <summary>
        /// 文件指针读取锁
        /// </summary>
        private Object wil_locker = new Object();

        internal WIL(String wilPath)
        {
            Loaded = false;
            VerFlag = 0;
            if (!File.Exists(Path.ChangeExtension(wilPath, "wix"))) return;
			FileStream fs_wix = new FileStream(Path.ChangeExtension(wilPath, "wix"), FileMode.Open, FileAccess.Read);
			FS_wil = new FileStream(wilPath, FileMode.Open, FileAccess.Read);
            fs_wix.Position += 44; // 跳过标题
            using (BinaryReader rwix = new BinaryReader(fs_wix))
            {
                int indexCount = rwix.ReadInt32(); // 索引数量(也是图片数量)
                if (VerFlag != 0)
                {
                    fs_wix.Position += 4; // 版本标识不为0需要跳过4字节
                }
                OffsetList = new int[indexCount + 1];
                for (int i = 0; i < indexCount; ++i)
                {
                    // 读取数据偏移量
                    OffsetList[i] = rwix.ReadInt32();
                }
                OffsetList[indexCount] = (int)FS_wil.Length;
            }
            //fs_wix.Dispose();
            FS_wil.Position += 44; // 跳过标题
			BR_wil = new BinaryReader(FS_wil);
			ImageCount = BR_wil.ReadInt32(); // 图片数量
			ColorCount = Delphi.ColorCountToBitCount(BR_wil.ReadInt32()); // 色深度
            if (ColorCount < 16)
            {
                // 8位灰度图可能版本标识不为0，此时操作不一样
                FS_wil.Position += 4; // 忽略调色板
				VerFlag = BR_wil.ReadInt32();
            }
            ImageInfos = new M2ImageInfo[ImageCount];
            for (int i = 0; i < ImageCount; ++i)
            {
                // 读取图片信息
                M2ImageInfo ii = new M2ImageInfo();
                FS_wil.Position = OffsetList[i];
				ii.Width = BR_wil.ReadUInt16();
				ii.Height = BR_wil.ReadUInt16();
				ii.OffsetX = BR_wil.ReadInt16();
				ii.OffsetY = BR_wil.ReadInt16();
                ImageInfos[i] = ii;
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
                lock (wil_locker)
                {
                    FS_wil.Position = OffsetList[index] + 8;
                    int pixelLength = OffsetList[index + 1] - OffsetList[index];
                    if (pixelLength < 13) return result;
					pixels = BR_wil.ReadBytes(pixelLength);
                }
                if (ColorCount == 8)
                {
                    int p_index = 0;
                    for (int h = ii.Height - 1; h >= 0; --h)
                        for (int w = 0; w < ii.Width; ++w)
                        {
                            // 跳过填充字节
                            if (w == 0)
                                p_index += Delphi.SkipBytes(8, ii.Width);
                            float[] pallete = Delphi.PALLETE[pixels[p_index++] & 0xff];
							result.SetPixel(w, ii.Height - h, new U3d.Color(pallete[1], pallete[2], pallete[3], pallete[0]));
                        }
                }
                else if (ColorCount == 16)
                {
                    using (MemoryStream mspixels = new MemoryStream(pixels))
                    {
                        using (BinaryReader rpixels = new BinaryReader(mspixels))
                        {
                            int p_index = 0;
                            for (int h = ii.Height - 1; h >= 0; --h)
                                for (int w = 0; w < ii.Width; ++w, p_index += 2)
                                {
                                    // 跳过填充字节
                                    if (w == 0)
                                        p_index += Delphi.SkipBytes(16, ii.Width);
                                    short pdata = rpixels.ReadInt16();
                                    float br = (((pdata & 0xF800) >> 8) / 255f);//byte br = (byte) ((data & 0b1111_1000_0000_0000) >> 8);// 由于是与16位做与操作，所以多出了后面8位
                                    float bg = (((pdata & 0x7E0) >> 3) / 255f);//byte bg = (byte) ((data & 0b0000_0111_1110_0000) >> 3);// 多出了3位，在强转时前8位会自动丢失
                                    float bb = (((pdata & 0x1F) << 3) / 255f);//byte bb = (byte) ((data & 0b0000_0000_0001_1111) << 3);// 少了3位
									result.SetPixel(w, ii.Height - h, new U3d.Color(br, bg, bb));
                                }
                        }
                    }
                }
				result.Apply ();
                return result;
            }
        }


        public void Dispose()
        {
            lock (wil_locker)
            {
                OffsetList = null;
                ImageInfos = null;
                Loaded = false;
				if (BR_wil != null)
                {
					((IDisposable)BR_wil).Dispose ();
                }
            }
        }
    }
}