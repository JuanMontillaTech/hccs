<template>
  <div>

    <nav class="navbar navbar-default">
      <div class="container-fluid">
        <div class="navbar-header">
          <h4>Listado de pagos recibidos</h4>
        </div>
        <div class="btn-group" role="group" aria-label="Basic example">
          <a
            title="Nuevo Registro"
            class="btn btn-primary btn-sm text-white"
            @click="showModal"
          >
            <i class="fas fa-file"></i>
            Nuevo</a
          >

          <a
            id="_btnRefresh"
            @click="getAllRows()"
            class="btn btn-light border btn-sm text-black-50 btnRefresh"
            name="_btnRefresh"
            ><i class="fas fa-sync-alt"></i> Actualizar Datos</a
          >
        </div>
      </div>
    </nav>

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
          <b-button-group class="mt-4 mt-md-0">
            <b-button
              size="sm"
              variant="danger"
              @click="RemoveRecord(props.row)"
            >
              <i class="fas fa-trash"></i>
            </b-button>
            <b-button size="sm" variant="info" @click="EditShow(props.row)">
              <i class="fas fa-edit"></i>
            </b-button>
          </b-button-group>
        </span>
        <span v-else>
          {{ props.formattedRow[props.column.field] }}
        </span>
      </template>
    </vue-good-table>

    <b-modal
      id="newModal"
      v-model="ShowModelCreate"
      :title="fromTitle"
      hide-footer
      size="xl"
    >
      <b-form v-if="show">
        <b-form-group id="input-group-1" label="Fecha:" label-for="input-1">
          <b-form-datepicker
            id="input-1"
            v-model="form.date"
            type="date"
            locale="es"
            :state="$v.form.date.$error ? false : null"
          ></b-form-datepicker>
          <p
            class="text-danger text-size-required m-0"
            v-if="$v.form.date.$error"
          >
            Fecha Requerida.
          </p>
        </b-form-group>

        <b-form-group
          id="input-group-2"
          label="Referencia:"
          label-for="input-2"
        >
          <b-form-input
            id="input-2"
            v-model="form.reference"
            :state="$v.form.reference.$error ? false : null"
          ></b-form-input>
          <p
            class="text-danger text-size-required m-0"
            v-if="$v.form.reference.$error"
          >
            Referencia requerida.
          </p>
        </b-form-group>
        <b-form-group
          id="input-group-2"
          label="Observaciones:"
          label-for="input-2"
        >
          <b-form-textarea
            id="textarea"
            v-model="form.commentary"
            rows="3"
            max-rows="6"
          ></b-form-textarea>
        </b-form-group>

        <table class="table striped table-border">
          <thead class="bg-Cprimary">
            <tr>
              <th style="width: 20%">Cuenta contable</th>
              <th style="width: 35%">Descripción</th>

              <th style="width: 20%">Crédito</th>
              <th style="width: 5%"></th>
            </tr>
          </thead>
          <tbody>
            <tr
              v-for="(JournalDetail, index) in form.journaDetails"
              v-bind:key="index"
            >
              <td>
                <vueselect
                  :options="LedgerAccountes"
                  v-model="JournalDetail.ledgerAccountId"
                  :reduce="(row) => row.id"
                  label="name"
                ></vueselect>
              </td>
              <td>
                <textarea
                  v-model="JournalDetail.commentary"
                  class="form-control"
                  id="exampleFormControlTextarea1"
                  rows="3"
                ></textarea>
              </td>

              <td>
                <input
                  v-model="JournalDetail.credit"
                  type="text"
                  v-on:keydown="GetTotal"
                  v-on:keyup="GetTotal"
                  class="form-control"
                  style="width: 60%"
                />
              </td>

              <td>
                <button
                  type="button"
                  class="btn btn-light btn-sm text-black-50"
                  title="Eliminar"
                  v-on:click="removeRow(index)"
                >
                  <i class="fa fa-trash"></i>
                </button>
              </td>
            </tr>
          </tbody>
          <tfoot>
            <tr>
              <td>
                <button
                  type="button"
                  class="btn btn-light btn-sm text-black-50"
                  title="Agregar"
                  v-on:click="addRow()"
                >
                  <i class="fa fa-plus"></i> Agregar
                </button>
              </td>
              <td></td>

              <td>{{ Tcredit }}</td>
            </tr>
          </tfoot>
        </table>
      </b-form>

      <div class="modal-footer">
        <div class="col-3 p-2">
          <b-button-group class="mt-4 mt-md-0">
            <b-button variant="danger" class="btn" @click="HideModal()">
              <i class="bx bx-x"></i> Cerrar
            </b-button>
            <b-button variant="success" class="btn" @click="Save()">
              <i class="bx bx-save"></i> Guardar
            </b-button>
          </b-button-group>
        </div>
      </div>
    </b-modal>
  </div>
