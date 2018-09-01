using System.Data;
using xxdwunity.warehouse;


public class SKILL_Dao
    {
        private static SKILL_Dao _dao = null;
        public static SKILL_Dao Instance
        {
            get
            {
                if (_dao == null)
                    _dao = new SKILL_Dao();
                return _dao;
            }
        }
        private CsvSheet cs;
        public Skill FindById(string SKILL_NUM)
        {
            this.cs = Document.Instance.GetSheet("SKILL");
            if (this.cs == null) return null;
            Skill ss = new Skill();
            DataRow[] drs = this.cs.Data.Select("SKILL_NUM='" + SKILL_NUM + "'");
            if (drs.Length > 0)
            {
                DataRow dr = drs[0];
                ss.SKILL_NUM = int.Parse(dr["SKILL_NUM"].ToString());
                ss.PLAYER_NUM = int.Parse(dr["PLAYER_NUM"].ToString());
                ss.LEVEL = int.Parse(dr["LEVEL"].ToString());
                ss.DIST = int.Parse(dr["DIST"].ToString());
                ss.SCOPE = int.Parse(dr["SCOPE"].ToString());
                ss.CD = double.Parse(dr["CD"].ToString());
                ss.DAMAGE_PCT = double.Parse(dr["DAMAGE_PCT"].ToString());
                ss.HPR_PCT = double.Parse(dr["HPR_PCT"].ToString());
                ss.HPX_PCT = double.Parse(dr["HPX_PCT"].ToString());
                ss.HPX_TIME = int.Parse(dr["HPX_TIME"].ToString());
                ss.DEBUFF_DEL = bool.Parse(dr["DEBUFF_DEF"].ToString());
            }
            return ss;
        }
    }

