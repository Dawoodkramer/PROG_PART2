/*
 * CODE ATTRIBUTION
 * Author: Dawood Kramer
 * Module: PROG6221
 * Task: Part 2
 * Description: The main logic and brain of the chatbot, featuring Generic Collections, State Memory, and custom Delegates for sentiment analysis.
 */

using System;
using System.Collections.Generic;

namespace PROG6221_PART2 
{ 
    // 
    // DELEGATE DEFINITION 
    // 
    // Declares a public delegate variable that holds a method used to dynamically modify the bot's tone based on the user's emotional state.
    public delegate string SentimentModifier(string botResponse);

    public class ChatBot 
    {
        private Random rand = new Random(); // Instantiates a random number generator object which is used to pick different responses.
        
        private Dictionary<string, List<string>> _responsesDatabase; // Declares a private generic Dictionary collection to map topic strings to a list of responses.
        private Dictionary<string, int> _lastUsedIndex; // Declares a private generic Dictionary to remember the last used index for each topic to prevent repetition.
        
        private string _lastDiscussedTopic = ""; // Declares a private string variable to track the short-term memory of the conversation.
        private string _favoriteTopic = ""; // Declares a private string variable to store the user's preferred topic for long-term recall.

        public ChatBot() // Constructor runs when the ChatBot object is first instantiated, loading all data.
        {
            _lastUsedIndex = new Dictionary<string, int> // Initializes the tracking dictionary with default values of -1.
            {
                { "password", -1 },
                { "phishing", -1 },
                { "browsing", -1 }
            };

            _responsesDatabase = new Dictionary<string, List<string>> // Initializes the main database mapping topics to their respective lists of cybersecurity tips.
            {
                { "password", new List<string>
                    { 
                        "For safe passwords, always use a mix of uppercase, lowercase, numbers, and symbols.\n  - Avoid common words or personal info.\n  - Recommendation: Consider using a dedicated password manager!", 
                        "A passphrase is often more secure and easier to remember than a complex short password.\n  - Example: A sequence of random words like 'PurpleElephantRunningFast!'\n  - Why: Length mathematically increases the time it takes to crack.", 
                        "Never reuse the same password across multiple websites.\n  - The Risk: If one minor site gets breached, hackers will try that same password on your banking and email accounts.\n  - Action: Generate unique credentials for every platform."
                    }
                },
                { "phishing", new List<string>
                    {
                        "Phishing is a cyber attack where attackers disguise themselves to get sensitive information.\n  - Action: Always verify the actual sender's address, not just their display name.\n  - Rule: Avoid clicking unknown links.", 
                        "Never provide personal or financial information in response to an unsolicited email or popup.\n  - Fact: Legitimate companies, banks, and IT departments will NEVER ask for your password via email.", 
                        "Look out for poor spelling and grammar in emails.\n  - Why: These are common red flags for phishing attempts originating from automated scam networks.\n  - Always read official correspondence carefully."
                    }
                },
                { "browsing", new List<string>
                    { 
                        "For safe browsing, ensure websites use HTTPS.\n  - The 'S' stands for Secure, meaning your data is encrypted between your browser and the website.\n  - Never download executable files from untrusted sources.", 
                        "Avoid using public, unsecured Wi-Fi networks for online banking or entering sensitive information.\n  - Risk: Hackers can easily intercept data on public networks.\n  - Solution: Use a Virtual Private Network (VPN) if you must.", 
                        "Keep your web browser and all its plugins updated to the latest versions.\n  - Why: Developers constantly release updates to patch newly discovered security vulnerabilities.\n  - Turn on auto-updates for maximum safety."
                    }
                }
            };
        }

