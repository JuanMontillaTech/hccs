<template>
  <div>
    <div v-if="$route.query.form == 'cotizacion'">
      <PageHeader title="Formulario Cotización" />
    </div>
    <div v-else-if="$route.query.form == 'notasDeCredito'">
    
        <PageHeader title="Formulario de Notas débito" />
    </div>
    <div v-else-if="$route.query.form == 'conduce'">
      <PageHeader title="Formulario de Conduces" />
    </div>
    <div v-else-if="$route.query.form == 'notasDeDebito'">
      <PageHeader title="Formulario de Notas débito" />
    </div>
    <div v-else-if="$route.query.form == 'ordenDeCompra'">
      <PageHeader title="Formulario de Orden de compra" />
    </div>
    <div v-else-if="$route.query.form == 'facturascompras'">
      <PageHeader title="Formulario de Facturas de compras" />
    </div>

    
    <div v-else><PageHeader title="Listado de Facturas" /></div>

    <div class="row">
      <div class="col-lg-12">
        <div class="card">
          <div class="card-body">
              <div class="col-lg-6 col-md-6 col-sm-12">
              <b-form-group>
                <h4 class="card-title">Código #: {{principalSchema.code}}</h4>
                 
              </b-form-group>
            </div>

            <div class="col-lg-6 col-md-6 col-sm-12">
              <b-form-group>
                <h4 class="card-title">Referencia</h4>
                <b-form-input
                  v-model="principalSchema.reference"
                  :disabled="$route.query.action == 'show'"
                  class="mb-2"
                ></b-form-input>
                <p
                  class="text-danger text-size-required m-0"
                  v-if="$v.principalSchema.reference.$error"
                >
                  Campo requerido.
                </p>
              </b-form-group>
            </div>
            <div class="col-lg-6 col-md-6 col-sm-12">
              <b-form-group>
                <h4 class="card-title">{{ dateLabel }}</h4>
                <b-form-datepicker
                  v-model="principalSchema.date"
                  locale="es"
                  :disabled="$route.query.action == 'show'"
                  class="mb-2"
                ></b-form-datepicker>
                <p
                  class="text-danger text-size-required m-0"
                  v-if="$v.principalSchema.date.$error"
                >
                  Campo requerido.
                </p>
              </b-form-group>
            </div>
            <div class="col-lg-6 col-md-6 col-sm-12">
              <b-form-group>
                <h4 class="card-title">{{ entityLabel }}</h4>
                <vueselect
                  :options="schemaSelectList"
                  v-model="principalSchema.contactId"
                  :reduce="(row) => row.id"
                  label="name"
                  :disabled="$route.query.action == 'show'"
                  size="sm"
                ></vueselect>
                <p
                  class="text-danger text-size-required m-0"
                  v-if="$v.principalSchema.contactId.$error"
                >
                  Campo requerido.
                </p>
              </b-form-group>
            </div>
            <div class="col-lg-6 col-md-6 col-sm-12">
              <b-form-group>
                <h4 class="card-title">Metodo de Pago</h4>
                <b-form-select
                  v-model="principalSchema.paymentMethodId"
                  :options="paymentOptions"
                  :disabled="$route.query.action == 'show'"
                ></b-form-select>
                <p
                  class="text-danger text-size-required m-0"
                  v-if="$v.principalSchema.paymentMethodId.$error"
                >
                  Campo requerido.
                </p>
              </b-form-group>
            </div>

            <table class="table striped table-border">
              <thead>
                <tr>
                  <th>
                    <template v-if="list.length < 1">
                      <b-button
                        variant="primary"
                        @click="addRow()"
                        :disabled="$route.query.action == 'show'"
                      >
                        <span> <i class="fas fa-plus"></i> </span>
                      </b-button>
                    </template>
                    Concepto
                  </th>
                  <th>Descripción</th>
                  <th>Cantidad</th>
                  <th>Precio</th>
                  <!-- <th>Descuento %</th> -->
                  <th>Neto</th>
                  <!-- <th>Impuesto %</th> -->
                  <th>Total</th>
                </tr>
              </thead>
              <tbody>
                <tr v-for="(item, index) in list" :key="index">
                  <td>
                    <b-form-group>
                      <vueselect
                        style="width: 350px"
                        :options="conceptSelectList"
                        v-model="item.referenceId"
                        :reduce="(row) => row.id"
                        label="reference"
                        :disabled="$route.query.action == 'show'"
                        @input="setSelected(item, index)"
                        size="sm"
                      ></vueselect>
                    </b-form-group>
                  </td>
                  <td>
                    <b-form-group>
                      <b-form-input
                        v-model="item.description"
                        class="mb-2"
                        :disabled="$route.query.action == 'show'"
                        size="sm"
                      ></b-form-input>
                    </b-form-group>
                  </td>
                  <td>
                    <b-form-group>
                      <b-form-input
                        v-model="item.amount"
                        class="mb-2"
                        type="number"
                        :disabled="$route.query.action == 'show'"
                        @change="calculateLineTotal(item)"
                        size="sm"
                      ></b-form-input>
                    </b-form-group>
                  </td>
                  <td>
                    <b-form-group>
                      <b-form-input
                        v-model="item.price"
                        class="mb-2"
                        type="number"
                        :disabled="$route.query.action == 'show'"
                        @change="calculateLineTotal(item)"
                        size="sm"
                      ></b-form-input>
                    </b-form-group>
                  </td>

                  <td>
                    <b-form-group>
                      <b-form-input
                        v-model="item.total"
                        class="mb-2"
                        type="number"
                        :disabled="$route.query.action == 'show'"
                        size="sm"
                      ></b-form-input>
                    </b-form-group>
                  </td>

                  <td>
                    <b-form-group>
                      <b-form-input
                        v-model="item.total"
                        class="mb-2"
                        type="number"
                        disabled
                        size="sm"
                      ></b-form-input>
                    </b-form-group>
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
            </table>

            <div class="row ml-0 mb-3">
              <div class="col-lg-3">
                <b-form-group>
                  <h4 class="card-title">SubTotal</h4>
                  <b-form-input
                    v-model="invoice_subtotal"
                    disabled
                  ></b-form-input>
                </b-form-group>
              </div>
              <div class="col-lg-3">
                <b-form-group>
                  <h4 class="card-title">Total</h4>
                  <b-form-input v-model="invoice_total" disabled></b-form-input>
                </b-form-group>
              </div>
              <div class="col-lg-3">
                <b-form-group>
                  <h4 class="card-title">Impuesto %</h4>
                  <b-form-input v-model="invoice_tax" disabled></b-form-input>
                </b-form-group>
              </div>
            </div>

            <div class="row justify-content-end w-100 gx-2">
              <div class="col-3 p-2" v-if="$route.query.action == 'edit'">
                <b-button-group class="mt-4 mt-md-0">
                  <b-button variant="secundary" class="btn" @click="GoBack()">
                    <i class="bx bx-arrow-back"></i> Regresar
                  </b-button>
                  <b-button variant="success" class="btn" @click="editSchema()">
                    <i class="bx bx-save"></i> Guardar
                  </b-button>
                </b-button-group>
              </div>
              <div class="col-3 p-2" v-else>
                <b-button-group class="mt-4 mt-md-0">
                  <b-button variant="secundary" class="btn" @click="GoBack()">
                    <i class="bx bx-arrow-back"></i> Regresar
                  </b-button>
                  <b-button variant="success" size="lg" @click="saveSchema()">
                    <i class="bx bx-save"></i> Guardar
                  </b-button>
                </b-button-group>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import { required } from "vuelidate/lib/validators";
