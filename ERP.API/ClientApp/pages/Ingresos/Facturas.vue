<template>
  <div class="container">
    <nav class="navbar navbar-default">
      <div class="container-fluid">
        <div class="navbar-header">
          <div>Listado de Facturas</div>
        </div>
        <div class="btn-group" role="group" aria-label="Basic example">
          <a
            title="Nuevo Registro"
            @click="showModal()"
            class="btn btn-primary btn-sm text-white"
          >
            <fa icon="file" class="ml-1"></fa>
            Nuevo</a
          >

          <a
            id="_btnRefresh"
            @click="GetAllSchemaRows()"
            class="btn btn-light border btn-sm text-black-50 btnRefresh"
            name="_btnRefresh"
            ><i class="fas fa-sync-alt"></i> Actualizar Datos</a
          >
        </div>
      </div>
    </nav>

    <div class="container">
      <div class="row border border-rounded bg-white p-3">
        <div class="col-lg-6 col-md-6 col-sm-12">
          <b-form-group label="#Referencia">
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
          <b-form-group label="Fecha">
            <b-form-datepicker
              v-model="principalSchema.date"
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
          <b-form-group label="Cliente">
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
          <b-form-group label="Metodo de Pago">
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
        <div class="container">
          <table>
            <thead>
              <tr>
                <th>Concepto</th>
                <th>Descripción</th>
                <th>Cantidad</th>
                <th>Precio</th>
                <th>Descuento %</th>
                <th>Neto</th>
                <th>Impuesto %</th>
                <th>Total</th>
              </tr>
            </thead>
            <tbody>
              <tr v-for="(item, index) in list" :key="index">
                <td>
                  {{ infoSelect }}
                  <b-form-group>
                    <vueselect
                      :options="conceptSelectList"
                      v-model="item.concept"
                      :reduce="(row) => row"
                      label="description"
                      :disabled="$route.query.action == 'show'"
                      @input="setSelected(item, index)"
                      size="sm"
                    ></vueselect>
                  </b-form-group>
                </td>
                <td>
                  <b-form-group>
                    <b-form-textarea
                      v-model="item.description"
                      class="mb-2"
                      :disabled="$route.query.action == 'show'"
                      size="sm"
                    ></b-form-textarea>
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
                      v-model="item.discount"
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
                      :disabled="$route.query.action == 'show'"
                      size="sm"
                    ></b-form-input>
                  </b-form-group>
                </td>
                <td>
                  <b-form-group>
                    <b-form-input
                      v-model="item.tax"
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
                  <b-button
                    class="mb-4"
                    variant="danger"
                    @click="removeRow(index)"
                    :disabled="$route.query.action == 'show'"
                    v-b-tooltip.hover
                    title="Eliminar"
                  >
                    <span>
                      <fa icon="trash"></fa>
                    </span>
                  </b-button>
                </td>
              </tr>
            </tbody>
          </table>
        </div>
        <div class="row ml-0 mb-3">
          <div class="col-lg-3">
            <b-form-group label="SubTotal">
              <b-form-input v-model="invoice_subtotal" disabled></b-form-input>
            </b-form-group>
          </div>
          <div class="col-lg-3">
            <b-form-group label="Total">
              <b-form-input v-model="invoice_total" disabled></b-form-input>
            </b-form-group>
          </div>
          <div class="col-lg-3">
            <b-form-group label="Impuesto %">
              <b-form-input v-model="invoice_tax" disabled></b-form-input>
            </b-form-group>
          </div>
        </div>
        <div class="row mx-3">
          <b-button
            variant="primary"
            @click="addRow()"
            :disabled="$route.query.action == 'show'"
          >
            <span><fa icon="plus"></fa></span> Agregar
          </b-button>
        </div>

        <div class="row justify-content-end w-100 gx-2">
          <div class="col-3 p-2" v-if="$route.query.action == 'edit'">
            <b-button
              class="w-100"
              style="background-color: #457b9d"
              @click="editSchema()"
            >
              <span>Guardar</span>
            </b-button>
          </div>
          <div class="col-3 p-2" v-else>
            <b-button
              class="w-100"
              style="background-color: #457b9d"
              @click="saveSchema()"
              :disabled="$route.query.action == 'show'"
            >
              <span>Guardar</span>
            </b-button>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import axios from "axios";
