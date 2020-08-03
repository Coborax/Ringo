using SubtitlesParser.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ringo.Models
{
    public class Subtitle
    {
        public int StartTime { get; set; }
        public int EndTime { get; set; }
        public string Line { get; set; }

        public Subtitle(int start, int end, string line)
        {
            StartTime = start;
            EndTime = end;
            Line = line;
        }

        public Subtitle(SubtitleItem subtitleItem)
        {
            StartTime = subtitleItem.StartTime;
            EndTime = subtitleItem.EndTime;

            foreach (string line in subtitleItem.Lines)
            {
                Line += line + "\n";
            }
        }
    }
}
