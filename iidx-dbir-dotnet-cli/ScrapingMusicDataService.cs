using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using AngleSharp;
using PuppeteerSharp;
using AngleSharp.Html.Parser;
using AngleSharp.Html.Dom;
using NLog;

namespace dbir
{
    public interface IScrapingMusicDataService
    {
        void Execute();
    }

    class ScrapingMusicDataService : IScrapingMusicDataService
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        private readonly IRepository<MusicMst> _musicMstRepository;

        public ScrapingMusicDataService(IRepository<MusicMst> musicMstRepository)
        {
            _musicMstRepository = musicMstRepository;
        }

        public void Execute()
        {
            Task scrapingMusicDataTask = ScrapingMusicData();
            scrapingMusicDataTask.Wait();
        }

        public async Task ScrapingMusicData()
        {
            logger.Debug("ScrapingMusicDataService.ExecuteAsync() BEGIN.");

            // 対象の曲一覧ページのURL一覧
            var listPageUrlList = new List<string>();
            listPageUrlList.Add("https://textage.cc/score/index.html?s911B000");
            listPageUrlList.Add("https://textage.cc/score/index.html?sA11B000");
            listPageUrlList.Add("https://textage.cc/score/index.html?sB11B000");
            listPageUrlList.Add("https://textage.cc/score/index.html?sC11B000");

            // スクレイピング処理する際の1曲毎の待ち時間ミリ秒
            const int SCRAPING_SLEEP_TIME_MS = 10000;

            try
            {
                foreach (var listPageUrl in listPageUrlList)
                {
                    // 対象の曲一覧ページから譜面ページURL一覧を取得
                    var chartsPageUrlList = await getChartsPageUrlList(listPageUrl);

                    foreach (var chartsPageUrl in chartsPageUrlList)
                    {
                        // 譜面ページURL一覧を取得
                        System.Threading.Thread.Sleep(SCRAPING_SLEEP_TIME_MS);
                        await getChartsPageData(chartsPageUrl);
                    }
                }
            }
            catch (Exception e)
            {
                ExceptionUtils.PrintStackTrace(e);
            }

            logger.Debug("ScrapingMusicDataService.ExecuteAsync() END.");
        }

        public async Task<string> doPuppeteer(string url)
        {
            await new BrowserFetcher().DownloadAsync(BrowserFetcher.DefaultRevision);
            var browser = await Puppeteer.LaunchAsync(new LaunchOptions
            {
                Headless = true
            });
            var page = await browser.NewPageAsync();
            await page.GoToAsync(url);
            string html = await page.GetContentAsync();

            page.Dispose();
            browser.Dispose();
            return html;
        }

