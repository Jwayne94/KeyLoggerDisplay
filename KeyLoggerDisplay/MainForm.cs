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
            // Добавляем нажатую клавишу в ListBox
            AddKeyToLog(key);

            // Выводим сообщение в консоль для проверки
            Console.WriteLine($"Key pressed: {key}");
        }

        private void AddKeyToLog(string key)
        {
            // Добавляем новую клавишу в начало списка
            keyLogListBox.Items.Insert(0, key);

            // Ограничиваем количество элементов в списке (например, до 5)
            if (keyLogListBox.Items.Count > 5)
            {
                keyLogListBox.Items.RemoveAt(keyLogListBox.Items.Count - 1);
            }
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
AddKeyToLog :
Добавляет новую клавишу в начало списка (Items.Insert(0, key)).
Удаляет лишние записи, если их больше 5 (Items.RemoveAt).
OnKeyPressed :
Вызывает метод AddKeyToLog, чтобы обновить список.
OnFormClosing : Когда форма закрывается, мы освобождаем ресурсы хука.
*/