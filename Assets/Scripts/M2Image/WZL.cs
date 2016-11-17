using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using U3d = UnityEngine;

namespace M2Image
{
    /// <summary>
    /// WZL(与WZX配套)文件读取工具类
    /// </summary>
    internal sealed class WZL : IDisposable
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
        private FileStream FS_wzl;
        /// <summary>
        /// 是否成功初始化
        /// </summary>
        internal bool Loaded { get; private set; }

        /// <summary>
        /// 文件指针读取锁
        /// </summary>
        private Object wzl_locker = new Object();

        internal WZL(String wzlPath)
        {
            Loaded = false;
            ColorCount = 8;
            if (!File.Exists(Path.ChangeExtension(wzlPath, "wzx"))) return;
            FileStream fs_wzx = new FileStream(Path.ChangeExtension(wzlPath, "wzx"), FileMode.Open, FileAccess.Read);
            fs_wzx.Position += 44; // 跳过标题
            using (BinaryReader rwzx = new BinaryReader(fs_wzx))
            {
                ImageCount = rwzx.ReadInt32();
                OffsetList = new int[ImageCount];
                for (int i = 0; i < ImageCount; ++i)
                {
                    // 读取数据偏移地址
                    OffsetList[i] = rwzx.ReadInt32();
                }
            }
            //fs_wzx.Dispose();
            FS_wzl = new FileStream(wzlPath, FileMode.Open, FileAccess.Read);
            using (BinaryReader rwzl = new BinaryReader(FS_wzl))
            {
                ImageInfos = new M2ImageInfo[ImageCount];
                LengthList = new int[ImageCount];
                for (int i = 0; i < ImageCount; ++i)
                {
                    // 读取图片信息和数据长度
                    M2ImageInfo ii = new M2ImageInfo();
                    FS_wzl.Position = OffsetList[i] + 4; // 跳过4字节未知数据
                    ii.Width = rwzl.ReadUInt16();
                    ii.Height = rwzl.ReadUInt16();
                    ii.OffsetX = rwzl.ReadInt16();
                    ii.OffsetY = rwzl.ReadInt16();
                    ImageInfos[i] = ii;
                    LengthList[i] = rwzl.ReadInt32();
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
                if (ii.Width == 0 && ii.Height == 0) return result;
                byte[] pixels = null;
                lock (wzl_locker)
                {
                    FS_wzl.Position = OffsetList[index] + 16;
                    using (BinaryReader rwzl = new BinaryReader(FS_wzl))
                    {
                        pixels = unzip(rwzl.ReadBytes(LengthList[index]));
                    }
                }
                int p_index = 0;
                for (int h = 0; h < ii.Height; ++h)
                    for (int w = 0; w < ii.Width; ++w)
                    {
                        // 跳过填充字节
                        if (w == 0)
                            p_index += Delphi.SkipBytes(8, ii.Width);
                        float[] pallete = Delphi.PALLETE[pixels[p_index++] & 0xff];
						result.SetPixel(w, ii.Height - h, new U3d.Color(pallete[1], pallete[2], pallete[3], pallete[0]));
					}
				result.Apply ();
                return result;
            }
        }

        private byte[] unzip(byte[] ziped)
        {
            InflaterInputStream iib = new InflaterInputStream(new MemoryStream(ziped));
            MemoryStream o = new MemoryStream();
            int i = 1024;
            byte[] buf = new byte[i];

            while ((i = iib.Read(buf, 0, i)) > 0)
            {
                o.Write(buf, 0, i);
            }
            return o.ToArray();
        }

        public void Dispose()
        {
            lock (wzl_locker)
            {
                OffsetList = null;
                ImageInfos = null;
                Loaded = false;
                if (FS_wzl != null)
                {
                    FS_wzl.Dispose();
                }
            }
        }
    }
}