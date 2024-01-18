<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="YakaKarti.aspx.cs" Inherits="ArcadiasDavet_Web.Yazici.YakaKarti" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <script>
        window.addEventListener('afterprint', (event) => {
            this.close();
        });
    </script>
</head>
<body>
    <div style="position:absolute; top:0; left:0; width:1000px; height:1414px; font-weight:bold; font-family: Arial">
        <div id="div_Cerceve" runat="server"></div>
    </div>
    
    <form id="form1" runat="server"></form>
</body>
</html>