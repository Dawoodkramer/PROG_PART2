using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace PROG6221_PART2
{
    // OOP: A utility class dedicated to handling visual element generation and animations
    public class UIHelper
    {
        // FIXED: Changed StackLayout to VerticalStackLayout to perfectly match your XAML
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
            try { await scrollView.ScrollToAsync(container, ScrollToPosition.End, false); } catch {}

            foreach (char c in message)
            {
                messageLabel.Text += c;
                await Task.Delay(speedMs); 
            }
        }

        public static void AddMessageToScreen(VerticalStackLayout container, ScrollView scrollView, string senderName, string message, Color textColor)
        {
            var messageLabel = new Label
            {
                Text = $"{senderName}: {message}",
                TextColor = textColor,
                FontSize = 14,
                FontFamily = "Menlo",
                Margin = new Thickness(0, 5)
            };
            container.Children.Add(messageLabel);
            try { scrollView.ScrollToAsync(container, ScrollToPosition.End, true); } catch {}
        }

        public static void AddDivider(VerticalStackLayout container, ScrollView scrollView)
        {
            var dividerLabel = new Label
            {
                Text = "---------------------------------------------------",
                TextColor = Colors.DarkGray,
                FontSize = 14,
                FontFamily = "Menlo",
                Margin = new Thickness(0, 5)
            };
            container.Children.Add(dividerLabel);
            try { scrollView.ScrollToAsync(container, ScrollToPosition.End, true); } catch {}
        }
    }
}