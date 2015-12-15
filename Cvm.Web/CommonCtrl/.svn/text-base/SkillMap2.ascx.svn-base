<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SkillMap2.ascx.cs" Inherits="Cvm.Web.CommonCtrl.SkillMap2" %>
<%@ Import Namespace="Cvm.Web.Navigation" %>
<%@ Import Namespace="Iesi.Collections.Generic" %>
<%@ Import Namespace="Napp.Backend.DataAccess" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="Napp.Backend.Hibernate" %>
<%@ Import Namespace="Napp.Web.Navigation" %>
<%@ Import Namespace="System.Collections.Generic" %>
<%@ Import Namespace="System.Linq" %>

<script runat="server">

</script>


  <%="<script language='javascript'>" %>


       var redraw;
       var height = 500;
       var width = 700;

       /* only do all this when document has finished loading (needed for RaphaelJS */
       $(document).ready(function () {

           var g = new Graph();
           <asp:Literal runat="server" ID="GraphLit" />
                


           /* layout the graph using the Spring layout implementation */
           var layouter = new Graph.Layout.Spring(g);
           layouter.layout();

           /* draw the graph using the RaphaelJS draw implementation */
           var renderer = new Graph.Renderer.Raphael('canvas', g, width, height);
           renderer.draw();

           redraw = function () {
               layouter.layout();
               renderer.draw();
           };
       });

  <%="</script>" %>

<div id="canvas"></div>
