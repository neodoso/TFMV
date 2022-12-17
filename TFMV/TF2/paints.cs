using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

//neodement: reorganized paints to match wiki ordering. added field for hex so we can save the filename using that too.

namespace TFMV.TF2
{
    public class paints
    {
        public static List<paint> red = new List<paint> 
        {
            new paint(Color.FromArgb(230, 230, 230), "An Extraordinary Abundance of Tinge", "E6E6E6"),
            new paint(Color.FromArgb(216, 190, 216), "Color No. 216-190-216", "D8BED8"),
            new paint(Color.FromArgb(197, 175, 145), "Peculiarly Drab Tincture", "C5AF91"),
            new paint(Color.FromArgb(126, 126, 126), "Aged Moustache Grey", "7E7E7E"),
            new paint(Color.FromArgb(20, 20, 20), "A Distinctive Lack of Hue", "141414"),
            new paint(Color.FromArgb(45, 45, 36), "After Eight", "2D2D24"),
            new paint(Color.FromArgb(105, 77, 58), "Radigan Conagher Brown", "694D3A"),
            new paint(Color.FromArgb(124, 108, 87), "Ye Olde Rustic Color", "7C6C57"),
            new paint(Color.FromArgb(165, 117, 69), "Muskelmannbraun", "A57545"),
            new paint(Color.FromArgb(207, 115, 54), "Mann Co. Orange", "CF7336"),
            new paint(Color.FromArgb(231, 181, 59), "Australium Gold", "E7B53B"),
            new paint(Color.FromArgb(240, 230, 140), "The Color of a Gentlemann's Business Pants", "F0E68C"),
            new paint(Color.FromArgb(233, 150, 122), "Dark Salmon Injustice", "E9967A"),    
            new paint(Color.FromArgb(255, 105, 180), "Pink as Hell", "FF69B4"),
            new paint(Color.FromArgb(125, 64, 113), "A Deep Commitment to Purple", "7D4071"),
            new paint(Color.FromArgb(81, 56, 74), "Noble Hatter's Violet", "51384A"),
            new paint(Color.FromArgb(47, 79, 79), "A Color Similar to Slate", "2F4F4F"),
            new paint(Color.FromArgb(66, 79, 59), "Zephaniah's Greed", "424F3B"),
            new paint(Color.FromArgb(128, 128, 0), "Drably Olive", "808000"),
            new paint(Color.FromArgb(114, 158, 66), "Indubitably Green", "729E42"),
            new paint(Color.FromArgb(50, 205, 50), "The Bitter Taste of Defeat and Lime", "32CD32"),
            new paint(Color.FromArgb(188, 221, 179), "A Mann's Mint", "BCDDB3"),
            new paint(Color.FromArgb(168, 154, 140), "Waterlogged Lab Coat (RED)", "A89A8C"),
            new paint(Color.FromArgb(59, 31, 35), "Balaclavas Are Forever (RED)", "3B1F23"),
            new paint(Color.FromArgb(184, 56, 59), "Team Spirit (RED)", "B8383B"),
            new paint(Color.FromArgb(72, 56, 56), "Operator's Overalls (RED)", "483838"),
            new paint(Color.FromArgb(128, 48, 32), "The Value of Teamwork (RED)", "803020"),
            new paint(Color.FromArgb(101, 71, 64), "An Air of Debonair (RED)", "654740"),
            new paint(Color.FromArgb(195, 108, 45), "Cream Spirit (RED)", "C36C2D")
        };


        public static List<paint> blu = new List<paint> 
        {
            new paint(Color.FromArgb(230, 230, 230), "An Extraordinary Abundance of Tinge", "E6E6E6"),
            new paint(Color.FromArgb(216, 190, 216), "Color No. 216-190-216", "D8BED8"),
            new paint(Color.FromArgb(197, 175, 145), "Peculiarly Drab Tincture", "C5AF91"),
            new paint(Color.FromArgb(126, 126, 126), "Aged Moustache Grey", "7E7E7E"),
            new paint(Color.FromArgb(20, 20, 20), "A Distinctive Lack of Hue", "141414"),
            new paint(Color.FromArgb(45, 45, 36), "After Eight", "2D2D24"),
            new paint(Color.FromArgb(105, 77, 58), "Radigan Conagher Brown", "694D3A"),
            new paint(Color.FromArgb(124, 108, 87), "Ye Olde Rustic Color", "7C6C57"),
            new paint(Color.FromArgb(165, 117, 69), "Muskelmannbraun", "A57545"),
            new paint(Color.FromArgb(207, 115, 54), "Mann Co. Orange", "CF7336"),
            new paint(Color.FromArgb(231, 181, 59), "Australium Gold", "E7B53B"),
            new paint(Color.FromArgb(240, 230, 140), "The Color of a Gentlemann's Business Pants", "F0E68C"),
            new paint(Color.FromArgb(233, 150, 122), "Dark Salmon Injustice", "E9967A"),
            new paint(Color.FromArgb(255, 105, 180), "Pink as Hell", "FF69B4"),
            new paint(Color.FromArgb(125, 64, 113), "A Deep Commitment to Purple", "7D4071"),
            new paint(Color.FromArgb(81, 56, 74), "Noble Hatter's Violet", "51384A"),
            new paint(Color.FromArgb(47, 79, 79), "A Color Similar to Slate", "2F4F4F"),
            new paint(Color.FromArgb(66, 79, 59), "Zephaniah's Greed", "424F3B"),
            new paint(Color.FromArgb(128, 128, 0), "Drably Olive", "808000"),
            new paint(Color.FromArgb(114, 158, 66), "Indubitably Green", "729E42"),
            new paint(Color.FromArgb(50, 205, 50), "The Bitter Taste of Defeat and Lime", "32CD32"),
            new paint(Color.FromArgb(188, 221, 179), "A Mann's Mint", "BCDDB3"),
            new paint(Color.FromArgb(131, 159, 163), "Waterlogged Lab Coat (BLU)", "839FA3"),
            new paint(Color.FromArgb(24, 35, 61), "Balaclavas Are Forever (BLU)", "18233D"),
            new paint(Color.FromArgb(88, 133, 162), "Team Spirit (BLU)", "5885A2"),
            new paint(Color.FromArgb(56, 66, 72), "Operator's Overalls (BLU)", "384248"),
            new paint(Color.FromArgb(37, 109, 141), "The Value of Teamwork (BLU)", "256D8D"),
            new paint(Color.FromArgb(40, 57, 77), "An Air of Debonair (BLU)", "28394D"),
            new paint(Color.FromArgb(184, 128, 53), "Cream Spirit (BLU)", "B88035")
        };


        public class paint
        {
            public Color color { get; set; }
            public string name { get; set; }
            public string hex { get; set; }

            public paint(Color _color, string _name, string _hex)
            {
                this.color = _color;
                this.name = _name;
                this.hex = _hex;
            }
        }
    }
}
