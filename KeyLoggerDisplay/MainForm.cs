using System;
using System.Windows.Forms;

namespace KeyLoggerDisplay
{
    public partial class MainForm : Form
    {
        private KeyboardHook _keyboardHook;

        public MainForm()
        {
            InitializeComponent();
            InitializeKeyboardHook();
        }

        private void InitializeKeyboardHook()
        {
            _keyboardHook = new KeyboardHook();
            _keyboardHook.KeyPressed += OnKeyPressed;
        }

        private void OnKeyPressed(string key)
        {
            // Здесь мы будем обрабатывать нажатые клавиши
            Console.WriteLine($"Key pressed: {key}");
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            _keyboardHook?.Dispose(); // Освободить ресурсы хука
        }
    }
}
/*
InitializeKeyboardHook : Создает экземпляр класса KeyboardHook и подписывается на событие KeyPressed.
OnKeyPressed : Метод, который вызывается каждый раз, когда нажата клавиша. На данном этапе мы просто выводим название клавиши в консоль.
OnFormClosing : Когда форма закрывается, мы освобождаем ресурсы хука.
*/