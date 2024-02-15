<script>
import Swal from "sweetalert2";

import StarRating from "vue-star-rating";
import moment from "moment/moment";
import numbro from "numbro";

export default {
  head() {
    return {
      title: ` Sistema Contable`,
    };
  },
  components: {
    StarRating,
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
      DataForm: {},
      CheckDate: true,
      showModal: false,
      showModalRating: false,
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
      fields: ["Acciones"],
      PageEdit: "",
      printPage: "/ExpressForm/Ticket",
      PageShow: "Detail",
      PageCreate: "",
      PageDelete: "Delete/",

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
    CheckDate: function (val) {
      this.myProvider(this.currentPage);
    }, DateStart: function (val) {
      this.myProvider(this.currentPage);
    },
    DateEnd: function (val) {
      this.myProvider(this.currentPage);
    },
    "$route.query.Form"() {
      this.CheckDate = true;
      this.GetForm();
    },
    perPage: function (val) {
      this.myProvider(this.currentPage);
    },
  },

  mounted() {
    this.GetForm();


  },
  methods: {
    FormatMoney(globalTotal) {
      return numbro(globalTotal).format("$0,0.00");
    },
    FormatDate(date) {
      return moment(date).lang("es").format("DD/MM/YYYY");
    },

    GetForm() {
      this.FormId = this.$route.query.Form;

      var url = `Form/GetById?Id=${this.FormId}`;
      this.DataForm = {};
      this.$axios
        .get(url)
        .then((response) => {

          this.DataForm = response.data.data;

          if (this.DataForm.formCode === "FEX") {
            this.PageEdit = "/ExpressForm/FuncionalFormExpress";
            this.PageCreate = "/ExpressForm/FuncionalFormExpress";

            this.PageShow === "";
          }
          if (this.DataForm.formCode === "EX") {
            this.PageEdit = "/ExpressForm/CreateOfEdit";
            this.PageCreate = "/ExpressForm/CreateOfEdit";

            this.PageShow === "";
          }
          if (this.DataForm.formCode === "CPX") {
            this.PageEdit = "/ExpressForm/FormComplex";
            this.PageCreate = "/ExpressForm/FormComplex";

            this.PageShow === "";
          }
          if (this.DataForm.formCode === "REC-") {
            this.PageEdit = "/ReciboIngreso";
            this.PageCreate = "/ReciboIngreso";

            this.PageShow === "";
          }

          this.GetFilds();
        })
        .catch((error) => {

          this.$toast.error(`${error}`, "ERROR", this.izitoastConfig);
        });
    },
    GetFilds() {
      this.$axios
        .get(`Formfields/GetByFormId/${this.FormId}`)
        .then((response) => {

          (this.fields = []),
            (this.fields = ["Acciones"]),
            response.data.data.map((schema) => {

              if (schema.isActive && schema.showList) {

                switch (schema.type) {
                  case 9:
                    this.fields.push({
                      label: schema.label,
                      key: schema.field,
                      formatter: (value, key, item) => {
                        return this.FormatDate(value)
                      },
                      sortable: true,
                    });
                    break;
                  case 2:
                    this.fields.push({
                      label: schema.label,
                      key: schema.field,
                      formatter: (value, key, item) => {
                        return this.FormatMoney(value)
                      },
                      sortable: true,
                    });
                    break;
                  default:
                    this.fields.push({
                      label: schema.label,
                      key: schema.field,
                      sortable: true,
                    });


                }

              }

            });

          this.myProvider(this.currentPage);
        })
        .catch((error) => {
          this.$toast.error(`${error}`, "ERROR", this.izitoastConfig);
        });


    },
    Tranfers(Id, TransactionsType, FormId) {
      let url = `Transaction/CreateChangeType?id=${Id}&transactionsType=${TransactionsType}&formId=${FormId}`;
      let result = null;

      this.$axios
        .post(url)
        .then((response) => {

          result = response;
          this.$toast.success(
            "El Registro ha sido creado correctamente.",
            "ÉXITO"
          );

        })
        .catch((error) => {
          result = error;
          this.$toast.error(`${result}`, "ERROR", this.izitoastConfig);
        });
    },

    editModalSchema(id) {
      this.$router.push({
        path: `${this.PageEdit}`,
        query: {
          Action: "edit",
          Form: this.DataForm.id,
          Id: id,
        },
      });
    },
    myProvider: async function (page) {

      this.isBusy = true;
      if (this.perPage === 0) this.perPage = 10;
      if (this.currentPage === 0) this.currentPage = 1;
      let url = `${this.DataForm.controller}/GetFilter?PageNumber=${page}&PageSize=${this.perPage}&Search=${this.filter}`;
      if (this.DataForm.formCode === "FEX") {
        url = `Transaction/GetFilter?PageNumber=${page}&PageSize=${this.perPage}&Search=${this.filter}
        &transactionsTypeId=${this.DataForm.transactionsType}&dateStart=${this.DateStart}&dateEnd=${this.DateEnd}&valideFilter=${this.CheckDate}`;
      }
       if(this.DataForm.formCode === "REC-"){
        url = `TransactionReceipt/GetFilter?PageNumber=${page}&PageSize=${this.perPage}&Search=${this.filter}
        &dateStart=${this.DateStart}&dateEnd=${this.DateEnd}&valideFilter=${this.CheckDate}`;
      }
      this.$axios
        .get(url)
        .then((response) => {
          this.tableData = [];
          this.isBusy = false;
          this.tableData = response.data.data.data;
          console.log(this.tableData)
          console.log(this.fields)
          this.currentPage = response.data.data.pageNumber;
          this.totalRows = response.data.data.totalPages;
          this.tableData.length = response.data.data.totalRecords;
          this.perPage = response.data.data.pageSize;
          if (this.perPage === 0) this.perPage = 10;
          if (this.currentPage === 0) this.currentPage = 1;
          return response.data.data.data;
        })
        .catch((error) => {
          this.$toast.error(`${error}`, "ERROR", this.izitoastConfig);
        });
    },

    goToUrl(urlToGo, id) {

      let url = urlToGo + "?id=" + id;
      this.$router.push({
        path: url,
      });
    },
    newRequest() {
      this.$router.push({
        path: `${this.PageCreate}`,
        query: {
          Form: this.DataForm.id,
          Action: "create",
          id: null,
        },
      });
    },

    goInvoiceRecipe(id) {

      this.$router.push({
        path: `/ExpressForm/FormReceipt?Type=${this.DataForm.transactionsType}&Form=${this.FormId}&Id=${id}`,
      });
    },

    printForm(id) {

      if(this.DataForm.formCode === "REC-"){
        this.$router.push({
          path: `/ExpressForm/TicketRecipeIncome?Action=print&Form=${this.FormId}&Id=${id}`,
        });
      }
      else{
        this.$router.push({
          path: `/ExpressForm/Ticket?Action=print&Form=${this.FormId}&Id=${id}`,
        });
      }
    },
    requestRating() {
      this.showModalRating = true;
    },
    onFiltered(filteredItems) {

    },
    handleSubmit() {

    },
    confirmCancellation(id) {
      let url = "";
      if (this.DataForm.formCode === "FEX") {

        url = `Transaction/${this.PageDelete}${id}`;

      } else {

        url = `${this.DataForm.controller}/${this.PageDelete}${id}`;
      }


      Swal.fire({
        title: "estas seguro?",
        text: "esta seguro que quiere remover esta fila",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Si , Remuévela!",
        cancelButtonText: 'Cancelar',
      }).then((result) => {
        if (result.isConfirmed) {
          this.$axios
            .delete(url)
            .then((response) => {
              Swal.fire("Removido!", "El registro esta removido.", "success");
              this.GetFilds();
            })
            .catch((error) => alert(error));
        }
      });
    },
  },
};
</script>

