using System;
using System.Collections.Generic; // <-- NEW: Required for Generic Collections (Dictionary, List)

namespace PROG6221_PART2 
{ 
    public class ChatBot 
    {
        private Random rand = new Random(); // Instantiates a random number generator object which is used to pick different responses.

        // ==========================================
        // NEW CODE: GENERIC COLLECTIONS (Rubric Requirement)
        // Replaced basic string[] arrays with a robust Dictionary that maps a topic string to a List of responses.
        // ==========================================
        private Dictionary<string, List<string>> _responsesDatabase;
        private Dictionary<string, int> _lastUsedIndex;

        // ==========================================
        // NEW CODE: MEMORY AND CONTEXT (Rubric Requirement)
        // State variables to remember user details and conversation flow
        // ==========================================
        private string _lastDiscussedTopic = "";
        private string _favoriteTopic = "";

        public ChatBot() // Constructor runs when the bot is first created
        {
            // Initialize the tracking dictionary to remember the last used response for each topic
            _lastUsedIndex = new Dictionary<string, int>
            {
                { "password", -1 },
                { "phishing", -1 },
                { "browsing", -1 }
            };

            // Initialize the Generic Collection Database
            _responsesDatabase = new Dictionary<string, List<string>>
            {
                { "password", new List<string>
                    { 
                        "For safe passwords, always use a mix of uppercase, lowercase, numbers, and symbols.\n  - Avoid common words or personal info.\n  - Recommendation: Consider using a dedicated password manager!", 
                        "A passphrase is often more secure and easier to remember than a complex short password.\n  - Example: A sequence of random words like 'PurpleElephantRunningFast!'\n  - Why: Length mathematically increases the time it takes to crack.", 
                        "Never reuse the same password across multiple websites.\n  - The Risk: If one minor site gets breached, hackers will try that same password on your banking and email accounts.\n  - Action: Generate unique credentials for every platform.", 
                        "Enable Multi-Factor Authentication (MFA) on all your accounts.\n  - How it works: It adds a crucial second layer of security beyond just your password.\n  - Example: Using an Authenticator app or receiving an SMS code.", 
                        "Change your passwords immediately if you hear about a data breach at a company where you have an account.\n  - Pro Tip: You can use services like 'Have I Been Pwned' to check if your data has been compromised.", 
                        "Avoid using easily guessable information in your passwords.\n  - Do NOT use: pet names, birthdates, or '123456'.\n  - Hackers use automated dictionaries that guess these combinations in seconds.", 
                        "Do not write your passwords down on sticky notes attached to your monitor.\n  - Physical security is just as important as digital security.\n  - Use a reputable, encrypted password manager instead.", 
                        "Consider updating critical passwords periodically.\n  - However, focus more on the overall length and complexity as your primary defense rather than just changing a weak password frequently." 
                    }
                },
                { "phishing", new List<string>
                    {
                        "Phishing is a cyber attack where attackers disguise themselves to get sensitive information.\n  - Action: Always verify the actual sender's address, not just their display name.\n  - Rule: Avoid clicking unknown links.", 
                        "Never provide personal or financial information in response to an unsolicited email or popup.\n  - Fact: Legitimate companies, banks, and IT departments will NEVER ask for your password via email.", 
                        "Look out for poor spelling and grammar in emails.\n  - Why: These are common red flags for phishing attempts originating from automated scam networks.\n  - Always read official correspondence carefully.", 
                        "Phishing often creates a false sense of urgency.\n  - Example: 'Your account will be suspended in 24 hours!'\n  - Strategy: Take a breath, do not click the link, and verify directly by logging into the service manually.", 
                        "Hover over links in emails without clicking them to see the actual destination URL.\n  - If the link text says 'paypal.com' but the hover destination shows a random string of letters, it is a trap. Don't click it.", 
                        "Spear phishing is a highly targeted version of phishing.\n  - How it works: The attacker uses your personal details scraped from social media to make the scam seem more convincing.\n  - Be careful what personal data you share publicly.", 
                        "Enable spam filters on your email account to automatically catch and quarantine many standard phishing emails.\n  - Modern email providers use AI to detect malicious patterns before they reach your inbox.", 
                        "If you suspect an email is a phishing attempt, report it immediately.\n  - At work: Forward it to your IT or security department.\n  - At home: Use the 'Report Phishing' button in your email client and delete it." 
                    }
                },
                { "browsing", new List<string>
                    { 
                        "For safe browsing, ensure websites use HTTPS.\n  - The 'S' stands for Secure, meaning your data is encrypted between your browser and the website.\n  - Never download executable files from untrusted sources.", 
                        "Look for the padlock icon in the browser address bar.\n  - Warning: Remember this only means the connection is secure. It does NOT necessarily mean the site itself is legitimate or safe from scams.", 
                        "Avoid using public, unsecured Wi-Fi networks for online banking or entering sensitive information.\n  - Risk: Hackers can easily intercept data on public networks.\n  - Solution: Use a Virtual Private Network (VPN) if you must.", 
                        "Keep your web browser and all its plugins updated to the latest versions.\n  - Why: Developers constantly release updates to patch newly discovered security vulnerabilities.\n  - Turn on auto-updates for maximum safety.", 
                        "Be wary of downloading free software from third-party distribution sites.\n  - They often bundle hidden malware or intrusive adware with the actual software you wanted to download.", 
                        "Use a reliable ad-blocker.\n  - The Threat: Malicious ads, known as 'malvertising', can infect your computer with malware even if you don't intentionally click on them.", 
                        "Clear your browser cookies and cache regularly.\n  - Benefit: This removes cross-site tracking data and helps maintain your online privacy against targeted advertising networks.", 
                        "Pay close attention to domain names.\n  - Scammers often use 'Typosquatting' (like 'g00gle.com' instead of 'google.com').\n  - This tricks you into visiting a visually identical fake site to steal your login credentials." 
                    }
                }
            };
        }

