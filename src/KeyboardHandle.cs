using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;

public class KeyboardHandle
{
    private const int WH_KEYBOARD_LL = 13;
    private const int WM_KEYDOWN = 0x0100;
    private LowLevelKeyboardProc _proc;
    private IntPtr _hookID;

    public static KeyboardHandle globinstance;

    private List<HotKey> Hotkeys = new List<HotKey>();

    public static KeyboardHandle KeyboardHost
    {
        get
        {
            if(globinstance == null)
            {
                globinstance = new KeyboardHandle();
            }
            return globinstance;
        }
    }

    public void InsertNewHotkey(HotKey hotkey)
    {
        Hotkeys.Add(hotkey);
    }

    private KeyboardHandle()
    { Runn(); }

    private void Runn()
    {
        _proc = HookCallback;
        _hookID = SetHook(_proc);
        //Application.Run();
        //UnhookWindowsHookEx(_hookID);
    }

    private IntPtr SetHook(LowLevelKeyboardProc proc)
    {
        using (Process curProcess = Process.GetCurrentProcess())
        using (ProcessModule curModule = curProcess.MainModule)
        {
            return SetWindowsHookEx(WH_KEYBOARD_LL, proc,
                GetModuleHandle(curModule.ModuleName), 0);
        }
    }

    private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

    private IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
    {
        int vkCode = Marshal.ReadInt32(lParam);
        bool buttonDown = (int)wParam == 256;
        Keys key = (Keys)vkCode;
        Hotkeys.ForEach(hotkey => hotkey.OnTrigger(key, buttonDown));
        Console.WriteLine("Key " + key + " is " + (buttonDown?"Down":"UP"));

        return CallNextHookEx(_hookID, nCode, wParam, lParam);
    }

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool UnhookWindowsHookEx(IntPtr hhk);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

    [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr GetModuleHandle(string lpModuleName);

}