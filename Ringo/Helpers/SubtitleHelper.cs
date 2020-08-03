using Ringo.Models;
using SubtitlesParser.Classes;
using SubtitlesParser.Classes.Parsers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Ringo.Helpers
{
    public class SubtitleHelper
    {
        SubParser _parser;

        public List<Subtitle> Subtitles { get; set; } = new List<Subtitle>();

        private List<SubtitleItem> _subtitleItems = new List<SubtitleItem>();

        public SubtitleHelper()
        {
            _parser = new SubParser();
        }

        public void LoadSubtitles(string path)
        {
            _subtitleItems.Clear();
            Subtitles.Clear();

            using (var fileStream = File.OpenRead(path))
            {
                _subtitleItems = _parser.ParseStream(fileStream);

                foreach (SubtitleItem subtitle in _subtitleItems)
                {
                    Subtitles.Add(new Subtitle(subtitle));
                }
            }
        }

        public SubtitleItem GetSubItemAtTime(int time)
        {
            foreach (SubtitleItem item in _subtitleItems)
            {
                if (item.StartTime <= time && item.EndTime >= time)
                    return item;
            }
            return null;
        }

        public string GetLineAtTime(int time)
        {
            Subtitle item = GetSubAtTime(time);

            if (item != null)
                return item.Line;

            return "";
        }

        public Subtitle GetSubAtTime(int time)
        {
            foreach (Subtitle item in Subtitles)
            {
                if (item.StartTime <= time && item.EndTime >= time)
                    return item;
            }
            return null;
        }

    }
}
