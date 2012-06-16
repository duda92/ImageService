<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" EnableViewState="True" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="Styles/Styles.css" rel="stylesheet" type="text/css" />
    <link href="Styles/jquery.jscrollpane.css" rel="stylesheet" type="text/css" />
    <link href="Styles/fileuploader.css" rel="stylesheet" type="text/css" />
    
    <script src="Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
    
    <script src="Scripts/async_update_requester.js" type="text/javascript"></script>
    
    <%--GridView scroll--%>
    <script src="Scripts/GridView/item_selection.js" type="text/javascript"></script>
    <script src="Scripts/GridView/jquery.jscrollpane.js" type="text/javascript"></script>
    <script src="Scripts/GridView/jquery.jscrollpane.min.js" type="text/javascript"></script>
    <script src="Scripts/GridView/jquery.mousewheel.js" type="text/javascript"></script>
    <script src="Scripts/GridView/mwheelIntent.js" type="text/javascript"></script>
    <%--GridView scroll--%>
    
    <script src="Scripts/FileUpload/ajaxupload.js" type="text/javascript"></script>
    <script src="Scripts/FileUpload/uploader_init.js" type="text/javascript"></script>

    <script language="javascript" type="text/javascript">

        var selected_file_name = '';

        function pageLoad() {            
            auto_select_item();
            init_uploader();
            $('#GridViewPanel').jScrollPane();
        }
    </script>

    <script type="text/javascript">
        function UpdPanelUpdate() {
            __doPostBack("<%= UpdateButton.ClientID %>", "");
        }

    </script>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true" EnablePartialRendering="true" >
    </asp:ScriptManager>
    <%------------------------------------------------------------------------------------------------%>
    <div id="ButtonsPanel">
        <table align="center" width="600px">
            <tr>
                <td height="50px" width="180px">
                    <asp:Button runat="server" Text="Upload" ID="UploadButton"
                        class="site_button" UseSubmitBehavior="True" />
                </td>
                <td height="50px" width="180px">
                    <asp:Button runat="server" Text="Update" OnClick="Update_Click" ID="UpdateButton"
                        class="site_button" UseSubmitBehavior="True" />
                </td>
                <td height="50px" width="180px">
                    <asp:Button runat="server" Text="Reset Connection" OnClick="ResetConnection_Click"
                        ID="ResetCnnectionButton" class="site_button" />
                </td>
            </tr>
        </table>
    </div>
    <%------------------------------------------------------------------------------------------------%>
    <div id="frame_div">
        <table align="center">
            <tr>
                <td valign="top">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <div id="GridViewPanel" style="float: left; overflow: auto;">
                                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" EnableRowClicwk="true" PageSize="5">
                                    <Columns>
                                        <asp:TemplateField HeaderText="File name">
                                            <ItemTemplate>
                                                <%# Eval("FileName") %>
                                            </ItemTemplate>
                                            <ItemStyle Width="50%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Last date modified">
                                            <ItemTemplate>
                                                <%# Eval("LastDateModified") %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="File name" Visible="false">
                                            <ItemTemplate>
                                                <%# Eval("ImageData")%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <EmptyDataRowStyle BackColor="LightBlue" ForeColor="Red" />
                                    <EmptyDataTemplate>
                                        No Data Found.
                                    </EmptyDataTemplate>
                                    <HeaderStyle Height="40px" CssClass="HeaderImagesGrid" Font-Bold="True" />
                                    <RowStyle BorderStyle="Solid" Height="30px" CssClass="Row" />
                                    <SelectedRowStyle BackColor="Blue" BorderColor="Blue" BorderStyle="Solid" />
                                </asp:GridView>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="UpdateButton"/>
                            <asp:AsyncPostBackTrigger ControlID="ResetCnnectionButton" />
                        </Triggers>
                    </asp:UpdatePanel>

                
                </td>
                <td valign="top">
                    <div id="ImagePanel" style="border: 1px solid #000000; float: left;">
                        <asp:Image ID="Image1" runat="server" />
                    </div>
                </td>
            </tr>
        </table>
    </div>

    <%------------------------------------------------------------------------------------------------%>
    <asp:ModalPopupExtender ID="MPE" runat="server" TargetControlID="UploadButton" PopupControlID="UploadPanel"
         CancelControlID="UploadPanelCancel">
        <Animations>
        <OnShown>
            <FadeIn Duration="0.9" Fps="40" />
        </OnShown>
        <OnHiding>
         <FadeOut Duration="0.9" Fps="40" />
         </OnHiding>
        </Animations>
    </asp:ModalPopupExtender>
    <asp:DropShadowExtender ID="dse" runat="server" TargetControlID="ShadowPanel" Opacity=".8" Rounded="true" />
    <asp:Panel ID="UploadPanel" runat="server"  Width="300px" Height="400px">
        <asp:Panel ID="ShadowPanel" runat="server" BackImageUrl="~/Styles/upload_panel_background.jpg">
            <div id="panel_div" >
                <div id="UploadPanelContent">
                    <asp:Image ImageUrl="Styles/upload_panel_image.png" runat="server" Height="100%" Width="100%" />
                </div>
                <div id="ok_cancel_part_OfUploadPanel">                 			
                    <center>
                        <div  id="file_upload" > 
                         </div>    
                        <asp:Button ID="UploadPanelCancel" runat="server" Text="Close" CssClass="site_button"
                            Height="30%" Width="40%"/>                                                    
                    </center>
                </div>
            </div>
        </asp:Panel>
    </asp:Panel>
    </form>
</body>
</html>
