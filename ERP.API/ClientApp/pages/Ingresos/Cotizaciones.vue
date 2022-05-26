<template>
  <div >
   
    <nav class="navbar navbar-default">
      <div class="container-fluid">
        <div class="navbar-header">
          <h4>Listado de Cotización</h4>
        </div>
        <div class="btn-group" role="group" aria-label="Basic example">
          <a
            title="Nuevo Registro"
            class="btn btn-primary btn-sm text-white"
            @click="newRecord()"
          >
             <i class="fas fa-file"></i> 
            Nuevo</a
          >

          <a
            id="_btnRefresh"
            @click="GetAllSchemaRows()"
            class="btn btn-light border btn-sm text-black-50 btnRefresh"
            name="_btnRefresh"
            ><i class="fas fa-sync-alt"></i> Actualizar Datos</a
          >
        </div>
      </div>
    </nav>
    <vue-good-table
      :columns="columns"
      :rows="rows"
      :search-options="{
        enabled: true,
      }"
      :pagination-options="{
        enabled: true,
        mode: 'records',
      }"
    >
         <template slot="table-row" slot-scope="props">
        <span v-if="props.column.field == 'action'">
          <b-button variant="light" size="sm" @click="showSchema(props.row.Id)">
            <i class="fas fa-eye"></i>
          </b-button>
          <b-button
            variant="danger"
            size="sm"
            @click="removeSchema(props.row.Id)"
          >
            <i class="fas fa-trash"></i>
          </b-button>
          <b-button
            variant="info"
            size="sm"
            @click="editModalSchema(props.row.Id)"
          >
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
import axios from "axios";
import { required } from "vuelidate/lib/validators";

