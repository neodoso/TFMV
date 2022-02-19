using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.Compression;

namespace TFMV.SourceEngine
{
    // used to read/write alpha mask data to VTF DXT5 texture files (for tf2 bodygroups transparency masking)
    // VTF alpha mask data can be saved to a binary file, takes a lot less space without the RGB nor the thumbnail data
    // used to store and load alpha masks to rewrite into cached player textures for bodygroup masking
    class VTFedit
    {

        // 2048x1024 (2:1) DXT5 (no mip maps, no thumbnail) alpha start at offset 80 (decimal) // then jump 16 bytes for each RGBA
        // 1024x512  (2:1) DXT5 (no mip maps, no thumbnail) alpha start at offset 80 (decimal) // then jump 16 bytes for each RGBA

        // extracts the alpha mask bytes from a VTF binary file, saves them into an array and writes to file
        public void alpha_to_array(string vtf_filepath, string out_filepath)
        {
            Stack<byte> alpha_bytes = new Stack<byte>();
            byte[] vtf_bytes = System.IO.File.ReadAllBytes(vtf_filepath);

            // starting position
            Int32 pos = 88;

            // read fil array to end
            while (pos < vtf_bytes.Length)
            {

                // get alpha (8 bits)
                for (int i = 0; i < 8; i++)
                {
                    alpha_bytes.Push(vtf_bytes[pos + i]);
                }

                pos += 16;
            }
            // compress alpha byte array
            byte[] compressed_alpha = compress_byte_array(alpha_bytes.Reverse().ToArray());
            // write to file
            File.WriteAllBytes(out_filepath, compressed_alpha);
        }

        // loads VTF (DXT5) and binary file containing the alpha mask bytes
        // rewrites the VTF's alpha channel and saves file
        public void write_alpha(string vtf_filepath, object alpha_mask)
        {

            if (!File.Exists(vtf_filepath)) { return; }

            byte[] alpha_bytes_compressed = new byte[0];
            byte[] alpha_bytes = new byte[0];

            // alpha_mask can either be a file path to a binary file
            // or a byte[] array where we already loaded the mask
            if (alpha_mask.GetType() == typeof(String))
            {
                alpha_bytes_compressed = System.IO.File.ReadAllBytes((String)alpha_mask);
                alpha_bytes = decompress_byte_array(alpha_bytes_compressed);
            }
            else if (alpha_mask.GetType() == typeof(byte[]))
            {
                alpha_bytes_compressed = (byte[])alpha_mask;
                alpha_bytes = alpha_bytes_compressed;
            }

            if (alpha_bytes_compressed.Length == 0) { return; }

            byte[] vtf_bytes = System.IO.File.ReadAllBytes(vtf_filepath);


            // starting position
            Int32 pos = 0;

            // read fil array to end
            while (pos < alpha_bytes.Length - 1)
            {
                //get alpha (8 bits)
                for (int i = 0; i < 8; i++)
                {
                    //Int32 vtf_pos = (pos * 2) + 160 + i; // if no mimaps 
                    Int32 vtf_pos = (pos * 2) + 88 + i; // if it has a thumbnail start at 96
                    vtf_bytes[vtf_pos] = alpha_bytes[pos + i];
                }

                pos += 8;
            }

            string out_path = Path.GetDirectoryName(vtf_filepath) + "\\" + Path.GetFileNameWithoutExtension(vtf_filepath) + ".vtf";
            File.WriteAllBytes(out_path, vtf_bytes);

        }



        public byte[] compress_byte_array(byte[] raw_data)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                using (GZipStream gzip = new GZipStream(memory, CompressionMode.Compress, true))
                {
                    gzip.Write(raw_data, 0, raw_data.Length);
                }

                return memory.ToArray();
            }
        }


        public byte[] decompress_byte_array(byte[] compressed_data)
        {
            using (GZipStream stream = new GZipStream(new MemoryStream(compressed_data), CompressionMode.Decompress))
            {
                const int size = 4096;
                byte[] buffer = new byte[size];
                using (MemoryStream memory = new MemoryStream())
                {
                    int count = 0;
                    do
                    {
                        count = stream.Read(buffer, 0, size);
                        if (count > 0)
                        {
                            memory.Write(buffer, 0, count);
                        }
                    }
                    while (count > 0);
                    return memory.ToArray();
                }
            }
        }


        // adds brighter pixels over darker pixels
        // useful to combine alpha masks
        public byte[] add_alpha_masks(byte[] mask, byte[] mask1)
        {
            if (mask.Length != mask1.Length) { return mask; }

            byte[] result = new byte[mask1.Length];

            // for each byte sum up the pixels values and clamp the value so it doesn't go over the max brightness value (255)
            for (int i = 0; i < mask.Length; i++)
            {
                int pixel = mask[i] + mask1[i];
                if (pixel > 255) { pixel = 255; }

                result[i] = Convert.ToByte(pixel);
            }

            return result;
        }


        // substracts brighter pixels making white pixels darker
        // useful to combine alpha masks
        // white alpha is made black on the new mask
        public byte[] multiply_alpha_masks(byte[] mask_a, byte[] mask_b)
        {
            if (mask_a.Length != mask_b.Length) { return mask_a; }

            byte[] result = new byte[mask_a.Length];


            for (int i = 0; i < mask_a.Length; i++)
            {
                int pixel = mask_a[i] - mask_b[i];
                if (pixel < 0) { pixel = 0; }
                result[i] = Convert.ToByte(pixel);
            }

            return result;
        }


        public byte[] invert_alpha_mask(byte[] mask)
        {
            byte[] result = new byte[mask.Length];

            for (int i = 0; i < mask.Length; i++)
            {
                result[i] = (byte) (Math.Abs(255 - mask[i]));
            }

            return result;
        }

    }
}
