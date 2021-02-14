using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dbir.Dto
{
    /// <summary>
    /// 曲リザルト
    /// </summary>
    public class MusicMstResult
    {
        /// <summary>【PK1】曲名</summary>
        public string Name { get; set; }

        /// <summary>【PK2】プレイスタイル</summary>
        public string PlayStyle { get; set; }

        /// <summary>【PK3】譜面タイプ</summary>
        public string ChartsType { get; set; }

        /// <summary>【PK4】プレイモード</summary>
        public string Mode { get; set; }

        /// <summary>クリアランプ</summary>
        public string ClearLamp { get; set; }

        /// <summary>EXスコア</summary>
        public int ExScore { get; set; }

        /// <summary>Bad+Poor</summary>
        public int Bp { get; set; }

        /// <summary>PGREAT</summary>
        public int PGreat { get; set; }

        /// <summary>GREAT</summary>
        public int Great { get; set; }

        /// <summary>GOOD</summary>
        public int Good { get; set; }

        /// <summary>BAD</summary>
        public int Bad { get; set; }

        /// <summary>POOR</summary>
        public int Poor { get; set; }

        /// <summary>Combo Break</summary>
        public int CB { get; set; }

        /// <summary>メモ</summary>
        public string Memo { get; set; }

        /// <summary>リザルト画像ID</summary>
        public string ResultImageId { get; set; }

        /// <summary>登録日時</summary>
        public DateTime CreateDate { get; set; }

        /// <summary>更新日時</summary>
        public DateTime UpdateDate { get; set; }

    }
}
