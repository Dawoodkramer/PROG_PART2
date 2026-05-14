/*
 * CODE ATTRIBUTION
 * Author: Dawood Kramer
 * Module: PROG6221
 * Task: Part 2
 * Description: The code-behind for the main graphical interface, acting as the coordinator between the UI, the ChatBot, and the AudioPlayer.
 */

using System;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace PROG6221_PART2;

public partial class MainPage : ContentPage // Declares the main graphical user interface class.
{
    private ChatBot _bot; // Declares a private object to handle the chatbot's brain and logic.
    private AudioPlayer _audioManager; // Declares a private object to manage audio playback.
    
    private bool _isBooting = true; // Declares a boolean control flag to track if the application is currently playing its boot animation.
    private bool _needsName = true; // Declares a boolean control flag to track if the user has provided their name yet.
    private string _userName = ""; // Declares a private string variable to store the user's name.
    private bool _hasBooted = false; // Declares a boolean control flag to ensure the boot sequence only triggers once per launch.

    public MainPage() // Constructor runs when the main page is first created.
    {
        InitializeComponent(); // Initializes the visual XAML components on the screen.
        
        _bot = new ChatBot(); // Instantiates the ChatBot class logic.
        _audioManager = new AudioPlayer(); // Instantiates the AudioPlayer class.
        
        UserInputEntry.IsEnabled = false; // Locks the text input box so the user cannot type during the boot sequence.
        SendBtn.IsEnabled = false; // Locks the send button during the boot sequence.

        AsciiLogoLabel.Text =  // Assigns the multi-line ASCII art string to the logo label at the top of the interface.
            "   ______      __               ____        __ \n" +
            "  / ____/_  __/ /_  ___  ____  / __ )____  / /_\n" +
            " / /   / / / / __ \\/ _ \\/ ___// __  / __ \\/ __/\n" +
            "/ /___/ /_/ / /_/ /  __/ /   / /_/ / /_/ / /_  \n" +
            "\\____/\\__, /_.___/\\___/_/   /_____/\\____/\\__/  \n" +
            "     /____/                                    ";
    }

    protected override async void OnAppearing() // Overrides the default method to run asynchronous code exactly when the window becomes visible to the user.
    {
        base.OnAppearing();
        if (!_hasBooted) // Checks if the sequence has already run to prevent loop bugs.
        {
            _hasBooted = true; // Changes the control flag to true.
            await RunBootSequenceAsync(); // Triggers the method below to simulate the terminal loading sequence.
        }
    }

    private async Task RunBootSequenceAsync() // Declares an asynchronous method to simulate the terminal loading sequence.
    {
        await Task.Delay(500); // Pauses the application briefly to let the UI settle before animating.
        
        _ = _audioManager.PlayGreetingAsync(); // Triggers the AudioPlayer object to play the .wav file in the background.

        try // Initiates a try-catch block to prevent UI thread crashes during animations.
        {
            await UIHelper.TypeMessageAsync(ChatContainer, ChatScrollView, "System", "INITIALIZING SECURE CONNECTION...", Colors.DarkGreen, 40); // Calls the static UI method to animate the text.
            await Task.Delay(600); // Pauses the application for 600 milliseconds to create a dramatic loading effect.
            await UIHelper.TypeMessageAsync(ChatContainer, ChatScrollView, "System", "ESTABLISHING ENCRYPTED TUNNEL...", Colors.DarkGreen, 40);
            await Task.Delay(600);
            await UIHelper.TypeMessageAsync(ChatContainer, ChatScrollView, "System", "AWARENESS PROTOCOL v2.0 ONLINE.", Colors.DarkGreen, 40);
            await Task.Delay(500);
            
            UIHelper.AddDivider(ChatContainer, ChatScrollView); // Calls the static method to draw the separation line.

            await UIHelper.TypeMessageAsync(ChatContainer, ChatScrollView, "Bot", "Hello! Welcome to the Cybersecurity Awareness Bot.", Colors.Cyan, 20);
            await UIHelper.TypeMessageAsync(ChatContainer, ChatScrollView, "Bot", "To configure your session profile, please enter your name:", Colors.Cyan, 20);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Animation error: {ex.Message}"); // Catches and logs any visual rendering errors.
        }
        finally // Executes this block regardless of whether the try block succeeded or crashed.
        {
            _isBooting = false; // Updates the state machine to indicate booting is finished.
            UserInputEntry.IsEnabled = true; // Unlocks the text input field.
            SendBtn.IsEnabled = true; // Unlocks the send button.
            UserInputEntry.Focus(); // Automatically places the user's cursor inside the text box.
        }
    }

