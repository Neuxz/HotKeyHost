using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

public class ActionHotKey : HotKey
{
    public Action OnHotkeyAction { get; set; }
    public ActionHotKey(Keys[] hotkeys, Action action = null, int holdtime = 0) : base(hotkeys, holdtime)
    { OnHotkeyAction = action; }

    protected override void HotkeyHandling()
    {
        if(OnHotkeyAction != null)
        OnHotkeyAction.Invoke();
    }

    public override string ToString()
    {
        return base.ToString() + " AsActionHost";
    }

}

