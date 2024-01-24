<template>
  <div>
    <h4>{{ DataForm.title }}</h4>
    <ValidationObserver ref="form">
      <form @submit.prevent="onSubmit">

    <div class="row">
      <div class="col-3 p-2" v-if="$route.query.Action === 'edit'">
        <b-button-group class="mt-4 mt-md-0">
          <b-button variant="secundary" class="btn" @click="GoBack()">
            <i class="bx bx-arrow-back"></i> Lista
          </b-button>

          <b-button
            variant="success"
            title="Imprimir"
            @click="editSchemaPrint()"
            size="sm"
          >
            <i class="uil uil-print font-size-18"></i> Guardar

          </b-button>
        </b-button-group>
      </div>
      <div class="col-3 p-2" v-else>
        <b-button-group class="mt-4 mt-md-0">
          <b-button variant="secundary" class="btn"   size="sm" @click="GoBack()">
            <i class="bx bx-arrow-back"></i> Lista
          </b-button>

          <b-button
            variant="success"
            title="Imprimir"
            @click="onSubmit()"
            size="sm"
          >
            <i class="uil uil-print font-size-18"></i> Guardar
          </b-button>

        </b-button-group>
      </div>
    </div>
      <div class="col-lg-12 p-4" >
        <div class="card">
          <div class="card-body justify-content-center">
            <div class="row justify-content-end ">
              <div class="col-md-2">
                <b-form-group
                  label="Caja"
                  class="mb-2"
                >
                  <vueselect
                    :options="ListBox"
                    :reduce="(row) => row.id"
                    label="name"
                    v-model="principalSchema.boxId"
                    size="sm"
                  >
                  </vueselect>
                </b-form-group>

              </div>
              <!-- <div class="col-md-2">
                <b-form-group
                  label="Banco"
                  class="mb-2"
                >
                  <vueselect
                    :options="ListBank"
                    :reduce="(row) => row.id"
                    label="name"
                    v-model="recipe.bankId"
                    size="sm"
                  >
                  </vueselect>
                </b-form-group>

              </div> -->

              <div class="col-md-2">
                <b-form-group
                  label="Fecha"
                  class="mb-4"
                >

                  <ValidationProvider
                    rules="required"
                    v-slot="{ errors }"
                  >
                    <b-form-input
                      v-model="principalSchema.date"
                      type="date"
                      size="sm"
                    ></b-form-input>
                    <span v-if="errors[0]" class="text-danger"> El campo es requerido </span>
                  </ValidationProvider>

                </b-form-group>
              </div>
            </div>

            <div class="row">

              <div class="col-md-6 ml-auto">
                <b-form-group label="Recibimos de" label-cols="3" class="mb-2">
                  <vSelectContact
                    v-if="item"
                    :field="item"
                    size="sm"
                    :select="Scheme[item.field]"
                    @CustomChange="GetLitValue"
                  >
                  </vSelectContact>
                </b-form-group>
              </div>
            </div>
            <div class="row">
              <div class="row ml-0 mb-3">
                <div class="col-lg-5">
                  <b-form-group>
                      <b-form-group label="Recibimos la cantidad de" label-cols="5" class="mb-2">
                        <b-form-input
                          type="number"
                          size="sm"
                        ></b-form-input>
                      </b-form-group>
                  </b-form-group>
                </div>
              </div>
            </div>

            <!-- <div class="row">
              <div class="col-md-2">
                <b-form-group
                  label="Documento"

                  class="mb-2"
                >
                  <b-form-input
                    readonly="true"
                    type="text"
                    size="sm"
                    v-model="recipe.code"
                  ></b-form-input>
                </b-form-group>
              </div>
              <div class="col-md-1">
                <b-form-group
                  label="#Fact"

                  class="mb-2"
                >
                  <b-form-input
                    readonly="true"
                    type="text"
                    v-model="principalSchema.code"
                    size="sm"
                  ></b-form-input>
                </b-form-group>
              </div>
            </div> -->

            <div class="row">

              <div class="col-md-8">
                <b-form-group
                  label="Metodo Pago"
                  class="mb-3"
                >

                <div class="col-md-12 d-flex justify-content-between">
                  <div v-for="paymentMethod in ListpaymentMethod" :key="paymentMethod.id"  class="ml-5">
                    <label :for="'radio-' + paymentMethod.id" style="font-size: 14px; font-family: Georgia, 'Times New Roman', Times, serif;">
                      {{ paymentMethod.name }}
                    </label>
                    <input type="radio" :id="'radio-' + paymentMethod.id" v-model="recipe.paymentMethodId" :value="paymentMethod.id" />
                  </div>
                </div>
                </b-form-group>

              </div>


            <div class="row" v-for="(ledger, index) in incomeReceipt" :key="index">
              <div class="col-md-10 m-auto">
                <b-form-group :label="ledger.label" label-cols="8" class="mb-2">
                  <b-form-input class="ledger-input w-75 text-center" size="sm" type="number" v-model.number="ledger.value" onchange="calcularTotal"></b-form-input>
                </b-form-group>
              </div>
            </div>

          </div>
          <hr>
          <div class="row justify-content-end">
             <div class="col-md-5">
                <b-form-group label="Total" label-cols="2" class="mb-2 fs-5">
                  <b-form-input class="ledger-input w-25 text-center" size="sm" type="number" v-model.number="principalSchema.globalTotal" disabled></b-form-input>
                </b-form-group>
              </div>
          </div>

          </div>
        </div>
      </div>

  </form>
  </ValidationObserver>
  </div>
