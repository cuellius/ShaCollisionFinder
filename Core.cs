﻿using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ShaCollisionFinder
{
    public class Core
    {
        private static readonly byte[] Prefix1 = { 37, 80, 68, 70, 45, 49, 46, 51, 10, 37, 226, 227, 207, 211, 10, 10, 10, 49, 32, 48, 32, 111, 98, 106, 10, 60, 60, 47, 87, 105, 100, 116, 104, 32, 50, 32, 48, 32, 82, 47, 72, 101, 105, 103, 104, 116, 32, 51, 32, 48, 32, 82, 47, 84, 121, 112, 101, 32, 52, 32, 48, 32, 82, 47, 83, 117, 98, 116, 121, 112, 101, 32, 53, 32, 48, 32, 82, 47, 70, 105, 108, 116, 101, 114, 32, 54, 32, 48, 32, 82, 47, 67, 111, 108, 111, 114, 83, 112, 97, 99, 101, 32, 55, 32, 48, 32, 82, 47, 76, 101, 110, 103, 116, 104, 32, 56, 32, 48, 32, 82, 47, 66, 105, 116, 115, 80, 101, 114, 67, 111, 109, 112, 111, 110, 101, 110, 116, 32, 56, 62, 62, 10, 115, 116, 114, 101, 97, 109, 10, 255, 216, 255, 254, 0, 36, 83, 72, 65, 45, 49, 32, 105, 115, 32, 100, 101, 97, 100, 33, 33, 33, 33, 33, 133, 47, 236, 9, 35, 57, 117, 156, 57, 177, 161, 198, 60, 76, 151, 225, 255, 254, 1, 115, 70, 220, 145, 102, 182, 126, 17, 143, 2, 154, 182, 33, 178, 86, 15, 249, 202, 103, 204, 168, 199, 248, 91, 168, 76, 121, 3, 12, 43, 61, 226, 24, 248, 109, 179, 169, 9, 1, 213, 223, 69, 193, 79, 38, 254, 223, 179, 220, 56, 233, 106, 194, 47, 231, 189, 114, 143, 14, 69, 188, 224, 70, 210, 60, 87, 15, 235, 20, 19, 152, 187, 85, 46, 245, 160, 168, 43, 227, 49, 254, 164, 128, 55, 184, 181, 215, 31, 14, 51, 46, 223, 147, 172, 53, 0, 235, 77, 220, 13, 236, 193, 168, 100, 121, 12, 120, 44, 118, 33, 86, 96, 221, 48, 151, 145, 208, 107, 208, 175, 63, 152, 205, 164, 188, 70, 41, 177 };
        private static readonly byte[] Prefix2 = { 37, 80, 68, 70, 45, 49, 46, 51, 10, 37, 226, 227, 207, 211, 10, 10, 10, 49, 32, 48, 32, 111, 98, 106, 10, 60, 60, 47, 87, 105, 100, 116, 104, 32, 50, 32, 48, 32, 82, 47, 72, 101, 105, 103, 104, 116, 32, 51, 32, 48, 32, 82, 47, 84, 121, 112, 101, 32, 52, 32, 48, 32, 82, 47, 83, 117, 98, 116, 121, 112, 101, 32, 53, 32, 48, 32, 82, 47, 70, 105, 108, 116, 101, 114, 32, 54, 32, 48, 32, 82, 47, 67, 111, 108, 111, 114, 83, 112, 97, 99, 101, 32, 55, 32, 48, 32, 82, 47, 76, 101, 110, 103, 116, 104, 32, 56, 32, 48, 32, 82, 47, 66, 105, 116, 115, 80, 101, 114, 67, 111, 109, 112, 111, 110, 101, 110, 116, 32, 56, 62, 62, 10, 115, 116, 114, 101, 97, 109, 10, 255, 216, 255, 254, 0, 36, 83, 72, 65, 45, 49, 32, 105, 115, 32, 100, 101, 97, 100, 33, 33, 33, 33, 33, 133, 47, 236, 9, 35, 57, 117, 156, 57, 177, 161, 198, 60, 76, 151, 225, 255, 254, 1, 127, 70, 220, 147, 166, 182, 126, 1, 59, 2, 154, 170, 29, 178, 86, 11, 69, 202, 103, 214, 136, 199, 248, 75, 140, 76, 121, 31, 224, 43, 61, 246, 20, 248, 109, 177, 105, 9, 1, 197, 107, 69, 193, 83, 10, 254, 223, 183, 96, 56, 233, 114, 114, 47, 231, 173, 114, 143, 14, 73, 4, 224, 70, 194, 48, 87, 15, 233, 212, 19, 152, 171, 225, 46, 245, 188, 148, 43, 227, 53, 66, 164, 128, 45, 152, 181, 215, 15, 42, 51, 46, 195, 127, 172, 53, 20, 231, 77, 220, 15, 44, 193, 168, 116, 205, 12, 120, 48, 90, 33, 86, 100, 97, 48, 151, 137, 96, 107, 208, 191, 63, 152, 205, 168, 4, 70, 41, 161 };
        private static readonly byte[] Postfix = { 101, 110, 100, 115, 116, 114, 101, 97, 109, 10, 101, 110, 100, 111, 98, 106, 10, 10, 50, 32, 48, 32, 111, 98, 106, 10, 48, 48, 48, 48, 48, 48, 49, 51, 54, 48, 10, 101, 110, 100, 111, 98, 106, 10, 10, 51, 32, 48, 32, 111, 98, 106, 10, 48, 48, 48, 48, 48, 48, 48, 53, 54, 56, 10, 101, 110, 100, 111, 98, 106, 10, 10, 52, 32, 48, 32, 111, 98, 106, 10, 47, 88, 79, 98, 106, 101, 99, 116, 10, 101, 110, 100, 111, 98, 106, 10, 10, 53, 32, 48, 32, 111, 98, 106, 10, 47, 73, 109, 97, 103, 101, 10, 101, 110, 100, 111, 98, 106, 10, 10, 54, 32, 48, 32, 111, 98, 106, 10, 47, 68, 67, 84, 68, 101, 99, 111, 100, 101, 10, 101, 110, 100, 111, 98, 106, 10, 10, 55, 32, 48, 32, 111, 98, 106, 10, 47, 68, 101, 118, 105, 99, 101, 82, 71, 66, 10, 101, 110, 100, 111, 98, 106, 10, 10, 56, 32, 48, 32, 111, 98, 106, 10, 48, 48, 48, 48, 48, 57, 48, 55, 55, 48, 10, 101, 110, 100, 111, 98, 106, 10, 10, 57, 32, 48, 32, 111, 98, 106, 10, 60, 60, 10, 32, 32, 47, 84, 121, 112, 101, 32, 47, 67, 97, 116, 97, 108, 111, 103, 10, 32, 32, 47, 80, 97, 103, 101, 115, 32, 49, 48, 32, 48, 32, 82, 10, 62, 62, 10, 101, 110, 100, 111, 98, 106, 10, 10, 49, 48, 32, 48, 32, 111, 98, 106, 10, 60, 60, 10, 32, 32, 47, 84, 121, 112, 101, 32, 47, 80, 97, 103, 101, 115, 10, 32, 32, 47, 67, 111, 117, 110, 116, 32, 49, 10, 32, 32, 47, 75, 105, 100, 115, 32, 91, 49, 49, 32, 48, 32, 82, 93, 10, 62, 62, 10, 101, 110, 100, 111, 98, 106, 10, 10, 49, 49, 32, 48, 32, 111, 98, 106, 10, 60, 60, 10, 32, 32, 47, 84, 121, 112, 101, 32, 47, 80, 97, 103, 101, 10, 32, 32, 47, 80, 97, 114, 101, 110, 116, 32, 49, 48, 32, 48, 32, 82, 10, 32, 32, 47, 77, 101, 100, 105, 97, 66, 111, 120, 32, 91, 48, 32, 48, 32, 48, 48, 48, 48, 48, 48, 49, 51, 54, 48, 32, 48, 48, 48, 48, 48, 48, 48, 53, 54, 56, 93, 10, 32, 32, 47, 67, 114, 111, 112, 66, 111, 120, 32, 91, 48, 32, 48, 32, 48, 48, 48, 48, 48, 48, 49, 51, 54, 48, 32, 48, 48, 48, 48, 48, 48, 48, 53, 54, 56, 93, 10, 32, 32, 47, 67, 111, 110, 116, 101, 110, 116, 115, 32, 49, 50, 32, 48, 32, 82, 10, 32, 32, 47, 82, 101, 115, 111, 117, 114, 99, 101, 115, 10, 32, 32, 60, 60, 10, 32, 32, 32, 32, 47, 88, 79, 98, 106, 101, 99, 116, 32, 60, 60, 47, 73, 109, 48, 32, 49, 32, 48, 32, 82, 62, 62, 10, 32, 32, 62, 62, 10, 62, 62, 10, 101, 110, 100, 111, 98, 106, 10, 10, 49, 50, 32, 48, 32, 111, 98, 106, 10, 60, 60, 47, 76, 101, 110, 103, 116, 104, 32, 52, 57, 62, 62, 10, 115, 116, 114, 101, 97, 109, 10, 113, 10, 32, 32, 48, 48, 48, 48, 48, 48, 49, 51, 54, 48, 32, 48, 32, 48, 32, 48, 48, 48, 48, 48, 48, 48, 53, 54, 56, 32, 48, 32, 48, 32, 99, 109, 10, 32, 32, 47, 73, 109, 48, 32, 68, 111, 10, 81, 10, 101, 110, 100, 115, 116, 114, 101, 97, 109, 10, 101, 110, 100, 111, 98, 106, 10, 10, 120, 114, 101, 102, 10, 48, 32, 49, 51, 32, 10, 48, 48, 48, 48, 48, 48, 48, 48, 48, 48, 32, 54, 53, 53, 51, 53, 32, 102, 32, 10, 48, 48, 48, 48, 48, 48, 48, 48, 49, 55, 32, 48, 48, 48, 48, 48, 32, 110, 32, 10, 48, 48, 48, 48, 48, 57, 49, 51, 53, 48, 32, 48, 48, 48, 48, 48, 32, 110, 32, 10, 48, 48, 48, 48, 48, 57, 49, 51, 55, 55, 32, 48, 48, 48, 48, 48, 32, 110, 32, 10, 48, 48, 48, 48, 48, 57, 49, 52, 48, 52, 32, 48, 48, 48, 48, 48, 32, 110, 32, 10, 48, 48, 48, 48, 48, 57, 49, 52, 50, 57, 32, 48, 48, 48, 48, 48, 32, 110, 32, 10, 48, 48, 48, 48, 48, 57, 49, 52, 53, 50, 32, 48, 48, 48, 48, 48, 32, 110, 32, 10, 48, 48, 48, 48, 48, 57, 49, 52, 55, 57, 32, 48, 48, 48, 48, 48, 32, 110, 32, 10, 48, 48, 48, 48, 48, 57, 49, 53, 48, 54, 32, 48, 48, 48, 48, 48, 32, 110, 32, 10, 48, 48, 48, 48, 48, 57, 49, 53, 51, 51, 32, 48, 48, 48, 48, 48, 32, 110, 32, 10, 48, 48, 48, 48, 48, 57, 49, 53, 56, 56, 32, 48, 48, 48, 48, 48, 32, 110, 32, 10, 48, 48, 48, 48, 48, 57, 49, 54, 53, 52, 32, 48, 48, 48, 48, 48, 32, 110, 32, 10, 48, 48, 48, 48, 48, 57, 49, 56, 53, 55, 32, 48, 48, 48, 48, 48, 32, 110, 32, 10, 10, 116, 114, 97, 105, 108, 101, 114, 32, 60, 60, 32, 47, 82, 111, 111, 116, 32, 57, 32, 48, 32, 82, 32, 47, 83, 105, 122, 101, 32, 49, 51, 62, 62, 10, 10, 115, 116, 97, 114, 116, 120, 114, 101, 102, 10, 48, 48, 48, 48, 48, 57, 49, 57, 53, 53, 10, 37, 37, 69, 79, 70, 10 };

        public static void GenerateFiles(string j1, string j2, string pdf1, string pdf2)
        {
            var u = JpegUtils.ReadImageAsJpegAndCompress(j1);
            var v = JpegUtils.ReadImageAsJpegAndCompress(j2);

            var n = u.Length + 8;

            var comment = new List<byte>();
            for (int i = 0; i < 16 * 15; i++) comment.Add(0);
            comment.Add(0);
            comment.Add(0);
            comment.Add(0xff);
            comment.Add(0xfe);
            comment.Add((byte)(n >> 8));
            comment.Add((byte)(n & 0xff));
            comment.Add(0);
            comment.Add(0);
            comment.Add(0);
            comment.Add(0);
            comment.Add(0);
            comment.Add(0);
            comment.Add(0);
            comment.Add(0);

            var j = JpegUtils.MergeJpegs(u, v);

            File.WriteAllBytes(pdf1, Prefix1.Concat(comment).Concat(j).Concat(Postfix).ToArray());
            File.WriteAllBytes(pdf2, Prefix2.Concat(comment).Concat(j).Concat(Postfix).ToArray());
        }
    }
}