using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using MinecraftLauncher.Models;

namespace MinecraftLauncher
{
    public partial class MainForm : Form
    {
        private Button playButton;
        private ProgressBar progressBar;
        private Label statusLabel;
        private Label serverNameLabel;
        private Label versionLabel;
        private GameUpdater updater;
        private MinecraftStarter starter;
        private ConfigManager configManager;
        private bool isUpdating = false;

        public MainForm()
        {
            InitializeComponent();
            InitializeServices();
            SetupForm();
        }

        /// <summary>
        /// Инициализация сервисов
        /// </summary>
        private void InitializeServices()
        {
            updater = new GameUpdater();
            starter = new MinecraftStarter();
            configManager = new ConfigManager();
        }

        /// <summary>
        /// Настройка формы
        /// </summary>
        private void SetupForm()
        {
            this.Text = "Minecraft Launcher";
            this.Size = new System.Drawing.Size(400, 300);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = true;
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        /// <summary>
        /// Обработчик нажатия кнопки "Играть"
        /// </summary>
        private async void PlayButton_Click(object sender, EventArgs e)
        {
            if (isUpdating) return;

            try
            {
                isUpdating = true;
                playButton.Enabled = false;
                playButton.Text = "Обновление...";

                await CheckForUpdates();

                // Загрузить профиль и запустить игру
                var profile = await configManager.LoadGameProfile();
                var success = await starter.StartMinecraft(profile);

                if (success)
                {
                    UpdateProgress(100, "Игра запущена!");
                    this.WindowState = FormWindowState.Minimized;
                }
                else
                {
                    UpdateProgress(0, "Ошибка при запуске игры");
                }
            }
            catch (Exception ex)
            {
                UpdateProgress(0, $"Ошибка: {ex.Message}");
            }
            finally
            {
                isUpdating = false;
                playButton.Enabled = true;
                playButton.Text = "Играть";
            }
        }

        /// <summary>
        /// Проверка обновлений
        /// </summary>
        private async Task CheckForUpdates()
        {
            UpdateProgress(10, "Проверка версии...");

            var serverInfo = await updater.GetServerVersion();
            if (serverInfo == null)
            {
                UpdateProgress(0, "Не удалось получить информацию о версии");
                return;
            }

            UpdateProgress(30, "Проверка локальных файлов...");

            var isValid = await updater.ValidateLocalFiles(serverInfo);
            if (isValid)
            {
                UpdateProgress(100, "Готово к игре");
                return;
            }

            UpdateProgress(50, "Загрузка обновлений...");

            // TODO: Здесь должна быть логика загрузки файлов
            // Для демонстрации просто имитируем загрузку
            for (int i = 50; i <= 90; i += 10)
            {
                UpdateProgress(i, "Загрузка модов...");
                await Task.Delay(200); // Имитация загрузки
            }

            UpdateProgress(100, "Готово к игре");
        }

        /// <summary>
        /// Обновление прогресса и статуса
        /// </summary>
        /// <param name="percentage">Процент выполнения</param>
        /// <param name="status">Статус операции</param>
        private void UpdateProgress(int percentage, string status)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<int, string>(UpdateProgress), percentage, status);
                return;
            }

            progressBar.Value = Math.Min(Math.Max(percentage, 0), 100);
            statusLabel.Text = status;
        }

        /// <summary>
        /// Освобождение ресурсов
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                updater?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
