<script>
import Swal from "sweetalert2";
var numbro = require("numbro");

var moment = require("moment");
/**
 * User list component
 */
export default {
  head() {
    return {
      title: `${this.DataForm.title} | Sistema Contable`,
    };
  },

  data() {
    return {
      FormId: "",
      url: "",
      DataForm: {},
      DataRows: [],
      fields: ["Acción"],
      items: [],
      izitoastConfig: {
        position: "topRight",
      },
      Controller: "",
      PageEdit: "",

      printPage:"",
      PageShow: "Detail",
      PageCreate: "",
      PageDelete: "",
      totalRows: 1,
      currentPage: 1,
      perPage: 10,
      pageOptions: [10, 25, 50, 100],
      filter: null,
      filterOn: [],
      sortBy: "age",
      sortDesc: false,
    };
  },
  computed: {
    /**
     * Total no. of records
     */
    rows() {
      return this.DataRows.length;
    },
  },
  mounted() {
    this.FormId = this.$route.query.Form;

    this.GetFilds();
    this.GetFormRows();
  },
  watch: {
    "$route.query.Form"() {
      this.DataForm = {};
      this.fields = [];
      (this.fields = ["Acción"]), (this.DataRows = []);
      this.items = [];
      this.FormId = this.$route.query.Form;
      this.GetFilds();
      this.GetFormRows();
    },
  },
  methods: {
    /**
     * Search the table data with search input
     */
    onFiltered(filteredItems) {
      // Trigger pagination to update the number of buttons/pages due to filtering
      this.totalRows = filteredItems.length;
      this.currentPage = 1;
    },
    newRecord() {
      this.$router.push({
        path: `${this.PageCreate}`,
        query: {
          Form: this.DataForm.id,
          Action: "create",
          id: null,
        },
      });
    },
    removeSchema(id) {
      let url ="";
      if (this.DataForm.formCode == "FEX") {

        url = `Transaction/${this.PageDelete}${id}`;

      }
      else{

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
              Swal.fire("Removido!", "El regisgro esta removido.", "success");
              this.GetFormRows();
            })
            .catch((error) => alert(error));
        }
      });
    },
    showSchema(id) {
    
     

      this.$router.push({
        path: `${this.PageShow}`,
        query: {
          Action: "Show",
          Form: this.DataForm.id,
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
    printForm(id) {
      this.$router.push({
        path: `${this.printPage}`,
        query: {
          Action: "print",
          Form: this.DataForm.id,
          Id: id,
        },
      });
    },
    GetDate(date) {
      return moment(date).lang("es").format("DD/MM/YYYY");
    },
    SetConfiguration() {
      if (this.DataForm.formCode == "FEX") {
        this.url =
          "Transaction/GetAllByType?TransactionsTypeId=" +
          this.DataForm.transactionsType;
        this.controller = "Transaction";
        this.PageEdit = "/ExpressForm/FuncionalFormExpress"; 
        this.PageCreate = "/ExpressForm/FuncionalFormExpress";
        this.PageDelete = "Delete/";
        this.PageShow == "";
      } else {
        
        this.url = `${this.DataForm.controller}/GetAll`;
        this.controller = "ExpressForm";
        this.PageEdit = "/ExpressForm/CreateOfEdit";
        this.PageCreate = "/ExpressForm/CreateOfEdit";
        this.PageDelete = "Delete/";
        this.PageShow == "";
      }
      this.printPage="/ExpressForm/Ticket"; 
    },
    GetAllSchemaRows() {
      this.DataRows = [];
      let url = this.url;

      this.$axios
        .get(url)
        .then((response) => {
          this.DataRows = response.data.data;
        })
        .catch((error) => {
          this.$toast.error(`${error}`, "ERROR", this.izitoastConfig);
        });
    },
    GetFormRows() {
      var url = `Form/GetById?Id=${this.FormId}`;
      this.DataForm = [];
      this.$axios
        .get(url)
        .then((response) => {
          this.DataForm = response.data.data;

          this.SetConfiguration();

          this.GetAllSchemaRows();
          // Set the initial number of items
          this.totalRows = this.items.length;
        })
        .catch((error) => {
          this.$toast.error(`${error}`, "ERROR", this.izitoastConfig);
        });
    },
    GetFilds() {
      this.$axios
        .get(`Formfields/GetByFormId/${this.FormId}`)
        .then((response) => {
          response.data.data.map((schema) => {
            if (schema.isActive && schema.showList)
              this.fields.push({
                label: schema.label,
                key: schema.field,
                sortable: true,
              });
          });
        })
        .catch((error) => {
          this.$toast.error(`${error}`, "ERROR", this.izitoastConfig);
        });

      //console.log(this.fields);
    },
  },
  middleware: "authentication",
};
</script>

<template>
  <div>
    <PageHeader :title="DataForm.title" :items="items" />
 
    <div class="row">
      <div class="row">
        <div class="col-md-4">
          <div>
            <button
              type="button"
              class="btn btn-success mb-3"
              @click="newRecord()"
            >
              <i class="mdi mdi-plus me-1"></i> Agregar
            </button>
            <a
              id="_btnRefresh"
              @click="GetAllSchemaRows()"
              class="btn btn-light border btn-sm text-black-50 btnRefresh"
              name="_btnRefresh"
              ><i class="fas fa-sync-alt"></i> Actualizar Datos</a
            >
          </div>
        </div>
        <div class="col-md-8" v-if="false">
          <div class="float-end">
            <div class="form-inline mb-3">
              <div
                class="input-daterange input-group"
                data-provide="datepicker"
                data-date-format="dd M, yyyy"
                data-date-autoclose="true"
              >
                <input
                  type="text"
                  class="form-control text-left"
                  placeholder="From"
                  name="From"
                />
                <input
                  type="text"
                  class="form-control text-left"
                  placeholder="To"
                  name="To"
                />
                <div class="input-group-append">
                  <button type="button" class="btn btn-primary">
                    <i class="mdi mdi-filter-variant"></i>
                  </button>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
      <div class="col-12">
        <div class="card">
          <div class="card-body">
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
              <!-- Search -->
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
                      class="form-control form-control-sm ms-2"
                    ></b-form-input>
                  </label>
                </div>
              </div>
              <!-- End search -->
            </div>
            <!-- Table -->
            <div class="table-responsive mb-0">
              <b-table
                class="table table-centered table-nowrap"
                :items="DataRows"
                :fields="fields"
                responsive="sm"
                :per-page="perPage"
                :current-page="currentPage"
                :sort-by.sync="sortBy"
                :sort-desc.sync="sortDesc"
                :filter="filter"
                :filter-included-fields="filterOn"
                @filtered="onFiltered"
              >
                <!-- <template v-slot:cell(check)="data">
                                    <div class="custom-control custom-checkbox">
                                        <input type="checkbox" class="custom-control-input"
                                            :id="`contacusercheck${data.item.id}`" />
                                        <label class="custom-control-label"
                                            :for="`contacusercheck${data.item.id}`"></label>
                                    </div>
                                </template>
                                <template v-slot:cell(name)="data">
                                    <img v-if="data.item.profile" :src="data.item.profile" alt
                                        class="avatar-xs rounded-circle me-2" />
                                    <div v-if="!data.item.profile" class="avatar-xs d-inline-block me-2">
                                        <div class="avatar-title bg-soft-primary rounded-circle text-primary">
                                            <i class="mdi mdi-account-circle m-0"></i>
                                        </div>
                                    </div>
                                    <a href="#" class="text-body">{{ data.item.contact.name }}</a>
                                </template> -->
                <template v-slot:cell(Acción)="data" class="col-sm-2 col-md-2">
                  <ul class="list-inline mb-0">
                    <li class="list-inline-item"  v-if="DataForm.print" >
                      <a
                        href="javascript:void(0);"
                        class="px-2 text-primary"
                        v-b-tooltip.hover
                        title="Imprimir"
                        @click="printForm(data.item.id)"
                      >
                        <i class="uil uil-print font-size-18"></i>
                      </a>
                    </li>

                    <li class="list-inline-item" v-if="DataForm.edit">
                      <a
                        href="javascript:void(0);"
                        class="px-2 text-primary"
                        v-b-tooltip.hover
                        title="Editar"
                        @click="editModalSchema(data.item.id)"
                      >
                        <i class="uil uil-pen font-size-18"></i>
                       
                      </a>
                    </li>

                    <li class="list-inline-item" v-if="DataForm.show">
                      <a
                        href="javascript:void(0);"
                        class="px-2 text-primary"
                        v-b-tooltip.hover
                        title="Ver"
                        @click="showSchema(data.item.id)"
                      >
                        <i class="uil uil-file font-size-18"></i>
                      </a>
                    </li>

                    <li class="list-inline-item" v-if="DataForm.delete">
                      <a
                        href="javascript:void(0);"
                        class="px-2 text-danger"
                        @click="removeSchema(data.item.id)"
                        v-b-tooltip.hover
                        title="Delete"
                      >
                        <i class="uil uil-trash-alt font-size-18"></i>
                      </a>
                    </li>
                    <!-- <b-dropdown
                      class="list-inline-item"
                      variant="white"
                      v-if="DataForm.plus"
                      right
                      toggle-class="text-muted font-size-18 px-2"
                    >
                      <template v-slot:button-content>
                        <i class="uil uil-ellipsis-v"></i>
                      </template>

                      <a class="dropdown-item" href="#">Acción</a>
                      <a class="dropdown-item" href="#">Another action</a>
                      <a class="dropdown-item" href="#">Something else here</a>
                    </b-dropdown> -->
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
                    <!-- pagination -->
                    <b-pagination
                      v-model="currentPage"
                      :total-rows="rows"
                      :per-page="perPage"
                    >
                    </b-pagination>
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
