using System;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace PROG6221_PART2;

public partial class MainPage : ContentPage
{
    // OOP: Instantiating our dedicated objects
    private ChatBot _bot;
    private AudioPlayer _audioManager;
    
    // State Machine Trackers
    private bool _isBooting = true;
    private bool _needsName = true;
    private string _userName = "";
    private bool _hasBooted = false;

    public MainPage()
    {
        InitializeComponent();
        
        _bot = new ChatBot();
        _audioManager = new AudioPlayer();
        
        UserInputEntry.IsEnabled = false;
        SendBtn.IsEnabled = false;

        AsciiLogoLabel.Text = 
            "   ______      __               ____        __ \n" +
            "  / ____/_  __/ /_  ___  ____  / __ )____  / /_\n" +
            " / /   / / / / __ \\/ _ \\/ ___// __  / __ \\/ __/\n" +
            "/ /___/ /_/ / /_/ /  __/ /   / /_/ / /_/ / /_  \n" +
            "\\____/\\__, /_.___/\\___/_/   /_____/\\____/\\__/  \n" +
            "     /____/                                    ";
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (!_hasBooted)
        {
            _hasBooted = true;
            await RunBootSequenceAsync();
        }
    }

    private async Task RunBootSequenceAsync()
    {
        await Task.Delay(500);
        
        // Use our new Audio object
        _ = _audioManager.PlayGreetingAsync();

        try 
        {
            // Use our new UIHelper static methods
            await UIHelper.TypeMessageAsync(ChatContainer, ChatScrollView, "System", "INITIALIZING SECURE CONNECTION...", Colors.DarkGreen, 40);
            await Task.Delay(600);
            await UIHelper.TypeMessageAsync(ChatContainer, ChatScrollView, "System", "ESTABLISHING ENCRYPTED TUNNEL...", Colors.DarkGreen, 40);
            await Task.Delay(600);
            await UIHelper.TypeMessageAsync(ChatContainer, ChatScrollView, "System", "AWARENESS PROTOCOL v2.0 ONLINE.", Colors.DarkGreen, 40);
            await Task.Delay(500);
            
            UIHelper.AddDivider(ChatContainer, ChatScrollView);

            await UIHelper.TypeMessageAsync(ChatContainer, ChatScrollView, "Bot", "Hello! Welcome to the Cybersecurity Awareness Bot.", Colors.Cyan, 20);
            await UIHelper.TypeMessageAsync(ChatContainer, ChatScrollView, "Bot", "To configure your session profile, please enter your name:", Colors.Cyan, 20);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Animation error: {ex.Message}");
        }
        finally 
        {
            _isBooting = false;
            UserInputEntry.IsEnabled = true;
            SendBtn.IsEnabled = true;
            UserInputEntry.Focus();
        }
    }

    private async void OnSendClicked(object sender, EventArgs e)
    {
        if (_isBooting) return; 

        string userInput = UserInputEntry.Text?.Trim() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(userInput)) return; 

        UserInputEntry.Text = string.Empty;

        // STATE 1: Capturing the Name
        if (_needsName)
        {
            UIHelper.AddMessageToScreen(ChatContainer, ChatScrollView, "User", userInput, Colors.LightGreen);
            _userName = userInput;
            _needsName = false; 
            
            UserInputEntry.IsEnabled = false;
            SendBtn.IsEnabled = false;

            await UIHelper.TypeMessageAsync(ChatContainer, ChatScrollView, "System", "Processing...", Colors.DarkGray, 20);
            await UIHelper.TypeMessageAsync(ChatContainer, ChatScrollView, "System", $"Identity confirmed. Welcome to the secure terminal, {_userName}.", Colors.Cyan, 30);
            await UIHelper.TypeMessageAsync(ChatContainer, ChatScrollView, "Bot", "How can I assist you today? (Topics: Passwords, Phishing, Safe Browsing)", Colors.Yellow, 30);
            await UIHelper.TypeMessageAsync(ChatContainer, ChatScrollView, "System", "[System: Type 'exit' or 'quit' at any time to close the terminal.]", Colors.DarkGray, 20);
            
            UIHelper.AddDivider(ChatContainer, ChatScrollView);
            
            UserInputEntry.IsEnabled = true;
            SendBtn.IsEnabled = true;
            UserInputEntry.Focus();
            return;
        }

        // STATE 2: The Exit Command
        if (userInput.ToLower() == "exit" || userInput.ToLower() == "quit")
        {
            UIHelper.AddMessageToScreen(ChatContainer, ChatScrollView, $"{_userName}@local", userInput, Colors.LightGreen);
            UserInputEntry.IsEnabled = false;
            SendBtn.IsEnabled = false;
            
            await UIHelper.TypeMessageAsync(ChatContainer, ChatScrollView, "System", $"CONNECTION TERMINATED. Stay secure out there, {_userName}.", Colors.DarkGray, 40);
            await Task.Delay(1500);
            Application.Current.Quit();
            return;
        }

        // STATE 3: Normal Chat Mode
        UIHelper.AddMessageToScreen(ChatContainer, ChatScrollView, $"{_userName}@local", userInput, Colors.LightGreen);
        
        UserInputEntry.IsEnabled = false;
        SendBtn.IsEnabled = false;
        
        await UIHelper.TypeMessageAsync(ChatContainer, ChatScrollView, "System", "Processing...", Colors.DarkGray, 10);
        
        // Query the ChatBot Brain
        string botResponse = _bot.GenerateResponse(userInput, _userName);
        
        await UIHelper.TypeMessageAsync(ChatContainer, ChatScrollView, "Bot", botResponse, Colors.Cyan, 15);
        UIHelper.AddDivider(ChatContainer, ChatScrollView);
        
        UserInputEntry.IsEnabled = true;
        SendBtn.IsEnabled = true;
        UserInputEntry.Focus();
    }
}