</template>

<script>

import Swal from "sweetalert2";
import vSelectContact from '../components/vSelectContact.vue'
var numbro = require("numbro");

var moment = require("moment");
export default {
  head() {
    return {
      title: `${this.DataForm.title} | ERP`,
    };
  },
  components:{
    vSelectContact
  },
  data() {
    return {
      FormId: "",
      DataForm: [],
      Total:0,
      ListpaymentMethod: [],
      ListBank: [],
      RecipeDetails: [],
      ListCurrency: [],
      DataFormSection: [],
      currency: {},
      item:{},
      Scheme:{},
      ledgerAccountList: [],
      principalSchema: {
        contactId: null,
        code: null,
        date: null,
        reference: null,
        globalDiscount: 0.0,
        globalTotal: 0.0,
        globalTotalTax: 0.0,
        transactionsType: 1,
        transactionsDetails: [],
        numerationId: null,
        boxId:null,
        paymentMethodId:null,
        currencyId: null ,
        commentary: "",
        taxesId: "69a423e6-1b00-4873-9003-e83d9ff13bda"
      },

      // TransactionsDetails: {
      //   id: null,
      //   transactionsId: null,
      //   referenceId: null,
      //   description: null,
      //   amount: 1,
      //   price: 0,
      //   discount: 0,
      //   total: 0,
      //   tax: 0,
      // },

      recipe:{
        date: null,
        currencyId: null ,
        bankId:'',
        reference:"",
        paymentMethodId:null,
        code:"AutoGenerado",
        transationId :null,
        globalTotal:0,
      },
      incomeReceipt: [
        { label: '10% Caja general', value:0 },
        { label: '1/3 Tercera parte final año', value: 0 },
        { label: '4% Intereses por atrasos', value: 0 },
        { label: 'Seguro Médico', value: 12 },
        { label: 'Seguro Vehículo', value: 0 },
        { label: 'Seguro Retiro', value: 0 },
        { label: 'Abono Capital', value: 0 },
        { label: 'Interés (4%)', value: 0 },
        { label: '10% Dólares', value: 0 },
        { label: '10% Euros', value: 0 },
        { label: 'Seguro Vejez Dólares', value: 0 },
        { label: 'Seguro Vejez Euros', value: 0 },
        { label: 'Abono deuda', value: 0 },
        { label: 'Hogar Madre Amadora', value: 0 },
        { label: 'Canonización del Cardenal Sancha', value: 0 },
        { label: 'Donaciones', value: 0 },
        { label: 'Formación', value: 0 },
        { label: 'Boletín Sanchino', value: 0 },
        { label: 'Telas, Hábito, Velas', value: 0 },
        { label: 'Libros', value: '' },
      ]
    };
  },

  //middleware: "authentication",

  created() {
    this.FormId = this.$route.query.Form;
    this.GetFormRows();
    const date = new Date();
    this.principalSchema.date = date.toISOString().substr(0, 10);
    this.principalSchema.transactionsDetails = []
  },

  computed: {
    calcularTotal() {
      return this.incomeReceipt.reduce((acumulador, ledger) => acumulador + ledger.valor, 0);
    },
  },
  methods: {
    async onSearch({label, value}) {
      this.search = label;

      if (label.length >= 3) {
        let url = `LedgerAccount/GetFilter?Search=${this.search}`;

        try {
          const response = await this.$axios.get(url);
          const items = response.data.data.data;

            this.principalSchema.transactionsDetails.push(
              {
                id: null,
                transactionsId: null,
                referenceId: items[0].id,
                description: null,
                amount: 1,
                price: value,
                discount: 0,
                total: value,
                tax: 0,
              }
            );

        } catch (error) {
          console.log(error)
        }
      }
    },
    SetTotal(globalTotal) {
      return numbro(globalTotal).format("0,0.00");
    },
    getDate() {

      const date = new Date();

      let day = date.getDate();

      let month = date.getMonth() + 1;

      let year = date.getFullYear();

      this.principalSchema.date = `${day}/${month}/${year}`;
    },
    GetLitValue(filds, Value) {
      this.Scheme[filds] = Value;
    },
    GetFilterDataOnlyshowForm(fields) {
      return fields.filter((rows) => rows.showForm === 1);
    },

    GetValueFormElement(formElemen) {
      this.principalSchema = formElemen;
    },
    async GetFormRows() {

        try {
          var url = `Form/GetById?Id=${this.FormId}`;
          this.DataForm = [];

          const response = await this.$axios.get(url);
          this.DataForm = response.data.data;

          await this.GetFilds();
          await this.getPaymentMethod();
          this.getBox();
          this.getBank();
          this.getCurrency();
          await this.getTransactionsDetails();
          await this.getRecipeDetails();

        } catch (error) {
          this.$toast.error(`${error}`, "ERROR", this.izitoastConfig);
        }

    },
     async GetFilds() {

      try {
        const response = await this.$axios.get(`Formfields/GetSectionWithFildsByFormID/${this.FormId}`);
        this.DataFormSection = response.data.data;

        this.DataFormSection[0].fields.forEach((field) => {
          if (field.type === 10) {
            this.item = field;
          }
        });

      } catch (error) {
        this.$toast.error(`${error}`, "ERROR", this.izitoastConfig);
      }
    },
    printForm(id) {
      this.$router.push({
        path: `/ExpressForm/TicketRecipe?Action=print&Form=${this.FormId}&Id=${id}`,
      });
    },
    getCurrency (){
      this.$axios
        .get(`Currency/GetAll`)
        .then((response) => {
          this.ListCurrency = response.data.data;
          this.recipe.currencyId = this.ListCurrency[0].id;

        })
        .catch((error) => {
          this.$toast.error(`${error}`, "ERROR", this.izitoastConfig);
        });
    },
    async getPaymentMethod() {

      try {
        const response = await this.$axios.get(`PaymentMethod/GetAll`);
        this.ListpaymentMethod = response.data.data;
        this.recipe.paymentMethodId = this.ListpaymentMethod[0].id;
        console.log(this.ListpaymentMethod);
      } catch (error) {
        this.$toast.error(`${error}`, "ERROR", this.izitoastConfig);
      }
    },
    getBox() {
      this.$axios
        .get(`Box/GetAll`)
        .then((response) => {
          this.ListBox = response.data.data;
          this.principalSchema.boxId = this.ListBox[0].id;

        })
        .catch((error) => {
          this.$toast.error(`${error}`, "ERROR", this.izitoastConfig);
        });
    },
    getBank() {
      this.$axios
        .get(`Bank/GetAll`)
        .then((response) => {
          this.ListBank = response.data.data;
          this.recipe.bankId = this.ListBank[0].id;

        })
        .catch((error) => {
          this.$toast.error(`${error}`, "ERROR", this.izitoastConfig);
        });
    },
    GoBack() {
      this.$router.push({ path: `/ExpressForm/Index?Form=${this.FormId}` });
    },

    FormatDate(date) {
      return moment(date).lang("es").format("DD/MM/YYYY");
    },

    editSchemaPrint() {
      this.putPrint(this.principalSchema);
    },

    async onSubmit() {

      try{

        for (const element of this.incomeReceipt) {
          if(element.value > 0)
          {
            await this.onSearch(element)
          }
        };
        await this.post(this.principalSchema);

      } catch (error) {
          console.error(error);
          // this.$toast.error(`${result}`, "ERROR", this.izitoastConfig);
      }
    },
    async getTransactionsDetails() {

      try {
        let url = `Transaction/GetById?id=${this.$route.query.Id}`;
        const response = await this.$axios.get(url);

        this.principalSchema = response.data.data;
        this.Scheme = this.principalSchema;

      } catch (error) {
        // this.$toast.error(`${error}`, "ERROR", this.izitoastConfig);
      }
    },
    async getRecipeDetails() {

      try {
        let url = `TransactionReceipt/GetByTransactionId?id=${this.$route.query.Id}`;
        const response = await this.$axios.get(url);

        this.RecipeDetails = response.data.data;
        console.log(response.data.data)

        this.getTotalRecipeDetails();
      } catch (error) {
        // this.$toast.error(`${error}`, "ERROR", this.izitoastConfig);
      }
    },
    async getTotalRecipeDetails() {
      let url = `TransactionReceipt/GetTotalByTransactionId?id=${this.$route.query.Id}`;
      this.$axios
        .get(url)
        .then((response) => {
          this.TotalPaid = response.data.data;
        })
        .catch((error) => {
          //this.$toast.error(`${error}`, "ERROR", this.izitoastConfig);
        });
    },
    postPrint(data,transationId) {

      data.globalTotal = this. principalSchema.globalTotal
      data.transationId = transationId;
      data.date = this. principalSchema.date

      let url = `TransactionReceipt/CreateRecipe`;
      let result = null;
      console.log(data)
      this.$axios
        .post(url, data)
        .then((response) => {
          result = response;
          console.log(result.data.data.id)

          this.$toast.success(
            "El Registro ha sido creado correctamente.",
            "ÉXITO"
          );

          this.printForm(result.data.data.id);
        })
        .catch((error) => {
          console.log(error)
          //  this.$toast.error(`${result}`, "ERROR", this.izitoastConfig);
        });
    },
    async post(data) {

      data.transactionsType = 9;
      data.formId = this.FormId;
      data.contactId = this.Scheme.contactId
      data.paymentMethodId = this.recipe.paymentMethodId
      data.currencyId = this.recipe.currencyId

      try {
        let url = `Transaction/Create`;
        let result = null;
        const response = await this.$axios.post(url, data);

        result = response;

        this.$toast.success(
          "El Registro ha sido creado correctamente.",
          "ÉXITO"
        );

        this.postPrint(this.recipe, result.data.data.id);
        // this.GoBack();
      } catch (error) {
        console.error(error);
        this.$toast.error(`${result}`, "ERROR", this.izitoastConfig);
      }

    },
    addLedgerAccount(data, idx) {
      var obj = this.TransactionsDetails.find((element, index) => index === idx);
      let row = this.ledgerAccountList.find(
        (element) => element.id == obj.referenceId
      );

      obj.referenceId = row.id;
      obj.price = row.priceSale;
      obj.priceWithTax = row.priceWithTax;
      this.calculateLineTotal(obj);
      this.calculateLineTotalWithTax(obj);
    },
    putPrint(data) {
      data.transactionsType = this.DataForm.transactionsType;
      data.formId = this.FormId;
      let frmResult = this.ValideForm();
      this.$axios
        .put("Transaction/Update", data)
        .then((response) => {
          this.$toast.success(
            "El Registro ha sido actualizado correctamente.",
            "EXITO"
          );

          this.printForm(data.id);
        })
        .catch((error) => {
          reject(error);
          //   this.$toast.error(`${error}`, "ERROR", this.izitoastConfig);
        });
    },
    confirmCancellation(id) {
      let url ="";



        url = `TransactionReceipt/Delete/${id}`;
      console.log(url)



      Swal.fire({
        title: "estas seguro?",
        text: "esta seguro que quiere remover esta fila",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Si , Remuévela!",
        cancelButtonText: 'Cancelar',
      }).then((result) => {
        if (result.isConfirmed) {
          this.$axios
            .delete(url)
            .then((response) => {
              Swal.fire("Removido!", "El registro esta removido.", "success");
              this.getRecipeDetails();
            })
            .catch((error) => alert(error));
        }
      });
    },

    calculateTotalTax() {
      var subtotal, total;
      subtotal = this.listTransactionsDetails.reduce(function (sum, product) {
        var lineTotal = parseFloat(product.totalTax);
        if (!isNaN(lineTotal)) {
          return sum + lineTotal;
        }
      }, 0);

      this.invoice_subtotalTax = subtotal.toFixed(2);

      total = subtotal * (this.invoice_tax / 100) + subtotal;
      total = parseFloat(total);
      if (!isNaN(total)) {
        this.invoice_totalTax = total.toFixed(2);
        this.principalSchema.globalTotalTax = this.invoice_totalTax;
      } else {
        this.invoice_totalTax = "0.00";
        this.principalSchema.globalTotalTax = this.invoice_totalTax;
      }
    },
    calculateTotal() {
      var subtotal, total;
      subtotal = this.listTransactionsDetails.reduce(function (sum, product) {
        var lineTotal = parseFloat(product.total);
        if (!isNaN(lineTotal)) {
          return sum + lineTotal;
        }
      }, 0);

      this.invoice_subtotal = subtotal.toFixed(2);

      total = subtotal * (this.invoice_tax / 100) + subtotal;
      total = parseFloat(total);
      if (!isNaN(total)) {
        this.invoice_total = total.toFixed(2);
        this.principalSchema.globalTotal = this.invoice_total;
      } else {
        this.invoice_total = "0.00";
        this.principalSchema.globalTotal = this.invoice_total;
      }
    },
    calculateLineTotal(invoiceProduct) {
      var total =
        parseFloat(invoiceProduct.price) * parseFloat(invoiceProduct.amount);
      if (!isNaN(total)) {
        invoiceProduct.total = total.toFixed(2);
      }
      this.calculateTotal();
      this.calculateLineTotalWithTax(invoiceProduct);
    },
    calculateLineTotalWithTax(invoiceProduct) {
      var total =
        parseFloat(invoiceProduct.priceWithTax) * parseFloat(invoiceProduct.amount);
      if (!isNaN(total)) {
        invoiceProduct.totalTax = total.toFixed(2);
      }
      this.calculateTotalTax();
    },

  },
};
</script>

<style>
/* Red border */
hr.new1 {
  border-top: 1px solid blue;
}

.text-size-required {
  font-size: 12px;
}

.ledger-input {
  border: none;
  border-bottom: 1px solid rgb(156, 156, 156);
  border-radius: 0;
  background-color: transparent;
  box-shadow: none;
  color: black;
}

</style>

