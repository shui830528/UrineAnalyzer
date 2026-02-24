using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Tool;


namespace DataModel
{
    /// <summary>
    /// 样本信息
    /// </summary>
    public class DBSampleInfo : INotifyPropertyChanged
    {
        private int sampleID;
        /// <summary>
        /// 样本号
        /// </summary>
        public int SampleID
        {
            get { return sampleID; }
            set { sampleID = value; OnPropertyChanged("SampleID"); }
        }

        private DateTime registDate;
        /// <summary>
        /// 生成日期
        /// </summary>
        public DateTime RegistDate
        {
            get { return registDate; }
            set { registDate = value; OnPropertyChanged("RegistDate"); }
        }
        private DateTime importDate;
        /// <summary>
        /// 导入日期
        /// </summary>
        public DateTime ImportDate
        {
            get { return importDate; }
            set { importDate = value; OnPropertyChanged("ImportDate"); }
        }

        private string imageFile = string.Empty;
        public string ImageFile { get { return imageFile; } set { imageFile = value; OnPropertyChanged("ImageFile"); } }


        private string barcode;
        /// <summary>
        /// 条码
        /// </summary>
        public string BarCode
        {
            get { return barcode; }
            set { barcode = value; OnPropertyChanged("BarCode"); }
        }

        private DateTime checkDate;
        /// <summary>
        /// 检验日期
        /// </summary>
        public DateTime CheckDate
        {
            get { return checkDate; }
            set { checkDate = value; OnPropertyChanged("CheckDate"); }
        }

