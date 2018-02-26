using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GlobalHotkeyHostTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Keys> KeysOfCurrentHotkey = new List<Keys>();

        KeyboardHandle keyController;

        public MainWindow()
        {
            InitializeComponent();
            KeysBox.ItemsSource = Enum.GetValues(typeof(Keys));
            HotKeyListBox.Items.Insert(0, "--");
            this.Loaded += (sende, e) =>
            keyController = KeyboardHandle.KeyboardHost;
        }

        private void AddKey(object sender, RoutedEventArgs e)
        {
            Keys key = (Keys)KeysBox.SelectedItem;
            KeysOfCurrentHotkey.Add(key);
            string keys = "";
            KeysOfCurrentHotkey.ForEach(keyli => keys += (keyli.ToString() + " - "));
            HotKeyListBox.Items.RemoveAt(0);
            HotKeyListBox.Items.Insert(0, keys);
        }

        private void FinalizeHotkey(object sender, RoutedEventArgs e)
        {
            HotKey hotkey;
            if(EventHotKey.IsChecked == true)
            {
                hotkey = new EventHotKey(KeysOfCurrentHotkey.ToArray());
                ((EventHotKey)hotkey).HotKeyTriggert += (HotKey senderh) => ActionsHandle.Content = senderh.ToString() + " is Triggert!";
            }
            else if(ActionHotKey.IsChecked == true)
            {
                hotkey = new ActionHotKey(KeysOfCurrentHotkey.ToArray());
                ((ActionHotKey)hotkey).OnHotkeyAction = (new Action(() => ActionsHandle.Content = hotkey.ToString() + " is Triggert!"));
            }
            else
            { return; }
            KeysOfCurrentHotkey = new List<Keys>();
            HotKeyListBox.Items.Add(hotkey.ToString());
            HotKeyListBox.Items.RemoveAt(0);
            HotKeyListBox.Items.Insert(0, "--");
            keyController.InsertNewHotkey(hotkey);
        }

    }
}
