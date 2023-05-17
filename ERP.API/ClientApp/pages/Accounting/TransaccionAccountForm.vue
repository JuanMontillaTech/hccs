<template>
  <div>
    <PageHeader  title="Transacciones automaticas"  />
    <div class="col-12">
      <div class="card">

        {{Scheme}}
        <div class="card-body">
          <div class="row">


          <b-form-group class="col-3" >
            <h4 class="card-title" >Formulario</h4>
            <vSelect
              :field="itemsFroms"
              @CustomChange="GetLitValue"
              :select="Scheme.FormId"
            >
            </vSelect>
          </b-form-group>
          <b-form-group  class="col-4">
            <h4 class="card-title" >Cuenta</h4>
            <vSelect
              :field="itemsAccountings"
              @CustomChange="GetLitValue"
              :select="Scheme.AccountingId"
            >
            </vSelect>
          </b-form-group>

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
      Scheme:{FormId:null, LedgerAccountId:null},
      itemsFroms:{
        label:"Formulario",
        sourceApi :"Form/GetFilter",
        name: "FormId",
        formSoportId:0,
        field: "FormId",

      },
      itemsAccountings:{
        label:"Cuenta",
        sourceApi :"LedgerAccount/GetFilter",
        name: "AccountingId",
        formSoportId:0,
        field: "AccountingId",
        sourceLabel: "name",
      },

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

      this.Scheme[filds] = Value;
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
