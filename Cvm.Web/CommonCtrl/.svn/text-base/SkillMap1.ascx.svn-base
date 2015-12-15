<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SkillMap1.ascx.cs" Inherits="Cvm.Web.CommonCtrl.SkillMap1" %>
<%@ Import Namespace="Napp.VeryBasic" %>
<%="<script>" %>
    var chart;
    $(document).ready(function () {
        chart = new Highcharts.Chart({
            chart: {
                renderTo: 'mapDiv',
                defaultSeriesType: 'column'
            },
            title: {
                text: 'CV Navigator Skills - Total years of experience'
            },
            subtitle: {
                text: ''
            },
            plotOptions: {
                column: {
                    /*pointPadding: 0.5,
                    borderWidth: 0,
                    lineWidth: 5*/
                }
            },

            /*legend: {
            layout: 'vertical',
            backgroundColor: Highcharts.theme.legendBackgroundColor || '#FFFFFF',
            align: 'left',
            verticalAlign: 'top',
            x: 100,
            y: 70,
            floating: true,
            shadow: true
            },*/
            /*tooltip: {
            formatter: function () {
            return '' +
            this.x + ': ' + this.y + ' mm';
            }
            },*/
            xAxis: {
                  labels: {
                      rotation: 90,
                      align:'left'
                },
                categories: [
                <%=CollectionExtensions.ConcatRowsToJsStringArray(this.MySkillMap.Rows, "SkillName") %>
         ]
            },
            yAxis: {
                min: 0,
                title: {
                    text: 'Total years of experience'
                }
            },
            series: [{
                name: 'XP',
                data: [<%=CollectionExtensions.ConcatRowsToJsIntArray(this.MySkillMap.Rows, "Total") %>]
            }]
        });


    });
<%="</script>" %>
<div id="mapDiv"  style="width:<%=this.DivWidth%>px"></div>