<template>
  <div>

    <b-modal
      v-model="showModalRating"
      title-class="text-black font-18"
      body-class="p-3"
      hide-footer
      size="sm"
      id="modal-rating"
      centered
    >
      <div class="col-xl-12 col-md-12 col-sm-12">
        <div class="p-4 text-center">
          <h4 class="mb-3">Califica tú solicitud de información</h4>
          <star-rating
            :star-size="25"
            :border-width="4"
            border-color="#d8d8d8"
            :rounded-corners="true"
            :star-points="[
              23, 2, 14, 17, 0, 19, 10, 34, 7, 50, 23, 43, 38, 50, 36, 34, 46,
              19, 31, 17,
            ]"
          ></star-rating>
        </div>
        <div class="text-center">
          <button class="btn btn-success">Calificar</button>
          <button class="btn btn-danger" @click="$bvModal.hide('modal-rating')">
            Cancelar
          </button>
        </div>
      </div>
    </b-modal>
    <b-modal
      v-model="showModal"
      title-class="text-black font-18"
      body-class="p-3"
      hide-footer
      size="xl"
      id="create-modal-request"
    >
      <RequestForm :action="2"/>
    </b-modal>
    <div class="row">
      <div class="col-md-4" v-if="includeNewOption">
        <div>
          <button
            type="button"
            class="btn btn-success mb-3"
            @click="newRequest()"
          >
            <i class="far fa-file-alt"></i> Nuevo registro
          </button>
        </div>

      </div>


      <div class="col-12">
        <div class="card">
          <div class="card-body">
            <h4 class="card-title">Lista de {{ DataForm.title }}</h4>
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
                  <label class="d-inline-flex align-items-center" v-if="DataForm.formCode === 'FEX'">
                    <b-form-checkbox
                      id="checkbox-1"
                      v-model="CheckDate"
                      name="checkbox-1"

                    >
                      Filtro
                    </b-form-checkbox>


                  </label>
                  <label class="d-inline-flex align-items-center" v-if="DataForm.formCode === 'FEX' &&  CheckDate ">

                    Fecha
                    <b-form-input

                      v-model="DateStart"
                      type="date"
                      class="form-control form-control-sm  "
                    ></b-form-input>
                  </label>
                  <label class="d-inline-flex align-items-center" v-if="DataForm.formCode === 'FEX' &&  CheckDate">
                    Hasta:
                    <b-form-input v-if="CheckDate"
                                  v-model="DateEnd"
                                  type="date"
                                  class="form-control form-control-sm "
                    ></b-form-input>
                  </label>
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
                @filtered="onFiltered"
              >
                <template #table-busy>
                  <h2 class="text-center text-primary my-2">
                    <b-spinner class="align-middle"></b-spinner>
                    <strong>Cargando...</strong>
                  </h2>
                </template>

                <template #cell(Acciones)="data">
                  <ul class="list-inline mb-0">

                    <li class="list-inline-item" v-if="DataForm.print">

                      <a

                        class="px-2 text-primary"
                        v-b-tooltip.hover
                        title="Imprimir"
                        @click="printForm(data.item.id)"
                      >
                        <i class="uil uil-print font-size-18"></i>
                      </a>
                    </li>
                    <li class="list-inline-item" v-if=" DataForm.transactionsType === 5">

                      <a

                        class="px-2 text-primary"
                        v-b-tooltip.hover
                        @click="goInvoiceRecipe(data.item.id)"

                      >
                        <i class="fas fa-file-invoice"></i>

                      </a>
                    </li>
                    <span v-for="item in customLinks" :key="item.key">
                      <li class="list-inline-item">

                        <a
                          :class="item.styleIcon"
                          v-b-tooltip.hover
                          :title="item.title"
                          @click="goToUrl(item.link, data.item.id)"
                        >
                          <i :class="item.icon"></i>
                        </a>
                      </li>
                    </span>
                    <li class="list-inline-item">

                      <a

                        class="px-2 text-primary"
                        v-b-tooltip.hover
                        title="Editar"
                        @click="editModalSchema(data.item.id)"
                      >
                        <i class="uil uil-pen font-size-18"></i>

                      </a>
                    </li>
                    <li class="list-inline-item" v-if="DataForm.transactionsType === 8">

                      <a

                        class="px-2 text-success"
                        v-b-tooltip.hover
                        title="Transferir"
                        @click="Tranfers(data.item.id,6,  '25F94E8C-8EA0-4EE0-ADF5-02149A0E080B') "
                      >
                        <i class="uil uil-eye font-size-18"></i>
                      </a>
                    </li>

                    <li class="list-inline-item">

                      <a

                        class="px-2 text-danger"
                        v-b-tooltip.hover
                        title="Cancelar solicitud"
                        @click="confirmCancellation(data.item.id)"
                      >
                        <i class="far fa-times-circle font-size-16"></i>
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
