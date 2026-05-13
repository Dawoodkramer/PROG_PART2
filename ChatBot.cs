using System;
using System.Collections.Generic;

namespace PROG6221_PART2 
{ 
    // ==========================================
    // NEW CODE: DELEGATE DEFINITION (Rubric Requirement)
    // A Delegate acts as a "variable that holds a method". We use it here to dynamically 
    // change the tone of the bot's response based on the user's emotional state.
    // ==========================================
    public delegate string SentimentModifier(string botResponse);

    public class ChatBot 
    {
        private Random rand = new Random(); 
        private Dictionary<string, List<string>> _responsesDatabase;
        private Dictionary<string, int> _lastUsedIndex;
        
        private string _lastDiscussedTopic = "";
        private string _favoriteTopic = "";

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

        public string GenerateResponse(string input, string userName) 
        { 
            // ==========================================
            // ERROR HANDLING (Rubric Requirement)
            // Wrap everything in a try-catch to prevent crashes on weird inputs
            // ==========================================
            try 
            {
                string lowerInput = input.ToLower();
                string coreResponse = "";
                bool isCoreTopic = false;

                // 1. Memory: Favorite Topic
                if (lowerInput.Contains("favorite") || lowerInput.Contains("favourite") || lowerInput.Contains("interested in"))
                {
                    if (lowerInput.Contains("password")) { _favoriteTopic = "password"; return $"Great, {userName}! I'll remember that you are highly interested in password security. Let me know when you want a tip!"; }
                    if (lowerInput.Contains("phish") || lowerInput.Contains("email")) { _favoriteTopic = "phishing"; return $"Excellent, {userName}! I've noted that phishing defense is your favorite topic. Ask me for a tip anytime!"; }
                    if (lowerInput.Contains("browse") || lowerInput.Contains("safe") || lowerInput.Contains("privacy")) { _favoriteTopic = "browsing"; return $"Noted, {userName}! Safe browsing is a crucial favorite topic to have. What would you like to know?"; }
                }

                // 2. Memory: Follow-up Questions
                if (lowerInput.Contains("tell me more") || lowerInput.Contains("another one") || lowerInput.Contains("explain more"))
                {
                    if (!string.IsNullOrEmpty(_lastDiscussedTopic))
                    {
                        coreResponse = GetRandomResponse(_lastDiscussedTopic);
                        isCoreTopic = true;
                    }
                    else if (!string.IsNullOrEmpty(_favoriteTopic))
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
                if (!isCoreTopic)
                {
                    if (lowerInput.Contains("password")) 
                    { 
                        _lastDiscussedTopic = "password"; 
                        coreResponse = GetRandomResponse("password"); 
                        isCoreTopic = true;
                    } 
                    else if (lowerInput.Contains("phishing") || lowerInput.Contains("email") || lowerInput.Contains("scam")) 
                    { 
                        _lastDiscussedTopic = "phishing"; 
                        coreResponse = GetRandomResponse("phishing"); 
                        isCoreTopic = true;
                    } 
                    else if (lowerInput.Contains("safe") || lowerInput.Contains("browsing") || lowerInput.Contains("link") || lowerInput.Contains("web")) 
                    { 
                        _lastDiscussedTopic = "browsing"; 
                        coreResponse = GetRandomResponse("browsing"); 
                        isCoreTopic = true;
                    } 
                }

                // ==========================================
                // THE SENTIMENT ENGINE (Using Delegates)
                // ==========================================
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

                    // Execute the delegate to modify the response before sending it back
                    return modifier(coreResponse);
                }

                // 4. Conversational Fallbacks
                if (lowerInput.Contains("hello") || lowerInput.Contains("hi")) return $"Hello, {userName}! How can I assist you with cybersecurity today?"; 
                if (lowerInput.Contains("how are you")) return $"I'm functioning at optimal parameters, {userName}! What's your purpose?"; 
                if (lowerInput.Contains("purpose") || lowerInput.Contains("what are you")) return "I am a Cybersecurity Awareness Assistant. My purpose is to help you stay safe online."; 

                return $"I'm not sure I understand, {userName}. Can you try rephrasing? Try asking about passwords, phishing, or safe browsing."; 
            }
            catch (Exception)
            {
                // Fallback Error Handling
                return "System Error: I encountered an unexpected input. Please stick to standard alphanumeric characters.";
            }
        } 

        private string GetRandomResponse(string topicKey) 
        { 
            List<string> responses = _responsesDatabase[topicKey];
            int lastIndex = _lastUsedIndex[topicKey];
            int newIndex; 
            
            do { newIndex = rand.Next(responses.Count); } while (newIndex == lastIndex); 

            _lastUsedIndex[topicKey] = newIndex; 
            return responses[newIndex]; 
        } 

        // ==========================================
        // DELEGATE METHODS
        // These methods match the signature of the SentimentModifier delegate
        // ==========================================
        private string ApplyEmpatheticTone(string response)
        {
            return "[Empathy Module Engaged] It is completely normal to feel concerned about that. Let's secure your digital life together. Here is what you need to know:\n\n" + response;
        }

        private string ApplyEnthusiasticTone(string response)
        {
            return "[Enthusiasm Module Engaged] That is the perfect proactive mindset to have! Here is some great information for you:\n\n" + response;
        }

        private string ApplyNeutralTone(string response)
        {
            return response; // Return exactly as is
        }
    } 
}