var numbro = require("numbro");
var moment = require("moment");
export default {
  head() {
    return {
      title: `${this.title} `,
    };
  },

  data() {
    return {
      ShowModalCreate: false,
      ShowModalEdit: false,
      ShowModalDelete: false,
      ShowModalDetails: false,
      schema: {
        client: null,
        concept: null,
        reference: null,
        description: null,
        quantity: null,
        price: null,
        discount: null,
        neto: null,
        tax: null,
      },
      izitoastConfig: {
        position: "topRight",
      },
      paymentOptions: [
        { value: 1, text: "Al contado" },
        { value: 2, text: "Tarjeta de crédito " },
        { value: 3, text: "Transfarencias bancarias" },
      ],
      schemaSelectList: [],
      conceptSelectList: [],
      rows: [],
      list: [
        {
          client: null,
          concept: null,
          reference: null,
          description: null,
          quantity: null,
          price: null,
          discount: null,
          neto: null,
          tax: null,
        },
      ],
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
  validations: {
    schema: {
      //   schemaProperty: {
      //     required,
      //   },
    },
  },
  created() {
    this.GetAllSchemaRows();
    this.getListForSelect();
    this.getListForSelectConcept();
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
    addRow() {
      this.list.push({
        client: null,
        concept: null,
        reference: null,
        description: null,
        quantity: null,
        price: null,
        discount: null,
        neto: null,
        tax: null,
      });
    },
    newRecord() {
      this.$router.push({
        path: "/Ingresos/Facturas",
        query: { form: "cotizacion" },
      });
    },
    showSchema(id) {
      this.$router.push({
        path: "/Ingresos/Facturas",
        query: { id: id, action: "show", form: "cotizacion" },
      });
    },
    editModalSchema(id) {
      this.$router.push({
        path: "/Ingresos/Facturas",
        query: { id: id, action: "edit", form: "cotizacion" },
      });
    },
    GetAllSchemaRows() {
      this.rows = [];
      this.$axios
        .get(this.$store.state.URL + "Transaction/GetAll", {
          headers: {
            "Content-Type": "application/json",
          },
        })
        .then((response) => {
          const data = response.data.data.filter(
            (transaction) => transaction.transactionsType === 3
          );
          data.map((schema) => {
             let objSchema = schema;
                     objSchema.date = this.GetDate(schema.date);
            console.log(objSchema);
            objSchema.globalTotal = this.SetTotal(schema.globalTotal);

            this.rows.push(objSchema);

          });
        })
        .catch((error) => {
          this.$toast.error(`${error}`, "ERROR", this.izitoastConfig);
        });
    },
    editSchema() {
      this.put(this.schema);
    },
    saveSchema() {
      this.$v.$touch();
      if (this.$v.$invalid) {
        this.$toast.error(
          "Por favor complete el formulario correctamente.",
          "ERROR",
          this.izitoastConfig
        );
      } else {
        this.ShowModalCreate = false;
        this.post(this.schema);
        this.clearForm();
      }
    },
    async getListForSelect() {
      let url = this.$store.state.URL + `Contact/GetAll`;
      let result = null;
      this.$axios
        .get(url, {
          headers: {
            "Content-Type": "application/json",
          },
        })
        .then((response) => {
          result = response;
          this.schemaSelectList = result.data.data;
        })
        .catch((error) => {
          result = error;
        });
    },
    async getListForSelectConcept() {
      let url = this.$store.state.URL + `Concept/GetAll`;
      let result = null;
      this.$axios
        .get(url, {
          headers: {
            "Content-Type": "application/json",
          },
        })
        .then((response) => {
          result = response;
          this.conceptSelectList = result.data.data;
        })
        .catch((error) => {
          result = error;
        });
    },
    async post(data) {
      return new Promise((resolve, reject) => {
        this.$axios
          .post(this.$store.state.URL + "schema/Create", data, {
            headers: {
              "Content-Type": "application/json",
            },
          })
          .then((response) => {
            resolve(response);
            this.$toast.success(
              "El Registro ha sido creado correctamente.",
              "EXITO",
              this.izitoastConfig
            );
            this.GetAllSchemaRows();
          })
          .catch((error) => {
            reject(error);
            this.$toast.error(`${error}`, "ERROR", this.izitoastConfig);
          });
      });
    },
    async put(data) {
      return new Promise((resolve, reject) => {
        this.$axios
          .put(this.$store.state.URL + "schema/Update", data, {
            headers: {
              "Content-Type": "application/json",
            },
          })
          .then((response) => {
            resolve(response);
            this.$toast.success(
              "El Registro ha sido actualizado correctamente.",
              "EXITO",
              this.izitoastConfig
            );
            this.ShowModalEdit = false;
          })
          .catch((error) => {
            reject(error);
            this.$toast.error(`${error}`, "ERROR", this.izitoastConfig);
          });
      });
    },
    removeSchema(id) {
      this.$toast.question(
        "Esta seguro que quiere eliminar esta registro?",
        "PREGUNTA",
        {
          timeout: 20000,
          close: false,
          overlay: true,
          toastOnce: true,
          id: "question",
          zindex: 999,
          position: "center",
          buttons: [
            [
              "<button><b>YES</b></button>",
              function (instance, toast) {
                instance.hide({ transitionOut: "fadeOut" }, toast, "button");
                axios
                  .delete(this.$store.state.URL + `Transaction/Delete/?id=${id}`, {
                    headers: {
                      Authorization: `Bearer ${localStorage.getItem("token")}`,
                    },
                  })
                  .then((response) => {
                    alert(
                      "EXITO: El Registro ha sido eliminado correctamente."
                    );
                    location.reload();
                  })
                  .catch((error) => alert(error));
              },
              true,
            ],
            [
              "<button>NO</button>",
              function (instance, toast) {
                instance.hide({ transitionOut: "fadeOut" }, toast, "button");
              },
            ],
          ],
        }
      );
    },
    clearForm() {
      for (const x in this.schema) {
        this.schema[x] = null;
      }
    },
  },
};
</script>

<style>
.modal-header {
  background-color: #457b9d !important;
  color: #fff;
}
.text-size-required {
  font-size: 12px;
}
</style>
