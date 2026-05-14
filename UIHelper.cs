/*
 * CODE ATTRIBUTION
 * Author: Dawood Kramer
 * Module: PROG6221
 * Task: Part 2
 * Description: A utility class dedicated to handling visual element generation and animations within the GUI.
 */

using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace PROG6221_PART2
{
    // Declares a public utility class dedicated to handling visual element generation and animations.
    public class UIHelper
    {
        public static async Task TypeMessageAsync(VerticalStackLayout container, ScrollView scrollView, string senderName, string message, Color textColor, int speedMs) // Declares a static asynchronous method that takes layout containers, a message, text color, and typing speed.
        {
            var messageLabel = new Label // Instantiates a new Label object to display the text on the screen.
            {
                Text = $"{senderName}: ", // Sets the initial text of the label to the sender's name.
                TextColor = textColor, // Changes the text color to whatever color was passed into the method.
                FontSize = 20,
                FontFamily = "Menlo",
                Margin = new Thickness(0, 5)
            };

            container.Children.Add(messageLabel); // Appends the newly created label to the VerticalStackLayout container.
            try { await scrollView.ScrollToAsync(container, ScrollToPosition.End, false); } catch {} // Safely attempts to scroll the chat view to the bottom before typing begins.

            foreach (char c in message) // Breaks the 'message' string down into individual characters and loops through them.
            {
                messageLabel.Text += c; // Adds the current character to the label's text.
                await Task.Delay(speedMs); // Pauses the application for a few milliseconds before printing the next character to simulate a typing effect.
            }
        }

        public static void AddMessageToScreen(VerticalStackLayout container, ScrollView scrollView, string senderName, string message, Color textColor) // Declares a static method to instantly add a message to the screen without the typing animation.
        {
            var messageLabel = new Label // Instantiates a new Label object to hold the instant message.
            {
                Text = $"{senderName}: {message}",
                TextColor = textColor,
                FontSize = 20,
                FontFamily = "Menlo",
                Margin = new Thickness(0, 5)
            };
            container.Children.Add(messageLabel); // Appends the instant message label to the layout container.
            try { scrollView.ScrollToAsync(container, ScrollToPosition.End, true); } catch {} // Safely scrolls the view to the bottom so the new message is visible.
        }

        public static void AddDivider(VerticalStackLayout container, ScrollView scrollView) // Declares a static method to draw a separation line in the UI.
        {
            var dividerLabel = new Label // Instantiates a new Label object specifically for the visual divider.
            {
                Text = "---------------------------------------------------",
                TextColor = Colors.DarkGray, // Sets the divider color to a muted dark gray.
                FontSize = 14,
                FontFamily = "Menlo",
                Margin = new Thickness(0, 5)
            };
            container.Children.Add(dividerLabel); // Appends the divider label to the layout container.
            try { scrollView.ScrollToAsync(container, ScrollToPosition.End, true); } catch {} // Safely scrolls the view to the bottom after adding the divider.
        }
    }
}