using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using U3d = UnityEngine;

namespace M2Image
{
    /// <summary>
    /// M2图片SDK入口，所有请求都由它接收并处理
    /// </summary>
    public static class M2Image
    {
        /* 缓存的WIL文件信息集合 */
        private static Dictionary<String, WIL> WILS = new Dictionary<string, WIL>();
        /* 缓存的WIS文件信息集合 */
        private static Dictionary<String, WIS> WISS = new Dictionary<string, WIS>();
        /* 缓存的WZL文件信息集合 */
        private static Dictionary<String, WZL> WZLS = new Dictionary<string, WZL>();

        /// <summary>
        /// <para>获取一副图片</para>
        /// <para>函数不会报错，但可能返回null或width/height为0的图片</para>
        /// <para>函数不会对图片数据进行缓存，但是会对图片信息进行缓存</para>
        /// <para>这样的话第一次读取会慢一些，后续很快，如果需要释放某个库的内存使用，用GC函数</para>
        /// </summary>
        /// <param name="fn">图片库路径，可以是相对路径(注意，相对路径在很多情况下不可控！)</param>
        /// <param name="index">图片在库中的索引，从0开始</param>
        /// <returns>解析出来的图片</returns>
        public static U3d.Texture2D Image(String fn, uint index)
        {
            if (String.IsNullOrEmpty(fn)) return null;
            if (Path.HasExtension(fn))
            {
                try { if (!File.Exists(Path.GetFullPath(fn))) return null; }
                catch (Exception) { return null; }
                if (Path.GetExtension(fn).ToLower().Equals(".wil"))
                {
                    try { return ImageFromWIL(Path.GetFullPath(fn), index); }
                    catch (Exception) { }
                }
                else if (Path.GetExtension(fn).ToLower().Equals(".wis"))
                {
                    try { return ImageFromWIS(Path.GetFullPath(fn), index); }
                    catch (Exception) { }
                }
                else if (Path.GetExtension(fn).ToLower().Equals(".wzl"))
                {
                    try { return ImageFromWZL(Path.GetFullPath(fn), index); }
                    catch (Exception) { }
                }
            }
            else
            {
                // 从wil/wis/wzl都试一下
                fn = Path.ChangeExtension(fn, "wzl");
                try { if (File.Exists(Path.GetFullPath(fn))) return ImageFromWZL(Path.GetFullPath(fn), index); }
                catch (Exception) { }
                fn = Path.ChangeExtension(fn, "wis");
                try { if (File.Exists(Path.GetFullPath(fn))) return ImageFromWIS(Path.GetFullPath(fn), index); }
                catch (Exception) { }
                fn = Path.ChangeExtension(fn, "wil");
                try { if (File.Exists(Path.GetFullPath(fn))) return ImageFromWIL(Path.GetFullPath(fn), index); }
                catch (Exception) { }
            }
            return null;
        }

        /// <summary>
        /// <para>获取图片横向偏移量</para>
        /// <para>基于图片左下角坐标</para>
        /// </summary>
        /// <param name="fn">图片所在库</param>
        /// <param name="index">图片在库中索引</param>
        /// <returns>偏移量</returns>
        public static short OffsetX(String fn, uint index)
        {
            if (String.IsNullOrEmpty(fn)) return 0;
            if (Path.HasExtension(fn))
            {
                try { if (!File.Exists(Path.GetFullPath(fn))) return 0; }
                catch (Exception) { return 0; }
                if (Path.GetExtension(fn).ToLower().Equals(".wil"))
                {
                    try { return InfoFromWIL(Path.GetFullPath(fn), index).OffsetX; }
                    catch (Exception) { }
                }
                else if (Path.GetExtension(fn).ToLower().Equals(".wis"))
                {
                    try { return InfoFromWIS(Path.GetFullPath(fn), index).OffsetX; }
                    catch (Exception) { }
                }
                else if (Path.GetExtension(fn).ToLower().Equals(".wzl"))
                {
                    try { return InfoFromWZL(Path.GetFullPath(fn), index).OffsetX; }
                    catch (Exception) { }
                }
            }
            else
            {
                // 从wil/wis/wzl都试一下
                fn = Path.ChangeExtension(fn, "wzl");
                try { if (File.Exists(Path.GetFullPath(fn))) return InfoFromWZL(Path.GetFullPath(fn), index).OffsetX; }
                catch (Exception) { }
                fn = Path.ChangeExtension(fn, "wis");
                try { if (File.Exists(Path.GetFullPath(fn))) return InfoFromWIS(Path.GetFullPath(fn), index).OffsetX; }
                catch (Exception) { }
                fn = Path.ChangeExtension(fn, "wil");
                try { if (File.Exists(Path.GetFullPath(fn))) return InfoFromWIL(Path.GetFullPath(fn), index).OffsetX; }
                catch (Exception) { }
            }
            return 0;
        }

