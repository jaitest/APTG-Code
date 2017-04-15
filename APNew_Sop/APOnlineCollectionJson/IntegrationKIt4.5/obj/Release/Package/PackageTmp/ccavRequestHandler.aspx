<%@ Page Language="C#" AutoEventWireup="true" Inherits="SubmitData" Codebehind="ccavRequestHandler.aspx.cs" %>
  
<html>
<head id="Head1" runat="server">
<script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1.10.2/jquery.min.js"></script>  
        <script type="text/javascript">
           $(document).ready(function () {
               $("#nonseamless").submit();
           });
       </script>
    <title>
    </title>
</head>
<body>
    <form id="nonseamless" method="post" name="redirect" action="https://secure.ccavenue.com/transaction/transaction.do?command=initiateTransaction"> 
        <input type="hidden" id="encRequest" name="encRequest" value="<%=strEncRequest%>"/>
        <input type="hidden" name="access_code" id="Hidden1" value="<%=strAccessCode%>"/>
    </form>
</body>
</html>
