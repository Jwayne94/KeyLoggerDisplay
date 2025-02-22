using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

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

        // Событие, которое будет вызываться при нажатии клавиш
        public event Action<string> KeyPressed;

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
                string key = ((Keys)vkCode).ToString();

                // Вызываем событие KeyPressed
                KeyPressed?.Invoke(key);
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
    }
}

/*
HookProc : Это делегат, который будет вызываться каждый раз, когда происходит событие клавиш.
SetHook : Метод устанавливает глобальный хук для отслеживания клавиш.
HookCallback : Этот метод обрабатывает события клавиш. Если клавиша была нажата (WM_KEYDOWN), он преобразует код клавиши в строку и вызывает событие KeyPressed.
Dispose : Метод освобождает ресурсы, связанные с хуком.
*/
