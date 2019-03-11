using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GlobalHotKey;
using SharpDX.Multimedia;

namespace RadioAnywhere
{
    public class RadioMenu
    {
        public readonly string Name;
        public readonly HotKey HotKey;
        public readonly List<Radio> Radios = new List<Radio>();

        public RadioMenu(string name, HotKey hotKey, IEnumerable<Radio> radios)
        {
            Name = name;
            HotKey = hotKey;
            Radios.AddRange(radios);
        }

        public override string ToString()
        {
            var builder = new StringBuilder($"{Name} Commands");
            builder.AppendLine();
            for (var i = 0; i < Radios.Count; i++)
            {
                builder.AppendLine($"{i + 1}. \"{Radios[i].Name}\"");
            }

            builder.AppendLine();
            builder.AppendLine("0. Exit");
            return builder.ToString();
        }

        public Radio GetRadioForKey(HotKey key)
        {
            return Radios[key.Key - Key.D1];
        }
    }

    public class Radio
    {
        public readonly string Name;
        public readonly string SoundPath;

        public Radio(string name, string soundPath)
        {
            Name = name;
            SoundPath = soundPath;
        }
    }
}
