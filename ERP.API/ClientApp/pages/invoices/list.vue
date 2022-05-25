<script>
/**
 * Factura-list component
 */
export default {
    head() {
        return {
            title: `${this.title} `,
        };
    },
    data() {
        return {
            title: "Listado Factura",
            items: [{
                    text: "Facturas",
                },
                {
                    text: "Listado Factura",
                    active: true,
                },
            ],
            
         facturalist: [],
            totalRows: 1,
            currentPage: 1,
            perPage: 10,
            pageOptions: [10, 25, 50, 100],
            filter: null,
            filterOn: [],
            sortBy: "code",
            sortDesc: false,
            fields: [ 
               
                {
                    key: "code",
                    label: "Codigo",
                    sortable: true,
                },
                {
                    key: "reference",
                    
                    label: "Referencia",
                     
                },
                {
                    key: "date", 
                    label: "Fecha",
                    
                },
                {
                    key: "globalTotal",
                    label: "Total",
                    sortable: true,
                },
              
                "action",
            ],
        };
    },
    computed: {
        /**
         * Total no. of records
         */
        rows() {
            return this.facturalist.length;
        },
    },
    mounted() {
        // Set the initial number of items
         this.GetAllSchemaRows();
        this.totalRows = this.facturalist.length;
    },
    methods: {
           GetDate(date) {
              return "sdfadf";
    //   return moment(date.date).lang("es").format("DD/MM/YYYY");
    },
         GetAllSchemaRows() {
      this.rows = [];
   
      this.$axios
        .get("https://localhost:44367/api/" + "Transaction/GetAll", {
          headers: {
            "Content-Type": "application/json",
          },
        })
        .then((response) => {
          
          const data = response.data.data.filter(
            (transaction) => transaction.transactionsType === 1
          );
          data.map((schema) => {
            
            // let objSchema = {
            //   Id: schema.id,
            //   code : schema.code,
            //   Reference: schema.reference,
            // };
            this.facturalist.push(schema);
          });
        })
        .catch((error) => {
          this.$toast.error(`${error}`, "ERROR", this.izitoastConfig);
        });
    },
        /**
         * Search the table data with search input
         */
        onFiltered(filteredItems) {
            // Trigger pagination to update the number of buttons/pages due to filtering
           
            this.totalRows = filteredItems.length;
            this.currentPage = 1;
        },
    },
    middleware: "authentication"
};
</script>

<template>
<div>
    <PageHeader :title="title" :items="items" />

    <div class="row">
        <div class="col-md-4">
            <div>
                <button type="button" class="btn btn-success mb-3">
                    <i class="mdi mdi-plus me-1"></i> Agregar Factura
                </button>
            </div>
        </div>
        <div class="col-md-8" v-if="false">
            <div class="float-end">
                <div class="form-inline mb-3">
                    <div class="input-daterange input-group" data-provide="datepicker" data-date-format="dd M, yyyy" data-date-autoclose="true">
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
    <div class="row">
        <div class="col-sm-12 col-md-6">
            <div id="tickets-table_length" class="dataTables_length">
                <label class="d-inline-flex align-items-center">
                    Muestra&nbsp;
                    <b-form-select v-model="perPage" size="sm" :options="pageOptions"></b-form-select>&nbsp;entradas
                </label>
            </div>
        </div>
        <!-- Search -->
        <div class="col-sm-12 col-md-6">
            <div id="tickets-table_filter" class="dataTables_filter text-md-end">
                <label class="d-inline-flex align-items-center">
                    Buscar:
                    <b-form-input v-model="filter" type="search" placeholder="Search..." class="form-control form-control-sm ms-2"></b-form-input>
                </label>
            </div>
        </div>
        <!-- End search -->
    </div>
    <!-- Table -->
    <div class="table-responsive mb-0">
        <b-table table-class="table table-centered datatable table-card-list" thead-tr-class="bg-transparent" :items="facturalist" :fields="fields" responsive="sm" :per-page="perPage" :current-page="currentPage" :sort-by.sync="sortBy" :sort-desc.sync="sortDesc" :filter="filter" :filter-included-fields="filterOn" @filtered="onFiltered">
            <template v-slot:cell(check)="data">
                <div class="custom-control custom-checkbox text-center">
                    <input type="checkbox" class="custom-control-input" :id="`contacusercheck${data.item.id}`" />
                    <label class="custom-control-label" :for="`contacusercheck${data.item.id}`"></label>
                </div>
            </template>
            <template v-slot:cell(id)="data">
                <a href="javascript: void(0);" class="text-dark fw-bold">
                    {{
            data.item.id
            }}
                </a>
            </template>

            <template v-slot:cell(status)="data">
                <div class="badge badge-pill bg-soft-success font-size-12" :class="{'bg-soft-warning': data.item.status === 'Pending'}">{{ data.item.status }}</div>
            </template>

            <template v-slot:cell(name)="data">
                <a href="#" class="text-body">{{ data.item.name }}</a>
            </template>
            <template v-slot:cell(download)>
                <div>
                    <button class="btn btn-light btn-sm w-xs">
                        Pdf
                        <i class="uil uil-download-alt ms-2"></i>
                    </button>
                </div>
            </template>
            <template v-slot:cell(action)>
                <ul class="list-inline mb-0">
                    <li class="list-inline-item">
                        <a href="javascript:void(0);" class="px-2 text-primary" v-b-tooltip.hover title="Edit">
                            <i class="uil uil-pen font-size-18"></i>
                        </a>
                    </li>
                    <li class="list-inline-item">
                        <a href="javascript:void(0);" class="px-2 text-danger" v-b-tooltip.hover title="Delete">
                            <i class="uil uil-trash-alt font-size-18"></i>
                        </a>
                    </li>
                </ul>
            </template>
        </b-table>
    </div>
    <div class="row">
        <div class="col">
            <div class="dataTables_paginate paging_simple_numbers float-end">
                <ul class="pagination pagination-rounded">
                    <!-- pagination -->
                    <b-pagination v-model="currentPage" :total-rows="rows" :per-page="perPage"></b-pagination>
                </ul>
            </div>
        </div>
    </div>
</div>
</template>
