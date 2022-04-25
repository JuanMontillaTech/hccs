<template>
  <div>
    <div class="card">
      <div class="card-header bg-Cprimary">Listado de Libro de Diario</div>

      <div class="card-body">
        <div class="btn-group" role="group" aria-label="Basic example">
          <a
            id="_btnRefresh"
            v-on:click="getAllRows()"
            class="btn btn-light btn-sm text-black-50 btnRefresh"
            name="_btnRefresh"
            ><i class="fas fa-sync-alt"></i> Actualizar Datos</a
          >
        </div>

        <vue-good-table
          :columns="columns"
          :rows="journales"
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
              <vue-good-table
                :columns="columnsInt"
                :rows="props.row.journaDetails"
              >
              </vue-good-table>
            </span>
          </template>
        </vue-good-table>
      </div>
    </div>
  </div>
</template>

<script>
var numbro = require("numbro");
var moment = require("moment");
export default {
  name: "LibroDiario",
  layout: "TheSlidebar",

  data() {
    return {
      journales: [],

      columnsInt: [
        {
          label: "Cuenta",
          field: this.fieldFn,
        },
        {
          label: "Debito",
          field: "debit",
          type: "number",
        },
        {
          label: "Credito",
          field: "credit",
          type: "number",
        },
      ],
      columns: [
        {
          label: "Asiento",
          field: "code",
        },
        {
          label: "Fecha",
          field: this.GetDate,
        },
        {
          label: "Cuentas del asiento",
          field: "action",
        },
      ],
      izitoastConfig: {
        position: "topRight",
      },
      fromTitle: "",
      controller: "Journal",

      LedgerAccountes: [],

      show: true,
    };
  },

  created: function () {
    this.getAllRows();
    this.getLeaderAccount();
  },
  methods: {
    GetDate(rowObj) {
      return moment(rowObj.date).lang("es").format("DD/MM/YYYY");
    },
    fieldFn(rowObj) {
      let foundRow;

      this.LedgerAccountes.forEach((element) => {
        if (element.id == rowObj.ledgerAccountId) {
          foundRow = element;
        }
      });

      return foundRow.name;
    },
    async getLeaderAccount() {
      let url = process.env.devUrl + `LedgerAccount/GetAll`;
      let result = null;
      this.$axios
        .get(url, {
          headers: {
            "Content-Type": "application/json",
          },
        })
        .then((response) => {
          result = response;
          this.LedgerAccountes = result.data.data;
        })
        .catch((error) => {
          result = error;
        });
    },
    async getAllRows() {
      let url = process.env.devUrl + `${this.controller}/GetAll`;
      let result = null;
      this.$axios
        .get(url, {
          headers: {
            "Content-Type": "application/json",
          },
        })
        .then((response) => {
          result = response;

          this.journales = result.data.data;
        })
        .catch((error) => {
          result = error;
        });
    },
  },
};
</script>
