<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" Debug="true" %>

<%@ Register Assembly="FastReport.Web" Namespace="FastReport.Web" TagPrefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>FastReport .NET Demo</title>
</head>
<body style="background-repeat: no-repeat; background-color: lightgrey;">
    <form id="form1" runat="server">
    <div>
        <table>
            <tr>
                <td style="height: 44px; text-align: center; width: 106px;">
                    <asp:Image ID="Image1" runat="server" ImageUrl="~/images/demo_logo.png" EnableViewState="False" /></td>
                <td style="height: 44px; width: 768px;">
                    <span style="font-size: 16pt; font-family: Tahoma"><strong>FastReport .NET Demo<br />
                        <asp:Label ID="Version" runat="server" Font-Size="Small" Text="ver. 1.0.0" Width="188px" EnableViewState="False"></asp:Label></strong></span></td>
            </tr>
        </table>        
        <hr />
    </div>
                <strong><span style="font-size: 10pt; font-family: Tahoma"></span></strong>
                <table>
                    <tr>
                        <td style="width: 300px; height: 884px;" valign="top">
                            
        <asp:Menu ID="LeftMenu" runat="server" BackColor="GhostWhite" DynamicHorizontalOffset="2"
            Font-Names="Arial" Font-Size="9pt" ForeColor="Black" StaticSubMenuIndent="10px" MaximumDynamicDisplayLevels="2" StaticDisplayLevels="2" Width="240px" Height="169px" OnInit="LeftMenu_Init" OnMenuItemClick="LeftMenu_MenuItemClick">
            <StaticSelectedStyle BackColor="LightSteelBlue" ForeColor="DarkBlue" />
            <StaticMenuItemStyle HorizontalPadding="5px" VerticalPadding="2px" />
            <DynamicHoverStyle BackColor="#990000" ForeColor="White" />
            <DynamicMenuStyle BackColor="#FFFBD6" />
            <DynamicItemTemplate>
                <%# Eval("Text") %>
            </DynamicItemTemplate>
            <DynamicSelectedStyle BackColor="Chocolate" />
            <DynamicMenuItemStyle HorizontalPadding="5px" VerticalPadding="2px" />
            <StaticHoverStyle BackColor="Lavender" ForeColor="DarkBlue" />
            <Items>
                <asp:MenuItem Text="Simple reports" Value="Simple reports" Selectable="False">
                    <asp:MenuItem Selected="True" Text="Report1" Value="Report1"></asp:MenuItem>
                    <asp:MenuItem Text="Report2" Value="Report2"></asp:MenuItem>
                    <asp:MenuItem Text="Report3" Value="Report3"></asp:MenuItem>
                </asp:MenuItem>
                <asp:MenuItem Selectable="False" Text="Matrix" Value="Matrix">
                    <asp:MenuItem Text="Matrix1" Value="Matrix1"></asp:MenuItem>
                    <asp:MenuItem Text="Matrix2" Value="Matrix2"></asp:MenuItem>
                    <asp:MenuItem Text="Matrix3" Value="Matrix3"></asp:MenuItem>
                </asp:MenuItem>
            </Items>
            <LevelMenuItemStyles>
                <asp:MenuItemStyle Font-Bold="True" Font-Underline="False" ForeColor="Black" />
                <asp:MenuItemStyle Font-Size="8pt" Font-Underline="False" />
            </LevelMenuItemStyles>
        </asp:Menu>
                        </td>
                        <td style="WIDTH: 100%; HEIGHT: 884px" valign="top">                            


                                <cc1:WebReport ID="WebReport1" runat="server" BackColor="White" Font-Bold="False"
				    Width="800px" Height="1000px"
				    OnStartReport="WebReport1_StartReport" Padding="5, 5, 5, 5" ToolbarColor="Lavender"
                                    PrintInPdf="True"
                                    PdfEmbeddingFonts="True"
                                    Layers="False" Zoom="1"
                                    ToolbarStyle= "Large"
                                    ToolbarBackgroundStyle="Light"
                                    ToolbarIconsStyle="Blue"
                                    />


                                </td>
                    </tr>
                </table>
                <br />
    </form>
</body>
</html>
