using System;
using System.Text;
using System.IO;

namespace PscdPack
{
    /// <summary>
    /// A representation of Sega Genesis Classics' ROM pack format.
    /// </summary>
    public class PscdFormat : IDisposable
    {
        static readonly string Magic = "PSCD";
        static readonly uint Signature = 0x12538971;
        static readonly uint KeyConversionFactor = 0x75a3bd72;
        public static readonly byte[] ClassicsKeyBytes = { 0x6B, 0xC3, 0xC7, 0x6B, 0x83, 0xBB, 0x83, 0xC7, 0xC5, 0xBB, 0xD1, 0xAB, 0x2B };
        public static readonly byte[] ClassicsGoldKeyBytes = { 0x6B, 0x83, 0xBB, 0x95, 0xAB, 0xAF, 0xAB, 0x81, 0xAB, 0x6B, 0xC3, 0xC7, 0x2B };

        readonly Stream pakStream;
        BinaryReader br;
        BinaryWriter bw;
        uint headerChecksum;
        bool loaded;
        byte[] keyBytes;
        string name;
        bool disposed;

        /// <summary>
        /// Gets the size of the ROM image.
        /// </summary>
        public uint ImageSize { get; private set; }
        /// <summary>
        /// Gets the size of the compressed ROM image.
        /// </summary>
        public uint ImageCompressedSize { get; private set; }
        /// <summary>
        /// Gets the checksum of the ROM image.
        /// </summary>
        public uint ImageChecksum { get; private set; }
        /// <summary>
        /// Gets the size of the embedded thumbnail.
        /// </summary>
        public uint ThumbnailSize { get; private set; } // 160x60 BMP
        /// <summary>
        /// Gets whether stored ROM image has words flipped.
        /// </summary>
        public bool WordFlipped { get; private set; }
        /// <summary>
        /// Gets or sets the reserved value in the pack header.
        /// </summary>
        public byte Reserved1 { get; set; }
        /// <summary>
        /// Gets or sets the reserved value in the pack header.
        /// </summary>
        public byte Reserved2 { get; set; }
        /// <summary>
        /// Gets or sets the reserved value in the pack header.
        /// </summary>
        public byte Reserved3 { get; set; }
        /// <summary>
        /// Gets or sets the reserved value in the pack header.
        /// </summary>
        public uint Reserved4 { get; set; }
        /// <summary>
        /// Gets or sets the page mask for where on-cartridge memory is mapped.
        /// 
        /// Value is the memory address right-shifted by 12 bits.
        /// </summary>
        public ushort ExtraSavePage { get; set; } // SRAM page or EEPROM /SDA line page, address up to 16383 4KiB pages;
        /// <summary>
        /// Gets or sets the size of on-cartridge memory.
        /// 
        /// It is the number of 256-byte pages the memory provides.
        /// </summary>
        public ushort ExtraSaveSizePageCount { get; set; }
        /// <summary>
        /// Gets or sets the mode for on-cartridge memory.
        /// </summary>
        public ExtraSaveMode ExtraSaveMode { get; set; }
        /// <summary>
        /// Gets or sets the game's region.
        /// </summary>
        public RomRegion Region { get; set; }
        /// <summary>
        /// Gets or sets the game name for display on the emulator.
        /// </summary>
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                if (value.Length > 0x30) throw new ArgumentException("ROM name is longer than 48 characters.");
                name = value;
            }
        }

        /// <summary>
        /// Gets if pack file is non-empty.
        /// </summary>
        public bool FileHasContent
        {
            get
            {
                return pakStream.Length > 0;
            }
        }

        /// <summary>
        /// Instantiates a new instance of a pack file.
        /// </summary>
        /// <param name="path">The path to the pack file.</param>
        /// <param name="keyBytes">The key bytes for use in encryption.</param>
        public PscdFormat(string path, byte[] keyBytes)
        {
            if (keyBytes == null || keyBytes.Length == 0) throw new ArgumentNullException("keyBytes");
            if (string.IsNullOrEmpty(path)) throw new ArgumentNullException("path");
            this.keyBytes = (byte[])keyBytes.Clone();
            Reserved3 = 1;
            Name = string.Empty;
            Region = RomRegion.Worldwide;
            pakStream = File.Open(path, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read);
            br = new BinaryReader(pakStream);
            bw = new BinaryWriter(pakStream);
        }

        /// <summary>
        /// Reads the pack file from disk.
        /// </summary>
        public void Load()
        {
            ensureUndisposed();
            loaded = false;
            pakStream.Seek(0, SeekOrigin.Begin);
            string m = new string(br.ReadChars(Magic.Length));
            uint sig = br.ReadUInt32();
            if (m != Magic || sig != Signature) throw new InvalidDataException("Invalid file magic/signature.");
            ImageSize = br.ReadUInt32();
            ImageCompressedSize = br.ReadUInt32();
            ImageChecksum = br.ReadUInt32();
            ThumbnailSize = br.ReadUInt32();
            WordFlipped = br.ReadBoolean();
            Reserved1 = br.ReadByte();
            Reserved2 = br.ReadByte();
            Reserved3 = br.ReadByte();
            Reserved4 = br.ReadUInt32();
            ExtraSavePage = br.ReadUInt16();
            ExtraSaveSizePageCount = br.ReadUInt16();
            ExtraSaveMode = (ExtraSaveMode)br.ReadInt32();
            Region = (RomRegion)br.ReadInt32();

            byte[] blob = br.ReadBytes(0x30);
            CryptBuffer(blob, ImageChecksum ^ KeyConversionFactor, keyBytes);
            // Treat name as ASCII C string
            var sb = new StringBuilder();
            for (int i = 0; i < blob.Length; ++i)
            {
                byte b = blob[i];
                if (b == 0) break;
                sb.Append((char)b);
            }
            name = sb.ToString();
            headerChecksum = br.ReadUInt32();

            // Rewind and calculate checksum
            pakStream.Seek(0, SeekOrigin.Begin);
            byte[] headerBytes = br.ReadBytes(0x2c);
            Array.Resize<byte>(ref headerBytes, headerBytes.Length + blob.Length);
            Buffer.BlockCopy(blob, 0, headerBytes, 0x2c, blob.Length);
            if (CalcChecksum(headerBytes) != headerChecksum) throw new InvalidDataException("Header checksum failed.");
            if (pakStream.Length != 0x60 + ImageCompressedSize + ThumbnailSize) throw new InvalidDataException("Pack length check failed.");
            loaded = true;
        }

        /// <summary>
        /// Gets the stored ROM image, unflipped.
        /// </summary>
        /// <returns>A stream with the ROM image.</returns>
        public MemoryStream GetImage()
        {
            ensureLoaded();
            pakStream.Seek(0x60, SeekOrigin.Begin);
            byte[] rom = br.ReadBytes((int)ImageCompressedSize);
            CryptBuffer(rom, ImageChecksum ^ KeyConversionFactor, keyBytes);
            var decompRom = new byte[ImageSize];
            Lzss.Decode(rom, decompRom);
            if (CalcChecksum(decompRom, true) != ImageChecksum) throw new InvalidDataException("Image checksum failed.");
            if (WordFlipped) FlipWords(decompRom);
            return new MemoryStream(decompRom);
        }

        /// <summary>
        /// Gets the embedded thumbnail.
        /// </summary>
        /// <returns>A stream with the embedded thumbnail.</returns>
        public MemoryStream GetThumbnail()
        {
            ensureLoaded();
            pakStream.Seek(0x60 + ImageCompressedSize, SeekOrigin.Begin);
            byte[] thumb = br.ReadBytes((int)ThumbnailSize);
            CryptBuffer(thumb, ImageChecksum ^ KeyConversionFactor, keyBytes);
            return new MemoryStream(thumb);
        }

        /// <summary>
        /// Writes a ROM to the pack.
        /// </summary>
        /// <param name="rom">The rom file to write.</param>
        /// <param name="flip">Whether to flip words. <c>true</c> by default.</param>
        public void SetImage(byte[] rom, bool flip = true)
        {
            ensureUndisposed();
            if (rom == null || rom.Length == 0) throw new ArgumentNullException("rom");
            var myRom = (byte[])rom.Clone();
            if (flip) FlipWords(myRom);
            WordFlipped = true;
            ImageSize = (uint)myRom.Length;
            ImageChecksum = CalcChecksum(myRom, true);
            var compRom = new byte[(myRom.Length + 7) / 8 + myRom.Length]; // Account for worst-case scenario (completely random data)
            ImageCompressedSize = (uint)Lzss.Encode(myRom, compRom);
            Array.Resize<byte>(ref compRom, (int)ImageCompressedSize);
            CryptBuffer(compRom, ImageChecksum ^ KeyConversionFactor, keyBytes);
            pakStream.Seek(0x60, SeekOrigin.Begin);
            bw.Write(compRom);
            loaded = true;
        }

        /// <summary>
        /// Writes the embedded thumbnail to the pack.
        /// </summary>
        /// <param name="thumb">The thumbnail to embed.</param>
        public void SetThumbnail(byte[] thumb)
        {
            ensureLoaded();
            if (thumb == null)
            {
                ThumbnailSize = 0;
                return;
            }
            var myThumb = (byte[])thumb.Clone();
            CryptBuffer(myThumb, ImageChecksum ^ KeyConversionFactor, keyBytes);
            pakStream.Seek(0x60 + ImageCompressedSize, SeekOrigin.Begin);
            bw.Write(myThumb);
            ThumbnailSize = (uint)myThumb.Length;
        }

        /// <summary>
        /// Writes the pack header to disk.
        /// </summary>
        public void Save()
        {
            ensureLoaded();
            pakStream.Seek(0, SeekOrigin.Begin);
            bw.Write(Magic.ToCharArray());
            bw.Write(Signature);
            bw.Write(ImageSize);
            bw.Write(ImageCompressedSize);
            bw.Write(ImageChecksum);
            bw.Write(ThumbnailSize);
            bw.Write(WordFlipped);
            bw.Write(Reserved1);
            bw.Write(Reserved2);
            bw.Write(Reserved3);
            bw.Write(Reserved4);
            bw.Write(ExtraSavePage);
            bw.Write(ExtraSaveSizePageCount);
            bw.Write((int)ExtraSaveMode);
            bw.Write((int)Region);

            byte[] blob = Encoding.ASCII.GetBytes(name);
            Array.Resize<byte>(ref blob, 0x30);

            // Rewind and calculate checksum
            pakStream.Seek(0, SeekOrigin.Begin);
            byte[] headerBytes = br.ReadBytes(0x2c);
            Array.Resize<byte>(ref headerBytes, headerBytes.Length + blob.Length);
            Buffer.BlockCopy(blob, 0, headerBytes, 0x2c, blob.Length);
            headerChecksum = CalcChecksum(headerBytes);

            CryptBuffer(blob, ImageChecksum ^ KeyConversionFactor, keyBytes);
            bw.Write(blob);
            bw.Write(headerChecksum);
            bw.Flush();
            pakStream.SetLength(0x60 + ImageCompressedSize + ThumbnailSize);
        }

        void ensureLoaded()
        {
            ensureUndisposed();
            if (!loaded) throw new InvalidOperationException("Pack not loaded.");
        }

        void ensureUndisposed()
        {
            if (disposed) throw new ObjectDisposedException(GetType().FullName);
        }

        /// <summary>
        /// Closes the pack file.
        /// </summary>
        public void Dispose()
        {
            disposed = true;
            pakStream.Close();
        }

        #region Crypto and checksumming

        /// <summary>
        /// Calculate the checksum of the buffer.
        /// </summary>
        /// <param name="data">The buffer to calculate the checksum over.</param>
        /// <param name="flip">Whether to flip index when reading data.</param>
        /// <returns>The checksum calculated for the buffer.</returns>
        public static uint CalcChecksum(byte[] data, bool flip = false)
        {
            return CalcChecksum(data, 0, data.Length, flip);
        }

        /// <summary>
        /// Calculate the checksum of the buffer.
        /// </summary>
        /// <param name="data">The buffer to calculate the checksum over.</param>
        /// <param name="offset">The offset to begin processing at.</param>
        /// <param name="length">The length of the data to process.</param>
        /// <param name="flip">Whether to flip index when reading data.</param>
        /// <returns>The checksum calculated for the buffer.</returns>
        public static uint CalcChecksum(byte[] data, int offset, int length, bool flip = false)
        {
            uint sum = 0x72345617;
            int p = 0;
            for (int i = 0; ; ++i)
            {
                int j = i ^ (flip ? 1 : 0);
                if (j >= length) break;
                uint b = unchecked((uint)(sbyte)data[offset + j]); // Looks like someone was treating bytes as chars
                sum = unchecked((b << p) ^ (b + sum));
                ++p;
                if (p < 0 && p >= -5) p += 5; // Don't ask me why, this was in the original code
            }
            return (uint)sum;
        }

        /// <summary>
        /// Encrypt/decrypt the buffer.
        /// </summary>
        /// <param name="data">The buffer to process.</param>
        /// <param name="keyNum">The integer key used in crypting.</param>
        /// <param name="keyBytes">The array key used in crypting.</param>
        public static void CryptBuffer(byte[] data, uint keyNum, byte[] keyBytes)
        {
            CryptBuffer(data, 0, data.Length, keyNum, keyBytes);
        }

        /// <summary>
        /// Encrypt/decrypt the buffer.
        /// </summary>
        /// <param name="data">The buffer to process.</param>
        /// <param name="offset">The offset to begin crypting at.</param>
        /// <param name="length">The length of the data to crypt.</param>
        /// <param name="keyNum">The integer key used in crypting.</param>
        /// <param name="keyBytes">The array key used in crypting.</param>
        public static void CryptBuffer(byte[] data, int offset, int length, uint keyNum, byte[] keyBytes)
        {
            uint k = keyNum;
            int keyPos = 0;
            CryptBuffer(data, offset, length, ref k, keyBytes, ref keyPos);
        }

        /// <summary>
        /// Encrypt/decrypt the buffer.
        /// </summary>
        /// <param name="data">The buffer to process.</param>
        /// <param name="offset">The offset to begin crypting at.</param>
        /// <param name="length">The length of the data to crypt.</param>
        /// <param name="keyNum">The integer key used in crypting.</param>
        /// <param name="keyBytes">The array key used in crypting.</param>
        /// <param name="keyPos">The key position to use in crypting.</param>
        public static void CryptBuffer(byte[] data, int offset, int length, ref uint keyNum, byte[] keyBytes, ref int keyPos)
        {
            for (int i = 0; i < length; ++i)
            {
                keyNum = ((uint)(keyNum << 3) | (keyNum >> 29)); // rol keyNum,3
                byte kb = (byte)((keyBytes[keyPos] + 0x17) >> 1);
                byte kn1 = (byte)(keyNum & 7);
                byte kn2 = (byte)(keyNum & 1);
                byte xorByte = (byte)((((kn1 + kb) ^ (kn2 - kb)) & 0x0f) ^ (kn1 + kb));
                data[offset + i] ^= xorByte;
                keyPos += 3;
                keyPos %= keyBytes.Length;
            }
        }

        /// <summary>
        /// Exchange every two bytes with each other in the buffer.
        /// </summary>
        /// <param name="data">The buffer to process.</param>
        public static void FlipWords(byte[] data)
        {
            for (int i = 0; i < data.Length - 1; i += 2)
            {
                byte b = data[i];
                data[i] = data[i + 1];
                data[i + 1] = b;
            }
        }

        #endregion
    }
}
