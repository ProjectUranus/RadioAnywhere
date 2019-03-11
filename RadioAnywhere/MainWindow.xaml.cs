using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Runtime.InteropServices;
using System.Threading;
using GlobalHotKey;

namespace RadioAnywhere
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private RadioOverlay radioOverlay;
        private HotKeyManager builtinManager = new HotKeyManager();

        public MainWindow()
        {
            InitializeComponent();
            radioOverlay = new RadioOverlay();
            BtnEnable.Click += BtnEnable_Click;

            builtinManager.Register(Key.Tab, ModifierKeys.Control);
            builtinManager.KeyPressed += BtnEnable_Click;
        }

        private void BtnEnable_Click(object sender, object e)
        {
            if (BtnEnable.Content.Equals("启用"))
            {
                BtnEnable.Content = "禁用";
                radioOverlay.HotKeyManager = new HotKeyManager();
                radioOverlay.RegisterHotKey();
            }
            else
            {
                BtnEnable.Content = "启用";
                radioOverlay.HotKeyManager.Dispose();
            }
        }
    }
}
