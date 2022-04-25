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
              class="mb-2"
            ></b-form-input>
          </b-form-group>
        </div>
        <div class="col-lg-6 col-md-6 col-sm-12">
          <b-form-group label="Fecha">
            <b-form-datepicker
              v-model="principalSchema.date"
              class="mb-2"
            ></b-form-datepicker>
          </b-form-group>
        </div>
        <div class="col-lg-6 col-md-6 col-sm-12">
          <b-form-group label="Cliente">
            <vueselect
              :options="schemaSelectList"
              v-model="principalSchema.contactId"
              :reduce="(row) => row.id"
              label="name"
            ></vueselect>
          </b-form-group>
        </div>
        <div class="col-lg-6 col-md-6 col-sm-12">
          <b-form-group label="Metodo de Pago">
            <b-form-select
              v-model="principalSchema.paymentMethodId"
              :options="paymentOptions"
            ></b-form-select>
          </b-form-group>
        </div>

        <div class="container">
          <div
            class="row border shadow p-3 my-2"
            v-for="(item, index) in list"
            :key="item.id"
          >
            <div class="col-lg-6 col-md-6 col-sm-12">
              <b-form-group label="Concepto">
                <vueselect
                  :options="conceptSelectList"
                  v-model="infoSelect"
                  :reduce="(row) => row"
                  label="description"
                ></vueselect>
              </b-form-group>
            </div>

            <div class="col-lg-6 col-md-6 col-sm-12">
              <b-form-group label="Descripción">
                <b-form-textarea
                  v-model="item.description"
                  class="mb-2"
                ></b-form-textarea>
              </b-form-group>
            </div>
            <div class="col-lg-2 col-md-2 col-sm-12">
              <b-form-group label="Cantidad">
                <b-form-input
                  v-model="item.amount"
                  class="mb-2"
                  type="number"
                ></b-form-input>
              </b-form-group>
            </div>
            <div class="col-lg-2 col-md-2 col-sm-12">
              <b-form-group label="Precio">
                <b-form-input
                  v-model="item.price"
                  class="mb-2"
                  type="number"
                ></b-form-input>
              </b-form-group>
            </div>
            <div class="col-lg-2 col-md-2 col-sm-12">
              <b-form-group label="Descuento %">
                <b-form-input
                  v-model="item.discount"
                  class="mb-2"
                  type="number"
                ></b-form-input>
              </b-form-group>
            </div>
            <div class="col-lg-2 col-md-2 col-sm-12">
              <b-form-group label="Neto">
                <b-form-input
                  v-model="item.total"
                  class="mb-2"
                  type="number"
                ></b-form-input>
              </b-form-group>
            </div>
            <div class="col-lg-2 col-md-2 col-sm-12">
              <b-form-group label="Impuesto %">
                <b-form-input
                  v-model="item.tax"
                  class="mb-2"
                  type="number"
                ></b-form-input>
              </b-form-group>
            </div>
            <div class="col-lg-2 col-md-2 col-sm-12">
              <b-form-group label="IRPF">
                <b-form-input
                  v-model="item.irpf"
                  class="mb-2"
                  type="number"
                ></b-form-input>
              </b-form-group>
            </div>
            <div class="row mx-3">
              <b-button variant="danger" @click="removeRow(index)">
                <span>
                  <fa icon="trash"></fa>
                </span>
                Eliminar
              </b-button>
            </div>
          </div>
        </div>
        <div class="row mx-3">
          <b-button variant="primary" @click="addRow()">
            <span><fa icon="plus"></fa></span> Agregar
          </b-button>
        </div>

        <div class="row justify-content-end w-100 gx-2">
          <div class="col-2 p-2">
            <b-button
              variant="danger"
              class="w-100"
              @click="ShowModalCreate = !ShowModalCreate"
              >Cerrar</b-button
            >
          </div>
          <div class="col-3 p-2">
            <b-button
              class="w-100"
              style="background-color: #457b9d"
              @click="saveSchema()"
            >
              <span>Guardar</span>
            </b-button>
          </div>
        </div>
      </div>
    </div>

    <!-- Modal for schema details -->
    <!-- <b-modal
      size="lg"
      title="Formulario de Schema"
      v-model="ShowModalDetails"
      hide-footer
    >
      <div class="container">
        <div class="row">
          <div class="col-lg-12 col-md-12 col-sm-12">
            <b-form-group label="Descripción del Schema">
              <b-form-input
                v-model="schema"
                size="sm"
                trim
                disabled
              ></b-form-input>
              <p
                class="text-danger text-size-required m-0"
                v-if="$v.schema.$error"
              >
                Nombre de la cuenta requerido.
              </p>
            </b-form-group>
          </div>
          <div class="col-lg-6 col-md-6 col-sm-12">
            <b-form-group label="Cuenta de Débito">
              <vueselect
                :options="schemaSelectList"
                v-model="schema.CreditLedgerAccountId"
                :reduce="(row) => row.id"
                label="name"
                disabled
              ></vueselect>
              <p
                class="text-danger text-size-required m-0"
                v-if="$v.schema.$error"
              >
                Cuenta de Débito requerida.
              </p>
            </b-form-group>
          </div>
          <div class="row justify-content-end w-100 gx-2">
            <div class="col-2 p-2">
              <b-button
                variant="danger"
                class="w-100"
                @click="ShowModalDetails = !ShowModalDetails"
                >Cerrar</b-button
              >
            </div>
            <div class="col-3 p-2">
              <b-button
                class="w-100"
                style="background-color: #457b9d"
                @click="saveSchema()"
                disabled
              >
                <span>Guardar</span>
              </b-button>
            </div>
          </div>
        </div>
      </div>
    </b-modal> -->
    <!-- Modal for edit schema -->
    <!-- <b-modal
      size="lg"
      title="Formulario de Schema"
      v-model="ShowModalEdit"
      hide-footer
    >
      <div class="container">
        <div class="row">
          <div class="col-lg-12 col-md-12 col-sm-12">
            <b-form-group label="Descripción del Schema">
              <b-form-input v-model="schema" size="sm" trim></b-form-input>
            </b-form-group>
          </div>
          <div class="col-lg-6 col-md-6 col-sm-12">
            <b-form-group label="Cuenta de Débito">
              <vueselect
                :options="schemaSelectList"
                v-model="schema.CreditLedgerAccountId"
                :reduce="(row) => row.id"
                label="name"
              ></vueselect>
            </b-form-group>
          </div>
          <div class="row justify-content-end w-100 gx-2">
            <div class="col-2 p-2">
              <b-button
                variant="danger"
                class="w-100"
                @click="ShowModalEdit = !ShowModalEdit"
                >Cerrar</b-button
              >
            </div>
            <div class="col-3 p-2">
              <b-button
                class="w-100"
                style="background-color: #457b9d"
                @click="editSchema()"
              >
                <span>Guardar</span>
              </b-button>
            </div>
          </div>
        </div>
      </div>
    </b-modal> -->
  </div>