        private string name;
        /// <summary>
        /// 患者姓名
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; OnPropertyChanged("Name"); }
        }

        private int age;
        /// <summary>
        /// 年龄
        /// </summary>
        public int Age
        {
            get { return age; }
            set { age = value; OnPropertyChanged("Age"); }
        }

        private string ageUnit;
        /// <summary>
        /// 年龄单位
        /// </summary>
        public string AgeUnit
        {
            get { return ageUnit; }
            set { ageUnit = value; OnPropertyChanged("AgeUnit"); }
        }

        private string sex;
        /// <summary>
        /// 性别
        /// </summary>
        public string Sex
        {
            get { return sex; }
            set { sex = value; OnPropertyChanged("Sex"); }
        }

        private bool isEmergency;
        /// <summary>
        /// 急诊
        /// </summary>
        public bool IsEmergency
        {
            get { return isEmergency; }
            set { isEmergency = value; OnPropertyChanged("IsEmergency"); }
        }
        private bool isTest;
        /// <summary>
        /// 检验
        /// </summary>
        public bool IsTest
        {
            get { return isTest; }
            set { isTest = value; OnPropertyChanged("IsTest"); }
        }
        private string caseNo;
        /// <summary>
        /// 病历号
        /// </summary>
        public string CaseNo
        {
            get { return caseNo; }
            set { caseNo = value; OnPropertyChanged("CaseNo"); }
        }

        private string bedNo;
        /// <summary>
        /// 床号
        /// </summary>
        public string BedNo
        {
            get { return bedNo; }
            set { bedNo = value; OnPropertyChanged("BedNo"); }
        }

        private string patientType;
        /// <summary>
        /// 患者类型
        /// </summary>
        public string PatientType
        {
            get { return patientType; }
            set { patientType = value; OnPropertyChanged("PatientType"); }
        }

        private string sendCheckDoctor;
        /// <summary>
        /// 送检医生
        /// </summary>
        public string SendCheckDoctor
        {
            get { return sendCheckDoctor; }
            set { sendCheckDoctor = value; OnPropertyChanged("SendCheckDoctor"); }
        }
        private string sendCheckOffice;
        /// <summary>
        /// 送检科室
        /// </summary>
        public string SendCheckOffice
        {
            get { return sendCheckOffice; }
            set { sendCheckOffice = value; OnPropertyChanged("SendCheckOffice"); }
        }
        private DateTime sendCheckDate;
        /// <summary>
        /// 送检日期
        /// </summary>
        public DateTime SendCheckDate
        {
            get { return sendCheckDate; }
            set { sendCheckDate = value; OnPropertyChanged("SendCheckDate"); }
        }

        private DateTime samplingDate;
        /// <summary>
        /// 采样日期
        /// </summary>
        public DateTime SamplingDate
        {
            get { return samplingDate; }
            set { samplingDate = value; OnPropertyChanged("SamplingDate"); }
        }

        private string sampleType;
        /// <summary>
        /// 样本类型
        /// </summary>
        public string SampleType
        {
            get { return sampleType; }
            set { sampleType = value; OnPropertyChanged("SampleType"); }
        }
        private string checkDoctor;
        /// <summary>
        /// 检验人
        /// </summary>
        public string CheckDoctor
        {
            get { return checkDoctor; }
            set { checkDoctor = value; OnPropertyChanged("CheckDoctor"); }
        }
        private string auditor;
        /// <summary>
        /// 审核者
        /// </summary>
        public string Auditor
        {
            get { return auditor; }
            set { auditor = value; OnPropertyChanged("Auditor"); }
        }
        private string clinical;
        /// <summary>
        /// 监床诊断
        /// </summary>
        public string Clinical
        {
            get { return clinical; }
            set { clinical = value; OnPropertyChanged("Clinical"); }
        }

        private string remarks;
        /// <summary>
        /// 备注信息
        /// </summary>
        public string Remarks
        {
            get { return remarks; }
            set { remarks = value; OnPropertyChanged("Remarks"); }
        }
        private bool isPrint;
        /// <summary>
        /// 是否打印
        /// </summary>
        public bool IsPrint
        {
            get { return isPrint; }
            set { isPrint = value; OnPropertyChanged("IsPrint"); }
        }
        private bool isAuditor;
        /// <summary>
        /// 是否审核
        /// </summary>
        public bool IsAuditor
        {
            get { return isAuditor; }
            set { isAuditor = value; OnPropertyChanged("IsAuditor"); }
        }
        private bool isLis;
        /// <summary>
        /// 是否发送LIS
        /// </summary>
        public bool IsLis
        {
            get { return isLis; }
            set { isLis = value; OnPropertyChanged("IsLis"); }
        }

        public void CopyToTarget(DBSampleInfo Target)
        {
            Target.RegistDate = RegistDate;
            Target.CheckDate = CheckDate;
            Target.SendCheckDate = SendCheckDate;
            Target.samplingDate = SamplingDate;
            Target.SampleID = SampleID;
            Target.BarCode = BarCode;
            Target.Name = Name;
            Target.Age = Age;
            Target.AgeUnit = AgeUnit;
            Target.Sex = Sex;
            Target.IsEmergency = IsEmergency;
            Target.CaseNo = CaseNo;
            Target.BedNo = BedNo;
            Target.PatientType = PatientType;
            Target.SendCheckDoctor = SendCheckDoctor;
            Target.SendCheckOffice = SendCheckOffice;
            Target.SampleType = SampleType;
            Target.CheckDoctor = CheckDoctor;
            Target.Auditor = Auditor;
            Target.Clinical = Clinical;
            Target.Remarks = Remarks;
            Target.IsAuditor = IsAuditor;
            Target.IsPrint = IsPrint;
            Target.ImageFile = ImageFile;
        }

        public DBSampleInfo()
        {
            RegistDate = DateTime.Now;
            CheckDate = RegistDate;
            SendCheckDate = RegistDate;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <summary>
    /// 样本信息数据模型
    /// </summary>
    public class SampleInfoModel
    {
        private DB db = new DB();


        /// <summary>
        /// 赋值
        /// </summary>
        /// <param name="info"></param>
        /// <param name="objReader"></param>
        private void CopySampleInfo(DBSampleInfo info, IDataReader objReader)
        {
            info.SampleID = objReader["SampleID"].Equals(DBNull.Value) ? 0 : Convert.ToInt32(objReader["SampleID"]);
            info.RegistDate = objReader["RegistDate"].Equals(DBNull.Value) ? DateTime.Now : Convert.ToDateTime(objReader["RegistDate"]);
            info.ImportDate = objReader["ImportDate"].Equals(DBNull.Value) ? DateTime.Now : Convert.ToDateTime(objReader["ImportDate"]);
            info.BarCode = objReader["BarCode"].Equals(DBNull.Value) ? "" : (objReader["BarCode"].ToString());
            info.CheckDate = objReader["CheckDate"].Equals(DBNull.Value) ? DateTime.Now : Convert.ToDateTime(objReader["CheckDate"]);
            info.Name = objReader["PatientName"].Equals(DBNull.Value) ? "" : (objReader["PatientName"].ToString());
            info.Age = objReader["PatientAge"].Equals(DBNull.Value) ? 0 : Convert.ToInt32(objReader["PatientAge"]);
            info.AgeUnit = objReader["PatientAgeUnit"].Equals(DBNull.Value) ? "" : (objReader["PatientAgeUnit"].ToString());
            info.Sex = objReader["PatientSex"].Equals(DBNull.Value) ? "" : (objReader["PatientSex"].ToString());
            info.IsEmergency = objReader["IsEmergency"].Equals(DBNull.Value) ? false : Convert.ToBoolean(objReader["IsEmergency"]);
            info.IsTest = objReader["IsTest"].Equals(DBNull.Value) ? false : Convert.ToBoolean(objReader["IsTest"]);
            info.CaseNo = objReader["CaseNo"].Equals(DBNull.Value) ? "" : (objReader["CaseNo"].ToString());
            info.BedNo = objReader["BedNo"].Equals(DBNull.Value) ? "" : (objReader["BedNo"].ToString());
            info.PatientType = objReader["PatientType"].Equals(DBNull.Value) ? "" : (objReader["PatientType"].ToString());
            info.SendCheckDoctor = objReader["SendCheckDoctor"].Equals(DBNull.Value) ? "" : (objReader["SendCheckDoctor"].ToString());
            info.SendCheckOffice = objReader["SendCheckOffice"].Equals(DBNull.Value) ? "" : (objReader["SendCheckOffice"].ToString());
            info.SendCheckDate = objReader["SendCheckDate"].Equals(DBNull.Value) ? DateTime.Now : Convert.ToDateTime(objReader["SendCheckDate"]);
            info.SampleType = objReader["SampleType"].Equals(DBNull.Value) ? "" : (objReader["SampleType"].ToString());
            info.CheckDoctor = objReader["CheckDoctor"].Equals(DBNull.Value) ? "" : (objReader["CheckDoctor"].ToString());
            info.Auditor = objReader["Auditor"].Equals(DBNull.Value) ? "" : (objReader["Auditor"].ToString());
            info.Clinical = objReader["Clinical"].Equals(DBNull.Value) ? "" : (objReader["Clinical"].ToString());
            info.Remarks = objReader["Remarks"].Equals(DBNull.Value) ? "" : (objReader["Remarks"].ToString());
            info.IsPrint = objReader["IsPrint"].Equals(DBNull.Value) ? false : Convert.ToBoolean(objReader["IsPrint"]);
            info.IsAuditor = objReader["IsAuditor"].Equals(DBNull.Value) ? false : Convert.ToBoolean(objReader["IsAuditor"]);
            info.IsLis = objReader["IsLis"].Equals(DBNull.Value) ? false : Convert.ToBoolean(objReader["IsLis"]);
            info.ImageFile = objReader["ImageFile"].Equals(DBNull.Value) ? "" : (objReader["ImageFile"].ToString());
        }
        /// <summary>
        /// 样本查询
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="list"></param>
        protected void Query(string sql, IList<DBSampleInfo> list)
        {
            try
            {
                list.Clear();
                IDataReader objReader = null;

                db.Query(sql, out objReader);

                while (objReader != null && objReader.Read())
                {
                    DBSampleInfo info = new DBSampleInfo();

                    CopySampleInfo(info, objReader);

                    list.Add(info);
                }
            }
            catch (Exception ex)
            {
                Log.WriteLog(ex.Message + " SampleInfoModel.Query Error " + sql);
            }
        }
        /// <summary>
        /// 查询当天
        /// </summary>
        /// <param name="list"></param>
        public void Query(IList<DBSampleInfo> list)
        {

            string sql = string.Format("SELECT * FROM Tab_SampleInfo Where RegistDate >= #{0} 00:00:00# and RegistDate <= #{0} 23:59:59# Order by SampleID",
              DateTime.Now.ToString(Const.DateFormat),
              DateTime.Now.ToString(Const.DateFormat)
              );

            Query(sql, list);

        }
        /// <summary>
        /// 按日期查询样本
        /// </summary>
        /// <param name="StartDate"></param>
        /// <param name="EndDate"></param>
        /// <param name="list"></param>
        public void Query(DateTime StartDate, DateTime EndDate, IList<DBSampleInfo> list)
        {
            string sql = string.Format("SELECT * FROM Tab_SampleInfo Where RegistDate >= #{0} 00:00:00# and RegistDate <= #{0} 23:59:59# Order by SampleID",
                StartDate.ToString(Const.DateFormat),
                EndDate.ToString(Const.DateFormat)
                );

            Query(sql, list);

        }

        public void Query(DateTime? StartDate, DateTime? EndDate, string otherCondition, IList<DBSampleInfo> list)
        {
            string strCondition = "";
            if (StartDate != null && EndDate != null)
            {
                if (otherCondition != string.Empty)
                {
                    strCondition = string.Format("(RegistDate >= #{0} 00:00:00# and RegistDate <= #{1} 23:59:59#) and (" + otherCondition + ")", ((DateTime)StartDate).ToString(Const.DateFormat), ((DateTime)EndDate).ToString(Const.DateFormat));
                }
                else
                {
                    strCondition = string.Format("(RegistDate >= #{0} 00:00:00# and RegistDate <= #{1} 23:59:59#)", ((DateTime)StartDate).ToString(Const.DateFormat), ((DateTime)EndDate).ToString(Const.DateFormat));
                }
            }
            else
            {
                strCondition = otherCondition;
            }
            string sql = "SELECT * FROM Tab_SampleInfo Where " + strCondition + " Order by SampleID";

            Query(sql, list);

        }
        /// <summary>
        /// 样本是否存在
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public bool IsExists(DBSampleInfo info)
        {
            string sql = string.Format("Select * From Tab_SampleInfo Where SampleID = {0} and RegistDate >= #{1} 00:00:00# and RegistDate <= #{2} 23:59:59#",
              info.SampleID,
              info.RegistDate.ToString(Const.DateFormat),
              info.RegistDate.ToString(Const.DateFormat)
              );

            try
            {
                IDataReader objReader = null;
                db.Query(sql, out objReader);
                while (objReader != null && objReader.Read())
                {
                    return true;
                }

            }
            catch (Exception ex)
            {
                Log.WriteLog(ex.Message + " SampleInfoModel.IsExists " + sql);
            }
            return false;
        }
        /// <summary>
        /// 样本更新
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public int Update(DBSampleInfo info)
        {
            int nCount = 0;
            string sqlUpdate = "Update Tab_SampleInfo Set " +
                  " BarCode =  '" + info.BarCode + "'," +
                  " CheckDate =  '" + info.CheckDate.ToString(Const.DateTimeFormat) + "'," +
                  " PatientName =  '" + info.Name + "'," +
                  " PatientAge =  " + info.Age + "," +
                  " PatientAgeUnit =  '" + info.AgeUnit + "'," +
                  " PatientSex =  '" + info.Sex + "'," +
                  " IsEmergency =  " + (info.IsEmergency ? "1" : "0") + "," +
                  " IsTest =  " + (info.IsTest ? "1" : "0") + "," +
                  " CaseNo =  '" + info.CaseNo + "'," +
                  " BedNo =  '" + info.BedNo + "'," +
                  " PatientType =  '" + info.PatientType + "'," +
                  " SendCheckDoctor =  '" + info.SendCheckDoctor + "'," +
                  " SendCheckOffice =  '" + info.SendCheckOffice + "'," +
                  " SendCheckDate =  '" + info.SendCheckDate.ToString(Const.DateTimeFormat) + "'," +
                  " SampleType =  '" + info.SampleType + "'," +
                  " CheckDoctor =  '" + info.CheckDoctor + "'," +
                  " Auditor = '" + info.Auditor + "'," +
                  " Clinical =  '" + info.Clinical + "'," +
                  " Remarks =  '" + info.Remarks + "'," +
                  " IsPrint =  " + (info.IsPrint ? "1" : "0") + "," +
                  " ImageFile =  '" + info.ImageFile + "'," +
                  " IsAuditor =  " + (info.IsAuditor ? "1" : "0") +
                  " Where SampleID = " + info.SampleID + " and RegistDate >= #" + info.RegistDate.ToString(Const.DateFormat) + " 00:00:00#" +
                  " and RegistDate <= #" + info.RegistDate.ToString(Const.DateFormat) + " 23:59:59#";

            string sqlInsert = "Insert Into Tab_SampleInfo(SampleID,RegistDate,ImportDate,BarCode,CheckDate,PatientName," +
                        "PatientAge,PatientAgeUnit,PatientSex,IsEmergency,IsTest,CaseNo,BedNo,PatientType,SendCheckDoctor," +
                        "SendCheckOffice,SendCheckDate,SampleType,CheckDoctor,Auditor," +
                        "Clinical,Remarks,IsPrint,IsAuditor,IsLis,ImageFile) values(" +
                        info.SampleID + "," +
                        "'" + info.RegistDate.ToString(Const.DateTimeFormat) + "'," +
                        "'" + info.ImportDate.ToString(Const.DateTimeFormat) + "'," +
                        "'" + info.BarCode + "'," +
                        "'" + info.CheckDate.ToString(Const.DateTimeFormat) + "'," +
                        "'" + info.Name + "'," +
                        info.Age + "," +
                        "'" + info.AgeUnit + "'," +
                        "'" + info.Sex + "'," +
                        (info.IsEmergency ? "1" : "0") + "," +
                        (info.IsTest ? "1" : "0") + "," +
                        "'" + info.CaseNo + "'," +
                        "'" + info.BedNo + "'," +
                        "'" + info.PatientType + "'," +
                        "'" + info.SendCheckDoctor + "'," +
                        "'" + info.SendCheckOffice + "'," +
                        "'" + info.SendCheckDate.ToString(Const.DateTimeFormat) + "'," +
                        "'" + info.SampleType + "'," +
                        "'" + info.CheckDoctor + "'," +
                        "'" + info.Auditor + "'," +
                        "'" + info.Clinical + "'," +
                        "'" + info.Remarks + "'," +
                        (info.IsPrint ? "1" : "0") + "," +
                        (info.IsAuditor ? "1" : "0") + "," +
                        (info.IsLis ? "1" : "0") + "," +
                        "'" + info.ImageFile + "')";

            string sql = "";
            try
            {
                if (IsExists(info))
                {
                    sql = sqlUpdate;
                }
                else
                {
                    sql = sqlInsert;
                }

                nCount = db.Execute(sql);
            }
            catch (Exception ex)
            {
                Log.WriteLog(ex.Message + "SampleInfoModel.Update" + sql);
            }



            return nCount;
        }
        /// <summary>
        /// 删除样本
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public int Delete(DBSampleInfo info)
        {
            int nCount = 0;

            string sql = string.Format("Delete From Tab_SampleInfo Where SampleID = {0} and RegistDate >= #{1} 00:00:00# and RegistDate <= #{2} 23:59:59#",
              info.SampleID,
              info.RegistDate.ToString(Const.DateFormat),
              info.RegistDate.ToString(Const.DateFormat)
              );
            string sqlAnalyzerResult = string.Format("Delete From Tab_SampleAnalyzerResult Where SampleID = {0} and RegistDate >= #{1} 00:00:00# and RegistDate <= #{2} 23:59:59#",
              info.SampleID,
              info.RegistDate.ToString(Const.DateFormat),
              info.RegistDate.ToString(Const.DateFormat)
              );
            string sqlManualResult = string.Format("Delete From Tab_SampleManualResult Where SampleID = {0} and RegistDate >= #{1} 00:00:00# and RegistDate <= #{2} 23:59:59#",
              info.SampleID,
              info.RegistDate.ToString(Const.DateFormat),
              info.RegistDate.ToString(Const.DateFormat)
              );

            try
            {
                List<string> sqlList = new List<string>();
                sqlList.Add(sql);
                sqlList.Add(sqlAnalyzerResult);
                sqlList.Add(sqlManualResult);
                nCount = db.BatchExecute(sqlList);
            }
            catch (Exception ex)
            {
                Log.WriteLog(ex.Message + "SampleInfoModel.Delete" + sql + sqlAnalyzerResult + sqlManualResult);
            }

            return nCount;
        }
        public int BatchDelete(DBSampleInfo StartSampleInfo, DBSampleInfo EndSampleInfo)
        {
            int nCount = 0;

            List<DBSampleInfo> deleteList = new List<DBSampleInfo>();
            string sqlDelete = string.Format("Select * From Tab_SampleInfo Where SampleID >= {0} and SampleID <= {1} and RegistDate >= #{2} 00:00:00# and RegistDate <= #{3} 23:59:59# and IsAuditor = 0",
              StartSampleInfo.SampleID,
              EndSampleInfo.SampleID,
              StartSampleInfo.RegistDate.ToString(Const.DateFormat),
              EndSampleInfo.RegistDate.ToString(Const.DateFormat)
              );

            Query(sqlDelete, deleteList);

            List<string> sqlList = new List<string>();

            foreach (DBSampleInfo info in deleteList)
            {
                string sql = string.Format("Delete From Tab_SampleInfo Where SampleID = {0} and RegistDate >= #{1} 00:00:00# and RegistDate <= #{2} 23:59:59#",
                  info.SampleID,
                  info.RegistDate.ToString(Const.DateFormat),
                  info.RegistDate.ToString(Const.DateFormat)
                  );
                string sqlAnalyzerResult = string.Format("Delete From Tab_SampleAnalyzerResult Where SampleID = {0} and RegistDate >= #{1} 00:00:00# and RegistDate <= #{2} 23:59:59#",
                  info.SampleID,
                  info.RegistDate.ToString(Const.DateFormat),
                  info.RegistDate.ToString(Const.DateFormat)
                  );
                string sqlManualResult = string.Format("Delete From Tab_SampleManualResult Where SampleID = {0} and RegistDate >= #{1} 00:00:00# and RegistDate <= #{2} 23:59:59#",
                  info.SampleID,
                  info.RegistDate.ToString(Const.DateFormat),
                  info.RegistDate.ToString(Const.DateFormat)
                  );



                sqlList.Add(sql);
                sqlList.Add(sqlAnalyzerResult);
                sqlList.Add(sqlManualResult);
            }




            try
            {

                nCount = db.BatchExecute(sqlList);
            }
            catch (Exception ex)
            {
                Log.WriteLog(ex.Message + "SampleInfoModel.BatchDelete" + sqlList.ToString());
            }
            return nCount;
        }
        /// <summary>
        /// 获取最大样本号
        /// </summary>
        /// <returns></returns>
        public int GetMaxSampleID()
        {
            int nCount = 0;

            string sql = string.Format("Select IIF(ISNULL(Max(SampleID)),1,Max(SampleID) + 1) as MaxSampleID From Tab_SampleInfo Where RegistDate >= #{0} 00:00:00# and RegistDate <= #{1} 23:59:59#",

              DateTime.Now.ToString(Const.DateFormat),
              DateTime.Now.ToString(Const.DateFormat)
              );

            try
            {
                IDataReader objReader = null;
                db.Query(sql, out objReader);
                while (objReader != null && objReader.Read())
                {
                    nCount = objReader["MaxSampleID"].Equals(DBNull.Value) ? 1 : Convert.ToInt32(objReader["MaxSampleID"]);
                }

            }
            catch (Exception ex)
            {
                Log.WriteLog(ex.Message + " SampleInfoModel.GetMaxSampleID " + sql);
            }


            return nCount;
        }


    }
}