import { required } from "vuelidate/lib/validators";
export default {
  name: "Schema",
  layout: "TheSlidebar",
  data() {
    return {
      ShowModalCreate: false,
      ShowModalEdit: false,
      ShowModalDelete: false,
      ShowModalDetails: false,
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
      schema: {
        conceptId: null,
        transactionsId: "937c9665-93a7-44bb-9636-2d6cff68fd1c",
        referenceId: "780b755a-9a3e-44e0-8de7-b8512b48df64",
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
          conceptId: null,
          concept: null,
          transactionsId: "937c9665-93a7-44bb-9636-2d6cff68fd1c",
          referenceId: "780b755a-9a3e-44e0-8de7-b8512b48df64",
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
  },
  methods: {
    setSelected(data, idx) {
      console.log(data);
      console.log(idx);
      console.log(this.list);
      const obj = this.list.find((element, index) => index === idx);
      console.log("obj", obj);
      obj.conceptId = data.concept.conceptId;
      obj.price = data.concept.priceSale;
    },
    calculateTotal() {
      console.log("test");
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
      } else {
        this.invoice_total = "0.00";
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
    removeRow(index) {
      this.list.splice(index, 1);
    },
    addRow() {
      this.list.push({
        concept: null,
        conceptId: null,
        transactionsId: "937c9665-93a7-44bb-9636-2d6cff68fd1c",
        referenceId: "780b755a-9a3e-44e0-8de7-b8512b48df64",
        description: null,
        amount: 1,
        price: 0,
        discount: 0,
        total: 0,
        tax: 0,
      });
    },
    showModal() {
      this.$router.push("/Ingresos/FacturasDeVenta");
      // this.ShowModalCreate = true;
      // this.clearForm();
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
        .get(process.env.devUrl + "schema/GetAll", {
          headers: {
            "Content-Type": "application/json",
          },
        })
        .then((response) => {
          response.data.data.map((schema) => {
            let objSchema = {
              Id: schema.id,
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
        process.env.devUrl + `Transaction/GetById?id=${this.$route.query.id}`;
      this.$axios
        .get(url, {
          headers: {
            "Content-Type": "application/json",
          },
        })
        .then((response) => {
          this.principalSchema = response.data.data;
          this.list = response.data.data.transactionsDetails;
        })
        .catch((error) => {
          this.$toast.error(`${error}`, "ERROR", this.izitoastConfig);
        });
    },
    async getListForSelect() {
      let url = process.env.devUrl + `Contact/GetAll`;
      let result = null;
      this.$axios
        .get(url, {
          headers: {
            "Content-Type": "application/json",
          },
        })
        .then((response) => {
          result = response;
          this.schemaSelectList = result.data.data;
        })
        .catch((error) => {
          result = error;
        });
    },
    async getListForSelectConcept() {
      let url = process.env.devUrl + `Concept/GetAll`;
      let result = null;
      this.$axios
        .get(url, {
          headers: {
            "Content-Type": "application/json",
          },
        })
        .then((response) => {
          result = response;
          this.conceptSelectList = result.data.data;
        })
        .catch((error) => {
          result = error;
        });
    },
    async post(data) {
      return new Promise((resolve, reject) => {
        this.$axios
          .post(process.env.devUrl + "Transaction/Create", data, {
            headers: {
              "Content-Type": "application/json",
            },
          })
          .then((response) => {
            resolve(response);
            this.$toast.success(
              "El Registro ha sido creado correctamente.",
              "EXITO",
              this.izitoastConfig
            );
            // this.GetAllSchemaRows();
            this.clearForm();
            this.list = [];
          })
          .catch((error) => {
            reject(error);
            this.$toast.error(`${error}`, "ERROR", this.izitoastConfig);
          });
      });
    },
    async put(data) {
      return new Promise((resolve, reject) => {
        this.$axios
          .put(process.env.devUrl + "Transaction/Update", data, {
            headers: {
              "Content-Type": "application/json",
            },
          })
          .then((response) => {
            resolve(response);
            this.$toast.success(
              "El Registro ha sido actualizado correctamente.",
              "EXITO",
              this.izitoastConfig
            );
            this.ShowModalEdit = false;
          })
          .catch((error) => {
            reject(error);
            this.$toast.error(`${error}`, "ERROR", this.izitoastConfig);
          });
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
                // fetch(process.env.devUrl + `Transaction/Delete/?id=${id}`, {
                //   method: "DELETE",
                // })
                //   .then((resp) => {
                //     alert(
                //       "EXITO: El Registro ha sido eliminado correctamente."
                //     );
                //   })
                //   .catch((error) => {
                //     alert(error);
                //   });
                axios
                  .delete(process.env.devUrl + `Transaction/Delete/?id=${id}`, {
                    headers: {
                      Authorization: `Bearer ${localStorage.getItem("token")}`,
                    },
                  })
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
      this.principalSchema = {
        contactId: null,
        code: null,
        date: null,
        reference: null,
        paymentMethodId: null,
        globalDiscount: 0.0,
        globalTotal: 0.0,
        transactionsType: 1,
        transactionsDetails: null,
      };
    },
  },
};
</script>

<style>
.modal-header {
  background-color: #457b9d !important;
  color: #fff;
}
.text-size-required {
  font-size: 12px;
}
</style>
