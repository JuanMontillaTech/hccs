<template>
  <div>

    <div class="col-12">
      <div class="card">

    <div class="card-body">
      <div
        v-for="(SectionRow, SectionIndex) in DataFormSection"
        :key="SectionIndex"
      >
        <div class="row ml-0 mb-3">
          <div class="col-lg-12 col-md-12 col-sm-12">
            <h4>{{ SectionRow.name }}</h4>
            <hr class="new1" />
          </div>
        </div>

        <div class="row">
          <div
            class="col-4"
            v-for="(fieldsRow, fieldIndex) in GetFilterDataOnlyshowForm(
                    SectionRow.fields
                  )"
            :key="fieldIndex"
          >

            <DynamicElementGrid
              @CustomChange="GetValueFormElement"
              :FieldsData="principalSchema"
              :item="fieldsRow"
              :labelShow="true"
            ></DynamicElementGrid>
          </div>
        </div>
      </div>
      <hr>
      <b-form-group
        id="input-group-1"
        label="Fecha:"
        label-for="input-1"
      >
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
          <th style="width: 20%">
            <template v-if="form.journaDetails.length < 1">
              <b-button
                variant="primary"
                @click="addRow()"


              >
                <span> <i class="fas fa-plus"></i> </span>
              </b-button>
            </template>
            Cuenta contable
          </th>
          <th style="width: 35%">Descripción</th>
          <th style="width: 20%">Débito</th>
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
              name="JournalDetail.debit"
              v-model="JournalDetail.debit"
              type="text"
              v-on:keydown="GetTotal"
              v-on:keyup="GetTotal"
              class="form-control"
            />
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
            <b-button-group class="mt-4 mt-md-0">
              <b-button
                size="sm"
                variant="danger"
                @click="removeRow(index)"
                :disabled="$route.query.action == 'show'"
                v-b-tooltip.hover
              >
                <i class="fas fa-trash"></i>
              </b-button>
              <b-button
                size="sm"
                variant="info"
                @click="addRow()"
                :disabled="$route.query.action == 'show'"
              >
                <i class="fas fa-plus"></i>
              </b-button>
            </b-button-group>
          </td>
        </tr>
        </tbody>
        <tfoot>
        <tr>
          <td>


          </td>
          <td></td>
          <td>{{ Tdebit }}</td>
          <td>{{ Tcredit }}</td>
        </tr>
        </tfoot>
      </table>

      <div>
      </div>
    </div>
    </div>
  </div>
  </div>

</template>
<script>
import { required } from "vuelidate/lib/validators";

var numbro = require("numbro");
var moment = require("moment");


export default {

  data() {
    return {

      file: "",
      filesTitle: [
        {
          key: "Acciones",
          label: "Archivos",
        },
      ],

      DataRows: [],
      Spinning: true,
      FormId: "",
      RowId: "",
      fields :["Acción"],
      fieldsHorizon:[],
      DataForm: [],
      DataFormSection: [],
      DataFormSectionGrids: [],
      principalSchema: {},
      principalHorisonSchema: [],
      SchemaTable: [],

      form: {
        id: null,
        code: null,
        reference: null,
        commentary: null,
        date: "",
        typeRegisterId: "5e17b36a-fbbe-4c73-93ac-b112ee3ff08a",

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
    }

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
    this.GetFormRows();
    //this.getLeaderAccount();
  },
  methods: {
    GetFilterDataOnlyshowForm(fields) {
      let results = fields.filter((rows) => rows.showForm == 1);
      return results;
    },
    GetValueFormElement(formElemen) {
      this.principalSchema = formElemen;
    },
    GetLitValue(filds, Value) {
      this.principalSchema[filds] = Value;
    },
    GetFormRows() {
      this.FormId = this.$route.query.Form;
      var url = `Form/GetById?Id=${this.$route.query.Form}`;
      this.DataForm = [];
      this.DataFormSection = [];
      this.DataFormSectionGrids = [];

      this.$axios
        .get(url)
        .then((response) => {
          this.DataForm = response.data.data;

          this.GetFilds();
          if (this.$route.query.Action === "edit") {
            this.RowId = this.$route.query.Id;
           // this.GetFildsData();
            //this.GetFile();
          }
        })
        .catch((error) => {
          //this.$toast.error(`${error}`, "ERROR", this.izitoastConfig);
        });
    },
    GetDate(date) {
      return moment(date.date).lang("es").format("DD/MM/YYYY");
    },
    GetFilds: function () {
      this.$axios
        .get(`Formfields/GetSectionWithFildsByFormID/${this.FormId}`)
        .then((response) => {
          this.DataFormSection = response.data.data;
          this.fieldsHorizon = response.data.data[0].fields;
          this.Spinning = false;
        })
        .catch((error) => {
          //this.$toast.error(`${error}`, "ERROR", this.izitoastConfig);
        });
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
        typeRegisterId: "5e17b36a-fbbe-4c73-93ac-b112ee3ff08a",
        journaDetails: item.journaDetails,
      };

      this.GetTotal();
      this.form = EditModel;
      this.fromTitle = "Editar Regisro";
      this.ShowModelCreate = true;
    },
    async getAllRows() {
      let url =  `${this.controller}/GetAll`;
      let result = null;
      this.$axios
        .get(url )
        .then((response) => {
          result = response;

          const data = result.data.data.filter(
            (Journals) =>
              Journals.isActive === true
          );

          this.journales = result.data.data;
        })
        .catch((error) => {
          result = error;
        });
    },
    async RemoveRecord(row) {
      let url =  `Journal/Delete?id=${row.id}`;
      let result = null;

      this.$axios
        .delete(url, this.form )
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
      let url =  `LedgerAccount/GetAll`;
      let result = null;
      this.$axios
        .get(url )
        .then((response) => {
          result = response;
          this.LedgerAccountes = result.data.data;
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
      if (this.Tcredit == 0 || this.Tdebit == 0) {
        this.$toast.error(
          `el debito y el credito no puede ser 0`,
          "Notificación",
          this.izitoastConfig
        );
        validate = false;
      }
      if (this.Tcredit !== this.Tdebit) {
        this.$toast.error(
          `el debito y el credito no son iguales`,
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
            .post(url, this.form )
            .then((response) => {
              this.$toast.success(
                "registro creado.",
                "EXITO",
                this.izitoastConfig
              );
              result = response;
              this.getAllRows();
              this.HideModal();
            })
            .catch((error) => {
              result = error;
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
        .put(url, this.form )
        .then((response) => {
          result = response;
          this.$toast.success(
            "Registro actualizado.",
            "EXITO",
            this.izitoastConfig
          );
          this.getAllRows();
          this.HideModal();
        })
        .catch((error) => {
          result = error;
          this.$toast.error(`${error}`, "ERROR", this.izitoastConfig);
        });
    },
    async showModal() {
      this.clearData();
      this.fromTitle = "Nueva entrada de diario";
      this.ShowModelCreate = true;
    },
    async HideModal() {
      this.getAllRows();
      this.ShowModelCreate = false;
    },
  },
}


</script>
