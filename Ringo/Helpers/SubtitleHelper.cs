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

        public List<SubtitleItem> SubtitleItems { get; set; } = new List<SubtitleItem>();

        public SubtitleHelper()
        {
            _parser = new SubParser();
        }

        public void LoadSubtitles(string path)
        {
            SubtitleItems.Clear();

            using (var fileStream = File.OpenRead(path))
            {
                SubtitleItems = _parser.ParseStream(fileStream);
            }
        }

        public string GetLineAtTime(int time)
        {
            SubtitleItem item = GetSubAtTime(time);

            if (item != null)
            {
                return item.Lines[0];
            }

            return "";
        }

        public SubtitleItem GetSubAtTime(int time)
        {
            foreach (SubtitleItem item in SubtitleItems)
            {
                if (item.StartTime <= time && item.EndTime >= time)
                    return item;
            }
            return null;
        }

    }
}