</template>

<script>
import { required } from "vuelidate/lib/validators";
var numbro = require("numbro");
var moment = require("moment");

export default {
  data() {
    return {
      journales: [],

      ShowModelCreate: false,
      columns: [
        {
          label: "Código",
          field: "code",
          type: "text",
        },

        {
          label: "Observaciones",
          field: "commentary",
          type: "text",
        },
        {
          label: "Fecha",
          field: this.GetDate,
        },
        {
          label: "Acciones",
          field: "action",
        },
      ],
      izitoastConfig: {
        position: "topRight",
      },
      fromTitle: "",
      controller: "Journal",
      form: {
        id: null,
        code: null,
        reference: null,
        commentary: null,
        date: "",
        typeRegisterId: "5dc90864-a835-4582-917c-53e5209f7aeb",

        journaDetails: [
          {
            id: null,
            contactId: null,
            JournalId: null,
            ledgerAccountId: null,
            debit: 0.0,
            credit: 0.0,
            commentary: "",
          },
        ],
      },
      LedgerAccountes: [],
      Tdebit: 0.0,
      Tcredit: 0.0,
      show: true,
    };
  },
  validations: {
    form: {
      reference: {
        required,
      },
      date: {
        required,
      },
    },
  },
  created: function () {
    this.getAllRows();
    this.getLeaderAccount();
  },
  methods: {
    GetDate(date) {
      return moment(date.date).lang("es").format("DD/MM/YYYY");
    },
    async clearData() {
      this.fromTitle = "Editar Regisro";
      this.form.code = "";
      this.form.reference = "";
      this.form.commentary = "";
      this.form.date = "";
      this.form.id = null;
      this.Tcredit = 0;
      this.Tdebit = 0;
      let row = {
        id: null,
        contactId: null,
        JournalId: null,
        ledgerAccountId: null,
        debit: 0.0,
        credit: 0.0,
        commentary: "",
      };
      this.form.journaDetails = [];
      this.form.journaDetails.push(row);
    },
    async capitalizeFirstLetter(string) {
      return string.charAt(0).toUpperCase() + string.slice(1);
    },
    async EditShow(item) {
      let EditModel = item;

      EditModel = {
        id: item.id,
        code: item.code,
        reference: item.reference,
        commentary: item.commentary,
        date: item.date,
        typeRegisterId: "5dc90864-a835-4582-917c-53e5209f7aeb",
        journaDetails: item.journaDetails,
      };

      this.form = EditModel;
      this.fromTitle = "Editar Regisro";
      this.GetTotal();
      this.ShowModelCreate = true;
    },
    async getAllRows() {
      let url =  `${this.controller}/GetAll`;
      let result = null;
      this.$axios
        .get(url)
        .then((response) => {
          result = response;
          const data = result.data.data.filter(
            (Journals) =>
              Journals.typeRegisterId === "5dc90864-a835-4582-917c-53e5209f7aeb"
          );
          this.journales = data;
        })
        .catch((error) => {
          result = error;
        });
    },
    async RemoveRecord(row) {
      let url =  `Journal/Delete?id=${row.id}`;
      let result = null;

      this.$axios
        .delete(url, this.form, {
          headers: {
            "Content-Type": "application/json",
          Authorization: `${localStorage.getItem("authUser")}`,
          },
        })
        .then((response) => {
          this.$toast.error(
            "registro eliminado.",
            "ERROR",
            this.izitoastConfig
          );
          result = response;
        })
        .catch((error) => {
          this.$toast.error(`${error}`, "ERROR", this.izitoastConfig);
        });
    },
    async getLeaderAccount() {
       let url =  `ConfigurationReport/GetAccountByCode?Code=PR`;

      let result = null;
      this.$axios
        .get(url
         )
        .then((response) => {
          result = response;


           const data = result.data.data.forEach(element =>  this.LedgerAccountes.push(element.ledgerAccount));

        })
        .catch((error) => {
          result = error;
        });
    },
    async addRow() {
      let newRow = {
        id: null,
        contactId: null,
        JournalId: null,
        ledgerAccountId: null,
        debit: 0.0,
        credit: 0.0,
        commentary: "",
      };
      this.form.journaDetails.push(newRow);
    },
    async removeRow(index) {
      this.form.journaDetails.splice(index, 1);
    },
    async GetTotal() {
      var Total = numbro(0);
      this.form.journaDetails.forEach((e) => Total.add(e.debit));
      this.Tdebit = Total.formatCurrency({
        thousandSeparated: true,
        mantissa: 2,
        negative: "parenthesis",
      });
      var TotalC = numbro(0);
      this.form.journaDetails.forEach((e) => TotalC.add(e.credit));
      this.Tcredit = TotalC.formatCurrency({
        thousandSeparated: true,
        mantissa: 2,
        negative: "parenthesis",
      });
    },
    async ValidaForm() {
      let validate = true;
      if (this.Tcredit == 0) {
        this.$toast.error(
          `  el credito no puede ser 0`,
          "Notificación",
          this.izitoastConfig
        );
        validate = false;
      }

      this.form.journaDetails.forEach((item) => {
        if (item.ledgerAccountId === null) {
          this.$toast.error(
            `Faltan por seleccionar cuentas contables`,
            "Notificación",
            this.izitoastConfig
          );
          validate = false;
        }
      });

      return validate;
    },

    async Save() {
      this.$v.$touch();

      if (this.$v.$invalid && this.ValidaForm()) {
        this.$toast.error(
          "Por favor complete el formulario correctamente.",
          "ERROR",
          this.izitoastConfig
        );
      } else {
        let url =  `Journal/Create`;
        let result = null;

        if (this.form.id == null) {
          this.$axios
            .post(url, this.form)
            .then((response) => {
              this.$toast.success(
                "Registro guardado.",
                "Notificación",
                this.izitoastConfig
              );
              result = response;
              this.getAllRows();
              this.clearData();
            })
            .catch((error) => {
              this.$toast.error(`${error}`, "ERROR", this.izitoastConfig);
            });
        } else {
          this.SaveEdit();
        }
        this.getAllRows();
        this.HideModal();
      }
    },

    async SaveEdit() {
      let url =  `Journal/Update`;
      let result = null;
      this.$axios
        .put(url, this.form, {
          headers: {
            "Content-Type": "application/json",
            Authorization: `${localStorage.getItem("authUser")}`,
          },
        })
        .then((response) => {
          this.$toast.success(
            "Registro actualizado correctamente.",
            "Notificación",
            this.izitoastConfig
          );
          this.getAllRows();
          this.clearData();
        })
        .catch((error) => {
          this.$toast.error(`${error}`, "ERROR", this.izitoastConfig);
        });
    },
    async showModal() {
      this.clearData();
      this.fromTitle = "Nueva registro";
      this.ShowModelCreate = true;
    },
    async HideModal() {
      this.getAllRows();
      this.ShowModelCreate = false;
    },
  },
};
</script>