    private async void OnSendClicked(object sender, EventArgs e) // Declares the event handler that triggers when the user clicks the Send button or presses Enter.
    {
        if (_isBooting) return; // Instantly exits the method if clicked while the animation is still playing.

        string userInput = UserInputEntry.Text?.Trim() ?? string.Empty; // Reads the user's input, safely checks for nulls, and strips away leading/trailing spaces.
        if (string.IsNullOrWhiteSpace(userInput)) return; // Checks if the user accidentally pressed enter without typing anything and cancels the action.

        UserInputEntry.Text = string.Empty; // Clears the text input box immediately after capturing the text.

        // STATE 1: Capturing the Name
        if (_needsName) // Checks the boolean flag to see if the application is currently waiting for the user's name.
        {
            UIHelper.AddMessageToScreen(ChatContainer, ChatScrollView, "User", userInput, Colors.LightGreen);
            _userName = userInput; // Saves the input directly into the name variable.
            _needsName = false; // Updates the control flag so this state is never entered again.
            
            UserInputEntry.IsEnabled = false; // Locks the inputs while the bot is generating its response.
            SendBtn.IsEnabled = false;

            await UIHelper.TypeMessageAsync(ChatContainer, ChatScrollView, "System", "Processing...", Colors.DarkGray, 20);
            await UIHelper.TypeMessageAsync(ChatContainer, ChatScrollView, "System", $"Identity confirmed. Welcome to the secure terminal, {_userName}.", Colors.Cyan, 30);
            await UIHelper.TypeMessageAsync(ChatContainer, ChatScrollView, "Bot", "How can I assist you with your cybersecurity needs today?", Colors.Yellow, 30);
            await UIHelper.TypeMessageAsync(ChatContainer, ChatScrollView, "Bot","You can ask questions about the following topics:", Colors.Yellow, 30);
            await UIHelper.TypeMessageAsync(ChatContainer, ChatScrollView, "Bot","1. Password safety", Colors.Yellow, 30);
            await UIHelper.TypeMessageAsync(ChatContainer, ChatScrollView, "Bot","2. Phishing", Colors.Yellow, 30);
            await UIHelper.TypeMessageAsync(ChatContainer, ChatScrollView, "Bot","3. Safe browsing", Colors.Yellow, 30);
            await UIHelper.TypeMessageAsync(ChatContainer, ChatScrollView, "System", "[System: Type 'exit' or 'quit' at any time to close the terminal.]", Colors.DarkGray, 20);
            
            UIHelper.AddDivider(ChatContainer, ChatScrollView);
            
            UserInputEntry.IsEnabled = true; // Unlocks the inputs so the user can begin asking questions.
            SendBtn.IsEnabled = true;
            UserInputEntry.Focus();
            return; // Escapes the method so it does not accidentally trigger State 3.
        }

        // STATE 2: The Exit Command
        if (userInput.ToLower() == "exit" || userInput.ToLower() == "quit") // Checks if the user typed a specific termination command.
        {
            UIHelper.AddMessageToScreen(ChatContainer, ChatScrollView, $"{_userName}@local", userInput, Colors.LightGreen);
            UserInputEntry.IsEnabled = false; // Permanently locks the UI.
            SendBtn.IsEnabled = false;
            
            await UIHelper.TypeMessageAsync(ChatContainer, ChatScrollView, "System", $"CONNECTION TERMINATED. Stay secure out there, {_userName}.", Colors.DarkGray, 40);
            await Task.Delay(1500); // Pauses dramatically to allow the user to read the message.
            Application.Current.Quit(); // Forces the MAUI application to shut down.
            return;
        }

        // STATE 3: Normal Chat Mode
        UIHelper.AddMessageToScreen(ChatContainer, ChatScrollView, $"{_userName}@local", userInput, Colors.LightGreen); // Displays the user's message in the chat log.
        
        UserInputEntry.IsEnabled = false; // Locks the inputs to prevent spam-clicking.
        SendBtn.IsEnabled = false;
        
        await UIHelper.TypeMessageAsync(ChatContainer, ChatScrollView, "System", "Processing...", Colors.DarkGray, 10); // Types a fast loading message.
        
        string botResponse = _bot.GenerateResponse(userInput, _userName); // Calls the generation method on the ChatBot object, passing the input and the user's name.
        
        await UIHelper.TypeMessageAsync(ChatContainer, ChatScrollView, "Bot", botResponse, Colors.Cyan, 15); // Displays the resulting string from the ChatBot object.
        UIHelper.AddDivider(ChatContainer, ChatScrollView);
        
        UserInputEntry.IsEnabled = true; // Unlocks the UI for the next interaction.
        SendBtn.IsEnabled = true;
        UserInputEntry.Focus();
    }
}