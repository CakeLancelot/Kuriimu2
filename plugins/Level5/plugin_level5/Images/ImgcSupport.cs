﻿using System.Collections.Generic;
using System.Drawing;
using Kanvas.Encoding;
using Kanvas.Swizzle;
using Komponent.IO.Attributes;
using Kontract.Kanvas;

namespace plugin_level5.Images
{
    class ImgcHeader
    {
        [FixedLength(4)]
        public string magic; // IMGC
        public int const1; // 30 30 00 00
        public short const2; // 30 00
        public byte imageFormat;
        public byte const3; // 01
        public byte combineFormat;
        public byte bitDepth;
        public short bytesPerTile;
        public short width;
        public short height;
        public int const4; // 30 00 00 00
        public int const5; // 30 00 01 00
        public int tableDataOffset; // always 0x48
        public int const6; // 03 00 00 00
        public int const7; // 00 00 00 00
        public int const8; // 00 00 00 00
        public int const9; // 00 00 00 00
        public int const10; // 00 00 00 00
        public int tileTableSize;
        public int tileTableSizePadded;
        public int imgDataSize;
        public int const11; // 00 00 00 00
        public int const12; // 00 00 00 00
    }

    class ImgcSwizzle : IImageSwizzle
    {
        private readonly MasterSwizzle _zOrder;

        public int Width { get; }
        public int Height { get; }

        public ImgcSwizzle(int width, int height)
        {
            Width = (width + 0x7) & ~0x7;
            Height = (height + 0x7) & ~0x7;

            _zOrder = new MasterSwizzle(Width, new Point(0, 0), new[] { (0, 1), (1, 0), (0, 2), (2, 0), (0, 4), (4, 0) });
        }

        public Point Transform(Point point)
        {
            return _zOrder.Get(point.Y * Width + point.X);
        }
    }

    class ImgcSupport
    {
        public static IDictionary<int, IColorEncoding> ImgcFormats = new Dictionary<int, IColorEncoding>
        {
            [0] = new Rgba(8, 8, 8, 8),
            [1] = new Rgba(4, 4, 4, 4),
            [2] = new Rgba(5, 5, 5, 1),
            [3] = new Rgba(8, 8, 8,"BGR"),
            [4] = new Rgba(5, 6, 5),
            [11] = new La(8, 8),
            [12] = new La(4, 4),
            [13] = new La(8, 0),
            [14] = new Rgba(8, 8, 0),
            [15] = new La(0, 8),
            [26] = new La(4, 0),
            //[27] = new LA(0, 4),
            [27] = new Etc1(false, true),
            [28] = new Etc1(false, true),
            [29] = new Etc1(true, true)
        };
    }
}
