using System;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Plugin.Maui.Audio; 

namespace PROG6221_PART2;

public partial class MainPage : ContentPage
{
    private ChatBot _bot;
    private IAudioPlayer _audioPlayer;
    
    // State Machine Trackers
    private bool _isBooting = true;
    private bool _needsName = true;
    private string _userName = "";
    private bool _hasBooted = false;

    public MainPage()
    {
        InitializeComponent();
        _bot = new ChatBot();
        
        // Lock the input box while booting
        UserInputEntry.IsEnabled = false;
        SendBtn.IsEnabled = false;

        // Draw the ASCII Logo immediately
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
        
        // Ensure the boot sequence only runs once per launch
        if (!_hasBooted)
        {
            _hasBooted = true;
            await RunBootSequenceAsync();
        }
    }

    private async Task RunBootSequenceAsync()
    {
        // Give the UI a half-second to settle before the sequence starts
        await Task.Delay(500);
        
        PlayWelcomeAudio();

        try 
        {
            // Simulate the Part 1 Boot Sequence
            await TypeMessageAsync("System", "INITIALIZING SECURE CONNECTION...", Colors.DarkGreen, 40);
            await Task.Delay(600);
            await TypeMessageAsync("System", "ESTABLISHING ENCRYPTED TUNNEL...", Colors.DarkGreen, 40);
            await Task.Delay(600);
            await TypeMessageAsync("System", "AWARENESS PROTOCOL v2.0 ONLINE.", Colors.DarkGreen, 40);
            await Task.Delay(500);
            
            AddDivider();

            await TypeMessageAsync("Bot", "Hello! Welcome to the Cybersecurity Awareness Bot. I'm here to help you stay safe online.", Colors.Cyan, 20);
            await TypeMessageAsync("Bot", "To configure your session profile, please enter your name:", Colors.Cyan, 20);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Animation error: {ex.Message}");
        }
        finally 
        {
            // FINALLY BLOCK: This guarantees the UI unlocks even if an animation frame drops
            _isBooting = false;
            UserInputEntry.IsEnabled = true;
            SendBtn.IsEnabled = true;
            UserInputEntry.Focus();
        }
    }

    private async void PlayWelcomeAudio()
    {
        try 
        {
            var audioStream = await FileSystem.OpenAppPackageFileAsync("voiceover-Dawood.wav");
            _audioPlayer = AudioManager.Current.CreatePlayer(audioStream);
            _audioPlayer.Play();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Audio failed to play: {ex.Message}");
        }
    }

    private async void OnSendClicked(object sender, EventArgs e)
    {
        if (_isBooting) return; // Prevent clicking during boot

        string userInput = UserInputEntry.Text?.Trim() ?? string.Empty;

        if (string.IsNullOrWhiteSpace(userInput))
            return; 

        UserInputEntry.Text = string.Empty;

        // STATE 1: Capturing the Name
        if (_needsName)
        {
            AddMessageToScreen("User", userInput, Colors.LightGreen);
            _userName = userInput;
            _needsName = false; // Move to the next state
            
            // Disable while bot responds
            UserInputEntry.IsEnabled = false;
            SendBtn.IsEnabled = false;

            await TypeMessageAsync("System", "Processing...", Colors.DarkGray, 20);
            await TypeMessageAsync("System", $"Identity confirmed. Welcome to the secure terminal, {_userName}.", Colors.Cyan, 30);
            await TypeMessageAsync("Bot", "How can I assist you with your cybersecurity needs today? (Topics: Passwords, Phishing, Safe Browsing)", Colors.Yellow, 30);
            
            // ==========================================
            // THE FIX: Added the missing system notification here!
            // ==========================================
            await TypeMessageAsync("System", "[System: Type 'exit' or 'quit' at any time to close the terminal.]", Colors.DarkGray, 20);
            
            AddDivider();
            
            UserInputEntry.IsEnabled = true;
            SendBtn.IsEnabled = true;
            UserInputEntry.Focus();
            return;
        }

        // ==========================================
        // THE FIX: Intercept the Exit Command
        // ==========================================
        if (userInput.ToLower() == "exit" || userInput.ToLower() == "quit")
        {
            AddMessageToScreen($"{_userName}@local", userInput, Colors.LightGreen);
            
            // Lock the terminal permanently
            UserInputEntry.IsEnabled = false;
            SendBtn.IsEnabled = false;
            
            // Print the termination sequence
            await TypeMessageAsync("System", $"CONNECTION TERMINATED. Stay secure out there, {_userName}.", Colors.DarkGray, 40);
            
            // Dramatic pause before actually closing the Mac application
            await Task.Delay(1500);
            Application.Current.Quit();
            return;
        }

        // STATE 2: Normal Chat Mode
        AddMessageToScreen($"{_userName}@local", userInput, Colors.LightGreen);
        
        // Disable input while bot "thinks"
        UserInputEntry.IsEnabled = false;
        SendBtn.IsEnabled = false;
        
        await TypeMessageAsync("System", "Processing...", Colors.DarkGray, 10);
        
        // Get response from ChatBot brain
        string botResponse = _bot.GenerateResponse(userInput, _userName);
        
        // Display response
        await TypeMessageAsync("Bot", botResponse, Colors.Cyan, 15);
        AddDivider();
        
        UserInputEntry.IsEnabled = true;
        SendBtn.IsEnabled = true;
        UserInputEntry.Focus();
    }

    // ==========================================
    // UI HELPER METHODS
    // ==========================================
    
    private async Task TypeMessageAsync(string senderName, string message, Color textColor, int speedMs)
    {
        var messageLabel = new Label
        {
            Text = $"{senderName}: ",
            TextColor = textColor,
            FontSize = 14,
            FontFamily = "Menlo",
            Margin = new Thickness(0, 5)
        };

        ChatContainer.Children.Add(messageLabel);
        
        try { await ChatScrollView.ScrollToAsync(ChatContainer, ScrollToPosition.End, false); } catch {}

        foreach (char c in message)
        {
            messageLabel.Text += c;
            await Task.Delay(speedMs); 
        }
    }

    private void AddMessageToScreen(string senderName, string message, Color textColor)
    {
        var messageLabel = new Label
        {
            Text = $"{senderName}: {message}",
            TextColor = textColor,
            FontSize = 14,
            FontFamily = "Menlo",
            Margin = new Thickness(0, 5)
        };

        ChatContainer.Children.Add(messageLabel);
        try { ChatScrollView.ScrollToAsync(ChatContainer, ScrollToPosition.End, true); } catch {}
    }

    private void AddDivider()
    {
        var dividerLabel = new Label
        {
            Text = "---------------------------------------------------",
            TextColor = Colors.DarkGray,
            FontSize = 14,
            FontFamily = "Menlo",
            Margin = new Thickness(0, 5)
        };
        ChatContainer.Children.Add(dividerLabel);
        try { ChatScrollView.ScrollToAsync(ChatContainer, ScrollToPosition.End, true); } catch {}
    }
}