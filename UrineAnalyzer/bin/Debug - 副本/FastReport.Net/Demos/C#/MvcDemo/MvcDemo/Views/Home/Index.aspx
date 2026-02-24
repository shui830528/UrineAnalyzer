<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Register assembly="FastReport.Web" namespace="FastReport.Web" tagprefix="cc1" %>
<script runat="server">

    private string report_path;

    protected void WebReport1_StartReport(object sender, EventArgs e)
    {
        RegisterData((sender as WebReport).Report);
        (sender as WebReport).Report.Load(report_path + "Cascaded Data Filtering.frx");
    }

    private void RegisterData(FastReport.Report FReport)
    {
        using (FastReport.Utils.XmlDocument xml = new FastReport.Utils.XmlDocument())
        {
            xml.Load(FastReport.Utils.Config.ApplicationFolder + "config.xml");
            foreach (FastReport.Utils.XmlItem item in xml.Root.Items)
                if (item.Name == "Config")
                    foreach (FastReport.Utils.XmlItem configitem in item.Items)
                        if (configitem.Name == "Reports")
                            report_path = FastReport.Utils.Config.ApplicationFolder + configitem.GetProp("Path");
        }

        System.Data.DataSet FDataSet = new System.Data.DataSet();
        FDataSet.ReadXml(report_path + "nwind.xml");
        FReport.RegisterData(FDataSet, "NorthWind");
    }

</script>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">                
        <cc1:WebReport ID="WebReport1" runat="server" BackColor="White" 
            BorderWidth="0px" Height="744px" 
            onstartreport="WebReport1_StartReport" Width="545px" BorderColor="White" />
</asp:Content>
