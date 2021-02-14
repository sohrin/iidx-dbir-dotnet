using Dapper;
using Jiifureit.Dapper.OutsideSql;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dbir.Dto;
using Dbir.Utils;

namespace Dbir.Repository
{
    public interface IRepository<T>
    {

        T GetOne(string name, string playStyle, string chartsType);

        //IEnumerable<T> GetAll();

        void Insert(T item);

        //void Update(T item);

        //void Delete(T item);

    }

    public class MusicMstRepository : IRepository<MusicMst>
    {
        private string _connectionString;

        internal System.Data.IDbConnection Connection
        {
            get
            {
                return new SqlConnection(_connectionString);
            }
        }

        public MusicMstRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public MusicMst GetOne(string name, string playStyle, string chartsType)
        {
            MusicMst result = null;
            using (IDbConnection db = Connection)
            {
                db.Open();
                try
                {
                    var path = "src\\Dbir\\Repository\\MusicMstRepository_SelectMusicMstByPK.sql";
                    var param = new DynamicParameters();
                    param.Add("Name",
                        name);
                    param.Add("PlayStyle",
                        playStyle);
                    param.Add("ChartsType",
                        chartsType);
                    var resultList = db.QueryOutsideSql<MusicMst>(path, param);
                    if (resultList.Count() == 1)
                    {
                        result = resultList.First();
                    }
                }
                catch (Exception e)
                {
                    ExceptionUtils.PrintStackTrace(e);
                }
            }
            return result;
        }

        public void Insert(MusicMst item)
        {
            using (IDbConnection db = Connection)
            {
                db.Open();
                using (var tran = db.BeginTransaction())
                {
                    try
                    {
                        var path = "src\\Dbir\\Repository\\MusicMstRepository_InsertMusicMst.sql";
                        var param = new DynamicParameters();
                        param.Add("Name",
                            item.Name);
                        param.Add("PlayStyle",
                            item.PlayStyle);
                        param.Add("ChartsType",
                            item.ChartsType);
                        param.Add("Level",
                            item.Level);
                        param.Add("Genre",
                            item.Genre);
                        param.Add("Composer",
                            item.Composer);
                        param.Add("MinBpm",
                            item.MinBpm);
                        param.Add("MaxBpm",
                            item.MaxBpm);
                        param.Add("AllNotesNum",
                            item.AllNotesNum);
                        param.Add("ScratchNum",
                            item.ScratchNum);
                        param.Add("ChargeNoteNum",
                            item.ChargeNoteNum);
                        param.Add("BackSpinScratchNum",
                            item.BackSpinScratchNum);

                        // TODO: エラーになる（現在はSQL側でCURRENT_TIMESTAMP固定にしている）。
                        //param.Add("CreateDate",
                        //    new DateTime());

                        // TODO: https://w.atwiki.jp/ohden/pages/648.html の通りやってもエラーになる（現在はSQL側でnull固定にしている）。
                        //param.Add("UpdateDate",
                        //    (DateTime?) null);
                        db.QueryOutsideSql(path, param, tran);
                        tran.Commit();
                    }
                    catch (Exception e)
                    {
                        ExceptionUtils.PrintStackTrace(e);
                        tran.Rollback();
                    }
                }
            }
        }

    }
}