        public string GenerateResponse(string input, string userName) // Declares a method that returns a string for the bot's answer
        { 
            string lowerInput = input.ToLower();

            // ==========================================
            // NEW CODE: MEMORY AND RECALL (Favorite Topic)
            // ==========================================
            if (lowerInput.Contains("favorite") || lowerInput.Contains("favourite") || lowerInput.Contains("interested in"))
            {
                if (lowerInput.Contains("password")) { _favoriteTopic = "password"; return $"Great, {userName}! I'll remember that you are highly interested in password security. Let me know when you want a tip!"; }
                if (lowerInput.Contains("phish") || lowerInput.Contains("email")) { _favoriteTopic = "phishing"; return $"Excellent, {userName}! I've noted that phishing defense is your favorite topic. Ask me for a tip anytime!"; }
                if (lowerInput.Contains("browse") || lowerInput.Contains("safe") || lowerInput.Contains("privacy")) { _favoriteTopic = "browsing"; return $"Noted, {userName}! Safe browsing is a crucial favorite topic to have. What would you like to know?"; }
            }

            // ==========================================
            // NEW CODE: CONVERSATION FLOW (Follow-up Questions)
            // ==========================================
            if (lowerInput.Contains("tell me more") || lowerInput.Contains("another one") || lowerInput.Contains("explain more"))
            {
                // First, check if we were already talking about something
                if (!string.IsNullOrEmpty(_lastDiscussedTopic))
                {
                    return $"Sure thing, {userName}! Here is another tip regarding {_lastDiscussedTopic}:\n\n" + GetRandomResponse(_lastDiscussedTopic);
                }
                // If not, fall back to their favorite topic if they set one
                else if (!string.IsNullOrEmpty(_favoriteTopic))
                {
                    return $"Since you haven't asked a specific question yet, but I know your favorite topic is {_favoriteTopic}, here is a tip on that:\n\n" + GetRandomResponse(_favoriteTopic);
                }
                else
                {
                    return "I'm not sure which topic you'd like more information on. Could you specify if you want to know more about passwords, phishing, or safe browsing?";
                }
            }

            // ==========================================
            // CORE TOPIC RECOGNITION
            // ==========================================
            if (lowerInput.Contains("password")) // Checks if the lowercase user input contains the word "password".
            { 
                _lastDiscussedTopic = "password"; // Save to memory
                return GetRandomResponse("password"); 
            } 

            if (lowerInput.Contains("phishing") || lowerInput.Contains("email") || lowerInput.Contains("scam")) 
            { 
                _lastDiscussedTopic = "phishing"; // Save to memory
                return GetRandomResponse("phishing"); 
            } 

            if (lowerInput.Contains("safe") || lowerInput.Contains("browsing") || lowerInput.Contains("link") || lowerInput.Contains("web") || lowerInput.Contains("privacy")) 
            { 
                _lastDiscussedTopic = "browsing"; // Save to memory
                return GetRandomResponse("browsing"); 
            } 

            // General conversational fallbacks
            if (lowerInput.Contains("hello") || lowerInput.Contains("hi")) 
                return $"Hello, {userName}! How can I assist you with cybersecurity today?"; 

            if (lowerInput.Contains("how are you")) 
                return $"I'm functioning at optimal parameters, {userName}! What's your purpose?"; 

            if (lowerInput.Contains("purpose") || lowerInput.Contains("what are you")) 
                return "I am a Cybersecurity Awareness Assistant. My purpose is to help you stay safe online by answering questions about phishing, password safety, and safe browsing."; 

            if (lowerInput.Contains("ask") || lowerInput.Contains("topics")) 
                return "You can ask me about password safety, phishing, safe browsing, or what to do if you encounter a suspicious link."; 

            // Fallback for unrecognised inputs
            return $"I'm not sure I understand, {userName}. Can you try rephrasing? Try asking about passwords, phishing, or safe browsing."; 
        } 

        // ==========================================
        // NEW CODE: GENERIC COLLECTION HELPER
        // Extracts a random response from the Dictionary based on the key
        // ==========================================
        private string GetRandomResponse(string topicKey) 
        { 
            List<string> responses = _responsesDatabase[topicKey];
            int lastIndex = _lastUsedIndex[topicKey];
            int newIndex; 
            
            do 
            { 
                newIndex = rand.Next(responses.Count); // Asks the generator to pick a number up to the List's size
            } 
            while (newIndex == lastIndex); // Ensures we don't repeat the exact same tip twice in a row

            _lastUsedIndex[topicKey] = newIndex; // Update the tracking dictionary in memory
            return responses[newIndex]; 
        } 
    } 
}