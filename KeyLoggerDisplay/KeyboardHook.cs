using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace KeyLoggerDisplay
{
    public class KeyboardHook : IDisposable
    {
        // Делегат для обработки событий клавиш
        public delegate IntPtr HookProc(int nCode, IntPtr wParam, IntPtr lParam);

        // Идентификатор типа хука для клавиш
        private const int WH_KEYBOARD_LL = 13;

        // Указатель на установленный хук
        private IntPtr _hookId = IntPtr.Zero;

        // События
        public event Action<string> KeyPressed;
        public event Action HotkeyPressed;

        // Текущая горячая клавиша (по умолчанию 'K')
        private Keys _hotkey = Keys.K;

        // Свойство для изменения горячей клавиши
        public Keys Hotkey
        {
            get => _hotkey;
            set
            {
                if (_hotkey != value)
                {
                    _hotkey = value;
                    Console.WriteLine($"Hotkey updated to Ctrl + Shift + {_hotkey}");
                }
            }
        }

        // Конструктор
        public KeyboardHook()
        {
            // Устанавливаем хук
            _hookId = SetHook(HookCallback);
        }

        // Метод установки хука
        private IntPtr SetHook(HookProc hookProc)
        {
            using (var curProcess = System.Diagnostics.Process.GetCurrentProcess())
            using (var curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_KEYBOARD_LL, hookProc, GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        // Обратный вызов для обработки событий клавиш
        private IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN)
            {
                int vkCode = Marshal.ReadInt32(lParam);

                // Проверяем состояние модификаторов
                bool isCtrlPressed = GetKeyState((int)Keys.ControlKey) < 0;
                bool isShiftPressed = GetKeyState((int)Keys.ShiftKey) < 0;
                bool isAltPressed = GetKeyState((int)Keys.Menu) < 0;

                // Собираем комбинацию клавиш
                string combination = "";

                if (isCtrlPressed) combination += "Ctrl + ";
                if (isShiftPressed) combination += "Shift + ";
                if (isAltPressed) combination += "Alt + ";

                // Добавляем основную клавишу
                string key = ((Keys)vkCode).ToString();
                combination += key;

                // Удаляем лишний пробел и "+", если комбинация пустая
                if (combination.EndsWith(" + ")) combination = combination.TrimEnd(' ', '+');

                // Вызываем событие KeyPressed с комбинацией
                KeyPressed?.Invoke(combination);

                // Обработка горячих клавиш
                if (isCtrlPressed && isShiftPressed && vkCode == (int)Keys.K)
                {
                    HotkeyPressed?.Invoke();
                    return IntPtr.Zero; // Прерываем дальнейшую обработку
                }
            }

            return CallNextHookEx(_hookId, nCode, wParam, lParam);
        }

        // Освобождение ресурсов
        public void Dispose()
        {
            UnhookWindowsHookEx(_hookId);
        }

        // Постоянные значения для работы с Windows API
        private const int WM_KEYDOWN = 0x0100;

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        [DllImport("user32.dll")]
        private static extern short GetKeyState(int nVirtKey);
    }
}