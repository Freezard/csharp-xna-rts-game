using System;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework;
using RTSgame.GameObjects;
using System.Collections.Generic;



namespace RTSgame.Utilities
{
    /// <summary>
    /// The soundplayer plays sounds...
    /// </summary>
    static class SoundPlayer
    {

        //static AudioEngine audioEngine = new AudioEngine("Content/Audio/SoundTestAudio.xgs");
        //static WaveBank waveBank = new WaveBank(audioEngine, "Content/Audio/Wave Bank.xwb");
        //static SoundBank soundBank = new SoundBank(audioEngine, "Content/Audio/Sound Bank.xsb");
        static private AudioEngine audioEngine;
        static private WaveBank waveBank;
        static private SoundBank soundBank;
        static private AudioEmitter emitter = new AudioEmitter();
        static private AudioListener listener = new AudioListener();
        static private List<Camera> cameras = new List<Camera>();

        public static void InitAudio()
        {
            audioEngine = new AudioEngine("Content/Audio/Sounds/RtsGameAudio.xgs");
            waveBank = new WaveBank(audioEngine, "Content/Audio/Sounds/Wave Bank.xwb");
            soundBank = new SoundBank(audioEngine, "Content/Audio/Sounds/Sound Bank.xsb");


        }
        public static void Update()
        {
            audioEngine.Update();
        }
        /// <summary>
        /// Play a music file (usually .mp3)
        /// </summary>
        /// <param name="songName"></param>
        public static void PlaySong(String songName)
        {
            MediaPlayer.Play(AssetBank.GetInstance().GetSong(songName));
            MediaPlayer.Volume = 0.05f;

        }
        /// <summary>
        /// Set if music should be looped
        /// </summary>
        /// <param name="looping"></param>
        public static void SetLoopingSong(Boolean looping)
        {
            MediaPlayer.IsRepeating = looping;
        }
        /// <summary>
        /// Play non-3D cue
        /// </summary>
        /// <param name="cueName"></param>
        public static void PlaySound(String cueName)
        {
            Cue cue = soundBank.GetCue(cueName);
            cue.Play();
        }
        /// <summary>
        /// Add a camera that can "listen" to sounds
        /// </summary>
        /// <param name="cam"></param>
        public static void addCamera(Camera cam)
        {
            cameras.Add(cam);
        }
        /// <summary>
        /// Play 3d-sound at a position
        /// </summary>
        /// <param name="cueName"></param>
        /// <param name="position"></param>
        public static void Play3DSound(String cueName, Vector3 position)
        {

            if (cameras.Count == 0)
            {
                throw new ArgumentException("No cameras set for 3D sound");
            }
            else
            {
                

                foreach (Camera camera in cameras)
                {
                    Cue cue = soundBank.GetCue(cueName);
                    listener.Position = camera.GetPosition();
                    emitter.Position = position;

                    cue.Apply3D(listener, emitter); // apply the 3D transform to the cue
                    cue.Play();
                }


            }
        }
    }
}
