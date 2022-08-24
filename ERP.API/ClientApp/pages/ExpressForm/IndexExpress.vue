<template>
  <div>
    <nav class="navbar navbar-default">
      <div class="container-fluid">
        <div class="navbar-header">

          <h4>Listado de {{ DataForm.title }}</h4>

        </div>
        <div class="btn-group" role="group" aria-label="Basic example">
          <a title="Nuevo Registro" class="btn btn-primary btn-sm text-white" @click="newRecord()">
            <i class="fas fa-file"></i>
            Nuevo</a>

          <a id="_btnRefresh" @click="GetAllSchemaRows()" class="btn btn-light border btn-sm text-black-50 btnRefresh"
            name="_btnRefresh"><i class="fas fa-sync-alt"></i> Actualizar Datos</a>
        </div>
      </div>
    </nav>
    <vue-good-table :columns="columns" :rows="rows" :search-options="{
      enabled: true,
    }" :pagination-options="{
  enabled: true,
  mode: 'records',
}">
      <template slot="table-row" slot-scope="props">
        <span v-if="props.column.field == 'action'">
          <b-button variant="light" size="sm" @click="showSchema(props.row.id)">
            <i class="fas fa-eye"></i>
          </b-button>
          <b-button variant="danger" size="sm" @click="removeSchema(props.row.id)">
            <i class="fas fa-trash"></i>
          </b-button>
          <b-button variant="info" size="sm" @click="editModalSchema(props.row.id)">
            <i class="fas fa-edit"></i>
          </b-button>
        </span>
        <span v-else>
          {{ props.formattedRow[props.column.field] }}
        </span>
      </template>
    </vue-good-table>

  </div>
</template>

<script>


var numbro = require("numbro");
var moment = require("moment");
export default {
  head() {
    return {
      title: `${this.DataForm.title} | ERP Cardenal Sancha`,
    };
  },
  data() {
    return {

      DataForm: {},
      FormId:"",
      izitoastConfig: {
        position: "topRight",
      },
      rows: [],
      columns: [
        {
          field: "code",
          label: "Codigo",
          sortable: true,
        },
        {
          field: "reference",

          label: "Referencia",
        },
        {
          field: "date",
          label: "Fecha",
        },
        {
          field: "globalTotal",
          label: "Total",
          sortable: true,
        },
        {
          label: "Acciones",
          field: "action",
        },
      ],
    };
  },

  middleware: "authentication",


  created() {
    this.FormId = this.$route.query.Form;
    this.GetFormRows();
  },
  methods: {
    GetFormRows() {
      var url = `Form/GetById?Id=${this.FormId}`;

      this.DataForm = [];
      this.$axios
        .get(url)
        .then((response) => {

          this.DataForm = response.data.data;
          this.GetAllSchemaRows();
        })
        .catch((error) => {

          this.$toast.error(`${error}`, "ERROR", this.izitoastConfig);
        });
    },

    SetTotal(globalTotal) {
      return numbro(globalTotal).format("0,0.00");
    },
    GetDate(date) {
      return moment(date).lang("es").format("DD/MM/YYYY");
    },
    removeRow(index) {
      this.list.splice(index, 1);
    },
    newRecord() {
      this.$router.push({

        path: "/ExpressForm/FuncionalFormExpress",
        query: {
          Form: this.DataForm.id,
          Action: "create",
          id: null,
        },
      });
    },

    showSchema(id) {
      this.$router.push({
        path: "/ExpressForm/detail",
        query: { id: id, Form: this.DataForm.id, },
      });
    },
    editModalSchema(id) {
      this.$router.push({
        path: "/ExpressForm/FuncionalFormExpress",
        query: {
          Form: this.DataForm.id,
          Action: "edit",
          Id: id,
        },
      });
    },
    GetAllSchemaRows() {
      this.rows = [];

      this.$axios
        .get("Transaction/GetAllByType?TransactionsTypeId=" + this.DataForm.transactionsType)
        .then((response) => {

          response.data.data.map((schema) => {
            let objSchema = schema;
            objSchema.date = this.GetDate(schema.date);
            objSchema.globalTotal = this.SetTotal(schema.globalTotal);
            if (objSchema.isActive) this.rows.push(objSchema);
          });
        })
        .catch((error) => {
          this.$toast.error(`${error}`, "ERROR", this.izitoastConfig);
        });
    },

    removeSchema(id) {
      var url = "Transaction/Delete";
      this.$axios
        .delete(url + `/?id=${id}`,)
        .then((response) => {

          this.GetAllSchemaRows();
        })
        .catch((error) => alert(error));

    },
  },
};
</script>

<style>
.text-size-required {
  font-size: 12px;
}
</style>
