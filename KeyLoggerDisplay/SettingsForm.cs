using System;
using System.Windows.Forms;

namespace KeyLoggerDisplay
{
    public partial class SettingsForm : Form
    {
        public string SelectedHotkey { get; private set; }

        public SettingsForm()
        {
            InitializeComponent();
            PopulateHotkeyComboBox();
        }

        private void PopulateHotkeyComboBox()
        {
            // Заполняем ComboBox доступными клавишами
            for (char c = 'A'; c <= 'Z'; c++)
            {
                hotkeyComboBox.Items.Add(c.ToString());
            }

            for (int i = 1; i <= 12; i++)
            {
                hotkeyComboBox.Items.Add($"F{i}");
            }

            // Устанавливаем текущую клавишу (например, K)
            hotkeyComboBox.SelectedItem = "K";
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            // Сохраняем выбранную клавишу
            SelectedHotkey = hotkeyComboBox.SelectedItem?.ToString();
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void SaveSettings(string hotkey)
        {
            // Сохраняем настройки в файл
            System.IO.File.WriteAllText("settings.txt", hotkey);
        }

        private string LoadSettings()
        {
            // Загружаем настройки из файла
            if (System.IO.File.Exists("settings.txt"))
            {
                return System.IO.File.ReadAllText("settings.txt");
            }
            return "K"; // 默认 значение
        }


    }
}

/*
PopulateHotkeyComboBox :
Заполняет ComboBox доступными клавишами (буквы A-Z и функциональные клавиши F1-F12).
saveButton_Click :
Сохраняет выбранную клавишу и закрывает форму.
SelectedHotkey :
Свойство для получения выбранной клавиши из формы настроек.
*/