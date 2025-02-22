using System;
using System.Drawing;
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

        private bool _isDragging = false;
        private Point _dragStartPoint;

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            if (e.Button == MouseButtons.Left)
            {
                _isDragging = true;
                _dragStartPoint = new Point(e.X, e.Y);
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);

            _isDragging = false;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (_isDragging)
            {
                // Рассчитываем новую позицию окна
                Point newLocation = new Point(
                    this.Location.X + (e.X - _dragStartPoint.X),
                    this.Location.Y + (e.Y - _dragStartPoint.Y)
                );

                this.Location = newLocation; // Обновляем позицию окна
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            // Центрируем окно на экране
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(
                (Screen.PrimaryScreen.Bounds.Width - this.Width) / 2,
                (Screen.PrimaryScreen.Bounds.Height - this.Height) / 2
            );
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            // Сворачиваем окно в трей после его полного отображения
            Hide();
            notifyIcon.Visible = true; // Показываем иконку в трее
        }

        private void showMenuItem_Click(object sender, EventArgs e)
        {
            // Показываем окно
            Show();
            WindowState = FormWindowState.Normal;
        }

        private void exitMenuItem_Click(object sender, EventArgs e)
        {
            // Закрываем приложение
            Application.Exit();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            // Если форма свернута, скрываем ее и показываем иконку в трее
            if (WindowState == FormWindowState.Minimized)
            {
                Hide();
                notifyIcon.Visible = true;
            }
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
OnMouseDown :
Когда пользователь нажимает левую кнопку мыши, мы начинаем отслеживать перемещение.
Сохраняем начальные координаты клика (_dragStartPoint).
OnMouseUp :
Когда пользователь отпускает кнопку мыши, мы прекращаем перемещение.
OnMouseMove :
Если перемещение активно (_isDragging == true), рассчитываем новую позицию окна относительно начальных координат и обновляем его местоположение.
MainForm_Load :
При запуске приложения окно автоматически сворачивается в трей.
showMenuItem_Click :
Когда пользователь выбирает пункт "Show", окно показывается, а иконка в трее скрывается.
exitMenuItem_Click :
Когда пользователь выбирает пункт "Exit", приложение закрывается.
OnResize :
Если пользователь сворачивает окно, оно скрывается, а иконка становится видимой в трее.
*/