<script>
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
            FormId: '',
            DataForm: {},
            DataRows: [],
            fields : ["Acción"],
            items: [],
            izitoastConfig: {
                position: "topRight",
            },
            Controller: "Transaction",
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
        '$route.query.Form'() {
            this.DataForm= {};
            this.fields=[];
            this.fields = ["Acción"],
            this.DataRows= [];
            this.items= [];
            this.FormId = this.$route.query.Form;
            this.GetFilds();
            this.GetFormRows();

        }
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

                path: "/ExpressForm/CreateOfEdit" ,
                query: {
                    Form: this.DataForm.id,
                    Action: "create",
                    FormId: this.DataForm.id,
                    id: null,
                },
            });
        },
        showSchema(id) {
            
            this.$router.push({

path: "/TransactionsExpress/FuncionalFormExpress",
query: {
  Form: this.DataForm.id,
  Action: "create",
  id: null,
},
});
        },
        editModalSchema(id) {

            this.$router.push({
        path: "/TransactionsExpress/FuncionalFormExpress",
        query: {
          Form: this.DataForm.id,
          Action: "edit",
          Id: id,
        },
      });
        },

        removeSchema(id) {
            var url = `${this.DataForm.controller}/Delete`;
            this.$axios
                .delete(url + `/?id=${id}`,)
                .then((response) => {

                    this.GetAllSchemaRows();
                })
                .catch((error) => alert(error));
        },
        GetDate(date) {
            return moment(date).lang("es").format("DD/MM/YYYY");
        },
        GetAllSchemaRows() {

            this.DataRows = [];
            this.$axios
                .get("Transaction/GetAllByType?TransactionsTypeId=" + this.DataForm.transactionsType )
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

                    this.GetAllSchemaRows();
                    // Set the initial number of items
                    this.totalRows = this.items.length;
                })
                .catch((error) => {

                    this.$toast.error(`${error}`, "ERROR", this.izitoastConfig);
                });
        }, GetFilds() {



            this.$axios
                .get(`Formfields/GetByFormId/${this.FormId}`)
                .then((response) => {
                    response.data.data.map((schema) => {

                        if (schema.isActive && schema.showList) this.fields.push({
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
                        <button type="button" class="btn btn-success mb-3" @click="newRecord()">
                            <i class="mdi mdi-plus me-1"></i> Agregar
                        </button>
                        <a id="_btnRefresh" @click="GetAllSchemaRows()"
                            class="btn btn-light border btn-sm text-black-50 btnRefresh" name="_btnRefresh"><i
                                class="fas fa-sync-alt"></i> Actualizar Datos</a>
                    </div>

                </div>
                <div class="col-md-8" v-if="false">
                    <div class="float-end">
                        <div class="form-inline mb-3">
                            <div class="input-daterange input-group" data-provide="datepicker"
                                data-date-format="dd M, yyyy" data-date-autoclose="true">
                                <input type="text" class="form-control text-left" placeholder="From" name="From" />
                                <input type="text" class="form-control text-left" placeholder="To" name="To" />
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
                                        <b-form-select v-model="perPage" size="sm" :options="pageOptions">
                                        </b-form-select>&nbsp;entradas
                                    </label>
                                </div>
                            </div>
                            <!-- Search -->
                            <div class="col-sm-12 col-md-6">

                                <div id="tickets-table_filter" class="dataTables_filter text-md-end">
                                    <label class="d-inline-flex align-items-center">
                                        Buscar:
                                        <b-form-input v-model="filter" type="search"
                                            class="form-control form-control-sm ms-2"></b-form-input>
                                    </label>
                                </div>
                            </div>
                            <!-- End search -->
                        </div>
                        <!-- Table -->
                        <div class="table-responsive mb-0 ">

                            <b-table class="table table-centered table-nowrap" :items="DataRows" :fields="fields"
                                responsive="sm" :per-page="perPage" :current-page="currentPage" :sort-by.sync="sortBy"
                                :sort-desc.sync="sortDesc" :filter="filter" :filter-included-fields="filterOn"
                                @filtered="onFiltered">
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
                                        <li class="list-inline-item" v-if="DataForm.edit">
                                            <a href="javascript:void(0);" class="px-2 text-primary" v-b-tooltip.hover
                                                title="Editar" @click="editModalSchema(data.item.id)">
                                                <i class="uil uil-pen font-size-18"></i>
                                            </a>
                                        </li>
                                        <li class="list-inline-item" v-if="DataForm.show" >
                                            <a href="javascript:void(0);" class="px-2 text-primary" v-b-tooltip.hover
                                                title="Ver" @click="showSchema(data.item.id)">
                                                <i class="uil uil-file font-size-18"></i>
                                            </a>
                                        </li>

                                        <li class="list-inline-item" v-if="DataForm.delete">
                                            <a href="javascript:void(0);" class="px-2 text-danger"
                                                @click="removeSchema(data.item.id)" v-b-tooltip.hover title="Delete">
                                                <i class="uil uil-trash-alt font-size-18"></i>
                                            </a>
                                        </li>
                                         <b-dropdown   class="list-inline-item" variant="white" v-if="DataForm.plus" right
                                            toggle-class="text-muted font-size-18 px-2">
                                            <template v-slot:button-content>
                                                <i class="uil uil-ellipsis-v"></i>
                                            </template>

                                            <a class="dropdown-item" href="#">Acción</a>
                                            <a class="dropdown-item" href="#">Another action</a>
                                            <a class="dropdown-item" href="#">Something else here</a>
                                        </b-dropdown>
                                    </ul>
                                </template>
                            </b-table>
                        </div>
                        <div class="row">
                            <div class="col">
                                <div class="dataTables_paginate paging_simple_numbers float-end">
                                    <ul class="pagination pagination-rounded mb-0">
                                        <!-- pagination -->
                                        <b-pagination v-model="currentPage" :total-rows="rows" :per-page="perPage">
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