        public async Task<List<string>> getChartsPageUrlList(string listPageUrl)
        {
            // 曲一覧ページ取得
            var listPageHtml = await doPuppeteer(listPageUrl);

            // パース
            var parser = new HtmlParser();
            var doc = await parser.ParseDocumentAsync(listPageHtml);

            // 曲一覧を取得
            var trNodes = doc.QuerySelectorAll("table:nth-of-type(2) tr");

            var chartsPageUrlList = new List<string>();
            var firstFlg = true;
            foreach (var trNode in trNodes)
            {
//                logger.Debug("■■■" + trNode.InnerHtml.Trim());
                // ヘッダ行判定
                if (firstFlg)
                {
                    firstFlg = false;
                    logger.Debug("【ヘッダ行】");
                    continue;
                }

                var tdNodes = trNode.QuerySelectorAll("td");

                // 削除フラグ判定
                var tdClassAttrVal = tdNodes[1].GetAttribute("class");
                var delFlg = false;
                if (tdClassAttrVal.StartsWith("x") )
                {
                    delFlg = true;

                    // td要素のclass属性値から1文字目を削除
                    tdClassAttrVal = tdClassAttrVal.Substring(1);

                    // TODO: 削除曲は現時点では対象外
                    logger.Debug("【削除曲（もしくは未収録黒譜面）】");
                    continue;
                }
                // logger.Debug("delFlg:[" + delFlg + "]");

                // 曲名取得（<b>で囲った後、スペースを削っているものがある。スペース復元しつつ、スペースの重複・不要パターンは置換で対応）
                var name = tdNodes[3].InnerHtml.Trim();
                name = name.Replace("<b><font color=\"#a000a0\">", "");
                name = name.Replace("<b><font color=\"#00a060\">", "");
                name = name.Replace("<b><font color=\"#ff0000\">", "");

                name = name.Replace("</font></b>", "");

                name = name.Replace("<b>ALFARSHEAR</b>", "ALFARSHEAR");
                name = name.Replace("&amp;", "&");  // 「Angels &amp; Demons」対応
                name = name.Replace("<b>Beautiful Harmony</b>", "Beautiful Harmony");
                name = name.Replace("<b>Bloody Tears</b>", "Bloody Tears");
                name = name.Replace("<b>BLUE DRAGON</b>", "BLUE DRAGON");
                name = name.Replace("Y&Co.Eurobeat", "Y&Co. Eurobeat");
                name = name.Replace("<b>fallen leaves</b>", "fallen leaves");
                name = name.Replace("<b></b>", "");
                name = name.Replace("<b>Fly Above</b>", "Fly Above");
                name = name.Replace("<b>GO OVER WITH GLARE</b>", "GO OVER WITH GLARE");
                name = name.Replace(" <br>", "");
                name = name.Replace("<b>Heavenly Sun</b>", "Heavenly Sun");
                name = name.Replace("<b>HYPER EUROBEAT</b>", "HYPER EUROBEAT");
                name = name.Replace("<b>Keep it</b>", "Keep it");
                name = name.Replace("<b>LOVE", "LOVE ");
                name = name.Replace("SHINE</b>", " SHINE");
                name = name.Replace("<b>Mr.T.</b>", "Mr.T.");
                name = name.Replace("<b>Raison d'etre</b>", "Raison d'être");
                name = name.Replace("<b>RIDE ON THE LIGHT</b>", "RIDE ON THE LIGHT");
                name = name.Replace("<b>Scripted Connection⇒</b>", "Scripted Connection⇒");
                name = name.Replace("<b>Ska-sh All Neurons!!</b>", "Ska-sh All Neurons!!");
                name = name.Replace("<b>The Sealer</b>", "The Sealer");
                name = name.Replace("<b>The Wind of China Express</b>", "The Wind of China Express");
                name = name.Replace("<b>tripping contact</b><span style=\"font-size:9pt\">(teranoid&MC Natsack Remix)</span>", "tripping contact(teranoid&MC Natsack Remix)");
                name = name.Replace("<b>     Warrior</b>", "Warrior");
                name = name.Replace("<b>50th Memorial Songs</b>", "50th Memorial Songs");
                name = name.Replace("50th Memorial Songs<br>", "50th Memorial Songs ");
                name = name.Replace("<b>少女アリスと箱庭幻想コンチェルト</b>", "少女アリスと箱庭幻想コンチェルト");
                name = name.Replace("<b>お米の美味しい炊き方、<br>そしてお米を食べることによるその効果。</b>", "お米の美味しい炊き方、そしてお米を食べることによるその効果。");
                name = name.Replace("<b>     Warrior</b>", "Warrior");
                name = name.Replace("<b>龍王の霊廟</b>", "龍王の霊廟");
                name = name.Replace("<b>夏色DIARY</b>", "夏色DIARY");
                name = name.Replace("<b>草原の王女</b>", "草原の王女");
                name = name.Replace("<b>旋律のドグマ</b>", "旋律のドグマ");
                name = name.Replace("<b>神謳</b>", "神謳");
                name = name.Replace("<b>水上の提督</b>", "水上の提督");
                name = name.Replace("<b>真 地獄超特急 -HELL or HELL-</b>", "真 地獄超特急 -HELL or HELL-");
                name = name.Replace("<b>廿</b>", "廿");
                name = name.Replace("<font color=\"#00a060\">", "");
                name = name.Replace("<font color=\"#c040ff\">", "");
                name = name.Replace("<font color=\"#808000\">", "");
                name = name.Replace("<font color=\"#c040c0\">", "");
                name = name.Replace("<font color=\"#ff8080\">", "");
                name = name.Replace("</font>", "");
                name = name.Replace("<div class=\"ltmodel\">", "");
                name = name.Replace("</div>", "");
                name = name.Replace("<b>", "");
                name = name.Replace("</b>", " ");
                // TODO: fontタグの正規表現置換
                //                logger.Debug("name:[" + name + "]");

                // プレイスタイル判定
                var listPageUrlArr = listPageUrl.Split("?");
                var playStyleFlg = listPageUrlArr[1].Substring(0, 1);
                string playStyle = null;
                if (playStyleFlg == "s")
                {
                    playStyle = "SP";
                }
                else if (playStyleFlg == "d")
                {
                    playStyle = "DP";
                }
//                logger.Debug("playStyle:[" + playStyle + "]");

                // 譜面タイプ判定
                var chartsTypeFlg = tdClassAttrVal.Substring(1, 1);
                string chartsType= null;
                if (chartsTypeFlg == "b")
                {
                    chartsType = "BEGINER";
                }
                else if (chartsTypeFlg == "n")
                {
                    chartsType = "NORMAL";
                }
                else if (chartsTypeFlg == "h")
                {
                    chartsType = "HYPER";
                }
                else if (chartsTypeFlg == "a")
                {
                    chartsType = "ANOTHER";
                }
                else if (chartsTypeFlg == "x")
                {
                    chartsType = "LEGGENDARIA";
                }
//                logger.Debug("chartsType:[" + chartsType + "]");

                // 取得済判定
                if (this._musicMstRepository.GetOne(name, playStyle, chartsType) != null)
                {
                    logger.Debug("【取得済（マスタレコード作成済）】");
                    continue;
                }

                // 譜面ページ無し判定
                var tdNodeSecondAnchors = tdNodes[1].QuerySelectorAll("a");
                if (tdNodeSecondAnchors.Length == 0)
                {
                    logger.Debug("【譜面ページ無し（新曲で名前がわかり譜面作成前）】");
                    continue;
                }

                logger.Debug("■■■" + trNode.InnerHtml.Trim());
                logger.Debug("name:[" + name + "]");
                logger.Debug("playStyle:[" + playStyle + "]");
                logger.Debug("chartsType:[" + chartsType + "]");

                // 譜面ページURL取得
                string chartsPageUrl = tdNodeSecondAnchors[1].GetAttribute("href");
                logger.Debug("chartsPageUrl:[" + chartsPageUrl + "]");
                chartsPageUrlList.Add("https://textage.cc/score/" + chartsPageUrl);
            }

            doc.Dispose();

            return chartsPageUrlList;
        }

