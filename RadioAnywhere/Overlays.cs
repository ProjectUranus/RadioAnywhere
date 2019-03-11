using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using GameOverlay.Drawing;
using GameOverlay.Windows;
using GlobalHotKey;
using SharpDX.Multimedia;
using Color = GameOverlay.Drawing.Color;

namespace RadioAnywhere
{
    public class RadioOverlay
    {

        public RadioMenu CurrentMenu;

        public static List<RadioMenu> GetBuiltinMenus()
        {
            var list = new List<RadioMenu>
            {
                new RadioMenu("Radio", new HotKey(Key.Z, ModifierKeys.None), new[]
                {
                    new Radio("Cover Me", "radio/ct_coverme.wav"),
                    new Radio("You Take The Point", "radio/takepoint.wav"),
                    new Radio("Hold This Position", "radio/position.wav"),
                    new Radio("Regroup Team", "radio/regroup.wav"),
                    new Radio("Follow Me", "radio/followme.wav"),
                    new Radio("Taking Fire, Need Assistance", "radio/fireassis.wav")
                }),
                new RadioMenu("Group Radio", new HotKey(Key.X, ModifierKeys.None), new[]
                {
                    new Radio("Go", "radio/com_go.wav"),
                    new Radio("Fall Back", "radio/fallback.wav"),
                    new Radio("Stick Together Team", "radio/sticktog.wav"),
                    new Radio("Get in Position", "radio/ct_inpos.wav"),
                    new Radio("Storm the Front", "radio/stormfront.wav"),
                    new Radio("Report In", "radio/com_reportin.wav")
                }),
                new RadioMenu("Radio Responses/Reports", new HotKey(Key.C, ModifierKeys.None), new[]
                {
                    new Radio("Affirmative/Roger", "radio/roger.wav"),
                    new Radio("Enemy Spotted", "radio/ct_enemys.wav"),
                    new Radio("Need Backup", "radio/ct_backup.wav"),
                    new Radio("Sector Clear", "radio/clear.wav"),
                    new Radio("I'm in Position", "radio/ct_inpos.wav"),
                    new Radio("Reporting In", "radio/ct_reportingin.wav"),
                    new Radio("She's gonna Blow!", "radio/blow.wav"),
                    new Radio("Negative", "radio/negative.wav"),
                    new Radio("Enemy Down", "radio/enemydown.wav")
                })
            };
            return list;
        }

        public static readonly List<RadioMenu> Menus = GetBuiltinMenus();
        public readonly Dictionary<HotKey, RadioMenu> MenuMap = new Dictionary<HotKey, RadioMenu>();
        public HotKeyManager HotKeyManager = new HotKeyManager();

        public RadioOverlay()
        {
            new Thread(() =>
            {
                var window = new OverlayWindow(20, 10, 800, 800)
                {
                    IsTopmost = true,
                    IsVisible = true
                };
                window.CreateWindow();
                var graphics = new Graphics(window.Handle)
                {
                    MeasureFPS = true,
                    Height = window.Height,
                    PerPrimitiveAntiAliasing = true,
                    TextAntiAliasing = true,
                    UseMultiThreadedFactories = false,
                    VSync = true,
                    Width = window.Width
                };

                graphics.Setup();
                var font = graphics.CreateFont("Arial", 24);
                var brush = graphics.CreateSolidBrush(255, 255, 255);
                window.Show();
                while (true)
                {
                    graphics.BeginScene();
                    graphics.ClearScene();
                    graphics.DrawText(font, 24, brush, 15, (float) SystemParameters.PrimaryScreenHeight / 2.5f,
                        CurrentMenu?.ToString() == null ? "" : CurrentMenu?.ToString());
                    graphics.EndScene();
                }
            }).Start();
        }

        public void RegisterHotKey()
        {

            Menus.ForEach(menu =>
            {
                HotKeyManager.Register(menu.HotKey);
                MenuMap[menu.HotKey] = menu;
            });

            for (var i = Key.D0; i <= Key.D9; i++)
            {
                HotKeyManager.Register(i, ModifierKeys.None);
            }

            HotKeyManager.KeyPressed += (sender, args) =>
            {
                if (MenuMap.ContainsKey(args.HotKey))
                {
                    CurrentMenu = MenuMap[args.HotKey];
                }
                else if (args.HotKey.Key >= Key.D0 &&
                         args.HotKey.Key <= Key.D9 &&
                         args.HotKey.Modifiers == ModifierKeys.None &&
                         CurrentMenu != null)
                {
                    if (args.HotKey.Key == Key.D0)
                    {
                        CurrentMenu = null;
                    }
                    else
                    {
                        var radio = CurrentMenu.GetRadioForKey(args.HotKey);
                        var soundPlayer = new SoundPlayer(Path.GetFullPath(radio.SoundPath));
                        soundPlayer.Play();
                        CurrentMenu = null;
                    }
                }
            };
        }
    }
}
