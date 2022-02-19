using System.Collections.Generic;
using System.Drawing;

namespace TFMV.TF2
{
    public class paints
    {
        public static List<paint> red = new List<paint>
        {
            new paint(Color.FromArgb(230, 230, 230), "An Extraordinary Abundance of Tinge"),
            new paint(Color.FromArgb(216, 190, 216), "Color No. 216-190-216"),
            new paint(Color.FromArgb(197, 175, 145), "Peculiarly Drab Tincture"),
            new paint(Color.FromArgb(126, 126, 126), "Aged Moustache Grey"),
            new paint(Color.FromArgb(20, 20, 20), "A Distinctive Lack of Hue"),
            new paint(Color.FromArgb(105, 77, 58), "Radigan Conagher Brown"),
            new paint(Color.FromArgb(124, 108, 87), "Ye Olde Rustic Color"),
            new paint(Color.FromArgb(165, 117, 69), "Muskelmannbraun"),
            new paint(Color.FromArgb(231, 181, 59), "Australium Gold"),
            new paint(Color.FromArgb(240, 230, 140), "The Color of a Gentlemann's Business Pants"),
            new paint(Color.FromArgb(233, 150, 122), "Dark Salmon Injustice"),
            new paint(Color.FromArgb(207, 115, 54), "Mann Co. Orange"),
            new paint(Color.FromArgb(255, 105, 180), "Pink as Hell"),
            new paint(Color.FromArgb(125, 64, 113), "A Deep Commitment to Purple"),
            new paint(Color.FromArgb(81, 56, 74), "Noble Hatter's Violet"),
            new paint(Color.FromArgb(47, 79, 79), "A Color Similar to Slate"),
            new paint(Color.FromArgb(50, 205, 50), "The Bitter Taste of Defeat and Lime"),
            new paint(Color.FromArgb(114, 158, 66), "Indubitably Green"),
            new paint(Color.FromArgb(128, 128, 0), "Drably Olive"),
            new paint(Color.FromArgb(66, 79, 59), "Zephaniah's Greed"),
            new paint(Color.FromArgb(188, 221, 179), "A Mann's Mint"),
            new paint(Color.FromArgb(45, 45, 36), "After Eight"),
            new paint(Color.FromArgb(168, 154, 140), "Waterlogged Lab Coat (RED)"),
            new paint(Color.FromArgb(59, 31, 35), "Balaclavas Are Forever (RED)"),
            new paint(Color.FromArgb(184, 56, 59), "Team Spirit (RED)"),
            new paint(Color.FromArgb(72, 56, 56), "Operator's Overalls (RED)"),
            new paint(Color.FromArgb(128, 48, 32), "The Value of Teamwork (RED)"),
            new paint(Color.FromArgb(101, 71, 64), "An Air of Debonair (RED)"),
            new paint(Color.FromArgb(195, 108, 45), "Cream Spirit (RED)")
        };


        public static List<paint> blu = new List<paint>
        {
            new paint(Color.FromArgb(230, 230, 230), "An Extraordinary Abundance of Tinge"),
            new paint(Color.FromArgb(216, 190, 216), "Color No. 216-190-216"),
            new paint(Color.FromArgb(197, 175, 145), "Peculiarly Drab Tincture"),
            new paint(Color.FromArgb(126, 126, 126), "Aged Moustache Grey"),
            new paint(Color.FromArgb(20, 20, 20), "A Distinctive Lack of Hue"),
            new paint(Color.FromArgb(105, 77, 58), "Radigan Conagher Brown"),
            new paint(Color.FromArgb(124, 108, 87), "Ye Olde Rustic Color"),
            new paint(Color.FromArgb(165, 117, 69), "Muskelmannbraun"),
            new paint(Color.FromArgb(231, 181, 59), "Australium Gold"),
            new paint(Color.FromArgb(240, 230, 140), "The Color of a Gentlemann's Business Pants"),
            new paint(Color.FromArgb(233, 150, 122), "Dark Salmon Injustice"),
            new paint(Color.FromArgb(207, 115, 54), "Mann Co. Orange"),
            new paint(Color.FromArgb(255, 105, 180), "Pink as Hell"),
            new paint(Color.FromArgb(125, 64, 113), "A Deep Commitment to Purple"),
            new paint(Color.FromArgb(81, 56, 74), "Noble Hatter's Violet"),
            new paint(Color.FromArgb(47, 79, 79), "A Color Similar to Slate"),
            new paint(Color.FromArgb(50, 205, 50), "The Bitter Taste of Defeat and Lime"),
            new paint(Color.FromArgb(114, 158, 66), "Indubitably Green"),
            new paint(Color.FromArgb(128, 128, 0), "Drably Olive"),
            new paint(Color.FromArgb(66, 79, 59), "Zephaniah's Greed"),
            new paint(Color.FromArgb(188, 221, 179), "A Mann's Mint"),
            new paint(Color.FromArgb(45, 45, 36), "After Eight"),
            new paint(Color.FromArgb(131, 159, 163), "Waterlogged Lab Coat (BLU)"),
            new paint(Color.FromArgb(24, 35, 61), "Balaclavas Are Forever (BLU)"),
            new paint(Color.FromArgb(88, 133, 162), "Team Spirit (BLU)"),
            new paint(Color.FromArgb(56, 66, 72), "Operator's Overalls (BLU)"),
            new paint(Color.FromArgb(37, 109, 141), "The Value of Teamwork (BLU)"),
            new paint(Color.FromArgb(40, 57, 77), "An Air of Debonair (BLU)"),
            new paint(Color.FromArgb(184, 128, 53), "Cream Spirit (BLU)")
        };


        public class paint
        {
            public Color color { get; set; }
            public string name { get; set; }

            public paint(Color _color, string _name)
            {
                this.color = _color;
                this.name = _name;
            }
        }
    }
}
