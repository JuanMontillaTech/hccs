<template>
  <div class="container">
    <b-modal
      size="lg"
      title="Formulario de Ingresos por concepto"
      v-model="ShowModalCreate"
      hide-footer
    >
      <div class="container">
        <div class="row">
          <div class="col-lg-6 col-md-6 col-sm-12">
            <b-form-group label="Fecha">
              <b-form-datepicker
                v-model="schema.date"
                size="sm"
                :state="$v.schema.date.$error ? false : null"
              >
              </b-form-datepicker>
              <p
                class="text-danger text-size-required m-0"
                v-if="$v.schema.date.$error"
              >
                Campo requerido.
              </p>
            </b-form-group>
          </div>
          <div class="col-lg-6 col-md-6 col-sm-12">
            <b-form-group label="Referencia">
              <b-form-input
                v-model="schema.reference"
                size="sm"
                :state="$v.schema.reference.$error ? false : null"
                trim
              ></b-form-input>
              <p
                class="text-danger text-size-required m-0"
                v-if="$v.schema.reference.$error"
              >
                Campo requerido.
              </p>
            </b-form-group>
          </div>
          <div class="col-lg-12 col-md-12 col-sm-12">
            <b-form-group label="Observaciones">
              <b-form-textarea
                v-model="schema.observations"
                size="sm"
                :state="$v.schema.observations.$error ? false : null"
                trim
              ></b-form-textarea>
              <p
                class="text-danger text-size-required m-0"
                v-if="$v.schema.observations.$error"
              >
                Campo requerido.
              </p>
            </b-form-group>
          </div>
          <table class="table striped table-border">
            <thead class="bg-Cprimary">
              <tr>
                <th style="width: 20%">Concepto</th>
                <th style="width: 35%">Descripción</th>
                <th style="width: 20%">Monto</th>
                <th style="width: 5%"></th>
              </tr>
            </thead>
            <tbody>
              <tr
                v-for="(CondenDetells, index) in schema.conceptListDetails"
                v-bind:key="index"
              >
                <td>
                  <!-- <vueselect
                  :options="conceptList"
                  v-model="name"
                  :reduce="(row) => row.id"
                  label="description"
                ></vueselect> -->
                </td>
                <td>
                  <textarea v-model="description" rows="3"></textarea>
                </td>
                <td>
                  <b-form-input v-model="amount" size="sm" trim></b-form-input>
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
                <!-- <td>{{ Tdebit }}</td>
                <td>{{ Tcredit }}</td> -->
              </tr>
            </tfoot>
          </table>
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
      title="Formulario de Ingresos por concepto"
      v-model="ShowModalDetails"
      hide-footer
    >
      <div class="container">
        <div class="row">
          <div class="col-lg-6 col-md-6 col-sm-12">
            <b-form-group label="Fecha">
              <b-form-datepicker
                v-model="schema.date"
                size="sm"
                :state="$v.schema.date.$error ? false : null"
                disabled
              >
              </b-form-datepicker>
              <p
                class="text-danger text-size-required m-0"
                v-if="$v.schema.date.$error"
              >
                Campo requerido.
              </p>
            </b-form-group>
          </div>
          <div class="col-lg-6 col-md-6 col-sm-12">
            <b-form-group label="Referencia">
              <b-form-input
                v-model="schema.reference"
                size="sm"
                :state="$v.schema.reference.$error ? false : null"
                trim
                disabled
              ></b-form-input>
              <p
                class="text-danger text-size-required m-0"
                v-if="$v.schema.reference.$error"
              >
                Campo requerido.
              </p>
            </b-form-group>
          </div>
          <div class="col-lg-12 col-md-12 col-sm-12">
            <b-form-group label="Observaciones">
              <b-form-textarea
                v-model="schema.observations"
                size="sm"
                :state="$v.schema.observations.$error ? false : null"
                trim
                disabled
              ></b-form-textarea>
              <p
                class="text-danger text-size-required m-0"
                v-if="$v.schema.observations.$error"
              >
                Campo requerido.
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
    </b-modal>
    <!-- Modal for edit schema -->
    <b-modal
      size="lg"
      title="Formulario de Ingresos por concepto"
      v-model="ShowModalEdit"
      hide-footer
    >
      <div class="container">
        <div class="row">
          <div class="col-lg-6 col-md-6 col-sm-12">
            <b-form-group label="Fecha">
              <b-form-datepicker
                v-model="schema.date"
                size="sm"
                :state="$v.schema.date.$error ? false : null"
              >
              </b-form-datepicker>
              <p
                class="text-danger text-size-required m-0"
                v-if="$v.schema.date.$error"
              >
                Campo requerido.
              </p>
            </b-form-group>
          </div>
          <div class="col-lg-6 col-md-6 col-sm-12">
            <b-form-group label="Referencia">
              <b-form-input
                v-model="schema.reference"
                size="sm"
                :state="$v.schema.reference.$error ? false : null"
                trim
              ></b-form-input>
              <p
                class="text-danger text-size-required m-0"
                v-if="$v.schema.reference.$error"
              >
                Campo requerido.
              </p>
            </b-form-group>
          </div>
          <div class="col-lg-12 col-md-12 col-sm-12">
            <b-form-group label="Observaciones">
              <b-form-textarea
                v-model="schema.observations"
                size="sm"
                :state="$v.schema.observations.$error ? false : null"
                trim
              ></b-form-textarea>
              <p
                class="text-danger text-size-required m-0"
                v-if="$v.schema.observations.$error"
              >
                Campo requerido.
              </p>
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
          <div>Listado de ingresos por concepto</div>
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
          <b-button class="btn btn-light btn-sm" @click="showSchema(props.row)">
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
            @click="editModalSchema(props.row)"
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
      conceptList: [],
      Tdebit: 0.0,
      Tcredit: 0.0,
      schema: {
        id: null,
        date: null,
        reference: null,
        observations: null,
        conceptListDetails: [
          {
            id: null,
            concept: null,
            description: null,
            amount: null,
          },
        ],
      },
      izitoastConfig: {
        position: "topRight",
      },
      schemaSelectList: [],
      rows: [
        {
          id: 1,
          date: "2022-08-08",
          reference: "reference 1",
          observations: "observations 1",
        },
        {
          id: 2,
          date: "2022-08-08",
          reference: "reference 2",
          observations: "observations 2",
        },
        {
          id: 3,
          date: "2022-08-08",
          reference: "reference 3",
          observations: "observations 3",
        },
        {
          id: 4,
          date: "2022-08-08",
          reference: "reference 4",
          observations: "observations 4",
        },
        {
          id: 5,
          date: "2022-08-08",
          reference: "reference 5",
          observations: "observations 5",
        },
      ],
      columns: [
        {
          label: "Fecha",
          field: "date",
        },
        {
          label: "Referencia",
          field: "reference",
        },
        {
          label: "Observaciones",
          field: "observations",
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
      date: {
        required,
      },
      reference: {
        required,
      },
      observations: {
        required,
      },
    },
  },
  created() {
    this.GetAllSchemaRows();
    this.getListForSelect();
  },
  methods: {
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
      //   this.rows = [];
      this.$axios
        .get(process.env.devUrl + "schema/GetAll", {
          headers: {
            "Content-Type": "application/json",
          },
        })
        .then((response) => {
          response.data.data.map((schema) => {
            // let objSchema = {
            //   Id: schema.id,
            //   Description: schema.description,
            //   CreditLedgerAccountId: schema.creditLedgerAccountId,
            //   DebitLedgerAccountId: schema.debitLedgerAccountId,
            // };
            // this.rows.push(objSchema);
          });
        })
        .catch((error) => {
          this.$toast.error(`${error}`, "ERROR", this.izitoastConfig);
        });
    },
    async GetTotal() {
      var Total = numbro(0);
      this.schema.journaDetails.forEach((e) => Total.add(e.debit));
      this.Tdebit = Total.formatCurrency({
        thousandSeparated: true,
        mantissa: 2,
        negative: "parenthesis",
      });
      var TotalC = numbro(0);
      this.form.conceptListDetails.forEach((e) => TotalC.add(e.credit));
      this.Tcredit = TotalC.formatCurrency({
        thousandSeparated: true,
        mantissa: 2,
        negative: "parenthesis",
      });
    },
    async addRow() {
      let newRow = {
        id: null,
        concept: null,
        description: null,
        amount: null,
      };
      this.schema.conceptListDetails.push(newRow);
    },
    removeRow(index) {
      this.schema.conceptListDetails.splice(index, 1);
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
        console.log(this.schema);
        // this.clearForm();
      }
    },
    async getListForSelect() {
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
          console.log(response);
          this.conceptList = result.data.data;
        })
        .catch((error) => {
          result = error;
        });
    },
    async post(data) {
      return new Promise((resolve, reject) => {
        this.$axios
          .post(this.$store.state.URL  + "schema/Create", data, {
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
                axios
                  .delete(process.env.devUrl + `schema/Delete/?id=${id}`, {
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
                // fetch(`schema/Delete/?id=${id}`, {
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