        /// <summary>
        /// <para>获取图片纵向偏移量</para>
        /// <para>基于图片左下角坐标</para>
        /// </summary>
        /// <param name="fn">图片所在库</param>
        /// <param name="index">图片在库中索引</param>
        /// <returns>偏移量</returns>
        public static short OffsetY(String fn, uint index)
        {
            if (String.IsNullOrEmpty(fn)) return 0;
            if (Path.HasExtension(fn))
            {
                try { if (!File.Exists(Path.GetFullPath(fn))) return 0; }
                catch (Exception) { return 0; }
                if (Path.GetExtension(fn).ToLower().Equals(".wil"))
                {
                    try { return InfoFromWIL(Path.GetFullPath(fn), index).OffsetY; }
                    catch (Exception) { }
                }
                else if (Path.GetExtension(fn).ToLower().Equals(".wis"))
                {
                    try { return InfoFromWIS(Path.GetFullPath(fn), index).OffsetY; }
                    catch (Exception) { }
                }
                else if (Path.GetExtension(fn).ToLower().Equals(".wzl"))
                {
                    try { return InfoFromWZL(Path.GetFullPath(fn), index).OffsetY; }
                    catch (Exception) { }
                }
            }
            else
            {
                // 从wil/wis/wzl都试一下
                fn = Path.ChangeExtension(fn, "wzl");
                try { if (File.Exists(Path.GetFullPath(fn))) return InfoFromWZL(Path.GetFullPath(fn), index).OffsetY; }
                catch (Exception) { }
                fn = Path.ChangeExtension(fn, "wis");
                try { if (File.Exists(Path.GetFullPath(fn))) return InfoFromWIS(Path.GetFullPath(fn), index).OffsetY; }
                catch (Exception) { }
                fn = Path.ChangeExtension(fn, "wil");
                try { if (File.Exists(Path.GetFullPath(fn))) return InfoFromWIL(Path.GetFullPath(fn), index).OffsetY; }
                catch (Exception) { }
            }
            return 0;
        }

        /// <summary>
        /// <para>释放某个库的内存占用</para>
        /// <para>应该在确定不使用库的情况下或者需要更新库(从磁盘上删除/改写库文件)时调用</para>
        /// </summary>
        /// <param name="fn">需要释放内存的库</param>
        public static void GC(String fn)
        {
            if (String.IsNullOrEmpty(fn)) return;
            if (Path.HasExtension(fn))
            {
                String ffn = null;
                try { ffn = Path.GetFullPath(fn); }
                catch (Exception) { return; }
                if (Path.GetExtension(fn).ToLower().Equals(".wil"))
                {
                    lock (WILS)
                    {
                        if (WILS.ContainsKey(ffn))
                        {
                            WILS[ffn].Dispose();
                            WILS.Remove(ffn);
                        }
                    }
                }
                else if (Path.GetExtension(fn).ToLower().Equals(".wis"))
                {
                    lock (WISS)
                    {
                        if (WISS.ContainsKey(ffn))
                        {
                            WISS[ffn].Dispose();
                            WISS.Remove(ffn);
                        }
                    }
                }
                else if (Path.GetExtension(fn).ToLower().Equals(".wzl"))
                {
                    lock (WZLS)
                    {
                        if (WZLS.ContainsKey(ffn))
                        {
                            WZLS[ffn].Dispose();
                            WZLS.Remove(ffn);
                        }
                    }
                }
            }
            else
            {
                fn = Path.ChangeExtension(fn, "wil");
                try {
                    String ffn = Path.GetFullPath(fn);
                    lock (WILS)
                    {
                        if (WILS.ContainsKey(ffn))
                        {
                            WILS[ffn].Dispose();
                            WILS.Remove(ffn);
                        }
                    }
                }
                catch (Exception) { }
                fn = Path.ChangeExtension(fn, "wis");
                try
                {
                    String ffn = Path.GetFullPath(fn);
                    lock (WISS)
                    {
                        if (WISS.ContainsKey(ffn))
                        {
                            WISS[ffn].Dispose();
                            WISS.Remove(ffn);
                        }
                    }
                }
                catch (Exception) { }
                fn = Path.ChangeExtension(fn, "wzl");
                try
                {
                    String ffn = Path.GetFullPath(fn);
                    lock (WZLS)
                    {
                        if (WZLS.ContainsKey(ffn))
                        {
                            WZLS[ffn].Dispose();
                            WZLS.Remove(ffn);
                        }
                    }
                }
                catch (Exception) { }
            }
        }

