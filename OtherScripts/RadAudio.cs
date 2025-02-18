using Godot;
using System.Collections.Generic;

// need to amke it - idk what im going to do for it though
namespace RadPipe
{
    internal class RadAudio
    {
        Stack<AudioStreamPlayer> Players = new Stack<AudioStreamPlayer>();
        static void PlaySound(AudioStream audio, Vector3 position)
        {
            var player = new AudioStreamPlayer();
            player.Stream = audio;
//            player.po
        }
    }

    public partial class RadAudioStreamPlayer : AudioStreamPlayer
    {
        public void PlaySound(AudioStream sound)
        {
            Stream = sound;
            Play();
        }
    }
}
