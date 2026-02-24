using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Text;
using System.IO;
using FastReport.Web;
using FastReport.Utils;
using FastReport.Export.Pdf;

namespace MvcRazor.Controllers
{
    public class HomeController : Controller
    {
        private WebReport webReport = new WebReport();

        public ActionResult Index()
        {
            SetReport();
            
            webReport.Width = 600;
            webReport.Height = 800;
            webReport.ToolbarIconsStyle = ToolbarIconsStyle.Black;

            ViewBag.WebReport = webReport;
            return View();
        }

        private void SetReport()
        {
            string report_path = GetReportPath();
            System.Data.DataSet dataSet = new System.Data.DataSet();
            dataSet.ReadXml(report_path + "nwind.xml");
            webReport.Report.RegisterData(dataSet, "NorthWind");
            webReport.Report.Load(report_path + "Simple List.frx");
        }

        public FileResult GetFile()
        {
            SetReport();
            webReport.Report.Prepare();
            Stream stream = new MemoryStream();
            webReport.Report.Export(new PDFExport(), stream);
            stream.Position = 0;            
            return File(stream, "application/zip", "report.pdf");
        }

        private string GetReportPath()
        {
            string report_path = Config.ApplicationFolder;
            using (XmlDocument xml = new XmlDocument())
            {
                xml.Load(report_path + "config.xml");
                foreach (XmlItem item in xml.Root.Items)
                    if (item.Name == "Config")
                        foreach (XmlItem configitem in item.Items)
                            if (configitem.Name == "Reports")
                                report_path += configitem.GetProp("Path");
            }
            return report_path;
        }

        public ActionResult Prepared()
        {
            webReport.Width = 600;
            webReport.Height = 800;
            webReport.ToolbarBackgroundStyle = ToolbarBackgroundStyle.Dark;
            webReport.ToolbarIconsStyle = ToolbarIconsStyle.Red;
            webReport.StartReport += new EventHandler(webReport_StartReport);
            ViewBag.WebReport = webReport;
            return View();
        }

        void webReport_StartReport(object sender, EventArgs e)
        {
            (sender as WebReport).ReportDone = true;
            string s = this.Server.MapPath("~/App_Data/Prepared.fpx");
            (sender as WebReport).Report.LoadPrepared(s);
        }

        public ActionResult Dialogs()
        {
            webReport.Width = 600;
            webReport.Height = 800;
            webReport.ToolbarBackgroundStyle = ToolbarBackgroundStyle.Light;
            webReport.ToolbarIconsStyle = ToolbarIconsStyle.Blue;
            webReport.ReportFile = this.Server.MapPath("~/App_Data/Dialogs.frx");
            ViewBag.WebReport = webReport;
            return View();
        }
    }
}
