using System;
using System.Threading.Tasks;
using Plugin.Maui.Audio;
using Microsoft.Maui.Storage;

namespace PROG6221_PART2
{
    // OOP: This class has a Single Responsibility - managing sound playback.
    public class AudioPlayer
    {
        private IAudioPlayer _player;

        public async Task PlayGreetingAsync(string fileName = "voiceover-Dawood.wav")
        {
            try 
            {
                var audioStream = await FileSystem.OpenAppPackageFileAsync(fileName);
                _player = AudioManager.Current.CreatePlayer(audioStream);
                _player.Play();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Audio Module Error: {ex.Message}");
            }
        }
    }
}