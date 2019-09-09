using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Speech.Synthesis;
using System.Threading;

namespace PlayHelper
{
    public class VoicePlay
    {
        public static SpeechSynthesizer speech;
        public static List<string> NeedSpeack;
        public static Task PlayTask;
        public VoicePlay()
        {
            speech = new SpeechSynthesizer
            {
                Rate = 0
            };
            speech.SelectVoice("Microsoft Huihui Desktop");
            NeedSpeack = new List<string>();
            PlayTask = new Task(Play);
            PlayTask.Start();
        }
        public static void PlayStr(string str)
        {
            NeedSpeack.Add(str);
        }
        public static void Dispose()
        {
            PlayTask.Dispose();
            speech.Dispose();
            NeedSpeack.Clear();
            GC.Collect();
        }
        public static void Play()
        {
            while (true)
            {
                if(NeedSpeack != null && NeedSpeack.Count > 0)
                {
                    List<string> will = new List<string>(NeedSpeack);
                    NeedSpeack.Clear();
                    foreach(var str in will)
                    {
                        speech.Speak(str);
                        Thread.Sleep(new TimeSpan(0, 0, 2));
                    }
                    will.Clear();
                }
                Thread.Sleep(new TimeSpan(0, 0, 5));
            }
        }
    }
}
