<template>
  <div class="container">
    <b-modal
      size="lg"
      title="Formulario de Schema"
      v-model="ShowModalCreate"
      hide-footer
    >
      <div class="container">
        <div class="row">
          <div class="col-lg-6 col-md-6 col-sm-12">
            <b-form-group label="#Referencia">
              <b-form-input
                v-model="schema.taxType"
                class="mb-2"
              ></b-form-input>
            </b-form-group>
          </div>
          <div class="col-lg-6 col-md-6 col-sm-12">
            <b-form-group label="Fecha">
              <b-form-datepicker
                v-model="schema.taxType"
                class="mb-2"
              ></b-form-datepicker>
            </b-form-group>
          </div>
          <div class="col-lg-4 col-md-4 col-sm-12">
            <b-form-group label="Cliente">
              <vueselect
                :options="schemaSelectList"
                v-model="schema.CreditLedgerAccountId"
                :reduce="(row) => row.id"
                label="name"
              ></vueselect>
            </b-form-group>
          </div>
          <div class="col-lg-4 col-md-4 col-sm-12">
            <b-form-group label="Metodo de Pago">
              <b-form-select
                v-model="schema.taxType"
                :options="paymentOptions"
              ></b-form-select>
            </b-form-group>
          </div>
          <div class="col-lg-4 col-md-4 col-sm-12">
            <b-form-group label="Total">
              <b-form-input v-model="schema.taxType"></b-form-input>
            </b-form-group>
          </div>
          <div class="container border">
            <div class="row" v-for="(item, index) in list" :key="item.id">
              <div class="col-lg-4 col-md-4 col-sm-12">
                <b-form-group label="Concepto">
                  <vueselect
                    :options="conceptSelectList"
                    v-model="item.concept"
                    :reduce="(row) => row.id"
                    label="description"
                  ></vueselect>
                </b-form-group>
              </div>

              <div class="col-lg-4 col-md-4 col-sm-12">
                <b-form-group label="Descripción">
                  <b-form-input
                    v-model="item.description"
                    class="mb-2"
                  ></b-form-input>
                </b-form-group>
              </div>
              <div class="col-lg-4 col-md-4 col-sm-12">
                <b-form-group label="Cantidad">
                  <b-form-input
                    v-model="item.quantity"
                    class="mb-2"
                    type="number"
                  ></b-form-input>
                </b-form-group>
              </div>
              <div class="col-lg-4 col-md-4 col-sm-12">
                <b-form-group label="Precio">
                  <b-form-input
                    v-model="item.price"
                    class="mb-2"
                    type="number"
                  ></b-form-input>
                </b-form-group>
              </div>
              <div class="col-lg-4 col-md-4 col-sm-12">
                <b-form-group label="Descuento %">
                  <b-form-input
                    v-model="item.discount"
                    class="mb-2"
                    type="number"
                  ></b-form-input>
                </b-form-group>
              </div>
              <div class="col-lg-4 col-md-4 col-sm-12">
                <b-form-group label="Neto">
                  <b-form-input
                    v-model="item.neto"
                    class="mb-2"
                    type="number"
                  ></b-form-input>
                </b-form-group>
              </div>
              <div class="col-lg-4 col-md-4 col-sm-12">
                <b-form-group label="Impuesto %">
                  <b-form-input
                    v-model="item.tax"
                    class="mb-2"
                    type="number"
                  ></b-form-input>
                </b-form-group>
              </div>
              <div class="col-lg-4 col-md-4 col-sm-12">
                <b-form-group label="IRPF">
                  <b-form-input
                    v-model="item.irpf"
                    class="mb-2"
                    type="number"
                  ></b-form-input>
                </b-form-group>
              </div>
              <div class="col-lg-2 col-md-2 col-sm-12">
                <b-button
                  variant="danger"
                  class="w-50"
                  @click="removeRow(index)"
                >
                  <fa icon="trash"></fa
                ></b-button>
              </div>
            </div>
          </div>
          <b-button variant="danger" style="width: 50px" @click="addRow()"
            ><fa icon="plus"></fa
          ></b-button>

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
    </b-modal>

    <!-- Modal for schema details -->
    <b-modal
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
              <!-- <p
                class="text-danger text-size-required m-0"
                v-if="$v.schema.$error"
              >
                Nombre de la cuenta requerido.
              </p> -->
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
              <!-- <p
                class="text-danger text-size-required m-0"
                v-if="$v.schema.$error"
              >
                Cuenta de Débito requerida.
              </p> -->
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
    </b-modal>
    <!-- Modal for edit schema -->
    <b-modal
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
              <!-- <p
                class="text-danger text-size-required m-0"
                v-if="$v.schema.$error"
              >
                Nombre de la cuenta requerido.
              </p> -->
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
              <!-- <p
                class="text-danger text-size-required m-0"
                v-if="$v.schema.$error"
              >
                Cuenta de Débito requerida.
              </p> -->
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
    </b-modal>
    <nav class="navbar navbar-default">
      <div class="container-fluid">
        <div class="navbar-header">
          <div>Listado de Facturas</div>
        </div>
        <div class="btn-group" role="group" aria-label="Basic example">
          <nuxt-link to="/Ingresos/Facturas">
            <a title="Nuevo Registro" class="btn btn-primary btn-sm text-white">
              <fa icon="file" class="ml-1"></fa>
              Nuevo</a
            >
          </nuxt-link>

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
    <vue-good-table
      :columns="columns"
      :rows="rows"
      :search-options="{
        enabled: true,
      }"
      :pagination-options="{
        enabled: true,
        mode: 'records',
      }"
    >
      <template slot="table-row" slot-scope="props">
        <span v-if="props.column.field == 'action'">
          <b-button
            class="btn btn-light btn-sm"
            @click="showSchema(props.row.Id)"
          >
            <fa icon="eye"></fa>
          </b-button>
          <b-button
            class="btn btn-light btn-sm"
            @click="removeSchema(props.row.Id)"
          >
            <fa icon="trash"></fa>
          </b-button>
          <b-button
            class="btn btn-light btn-sm"
            @click="editModalSchema(props.row.Id)"
          >
            <fa icon="edit"></fa>
            ></b-button
          >
        </span>
        <span v-else>
          {{ props.formattedRow[props.column.field] }}
        </span>
      </template>
    </vue-good-table>
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
      schema: {
        client: null,
        concept: null,
        reference: null,
        description: null,
        quantity: null,
        price: null,
        discount: null,
        neto: null,
        tax: null,
        irpf: null,
      },
      izitoastConfig: {
        position: "topRight",
      },
      paymentOptions: [
        { value: 1, text: "Al contado" },
        { value: 2, text: "Tarjeta de crédito " },
        { value: 3, text: "Transfarencias bancarias" },
      ],
      schemaSelectList: [],
      conceptSelectList: [],
      rows: [],
      list: [
        {
          client: null,
          concept: null,
          reference: null,
          description: null,
          quantity: null,
          price: null,
          discount: null,
          neto: null,
          tax: null,
          irpf: null,
        },
      ],
      columns: [
        {
          label: "Descripción",
          field: "Reference",
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
    this.GetAllSchemaRows();
    this.getListForSelect();
    this.getListForSelectConcept();
  },
  methods: {
    removeRow(index) {
      this.list.splice(index, 1);
    },
    addRow() {
      this.list.push({
        client: null,
        concept: null,
        reference: null,
        description: null,
        quantity: null,
        price: null,
        discount: null,
        neto: null,
        tax: null,
        irpf: null,
      });
    },
    showSchema(id) {
      this.$router.push({
        path: "/Ingresos/Facturas",
        query: { id: id, action: "show" },
      });
      // this.schema = schema;
      // this.ShowModalDetails = true;
    },
    editModalSchema(id) {
      this.$router.push({
        path: "/Ingresos/Facturas",
        query: { id: id, action: "edit" },
      });
      // this.schema = schema;
      // this.ShowModalEdit = true;
    },
    GetAllSchemaRows() {
      this.rows = [];
      this.$axios
        .get(process.env.devUrl + "Transaction/GetAll", {
          headers: {
            "Content-Type": "application/json",
          },
        })
        .then((response) => {
          response.data.data.map((schema) => {
            let objSchema = {
              Id: schema.id,
              Reference: schema.reference,
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
        this.post(this.schema);
        this.clearForm();
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
          .post(process.env.devUrl + "schema/Create", data, {
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
            this.GetAllSchemaRows();
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
          .put(process.env.devUrl + "schema/Update", data, {
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
                    location.reload();
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
      for (const x in this.schema) {
        this.schema[x] = null;
      }
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
