using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataModel;
using System.IO;

namespace ViewModel
{
  public class BackupDBObject
  {
    public string Caption { get; set; }
    public string DBName { get; set; }
  }
  public class BackupDBViewModel
  {
    private BackupDBModel backupDBModel = new BackupDBModel();
    private string backupPath = System.AppDomain.CurrentDomain.BaseDirectory + "DBBackup";
    private string testDB = System.AppDomain.CurrentDomain.BaseDirectory + "Database.mdb";
    public void ClearTestData()
    {
      backupDBModel.ClearTestData();
    }
    public bool BackUpDB()
    {
      bool success = true;
      try
      {
        File.Copy(testDB, backupPath + "\\Database" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".mdb");
      }
      catch
      {
        success = false;
      }
      return success;
    }
    public List<BackupDBObject> GetBackupDBs()
    {
      List<BackupDBObject> backUpDBs = new List<BackupDBObject>();
      string[] backupDBFiles = Directory.GetFiles(backupPath);
      backUpDBs.Add(new BackupDBObject() { Caption = "测试库", DBName = testDB });
      for (int i = 0; i < backupDBFiles.Length; i++)
      {
        BackupDBObject backupDB = new BackupDBObject();
        FileInfo fi = new FileInfo(backupDBFiles[i]);
        backupDB.Caption = fi.CreationTime.ToString("yyyy-MM-dd HH:mm:ss") + "  备份库";
        backupDB.DBName = fi.FullName;
        backUpDBs.Add(backupDB);
      }
      return backUpDBs;
    }
  }
}
