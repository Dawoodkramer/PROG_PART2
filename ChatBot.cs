using System;

namespace PROG6221_PART2 
{ 
    internal class ChatBot 
    {
        private Random rand = new Random(); 
        private int lastPasswordIndex = -1; 
        private int lastPhishingIndex = -1; 
        private int lastBrowsingIndex = -1; 

        private string[] passwordResponses = new string[] 
        { 
            "For safe passwords, always use a mix of uppercase, lowercase, numbers, and symbols.\n  - Avoid common words or personal info.\n  - Recommendation: Consider using a dedicated password manager!", 
            "A passphrase is often more secure and easier to remember than a complex short password.\n  - Example: A sequence of random words like 'PurpleElephantRunningFast!'\n  - Why: Length mathematically increases the time it takes to crack.", 
            "Never reuse the same password across multiple websites.\n  - The Risk: If one minor site gets breached, hackers will try that same password on your banking and email accounts.\n  - Action: Generate unique credentials for every platform.", 
            "Enable Multi-Factor Authentication (MFA) on all your accounts.\n  - How it works: It adds a crucial second layer of security beyond just your password.\n  - Example: Using an Authenticator app or receiving an SMS code.", 
            "Change your passwords immediately if you hear about a data breach at a company where you have an account.\n  - Pro Tip: You can use services like 'Have I Been Pwned' to check if your data has been compromised.", 
            "Avoid using easily guessable information in your passwords.\n  - Do NOT use: pet names, birthdates, or '123456'.\n  - Hackers use automated dictionaries that guess these combinations in seconds.", 
            "Do not write your passwords down on sticky notes attached to your monitor.\n  - Physical security is just as important as digital security.\n  - Use a reputable, encrypted password manager instead.", 
            "Consider updating critical passwords periodically.\n  - However, focus more on the overall length and complexity as your primary defense rather than just changing a weak password frequently." 
        }; 

        private string[] phishingResponses = new string[] 
        {
            "Phishing is a cyber attack where attackers disguise themselves to get sensitive information.\n  - Action: Always verify the actual sender's address, not just their display name.\n  - Rule: Avoid clicking unknown links.", 
            "Never provide personal or financial information in response to an unsolicited email or popup.\n  - Fact: Legitimate companies, banks, and IT departments will NEVER ask for your password via email.", 
            "Look out for poor spelling and grammar in emails.\n  - Why: These are common red flags for phishing attempts originating from automated scam networks.\n  - Always read official correspondence carefully.", 
            "Phishing often creates a false sense of urgency.\n  - Example: 'Your account will be suspended in 24 hours!'\n  - Strategy: Take a breath, do not click the link, and verify directly by logging into the service manually.", 
            "Hover over links in emails without clicking them to see the actual destination URL.\n  - If the link text says 'paypal.com' but the hover destination shows a random string of letters, it is a trap. Don't click it.", 
            "Spear phishing is a highly targeted version of phishing.\n  - How it works: The attacker uses your personal details scraped from social media to make the scam seem more convincing.\n  - Be careful what personal data you share publicly.", 
            "Enable spam filters on your email account to automatically catch and quarantine many standard phishing emails.\n  - Modern email providers use AI to detect malicious patterns before they reach your inbox.", 
            "If you suspect an email is a phishing attempt, report it immediately.\n  - At work: Forward it to your IT or security department.\n  - At home: Use the 'Report Phishing' button in your email client and delete it." 
        }; 

        private string[] browsingResponses = new string[] 
        { 
            "For safe browsing, ensure websites use HTTPS.\n  - The 'S' stands for Secure, meaning your data is encrypted between your browser and the website.\n  - Never download executable files from untrusted sources.", 
            "Look for the padlock icon in the browser address bar.\n  - Warning: Remember this only means the connection is secure. It does NOT necessarily mean the site itself is legitimate or safe from scams.", 
            "Avoid using public, unsecured Wi-Fi networks for online banking or entering sensitive information.\n  - Risk: Hackers can easily intercept data on public networks.\n  - Solution: Use a Virtual Private Network (VPN) if you must.", 
            "Keep your web browser and all its plugins updated to the latest versions.\n  - Why: Developers constantly release updates to patch newly discovered security vulnerabilities.\n  - Turn on auto-updates for maximum safety.", 
            "Be wary of downloading free software from third-party distribution sites.\n  - They often bundle hidden malware or intrusive adware with the actual software you wanted to download.", 
            "Use a reliable ad-blocker.\n  - The Threat: Malicious ads, known as 'malvertising', can infect your computer with malware even if you don't intentionally click on them.", 
            "Clear your browser cookies and cache regularly.\n  - Benefit: This removes cross-site tracking data and helps maintain your online privacy against targeted advertising networks.", 
            "Pay close attention to domain names.\n  - Scammers often use 'Typosquatting' (like 'g00gle.com' instead of 'google.com').\n  - This tricks you into visiting a visually identical fake site to steal your login credentials." 
        }; 

        private string GetRandomResponse(string[] responses, ref int lastIndex) 
        { 
            int newIndex; 
            do 
            { 
                newIndex = rand.Next(responses.Length); 
            } 
            while (newIndex == lastIndex); 

            lastIndex = newIndex; 
            return responses[newIndex]; 
        } 

        public string GenerateResponse(string input, string userName) 
        { 
            string lowerInput = input.ToLower();

            if (lowerInput.Contains("password")) 
                return GetRandomResponse(passwordResponses, ref lastPasswordIndex); 

            if (lowerInput.Contains("phishing") || lowerInput.Contains("email")) 
                return GetRandomResponse(phishingResponses, ref lastPhishingIndex); 

            if (lowerInput.Contains("safe") || lowerInput.Contains("browsing") || lowerInput.Contains("link") || lowerInput.Contains("web")) 
                return GetRandomResponse(browsingResponses, ref lastBrowsingIndex); 

            if (lowerInput.Contains("hello") || lowerInput.Contains("hi")) 
                return $"Hello, {userName}! How can I assist you with cybersecurity today?"; 

            if (lowerInput.Contains("how are you")) 
                return $"I'm functioning at optimal parameters, {userName}! What's your purpose?"; 

            if (lowerInput.Contains("purpose") || lowerInput.Contains("what are you")) 
                return "I am a Cybersecurity Awareness Assistant. My purpose is to help you stay safe online by answering questions about phishing, password safety, and safe browsing."; 

            if (lowerInput.Contains("ask") || lowerInput.Contains("topics")) 
                return "You can ask me about password safety, phishing, safe browsing, or what to do if you encounter a suspicious link."; 

            return $"I didn't quite understand that, {userName}. Could you rephrase? Try asking about passwords, phishing, or safe browsing."; 
        } 
    } 
}