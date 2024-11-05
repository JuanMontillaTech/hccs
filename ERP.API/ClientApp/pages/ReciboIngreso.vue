<template>
  <div>
    <h4>{{ DataForm.title }}</h4>
    <ValidationObserver ref="form">
      <form>
        <div class="row">
          <div class="col-3 p-2">
            <b-button-group class="mt-4 mt-md-0">
              <b-button variant="secundary"
                        class="btn"
                        size="sm"
                        @click="GoBack()">
                <i class="bx bx-arrow-back"></i> Lista
              </b-button>

              <b-button variant="success"
                        title="Imprimir"
                        @click="onSubmit()"
                        size="sm">
                <i class="uil uil-print font-size-18"></i> Guardar
              </b-button>
            </b-button-group>
          </div>

          <div class="col-lg-12 p-4">
            <div class="card">
              <div class="card-header" style="background-color: white">
                <Companyinfo class="text-center"
                             :title="DataForm.title"></Companyinfo>
              </div>
              <div class="card-body justify-content-center container">
                <div class="row justify-content-between">
                  <div class="row d-flex justify-content-between align-content-center mb-3">
                    <div class="w-50 d-flex flex-column">
                      <div class="col-md-4" v-if="Ticket.document">
                        <b-form-group class="mb-2">
                          {{ Ticket.document }}
                        </b-form-group>
                      </div>
                      <div class="col-md-4">
                        <b-form-group label="Caja" class="mb-2">
                          <vueselect :options="ListBox"
                                     :reduce="(row) => row.id"
                                     label="name"
                                     v-model="principalSchema.boxId"
                                     size="sm">
                          </vueselect>
                        </b-form-group>
                      </div>
                      <div class="col-md-4">
                        <b-form-group label="Fecha" class="mb-4">
                          <ValidationProvider rules="required"
                                              v-slot="{ errors }">
                            <b-form-input v-model="principalSchema.date"
                                          type="date"
                                          size="sm"></b-form-input>
                            <span v-if="errors[0]" class="text-danger">
                              El campo es requerido
                            </span>
                          </ValidationProvider>
                        </b-form-group>
                      </div>
                    </div>
                    <div class="d-flex w-25 justify-content-between align-items-center"></div>
                  </div>
                </div>
                <br />
                <div class="row">
                  <div class="col-md-6 ml-auto">
                    <b-form-group label="Recibimos de"
                                  label-cols="3"
                                  class="mb-2">
                      <vSelectContact v-if="item"
                                      :field="item"
                                      size="sm"
                                      :select="Scheme.contactId"
                                      @CustomChange="GetLitValue">
                      </vSelectContact>
                    </b-form-group>
                  </div>
                </div>
                <div class="row">
                  <div class="row ml-0 mb-3">
                    <div class="col-lg-5">
                      <b-form-group>
                        <b-form-group v-if="DataForm.transactionsType === 11"
                                      label="Pagamos la cantidad de"
                                      label-cols="6"
                                      class="mb-2">
                          <b-form-input type="number"
                                        size="sm"
                                        v-model="principalSchema.globalTotal"
                                        disabled></b-form-input>
                        </b-form-group>
                        <b-form-group v-if="DataForm.transactionsType === 10"
                                      label="Recibimos la cantidad de"
                                      label-cols="6"
                                      class="mb-2">
                          <b-form-input type="number"
                                        size="sm"
                                        v-model="principalSchema.globalTotal"
                                        disabled></b-form-input>
                        </b-form-group>
                      </b-form-group>
                    </div>
                  </div>
                </div>

                <div class="row">
                  <div class="col-md-8">
                    <b-form-group label="Metodo Pago" class="mb-3">
                      <div class="col-md-12 d-flex justify-content-between">
                        <div v-for="rowpaymentMethod in ListpaymentMethod"
                             :key="rowpaymentMethod.id"
                             class="ml-5">
                          <label :for="'radio-' + rowpaymentMethod.id"
                                 style="
                              font-size: 14px;
                              font-family: Georgia, 'Times New Roman', Times,
                                serif;
                            ">
                            {{ rowpaymentMethod.name }}
                          </label>
                          <input type="radio"
                                 :id="'radio-' + rowpaymentMethod.id"
                                 v-model="paymentMethod"
                                 :value="rowpaymentMethod"
                                 v-if="rowpaymentMethod" />
                        </div>

                      </div>
                    </b-form-group>
                  </div>
                  <div class="row">
                    <div class="col-md-4 ">
                      <div v-if="paymentMethod.additionalField"
                           class="alert alert-primary"
                           role="alert">
                        Referencia {{ paymentMethod.name }}
                        <b-form-input v-model="recipe.reference"
                                      size="sm"
                                      type="text"
                                      class="  form-control"></b-form-input>
                      </div>
                    </div>
                  </div>

                  <div class="row"
                       v-for="(ledger, index) in incomeReceipt"
                       :key="index">
                    <div class="col-md-10 m-auto">
                      <b-form-group :label="ledger.label"
                                    label-cols="8"
                                    class="mb-2">
                        <b-form-input class="ledger-input w-75 text-center"
                                      size="sm"
                                      type="number"
                                      v-model.number="ledger.value"
                                      onchange="calcularTotal"></b-form-input>
                      </b-form-group>
                    </div>
                  </div>
                </div>
                <hr />
                <div class="row justify-content-end">
                  <div class="col-md-5">
                    <b-form-group label="Total"
                                  label-cols="2"
                                  class="mb-2 fs-5">
                      <b-form-input class="ledger-input w-25 text-center"
                                    size="sm"
                                    type="number"
                                    v-model.number="principalSchema.globalTotal"
                                    disabled></b-form-input>
                    </b-form-group>
                  </div>
                </div>
              </div>
            </div>
          </div>
          <div class="row">
            <div class="col-3 p-2">
              <b-button-group class="mt-4 mt-md-0">
                <b-button variant="secundary"
                          class="btn"
                          size="sm"
                          @click="GoBack()">
                  <i class="bx bx-arrow-back"></i> Lista
                </b-button>

                <b-button variant="success"
                          title="Imprimir"
                          @click="onSubmit()"
                          size="sm">
                  <i class="uil uil-print font-size-18"></i> Guardar
                </b-button>
              </b-button-group>
            </div>
          </div>
        </div>
      </form>
    </ValidationObserver>
  </div>
