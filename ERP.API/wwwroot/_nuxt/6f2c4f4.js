(window.webpackJsonp=window.webpackJsonp||[]).push([[109,42],{589:function(t,e,l){"use strict";l.r(e);var n={components:{},props:{title:{type:String,default:""},items:{type:Array,default:function(){return[]}}}},r=l(18),component=Object(r.a)(n,(function(){var t=this,e=t._self._c;return e("div",{staticClass:"row"},[e("div",{staticClass:"col-12"},[e("div",{staticClass:"page-title-box d-flex align-items-center justify-content-between"},[e("h4",{staticClass:"mb-0"},[t._v(t._s(t.title))]),t._v(" "),e("div",{staticClass:"page-title-right"},[e("b-breadcrumb",{staticClass:"m-0",attrs:{items:t.items}})],1)])])])}),[],!1,null,null,null);e.default=component.exports},639:function(t,e,l){t.exports=l.p+"img/avatar-2.02aea0c.jpg"},652:function(t,e,l){t.exports=l.p+"img/avatar-7.9524b09.jpg"},653:function(t,e,l){t.exports=l.p+"img/avatar-8.bc11bfd.jpg"},881:function(t,e,l){"use strict";l.r(e);l(4),l(2),l(25);var n={head:function(){return{title:"".concat(this.title," | Nuxtjs Responsive Bootstrap 5 Admin Dashboard")}},data:function(){return{title:"User List",items:[{text:"Contacts"},{text:"User List",active:!0}],userList:[{id:1,profile:l(639),name:"Simon Ryles",position:"Full Stack Developer",email:"SimonRyles@minible.com"},{id:2,profile:l(283),name:"Marion Walker",position:"Frontend Developer",email:"MarionWalker@minible.com"},{id:3,name:"Frederick White",position:"Frontend Developer",email:"MarionWalker@minible.com"},{id:4,profile:l(185),name:"Shanon Marvin",position:"Backend Developer",email:"ShanonMarvin@minible.com"},{id:5,name:"Mark Jones",position:"Frontend Developer",email:"MarkJones@minible.com"},{id:6,profile:l(652),name:"Patrick Petty",position:"UI/UX Designer",email:"PatrickPetty@minible.com"},{id:7,profile:l(653),name:"Marilyn Horton",position:"Backend Developer",email:"MarilynHorton@minible.com"},{id:8,profile:l(639),name:"Neal Womack",position:"Full Stack Developer",email:"NealWomack@minible.com"},{id:9,profile:l(639),name:"Steven Williams",position:"Frontend Developer",email:"StevenWilliams@minible.com"}],totalRows:1,currentPage:1,perPage:10,pageOptions:[10,25,50,100],filter:null,filterOn:[],sortBy:"age",sortDesc:!1,fields:[{key:"check",label:""},{key:"name"},{key:"position"},{key:"email"},"action"]}},computed:{rows:function(){return this.userList.length}},mounted:function(){this.totalRows=this.items.length},methods:{onFiltered:function(t){this.totalRows=t.length,this.currentPage=1}},middleware:"authentication"},r=l(18),component=Object(r.a)(n,(function(){var t=this,e=t._self._c;return e("div",[e("PageHeader",{attrs:{title:t.title,items:t.items}}),t._v(" "),e("div",{staticClass:"row"},[e("div",{staticClass:"col-12"},[e("div",{staticClass:"card"},[e("div",{staticClass:"card-body"},[e("div",{staticClass:"row mt-4"},[e("div",{staticClass:"col-sm-12 col-md-6"},[e("div",{staticClass:"dataTables_length",attrs:{id:"tickets-table_length"}},[e("label",{staticClass:"d-inline-flex align-items-center"},[t._v("\r\n                                    Show \r\n                                    "),e("b-form-select",{attrs:{size:"sm",options:t.pageOptions},model:{value:t.perPage,callback:function(e){t.perPage=e},expression:"perPage"}}),t._v(" entries\r\n                                ")],1)])]),t._v(" "),e("div",{staticClass:"col-sm-12 col-md-6"},[e("div",{staticClass:"dataTables_filter text-md-end",attrs:{id:"tickets-table_filter"}},[e("label",{staticClass:"d-inline-flex align-items-center"},[t._v("\r\n                                    Search:\r\n                                    "),e("b-form-input",{staticClass:"form-control form-control-sm ms-2",attrs:{type:"search"},model:{value:t.filter,callback:function(e){t.filter=e},expression:"filter"}})],1)])])]),t._v(" "),e("div",{staticClass:"table-responsive mb-0"},[e("b-table",{staticClass:"table table-centered table-nowrap",attrs:{items:t.userList,fields:t.fields,responsive:"sm","per-page":t.perPage,"current-page":t.currentPage,"sort-by":t.sortBy,"sort-desc":t.sortDesc,filter:t.filter,"filter-included-fields":t.filterOn},on:{"update:sortBy":function(e){t.sortBy=e},"update:sort-by":function(e){t.sortBy=e},"update:sortDesc":function(e){t.sortDesc=e},"update:sort-desc":function(e){t.sortDesc=e},filtered:t.onFiltered},scopedSlots:t._u([{key:"cell(check)",fn:function(data){return[e("div",{staticClass:"custom-control custom-checkbox"},[e("input",{staticClass:"custom-control-input",attrs:{type:"checkbox",id:"contacusercheck".concat(data.item.id)}}),t._v(" "),e("label",{staticClass:"custom-control-label",attrs:{for:"contacusercheck".concat(data.item.id)}})])]}},{key:"cell(name)",fn:function(data){return[data.item.profile?e("img",{staticClass:"avatar-xs rounded-circle me-2",attrs:{src:data.item.profile,alt:""}}):t._e(),t._v(" "),data.item.profile?t._e():e("div",{staticClass:"avatar-xs d-inline-block me-2"},[e("div",{staticClass:"avatar-title bg-soft-primary rounded-circle text-primary"},[e("i",{staticClass:"mdi mdi-account-circle m-0"})])]),t._v(" "),e("a",{staticClass:"text-body",attrs:{href:"#"}},[t._v(t._s(data.item.name))])]}},{key:"cell(action)",fn:function(){return[e("ul",{staticClass:"list-inline mb-0"},[e("li",{staticClass:"list-inline-item"},[e("a",{directives:[{name:"b-tooltip",rawName:"v-b-tooltip.hover",modifiers:{hover:!0}}],staticClass:"px-2 text-primary",attrs:{href:"javascript:void(0);",title:"Edit"}},[e("i",{staticClass:"uil uil-pen font-size-18"})])]),t._v(" "),e("li",{staticClass:"list-inline-item"},[e("a",{directives:[{name:"b-tooltip",rawName:"v-b-tooltip.hover",modifiers:{hover:!0}}],staticClass:"px-2 text-danger",attrs:{href:"javascript:void(0);",title:"Delete"}},[e("i",{staticClass:"uil uil-trash-alt font-size-18"})])]),t._v(" "),e("b-dropdown",{staticClass:"list-inline-item",attrs:{variant:"white",right:"","toggle-class":"text-muted font-size-18 px-2"},scopedSlots:t._u([{key:"button-content",fn:function(){return[e("i",{staticClass:"uil uil-ellipsis-v"})]},proxy:!0}])},[t._v(" "),e("a",{staticClass:"dropdown-item",attrs:{href:"#"}},[t._v("Action")]),t._v(" "),e("a",{staticClass:"dropdown-item",attrs:{href:"#"}},[t._v("Another action")]),t._v(" "),e("a",{staticClass:"dropdown-item",attrs:{href:"#"}},[t._v("Something else here")])])],1)]},proxy:!0}])})],1),t._v(" "),e("div",{staticClass:"row"},[e("div",{staticClass:"col"},[e("div",{staticClass:"dataTables_paginate paging_simple_numbers float-end"},[e("ul",{staticClass:"pagination pagination-rounded mb-0"},[e("b-pagination",{attrs:{"total-rows":t.rows,"per-page":t.perPage},model:{value:t.currentPage,callback:function(e){t.currentPage=e},expression:"currentPage"}})],1)])])])])])])])],1)}),[],!1,null,null,null);e.default=component.exports;installComponents(component,{PageHeader:l(589).default})}}]);