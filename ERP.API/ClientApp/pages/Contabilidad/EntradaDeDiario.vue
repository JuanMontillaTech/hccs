<template>
  <div>
    <b-card header="Nueva entrada de diario">
      <b-form @submit="onSubmit" @reset="onReset" v-if="show">
        <b-form-group id="input-group-1" label="Fecha:" label-for="input-1">
          <b-form-datepicker
            id="input-1"
            v-model="form.Date"
            type="date"
            locale="es"
            required
          ></b-form-datepicker>
        </b-form-group>

        <b-form-group
          id="input-group-2"
          label="Regeferencia:"
          label-for="input-2"
        >
          <b-form-input id="input-2" v-model="form.Reference"></b-form-input>
        </b-form-group>
        <b-form-group
          id="input-group-2"
          label="Observaciones:"
          label-for="input-2"
        >
          <b-form-textarea
            id="textarea"
            v-model="form.Commentary"
            rows="3"
            max-rows="6"
          ></b-form-textarea>
        </b-form-group>

						
        <table class="table striped table-border">
          <thead class="bg-Cprimary">
            <tr>
              <th style="width: 20%">Cuenta contable</th>
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
                  v-model="JournalDetail.Commentary"
                  class="form-control"
                  id="exampleFormControlTextarea1"
                  rows="3"
                ></textarea>
              </td>
              <td>
                <input
                  name="JournalDetail.Debit"
                  v-model="JournalDetail.Debit"
                  type="text"
                  v-on:keydown="GetTotal"
                  v-on:keyup="GetTotal"
                  class="form-control"
                />
              </td>
              <td>
                <input
                  v-model="JournalDetail.Credit"
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
              <td>{{ TDebit }}</td>
              <td>{{ TCredit }}</td>
            </tr>
          </tfoot>
        </table>

        <b-button type="submit" variant="primary">Submit</b-button>
        <b-button type="reset" variant="danger">Reset</b-button>
      </b-form>
    </b-card>
  </div>
</template>

<script>
  
export default {
  name: "Entrada_Diario",
  layout: "TheSlidebar",
 
  data() {
    return {
      tableColumns1: [
			{
				label: "Character name",
				field: "charName",
				numeric: false,
				html: false
			},
			{
				label: "First appearance",
				field: "firstAppearance",
				numeric: false,
				html: false
			},
			{
				label: "Created by",
				field: "createdBy",
				numeric: false,
				html: false
			},
			{
				label: "Voiced by",
				field: "voicedBy",
				numeric: false,
				html: false
			}
		],
		tableRows1: [
			{
				charName: "Abu",
				firstAppearance: "Alladin (1992)",
				createdBy: "Joe Grant",
				voicedBy: "Frank Welker"
			},
			{
				charName: "Magic Carpet",
				firstAppearance: "Alladin (1992)",
				createdBy: "Randy Cartwright",
				voicedBy: "N/A"
			},
			{
				charName: "The Sultan",
				firstAppearance: "Alladin (1992)",
				createdBy: "Navid Negahban",
				voicedBy: "Douglas Seale"
			}
		],
      form: {
        
        Code: null,
        Reference: null,
        Commentary: null,
        Date: "",
        journaDetails: [
          {
            ContactId: null,
            JournalId: null,
            ledgerAccountId: null,
            Debit: 0.0,
            Credit: 0.0,
            Commentary: "",
          },
        ],
      },
      LedgerAccountes: [],
      TDebit: 0.0,
      TCredit: 0.0,
      show: true,
    };
  },
  created: function () {
    //    this.getAllRows();
    this.getLeaderAccount();
  },
  methods: {
    async getLeaderAccount() {
      let url = `https://localhost:44367/api/LedgerAccount/GetAll`;
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
    async addRow() {
      let newRow = {
        ContactId: null,
        JournalId: null,
        ledgerAccountId: null,
        Debit: 0.0,
        Credit: 0.0,
        Commentary: "",
      };
      this.form.journaDetails.push(newRow);
    },
    async removeRow(index) {
      this.form.journaDetails.splice(index, 1);
    },
    async GetTotal() {
      var Total = numbro(0);
      this.form.journaDetails.forEach((e) => Total.add(e.Debit));
      this.TDebit = Total.formatCurrency({
        thousandSeparated: true,
        mantissa: 2,
        negative: "parenthesis",
      });
      var TotalC = numbro(0);
      this.form.journaDetails.forEach((e) => TotalC.add(e.Credit));
      this.TCredit = TotalC.formatCurrency({
        thousandSeparated: true,
        mantissa: 2,
        negative: "parenthesis",
      });
    },
    onSubmit(event) {
      event.preventDefault();
     // alert(JSON.stringify(this.form));

        let url = `https://localhost:44367/api/Journal/Create`;
      let result = null;
      this.$axios
        .post(url,this.form, {
          headers: {
            "Content-Type": "application/json",
          },
        })
        .then((response) => {
          result = response;
          console.log(response); 
        })
        .catch((error) => {
          result = error;
          console.log(error);
        });

    },
    onReset(event) {
      event.preventDefault();
      // Reset our form values
      this.form.Code = "";
      this.form.Reference = "";
      this.form.Commentary = "";
      this.form.Date = "";
      // Trick to reset/clear native browser form validation state
      this.show = "";
      this.$nextTick(() => {
        this.show = true;
      });
    },
  },
};
</script>