        public string GenerateResponse(string input, string userName) // Declares a method that processes the user's input and returns a dynamically generated string.
        { 
            // 
            // ERROR HANDLING 
            //
            try // Initiates a try-catch block to prevent the application from crashing on unexpected inputs.
            {
                string lowerInput = input.ToLower(); // Converts the user's input to lowercase to make keyword matching easier.
                string coreResponse = ""; // Declares an empty string variable to hold the primary educational response.
                bool isCoreTopic = false; // Declares a boolean control flag to track if a primary cybersecurity topic was identified.

                // 1. Memory: Favorite Topic
                if (lowerInput.Contains("favorite") || lowerInput.Contains("favourite") || lowerInput.Contains("interested in")) // Checks if the user is establishing a long-term preference.
                {
                    if (lowerInput.Contains("password")) { _favoriteTopic = "password"; return $"Great, {userName}! I'll remember that you are highly interested in password security. Let me know when you want a tip!"; }
                    if (lowerInput.Contains("phish") || lowerInput.Contains("email")) { _favoriteTopic = "phishing"; return $"Excellent, {userName}! I've noted that phishing defense is your favorite topic. Ask me for a tip anytime!"; }
                    if (lowerInput.Contains("browse") || lowerInput.Contains("safe") || lowerInput.Contains("privacy")) { _favoriteTopic = "browsing"; return $"Noted, {userName}! Safe browsing is a crucial favorite topic to have. What would you like to know?"; }
                }

                // 2. Memory: Follow-up Questions
                if (lowerInput.Contains("tell me more") || lowerInput.Contains("another one") || lowerInput.Contains("explain more")) // Checks if the user is asking a contextual follow-up question.
                {
                    if (!string.IsNullOrEmpty(_lastDiscussedTopic)) // Checks the short-term memory to see if a topic was recently discussed.
                    {
                        coreResponse = GetRandomResponse(_lastDiscussedTopic);
                        isCoreTopic = true;
                    }
                    else if (!string.IsNullOrEmpty(_favoriteTopic)) // Falls back to long-term memory if no recent topic exists.
                    {
                        coreResponse = GetRandomResponse(_favoriteTopic);
                        isCoreTopic = true;
                    }
                    else
                    {
                        return "I'm not sure which topic you'd like more information on. Could you specify if you want to know more about passwords, phishing, or safe browsing?";
                    }
                }

                // 3. Core Topic Recognition
                if (!isCoreTopic) // Bypasses topic searching if a follow-up response was already generated.
                {
                    if (lowerInput.Contains("password")) // Checks if the lowercase user input contains the word "password".
                    { 
                        _lastDiscussedTopic = "password"; // Updates the tracker variable in memory.
                        coreResponse = GetRandomResponse("password"); 
                        isCoreTopic = true;
                    } 
                    else if (lowerInput.Contains("phishing") || lowerInput.Contains("email") || lowerInput.Contains("scam")) // Checks for phishing-related keywords.
                    { 
                        _lastDiscussedTopic = "phishing"; 
                        coreResponse = GetRandomResponse("phishing"); 
                        isCoreTopic = true;
                    } 
                    else if (lowerInput.Contains("safe") || lowerInput.Contains("browsing") || lowerInput.Contains("link") || lowerInput.Contains("web")) // Checks for browsing-related keywords.
                    { 
                        _lastDiscussedTopic = "browsing"; 
                        coreResponse = GetRandomResponse("browsing"); 
                        isCoreTopic = true;
                    } 
                }

                // 
                // THE SENTIMENT ENGINE (Using Delegates)
                // 
                if (isCoreTopic) // Only applies emotional modifiers if a core educational response was actually generated.
                {
                    SentimentModifier modifier = ApplyNeutralTone; // Assigns the default neutral method to the delegate.

                    // Scans the input for emotional keywords and reassigns the delegate method if a match is found.
                    if (lowerInput.Contains("worried") || lowerInput.Contains("scared") || lowerInput.Contains("hacked") || lowerInput.Contains("anxious"))
                    {
                        modifier = ApplyEmpatheticTone; // Reassigns the delegate to the empathetic method.
                    }
                    else if (lowerInput.Contains("curious") || lowerInput.Contains("excited") || lowerInput.Contains("love"))
                    {
                        modifier = ApplyEnthusiasticTone; // Reassigns the delegate to the enthusiastic method.
                    }

                    return modifier(coreResponse); // Executes the delegate, modifying the core response string before returning it to the user.
                }

                // 4. Conversational Fallbacks
                if (lowerInput.Contains("hello") || lowerInput.Contains("hi")) return $"Hello, {userName}! How can I assist you with cybersecurity today?"; 
                if (lowerInput.Contains("how are you")) return $"I'm functioning at optimal parameters, {userName}! What's your purpose?"; 
                if (lowerInput.Contains("purpose") || lowerInput.Contains("what are you")) return "I am a Cybersecurity Awareness Assistant. My purpose is to help you stay safe online."; 

                // Final fallback if the system genuinely cannot parse the user's intent.
                return $"I'm not sure I understand, {userName}. Can you try rephrasing? Try asking about passwords, phishing, or safe browsing."; 
            }
            catch (Exception)
            {
                // Fallback Error Handling to ensure the bot never completely crashes the application.
                return "System Error: I encountered an unexpected input. Please stick to standard alphanumeric characters.";
            }
        } 

        private string GetRandomResponse(string topicKey) // Declares a private helper method that takes a topic key and extracts a random string from the generic collection.
        { 
            List<string> responses = _responsesDatabase[topicKey]; // Retrieves the specific List of strings mapped to the topic key.
            int lastIndex = _lastUsedIndex[topicKey]; // Retrieves the index integer of the last response given for this specific topic.
            int newIndex; // Declares an empty integer variable to hold the newly generated random number.
            
            do { newIndex = rand.Next(responses.Count); } while (newIndex == lastIndex); // Asks the generator to pick a number and loops if it exactly matches the last used number.

            _lastUsedIndex[topicKey] = newIndex; // Updates the tracker integer in memory to equal the newly chosen number.
            return responses[newIndex]; 
        } 

        // 
        // DELEGATE METHODS
        // 
        
        private string ApplyEmpatheticTone(string response) // Declares a method matching the delegate signature that adds an empathetic prefix to the response.
        {
            return "[Empathy Module Engaged] It is completely normal to feel concerned about that. Let's secure your digital life together. Here is what you need to know:\n\n" + response;
        }

        private string ApplyEnthusiasticTone(string response) // Declares a method matching the delegate signature that adds a positive prefix to the response.
        {
            return "[Enthusiasm Module Engaged] That is the perfect proactive mindset to have! Here is some great information for you:\n\n" + response;
        }

        private string ApplyNeutralTone(string response) // Declares a method matching the delegate signature that returns the exact string unaltered.
        {
            return response; 
        }
    } 
}