        public async Task getChartsPageData(string chartsPageUrl)
        {
            logger.Debug("★★★chartsPageUrl:[" + chartsPageUrl + "]");

            // 曲一覧ページ取得
            var chartsPageHtml = await doPuppeteer(chartsPageUrl);

            // パース
            var parser = new HtmlParser();
            var doc = await parser.ParseDocumentAsync(chartsPageHtml);

            // マスタレコード生成
            var musicMst = new MusicMst();

            // 譜面ページのヘッダを取得
            var headerNode = doc.QuerySelector("nobr");
            var headerNodeText = headerNode.TextContent.Trim();
            logger.Debug(headerNodeText);

            // V2等、曲名に改行が入る場合のため改行を空文字に置換しておく
            // 「V2/ TAKA」→「V2 / TAKA」対応
            headerNodeText = headerNodeText.Replace("\n", "");
            headerNodeText = headerNodeText.Replace("V2/", "V2 /");
            // LEVEL ONEのジャンル「Drumstep / DnB」対応
            headerNodeText = headerNodeText.Replace("Drumstep / DnB", "Drumstep/DnB");

            // ReplaceFirstしたいため、VBのReplaceを実行
            headerNodeText = Microsoft.VisualBasic.Strings.Replace(headerNodeText, "\"", "", 1, 1, Microsoft.VisualBasic.CompareMethod.Binary);
            headerNodeText = Microsoft.VisualBasic.Strings.Replace(headerNodeText, "\"[", ":::::", 1, 1, Microsoft.VisualBasic.CompareMethod.Binary);
            headerNodeText = Microsoft.VisualBasic.Strings.Replace(headerNodeText, "] ", ":::::", 1, 1, Microsoft.VisualBasic.CompareMethod.Binary);
            headerNodeText = Microsoft.VisualBasic.Strings.Replace(headerNodeText, " / ", ":::::", 1, 1, Microsoft.VisualBasic.CompareMethod.Binary);
            headerNodeText = Microsoft.VisualBasic.Strings.Replace(headerNodeText, " bpm:", ":::::", 1, 1, Microsoft.VisualBasic.CompareMethod.Binary);
            headerNodeText = Microsoft.VisualBasic.Strings.Replace(headerNodeText, " - ★", ":::::", 1, 1, Microsoft.VisualBasic.CompareMethod.Binary);
            headerNodeText = Microsoft.VisualBasic.Strings.Replace(headerNodeText, " Notes:", ":::::", 1, 1, Microsoft.VisualBasic.CompareMethod.Binary);
            var headerNodeTextArr = headerNodeText.Split(":::::");
            logger.Debug(headerNodeText);

            musicMst.Name = headerNodeTextArr[2];

            var playStyleChartsTypeArr = headerNodeTextArr[1].Split(" ");
            musicMst.PlayStyle = playStyleChartsTypeArr[0];
            musicMst.ChartsType = playStyleChartsTypeArr[1];

            if (headerNodeTextArr[5] == "？")
            {
                logger.Warn("【☆難易度が「？」】");
                return;
            }
            else if (headerNodeTextArr[5] == "１")
            {
                headerNodeTextArr[5] = "1";
            }
            else if (headerNodeTextArr[5] == "２")
            {
                headerNodeTextArr[5] = "2";
            }
            else if (headerNodeTextArr[5] == "３")
            {
                headerNodeTextArr[5] = "3";
            }
            else if (headerNodeTextArr[5] == "４")
            {
                headerNodeTextArr[5] = "4";
            }
            else if (headerNodeTextArr[5] == "５")
            {
                headerNodeTextArr[5] = "5";
            }
            else if (headerNodeTextArr[5] == "６")
            {
                headerNodeTextArr[5] = "6";
            }
            else if (headerNodeTextArr[5] == "７")
            {
                headerNodeTextArr[5] = "7";
            }
            else if (headerNodeTextArr[5] == "８")
            {
                headerNodeTextArr[5] = "8";
            }
            else if (headerNodeTextArr[5] == "９")
            {
                headerNodeTextArr[5] = "9";
            }
            musicMst.Level = Int32.Parse(headerNodeTextArr[5]);

            // LEVEL ONEのジャンル「Drumstep / DnB」対応
            headerNodeTextArr[0] = headerNodeTextArr[0].Replace("Drumstep/DnB", "Drumstep / DnB");
            musicMst.Genre = headerNodeTextArr[0];

            musicMst.Composer = headerNodeTextArr[3];

            var bpmArr = headerNodeTextArr[4].Split("～");
            if (bpmArr.Length == 1)
            {
                musicMst.MinBpm = Int32.Parse(bpmArr[0]);
                musicMst.MaxBpm = Int32.Parse(bpmArr[0]);
            }
            else
            {
                musicMst.MinBpm = Int32.Parse(bpmArr[0]);
                musicMst.MaxBpm = Int32.Parse(bpmArr[1]);
            }

            musicMst.AllNotesNum = Int32.Parse(headerNodeTextArr[6]);

            // 譜面ページのフッタを取得
            var footerNode = doc.QuerySelector("table + font");
            var footerNodeText = footerNode.TextContent.Trim();
            logger.Debug(footerNodeText);
            footerNodeText = footerNodeText.Replace(", ", ",");
            footerNodeText = footerNodeText.Replace(")", "");
            var footerNodeTextArr = footerNodeText.Split(" (");
            var specialNotesTexts = footerNodeTextArr[1];

            // MEMO: 皿無しでも必ずSCR=0の記載がある。また、DPだとこの時インデックス2に左右別ノーツ数の情報がある。＜例＞天空DPA：AAA:4020, AA:3518, A:3015 (SCR=0) (1126 / 1135)
            musicMst.ScratchNum = 0;
            musicMst.ChargeNoteNum = 0;
            musicMst.BackSpinScratchNum = 0;

            var specialNotesTextArr = specialNotesTexts.Split(" ");
            foreach (var specialNotesText in specialNotesTextArr)
            {
                var specialNoteArr = specialNotesText.Split("=");
                if (specialNoteArr[0] == "SCR")
                {
                    musicMst.ScratchNum = Int32.Parse(specialNoteArr[1]);
                }
                else if (specialNoteArr[0] == "CN")
                {
                    musicMst.ChargeNoteNum = Int32.Parse(specialNoteArr[1]);
                }
                else if (specialNoteArr[0] == "BSS")
                {
                    musicMst.BackSpinScratchNum = Int32.Parse(specialNoteArr[1]);
                }
            }

            try
            {
                this._musicMstRepository.Insert(musicMst);
            }
            catch (Exception e)
            {
                logger.Debug("musicMstRepository.Insert() Error Occured.");
            }


            doc.Dispose();
        }

    }
}
