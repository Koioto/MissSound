using Amaoto;
using Koioto.Plugin;
using Koioto.Support;
using Koioto.Support.FileReader;
using System.IO;

namespace Koioto.SamplePlugin.MissSound
{
    public class MissSound : KoiotoPlugin
    {
        public override string Name => "MissSound";
        public override string[] Creator => new string[] { "AioiLight" };
        public override string Description => "Play sound when miss.";
        public override string Version => "1.2";

        public override void OnEnable()
        {
            // プラグインのフォルダとプラグイン名を結合してプラグインが使用するフォルダを決定する
            // Combine the plugin's folder with the plugin name to determine which folder the plugin will use
            var dir = Path.Combine(Bridge.PluginsDir, Name);
            // Amaoto.Soundのインスタンスを作成して音を鳴らす準備をする
            // Create an instance of Amaoto.Sound and get ready to play the sound.
            var sound = new Sound(Path.Combine(dir, @"miss-sound.wav"));
            Sound = Bridge.SoundManager.AddManagedSound(new ManagedSound(sound, SoundType.SE)) as Sound;
            base.OnEnable();
        }

        public override void OnDisable()
        {
            // 音の破棄をする。忘れるとメモリリークする可能性があるので必ずすること
            // Dipose the sound.
            // Be sure to do this because there is a possibility of a memory leak if you forget to do it.
            Bridge.SoundManager.RemoveManagedSound(Sound);
            Sound?.Dispose();
            base.OnDisable();
        }

        public override void OnHitNote(Chip chip, int player)
        {
            // チップがヒットしたときの処理
            // What to do when a chip is hit
            if (chip.IsNote && chip.IsHitted)
            {
                // 音符である、かつチップがヒットしたとき
                // When it is a note and the chip is hit
                if (chip.Judge == Judge.Miss || chip.Judge == Judge.Bad)
                {
                    // 判定が見過ごし不可、または叩いて不可のとき、音を鳴らす。
                    // A sound is sounded when the judgement is Miss or Bad.
                    Sound?.Play();
                }
            }
            base.OnHitNote(chip, player);
        }

        private Sound Sound;
    }
}