        private static U3d.Texture2D ImageFromWZL(String fn, uint index)
        {
            WZL wzl = null;
            lock (WZLS)
            {
                if (WZLS.ContainsKey(fn)) wzl = WZLS[fn];
                else
                {
                    WZL _wzl = new WZL(fn);
                    if (_wzl.Loaded)
                    {
                        WZLS.Add(fn, _wzl);
                        wzl = _wzl;
                    }
                }
            }
            if (wzl == null || !wzl.Loaded || (wzl.ImageCount - 1) < index) return null;
            return wzl[index];
        }

        private static M2ImageInfo InfoFromWZL(String fn, uint index)
        {
            WZL wzl = null;
            lock (WZLS)
            {
                if (WZLS.ContainsKey(fn)) wzl = WZLS[fn];
                else
                {
                    WZL _wzl = new WZL(fn);
                    if (_wzl.Loaded)
                    {
                        WZLS.Add(fn, _wzl);
                        wzl = _wzl;
                    }
                }
            }
            if (wzl == null || !wzl.Loaded || (wzl.ImageCount - 1) < index) return null;
            return wzl.ImageInfos[index];
        }

        private static U3d.Texture2D ImageFromWIS(String fn, uint index)
        {
            WIS wis = null;
            lock (WISS)
            {
                if (WISS.ContainsKey(fn)) wis = WISS[fn];
                else
                {
                    WIS _wis = new WIS(fn);
                    if (_wis.Loaded)
                    {
                        WISS.Add(fn, _wis);
                        wis = _wis;
                    }
                }
            }
            if (wis == null || !wis.Loaded || (wis.ImageCount - 1) < index) return null;
            return wis[index];
        }

        private static M2ImageInfo InfoFromWIS(String fn, uint index)
        {
            WIS wis = null;
            lock (WISS)
            {
                if (WISS.ContainsKey(fn)) wis = WISS[fn];
                else
                {
                    WIS _wis = new WIS(fn);
                    if (_wis.Loaded)
                    {
                        WISS.Add(fn, _wis);
                        wis = _wis;
                    }
                }
            }
            if (wis == null || !wis.Loaded || (wis.ImageCount - 1) < index) return null;
            return wis.ImageInfos[index];
        }

        private static U3d.Texture2D ImageFromWIL(String fn, uint index)
        {
            WIL wil = null;
            lock (WILS)
            {
                if (WILS.ContainsKey(fn)) wil = WILS[fn];
                else
                {
                    WIL _wil = new WIL(fn);
                    if (_wil.Loaded)
                    {
                        WILS.Add(fn, _wil);
                        wil = _wil;
                    }
                }
            }
            if (wil == null || !wil.Loaded || (wil.ImageCount - 1) < index) return null;
            return wil[index];
        }

        private static M2ImageInfo InfoFromWIL(String fn, uint index)
        {
            WIL wil = null;
            lock (WILS)
            {
                if (WILS.ContainsKey(fn)) wil = WILS[fn];
                else
                {
                    WIL _wil = new WIL(fn);
                    if (_wil.Loaded)
                    {
                        WILS.Add(fn, _wil);
                        wil = _wil;
                    }
                }
            }
            if (wil == null || !wil.Loaded || (wil.ImageCount - 1) < index) return null;
            return wil.ImageInfos[index];
        }
    }
}