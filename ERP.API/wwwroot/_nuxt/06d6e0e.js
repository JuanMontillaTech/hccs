(window.webpackJsonp=window.webpackJsonp||[]).push([[99,42],{589:function(t,e,r){"use strict";r.r(e);var n={components:{},props:{title:{type:String,default:""},items:{type:Array,default:function(){return[]}}}},o=r(18),component=Object(o.a)(n,(function(){var t=this,e=t._self._c;return e("div",{staticClass:"row"},[e("div",{staticClass:"col-12"},[e("div",{staticClass:"page-title-box d-flex align-items-center justify-content-between"},[e("h4",{staticClass:"mb-0"},[t._v(t._s(t.title))]),t._v(" "),e("div",{staticClass:"page-title-right"},[e("b-breadcrumb",{staticClass:"m-0",attrs:{items:t.items}})],1)])])])}),[],!1,null,null,null);e.default=component.exports},907:function(t,e,r){"use strict";r.r(e);r(25);var n=r(30),o=(r(75),{head:function(){return{title:"Reporte de ".concat(this.title)}},data:function(){return{name:"MayorGeneral",title:"Mayor General",items:[{text:"Reportes"},{text:"Mayor General",active:!0}],totales:{},headers:[{key:"name",label:"Cuenta"},{key:"totalDebit",label:"Débito"},{key:"totalCredit",label:"Crédito"},{key:"debtor",label:"Deudor"},{key:"creditor",label:"Acreedor"}],itemsData:[],izitoastConfig:{position:"topRight"}}},created:function(){this.getAll(),this.getTotals()},methods:{getAll:function(){var t=this;return Object(n.a)(regeneratorRuntime.mark((function e(){return regeneratorRuntime.wrap((function(e){for(;;)switch(e.prev=e.next){case 0:"Journal/MajorGeneral",t.$axios.get("Journal/MajorGeneral",{headers:{"Content-Type":"application/json"}}).then((function(e){console.log(e.data.data),t.itemsData=e.data.data})).catch((function(e){t.$toast.error("".concat(e),"ERROR",t.izitoastConfig)}));case 2:case"end":return e.stop()}}),e)})))()},getTotals:function(){var t=this;return Object(n.a)(regeneratorRuntime.mark((function e(){return regeneratorRuntime.wrap((function(e){for(;;)switch(e.prev=e.next){case 0:"Journal/Totals",t.$axios.get("Journal/Totals",{headers:{"Content-Type":"application/json"}}).then((function(e){t.totales=e.data.data,console.log(e.data.data)})).catch((function(e){t.$toast.error("".concat(e),"ERROR",t.izitoastConfig)}));case 2:case"end":return e.stop()}}),e)})))()},printReport:function(){var t=window.open("","PRINT","height=800,width=1200");return t.document.write("<html><head><title>"+document.title+"</title>"),t.document.write("</head><body >"),t.document.write('<link href="https://cdn.jsdelivr.net/npm/bootstrap@5.1.3/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-1BmE4kWBq78iYhFldvKuhfTAU6auU8tT94WrHftjDbrCEXSU1oBoqyl2QvZ6jIW3" crossorigin="anonymous">'),t.document.write("<h1>"+document.title+"</h1>"),t.document.write(document.getElementById("report").innerHTML),t.document.write("</body></html>"),t.document.close(),t.focus(),t.print(),t.close(),!0}}}),l=r(18),component=Object(l.a)(o,(function(){var t=this,e=t._self._c;return e("div",[e("PageHeader",{attrs:{title:t.title,items:t.items}}),t._v(" "),e("div",{staticClass:"btn-group pb-2",attrs:{role:"group","aria-label":"Basic example"}},[e("a",{staticClass:"btn btn-primary btn-sm text-white",on:{click:function(e){return t.printReport()}}},[e("i",{staticClass:"uil-print me-2"}),t._v("Imprimir\n          ")]),t._v(" "),e("a",{staticClass:"btn btn-success btn-sm text-white",on:{click:function(e){return t.downloadExcel()}}},[e("i",{staticClass:"bx bx-spreadsheet me-2"}),t._v("Excel\n          ")])]),t._v(" "),t.itemsData.length>0?e("div",{attrs:{id:"report"}},[e("div",{staticClass:"row",staticStyle:{display:"flex","justify-content":"space-around","margin-bottom":"20px"}},[this.totales.totalDebit>0?e("div",{staticClass:"col-lg-3"},[e("b-card",{staticClass:"border border-success",attrs:{"header-class":"bg-transparent border-success"},scopedSlots:t._u([{key:"header",fn:function(){return[e("h5",{staticClass:"my-0 text-success"},[e("i",{staticClass:"uil-arrow-growth me-3"}),t._v("Total Debito\n          ")])]},proxy:!0}],null,!1,3778208645)},[t._v(" "),e("h3",{},[t._v("$ "+t._s(this.totales.totalDebit))])])],1):e("div",{staticClass:"col-lg-3"},[e("b-card",{staticClass:"border border-danger",attrs:{"header-class":"bg-transparent border-danger"},scopedSlots:t._u([{key:"header",fn:function(){return[e("h5",{staticClass:"my-0 text-danger"},[e("i",{staticClass:"mdi mdi-block-helper me-3"}),t._v("Total Debito\n          ")])]},proxy:!0}],null,!1,2785284458)},[t._v(" "),e("h3",{},[t._v("$ "+t._s(this.totales.totalDebit))])])],1),t._v(" "),this.totales.totalCredit>0?e("div",{staticClass:"col-lg-3"},[e("b-card",{staticClass:"border border-success",attrs:{"header-class":"bg-transparent border-success"},scopedSlots:t._u([{key:"header",fn:function(){return[e("h5",{staticClass:"my-0 text-success"},[e("i",{staticClass:"uil-arrow-growth me-3"}),t._v("Total Credito\n          ")])]},proxy:!0}],null,!1,1430554902)},[t._v(" "),e("h3",{},[t._v("$ "+t._s(this.totales.totalCredit))])])],1):e("div",{staticClass:"col-lg-3"},[e("b-card",{staticClass:"border border-danger",attrs:{"header-class":"bg-transparent border-danger"},scopedSlots:t._u([{key:"header",fn:function(){return[e("h5",{staticClass:"my-0 text-danger"},[e("i",{staticClass:"mdi mdi-block-helper me-3"}),t._v("Total Credito\n          ")])]},proxy:!0}],null,!1,3109873881)},[t._v(" "),e("h3",{},[t._v("$ "+t._s(this.totales.totalCredit))])])],1)]),t._v(" "),e("div",{staticClass:"card"},[e("div",{staticClass:"card-body"},[e("div",{staticClass:"table-responsive"},[e("table",{staticClass:"table table-striped mb-0"},[e("thead",[e("tr",t._l(t.headers,(function(r){return e("th",[t._v(t._s(r.label))])})),0)]),t._v(" "),e("tbody",t._l(t.itemsData,(function(r){return e("tr",[e("th",{attrs:{scope:"row"}},[t._v(t._s(r.name))]),t._v(" "),e("td",[t._v(t._s(r.totalDebit))]),t._v(" "),e("td",[t._v(t._s(r.totalCredit))]),t._v(" "),e("td",[t._v(t._s(r.debtor))]),t._v(" "),e("td",[t._v(t._s(r.creditor))])])})),0)])])])])]):e("div",{staticClass:"w-100 d-flex justify-content-center align-items-center snipper-h h-100"},[e("b-spinner",{staticStyle:{width:"3rem",height:"3rem"},attrs:{label:"Large Spinner"}})],1)],1)}),[],!1,null,null,null);e.default=component.exports;installComponents(component,{PageHeader:r(589).default})}}]);