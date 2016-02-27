using System;
using System.Collections.Generic;

namespace PscdPack
{
	/// <summary>
    /// LZSS encoder/decoder based on Haruhiko Okumura's 1994 implementation.
	/// </summary>
	public static class Lzss
	{
		const int N = 4096;
		const int NIL = N;
		const int F = 18;
		const int THRESHOLD = 2;
		
        /// <summary>
        /// Decode LZSS compressed data
        /// </summary>
        /// <param name="inData">The buffer containing compressed data.</param>
        /// <param name="outData">The buffer to write decompressed data to.</param>
		public static void Decode(byte[] inData, byte[] outData)
		{
			var dict = new byte[N + F - 1];
			for (int i = 0; i < N - F; ++i) dict[i] = 0x20;
			int oPos = 0;
			int iPos = 0;
			int dPos = N - F;
			for (int flags = 0; ; flags >>= 1)
			{
				if ((flags & 0x100) == 0) // All flag bits read
				{
					if (iPos >= inData.Length) break;
					flags = inData[iPos++] | 0xff00; // Read next flag byte
				}
				
				if ((flags & 1) == 1) // Literal
				{
					if (iPos >= inData.Length) break;
					dict[dPos] = inData[iPos++];
					if (oPos >= outData.Length) break;
					outData[oPos++] = dict[dPos];
					dPos = (dPos + 1) % N;
				}
				else // Dictionary lookup
				{
					if (iPos >= inData.Length) break;
					int p = inData[iPos++]; // Dictionary position
					if (iPos >= inData.Length) break;
					byte c = inData[iPos++];
					p |= (c & 0xf0) << 4;
					c = (byte)((c & 0x0f) + THRESHOLD); // Copy length
					for (int j = 0; j <= c; ++j) // Copy bytes
					{
						byte b = dict[(p + j) % N];
						if (oPos >= outData.Length) break;
						outData[oPos++] = b;
						dict[dPos] = b;
						dPos = (dPos + 1) % N;
					}
				}
			}
		}
		
        /// <summary>
        /// Encodes data as if it is completely random.
        /// </summary>
        /// <param name="inData">The buffer containing data to encode.</param>
        /// <param name="outData">The buffer to write encoded data to.</param>
        /// <returns>The number of bytes written to the output buffer.</returns>
		public static int DumbEncode(byte[] inData, byte[] outData)
		{
			if (outData.Length < (inData.Length + 7) / 8 + inData.Length) throw new ArgumentException("Output buffer too small.", "outData");
			int oPos = 0;
			for (int i = 0; i < inData.Length; ++i)
			{
				if (i % 8 == 0) outData[oPos++] = 0xff;
				outData[oPos++] = inData[i];
			}
			return oPos;
		}

        /// <summary>
        /// Compresses data using LZSS.
        /// </summary>
        /// <param name="inData">The buffer containing data to encode.</param>
        /// <param name="outData">The buffer to write encoded data to.</param>
        /// <returns>The number of bytes written to the output buffer.</returns>
        public static int Encode(byte[] inData, byte[] outData)
        {
            if (inData.Length == 0) return 0;
            int iPos = 0;
            int oPos = 0;

            var codeBuffer = new byte[17]; // Flags byte plus 8 pos/length pairs
            int cPos = 1;
            int mask = 1;

            var dict = new byte[N + F - 1];
            int dPos = N - F;
            int s = 0;
            int len;
            for (int i = 0; i < N - F; ++i) dict[i] = 0x20; // Init dictionary
            for (len = 0; len < F && len < inData.Length; ++len, ++iPos) dict[dPos + len] = inData[iPos]; // Read some literals

            LzTree tree = new LzTree(dict, F);
            for (int i = 1; i <= F; ++i) tree.InsertNode(dPos - i); // Insert into tree in reverse
            tree.InsertNode(dPos);

            do
            {
                if (tree.MatchLength > len) tree.MatchLength = len; // Clamp match length
                if (tree.MatchLength <= THRESHOLD)
                {
                    // Match not long enough, write literal
                    tree.MatchLength = 1;
                    codeBuffer[0] |= (byte)mask;
                    codeBuffer[cPos++] = dict[dPos];
                }
                else
                {
                    // Write position/length pair
                    codeBuffer[cPos++] = (byte)(tree.MatchPos & 0xff);
                    codeBuffer[cPos++] = (byte)(((tree.MatchPos >> 4) & 0xf0) | (tree.MatchLength - (THRESHOLD + 1)));
                }

                mask <<= 1;
                if (mask > 0xff)
                {
                    // Flush code buffer
                    for (int i = 0; i < cPos; ++i) outData[oPos++] = codeBuffer[i];
                    codeBuffer[0] = 0;
                    cPos = 1;
                    mask = 1;
                }

                int lastMatchLength = tree.MatchLength;
                int j;
                for (j = 0; j < lastMatchLength && iPos < inData.Length; ++j, ++iPos)
                {
                    tree.DeleteNode(s);
                    dict[s] = inData[iPos];
                    if (s < F - 1) dict[s + N] = inData[iPos];
                    ++s;
                    s %= N;
                    ++dPos;
                    dPos %= N;
                    tree.InsertNode(dPos);
                }
                while (j++ < lastMatchLength)
                {
                    tree.DeleteNode(s);
                    ++s;
                    s %= N;
                    ++dPos;
                    dPos %= N;
                    --len;
                    if (len > 0) tree.InsertNode(dPos);
                }
            } while (len > 0);

            // Flush code buffer
            if (cPos > 1)
            {
                for (int i = 0; i < cPos; ++i) outData[oPos++] = codeBuffer[i];
            }

            return oPos;
        }
	}
}