export default {
  data() {
    return {
      ShowModalCreate: false,
      ShowModalEdit: false,
      ShowModalDelete: false,
      ShowModalDetails: false,
      dateLabel: "Fecha",
      principalSchema: {
        contactId: null,
        code: null,
        date: null,
        reference: null,
        paymentMethodId: null,
        globalDiscount: 0.0,
        globalTotal: 0.0,
        transactionsType: 1,
        transactionsDetails: null,
      },
      infoSelect: null,
      entityLabel: "Cliente",
      schema: {
        id: null,
        transactionsId: null,
        referenceId: null,
        description: null,
        amount: 1,
        price: 0,
        discount: 0,
        total: 0,
        tax: 0,
      },
      izitoastConfig: {
        position: "topRight",
      },
      paymentOptions: [
        { value: "e1d7714b-bffb-403d-bbb4-fc6e4144c649", text: "Al contado" },
        {
          value: "d4dfc779-cb98-4ef0-bff3-cb6c648cf53c",
          text: "Tarjeta de crédito ",
        },
        {
          value: "a05307c4-71e7-4c7f-a5e0-2bdfc89b7c83",
          text: "Transfarencias bancarias",
        },
      ],
      schemaSelectList: [],
      conceptSelectList: [],
      rows: [],
      invoice_subtotal: 0,
      invoice_total: 0,
      invoice_tax: 5,
      list: [
        {
           id: null,
          concept: null,
          transactionsId: null,
          referenceId: null,
          description: null,
          amount: 1,
          price: 0,
          discount: 0,
          total: 0,
          tax: 0,
        },
      ],
      columns: [
        {
          label: "Código",
          field: "code",
          type: "text",
        },
        {
          label: "Descripción",
          field: "Description",
        },
        {
          label: "Acciones",
          field: "action",
        },
      ],
    };
  },
  validations: {
    principalSchema: {
      reference: {
        required,
      },
      date: {
        required,
      },
      paymentMethodId: {
        required,
      },
      contactId: {
        required,
      },
    },
  },

  created() {
    // this.GetAllSchemaRows();
    this.$route.query.action === undefined ? "" : this.getTransactionsDetails();
    this.getListForSelect();
    this.getListForSelectConcept();
    if (this.$route.query.form == "cotizacion") {
      this.dateLabel = "Fecha de vencimiento";
    }
    if (this.$route.query.form == "conduce") {
      this.dateLabel = "Fecha de vencimiento";
    }
    if (this.$route.query.form == "ordenDeCompra") {
      this.dateLabel = "Fecha de entrega";
    }
       if (this.$route.query.form == "facturascompras") {
      this.dateLabel = "Fecha de compra";
    }
  },
  methods: {
    setSelected(data, idx) {
      var obj = this.list.find((element, index) => index === idx);
      let row = this.conceptSelectList.find(
        (element) => element.id == obj.referenceId
      );
      obj.referenceId = row.id;
      obj.price = row.priceSale;
      this.calculateLineTotal(obj);
    },
    calculateTotal() {
      var subtotal, total;
      subtotal = this.list.reduce(function (sum, product) {
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
    },
    GoBack() {
   
      if (this.$route.query.form === undefined)
        this.$router.push({ path: "FacturasDeVenta" });
      switch (this.$route.query.form) {
        case "conduce":
          this.$router.push({ path: "Conduces" });
          break;
        case "cotizacion":
          this.$router.push({ path: "Cotizaciones" });
          break;
        case "notasDeCredito":
          this.$router.push({ path: "NotasDeCredito" });
          break;

        case "notasDeDebito":
          this.$router.push({ path: "/compras/NotasDebito" });
          break;
        case "ordenDeCompra":
          this.$router.push({ path: "/compras/OrdenesDeCompra" });
          break;
        case "facturascompras":
          this.$router.push({ path: "/compras/ComprasPorConcepto" });
          break;

        default:
          break;
      }
    },
    GetClientOrProvider(){
      
  if (this.$route.query.form === undefined)
      return true;
      switch (this.$route.query.form) {
        case "conduce":
         
          return true;
          break;
        case "cotizacion":
          
          return true;
          break;
        case "notasDeCredito":
         
          return true;
          break;

        case "notasDeDebito":
       
          return false;
          break;
        case "ordenDeCompra":
          return false;
          break;
        case "facturascompras":
            return false;
          break;

        default:
          return true;
          break;

      }
    },
    removeRow(index) {
      this.list.splice(index, 1);
    },
    addRow() {
      this.list.push({
        id: null,
        transactionsId: null,
        referenceId: null,
        description: null,
        amount: 1,
        price: 0,
        discount: 0,
        total: 0,
        tax: 0,
        code: null,
        concept: null,
      });
    },
    showModal() {
      this.$router.push("/Ingresos/FacturasDeVenta");
    },
    showSchema(schema) {
      this.schema = schema;
      this.ShowModalDetails = true;
    },
    editModalSchema(schema) {
      this.schema = schema;
      this.ShowModalEdit = true;
    },
    GetAllSchemaRows() {
      this.rows = [];
      this.$axios
        .get(this.$store.state.URL + "Transaction/GetAll", {
          headers: {
            "Content-Type": "application/json",
          },
        })
        .then((response) => {
          response.data.data.map((schema) => {
            let objSchema = {
              Id: schema.id,
              code: schema.code,
              Description: schema.description,
              CreditLedgerAccountId: schema.creditLedgerAccountId,
              DebitLedgerAccountId: schema.debitLedgerAccountId,
            };
            this.rows.push(objSchema);
          });
        })
        .catch((error) => {
          this.$toast.error(`${error}`, "ERROR", this.izitoastConfig);
        });
    },
    editSchema() {
      this.put(this.principalSchema);
    },
    saveSchema() {
      this.$v.$touch();
      if (this.$v.$invalid) {
        this.$toast.error(
          "Por favor complete el formulario correctamente.",
          "ERROR",
          this.izitoastConfig
        );
      } else {
        this.ShowModalCreate = false;
        this.principalSchema.transactionsDetails = this.list;
        this.post(this.principalSchema);
      }
    },
    async getTransactionsDetails() {
      let url =
        this.$store.state.URL +
        `Transaction/GetById?id=${this.$route.query.id}`;
      this.$axios
        .get(url, {
          headers: {
            "Content-Type": "application/json",
          },
        })
        .then((response) => {
          this.principalSchema = response.data.data;
          this.list = response.data.data.transactionsDetails;
          this.calculateTotal();
        })
        .catch((error) => {
          this.$toast.error(`${error}`, "ERROR", this.izitoastConfig);
        });
    },
    async getListForSelect() {
      let url = this.$store.state.URL + `Contact/GetAll`;
      let result = null;
      this.$axios
        .get(url, {
          headers: {
            "Content-Type": "application/json",
          },
        })
        .then((response) => {
          result = response;
          if (  this.GetClientOrProvider()   ) {
                this.entityLabel = "Cliente";
            this.schemaSelectList = result.data.data.filter(
              (person) => person.isClient == true
            );
          } else {
       
             this.entityLabel = "Proveedor";
            this.schemaSelectList = result.data.data.filter(
              (person) => person.isSupplier == true
            );
          }
        })
        .catch((error) => {
          result = error;
        });
    },
    async getListForSelectConcept() {
      let url = this.$store.state.URL + `Concept/GetAll`;
      let result = null;
      this.$axios
        .get(url, {
          headers: {
            "Content-Type": "application/json",
          },
        })
        .then((response) => {
        
            if (  this.GetClientOrProvider()  ) {
          this.conceptSelectList = response.data.data.filter(
            (concept) =>concept.isPurchase === true
          );
        } else {  this.conceptSelectList = response.data.data.filter(
            (concept) =>  concept.forSale === true
          );}
        })
        .catch((error) => {
          this.$toast.error(`${error}`, "ERROR", this.izitoastConfig);
        });
    },
      post(data) {
      if (this.$route.query.form == "cotizacion") {
        data.transactionsType = 3;
      }
      if (this.$route.query.form == "notasDeCredito") {
        data.transactionsType = 4;
      }
      if (this.$route.query.form == "conduce") {
        data.transactionsType = 5;
      }
      if (this.$route.query.form == "notasDeDebito") {
        data.transactionsType = 6;
      }
      if (this.$route.query.form == "ordenDeCompra") {
        data.transactionsType = 7;
      }
      if (this.$route.query.form == "facturascompras") {
        data.transactionsType = 2;
      }

        let url = this.$store.state.URL + `Transaction/Create`;
      let result = null;
     
      this.$axios
        .post(url, data, {
          headers: {
            "Content-Type": "application/json",
          },
        })
        .then((response) => {
          result = response; 
          this.$toast.success(
            "El Registro ha sido creado correctamente.",
            "ÉXITO" 
          );
          this.GoBack();
        })
        .catch((error) => {
          result = error;
          this.$toast.error(`${result}`, "ERROR", this.izitoastConfig);
        });
        
    },
      put(data) {
      if (this.$route.query.form == "cotizacion") {
        data.transactionsType = 3;
      }
      if (this.$route.query.form == "notasDeCredito") {
        data.transactionsType = 4;
      }
      if (this.$route.query.form == "conduce") {
        data.transactionsType = 5;
      }
      if (this.$route.query.form == "notasDeDebito") {
        data.transactionsType = 6;
      }
      if (this.$route.query.form == "ordenDeCompra") {
        data.transactionsType = 7;
      }
      if (this.$route.query.form == "facturascompras") {
        data.transactionsType = 2;
      }
       
 
        this.$axios
          .put(this.$store.state.URL + "Transaction/Update", data, {
           headers: {
                "Content-Type": "application/json",
                 
              },
          })
          .then((response) => {
        
            this.$toast.success(
              "El Registro ha sido actualizado correctamente.",
              "EXITO" 
            );
           
             this.GoBack();
          })
          .catch((error) => {
            reject(error);
            this.$toast.error(`${error}`, "ERROR", this.izitoastConfig);
          });
    
    },
    removeSchema(id) {
      this.$toast.question(
        "Esta seguro que quiere eliminar esta cuenta?",
        "PREGUNTA",
        {
          timeout: 20000,
          close: false,
          overlay: true,
          toastOnce: true,
          id: "question",
          zindex: 999,
          position: "center",
          buttons: [
            [
              "<button><b>YES</b></button>",
              function (instance, toast) {
                instance.hide({ transitionOut: "fadeOut" }, toast, "button");
                axios
                  .delete(
                    this.$store.state.URL + `Transaction/Delete/?id=${id}`,
                    {
                      headers: {
                        Authorization: `Bearer ${localStorage.getItem(
                          "token"
                        )}`,
                      },
                    }
                  )
                  .then((response) => {
                    alert(
                      "EXITO: El Registro ha sido eliminado correctamente."
                    );
                    location.reload();
                  })
                  .catch((error) => alert(error));
              },
              true,
            ],
            [
              "<button>NO</button>",
              function (instance, toast) {
                instance.hide({ transitionOut: "fadeOut" }, toast, "button");
              },
            ],
          ],
        }
      );
    },
    clearForm() {
      (this.invoice_subtotal = 0),
        (this.invoice_total = 0),
        (this.principalSchema = {
          contactId: null,
          code: null,
          date: null,
          reference: null,
          paymentMethodId: null,
          globalDiscount: 0.0,
          globalTotal: 0.0,
          transactionsType: 1,
          transactionsDetails: null,
        });
    },
  },
};
</script>

<style>

.text-size-required {
  font-size: 12px;
}
</style>
