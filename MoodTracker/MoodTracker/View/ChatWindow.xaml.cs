using MoodTracker.Data;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using static MoodTracker.Data.ApplicationDbContext;

namespace MoodTracker.View
{
    public partial class ChatWindow : System.Windows.Controls.Page
    {

        private readonly SimpleAIChat _chatService = new SimpleAIChat("sk-76716f1a8fca45a9bfe98c01b1a6c310");
        // 如下是林豪的的AIChat对应 API （但是还没付钱）
        // private readonly SimpleAIChat _chatService = new SimpleAIChat("sk-d3b1587bd09a492ca9d6221509e9bb9d");

        public ObservableCollection<ChatRecord> ChatHistory { get; } = new();
        private readonly string ChatHistoryPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "MoodTracker", "chat_history.json");

        public ChatWindow()
        {
            InitializeComponent();
            DataContext = this;
            LoadChatHistory();
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            ProcessMessage();
        }

        private void InputBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && Keyboard.Modifiers != ModifierKeys.Shift)
            {
                ProcessMessage();
                e.Handled = true;
            }
        }

        private void ProcessMessage()
        {
            var message = InputBox.Text.Trim();
            if (string.IsNullOrWhiteSpace(message)) return;

            // 添加用户消息
            var userRecord = new ChatRecord
            {
                UserMessage = message,
                AIMessage = "Thinking...",
                UserId = "current_user_id"
            };
            ChatHistory.Add(userRecord);

            try
            {
                var response = _chatService.GetChatResponse(message).Result;

                userRecord.AIMessage = response;
                ChatHistory[^1] = userRecord;
                SaveChatHistory();
            }
            catch (Exception ex)
            {
                userRecord.AIMessage = $"Error: {ex.Message}";
                ChatHistory[^1] = userRecord;
            }

            InputBox.Clear();
            ChatList.ScrollIntoView(ChatHistory[^1]);
        }

        private void SaveChatHistory()
        {
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(ChatHistoryPath)!);
                var json = JsonSerializer.Serialize(ChatHistory);
                File.WriteAllText(ChatHistoryPath, json);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"保存聊天记录失败：{ex.Message}");
            }
        }

        private void LoadChatHistory()
        {
            try
            {
                if (File.Exists(ChatHistoryPath))
                {
                    var json = File.ReadAllText(ChatHistoryPath);
                    var history = JsonSerializer.Deserialize<ObservableCollection<ChatRecord>>(json);
                    if (history != null)
                    {
                        foreach (var record in history)
                            ChatHistory.Add(record);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载聊天记录失败：{ex.Message}");
            }
        }

    }

}