<div align="center">
  <br />
    <a href="#" target="_blank">
      <img src="https://placehold.co/1000x300/04050C/3DD9B3/png?text=CyberBot:+Awareness+Protocol+v2.0&font=monospace" alt="Project Banner">
    </a>
  <br />

  <div>
    <img src="https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white" alt="C#" />
    <img src="https://img.shields.io/badge/.NET_10-512BD4?style=for-the-badge&logo=dotnet&logoColor=white" alt=".NET 10" />
    <img src="https://img.shields.io/badge/.NET_MAUI-5C2D91?style=for-the-badge&logo=dotnet&logoColor=white" alt=".NET MAUI" />
    <img src="https://img.shields.io/badge/PROG6221-Part_2-FD366E?style=for-the-badge" alt="PROG6221" />
  </div>

<h3 align="center">Cross-Platform GUI & Intelligent Cybersecurity Chatbot</h3>

   <div align="center">
     Developed by Dawood for the PROG6221 Application Development module.
    </div>
</div>

## 📋 <a name="table">Table of Contents</a>

1. 🚨 [Incident Report & Technical Deviation](#incident-report)
2. 🛡️ [Introduction](#introduction)
3. ⚙️ [Tech Stack & Architecture](#tech-stack)
4. 🔋 [Features](#features)
5. 🤸 [Quick Start](#quick-start)
6. 🎥 [Project Demonstration](#video)
7. 🖼️ [Application Gallery](#gallery)
8. 🕸️ [Code Snippets](#snippets)

---

## <a name="incident-report">🚨 Incident Report & Technical Deviation</a>

**Notice: CloudLabs VM Failure & .NET MAUI Pivot**

On Monday, May 11th, at 17:00, my campus-provided CloudLabs Windows VM suffered a critical infrastructure failure, consistently throwing a `0x204 Remote Desktop Connection` error. With the deadline approaching and no access to a native Windows environment to develop a standard WinForms/WPF application, I executed a disaster recovery pivot.

I migrated the Part 1 C# Console codebase to my local Apple MacBook Air. To fulfill the GUI requirement of Part 2 across different operating systems, I utilized **.NET MAUI** (Multi-platform App UI). This allowed me to engineer the application natively on macOS while ensuring cross-platform compilation. The application is configured to build and run seamlessly as a native WinUI 3 desktop application on the marker's Windows Visual Studio 2022 environment.

---

## <a name="introduction">🛡️ Introduction</a>

**CyberBot v2.0** is an intelligent, cross-platform Graphical User Interface (GUI) application designed to educate users on fundamental cybersecurity practices. 

Evolving from the Part 1 console app, this iteration introduces a robust Object-Oriented architecture, Generic Collections (`Dictionary`, `List`) for highly optimized memory lookup, State Variables for contextual conversational flow, and Custom Delegates to drive a dynamic Sentiment Engine that reacts to the user's emotional tone.

## <a name="tech-stack">⚙️ Tech Stack & Architecture</a>

- **Language:** C#
- **Framework:** .NET 10.0 & .NET MAUI (Multi-platform App UI)
- **Audio Integration:** Native MAUI Media Handling
- **Data Structures:** Generic Collections (`Dictionary<TKey, TValue>`, `List<T>`)
- **Advanced Logic:** Custom Delegates (`SentimentModifier`)
- **Architecture:** Strictly decoupled OOP (UI layer, Logic Brain, Animation Utility, Audio Manager)

## <a name="features">🔋 Features</a>

👉 **Cross-Platform GUI Translation:** Translated the Task 1 terminal aesthetic into a modern XAML interface featuring dark mode, cyber-green highlights, a preserved monospaced ASCII logo, and asynchronous typing animations.

👉 **State Memory & Contextual Flow:** The bot tracks `_userName`, `_lastDiscussedTopic`, and `_favoriteTopic`. It smoothly handles contextual follow-up questions (e.g., "tell me more" or "explain more") without forcing the user to repeat the core topic.

👉 **Delegate-Driven Sentiment Detection:** A custom `SentimentModifier` delegate scans user inputs for emotional keywords (e.g., "worried", "hacked", "curious"). It dynamically attaches empathetic or enthusiastic prefixes to the cybersecurity tips in real-time.

👉 **Optimized Random Response Engine:** Upgraded from standard arrays to `Dictionary<string, List<string>>` for O(1) lookups. A `do-while` loop cross-references a secondary dictionary to ensure the bot never gives the exact same tip sequentially.

👉 **Defensive Programming & Edge Cases:** The core logic is protected by a robust `try-catch` block, preventing application crashes from unexpected inputs and providing a clean, conversational fallback message for unrecognized keywords.

## <a name="quick-start">🤸 Quick Start</a>

Follow these steps to set up the project locally on a Windows machine.

**Prerequisites**
- [Visual Studio 2022](https://visualstudio.microsoft.com/) with the **.NET Multi-platform App UI development** workload installed.
- .NET 10.0 SDK

**Installation & Running**

1. Clone the repository to your local machine:
   ```bash
   git clone [https://github.com/EMGPSD/prog6221-g1-2026-part2-Dawoodkramer.git](https://github.com/EMGPSD/prog6221-g1-2026-part2-Dawoodkramer.git)
   
2. Open the PROG6221_PART2.sln file in Visual Studio 2022.

3. Allow NuGet packages to restore automatically (this will download the MAUI audio plugin).

4. Ensure the build target at the top of Visual Studio is set to Windows Machine.

5. Press F5 or click Start to run the application natively.

<a name="video">🎥 Project Demonstration</a>
<div align="center">
<a href="YOUR_NEW_YOUTUBE_LINK_HERE" target="_blank">
<img src="https://img.youtube.com/vi/YOUR_NEW_VIDEO_ID_HERE/maxresdefault.jpg" alt="Watch the CyberBot v2.0 Demonstration" width="800" />
</a>

<i>Click the image above to watch the full 8-minute technical walkthrough of the MAUI application.</i>
</div>

<a name="gallery">🖼️ Application Gallery</a>
<div align="center">
<img src="Screenshot 2026-05-14 at 18.53.27.png" alt="XAML Boot Sequence and ASCII Art" width="800"/>

<i>Cross-Platform XAML Boot Sequence & Dark Mode UI</i>

<img src="Screenshot 2026-05-14 at 18.57.24.png" alt="Contextual Chat Flow" width="800"/>

<i>Seamless Conversational Flow & State Memory</i>

<img src="Screenshot 2026-05-14 at 19.00.00.png" alt="Sentiment Delegate Engine" width="800"/>

<i>Dynamic Sentiment Detection & Personalized Responses</i>

<img src="Screenshot 2026-05-14 at 19.01.20.png" alt="Error Handling Fallback" width="800"/>

<i>Robust Try-Catch Edge Case Handling</i>
</div>

<a name="ci-status">🧪 Continuous Integration Status</a>
<div align="center">
<img src="Screenshot 2026-05-14 at 19.03.24.png" alt="Successful Releases and Tags" width="800"/>

<i>GitHub Actions: Releases and Tags</i>

<a name="ci-status">🧪 Continuous Integration Status</a>
<div align="center">
<img src="Screenshot 2026-05-14 at 19.16.58.png" alt="Successful Releases and Tags" width="800"/>

<i>GitHub Actions: Workflow</i>
</div>

<a name="snippets">🕸️ Featured Code Snippets</a>
<details>
<summary><code>ChatBot.cs (The Sentiment Delegate Engine)</code></summary>

```csharp
// 1. Delegate Definition
public delegate string SentimentModifier(string botResponse);

// 2. Execution inside GenerateResponse()
if (isCoreTopic)
{
    // Default to neutral
    SentimentModifier modifier = ApplyNeutralTone;

    // Scan for emotional keywords and assign the matching delegate method
    if (lowerInput.Contains("worried") || lowerInput.Contains("scared") || lowerInput.Contains("hacked") || lowerInput.Contains("anxious"))
    {
        modifier = ApplyEmpatheticTone;
    }
    else if (lowerInput.Contains("curious") || lowerInput.Contains("excited") || lowerInput.Contains("love"))
    {
        modifier = ApplyEnthusiasticTone;
    }

    // Execute the delegate to modify the response string dynamically
    return modifier(coreResponse);
}

// Using highly efficient Dictionaries mapping strings to Lists for O(1) lookups
private Dictionary<string, List<string>> _responsesDatabase;
private Dictionary<string, int> _lastUsedIndex;

public ChatBot() 
{
    _lastUsedIndex = new Dictionary<string, int>
    {
        { "password", -1 },
        { "phishing", -1 },
        { "browsing", -1 }
    };

    _responsesDatabase = new Dictionary<string, List<string>>
    {
        { "password", new List<string>
            { 
                "For safe passwords, always use a mix of uppercase, lowercase, numbers, and symbols.\n  - Avoid common words or personal info.\n  - Recommendation: Consider using a dedicated password manager!", 
                "A passphrase is often more secure and easier to remember than a complex short password.\n  - Example: A sequence of random words like 'PurpleElephantRunningFast!'\n  - Why: Length mathematically increases the time it takes to crack."
            }
        }
    };
}

// Safely updating the MAUI UI thread with a custom typing delay
public static async Task TypeMessageAsync(VerticalStackLayout container, ScrollView scrollView, string senderName, string message, Color textColor, int speedMs)
{
    var messageLabel = new Label
    {
        Text = $"{senderName}: ",
        TextColor = textColor,
        FontSize = 14,
        FontFamily = "Menlo",
        Margin = new Thickness(0, 5)
    };

    container.Children.Add(messageLabel);
    
    // Safely auto-scroll to the bottom of the chat view
    try { await scrollView.ScrollToAsync(container, ScrollToPosition.End, false); } catch {}

    foreach (char c in message)
    {
        messageLabel.Text += c;
        await Task.Delay(speedMs); // Custom thread delay for retro-terminal feel
    }
}