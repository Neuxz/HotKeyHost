using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

public class EventHotKey : HotKey
{
    public event OnHotkey HotKeyTriggert;
    public delegate void OnHotkey(HotKey sender);

    public EventHotKey(Keys[] hotkeys, int holdtime = 0) : base(hotkeys, holdtime)
    { }
    protected override void HotkeyHandling()
    {
        if(HotKeyTriggert == null)
        {
            HotKeyTriggert += (HotKey send) => { Console.WriteLine(send); };
        }
        HotKeyTriggert(this);
    }

    public override string ToString()
    {
        return base.ToString() + " AsEventTrigger";
    }
}

