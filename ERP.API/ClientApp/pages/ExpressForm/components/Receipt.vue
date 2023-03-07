<template>
  <div>
    <h4>Recibo de {{ DataForm.title }}</h4>
    <div class="row  ">
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
            @click="saveSchemaPrint()"
            size="sm"
          >
            <i class="uil uil-print font-size-18"></i> Guardar
          </b-button>
        </b-button-group>
      </div>
    </div>

    <div class="row">
      <div class="col-lg-12">
        <div class="card">
          <div class="card-body">
            <div class="row">
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
              <div class="col-md-1">
                <b-form-group
                  label="Fecha"

                  class="mb-2"
                >
                  <b-form-input
              v-model="recipe.date"
                    type="date"
              size="sm"
                  ></b-form-input>
                </b-form-group>
              </div>

                <div class="col-md-3">
                  <b-form-group
                    label="Caja/Banco"

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
                    label="Cliente"

                    class="mb-2"
                  >
                    <b-form-input
                      size="sm"
                      readonly="true"
                      type="text"
                      v-model="principalSchema.name"
                    ></b-form-input>
                  </b-form-group>
                </div>
                <div class="col-md-2">
                <b-form-group
                  label="Metodo Pago"

                  class="mb-3"
                >


                  <vueselect
                   ç
                    :options="ListpaymentMethod"
                    :reduce="(row) => row.id"
                    label="name"
                    v-model="recipe.paymentMethodId"


                  >
                  </vueselect>


                </b-form-group>

              </div>


            </div>

            <div class="row">
              <div class="col-md-2">
                <b-form-group
                  label="Reference"

                  class="mb-2"
                >
                  <b-form-input
                    size="sm"
                    type="text"
                    v-model="recipe.reference"

                  ></b-form-input>

                </b-form-group>
              </div>


          </div>



            <div class="row ml-0 mb-3">
              <div class="col-lg-2">
                <b-form-group>
                  <h4 class="card-title">Total Facturado</h4>
                  <b-form-input
                    v-model="principalSchema.globalTotal"
                    disabled
                    size="sm"
                  ></b-form-input>
                </b-form-group>
              </div>

              <div class="col-lg-2">
                <b-form-group>
                  <h4 class="card-title">a pagar {{recipe.pay}}</h4>
                  <b-form-input v-model="recipe.pay"   size="sm"></b-form-input>

                </b-form-group>
              </div>
              <div class="col-md-2">
                <b-form-group
                  label="Moneda"
                  class="mb-2"
                >

                  <vueselect
                    :options="ListCurrency"
                    :reduce="(row) => row"
                    label="name"
                    class="sm"
                    v-model="recipe.currencyId"

                  >
                    <template v-slot:option="option" >
                          <span>
                            Moneda {{ option.name }} <strong  > Tasa:  {{ option.rate }}   </strong>

                          </span>

                    </template>

                  </vueselect>
                </b-form-group>

              </div>
            </div>

            <div class="row ml-0 mb-3">
              <div class="col-lg-12 col-md-12 col-sm-12">
                <hr class="new1" />
                <b-form-group id="input-group-2" label-for="input-2">
                  <h4 class="card-title">Comentario</h4>
                  <b-form-textarea
                    id="textarea"
                    v-model="principalSchema.commentary"
                    rows="3"
                    max-rows="6"
                  >
                  </b-form-textarea>
                </b-form-group>
              </div>
            </div>
            <div class="row ml-0 mb-3">
              <div class="col-lg-12 col-md-12 col-sm-12">
                <h4>Auditoría</h4>
                <hr class="new1" />
              </div>
            </div>
            <div class="row ml-0 mb-3">
              <div class="col-lg-6 col-md-6 col-sm-6">
                <b-form-group
                  id="input-group-4"
                  label="Creado Por :"
                  label-for="input-2"
                >
                  <b-form-input
                    id="textarea"
                    v-model="principalSchema.createdBy"
                    rows="3"
                    disabled
                    max-rows="6"
                    size="sm"
                  >
                  </b-form-input>
                </b-form-group>
              </div>
              <div class="col-lg-6 col-md-6 col-sm-6">
                <b-form-group
                  id="input-group-4"
                  label="Creado en :"
                  label-for="input-2"
                >
                  <b-form-input
                    id="textarea"
                    v-model="principalSchema.createdDate"
                    rows="3"
                    disabled
                    max-rows="6"
                    size="sm"
                  >
                  </b-form-input>
                </b-form-group>
              </div>
            </div>
            <div class="row ml-0 mb-3">
              <div class="col-lg-6 col-md-6 col-sm-6">
                <b-form-group
                  id="input-group-2"
                  label="Modificado Por:"
                  label-for="input-2"
                >
                  <b-form-input
                    id="textarea"
                    v-model="principalSchema.createdBy"
                    rows="3"
                    disabled
                    size="sm"
                    max-rows="6"
                  >
                  </b-form-input>
                </b-form-group>
              </div>
              <div class="col-lg-6 col-md-6 col-sm-6">
                <b-form-group
                  id="input-group-2"
                  label="Modificado en:"
                  label-for="input-2"
                >
                  <b-form-input
                    id="textarea"
                    v-model="principalSchema.lastModifiedDate"
                    rows="3"
                    disabled
                    max-rows="6"
                    size="sm"
                  >
                  </b-form-input>
                </b-form-group>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