</template>

<script>
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
        transactionsId: "937c9665-93a7-44bb-9636-2d6cff68fd1c",
        referenceId: "780b755a-9a3e-44e0-8de7-b8512b48df64",
        description: null,
        amount: 1,
        price: 0,
        discount: 0,
        total: 0,
        tax: 0,
        irpf: 0,
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
      list: [
        {
          transactionsId: "937c9665-93a7-44bb-9636-2d6cff68fd1c",
          referenceId: "780b755a-9a3e-44e0-8de7-b8512b48df64",
          description: null,
          amount: 1,
          price: 0,
          discount: 0,
          total: 0,
          tax: 0,
          irpf: 0,
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
    schema: {
      //   schemaProperty: {
      //     required,
      //   },
    },
  },

  created() {
    // this.GetAllSchemaRows();
    this.getListForSelect();
    this.getListForSelectConcept();
  },
  methods: {
    removeRow(index) {
      this.list.splice(index, 1);
    },
    addRow() {
      this.list.push({
        transactionsId: "937c9665-93a7-44bb-9636-2d6cff68fd1c",
        referenceId: "780b755a-9a3e-44e0-8de7-b8512b48df64",
        description: null,
        amount: 1,
        price: 0,
        discount: 0,
        total: 0,
        tax: 0,
        irpf: 0,
      });
    },
    showModal() {
      this.ShowModalCreate = true;
      this.clearForm();
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
      this.put(this.schema);
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
        // let url = "Transaction/Create";
        // fetch(url, {
        //   method: "POST", // or 'PUT'
        //   body: JSON.stringify(this.principalSchema), // data can be `string` or {object}!
        //   headers: {
        //     "Content-Type": "application/json",
        //   },
        // })
        //   .then((res) => res.json())
        //   .catch((error) => console.error("Error:", error))
        //   .then((response) => console.log("Success:", response));
        // this.$axios
        //   .post(url, this.principalSchema, {
        //     headers: {
        //       "Content-Type": "application/json",
        //     },
        //   })
        //   .then((response) => console.log(response))
        //   .catch((error) => console.log(error));
        this.post(this.principalSchema);
        // this.clearForm();
      }
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
                fetch(process.env.devUrl + `Transaction/Delete/?id=${id}`, {
                  method: "DELETE",
                })
                  .then((resp) => {
                    alert(
                      "EXITO: El Registro ha sido eliminado correctamente."
                    );
                  })
                  .catch((error) => {
                    alert(error);
                  });
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
