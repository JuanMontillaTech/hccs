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
          <div class="card-body justify-content-center container">
            <div class="row justify-content-between ">
              <div class="row d-flex justify-content-between align-content-center mb-3">
                <div class="d-flex w-25 justify-content-between align-items-center">
                  <img
                      src="~/assets/images/logo-smsancha.png"
                      alt=""
                      style="width:100px; height:100px;"
                      class="logo logo-dark"
                  />
                  <p class="w-50 m-0">
                    HERMANAS DE LA CARIDAD DEL CARDENAL SANCHA
                  </p>
                </div>
                <div class="w-50 d-flex flex-column align-items-end">
                  <div class="col-md-4">
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
                  <div class="col-md-4">
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
              </div>




            </div>
            <br>
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
                      <b-form-group label="Recibimos la cantidad de" label-cols="6" class="mb-2">
                        <b-form-input
                          type="number"
                          size="sm"
                          v-model="principalSchema.globalTotal"
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
                    <input type="radio" :id="'radio-' + paymentMethod.id" v-model="recipe.paymentMethodId" :value="paymentMethod.id" v-if="paymentMethod" />
                  </div>
                </div>
                </b-form-group>

                <!--
                <div class="col-md-2" v-if="recipe.paymentMethodId !== null">
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
                </div>
                -->
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
    vSelectContact,
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
      Ticket:{transactionReceipt:{}},
      item:{},
      Scheme:{},
      ledgerAccountList: [],
      principalSchema: {
        id:null,
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
        { label: 'Seguro Médico', value: 0 },
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
        { label: 'Libros', value: 0 },
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
  watch:{
    incomeReceipt:{
      handler(){
        this.principalSchema.globalTotal = this.incomeReceipt.reduce((acumulador, {value}) => acumulador + value, 0);
      },
      deep:true
    }
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
        path: `/ExpressForm/TicketRecipeIncome?Action=print&Form=${this.FormId}&Id=${id}`,
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
    async editSchemaPrint() {
      try{
         if (this.Scheme.contactId != null) {
          if (this.principalSchema.globalTotal > 0) {

            for (const element of this.incomeReceipt) {
              if(element.value > 0)
              {
                await this.onSearch(element)
              }
            };
            this.put(this.principalSchema);
          }
          else {
            this.$toast.error(`Total no puede ser 0`, "ERROR", this.izitoastConfig);
          }
        } else {
          this.$toast.error(`Contacto es requerido`, "ERROR", this.izitoastConfig);
        }
      } catch (error) {
          console.error(error);
          // this.$toast.error(`${result}`, "ERROR", this.izitoastConfig);
      }
    },

    async onSubmit() {

      try{

        if (this.Scheme.contactId != null) {
          if (this.principalSchema.globalTotal > 0) {
            for (const element of this.incomeReceipt) {
            if(element.value > 0)
            {
              await this.onSearch(element)

            }
            };
            await this.post(this.principalSchema);

          }
          else {
            this.$toast.error(`Total no puede ser 0`, "ERROR", this.izitoastConfig);
          }
        } else {
          this.$toast.error(`Contacto es requerido`, "ERROR", this.izitoastConfig);
        }

      } catch (error) {
          console.error(error);
          // this.$toast.error(`${result}`, "ERROR", this.izitoastConfig);
      }
    },
    async getRecipeDetails() {
      try{
        let url = `TransactionReceipt/GetRecipeById?id=${this.$route.query.Id}`;
        const response = await this.$axios.get(url);
        this.Ticket = response.data.data;

        this.principalSchema.contactId= this.Ticket.transactionReceiptDetails[0].transactions.contactId,
        this.principalSchema.code= this.Ticket.transactionReceiptDetails[0].transactions.code,
        this.principalSchema.globalDiscount= this.Ticket.transactionReceiptDetails[0].transactions.globalDiscount,
        this.principalSchema.globalTotal= this.Ticket.transactionReceiptDetails[0].transactions.globalTotal,
        this.principalSchema.globalTotalTax= this.Ticket.transactionReceiptDetails[0].transactions.globalTotalTax,
        this.principalSchema.transactionsType= this.Ticket.transactionReceiptDetails[0].transactions.transactionsType,
        this.principalSchema.boxId=this.Ticket.transactionReceiptDetails[0].transactions.boxId,
        this.principalSchema.paymentMethodId=this.Ticket.transactionReceiptDetails[0].transactions.paymentMethodId,
        this.principalSchema.currencyId= this.Ticket.transactionReceiptDetails[0].transactions.currencyId,
        this.principalSchema.commentary= this.Ticket.transactionReceiptDetails[0].transactions.commentary,
        this.principalSchema.id= this.Ticket.transactionReceiptDetails[0].transactions.id,
        this.recipe.currencyId= this.Ticket.transactionReceipt.currencyId ,
        this.recipe.bankId=this.Ticket.transactionReceipt.bankId,
        this.recipe.paymentMethodId=this.Ticket.transactionReceipt.paymentMethodId,
        this.recipe.code=this.Ticket.transactionReceiptDetails[0].transactions.code,
        this.recipe.transationId =this.Ticket.transactionReceiptDetails[0].transactionsId,
        this.recipe.globalTotal=this.Ticket.transactionReceiptDetails[0].transactions.globalTotal,
        this.principalSchema.transactionsDetails = this.Ticket.transactionReceiptDetails[0].transactions.transactionsDetails

        this.Scheme.contactId= this.Ticket.transactionReceipt.contact.name
        for (let transactionDetail of this.principalSchema.transactionsDetails)
        {
            let url = `LedgerAccount/GetById?Id=${transactionDetail.referenceId}`;

            try {
              const response = await this.$axios.get(url);
              const items = response.data.data;
              for(let receipt of this.incomeReceipt)
              {
                if(receipt.label === items.name){
                  receipt.value = transactionDetail.price
                }
              }
            } catch (error) {
              console.log(error)
            }
        }
      }catch(error) {
        //this.$toast.error(`${error}`, "ERROR", this.izitoastConfig);
      };
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
        this.GoBack()
        //this.postPrint(this.recipe, result.data.data.id);
      } catch (error) {
        console.error(error);
        this.$toast.error(`${result}`, "ERROR", this.izitoastConfig);
      }

    },
    async put(data) {
      try{

        data.transactionsType = 9;
        data.formId = this.FormId;
        data.contactId = this.principalSchema.contactId
        data.paymentMethodId = this.recipe.paymentMethodId
        data.currencyId = this.recipe.currencyId
        let result = null;

        const response = await this.$axios.put("Transaction/Update", data);
        result = response;

        this.$toast.success(
          "El Registro ha sido actualizado correctamente.",
          "EXITO"
        );

        this.putPrint(this.recipe, result.data.data.id);
      }catch (error) {
        console.log(error)
        // this.$toast.error(`${error}`, "ERROR", this.izitoastConfig);
        reject(error);
      }
    },
    putPrint(data,transationId){
      data.globalTotal = this.principalSchema.globalTotal
      data.transationId = transationId;
      data.date = this.principalSchema.date
      console.log(data)
      let url = `TransactionReceipt/Update`;
      let result = null;
      this.$axios
        .put(url, data)
        .then((response) => {
          result = response;
          console.log(result.data.data.id)

          this.$toast.success(
            "El Registro ha sido actualizado correctamente.",
            "ÉXITO"
          );

          this.printForm(result.data.data.id);
        })
        .catch((error) => {
          console.log(error)
          //  this.$toast.error(`${result}`, "ERROR", this.izitoastConfig);
        });
    }
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
.container{
  width: 90%;
  margin: auto;
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

