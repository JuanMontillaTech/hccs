<template>
  <div>
    <nav class="navbar navbar-default">
      <div class="container-fluid">
        <div class="navbar-header">
          <h4>Listado de {{ Title }}</h4>

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
      title: `${this.Title} | ERP Cardenal Sancha`,
    };
  },

  data() {
    return {
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
  props: [
    "Title",
    "Form",
    "TransactionsType",
    "IsClient",
    "DateLabel",
    "Path",
    "DocumentTypeId"
  ],
  created() {
    console.log();


    this.GetAllSchemaRows();
  },
  methods: {
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

        path: "/Formulario/form",
        query: {
          Form: this.Form,
          Action: "create",
          Title: this.Title,
          TransactionsType: this.TransactionsType,
          DateLabel: this.DateLabel,
          Path: this.Path,
          isClient: true,
          DocumentTypeId: this.DocumentTypeId,
          id: null,
        },
      });
    },
    showSchema(id) {
      this.$router.push({
        path: "/formulario/detail",
        query: { id: id, type: this.Form },
      });
    },
    editModalSchema(id) {
      this.$router.push({
        path: "/Formulario/form",
        query: {
          Form: this.Form,
          Action: "edit",
          Title: this.Title,
          TransactionsType: this.TransactionsType,
          DateLabel: this.DateLabel,
          Path: this.Path,
          IsClient: this.IsClient,
          DocumentTypeId: this.DocumentTypeId,
          Id: id,
        },
      });
    },
    GetAllSchemaRows() {
      this.rows = [];

      this.$axios
        .get(  "Transaction/GetAllByType?TransactionsTypeId=" + this.TransactionsType)
        .then((response) => {
          console.log(response.data);
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
      var url =  "Transaction/Delete";
       this.$axios
                  .delete(url + `/?id=${id}`, )
                  .then((response) => {
                   
                   this.GetAllSchemaRows();
                  })
                  .catch((error) => alert(error));
      // this.$toast.question(
      //   "Esta seguro que quiere eliminar esta cuenta?",
      //   "PREGUNTA",
      //   {
      //     timeout: 20000,
      //     close: false,
      //     overlay: true,
      //     toastOnce: true,
      //     id: "question",
      //     zindex: 999,
      //     position: "center",
      //     buttons: [
      //       [
      //         "<button><b>YES</b></button>",
      //         function (instance, toast) {
      //           instance.hide({ transitionOut: "fadeOut" }, toast, "button");
      //           this.$axios
      //             .delete(url + `/?id=${id}`, )
      //             .then((response) => {
      //               alert(
      //                 "EXITO: El Registro ha sido eliminado correctamente."
      //               );
      //               location.reload();
      //             })
      //             .catch((error) => alert(error));
      //         },
      //         true,
      //       ],
      //       [
      //         "<button>NO</button>",
      //         function (instance, toast) {
      //           instance.hide({ transitionOut: "fadeOut" }, toast, "button");
      //         },
      //       ],
      //     ],
      //   }
      // );
    },
  },
};
</script>

<style>
.text-size-required {
  font-size: 12px;
}
</style>
