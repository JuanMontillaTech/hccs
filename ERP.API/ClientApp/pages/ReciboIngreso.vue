<template>
  <div>
    <h4>{{ DataForm.title }}</h4>
    <ValidationObserver ref="form">
      <form @submit.prevent="onSubmit">

    <div class="row  ">

      <div class="col-3 p-2" >
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

      <div class="col-lg-12" >
        <div class="card">
          <div class="card-body justify-content-center">
            <div class="row justify-content-end ">
              <div class="col-md-2">
                <b-form-group
                  label="Caja"
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
                      v-model="recipe.date"
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
                      <!-- v-model="" -->
                        <b-form-input
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
                  <b-form-input class="ledger-input w-75 text-center" size="sm" type="number" v-model="ledger.value"></b-form-input>
                </b-form-group>
              </div>
            </div>

          </div>
          <hr>
          <div class="row justify-content-end">
             <div class="col-md-5">
                <b-form-group label="Total" label-cols="2" class="mb-2 fs-5">
                  <b-form-input class="ledger-input w-25 text-center" size="sm" type="number" v-model="Total"></b-form-input>
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
      Schema: {

      },
      principalSchema: {
        id:null,
        code: null,
        date: null,
        globalDiscount: 0.0,
        globalTotal: 0.0,
        transactionsType: 1,
        transactionsDetails: null,
        numerationId: null,
        commentary: "",
      },

      recipe:
        {

          date: null,
          currencyId: null ,
          reference:"",
          paymentMethodId:null,
          bankId:null,
          code:"AutoGenerado",
          pay:0,
          transationId :null,
          globalTotal:0,
        },
      incomeReceipt: [
        { label: '10% Caja general', value: '' },
        { label: '1/3 Tercera parte final año', value: '' },
        { label: '4% Intereses por atrasos', value: '' },
        { label: 'Seguro Médico', value: '' },
        { label: 'Seguro Vehículo', value: '' },
        { label: 'Seguro Retiro', value: '' },
        { label: 'Abono Capital', value: '' },
        { label: 'Interés (4%)', value: '' },
        { label: '10% Dólares', value: '' },
        { label: '10% Euros', value: '' },
        { label: 'Seguro Vejez Dólares', value: '' },
        { label: 'Seguro Vejez Euros', value: '' },
        { label: 'Abono deuda', value: '' },
        { label: 'Hogar Madre Amadora', value: '' },
        { label: 'Canonización del Cardenal Sancha', value: '' },
        { label: 'Donaciones', value: '' },
        { label: 'Formación', value: '' },
        { label: 'Boletín Sanchino', value: '' },
        { label: 'Telas, Hábito, Velas', value: '' },
        { label: 'Libros', value: '' },
      ]
    };
  },

  //middleware: "authentication",

  created() {
    this.FormId = this.$route.query.Form;
    this.GetFormRows();
    const date = new Date();
    this.recipe.date = date.toISOString().substr(0, 10);

  },

  computed: {
    calcularTotal() {
      return this.incomeReceipt.reduce((acumulador, ledger) => acumulador + ledger.valor, 0);
    },
  },
  methods: {
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
          this.getBank();
          this.getCurrency();
          await this.getTransactionsDetails();
          await this.getRecipeDetails();

        } catch (error) {
          this.$toast.error(`${error}`, "ERROR", this.izitoastConfig);
        }
        console.log(this.Scheme)
        console.log(this.item)

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
      // this.$axios
      //   .get(`PaymentMethod/GetAll`)
      //   .then((response) => {
      //     this.ListpaymentMethod = response.data.data;
      //     this.recipe.paymentMethodId= this.ListpaymentMethod[0].id;
      //     console.log(this.ListpaymentMethod)
      //   })
      //   .catch((error) => {
      //     this.$toast.error(`${error}`, "ERROR", this.izitoastConfig);
      //   });
      try {
        const response = await this.$axios.get(`PaymentMethod/GetAll`);
        this.ListpaymentMethod = response.data.data;
        this.recipe.paymentMethodId = this.ListpaymentMethod[0].id;
        console.log(this.ListpaymentMethod);
      } catch (error) {
        this.$toast.error(`${error}`, "ERROR", this.izitoastConfig);
      }
    },
    getBank() {
      this.$axios
        .get(`Box/GetAll`)
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

    removeRow(index) {
      this.list.splice(index, 1);
    },

    FormatDate(date) {
      return moment(date).lang("es").format("DD/MM/YYYY");
    },

    editSchemaPrint() {
      this.putPrint(this.principalSchema);
    },

    onSubmit() {

      this.postPrint(this.recipe);

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
      // let url = `TransactionReceipt/GetByTransactionId?id=${this.$route.query.Id}`;
      // this.$axios
      //   .get(url)
      //   .then((response) => {
      //     this.RecipeDetails = response.data.data;
      //     this.getTotalRecipeDetails();
      //   })
      //   .catch((error) => {
      //     //this.$toast.error(`${error}`, "ERROR", this.izitoastConfig);
      //   });
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
    postPrint(data) {

      data.globalTotal = this. principalSchema.globalTotal
      data.transationId = this.principalSchema.id;

      let url = `TransactionReceipt/CreateRecipe`;
      let result = null;

      this.$axios
        .post(url, data)
        .then((response) => {
          result = response;

          this.$toast.success(
            "El Registro ha sido creado correctamente.",
            "ÉXITO"
          );

          this.printForm(result.data.data.id);
        })
        .catch((error) => {
          result = error;
          //  this.$toast.error(`${result}`, "ERROR", this.izitoastConfig);
        });
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

