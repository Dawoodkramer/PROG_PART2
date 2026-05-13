/*
 * CODE ATTRIBUTION
 * Author: Dawood Kramer
 * Module: PROG6221
 * Task: Part 2
 * Description: Manages the audio playback functionality for the chatbot application using OOP principles.
 */

using System;
using System.Threading.Tasks;
using Plugin.Maui.Audio;
using Microsoft.Maui.Storage;

namespace PROG6221_PART2
{
    // Declares a public class dedicated to a single responsibility: managing sound playback.
    public class AudioPlayer
    {
        private IAudioPlayer _player; // Declares a private interface variable to hold the audio player instance.

        public async Task PlayGreetingAsync(string fileName = "voiceover-Dawood.wav") // Declares an asynchronous method that accepts a file name, specifically the .wav file.
        {
            try 
            {
                var audioStream = await FileSystem.OpenAppPackageFileAsync(fileName); // Retrieves the audio file from the application's package folder asynchronously.
                _player = AudioManager.Current.CreatePlayer(audioStream); // Instantiates the audio player object using the retrieved audio stream.
                _player.Play(); // Triggers the audio file to start playing through the system speakers.
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Audio Module Error: {ex.Message}"); // Catches any errors during audio playback and writes the error message to the console.
            }
        }
    }
}