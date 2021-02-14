using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dbir.Dto
{
    /// <summary>
    /// 曲マスタ＋曲リザルト＋算出値
    /// </summary>
    public class MusicResult

        /*
         * 曲マスタと曲リザルトのキー情報
         */
    {
        /// <summary>【PK1】曲名</summary>
        public string Name { get; set; }

        /// <summary>【PK2】プレイスタイル</summary>
        public string PlayStyle { get; set; }

        /// <summary>【PK3】譜面タイプ</summary>
        public string ChartsType { get; set; }

        /// <summary>【曲リザルトのPK4】リザルト.プレイモード</summary>
        public string Mode { get; set; }

        /*
         * 曲マスタのカラム
         */

        /// <summary>マスタ.☆難易度</summary>
        public int Level { get; set; }

        /// <summary>マスタ.ジャンル</summary>
        public string Genre { get; set; }

        /// <summary>マスタ.コンポーザー</summary>
        public string Composer { get; set; }

        /// <summary>マスタ.最小BPM</summary>
        public int MinBpm { get; set; }

        /// <summary>マスタ.最大BPM</summary>
        public int MaxBpm { get; set; }

        /// <summary>マスタ.全ノーツ数（TextageのNotesに該当。皿、CNも含む。CNは開始と終了で合計2ノーツとしてカウント）</summary>
        public int AllNotesNum { get; set; }

        /// <summary>マスタ.スクラッチ数（TextageのSCRに該当。）</summary>
        public int ScratchNum { get; set; }

        /// <summary>マスタ.チャージノート数（TextageのCNに該当。ノーツ数と異なり、開始・終了のペアの数）</summary>
        public int ChargeNoteNum { get; set; }

        /// <summary>マスタ.バックスピンスクラッチ数（TextageのBSSに該当。ノーツ数と異なり、開始・終了のペアの数）</summary>
        public int BackSpinScratchNum { get; set; }

        /// <summary>マスタ.登録日時</summary>
        public DateTime MstCreateDate { get; set; }

        /// <summary>マスタ.更新日時</summary>
        public DateTime MstUpdateDate { get; set; }

        /*
         * 曲リザルトのカラム
         */

        /// <summary>リザルト.クリアランプ</summary>
        public string ClearLamp { get; set; }

        /// <summary>リザルト.EXスコア</summary>
        public int ExScore { get; set; }

        /// <summary>リザルト.Bad+Poor</summary>
        public int Bp { get; set; }

        /// <summary>リザルト.PGREAT</summary>
        public int PGreat { get; set; }

        /// <summary>リザルト.GREAT</summary>
        public int Great { get; set; }

        /// <summary>リザルト.GOOD</summary>
        public int Good { get; set; }

        /// <summary>リザルト.BAD</summary>
        public int Bad { get; set; }

        /// <summary>リザルト.POOR</summary>
        public int Poor { get; set; }

        /// <summary>リザルト.Combo Break</summary>
        public int CB { get; set; }

        /// <summary>リザルト.メモ</summary>
        public string Memo { get; set; }

        /// <summary>リザルト.リザルト画像ID</summary>
        public string ResultImageId { get; set; }

        /// <summary>リザルト.登録日時</summary>
        public DateTime ResultCreateDate { get; set; }

        /// <summary>リザルト.更新日時</summary>
        public DateTime ResultUpdateDate { get; set; }

    }
}