var numbro = require("numbro");
var moment = require("moment");
export default {
  head() {
    return {
      title: `${this.DataForm.title} | ERP`,
    };
  },
  data() {
    return {
      FormId: "",
      DataForm: [],
      ListpaymentMethod: [],
      ListBank: [],
      ListCurrency: [],
      DataFormSection: [],
      currency: {},
      fields: [],
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

      recipe: [
        {
          date: null,
          currencyId: null ,
          reference:"",
          paymentMethodId:null,
          bankId:null,
          code:"AutoGenerado",
          pay:0,
          transationId :null





        },
      ],
    };
  },

  //middleware: "authentication",

  created() {
    this.FormId = this.$route.query.Form;
    this.GetFormRows();
    const date = new Date();
    this.recipe.date = date.toISOString().substr(0, 10);
    if (this.$route.query.Action === "edit") {
      this.getTransactionsDetails();
    }
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

      // This arrangement can be altered based on how we want the date's format to appear.
      this.principalSchema.date = `${day}/${month}/${year}`;
    },


    GetFilterDataOnlyshowForm(fields) {
      return fields.filter((rows) => rows.showForm === 1);
    },

    GetFormRows() {
      var url = `Form/GetById?Id=${this.FormId}`;
      this.DataForm = [];
      this.DataFormSection = [];
      this.$axios
        .get(url)
        .then((response) => {
          this.DataForm = response.data.data;
          this.getPaymentMethod();
          this.getBank();
          this.getCurrency();
          this.getTransactionsDetails();
        })
        .catch((error) => {
          this.$toast.error(`${error}`, "ERROR", this.izitoastConfig);
        });
    },
    printForm(id) {
      this.$router.push({
        path: `/ExpressForm/Ticket?Action=print&Form=${this.FormId}&Id=${id}`,
      });
    },
    getCurrency (){
      this.$axios
        .get(`Currency/GetAll`)
        .then((response) => {
          this.ListCurrency = response.data.data;

        })
        .catch((error) => {
          this.$toast.error(`${error}`, "ERROR", this.izitoastConfig);
        });
    },
    getPaymentMethod() {
      this.$axios
        .get(`PaymentMethod/GetAll`)
        .then((response) => {
          this.ListpaymentMethod = response.data.data;

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

    saveSchemaPrint() {
      this.principalSchema.transactionsDetails = this.list;
      this.postPrint(this.principalSchema);
    },
    async getTransactionsDetails() {
      let url = `Transaction/GetById?id=${this.$route.query.Id}`;
      this.$axios
        .get(url)
        .then((response) => {
          this.principalSchema = response.data.data;
          this.list = response.data.data.transactionsDetails;
          this.calculateTotal();
        })
        .catch((error) => {
          //this.$toast.error(`${error}`, "ERROR", this.izitoastConfig);
        });
    },

    postPrint(data) {

      data.transationId = this.principalSchema.id;

      let url = `Transaction/CreateRecipe`;
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
</style>
