<%@ Page Language="C#" MasterPageFile="~/P5s.Master" AutoEventWireup="true" CodeBehind="TradeProgramMarkDSR.aspx.cs"
    Inherits="MMV.fsmdls.mmcv4.TradeProgramMarkDSR" %>

<asp:Content ID="Content1" ContentPlaceHolderID="BodyContentPlaceHolder" runat="server">
    <link href="../../libs/PgwSlideshow/PgwSlideshow.css" rel="stylesheet" type="text/css" />

    <script src="../../libs/PgwSlideshow/PgwSlideshow.js" type="text/javascript"></script>

    <script src="../../libs/Zoomimage/jquery.ez-plus.js" type="text/javascript"></script>

    <link href="CSS/fscstyles_181103.css" rel="stylesheet" />

    <script type="text/javascript">
        var $P5s = jQuery.noConflict(true);
        $P5s(document).ready(function () {
            var show = $('#<%=CheckShow.ClientID %>').val();
            if (show == "1") {
                $('#formsearch').show();
                $('#showhide').text('Hide');
            } else {
                $('#formsearch').hide();
                $('#showhide').text('show');
            }
        });
        function Showhide() {
            var show = $('#<%=CheckShow.ClientID %>').val();
            if (show == "1") {
                show = false;
                $('#formsearch').hide();
                $('#showhide').text('Show');
                $('#<%=CheckShow.ClientID %>').val('0');
            }
            else {
                show = true;
                $('#formsearch').show();
                $('#showhide').text('Hide');
                $('#<%=CheckShow.ClientID %>').val('1');
            }
            return false;
        };



        function LoadLevelApprove() {

            //P5sHdfData
            var data = document.getElementById('<%=P5sHdfData.ClientID%>').value;
            var arrayData = new Array();
            arrayData = data.split("❂");
            var objectLevelApprove = $.parseJSON(arrayData[0].toString());
            var objectCondition = $.parseJSON(arrayData[1].toString());
            var objectItemCombo = $.parseJSON(arrayData[2].toString());

            var disabled = '';
            for (var i = 0; i < objectLevelApprove.length; i++) {
                disabled = '';
                if (objectLevelApprove[i].CHECK == '0') {
                    disabled = 'disabled="disabled"';
                }
                var node = document.createElement("fieldset");
                var html = '<legend>' + objectLevelApprove[i].LEVEL_APPROVE_NAME + '</legend>';
                html = html + '<table cellpadding="0" cellspacing="5px" border="0" width="100%"><tbody>';

                for (var j = 0; j < objectCondition.length; j++) {
                    if (objectLevelApprove[i].LEVEL_APPROVE_CD == objectCondition[j].LEVEL_APPROVE_CD) {

                        html = html + '<tr>';
                        html = html + '<td style="white-space: nowrap; width: 1%; vertical-align: center; font-weight: bold">' + objectCondition[j].CONDITION_NAME;
                        html = html + ': </td><td style="white-space: nowrap; width: 100%; vertical-align: top">';
                        if (objectCondition[j].CONDITION_TYPE_CD == 1) {

                            html = html + '<input type="text" style="width: 97%;" id="condition_' + objectCondition[j].CONDITION_CD + '" name="condition_' + objectCondition[j].CONDITION_CD + '" value="' + objectCondition[j].VALUE + '" ' + disabled + ' />';
                        } else {
                            html = html + '<select id="condition_' + objectCondition[j].CONDITION_CD + '" style="width: 100%;" name = "condition_' + objectCondition[j].CONDITION_CD + '" ' + disabled + '><option value="0">-----Pls select-----</option>';
                            for (var k = 0; k < objectItemCombo.length; k++) {
                                if (objectItemCombo[k].CONDITION_CD == objectCondition[j].CONDITION_CD) {

                                    var selected = '';
                                    if (objectItemCombo[k].COMBOBOX_VALUE_CD == objectCondition[j].COMBOBOX_VALUE_CD) {
                                        selected = ' selected="selected" '
                                    }
                                    html = html + '<option value="' + objectItemCombo[k].COMBOBOX_VALUE_CD + '" ' + selected + '>' + objectItemCombo[k].COMBOBOX_VALUE + '</option>';
                                }
                            }
                            html = html + '</select>';
                        }
                        html = html + '</td></tr>';
                    }
                }
                if (objectLevelApprove[i].CHECK == '1') {
                    html = html + '<tr><td style="white-space: nowrap; width: 1%; vertical-align: top; font-weight: bold"></td><td style="white-space: nowrap; width: 1%; vertical-align: top; font-weight: bold">';
                    html = html + '<button type="button" id="save_' + objectLevelApprove[i].LEVEL_APPROVE_CD + '" ' + disabled + ' onclick="Save(' + objectLevelApprove[i].LEVEL_APPROVE_CD + ',' + objectLevelApprove[i].TRADE_PROGRAM_CUSTOMER_CD + ')">Save</button></td></tr>';
                }

                html = html + '</tbody></table>';
                node.innerHTML = html;
                document.getElementById('LoadLevel').appendChild(node);
            }
        }


        function Save(val, val1) {
            document.getElementById('save_' + val).disabled = true;
            var result = new Object();
            var data = document.getElementById('<%=P5sHdfData.ClientID%>').value;
            var arrayData = new Array();
            arrayData = data.split("❂");
            var objectCondition = $.parseJSON(arrayData[1].toString());
            result["Level"] = val;
            result["TRADE_PROGRAM_CUSTOMER_CD"] = val1;
            for (var i = 0; i < objectCondition.length; i++) {
                if (val == objectCondition[i].LEVEL_APPROVE_CD) {
                    var value = document.getElementById('condition_' + objectCondition[i].CONDITION_CD).value.replace("'", "").trim();
                    if (objectCondition[i].CONDITION_TYPE_CD == 1) {
                        if (value == null || value == '') {
                            alert('The ' + objectCondition[i].CONDITION_NAME + ' field is required!');
                            document.getElementById('condition_' + objectCondition[i].CONDITION_CD).focus();
                            document.getElementById('save_' + val).disabled = false;
                            return;
                        }

                    } else {
                        if (value == null || value == '' || value == '0') {
                            alert('The ' + objectCondition[i].CONDITION_NAME + ' field is required!');
                            document.getElementById('condition_' + objectCondition[i].CONDITION_CD).focus();
                            document.getElementById('save_' + val).disabled = false;
                            return;
                        }
                    }
                    result[objectCondition[i].CONDITION_CD] = value;
                }
            }
            //JSON.stringify(result)
            $.ajax({
                url: 'TradeProgramMarkDSR.aspx/ResultTradeProgram',
                data: "{ 'result': '" + JSON.stringify(result) + "' }",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    if (data == "T") {
                        for (var i = 0; i < objectCondition.length; i++) {
                            if (val == objectCondition[i].LEVEL_APPROVE_CD) {
                                document.getElementById('condition_' + objectCondition[i].CONDITION_CD).disabled = true;
                            }
                        }
                        document.getElementById('save_' + val).disabled = true;
                        alert('Successful.');
                    } else {
                        document.getElementById('save_' + val).disabled = false;
                        alert(data);
                    }
                },
                error: function (data, status, jqXHR) { alert(jqXHR); document.getElementById('save_' + val).disabled = false; }
            });

        }
    </script>

    <form id="frmMain" runat="server">
        <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManagerMain" runat="server">
        </ajaxToolkit:ToolkitScriptManager>
        <!-------------Begin Search  ----------------------->
        <!-------------Begin Search  ----------------------->
        <asp:UpdatePanel ID="P5sUpanelSearch" runat="server">
            <ContentTemplate>
                <table cellpadding="0" cellspacing="0" border="0" width="100%" style="background-color: #D0D0D0">
                    <tr>
                        <td colspan="6">&nbsp
                        </td>
                    </tr>
                    <tr id="formsearch">
                        <td style="width: 10%">
                        </td>
                        <td style="width: 90%" id="TdPnlFilter">
                            <table cellpadding="0px" cellspacing="10px" border="0" width="100%">
                                <tr>
                                    <td style="width: 50%">
                                        <table cellpadding="0px" cellspacing="10px" border="0" width="100%">
                                            <tr>
                                                <td style="white-space: nowrap; vertical-align: top">From date:
                                                </td>
                                                <td style="white-space: nowrap; vertical-align: top">
                                                    <asp:TextBox ID="P5sTxtFrom" Width="150px" CssClass="textbox" AutoPostBack="true"
                                                        runat="server" OnTextChanged="P5sTxtFrom_TextChanged1"></asp:TextBox>
                                                    <ajaxToolkit:CalendarExtender ID="CalendarExtenderFrom" runat="server" TargetControlID="P5sTxtFrom"
                                                        Format="dd-MM-yyyy">
                                                    </ajaxToolkit:CalendarExtender>
                                                </td>
                                                <td style="white-space: nowrap; vertical-align: top">To date:
                                                </td>
                                                <td style="white-space: nowrap; vertical-align: top">
                                                    <asp:TextBox ID="P5sTxtTo" Width="150px" CssClass="textbox" AutoPostBack="true" runat="server" OnTextChanged="P5sTxtTo_TextChanged1"></asp:TextBox>
                                                    <ajaxToolkit:CalendarExtender ID="CalendarExtenderTo" runat="server" TargetControlID="P5sTxtTo"
                                                        Format="dd-MM-yyyy">
                                                    </ajaxToolkit:CalendarExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 1%; vertical-align: top">Program:<span style="color: Red;white-space: nowrap;"> *</span>
                                                    <br />
                                                </td>
                                                <td colspan="4" style="white-space: nowrap; width: 1000px; vertical-align: top">
                                                    <asp:TextBox ID="P5sTxtTradeProgramCD" Width="394px" CssClass="textbox" runat="server" Visible="false"></asp:TextBox>
                                                    <asp:DropDownList Width="95%" CssClass="input-sm" ID="P5sTxtTradeProgram_CD" AutoPostBack="true" runat="server"></asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 1%; vertical-align: top">Region:<span style="color: Red;white-space: nowrap;"> *</span>
                                                    <br />
                                                </td>
                                                <td colspan="4" style="white-space: nowrap; width: 1000px; vertical-align: top">
                                                    <asp:DropDownList Width="60%" CssClass="input-sm" ID="P5sDDLRegion" AutoPostBack="true" runat="server" OnSelectedIndexChanged="P5sDDLRegion_SelectedIndexChanged"></asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 1%; vertical-align: top">Area:<span style="color: Red;white-space: nowrap;"> *</span>
                                                    <br />
                                                </td>
                                                <td colspan="4" style="white-space: nowrap; width: 1000px; vertical-align: top">
                                                    <asp:DropDownList Width="60%" CssClass="input-sm" ID="P5sDDLArea" AutoPostBack="true" runat="server" OnSelectedIndexChanged="P5sDDLArea_SelectedIndexChanged"></asp:DropDownList>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td style="width: 50%">
                                        <table cellpadding="0px" cellspacing="10px" border="0" width="100%">
                                            <tr>
                                                <td style="white-space: nowrap; width: 1%; vertical-align: top">Distributor:<span style="color: Red; white-space: nowrap;"> *</span>
                                                    <br />
                                                </td>
                                                <td colspan="4" style="white-space: nowrap; width: 1000px; vertical-align: top">
                                                    <asp:DropDownList Width="100%" CssClass="input-sm" ID="P5sDDLDistributor" AutoPostBack="true" runat="server" OnSelectedIndexChanged="P5sDDLDistributor_SelectedIndexChanged" ></asp:DropDownList>
                                                    <%--<select class="input-sm" style="width: 95%;" id="P5sTxtDistributor_CD" runat="server" multiple="true" onserverchange="Distributor_ServerChange" onclick="__doPostBack()">
                                                    </select>--%>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="white-space: nowrap; width: 1%; vertical-align: top">DSR:
                                        <br />
                                                </td>
                                                <td colspan="4" style="white-space: nowrap; width: 1000px; vertical-align: top">
                                                    <select class="input-sm" style="width: 70%;" id="P5sTxtSales_CD" runat="server" multiple="true">
                                                    </select>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="white-space: nowrap; width: 1%; vertical-align: top">AMS:
                                        <br />
                                                </td>
                                                <td colspan="4" style="white-space: nowrap; width: 1000px; vertical-align: top">From
                                        <asp:TextBox ID="P5sTxtAMSFrom" Width="165px" CssClass="textbox" Text="0" runat="server"></asp:TextBox>
                                                    To
                                        <asp:TextBox ID="P5sTxtAMSTo" Width="165px" CssClass="textbox" runat="server"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" style="padding-left:58%">
                                        <asp:Button ID="P5sLbtnSearch" runat="server" CausesValidation="False" OnClick="P5sLbtnSearch_Click" Text="Search" />
                                    </td>
                                </tr>
                            </table>
                        </td>

                    </tr>
                    <tr>
                        <td style="width: 1%">
                            <asp:HiddenField ID="CheckShow" Value="1" runat="server"></asp:HiddenField>
                            <button onclick='return Showhide();' id='showhide'>Hide</button>
                        </td>
                        <td colspan="5"></td>
                    </tr>
                </table>
                <span class="normaltxtNavi">
                    <%=HttpContext.Current.Session["P5s.Master.FormCaption"]%>
                </span>
                <br />
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="P5sLbtnSearch" />
                <asp:PostBackTrigger ControlID="P5sTxtFrom" />
                <asp:PostBackTrigger ControlID="P5sTxtTo" />
            </Triggers>
        </asp:UpdatePanel>
       <asp:UpdatePanel ID="P5sUPanlMain" runat="server">
            <ContentTemplate>
                <table cellpadding="0" cellspacing="5px" border="0" width="100%">
                    <tr>
                        <td style="max-width: 400px; vertical-align: top; width: 60%">
                            <asp:HiddenField ID="P5sHdfDataImage" runat="server" />
                            <asp:HiddenField ID="P5sHdfImageCDSelected" runat="server" />
                            <asp:Panel ID="P5sPnlMainDetail" runat="server" Width="100%" Visible="False">
                                <%=L5sDmComm.L5sFormControl.L5sTopOfForm()%>
                                <table cellpadding="0" cellspacing="5px" border="0" width="100%">
                                    <tr>
                                        <td style="white-space: nowrap; width: 1%; vertical-align: top; font-weight: bold">
                                            Program :
                                        </td>
                                        <td colspan="5" style="white-space: nowrap; width: 1%; vertical-align: top">
                                            <asp:Label ID="P5sLblTradeProgramDesc" runat="server" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="white-space: nowrap; width: 1%; vertical-align: top; font-weight: bold">
                                            Region :
                                        </td>
                                        <td style="white-space: nowrap; width: 1%; vertical-align: top">
                                            <asp:Label ID="P5sLblRegionCode" runat="server" Text=""></asp:Label>
                                        </td>
                                        <td style="white-space: nowrap; width: 5%; vertical-align: top">
                                            &nbsp;
                                        </td>
                                        <td style="white-space: nowrap; width: 1%; vertical-align: top; font-weight: bold">
                                            Area :
                                        </td>
                                        <td style="white-space: nowrap; width: 1%; vertical-align: top">
                                            <asp:Label ID="P5sLblAreaCode" runat="server" Text=""></asp:Label>
                                        </td>
                                        <td style="white-space: nowrap; width: 100%; vertical-align: top">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="white-space: nowrap; width: 1%; vertical-align: top; font-weight: bold">
                                            Distributor :
                                        </td>
                                        <td style="white-space: nowrap; width: 1%; vertical-align: top">
                                            <asp:Label ID="P5sLblDistributorDesc" runat="server" Text=""></asp:Label>
                                        </td>
                                        <td style="white-space: nowrap; width: 5%; vertical-align: top">
                                            &nbsp;
                                        </td>
                                        <td style="white-space: nowrap; width: 1%; vertical-align: top; font-weight: bold">
                                            DSR :
                                        </td>
                                        <td style="white-space: nowrap; width: 1%; vertical-align: top">
                                            <asp:Label ID="P5sLblSalesDesc" runat="server" Text=""></asp:Label>
                                        </td>
                                        <td style="white-space: nowrap; width: 100%; vertical-align: top">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="white-space: nowrap; width: 1%; vertical-align: top; font-weight: bold">
                                            Customer :
                                        </td>
                                        <td style="white-space: nowrap; width: 1%; vertical-align: top">
                                            <asp:Label ID="P5sLblCustomerDesc" runat="server" Text=""></asp:Label>
                                        </td>
                                        <td style="white-space: nowrap; width: 5%; vertical-align: top">
                                            &nbsp;
                                        </td>
                                        <td style="white-space: nowrap; width: 1%; vertical-align: top; font-weight: bold">
                                            Address :
                                        </td>
                                        <td style="white-space: nowrap; width: 1%; vertical-align: top">
                                            <asp:Label ID="P5sLblCustomerAddress" runat="server" Text=""></asp:Label>
                                        </td>
                                        <td style="white-space: nowrap; width: 100%; vertical-align: top">
                                        </td>
                                    </tr>
                                </table>
                                <%=L5sDmComm.L5sFormControl.L5sBottomOfForm()%>
                                <br>
                                </br>
                                <%=L5sDmComm.L5sFormControl.L5sTopOfButton(P5sLbtnRemovePictures)%>
                                <asp:LinkButton ID="P5sLbtnRemovePictures" runat="server" CausesValidation="False"
                                    OnClick="P5sLbtnRemovePictures_Click"
                                    OnClientClick="if(!confirm('Are you sure?')) return false;" 
                                    >Delete photo</asp:LinkButton>
                                <%=L5sDmComm.L5sFormControl.L5sBottomOfButton(P5sLbtnRemovePictures)%>
                            </asp:Panel>
                            <div class="cntr mt20">
                                <ul class="pgwSlideshow">

                                    <script type="text/javascript">                                              
                                                var data = document.getElementById('<%= P5sHdfDataImage.ClientID %>').value;                                              
                                                if(data.length > 0)
                                                {
                                                   var object = $.parseJSON(data);                                                                          
                                                    for(var i = 0; i < object.length; i++){    
                                                  
                                                        var path = object[i].ImagePath;
                                                        var takeDay = "Created date: " +  object[i].ImageTakeDay;                                                       
                                                        var note = "Photo:  " + (i +1) + " / " + object.length ; 
                                                        document.write('<li>');                            
                                                        document.write('<img src="'+ path + '" alt="' + takeDay +   '" id="' + i +  '"  data-description="' + note + '"></li>');                            
                                                        document.write('</li>'); 
                                                      }       
                                                } 
                                                         
                                                $P5s(document).ready(function($){    
                                                   try {
                                                            var pgwSlideshow =  $P5s('.pgwSlideshow').pgwSlideshow();
                                                        }
                                                        catch(err) {
                                                        }                                   
                                                });    
                                                
                                                                                                
                                                function P5sLoadDefaultDataForCustomerPhoto(index)
                                                 {                                                    
                                                    var data = document.getElementById('<%= P5sHdfDataImage.ClientID %>').value; 
                                                   
                                                                                                                                                           
                                                    if(data.length > 0)
                                                    {
                                                         var object = $.parseJSON(data)[index];      
                                                         var photoNotes1 = document.getElementById('<%= P5sTxtPHOTO_NOTES_1.ClientID %>');   
                                                         var photoNotes2 = document.getElementById('<%= P5sTxtPHOTO_NOTES_2.ClientID %>');   
                                                         var photoNotes3 = document.getElementById('<%= P5sTxtPHOTO_NOTES_3.ClientID %>');   
                                                         var photoNotes4 = document.getElementById('<%= P5sTxtPHOTO_NOTES_4.ClientID %>');   
                                                         var photoNotes5 = document.getElementById('<%= P5sTxtPHOTO_NOTES_5.ClientID %>');   
                                                                                                                                                                            
                                                         photoNotes1.value  = object.PHOTO_NOTES_1;
                                                         photoNotes2.value  = object.PHOTO_NOTES_2;
                                                         photoNotes3.value  = object.PHOTO_NOTES_3;
                                                         photoNotes4.value  = object.PHOTO_NOTES_4;
                                                         photoNotes5.value  = object.PHOTO_NOTES_5;
                                                         
                                                         //set image CD ?? x? d?ng cho tính n?ng xóa hình ?nh
                                                         document.getElementById('<%= P5sHdfImageCDSelected.ClientID %>').value = object.TRADE_PROGRAM_PHOTO_CD;                                                                                                                                    
                                                    }
                                                 }
                                                            
                                                                                           
                                    </script>

                                </ul>
                            </div>
                        </td>
                        <td style="white-space: nowrap; vertical-align: top; width: 39%">
                            <asp:Panel ID="P5sPnlInfo" runat="server" Width="100%" Visible="False">
                                <asp:Panel ID="P5sPnlDSRNote" runat="server" GroupingText="DSR">
                                    <table cellpadding="0" cellspacing="5px" border="0" width="100%">
                                        <tr>
                                            <td style="white-space: nowrap; width: 1%; vertical-align: top; font-weight: bold">Info 1
                                            </td>
                                            <td style="white-space: nowrap; width: 100%; vertical-align: top">
                                                <asp:TextBox ID="P5sTxtPHOTO_NOTES_1" Style="width: 100%" Enabled="false" runat="server"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="white-space: nowrap; width: 1%; vertical-align: top; font-weight: bold">Info 2
                                            </td>
                                            <td style="white-space: nowrap; width: 1%; vertical-align: top">
                                                <asp:TextBox ID="P5sTxtPHOTO_NOTES_2" Style="width: 100%" Enabled="false" runat="server"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="white-space: nowrap; width: 1%; vertical-align: top; font-weight: bold">Info 3
                                            </td>
                                            <td style="white-space: nowrap; width: 1%; vertical-align: top">
                                                <asp:TextBox ID="P5sTxtPHOTO_NOTES_3" Style="width: 100%" Enabled="false" runat="server"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="white-space: nowrap; width: 1%; vertical-align: top; font-weight: bold">Info 4
                                            </td>
                                            <td style="white-space: nowrap; width: 1%; vertical-align: top">
                                                <asp:TextBox ID="P5sTxtPHOTO_NOTES_4" Style="width: 100%" Enabled="false" runat="server"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="white-space: nowrap; width: 1%; vertical-align: top; font-weight: bold">Info 5
                                            </td>
                                            <td style="white-space: nowrap; width: 1%; vertical-align: top">
                                                <asp:TextBox ID="P5sTxtPHOTO_NOTES_5" Style="width: 100%" Enabled="false" runat="server"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>

                                <div id="LoadLevel"></div>
                                <asp:HiddenField runat="server" ID="P5sHdfData" Value=""></asp:HiddenField>

                            </asp:Panel>
                        </td>
                        <td style="white-space: nowrap; vertical-align: top; width: 1%"></td>
                    </tr>
                </table>
                <asp:HiddenField ID="P5sHdfCUSTOMER_CD" runat="server" />
                <asp:DropDownList ID="P5sDdlMainPaging" runat="server" CssClass="dropdown" AutoPostBack="True">
                </asp:DropDownList>
                <asp:GridView ID="P5sMainGridView" runat="server" AutoGenerateColumns="False" CssClass="gridview"
                    OnRowDataBound="P5sMainGridView_RowDataBound" OnRowEditing="P5sMainGridView_RowEditing">
                    <Columns>
                        <asp:BoundField DataField="ROWNUMBER" HeaderText="No." />
                        <asp:BoundField DataField="TRADE_PROGRAM_CODE" HeaderText="Program Code" />
                        <asp:BoundField DataField="SALES_DESC" HeaderText="DSR" />
                        <asp:TemplateField HeaderText="Cust Code" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:LinkButton ID="P5sEdit" runat="server" CommandName="Edit" Text='<%# Eval("CUSTOMER_CODE") %>'
                                    CommandArgument="<%# Container.DataItemIndex %>" CausesValidation="False">Edit</asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="CUSTOMER_NAME" HeaderText="Cust Name" />
                        <asp:BoundField DataField="CUSTOMER_CHAIN_CODE" HeaderText="Channel" />
                        <asp:BoundField DataField="AMS" HeaderText="AMS" DataFormatString="{0:0,0;#;-}" />
                        <asp:BoundField DataField="NUMBER_PHOTO" HeaderText="No. of photo" />
                        <asp:BoundField DataField="LEVEL_1_REVIEW_DESC" HeaderText="Reviewing approve" />
                        <asp:BoundField DataField="DISTRIBUTOR_DESC" HeaderText="Distributor" />
                        <asp:BoundField DataField="REGION_CODE" HeaderText="Region" />
                        <asp:BoundField DataField="AREA_CODE" HeaderText="Area" />
                        <asp:BoundField DataField="REGION_CODE" HeaderText="" />
                        <asp:BoundField DataField="AREA_CODE" HeaderText="" />
                        <asp:BoundField DataField="DISTRIBUTOR_DESC" HeaderText="" />
                        <asp:BoundField DataField="SALES_DESC" HeaderText="" />
                        <asp:BoundField DataField="CUSTOMER_DESC" HeaderText="" />
                        <asp:BoundField DataField="CUSTOMER_ADDRESS" HeaderText="" />
                        <asp:BoundField DataField="TRADE_PROGRAM_DESC" HeaderText="" />
                        <asp:BoundField DataField="LEVEL_1_REVIEW" HeaderText="" />
                        <asp:BoundField DataField="LEVEL_1_REVIEW" HeaderText="" />
                    </Columns>
                </asp:GridView>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="P5sLbtnRemovePictures" />
                <asp:PostBackTrigger ControlID="P5sDdlMainPaging" />
            </Triggers>
        </asp:UpdatePanel>
    </form>
</asp:Content>
