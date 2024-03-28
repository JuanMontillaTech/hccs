 <script>

import moment from "moment/moment";
import numbro from "numbro";

export default {
  layout: "authWh",
  head() {
    return {
      title: ` Sistema Contable`,
    };
  },


  props: {
    customLinks: {
      type: Array,
      require: true,
    },

    includeNewOption: {
      type: Boolean,
      require: true,
      default: false,
    },
    isCancel: {
      type: Boolean,
      require: true,
      default: false,
    },
  },
  data() {
    return {
      DateStart: new Date().toISOString().substr(0, 10),
      DateEnd: new Date().toISOString().substr(0, 10),

      tableData: [],
      isBusy: false,
      currentPage: 1,
      totalRows: 0,
      perPage: 10,
      pageOptions: [10, 25, 50, 100],
      filter: "",
      filterOn: [],
      sortBy: "Number",
      sortDesc: false,
      fields: [{
        label: 'Entidad',
        key: 'sysCompany.companyName',
        sortable: true,
      },"Acciones"],


    };
  },
  computed: {
    rows() {
      return this.tableData.length;
    },
  },
  watch: {
    filter: function (val) {
      if (val === "" || val.length === 0) {
        this.myProvider(this.currentPage);
      }
      if (val.length >= 3) {
        this.myProvider(this.currentPage);
      }
    },
    perPage: function (val) {
      this.myProvider(this.currentPage);
    },

  },

  mounted() {
    this.myProvider(this.currentPage);


  },
  methods: {
    FormatMoney(globalTotal) {
      return numbro(globalTotal).format("$0,0.00");
    },
    FormatDate(date) {
      return moment(date).lang("es").format("DD/MM/YYYY");
    },

    myProvider: async function (page) {

      this.isBusy = true;
      if (this.perPage === 0) this.perPage = 10;
      if (this.currentPage === 0) this.currentPage = 1;
      let url = `SysUserCompany/GetFilterUser?PageNumber=${page}&PageSize=${this.perPage}&Search=${this.filter}`;

      this.$axios
        .get(url)
        .then((response) => {
          this.tableData = [];
          this.isBusy = false;

          this.tableData = response.data.data.data;


          this.currentPage = response.data.data.pageNumber;
          this.totalRows = response.data.data.totalPages;
          this.tableData.length = response.data.data.totalRecords;
          this.perPage = response.data.data.pageSize;
          if (this.perPage === 0) this.perPage = 10;
          if (this.currentPage === 0) this.currentPage = 1;
          return response.data.data.data;
        })
        .catch((error) => {

        });
    },

    goToUrl(id, CompanyName) {

      let url = "Security/GetTokenWith?Companyid=" + id;

      this.$axios
        .get(url)
        .then((response) => {

          const token = response.data.data;

          this.$axios.setToken(token, "Bearer");
          this.$axios.setHeader("Authorization", token);
          localStorage.setItem("authUser", token);
          localStorage.setItem("token", token);
          localStorage.setItem("Authorization", token);
          localStorage.setItem("CompanyName", CompanyName);


        this.$router.push("/SetRoll");



        })
        .catch((error) => {

        });


    },
  },
};
</script>

<template>
  <div  >


    <h4 class="card-title">Seleccione la entidad a trabajar</h4>
    <div class="row  mb-1"  >


      <div class="col-12">
        <div class="card">
          <div class="card-body">
            <h4 class="card-title">Lista de Entidades</h4>

            <div class="row mt-4">
              <div class="col-sm-12 col-md-6">
                <div id="tickets-table_length" class="dataTables_length">
                  <label class="d-inline-flex align-items-center">
                    Mostrar&nbsp;
                    <b-form-select
                      v-model="perPage"
                      size="sm"
                      :options="pageOptions"
                    >
                    </b-form-select
                    >&nbsp;entradas
                  </label>
                </div>
              </div>

              <div class="col-sm-12 col-md-6">
                <div
                  id="tickets-table_filter"
                  class="dataTables_filter text-md-end"
                >
                  <label class="d-inline-flex align-items-center">
                    Buscar:
                    <b-form-input
                      v-model="filter"
                      type="search"
                      class="form-control form-control-sm ml-2"
                    ></b-form-input>
                  </label>
                </div>
              </div>
            </div>

            <div class="table-responsive mb-0">
              <b-table
                :items="tableData"
                :fields="fields"

                responsive="sm"
                :busy="isBusy"

              >
                <template #table-busy>
                  <h2 class="text-center text-primary my-2">
                    <b-spinner class="align-middle"></b-spinner>
                    <strong>Cargando...</strong>
                  </h2>
                </template>

                <template #cell(Acciones)="data">
                  <ul class="list-inline mb-0">



                    <li class="list-inline-item">

                      <a

                        class="px-2 text-success"
                        v-b-tooltip.hover
                        title="Ir "
                        @click="goToUrl(data.item.sysCompanyId, data.item.sysCompany.companyName)"
                      >
                        <i class="fa fa-arrow-right font-size-16"></i> {{data.item.sysCompany.companyName}}
                      </a>
                    </li>
                  </ul>
                </template>
              </b-table>
            </div>
            <div class="row">
              <div class="col">
                <div
                  class="dataTables_paginate paging_simple_numbers float-end"
                >
                  <ul class="pagination pagination-rounded mb-0">
                    <b-pagination
                      v-model="currentPage"
                      :total-rows="rows"
                      :per-page="perPage"
                      @change="myProvider"
                    ></b-pagination>
                  </ul>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>



