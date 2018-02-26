using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


public abstract class HotKey
{
    private List<KeyState> Hotkeys = new List<KeyState>();

    Timer downTimer = new Timer();

    public HotKey(Keys[] hotkeys, int holdtime = 0)
    {
        Hotkeys = hotkeys.Select(key => new KeyState() { Key = key, IsDown = false }).ToList();
        if (holdtime > 0)
        {
            downTimer.Interval = holdtime;
        }
    }

    public void OnTrigger(Keys key, bool isDown)
    {
        Hotkeys.Where(keystate => keystate.Key == key).ToList().ForEach(keystate => keystate.IsDown = isDown);

        if(Hotkeys.Count(keystate => keystate.IsDown == true) >= Hotkeys.Count())
        {
            HotkeyHandling();
        }
    }

    public virtual String ToString()
    {
        string keysS = "Hotkeyset - ";
        Hotkeys.ForEach(keyset => keysS += keyset.Key.ToString() + " - ");
        return keysS;
    }

    protected abstract void HotkeyHandling();

    private class KeyState
    {
        public Keys Key { get; set; }
        public bool IsDown { get; set; }
    }
}

