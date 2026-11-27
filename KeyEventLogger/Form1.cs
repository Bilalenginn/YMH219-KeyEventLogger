namespace KeyEventLogger;

public partial class Form1 : Form
{
    private readonly KeyLogger logger = new KeyLogger();

    private readonly ListBox logListBox = new ListBox();
    private readonly Label statusLabel = new Label();
    private readonly Label statsLabel = new Label();
    private readonly Button startButton = new Button();
    private readonly Button stopButton = new Button();
    private readonly Button clearButton = new Button();
    private readonly Button saveButton = new Button();

    public Form1()
    {
        InitializeComponent();
        BuildInterface();

        KeyPreview = true;
        KeyDown += Form1_KeyDown;
        KeyUp += Form1_KeyUp;

        UpdateStatus();
    }

    private void BuildInterface()
    {
        Text = "Key Event Logger - YMH219 (Eğitsel)";
        ClientSize = new Size(640, 500);
        StartPosition = FormStartPosition.CenterScreen;

        statusLabel.SetBounds(10, 10, 620, 32);
        statusLabel.Font = new Font("Segoe UI", 12, FontStyle.Bold);
        statusLabel.TextAlign = ContentAlignment.MiddleCenter;
        statusLabel.ForeColor = Color.White;

        logListBox.SetBounds(10, 52, 620, 330);
        logListBox.Font = new Font("Consolas", 10);
        logListBox.TabStop = false;

        statsLabel.SetBounds(10, 390, 620, 40);
        statsLabel.Font = new Font("Segoe UI", 10);

        SetupButton(startButton, "Başlat", 10, StartButton_Click);
        SetupButton(stopButton, "Durdur", 165, StopButton_Click);
        SetupButton(clearButton, "Temizle", 320, ClearButton_Click);
        SetupButton(saveButton, "Kaydet (.txt)", 475, SaveButton_Click);

        Controls.AddRange(new Control[]
        {
            statusLabel, logListBox, statsLabel,
            startButton, stopButton, clearButton, saveButton
        });
    }

    private void SetupButton(Button button, string text, int x, EventHandler onClick)
    {
        button.Text = text;
        button.SetBounds(x, 445, 155, 40);
        button.TabStop = false; // Space/Enter ile yanlışlıkla tetiklenmesin
        button.Click += onClick;
    }

    private void Form1_KeyDown(object? sender, KeyEventArgs e)
    {
        Log(e, "KeyDown");
    }

    private void Form1_KeyUp(object? sender, KeyEventArgs e)
    {
        Log(e, "KeyUp");
    }

    private void Log(KeyEventArgs e, string eventType)
    {
        if (!logger.IsRecording) return;

        string keyName = e.Modifiers == Keys.None
            ? e.KeyCode.ToString()
            : $"{e.Modifiers}+{e.KeyCode}";

        var entry = logger.Record(keyName, eventType);
        if (entry != null)
        {
            logListBox.Items.Add(entry.ToString());
            logListBox.TopIndex = logListBox.Items.Count - 1;
            UpdateStatus();
        }

        // Tuş, butonları veya listeyi tetiklemesin
        e.Handled = true;
        e.SuppressKeyPress = true;
    }

    private void StartButton_Click(object? sender, EventArgs e)
    {
        logger.Start();
        UpdateStatus();
        Focus();
    }

    private void StopButton_Click(object? sender, EventArgs e)
    {
        logger.Stop();
        UpdateStatus();
    }

    private void ClearButton_Click(object? sender, EventArgs e)
    {
        logger.Clear();
        logListBox.Items.Clear();
        UpdateStatus();
    }

    private void SaveButton_Click(object? sender, EventArgs e)
    {
        if (logger.Entries.Count == 0)
        {
            MessageBox.Show("Kaydedilecek kayıt yok.", "Bilgi",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        using var dialog = new SaveFileDialog
        {
            Filter = "Metin dosyası (*.txt)|*.txt",
            FileName = $"keylog_{DateTime.Now:yyyyMMdd_HHmmss}.txt"
        };

        if (dialog.ShowDialog() != DialogResult.OK) return;

        try
        {
            logger.SaveToFile(dialog.FileName);
            MessageBox.Show("Kayıtlar dosyaya kaydedildi.", "Başarılı",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException)
        {
            MessageBox.Show($"Dosya kaydedilemedi:\n{ex.Message}", "Hata",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void UpdateStatus()
    {
        bool recording = logger.IsRecording;

        statusLabel.Text = recording
            ? "● KAYIT AÇIK (yalnızca bu pencere)"
            : "● KAYIT KAPALI";
        statusLabel.BackColor = recording ? Color.Firebrick : Color.DimGray;

        startButton.Enabled = !recording;
        stopButton.Enabled = recording;

        var top = logger.TopKeys(3).Select(p => $"{p.Key} ({p.Value})");
        statsLabel.Text = $"Toplam tuş: {logger.KeyDownCount}    " +
                          $"En sık: {(logger.KeyDownCount == 0 ? "-" : string.Join(", ", top))}";
    }
}