</template>

<script>
  import Swal from "sweetalert2";
  import vSelectContact from "../components/vSelectContact.vue";
  var numbro = require("numbro");
  var moment = require("moment");

  export default {
    head() {
      return {
        title: `${this.DataForm.title} | ERP`,
      };
    },
    components: {
      vSelectContact,
    },
    data() {
      return {
        FormId: "",
        DataForm: [],
        Total: 0,
        ListpaymentMethod: [],
        ListBank: [],
        RecipeDetails: [],
        ListCurrency: [],
        DataFormSection: [],
        currency: {},
        Ticket: { transactionReceipt: {} },
        item: {},
        Scheme: {},
        ledgerAccountList: [],
        principalSchema: {
          id: null,
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
          boxId: null,
          paymentMethodId: null,
          currencyId: null,
          commentary: "",
          taxesId: "69a423e6-1b00-4873-9003-e83d9ff13bda",
        },
        paymentMethod: { id: "" },
        recipe: {
          date: null,
          currencyId: null,
          bankId: "",
          reference: "",
          paymentMethodId: null,
          code: "AutoGenerado",
          transationId: null,
          globalTotal: 0,
        },
        incomeReceipt: [],
      };
    },
    //middleware: "authentication",

    created() {
      this.FormId = this.$route.query.Form;
      this.GetFormRows();

      this.principalSchema.transactionsDetails = [];
    },
    watch: {
      paymentMethod: {
        handler(newVal) {
          this.recipe.paymentMethodId = newVal.id;
        },
        deep: true,
      },
      incomeReceipt: {
        handler() {
          this.principalSchema.globalTotal = this.incomeReceipt.reduce(
            (acumulador, { value }) => acumulador + value,
            0
          );
        },
        deep: true,
      },
    },
    methods: {
      async onSearch({ id, label, value }) {
        this.search = label;

        if (label.length >= 3) {
          let url = `LedgerAccount/GetFilter?Search=${this.search}`;

          try {
            const response = await this.$axios.get(url);
            const items = response.data.data.data;

            this.principalSchema.transactionsDetails.push({
              id: null,
              transactionsId: null,
              referenceId: items[0].id,
              description: null,
              amount: 1,
              price: value,
              discount: 0,
              total: value,
              tax: 0,
            });
          } catch (error) {
            console.log(error);
          }
        }
      },
      SetTotal(globalTotal) {
        return numbro(globalTotal).format("0,0.00");
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

          if (this.$route.query.Action === "edit") {
            await this.getRecipeDetails();
            await this.GetLedgerByForm();
            this.GetAccountValues();
          } else {
            const date = new Date();
            this.principalSchema.date = date.toISOString().substr(0, 10);
            await this.GetLedgerByForm();
          }

        } catch (error) {
          this.$toast.error(`${error}`, "ERROR", this.izitoastConfig);
        }
      },
      async GetFilds() {
        try {
          const response = await this.$axios.get(
            `Formfields/GetSectionWithFildsByFormID/${this.FormId}`
          );
          //if DataFormSection null
          if ( response.data.data === null) return;
          console.log(response.data.data);
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
      async GetLedgerByForm() {
        //let _year = new Date(this.principalSchema.date).getFullYear();
        let _year = 2024;
        let url = `FormLedgerAccount/GetByFormIdYear?formId=${this.FormId}&year=${_year}`;
        try {
          const response = await this.$axios.get(url);
          const formLedgerAccounts = response.data.data;
          for (const ledgerAccount of formLedgerAccounts) {
            this.incomeReceipt.push({
              id: null,
              referenceId: ledgerAccount.id,
              label: ledgerAccount.name,
              value: 0,
            });
          }
        } catch (error) {
          console.error(error);
        }
      },
      printForm(id) {
        this.$router.push({
          path: `/ExpressForm/TicketRecipeIncome?Action=print&Form=${this.FormId}&Id=${id}`,
        });
      },
      getCurrency() {
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
          this.paymentMethod = this.ListpaymentMethod[0];

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
        try {
          if (this.Scheme.contactId != null) {
            if (this.principalSchema.globalTotal > 0) {
              for (const element of this.incomeReceipt) {
                if (element.value > 0) {
                  await this.onSearch(element);
                }
              }
              this.put(this.principalSchema);
            } else {
              this.$toast.error(
                `Total no puede ser 0`,
                "ERROR",
                this.izitoastConfig
              );
            }
          } else {
            this.$toast.error(
              `Contacto es requerido`,
              "ERROR",
              this.izitoastConfig
            );
          }
        } catch (error) {
          console.error(error);
          // this.$toast.error(`${result}`, "ERROR", this.izitoastConfig);
        }
      },

      async onSubmit() {
        try {
          if (this.Scheme.contactId != null) {
            if (this.principalSchema.globalTotal > 0) {
              if (this.$route.query.Action === "edit") {
                this.put(this.principalSchema);
              } else {
                await this.post(this.principalSchema);
              }
            } else {
              this.$toast.error(
                `Total no puede ser 0`,
                "ERROR",
                this.izitoastConfig
              );
            }
          } else {
            this.$toast.error(
              `Contacto es requerido`,
              "ERROR",
              this.izitoastConfig
            );
          }
        } catch (error) {
          console.error(error);
          // this.$toast.error(`${result}`, "ERROR", this.izitoastConfig);
        }
      },
      async getRecipeDetails() {
        try {
          let url = `TransactionReceipt/GetRecipeById?id=${this.$route.query.Id}`;

          const response = await this.$axios.get(url);

          this.Ticket = response.data.data;
          this.principalSchema.date = new Date(this.Ticket.date)
            .toISOString()
            .substr(0, 10);

          this.principalSchema.contactId = this.Ticket.contactId;
          this.principalSchema.code = this.Ticket.document;
          this.principalSchema.globalDiscount = this.Ticket.total;
          this.principalSchema.globalTotal = this.Ticket.total;
          this.principalSchema.globalTotalTax = this.Ticket.total;
          this.principalSchema.transactionsType = this.Ticket.type;
          this.principalSchema.boxId = this.Ticket.boxId;
          this.principalSchema.paymentMethodId = this.Ticket.paymentMethodId;
          this.principalSchema.currencyId = this.Ticket.currencyId;
          this.principalSchema.commentary = this.Ticket.commentary;
          this.principalSchema.id = this.Ticket.id;
          this.recipe.currencyId = this.Ticket.currencyId;
          this.recipe.bankId = this.Ticket.boxId;
          this.recipe.paymentMethodId = this.Ticket.paymentMethodId;
          if (this.ListpaymentMethod.length > 0) {
            this.paymentMethod = this.ListpaymentMethod.find(
              method => method.id === this.Ticket.paymentMethodId
            );
          }
          this.recipe.code = this.Ticket.document;
          this.recipe.globalTotal = this.Ticket.total;
          this.recipe.reference = this.Ticket.reference;


          this.principalSchema.transactionsDetails =
            this.Ticket.transactionReceiptDetails;

          this.Scheme.contactId = this.Ticket.contact.id;

        } catch (error) {
          //this.$toast.error(`${error}`, "ERROR", this.izitoastConfig);
        }
      },
      GetAccountValues() {
        for (let transactionDetail of this.principalSchema
          .transactionsDetails) {
          try {
            for (let receipt of this.incomeReceipt) {
              if (receipt.referenceId === transactionDetail.referenceId) {
                receipt.id = transactionDetail.id;
                receipt.value = transactionDetail.paid;
              }
            }
          } catch (error) {
            console.log(error);
          }
        }
      },
      post(data) {
        data.transactionsType = this.DataForm.transactionsType;
        data.formId = this.FormId;
        data.contactId = this.Scheme.contactId;
        data.paymentMethodId = this.recipe.paymentMethodId;
        data.currencyId = this.recipe.currencyId;
        data.globalTotal = this.principalSchema.globalTotal;
        data.type = this.principalSchema.transactionsType;
        data.date = this.principalSchema.date;
        data.recipeDetalles = this.incomeReceipt;
        data.boxId = this.principalSchema.boxId;
        data.reference = this.recipe.reference;

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

            this.GoBack();
            // this.printForm(result.data.data.id);
          })
          .catch((error) => {
            console.log(error);
            //  this.$toast.error(`${result}`, "ERROR", this.izitoastConfig);
          });
      },

      async put(data) {
        try {
          data.transactionsType = this.DataForm.transactionsType;
          data.formId = this.FormId;
          data.contactId = this.Scheme.contactId;
          data.paymentMethodId = this.recipe.paymentMethodId;
          data.currencyId = this.recipe.currencyId;
          data.globalTotal = this.principalSchema.globalTotal;
          data.type = this.principalSchema.transactionsType;
          data.date = this.principalSchema.date;
          data.recipeDetalles = this.incomeReceipt;
          data.boxId = this.principalSchema.boxId;
          data.reference = this.recipe.reference;

          let url = "TransactionReceipt/Update";

          let result = null;
          this.$axios
            .put(url, data)
            .then((response) => {
              result = response;

              this.$toast.success(
                "El Registro ha sido creado correctamente.",
                "ÉXITO"
              );

              this.GoBack();
              // this.printForm(result.data.data.id);
            })
            .catch((error) => {
              console.log(error);
              //  this.$toast.error(`${result}`, "ERROR", this.izitoastConfig);
            });
        } catch (error) {
          console.log(error);
          // this.$toast.error(`${error}`, "ERROR", this.izitoastConfig);
          reject(error);
        }
      },
      putPrint(data, transationId) {
        data.globalTotal = this.principalSchema.globalTotal;
        data.transationId = transationId;
        data.date = this.principalSchema.date;
        let url = `TransactionReceipt/Update`;
        let result = null;
        this.$axios
          .put(url, data)
          .then((response) => {
            result = response;
            console.log(result.data.data.id);

            this.$toast.success(
              "El Registro ha sido actualizado correctamente.",
              "ÉXITO"
            );

            this.printForm(result.data.data.id);
          })
          .catch((error) => {
            console.log(error);
            //  this.$toast.error(`${result}`, "ERROR", this.izitoastConfig);
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

  .container {
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
