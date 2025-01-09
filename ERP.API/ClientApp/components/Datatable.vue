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
      fields: [
        { key: 'Acciones', class: 'acciones-cell', variant: 'danger' }
      ],
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
    },
    DateStart: function (val) {
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


          const pageMappings = {
            FEX: {
              PageEdit: '/ExpressForm/FuncionalFormExpress',
              PageCreate: '/ExpressForm/FuncionalFormExpress',
              PageShow: ''
            },
            EX: {
              PageEdit: '/ExpressForm/CreateOfEdit',
              PageCreate: '/ExpressForm/CreateOfEdit',
              PageShow: ''
            },
            CPX: {
              PageEdit: '/ExpressForm/FormComplex',
              PageCreate: '/ExpressForm/FormComplex',
              PageShow: ''
            },
          };

          const transactionTypeMappings = {
            11: {
              PageEdit: '/ReciboIngreso',
              PageCreate: '/ReciboIngreso',
              PageShow: ''
            },
            10: {
              PageEdit: '/ReciboIngreso',
              PageCreate: '/ReciboIngreso',
              PageShow: ''
            },
          };

          if (pageMappings[this.DataForm.formCode]) {
            const mapping = pageMappings[this.DataForm.formCode];
            this.PageEdit = mapping.PageEdit;
            this.PageCreate = mapping.PageCreate;
            this.PageShow = mapping.PageShow;
          } else if (transactionTypeMappings[this.DataForm.transactionsType]) {
            const mapping = transactionTypeMappings[this.DataForm.transactionsType];
            this.PageEdit = mapping.PageEdit;
            this.PageCreate = mapping.PageCreate;
            this.PageShow = mapping.PageShow;
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
            (this.fields = [
              { key: 'Acciones', thClass: 'header-cell', class: 'acciones-cell' }
            ]),
            response.data.data.map((schema) => {
              if (schema.isActive && schema.showList) {
                switch (schema.type) {
                  case 9:
                    this.fields.push({
                      label: schema.label,
                      key: schema.field,
                      formatter: (value, key, item) => {
                        return this.FormatDate(value);
                      },
                      sortable: true,
                    });
                    break;
                  case 2:
                    this.fields.push({
                      label: schema.label,
                      key: schema.field,
                      formatter: (value, key, item) => {
                        return this.FormatMoney(value);
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
    Tranfers(Id, FormId) {
      let url = `Transaction/CreateClonePost`;
      let result = null;
      let data = {
        id: Id,
        formId: FormId,
      };
      Swal.fire({
        title: "Quiere crear una Factura de esta cotizació?",
        text: "Si acepta esta acción se creara una factura con los datos de la cotización.",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Si , Tranferir!",
        cancelButtonText: "Cancelar",
      }).then((Cofirmation) => {
        if (Cofirmation.isConfirmed) {
          this.$axios
            .post(url, data)
            .then((response) => {
              result = response;
              this.$toast.success(
                "El Registro ha sido creado y tranferido correctamente.",
                "ÉXITO"
              );

              this.GoEdit(
                result.data.data.id,
                FormId,
                "/ExpressForm/FuncionalFormExpress",
                "edit"
              );
            })
            .catch((error) => {
              result = error;
              this.$toast.error(`${result}`, "ERROR", this.izitoastConfig);
            });
        }
      });
    },
    GoEdit(id, formid, PageEdit, Action) {
      this.$router.push({
        path: `${PageEdit}`,
        query: {
          Action: "edit",
          Form: formid,
          Id: id,
        },
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
    buildUrl(page) {
      let params = {
        PageNumber: page,
        PageSize: this.perPage,
        Search: this.filter
      };

      if (this.DataForm.formCode === "FEX") {
        params.transactionsTypeId = this.DataForm.transactionsType;
        params.dateStart = this.DateStart;
        params.dateEnd = this.DateEnd;
        params.valideFilter = this.CheckDate;
      } else if (this.DataForm.transactionsType === 11 || this.DataForm.transactionsType === 10) {
        params.dateStart = this.DateStart;
        params.dateEnd = this.DateEnd;
        params.valideFilter = this.CheckDate;
        params.typeTransaction = this.DataForm.transactionsType;
      }

      let baseUrl = `${this.DataForm.controller}/GetFilter`;
      if (this.DataForm.formCode === "FEX") {
        baseUrl = `Transaction/GetFilter`;
      } else if (this.DataForm.transactionsType === 11 || this.DataForm.transactionsType === 10) {
        baseUrl = `TransactionReceipt/GetFilter`;
      }

      const queryParams = new URLSearchParams(params);
      return `${baseUrl}?${queryParams.toString()}`;
    },
    myProvider: async function (page) {
      this.isBusy = true;
      if (this.perPage === 0) this.perPage = 10;
      if (this.currentPage === 0) this.currentPage = 1;
      const url = this.buildUrl(page, this.perPage, this.filter);


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
      switch (this.DataForm.transactionsType) {
        case 11:
          this.$router.push({
            path: `/ExpressForm/TicketRecipeIncome?Action=print&Form=${this.FormId}&Id=${id}`,
          });
        case 10:
          this.$router.push({
            path: `/ExpressForm/TicketRecipeIncome?Action=print&Form=${this.FormId}&Id=${id}`,
          });
          break;
        default:
          this.$router.push({
            path: `/ExpressForm/Ticket?Action=print&Form=${this.FormId}&Id=${id}`,
          });
      }
    },
    requestRating() {
      this.showModalRating = true;
    },
    onFiltered(filteredItems) { },
    handleSubmit() { },
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
        cancelButtonText: "Cancelar",
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
    <b-modal v-model="showModalRating" title-class="text-black font-18" body-class="p-3" hide-footer size="sm"
      id="modal-rating" centered>
      <div class="d-flex flex-column align-items-center">
        <h4 class="mb-3">Califica tu solicitud de información</h4>
        <star-rating :star-size="25" :border-width="4" border-color="#d8d8d8" :rounded-corners="true"
          :star-points="[23, 2, 14, 17, 0, 19, 10, 34, 7, 50, 23, 43, 38, 50, 36, 34, 46, 19, 31, 17]">
        </star-rating>
        <div class="mt-3">
          <button class="btn btn-success mr-2">Calificar</button>
          <button class="btn btn-danger" @click="$bvModal.hide('modal-rating')">Cancelar</button>
        </div>
      </div>
    </b-modal>

    <b-modal v-model="showModal" title-class="text-black font-18" body-class="p-3" hide-footer size="xl"
      id="create-modal-request">
      <RequestForm :action="2" />
    </b-modal>

    <div class="row">
      <div class="col-md-4" v-if="includeNewOption">
        <button v-if="DataForm.create" type="button" class="btn btn-success mb-3" @click="newRequest()">
          <i class="far fa-file-alt mr-2"></i> Nuevo registro
        </button>
      </div>

      <div class="col-12">
        <div class="card">
          <div class="card-body">
            <h4 class="card-title">Lista de {{ DataForm.title }}</h4>
            <div class="row mt-4">
              <div class="col-sm-12 col-md-6">
                <div class="d-flex align-items-center">
                  <label for="perPageSelect">Mostrar </label>
                  <b-form-select id="perPageSelect" v-model="perPage" size="sm" :options="pageOptions" class="mx-2">
                  </b-form-select>
                  <label>entradas</label>
                </div>
              </div>

              <div class="col-sm-12 col-md-6">
                <div class="d-flex align-items-center justify-content-end">
                  <div v-if="DataForm.formCode === 'FEX'" class="mr-3">
                    <b-form-checkbox id="checkbox-1" v-model="CheckDate" name="checkbox-1">
                      Filtro
                    </b-form-checkbox>
                  </div>
                  <div v-if="DataForm.formCode === 'FEX' && CheckDate" class="mr-3">
                    <label for="dateStart">Fecha </label>
                    <b-form-input id="dateStart" v-model="DateStart" type="date"
                      class="form-control form-control-sm ml-2">
                    </b-form-input>
                  </div>
                  <div v-if="DataForm.formCode === 'FEX' && CheckDate" class="mr-3">
                    <label for="dateEnd">Hasta: </label>
                    <b-form-input id="dateEnd" v-if="CheckDate" v-model="DateEnd" type="date"
                      class="form-control form-control-sm ml-2">
                    </b-form-input>
                  </div>
                  <div>
                    <label for="filterInput">Buscar: </label>
                    <b-form-input id="filterInput" v-model="filter" type="search"
                      class="form-control form-control-sm ml-2">
                    </b-form-input>
                  </div>
                </div>
              </div>
            </div>

            <div class="table-responsive mb-0">
              <b-table :items="tableData" :fields="fields" :busy="isBusy" head-variant="dark"
               hover="hover" fixed="fixed" @filtered="onFiltered">
                <template #table-busy>
                  <div class="text-center my-2">
                    <b-spinner class="align-middle mr-2"></b-spinner>
                    <strong class="text-primary">Cargando...</strong>
                  </div>
                </template>

                <template #cell(Acciones)="data">
                  <div>
                    <a v-if="DataForm.print" @click="printForm(data.item.id)" class="text-primary" v-b-tooltip.hover
                      title="Imprimir">
                      <i class="uil uil-print"></i>
                    </a>
                    <a v-if="DataForm.transactionsType === 5" @click="goInvoiceRecipe(data.item.id)"
                      class="text-primary" v-b-tooltip.hover>
                      <i class="fas fa-file-invoice"></i>
                    </a>
                    <span v-for="item in customLinks" :key="item.key">
                      <a :class="item.styleIcon" @click="goToUrl(item.link, data.item.id)" v-b-tooltip.hover
                        :title="item.title">
                        <i :class="item.icon"></i>
                      </a>
                    </span>
                    <a @click="editModalSchema(data.item.id)" class="text-primary" v-b-tooltip.hover title="Editar">
                      <i class="uil uil-pen"></i>
                    </a>
                    <a v-if="DataForm.transactionsType === 4"
                      @click="Tranfers(data.item.id, '25F94E8C-8EA0-4EE0-ADF5-02149A0E080B')" class="text-success"
                      v-b-tooltip.hover title="Transferir">
                      <i class="fas fa-arrow-right"></i>
                    </a>
                    <a @click="confirmCancellation(data.item.id)" class="text-danger" v-b-tooltip.hover
                      title="Cancelar solicitud">
                      <i class="far fa-times-circle"></i>
                    </a>
                  </div>
                </template>

              </b-table>

 
            </div>
            <div class="row">
              <div class="col">
                <div class="d-flex justify-content-end">
                  <b-pagination v-model="currentPage" :total-rows="rows" :per-page="perPage" @change="myProvider">
                  </b-pagination>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>
<style scoped>
.acciones-cell {
  display: flex;
  justify-content: space-around;
  align-items: center;
}

.header-cell {
  
  width: 50px;
  font-size: 24px;
  font-weight:  normal;
}
.header-cell div {
  color: blue;
  width: 50px;
  font-size: 24px;
  font-weight: bold;
}
</style>