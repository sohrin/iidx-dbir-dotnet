using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dbir
{
    /// <summary>
    /// 曲マスタ
    /// </summary>
    public class MusicMst
    {
        /// <summary>【PK1】曲名</summary>
        public string Name { get; set; }

        /// <summary>【PK2】プレイスタイル</summary>
        public string PlayStyle { get; set; }

        /// <summary>【PK3】譜面タイプ</summary>
        public string ChartsType { get; set; }

        /// <summary>☆難易度</summary>
        public int Level { get; set; }

        /// <summary>ジャンル</summary>
        public string Genre { get; set; }

        /// <summary>コンポーザー</summary>
        public string Composer { get; set; }

        /// <summary>最小BPM</summary>
        public int MinBpm { get; set; }

        /// <summary>最大BPM</summary>
        public int MaxBpm { get; set; }

        /// <summary>全ノーツ数（TextageのNotesに該当。皿、CNも含む。CNは開始と終了で合計2ノーツとしてカウント）</summary>
        public int AllNotesNum { get; set; }

        /// <summary>スクラッチ数（TextageのSCRに該当。）</summary>
        public int ScratchNum { get; set; }

        /// <summary>チャージノート数（TextageのCNに該当。ノーツ数と異なり、開始・終了のペアの数）</summary>
        public int ChargeNoteNum { get; set; }

        /// <summary>バックスピンスクラッチ数（TextageのBSSに該当。ノーツ数と異なり、開始・終了のペアの数）</summary>
        public int BackSpinScratchNum { get; set; }
    }
}
